using Configuration;
using DataConfiguration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Security.AccessControl;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using static ApplicationData.TalonFX;
using static System.Net.Mime.MediaTypeNames;

//todo handle optional elements such as followID in a motorcontroller
//todo the range of pdpID for ctre is 0-15, for REV it is 0-19. How to adjust the range allowed in the GUI. If initially REV is used and an id > 15 is used, then user chooses CTRE, what to do?
//todo make mechanism instances separate files so that it is easier for multiple people to work on the robot in parallel
//todo run a sanity check on a click of a button or on every change?
//todo in the treeview, place the "name" nodes at the top
//todo in the robot code, check that an enum belonging to another robot is not used
//todo check naming convention
//todo getDisplayName gets called multiple times when a solenoid name is changed in a mechanism
//todo handle DistanceAngleCalcStruc should this be split into 2 separate structs? one ofr dist , 2nd for angle?
//todo when mechanisms are renamed, the GUIDs get messed up
//todo if a decorator mod file exists, do not write it
//todo show the DataDescription information

// =================================== Rules =====================================
// A property named __units__ will be converted to the list of physical units
// A property named value__ will not be shown in the tree directly. Its value is shown in the parent node
// Attributes are only allowed on the standard types (uint, int, double, bool) and on doubleParameter, unitParameter, intParameter, boolParameter
// The attribute PhysicalUnitsFamily can only be applied on doubleParameter, uintParameter, intParameter, boolParameter
// A class can only contain one List of a particular type

namespace ApplicationData
{
    #region general enums
    [Serializable()]
    public enum CAN_BUS
    {
        rio,
    }

    [Serializable()]
    public enum MotorType
    {

        TALONSRX,

        FALCON,

        BRUSHLESS_SPARK_MAX,

        BRUSHED_SPARK_MAX,

        FALCON500,

        NEOMOTOR,

        NEO500MOTOR,

        CIMMOTOR,

        MINICIMMOTOR,

        BAGMOTOR,

        PRO775,

        ANDYMARK9015,

        ANDYMARKNEVEREST,

        ANDYMARKRS775125,

        ANDYMARKREDLINEA,

        REVROBOTICSHDHEXMOTOR,

        BANEBOTSRS77518V,

        BANEBOTSRS550,

        MODERNROBOTICS12VDCMOTOR,

        JOHNSONELECTRICALGEARMOTOR,

        TETRIXMAXTORQUENADOMOTOR,
    }

    [Serializable()]
    public enum motorFeedbackDevice
    {

        NONE,

        INTERNAL,

        QUADENCODER,

        ANALOG,

        TACHOMETER,

        PULSEWIDTHENCODERPOSITION,

        SENSORSUM,

        SENSORDIFFERENCE,

        REMOTESENSOR0,

        REMOTESENSOR1,

        SOFTWAREEMULATEDSENSOR,
    }
    #endregion

    [Serializable()]
    public class topLevelAppDataElement
    {
        public List<applicationData> Robots { get; set; }

        public List<mechanism> Mechanisms { get; set; }

        public topLevelAppDataElement()
        {
            helperFunctions.initializeNullProperties(this);
            helperFunctions.initializeDefaultValues(this);
        }

        public string getDisplayName()
        {
            return "robotBuildDefinition";
        }
    }

    [Serializable()]
    public partial class applicationData
    {
#if !enableTestAutomation
        public pdp PowerDistributionPanel { get; set; }
        public List<pcm> PneumaticControlModules { get; set; }
        public List<pigeon> Pigeons { get; set; }
        public List<limelight> Limelights { get; set; }
        public chassis Chassis { get; set; }
        public List<mechanismInstance> mechanismInstances { get; set; }
        public List<camera> Cameras { get; set; }
        public List<roborio> Roborios { get; set; }
        public List<led> Leds { get; set; }

        [DefaultValue(1u)]
        [Range(typeof(uint), "1", "9999")]
        public uintParameter robotID { get; set; }

        public string name { get; set; } = "Example";

        public applicationData()
        {
            helperFunctions.initializeNullProperties(this);
            helperFunctions.initializeDefaultValues(this);
        }

        public string getDisplayName(string propertyName, out helperFunctions.RefreshLevel refresh)
        {
            refresh = helperFunctions.RefreshLevel.none;

            if (string.IsNullOrEmpty(propertyName))
                return string.Format("Robot #{0}", robotID.value);
            //else if (propertyName == "testClass")
            //    return string.Format("{0} ({1}))", propertyName, testClass.name);
            else if (propertyName == "pdp")
                return string.Format("{0} ({1})", propertyName, PowerDistributionPanel.type);

            return "robot class - incomplete getDisplayName";
        }

        public string getFullRobotName()
        {
            return string.Format("{0}_{1}", name, robotID.value);
        }

        public List<string> generate(string generateFunctionName)
        {
            List<string> sb = new List<string>();

            PropertyInfo[] propertyInfos = this.GetType().GetProperties();
            foreach (PropertyInfo pi in propertyInfos) // add its children
            {
                if (baseDataConfiguration.isACollection(pi.PropertyType))
                {
                    object theObject = pi.GetValue(this);
                    if (theObject != null)
                    {
                        Type elementType = theObject.GetType().GetGenericArguments().Single();
                        ICollection ic = theObject as ICollection;
                        foreach (var v in ic)
                        {
                            if (v != null)
                            {
                                sb.AddRange(generate(v, generateFunctionName));
                            }
                        }
                    }
                }
                else
                {
                    object theObject = pi.GetValue(this);
                    if (theObject != null)
                        sb.AddRange(generate(theObject, generateFunctionName));
                }
            }

            return sb;
        }

        private List<string> generate(object obj, string generateFunctionName)
        {
            MethodInfo mi = obj.GetType().GetMethod(generateFunctionName);
            if (mi != null)
            {
                object[] parameters = new object[] { };
                return (List<string>)mi.Invoke(obj, parameters);
            }

            return new List<string>();
        }
#endif
    }

    [Serializable()]
    public class mechanismInstance
    {
        [XmlIgnore]
        public object theTreeNode = null;

        public string name { get; set; }

        public mechanism mechanism { get; set; }

        public mechanismInstance()
        {
            name = "mechanismInstanceName";
            helperFunctions.initializeNullProperties(this);
            helperFunctions.initializeDefaultValues(this);
        }

        public string getDisplayName(string propertyName, out helperFunctions.RefreshLevel refresh)
        {
            refresh = helperFunctions.RefreshLevel.parentHeader;

            if (propertyName == "")
                return name;

            PropertyInfo pi = this.GetType().GetProperty(propertyName);
            if (pi != null)
            {
                object value = pi.GetValue(this);
                return string.Format("{0} ({1})", propertyName, value.ToString());
            }

            return null;
        }

        public List<string> generate(string generateFunctionName)
        {
            List<string> sb = new List<string>();
            object obj = this.mechanism;

            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
            foreach (PropertyInfo pi in propertyInfos) // add its children
            {
                if (baseDataConfiguration.isACollection(pi.PropertyType))
                {
                    object theObject = pi.GetValue(obj);
                    if (theObject != null)
                    {
                        Type elementType = theObject.GetType().GetGenericArguments().Single();
                        ICollection ic = theObject as ICollection;
                        foreach (var v in ic)
                        {
                            if (v != null)
                            {
                                sb.AddRange(generate(v, generateFunctionName));
                            }
                        }
                    }
                }
                else
                {
                    object theObject = pi.GetValue(obj);
                    if (theObject != null)
                        sb.AddRange(generate(theObject, generateFunctionName));
                }
            }

            return sb;
        }

        private List<string> generate(object obj, string generateFunctionName)
        {
            MethodInfo mi = obj.GetType().GetMethod(generateFunctionName);
            if (mi != null)
            {
                object[] parameters = new object[] { };
                return (List<string>)mi.Invoke(obj, parameters);
            }

            return new List<string>();
        }

        public string getIncludePath()
        {
            return String.Format("mechanisms/{0}/decoratormods/{0}.h", name);
        }
    }

    [Serializable()]
    public partial class mechanism
    {
        [XmlIgnore]
        public object theTreeNode = null;

        public Guid GUID;

        [ConstantInMechInstance]
        public string name { get; set; }

        public override string ToString()
        {
            return name;
        }
#if !enableTestAutomation
        public List<MotorController> MotorControllers { get; set; }
        public List<PIDFZ> closedLoopControlParameters { get; set; }
        public List<solenoid> solenoid { get; set; }
        public List<servo> servo { get; set; }
        public List<analogInput> analogInput { get; set; }
        public List<digitalInput> digitalInput { get; set; }
        // not defined in /hw/Dragon.. public List<colorSensor> colorSensor { get; set; }
        public List<CANcoder> cancoder { get; set; }
        //public List<state> state { get; set; }

        public mechanism()
        {
            if ((GUID == null) || (GUID == new Guid()))
                GUID = Guid.NewGuid();

            helperFunctions.initializeNullProperties(this);

            name = GetType().Name;

            helperFunctions.initializeDefaultValues(this);
        }

        public string getDisplayName(string propertyName, out helperFunctions.RefreshLevel refresh)
        {
            refresh = helperFunctions.RefreshLevel.none;
            if (propertyName == "name")
                refresh = helperFunctions.RefreshLevel.parentHeader;

            return string.Format("{0}", name);
        }

        public List<string> generate(string generateFunctionName)
        {
            List<string> sb = new List<string>();

            PropertyInfo[] propertyInfos = this.GetType().GetProperties();
            foreach (PropertyInfo pi in propertyInfos) // add its children
            {
                if (baseDataConfiguration.isACollection(pi.PropertyType))
                {
                    object theObject = pi.GetValue(this);
                    if (theObject != null)
                    {
                        Type elementType = theObject.GetType().GetGenericArguments().Single();
                        ICollection ic = theObject as ICollection;
                        foreach (var v in ic)
                        {
                            if (v != null)
                            {
                                sb.AddRange(generate(v, generateFunctionName));
                            }
                        }
                    }
                }
                else
                {
                    object theObject = pi.GetValue(this);
                    if (theObject != null)
                        sb.AddRange(generate(theObject, generateFunctionName));
                }
            }

            return sb;
        }

        private List<string> generate(object obj, string generateFunctionName)
        {
            MethodInfo mi = obj.GetType().GetMethod(generateFunctionName);
            if (mi != null)
            {
                object[] parameters = new object[] { };
                return (List<string>)mi.Invoke(obj, parameters);
            }

            return new List<string>();
        }
#endif
    }

    [Serializable]
    public class CANcoderInstance : baseRobotElementClass
    {

    }

#if !enableTestAutomation
    [Serializable()]
    public class PID : baseRobotElementClass
    {
        [DefaultValue(0D)]
        [DataDescription("The proportional gain of the PIDF controller.")]
        [TunableParameter()]
        public doubleParameter pGain { get; set; }

        [DefaultValue(0D)]
        [DataDescription("The integral gain of the PIDF controller.")]
        [TunableParameter()]
        public doubleParameter iGain { get; set; }

        [DefaultValue(0D)]
        [DataDescription("The differential gain of the PIDF controller.")]
        [TunableParameter()]
        public doubleParameter dGain { get; set; }
        public PID()
        {
        }
    }

    [Serializable()]
    public class PIDF : PID
    {
        [DefaultValue(0D)]
        [DataDescription("The feed forward gain of the PIDF controller.")]
        [TunableParameter()]
        public doubleParameter fGain { get; set; }

        public PIDF()
        {
        }
    }

    [Serializable()]
    public class PIDFslot : PIDF
    {

        [DefaultValue(0D)]
        [DataDescription("The slot to store the PIDF settings.")]
        [Range(0, 3)]
        public intParameter slot { get; set; }

        public PIDFslot()
        {
        }
    }

    [Serializable()]
    public class PIDFZ : PIDF
    {
        [DefaultValue(0D)]
        [TunableParameter()]
        public doubleParameter iZone { get; set; }

        public PIDFZ()
        {
        }
    }

    [Serializable()]
    public class pdp : baseRobotElementClass
    {
        public enum pdptype { CTRE, REV, }

        [DefaultValue(pdptype.REV)]
        public pdptype type { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "62")]
        public uintParameter canID { get; set; }

        public pdp()
        {

        }
    }

    [Serializable()]
    public class pigeon : baseRobotElementClass
    {
        public enum pigeonType
        {
            pigeon1,
            pigeon2,
        }

        public enum pigeonPosition
        {
            CENTER_OF_ROTATION,
        }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "62")]
        public uintParameter canID { get; set; }

        [DefaultValue(CAN_BUS.rio)]
        public CAN_BUS canBus { get; set; }

        [DefaultValue("0.0")]
        [PhysicalUnitsFamily(physicalUnit.Family.angle)]
        public doubleParameter rotation { get; set; }

        [DefaultValue(pigeonType.pigeon1)]
        public pigeonType type { get; set; }

        [DefaultValue(pigeonPosition.CENTER_OF_ROTATION)]
        public pigeonPosition position { get; set; }

        public pigeon()
        {
        }
    }

    [Serializable()]
    public class pcm : baseRobotElementClass
    {
        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "62")]
        public uintParameter canID { get; set; }

        [DefaultValue(95.0)]
        [PhysicalUnitsFamily(physicalUnit.Family.pressure)]
        public doubleParameter minPressure { get; set; }

        [DefaultValue(115.0)]
        [PhysicalUnitsFamily(physicalUnit.Family.pressure)]
        public doubleParameter maxPressure { get; set; }

        public pcm()
        {
        }
    }

    [Serializable()]
    [XmlInclude(typeof(TalonFX))]
    [XmlInclude(typeof(TalonSRX))]
    public class MotorController : baseRobotElementClass
    {
        public enum RemoteSensorSource
        {
            Off,
            TalonSRX_SelectedSensor,
            Pigeon_Yaw,
            Pigeon_Pitch,
            Pigeon_Roll,
            CANifier_Quadrature,
            CANifier_PWMInput0,
            CANifier_PWMInput1,
            CANifier_PWMInput2,
            CANifier_PWMInput3,
            GadgeteerPigeon_Yaw,
            GadgeteerPigeon_Pitch,
            GadgeteerPigeon_Roll,
            CANCoder,
            TalonFX_SelectedSensor = TalonSRX_SelectedSensor,
        };

        public enum MOTOR_TYPE
        {
            UNKNOWN_MOTOR = -1,
            FALCON500,
            NEOMOTOR,
            NEO500MOTOR,
            CIMMOTOR,
            MINICIMMOTOR,
            BAGMOTOR,
            PRO775,
            ANDYMARK9015,
            ANDYMARKNEVEREST,
            ANDYMARKRS775125,
            ANDYMARKREDLINEA,
            REVROBOTICSHDHEXMOTOR,
            BANEBOTSRS77518V,
            BANEBOTSRS550,
            MODERNROBOTICS12VDCMOTOR,
            JOHNSONELECTRICALGEARMOTOR,
            TETRIXMAXTORQUENADOMOTOR,
            NONE,
            MAX_MOTOR_TYPES
        };

        [XmlIgnore]
        [Constant()]
        public string motorControllerType { get; protected set; }

        [DefaultValue(MOTOR_TYPE.UNKNOWN_MOTOR)]
        public MOTOR_TYPE motorType { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "62")]
        public uintParameter canID { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "62")]
        public uintParameter remoteSensorCanID { get; set; }

        [DefaultValue(CAN_BUS.rio)]
        public CAN_BUS canBusName { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "15")]
        public uintParameter pdpID { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "19")] // REV is 0-19, CTRE 0-15, cannot handle 2 ranges for now
        public uintParameter followID { get; set; }

        [DefaultValue(false)]
        public boolParameter enableFollowID { get; set; }

        [DefaultValue(RemoteSensorSource.Off)]
        public RemoteSensorSource remoteSensorSource { get; set; }

        [Serializable]
        public class FusedCANcoder : baseDataClass
        {
            [DefaultValue(false)]
            public boolParameter enable { get; set; }

            public CANcoderInstance fusedCANcoder { get; set; }

            public doubleParameter sensorToMechanismRatio { get; set; }
            public doubleParameter rotorToSensorRatio { get; set; }

            public FusedCANcoder()
            {
                defaultDisplayName = "FusedCANcoder";
            }
        }

        public FusedCANcoder fusedCANcoder { get;set; }

        public MotorController()
        {
            motorControllerType = this.GetType().Name;
        }


    }

    [Serializable()]
    [ImplementationName("DragonTalonFX")]
    [UserIncludeFile("hw/DragonTalonFX.h")]
    [Using("ctre::phoenixpro::signals::ForwardLimitSourceValue")]
    [Using("ctre::phoenixpro::signals::ForwardLimitTypeValue")]
    [Using("ctre::phoenixpro::signals::ReverseLimitSourceValue")]
    [Using("ctre::phoenixpro::signals::ReverseLimitTypeValue")]
    [Using("ctre::phoenixpro::signals::InvertedValue")]
    [Using("ctre::phoenixpro::signals::NeutralModeValue")]
    [Using("ctre::phoenix::motorcontrol::RemoteSensorSource")]
    public class TalonFX : MotorController
    {
        [Serializable]
        public class CurrentLimits : baseDataClass
        {
            [DefaultValue(false)]
            public boolParameter enableStatorCurrentLimit { get; set; }

            [DefaultValue(0)]
            [Range(typeof(double), "0", "40.0")] //todo choose a valid range
            [PhysicalUnitsFamily(physicalUnit.Family.current)]
            public doubleParameter statorCurrentLimit { get; set; }

            [DefaultValue(false)]
            public boolParameter enableSupplyCurrentLimit { get; set; }

            [DefaultValue(0)]
            [Range(typeof(double), "0", "40.0")] //todo choose a valid range
            [PhysicalUnitsFamily(physicalUnit.Family.current)]
            public doubleParameter supplyCurrentLimit { get; set; }

            [DefaultValue(0)]
            [Range(typeof(double), "0", "40.0")] //todo choose a valid range
            [PhysicalUnitsFamily(physicalUnit.Family.current)]
            public doubleParameter supplyCurrentThreshold { get; set; }

            [DefaultValue(0)]
            [Range(typeof(double), "0", "40.0")] //todo choose a valid range
            [PhysicalUnitsFamily(physicalUnit.Family.time)]
            public doubleParameter supplyTimeThreshold { get; set; }

            public CurrentLimits()
            {
                defaultDisplayName = "CurrentLimits";
            }
        }
        public CurrentLimits theCurrentLimits { get; set; }

        public List<PIDFslot> PIDFs { get; set; }

        [Serializable]
        public class ConfigHWLimitSW : baseDataClass
        {
            public enum ForwardLimitSourceValue { LimitSwitchPin }
            public enum ForwardLimitTypeValue { NormallyOpen, NormallyClosed }
            public enum ReverseLimitSourceValue { LimitSwitchPin }
            public enum ReverseLimitTypeValue { NormallyOpen, NormallyClosed }
            public boolParameter enableForward { get; set; }
            public intParameter remoteForwardSensorID { get; set; }
            public boolParameter forwardResetPosition { get; set; }
            public doubleParameter forwardPosition { get; set; }
            public ForwardLimitSourceValue forwardType { get; set; }
            public ForwardLimitTypeValue forwardOpenClose { get; set; }
            public boolParameter enableReverse { get; set; }
            public intParameter remoteReverseSensorID { get; set; }
            public boolParameter reverseResetPosition { get; set; }
            public doubleParameter reversePosition { get; set; }
            public ReverseLimitSourceValue revType { get; set; }
            public ReverseLimitTypeValue revOpenClose { get; set; }

            public ConfigHWLimitSW()
            {
                defaultDisplayName = "ConfigHWLimitSW";
            }
        }
        public ConfigHWLimitSW theConfigHWLimitSW { get; set; }

        [Serializable]
        public class ConfigMotorSettings : baseDataClass
        {
            public enum InvertedValue { CounterClockwise_Positive, Clockwise_Positive }
            public enum NeutralModeValue { Coast, Brake }

            [DefaultValue(0)]
            [Range(typeof(double), "0", "100")]
            [PhysicalUnitsFamily(physicalUnit.Family.percent)]
            [TunableParameter()]
            public doubleParameter deadbandPercent { get; set; }

            [DefaultValue(0)]
            [Range(typeof(double), "0", "1.0")]
            [PhysicalUnitsFamily(physicalUnit.Family.none)]
            [TunableParameter()]
            public doubleParameter peakForwardDutyCycle { get; set; }

            [DefaultValue(0)]
            [Range(typeof(double), "-1.0", "0.0")]
            [PhysicalUnitsFamily(physicalUnit.Family.none)]
            public doubleParameter peakReverseDutyCycle { get; set; }

            [DefaultValue(InvertedValue.CounterClockwise_Positive)]
            public InvertedValue inverted { get; set; }

            [DefaultValue(NeutralModeValue.Coast)]
            public NeutralModeValue mode { get; set; }

            public ConfigMotorSettings()
            {
                defaultDisplayName = "ConfigMotorSettings";
            }
        }
        public ConfigMotorSettings theConfigMotorSettings { get; set; }

        public doubleParameter diameter { get; set; }

        /* It seems that the following are not needed

        [Serializable]
        public class VoltageConfigs : baseDataClass
        {
            [DefaultValue(0)]
            [Range(typeof(double), "0", "40.0")] //todo choose a valid range
            [PhysicalUnitsFamily(physicalUnit.Family.time)]
            public doubleParameter peakForwardVoltage { get; set; }

            [DefaultValue(0)]
            [Range(typeof(double), "0", "40.0")] //todo choose a valid range
            [PhysicalUnitsFamily(physicalUnit.Family.time)]
            public doubleParameter peakReverseVoltage { get; set; }

            [DefaultValue(0)]
            [Range(typeof(double), "0", "40.0")] //todo choose a valid range
            [PhysicalUnitsFamily(physicalUnit.Family.time)]
            public doubleParameter supplyVoltageTime { get; set; }

            public VoltageConfigs()
            {
                defaultDisplayName = "VoltageConfigs";
            }
        }
        public VoltageConfigs theVoltageConfigs { get; set; }

        [Serializable]
        public class TorqueConfigs : baseDataClass
        {
            [DefaultValue(0)]
            [Range(typeof(double), "0", "40.0")] //todo choose a valid range
            [PhysicalUnitsFamily(physicalUnit.Family.current)]
            public doubleParameter peakForwardTorqueCurrent { get; set; }

            [DefaultValue(0)]
            [Range(typeof(double), "0", "40.0")] //todo choose a valid range
            [PhysicalUnitsFamily(physicalUnit.Family.current)]
            public doubleParameter peakReverseTorqueCurrent { get; set; }

            [DefaultValue(0)]
            [Range(typeof(double), "0", "40.0")] //todo choose a valid range
            [PhysicalUnitsFamily(physicalUnit.Family.current)]
            public doubleParameter torqueNeutralDeadband { get; set; }

            public TorqueConfigs()
            {
                defaultDisplayName = "TorqueConfigs";
            }
        }
        public TorqueConfigs theTorqueConfigs { get; set; }

        [Serializable]
        public class FeedbackConfigs : baseDataClass
        {
            public enum FeedbackSensorSource { RotorSensor, RemoteCANcoder, FusedCANcoder }

            [DefaultValue(0)]
            [Range(typeof(double), "0", "40.0")] //todo choose a valid range
            [PhysicalUnitsFamily(physicalUnit.Family.angle)]
            public doubleParameter feedbackRotorOffset { get; set; }

            [DefaultValue(FeedbackSensorSource.RotorSensor)]
            public FeedbackSensorSource feedbackSensor { get; set; }

            [DefaultValue(0)]
            public intParameter remoteSensorID { get; set; }

            public FeedbackConfigs()
            {
                defaultDisplayName = "FeedbackConfigs";
            }
        }
        public FeedbackConfigs theFeedbackConfigs { get; set; }

        */
        public TalonFX()
        {
        }

        override public List<string> generateInitialization()
        {
            List<string> initCode = new List<string>();

            initCode.Add(string.Format(@"{0}->SetCurrentLimits({1},
                                            {2}({3}),
                                            {4},
                                            {5}({6}),
                                            {7}({8}),
                                            {9}({10}));",
                                            name, theCurrentLimits.enableStatorCurrentLimit.value.ToString().ToLower(),
                                            generatorContext.theGeneratorConfig.getWPIphysicalUnitType(theCurrentLimits.statorCurrentLimit.__units__), theCurrentLimits.statorCurrentLimit.value,
                                            theCurrentLimits.enableSupplyCurrentLimit.value.ToString().ToLower(),
                                            generatorContext.theGeneratorConfig.getWPIphysicalUnitType(theCurrentLimits.supplyCurrentLimit.__units__), theCurrentLimits.supplyCurrentLimit.value,
                                            generatorContext.theGeneratorConfig.getWPIphysicalUnitType(theCurrentLimits.supplyCurrentThreshold.__units__), theCurrentLimits.supplyCurrentThreshold.value,
                                            generatorContext.theGeneratorConfig.getWPIphysicalUnitType(theCurrentLimits.supplyTimeThreshold.__units__), theCurrentLimits.supplyTimeThreshold.value
                                            ));

            foreach (PIDFslot pIDFslot in PIDFs)
            {
                initCode.Add(string.Format(@"{0}->SetPIDConstants({1}, // slot
                                                                    {2}, // P
                                                                    {3}, // I
                                                                    {4}, // D
                                                                    {5}); // F",
                                            name,
                                            pIDFslot.slot.value,
                                            pIDFslot.pGain.value,
                                            pIDFslot.iGain.value,
                                            pIDFslot.dGain.value,
                                            pIDFslot.fGain.value
                                            ));
            }

            initCode.Add(string.Format(@"{0}->ConfigHWLimitSW({1}, // enableForward
                                            {2}, // remoteForwardSensorID                  
                                            {3}, // forwardResetPosition                 
                                            {4}, // forwardPosition                 
                                            {5}::{6}, // forwardType
                                            {7}::{8}, // forwardOpenClose
                                            {9}, // enableReverse
                                            {10}, // remoteReverseSensorID
                                            {11}, // reverseResetPosition
                                            {12}, // reversePosition
                                            {13}::{14}, // revType
                                            {15}::{16} ); // revOpenClose"
                                            ,
                                            name,
                                            theConfigHWLimitSW.enableForward.value.ToString().ToLower(),
                                            theConfigHWLimitSW.remoteForwardSensorID.value,
                                            theConfigHWLimitSW.forwardResetPosition.value.ToString().ToLower(),
                                            theConfigHWLimitSW.forwardPosition.value,

                                            theConfigHWLimitSW.forwardType.GetType().Name,
                                            theConfigHWLimitSW.forwardType,
                                            theConfigHWLimitSW.forwardOpenClose.GetType().Name,
                                            theConfigHWLimitSW.forwardOpenClose,

                                            theConfigHWLimitSW.enableReverse.value.ToString().ToLower(),
                                            theConfigHWLimitSW.remoteReverseSensorID.value,
                                            theConfigHWLimitSW.reverseResetPosition.value.ToString().ToLower(),
                                            theConfigHWLimitSW.reversePosition.value,

                                            theConfigHWLimitSW.revType.GetType().Name,
                                            theConfigHWLimitSW.revType,
                                            theConfigHWLimitSW.revOpenClose.GetType().Name,
                                            theConfigHWLimitSW.revOpenClose
                                           ));

            initCode.Add(string.Format(@"{0}->ConfigMotorSettings({1}::{2}, // ctre::phoenixpro::signals::InvertedValue
                                            {3}::{4}, // ctre::phoenixpro::signals::NeutralModeValue                  
                                            {5}, // deadbandPercent                 
                                            {6}, // peakForwardDutyCycle                 
                                            {7} ); // peakReverseDutyCycle"
                                            ,
                                            name,

                                            theConfigMotorSettings.inverted.GetType().Name,
                                            theConfigMotorSettings.inverted,

                                            theConfigMotorSettings.mode.GetType().Name,
                                            theConfigMotorSettings.mode,

                                            theConfigMotorSettings.deadbandPercent.value,
                                            theConfigMotorSettings.peakForwardDutyCycle.value,
                                            theConfigMotorSettings.peakReverseDutyCycle.value
                                           ));

            initCode.Add(string.Format(@"{0}->SetAsFollowerMotor({1} ); // masterCANID",
                                            name,
                                            followID.value
                                            ));

            initCode.Add(string.Format(@"{0}->SetRemoteSensor({1}, // canID
                                                              {2}::{2}_{3} ); // ctre::phoenix::motorcontrol::RemoteSensorSource",
                                            name,
                                            remoteSensorCanID.value,
                                            remoteSensorSource.GetType().Name,
                                            remoteSensorSource
                                            ));

            if( fusedCANcoder.enable.value == true)
            {
                initCode.Add(string.Format(@"{0}->FuseCancoder(*{1}, // DragonCanCoder &cancoder
                                                               {2}, // sensorToMechanismRatio
                                                               {3} ); // rotorToSensorRatio",
                                            name,
                                            fusedCANcoder.fusedCANcoder.name,
                                            fusedCANcoder.sensorToMechanismRatio.value,
                                            fusedCANcoder.rotorToSensorRatio.value
                                            ));
            }

            initCode.Add(string.Format(@"{0}->SetDiameter({1} ); // double diameter",
                                name,
                                diameter.value
                                ));

            return initCode;
        }

        override public List<string> generateObjectCreation()
        {
            string creation = string.Format("{0} = new {1}(\"{0}\",RobotElementNames::{2},{3},\"{4}\")",
                name,
                getImplementationName(),
                utilities.ListToString(generateElementNames()).ToUpper().Replace("::", "_USAGE::"),
                canID.value.ToString(),
                canBusName.ToString());

            return new List<string> { creation };
        }
    }

    [Serializable()]
    [ImplementationName("DragonTalonSRX")]
    [UserIncludeFile("hw/DragonTalonSRX.h")]
    public class TalonSRX : MotorController
    {
        [Serializable]
        public class DistanceAngleCalcInfo : baseDataClass
        {
            [DefaultValue(0)]
            public intParameter countsPerRev { get; set; }

            [DefaultValue(1.0)]
            public doubleParameter gearRatio { get; set; }

            [DefaultValue(1.0)]
            [PhysicalUnitsFamily(physicalUnit.Family.length)]
            public doubleParameter diameter { get; set; }

            [DefaultValue(0)]
            public doubleParameter countsPerInch { get; set; }

            [DefaultValue(0)]
            public doubleParameter countsPerDegree { get; set; }

            public DistanceAngleCalcInfo()
            {
            }

            public override string ToString()
            {
                return string.Format("{{ {0}, {1}, {2}, {3}, {4} }}", countsPerRev.value, gearRatio.value, diameter.value, countsPerInch.value, countsPerDegree.value);
            }
        }
        public DistanceAngleCalcInfo theDistanceAngleCalcInfo { get; set; }


        [DefaultValue(1.1)]
        [Range(typeof(double), "0", "62")]
        [TunableParameter()]
        public doubleParameter deadbandPercent_ { get; set; }

        [DefaultValue(2.2)]
        [Range(typeof(double), "-1.0", "3.0")]
        public doubleParameter peakMin_ { get; set; }

        [DefaultValue(4.4)]
        [Range(typeof(double), "-10.0", "20.0")]
        [TunableParameter()]
        public doubleParameter peakMax_ { get; set; }

        public TalonSRX()
        {
        }

        override public List<string> generateInitialization()
        {
            List<string> initCode = new List<string>();

            //initCode.Add(string.Format(@"{0}->SetCurrentLimits({1},
            //                                {2}({3}),
            //                                {4},
            //                                {5}({6}),
            //                                {7}({8}),
            //                                {9}({10}));",
            //                                name, theCurrentLimits.enableStatorCurrentLimit.value.ToString().ToLower(),
            //                                generatorContext.theGeneratorConfig.getWPIphysicalUnitType(theCurrentLimits.statorCurrentLimit.__units__), theCurrentLimits.statorCurrentLimit.value,
            //                                theCurrentLimits.enableSupplyCurrentLimit.value.ToString().ToLower(),
            //                                generatorContext.theGeneratorConfig.getWPIphysicalUnitType(theCurrentLimits.supplyCurrentLimit.__units__), theCurrentLimits.supplyCurrentLimit.value,
            //                                generatorContext.theGeneratorConfig.getWPIphysicalUnitType(theCurrentLimits.supplyCurrentThreshold.__units__), theCurrentLimits.supplyCurrentThreshold.value,
            //                                generatorContext.theGeneratorConfig.getWPIphysicalUnitType(theCurrentLimits.supplyTimeThreshold.__units__), theCurrentLimits.supplyTimeThreshold.value
            //                                ));

            //todo add the TalonSRX initialization

            return initCode;
        }

        override public List<string> generateObjectCreation()
        {
            /*
                 DragonTalonSRX(std::string networkTableName,
                   RobotElementNames::MOTOR_CONTROLLER_USAGE deviceType,
                   int deviceID,
                   int pdpID,
                   const DistanceAngleCalcStruc &calcStruc,
                   IDragonMotorController::MOTOR_TYPE motortype

    );
             */
            string creation = string.Format("{0} = new {1}(\"{0}\",RobotElementNames::{2},{3},{4},{5}, IDragonMotorController::MOTOR_TYPE::{6})",
                name,
                getImplementationName(),
                utilities.ListToString(generateElementNames()).ToUpper().Replace("::", "_USAGE::"),
                canID.value.ToString(),
                pdpID.value.ToString(),
                theDistanceAngleCalcInfo.ToString(),
                motorType);

            return new List<string> { creation };
        }
    }

    [Serializable()]
    [ImplementationName("DragonAnalogInput")]
    [UserIncludeFile("hw/DragonAnalogInput.h")]
    public class analogInput : baseRobotElementClass
    {
        public enum analogInputType
        {
            ANALOG_GENERAL,
            ANALOG_GYRO,
            POTENTIOMETER,
            PRESSURE_GAUGE,
            ELEVATOR_HEIGHT
        }

        [DefaultValue(analogInputType.PRESSURE_GAUGE)]
        [TunableParameter()]
        public analogInputType type { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "7")]
        public uintParameter analogId { get; set; }

        [DefaultValue(0D)]
        public doubleParameter voltageMin { get; set; }

        [DefaultValue(5D)]
        public doubleParameter voltageMax { get; set; }

        public doubleParameter outputMin { get; set; }

        public doubleParameter outputMax { get; set; }

        public analogInput()
        {
        }

        override public List<string> generateObjectCreation()
        {
            string creation = string.Format("{0} = new {1}(\"{0}\",{1}::ANALOG_SENSOR_TYPE::{2},{3},{4},{5},{6},{7})",
                name,
                getImplementationName(),
                type,
                analogId.value,
                voltageMin.value,
                voltageMax.value,
                outputMin.value,
                outputMax.value
                );

            return new List<string> { creation };
        }

        override public List<string> generateInitialization()
        {
            List<string> initCode = new List<string>()
            {
                string.Format("// {0} : Analog inputs do not have initialization needs", name)
            };

            return initCode;
        }
    }

    [Serializable()]
    public class limelight : baseRobotElementClass
    {
        public enum limelightRotation
        {
            Angle_0_deg = 0,
            Angle_90_deg = 90,
            Angle_180_deg = 180,
            Angle_270_deg = 270,
        }
        public enum limelightDefaultLedMode
        {
            currentPipeline,
            off,
            blink,
            on,
        }
        public enum limelightDefaultCamMode
        {
            vision,
            driverCamera,
        }
        public enum limelightStreamMode
        {
            sideBySide,
            pipMain,
            pipSecondary,
        }
        public enum limelightSnapshots
        {
            off,
            twoPerSec,
        }

        [DefaultValue(0.0)]
        [PhysicalUnitsFamily(physicalUnit.Family.length)]
        public doubleParameter mountingheight { get; set; }

        [DefaultValue(0.0)]
        [PhysicalUnitsFamily(physicalUnit.Family.length)]
        public doubleParameter horizontaloffset { get; set; }

        [DefaultValue(0.0)]
        [PhysicalUnitsFamily(physicalUnit.Family.angle)]
        public doubleParameter mountingangle { get; set; }

        [DefaultValue(limelightRotation.Angle_0_deg)]
        [PhysicalUnitsFamily(physicalUnit.Family.angle)]
        public limelightRotation rotation { get; set; }

        public List<doubleParameterUserDefinedTunable> tunableParameters { get; set; }

        [DefaultValue(limelightDefaultLedMode.currentPipeline)]
        public limelightDefaultLedMode defaultledmode { get; set; }

        [DefaultValue(limelightDefaultCamMode.vision)]
        public limelightDefaultCamMode defaultcammode { get; set; }

        [DefaultValue(limelightStreamMode.sideBySide)]
        public limelightStreamMode streammode { get; set; }

        [DefaultValue(limelightSnapshots.off)]
        public limelightSnapshots snapshots { get; set; }

        public limelight()
        {
        }
    }


    [Serializable()]
    public class chassis
    {
        public enum chassisType
        {
            TANK,
            MECANUM,
            SWERVE,
        }
        public enum chassisWheelSpeedCalcOption
        {
            WPI,
            ETHER,
            _2910,
        }
        public enum chassisPoseEstimationOption
        {
            WPI,
            EULERCHASSIS,
            EULERWHEEL,
            POSECHASSIS,
            POSEWHEEL,
        }

        public List<MotorController> motor { get; set; }
        public List<swerveModule> swervemodule { get; set; }

        [DefaultValue(chassisType.TANK)]
        public chassisType type { get; set; }

        [DefaultValue(1.0)]
        [PhysicalUnitsFamily(physicalUnit.Family.length)]
        public doubleParameter wheelDiameter { get; set; }

        [DefaultValue(1.0)]
        [PhysicalUnitsFamily(physicalUnit.Family.length)]
        public doubleParameter wheelBase { get; set; }

        [DefaultValue(1.0)]
        [PhysicalUnitsFamily(physicalUnit.Family.length)]
        public doubleParameter track { get; set; }

        [DefaultValue(chassisWheelSpeedCalcOption.ETHER)]
        public chassisWheelSpeedCalcOption wheelSpeedCalcOption { get; set; }

        [DefaultValue(chassisPoseEstimationOption.EULERCHASSIS)]
        public chassisPoseEstimationOption poseEstimationOption { get; set; }

        [TunableParameter]
        [PhysicalUnitsFamily(physicalUnit.Family.velocity)]
        public doubleParameter maxVelocity { get; set; }

        [TunableParameter]
        [PhysicalUnitsFamily(physicalUnit.Family.angularVelocity)]
        public doubleParameter maxAngularVelocity { get; set; }

        [TunableParameter]
        [PhysicalUnitsFamily(physicalUnit.Family.acceleration)]
        public doubleParameter maxAcceleration { get; set; }

        [TunableParameter]
        [PhysicalUnitsFamily(physicalUnit.Family.angularAcceleration)]
        public doubleParameter maxAngularAcceleration { get; set; }

        public chassis()
        {
            helperFunctions.initializeNullProperties(this);
            helperFunctions.initializeDefaultValues(this);
        }


        public string getDisplayName(string propertyName, out helperFunctions.RefreshLevel refresh)
        {
            refresh = helperFunctions.RefreshLevel.none;

            if (string.IsNullOrEmpty(propertyName))
                return "Chassis";

            PropertyInfo pi = this.GetType().GetProperty(propertyName);
            if (pi == null)
                return string.Format("chassis.getDisplayName : pi is null for propertyName {0}", propertyName);

            object obj = pi.GetValue(this);
            if (obj == null)
                return string.Format("chassis.getDisplayName : obj is null for propertyName {0}", propertyName);

            if (obj is parameter)
            {
                pi = this.GetType().GetProperty("value");
                obj = pi.GetValue(this);
            }

            return string.Format("{0} ({1})", propertyName, obj.ToString());
        }
    }

    [Serializable()]
    [ImplementationName("DragonDigitalInput")]
    [UserIncludeFile("hw/DragonDigitalInput.h")]
    public class digitalInput : baseRobotElementClass
    {
        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "25")]
        [PhysicalUnitsFamily(physicalUnit.Family.none)]
        public uintParameter digitalId { get; set; }

        [DefaultValue(false)]
        public boolParameter reversed { get; set; }

        [DefaultValue(0D)]
        [PhysicalUnitsFamily(physicalUnit.Family.time)]
        public doubleParameter debouncetime { get; set; }

        public digitalInput()
        {
        }

        override public List<string> generateObjectCreation()
        {
            string creation = string.Format("{0} = new {1}(\"{0}\",RobotElementNames::{2},{3},{4},{5}({6}))",
                name,
                getImplementationName(),
                utilities.ListToString(generateElementNames()).ToUpper().Replace("::", "_USAGE::"),
                digitalId.value,
                reversed.value.ToString().ToLower(),
                generatorContext.theGeneratorConfig.getWPIphysicalUnitType(debouncetime.__units__),
                debouncetime.value
                );

            return new List<string> { creation };
        }

        override public List<string> generateInitialization()
        {
            List<string> initCode = new List<string>()
            {
                string.Format("// {0} : Digital inputs do not have initialization needs", name)
            };

            return initCode;
        }
    }

    [Serializable()]
    public class swerveModule : baseRobotElementClass
    {
        public enum swervemoduletype
        {
            LEFT_FRONT,
            RIGHT_FRONT,
            LEFT_BACK,
            RIGHT_BACK,
        }

        public List<MotorController> motor { get; set; }
        public CANcoder cancoder { get; set; }
        [DefaultValue(swervemoduletype.LEFT_FRONT)]
        public swervemoduletype position { get; set; }
        public PIDFZ controlParameters { get; set; }

        [DefaultValue(0.0)]
        [TunableParameter()]
        [PhysicalUnitsFamily(physicalUnit.Family.percent)]
        public doubleParameter turn_nominal_val { get; set; }

        [DefaultValue(0.0)]
        [TunableParameter()]
        [PhysicalUnitsFamily(physicalUnit.Family.percent)]
        public doubleParameter turn_peak_val { get; set; }

        [DefaultValue(0.0)]
        [TunableParameter()]
        [PhysicalUnitsFamily(physicalUnit.Family.angularAcceleration)]
        public doubleParameter turn_max_acc { get; set; }

        [DefaultValue(0.0)]
        [TunableParameter()]
        [PhysicalUnitsFamily(physicalUnit.Family.angularVelocity)]
        public doubleParameter turn_cruise_vel { get; set; }

        [DefaultValue(1.0)]
        public uintParameter countsOnTurnEncoderPerDegreesOnAngleSensor { get; set; }

        public swerveModule()
        {
            motor = new List<MotorController>();
            helperFunctions.initializeNullProperties(this);
            helperFunctions.initializeDefaultValues(this);
        }
    }


    [Serializable()]
    [ImplementationName("DragonCanCoder")]
    [UserIncludeFile("hw/DragonCanCoder.h")]
    public class CANcoder : baseRobotElementClass
    {
        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "62")]
        public uintParameter canID { get; set; }

        [DefaultValue(CAN_BUS.rio)]
        public CAN_BUS canBusName { get; set; }

        [DefaultValue(0D)]
        public doubleParameter offset { get; set; }

        [DefaultValue(false)]
        public boolParameter reverse { get; set; }

        public CANcoder()
        {
        }

        override public List<string> generateObjectCreation()
        {
            string creation = string.Format("{0} = new {1}(\"{0}\",RobotElementNames::{2},{3},\"{4}\",{5},{6})",
                name,
                getImplementationName(),
                utilities.ListToString(generateElementNames()).ToUpper().Replace("::", "_USAGE::"),
                canID.value,
                canBusName,
                offset.value,
                reverse.value.ToString().ToLower()
                );

            return new List<string> { creation };
        }

        override public List<string> generateInitialization()
        {
            List<string> initCode = new List<string>()
            {
                string.Format("// {0} : CANcoder inputs do not have initialization needs", name)
            };

            return initCode;
        }
    }


    [Serializable()]
    [ImplementationName("DragonSolenoid")]
    [UserIncludeFile("hw/DragonSolenoid.h")]
    public class solenoid : baseRobotElementClass
    {
        public enum solenoidtype
        {
            CTREPCM,
            REVPH,
        }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "62")]
        public uintParameter CAN_ID { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "7")]
        public uintParameter channel { get; set; }

        [DefaultValue(false)]
        public boolParameter enableDualChannel { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "7")]
        public uintParameter forwardChannel { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "7")]
        public uintParameter reverseChannel { get; set; }


        [DefaultValue(false)]
        public boolParameter reversed { get; set; }

        [DefaultValue(solenoidtype.REVPH)]
        public solenoidtype type { get; set; }

        public solenoid()
        {
        }

        override public List<string> generateObjectCreation()
        {
            string creation = "";

            if (enableDualChannel.value)
            {
                creation = string.Format("{0} = new {1}(\"{0}\",RobotElementNames::{2},{3},frc::PneumaticsModuleType::{4},{5},{6},{7})",
                    name,
                    getImplementationName(),
                    utilities.ListToString(generateElementNames()).ToUpper().Replace("::", "_USAGE::"),
                    CAN_ID.value,
                    type,
                    forwardChannel.value,
                    reverseChannel.value,
                    reversed.value.ToString().ToLower()
                    );
            }
            else
            {
                creation = string.Format("{0} = new {1}(\"{0}\",RobotElementNames::{2},{3},frc::PneumaticsModuleType::{4},{5},{6})",
                    name,
                    getImplementationName(),
                    utilities.ListToString(generateElementNames()).ToUpper().Replace("::", "_USAGE::"),
                    CAN_ID.value,
                    type,
                    channel.value,
                    reversed.value.ToString().ToLower()
                    );
            }

            return new List<string> { creation };
        }

        override public List<string> generateInitialization()
        {
            List<string> initCode = new List<string>()
            {
                string.Format("// {0} : Solenoids do not have initialization needs", name)
            };

            return initCode;
        }
    }

    [Serializable()]
    [ImplementationName("DragonServo")]
    [UserIncludeFile("hw/DragonServo.h")]
    public class servo : baseRobotElementClass
    {
        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "19")]
        public uintParameter Id { get; set; }

        [DefaultValue(0.0)]
        [Range(typeof(double), "0", "360")]
        [PhysicalUnitsFamily(physicalUnit.Family.angle)]
        public doubleParameter minAngle { get; set; }

        [DefaultValue(360.0)]
        [Range(typeof(double), "0", "360")]
        [PhysicalUnitsFamily(physicalUnit.Family.angle)]
        public doubleParameter maxAngle { get; set; }

        public servo()
        {
        }

        override public List<string> generateObjectCreation()
        {
            string creation = string.Format("{0} = new {1}(RobotElementNames::{2},{3},{4}({5}),{6}({7}))",
                name,
                getImplementationName(),
                utilities.ListToString(generateElementNames()).ToUpper().Replace("::", "_USAGE::"),
                Id.value,
                generatorContext.theGeneratorConfig.getWPIphysicalUnitType(minAngle.__units__),
                minAngle.value,
                generatorContext.theGeneratorConfig.getWPIphysicalUnitType(maxAngle.__units__),
                maxAngle.value
                );

            return new List<string> { creation };
        }

        override public List<string> generateInitialization()
        {
            List<string> initCode = new List<string>()
            {
                string.Format("// {0} : Servos do not have initialization needs", name)
            };

            return initCode;
        }
    }


    [Serializable()]
    [ImplementationName("DragonColorSensor")]
    [UserIncludeFile("hw/DragonColorSensor.h")]
    public class colorSensor : baseRobotElementClass
    {
        public enum colorSensorPort
        {
            kOnboard,
            kMXP,
        }

        [DefaultValue(colorSensorPort.kOnboard)]
        public colorSensorPort port { get; set; }

        public colorSensor()
        {
        }
    }

    [Serializable()]
    public class camera : baseRobotElementClass
    {
        public enum cameraformat
        {
            KMJPEG,
            KYUYV,
            KRGB565,
            KBGR,
            KGRAY,
        }

        [DefaultValue("0")]
        public uintParameter id { get; set; }

        [DefaultValue(cameraformat.KMJPEG)]
        public cameraformat format { get; set; }

        [DefaultValue(640)]
        public uintParameter width { get; set; }

        [DefaultValue(480)]
        public uintParameter height { get; set; }

        [DefaultValue(30)]
        public uintParameter fps { get; set; }

        [DefaultValue(false)]
        public boolParameter thread { get; set; }

        public camera()
        {
        }
    }

    [Serializable()]
    public class roborio : baseRobotElementClass
    {
        public enum Orientation
        {
            X_FORWARD_Y_LEFT,
            X_LEFT_Y_BACKWARD,
            X_BACKWARD_Y_RIGHT,
            X_RIGHT_Y_FORWARD,
            X_FORWARD_Y_RIGHT,
            X_LEFT_Y_FORWARD,
            X_BACKWARD_Y_LEFT,
            X_RIGHT_Y_BACKWARD,
            X_UP_Y_LEFT,
            X_LEFT_Y_DOWN,
            X_DOWN_Y_RIGHT,
            X_RIGHT_Y_UP,
            X_UP_Y_RIGHT,
            X_LEFT_Y_UP,
            X_DOWN_Y_LEFT,
            X_RIGHT_Y_DOWN,
        }

        [DefaultValue(Orientation.X_FORWARD_Y_LEFT)]
        public Orientation orientation { get; set; }

        public roborio()
        {
            helperFunctions.initializeNullProperties(this);
            helperFunctions.initializeDefaultValues(this);
        }
    }


    [Serializable()]
    public class led : baseRobotElementClass
    {
        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "19")]
        public uintParameter Id { get; set; }

        [DefaultValue(0u)]
        public uintParameter count { get; set; }

        public led()
        {
        }
    }

    [Serializable()]
    public class talontach : baseRobotElementClass
    {
        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "62")]
        public uintParameter CAN_ID { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "6")]
        public uintParameter usage { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "11")]
        public uintParameter generalpin { get; set; }

        public talontach()
        {
        }
    }
#endif

    [Serializable]
    public class baseRobotElementClass
    {
        [ConstantInMechInstance]
        public string name { get; set; }

        public baseRobotElementClass()
        {
            helperFunctions.initializeNullProperties(this, true);
            helperFunctions.initializeDefaultValues(this);
            name = GetType().Name;
        }

        virtual public string getDisplayName(string propertyName, out helperFunctions.RefreshLevel refresh)
        {
            refresh = helperFunctions.RefreshLevel.none;

            if (string.IsNullOrEmpty(propertyName))
                refresh = helperFunctions.RefreshLevel.none;
            else if (propertyName == "name")
                refresh = helperFunctions.RefreshLevel.parentHeader;

            if (string.IsNullOrEmpty(propertyName))
                return name;

            PropertyInfo pi = this.GetType().GetProperty(propertyName);
            if (pi == null)
                return string.Format("baseRobotElementClass.getDisplayName : pi is null for propertyName {0}", propertyName);

            object obj = pi.GetValue(this);
            if (obj == null)
                return string.Format("baseRobotElementClass.getDisplayName : obj is null for propertyName {0}", propertyName);

            if (obj is parameter)
            {
                pi = this.GetType().GetProperty("value");
                obj = pi.GetValue(this);
            }

            return string.Format("{0} ({1})", propertyName, obj.ToString());
        }

        virtual public List<string> generateElementNames()
        {
            Type baseType = GetType();
            while ((baseType.BaseType != typeof(object)) && (baseType.BaseType != typeof(baseRobotElementClass)))
                baseType = baseType.BaseType;
            if (generatorContext.theMechanism != null)
            {
                return new List<string> { string.Format("{2}::{0}_{1}", ToUnderscoreCase(generatorContext.theMechanism.name), ToUnderscoreCase(name), ToUnderscoreCase(baseType.Name)) };
            }
            else if (generatorContext.theRobot != null)
                return new List<string> { string.Format("{1}::{0}", ToUnderscoreCase(name), ToUnderscoreCase(baseType.Name)) };
            else
                return new List<string> { "generateElementNames got to the else statement...should not be here" };
        }
        public string getImplementationName()
        {
            ImplementationNameAttribute impNameAttr = this.GetType().GetCustomAttribute<ImplementationNameAttribute>();
            if (impNameAttr == null)
                return this.GetType().Name;

            return impNameAttr.name;
        }
        virtual public List<string> generateDefinition()
        {
            return new List<string> { string.Format("{0}* {1};", getImplementationName(), name) };
        }
        virtual public List<string> generateInitialization()
        {
            return new List<string> { "baseRobotElementClass.generateInitialization needs to be overridden" };
        }
        virtual public List<string> generateObjectCreation()
        {
            return new List<string> { "baseRobotElementClass.generateInitialization needs to be overridden" };
        }

        virtual public List<string> generateIncludes()
        {
            List<string> sb = new List<string>();
            List<UserIncludeFileAttribute> userIncludesAttr = this.GetType().GetCustomAttributes<UserIncludeFileAttribute>().ToList();
            foreach (UserIncludeFileAttribute include in userIncludesAttr)
                sb.Add(string.Format("#include \"{0}\"{1}", include.pathName, Environment.NewLine));

            List<SystemIncludeFileAttribute> sysIncludesAttr = this.GetType().GetCustomAttributes<SystemIncludeFileAttribute>().ToList();
            foreach (SystemIncludeFileAttribute include in sysIncludesAttr)
                sb.Add(string.Format("#include <{0}>{1}", include.pathName, Environment.NewLine));

            return sb;
        }

        virtual public List<string> generateUsings()
        {
            List<string> sb = new List<string>();
            List<UsingAttribute> usingsAttr = this.GetType().GetCustomAttributes<UsingAttribute>().ToList();
            foreach (UsingAttribute u in usingsAttr)
                sb.Add(string.Format("using {0}", u.theUsing));

            return sb;
        }

        internal string ToUnderscoreCase(string str)
        {
            if (str.Contains("_"))
                return str;

            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) && char.IsLower(str[i - 1]) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }

    [Serializable]
    public class baseDataClass
    {
        protected string defaultDisplayName { get; set; } = "defaultDisplayName";

        virtual public string getDisplayName(string propertyName, out helperFunctions.RefreshLevel refresh)
        {
            refresh = helperFunctions.RefreshLevel.none;

            if (propertyName == "")
                return defaultDisplayName;

            PropertyInfo pi = this.GetType().GetProperty(propertyName);
            if (pi != null)
            {
                object value = pi.GetValue(this);
                return string.Format("{0} ({1})", propertyName, value.ToString());
            }

            return null;
        }

        public baseDataClass()
        {
            helperFunctions.initializeNullProperties(this, true);
            helperFunctions.initializeDefaultValues(this);
        }
    }

    public static class generatorContext
    {
        public static mechanism theMechanism { get; set; }
        public static mechanismInstance theMechanismInstance { get; set; }
        public static applicationData theRobot { get; set; }
        public static toolConfiguration theGeneratorConfig { get; set; }

        public static void clear()
        {
            theMechanism = null;
            theMechanismInstance = null;
            theRobot = null;
        }
    }

    public static class utilities
    {
        public static string ListToString(List<string> list, string delimeter)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = list[i].Trim();
                if (!string.IsNullOrWhiteSpace(list[i]))
                    sb.AppendLine(string.Format("{0}{1}", list[i], delimeter));
            }

            return sb.ToString().Trim();
        }
        public static string ListToString(List<string> list)
        {
            return ListToString(list, "");
        }
    }

    [Serializable()]
    public class controlData
    {
        public string name { get; set; }

        PIDFZ pid { get; set; }

        public double maxAccel { get; set; }

        public controlData()
        {
            name = GetType().Name;
        }
    }

    [Serializable()]
    public class actuatorTarget
    {
        public string name { get; set; }

        PIDFZ pid { get; set; }

        public double maxAccel { get; set; }

        public actuatorTarget()
        {
            name = GetType().Name;
        }
    }
    [Serializable()]
    public class state
    {
        public string name { get; set; }

        public List<controlData> controlData { get; set; }

        public state()
        {
            name = GetType().Name;
            controlData = new List<controlData>();
        }
    }

}

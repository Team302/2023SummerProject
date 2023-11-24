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
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

//todo handle optional elements such as followID in a motorcontroller
//todo the range of pdpID for ctre is 0-15, for REV it is 0-19. How to adjust the range allowed in the GUI. If initially REV is used and an id > 15 is used, then user chooses CTRE, what to do?
//todo make mechanism instances separate files so that it is easier for multiple people to work on the robot in parallel
//todo run a sanity check on a click of a button or on every change?
//todo in the treeview, place the "name" nodes at the top
//todo in the robot code check that an enum belonging to another robot is not used
//todo check naming convention

// =================================== Rules =====================================
// A property named __units__ will be converted to the list of physical units
// A property named value__ will not be shown in the tree directly. Its value is shown in the parent node
// Attributes are only allowed on the standard types (uint, int, double, bool) and on doubleParameter, unitParameter, intParameter, boolParameter
// The attribute PhysicalUnitsFamily can only be applied on doubleParameter, uintParameter, intParameter, boolParameter
// A class can only contain one List of a particular type

namespace ApplicationData
{
    [Serializable()]
    public class topLevelAppDataElement
    {
        public List<applicationData> Robots { get; set; }

        public List<mechanism> Mechanisms { get; set; }

        public topLevelAppDataElement()
        {
            Robots = new List<applicationData>();
            Mechanisms = new List<mechanism>();

            helperFunctions.initializeDefaultValues(this);
        }

        public string getDisplayName()
        {
            return "robotBuildDefinition";
        }
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
            return String.Format("mechanisms/{0}/decoratormods/{0}.h",name);
        }
    }

    [Serializable()]
    public partial class applicationData
    {
#if !enableTestAutomation
        public pdp PowerDistributionPanel { get; set; }
        public List<pcm> PneumaticControlModules { get; set; }
        public List<mechanismInstance> mechanismInstances { get; set; }

        /*        
                public List<pigeon> pigeon { get; set; }
                public List<limelight> limelight { get; set; }
                public chassis chassis { get; set; }
                public List<camera> camera { get; set; }
                public List<roborio> roborio { get; set; }
        */

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
#endif
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
        public List<closedLoopControlParameters> closedLoopControlParameters { get; set; }
        public List<solenoid> solenoid { get; set; }
        public List<servo> servo { get; set; }
        public List<analogInput> analogInput { get; set; }
        public List<digitalInput> digitalInput { get; set; }
        /*
        public colorsensor colorsensor { get; set; }
        public List<cancoder> cancoder { get; set; }
                public List<state> state { get; set; }
        */
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

#if !enableTestAutomation
    [Serializable()]
    public class closedLoopControlParameters
    {
        public string name { get; set; }

        [DefaultValue(0D)]
        [System.ComponentModel.Description("The proportional gain of the PID controller.")]
        [TunableParameter()]
        public doubleParameter pGain { get; set; }

        [DefaultValue(0D)]
        [System.ComponentModel.Description("The integral gain of the PID controller.")]
        [TunableParameter()]
        public doubleParameter iGain { get; set; }

        [DefaultValue(0D)]
        [System.ComponentModel.Description("The differential gain of the PID controller.")]
        [TunableParameter()]
        public doubleParameter dGain { get; set; }

        [DefaultValue(0D)]
        [System.ComponentModel.Description("The feed forward gain of the PID controller.")]
        [TunableParameter()]
        public doubleParameter fGain { get; set; }

        [DefaultValue(0D)]
        [TunableParameter()]
        public doubleParameter iZone { get; set; }

        public closedLoopControlParameters()
        {
            helperFunctions.initializeNullProperties(this);
            helperFunctions.initializeDefaultValues(this);

            name = GetType().Name;

        }

        public string getDisplayName()
        {
            return string.Format("{0}", name);
        }

        public string generateDefinition()
        {
            ImplementationNameAttribute impNameAttr = this.GetType().GetCustomAttribute<ImplementationNameAttribute>();
            if (impNameAttr != null)
                return string.Format("{0} {1};", impNameAttr.name, name);

            return string.Format("{0} {1};", this.GetType().Name, name);
        }
    }

    [Serializable()]
    public class pdp
    {
        public enum pdptype { CTRE, REV, }

        [DefaultValue(pdptype.REV)]
        public pdptype type { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "62")]
        public uintParameter canID { get; set; }

        public pdp()
        {
            helperFunctions.initializeNullProperties(this);
            helperFunctions.initializeDefaultValues(this);
        }

        public string getDisplayName(string propertyName, out helperFunctions.RefreshLevel refresh)
        {
            refresh = helperFunctions.RefreshLevel.parentHeader;
            if (string.IsNullOrEmpty(propertyName))
                refresh = helperFunctions.RefreshLevel.none;
            return "Power Distribution Panel (" + type.ToString() + ")";
        }
    }

    [Serializable()]
    public class pigeon
    {
        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "62")]
        public uintParameter canID { get; set; }

        [DefaultValue(CAN_BUS.rio)]
        public CAN_BUS canBusName { get; set; }

        [DefaultValue("0.0")]
        public string rotation { get; set; }

        [DefaultValue(pigeontype.pigeon1)]
        public pigeontype type { get; set; }

        [DefaultValue(pigeonname.CENTER_OF_ROTATION)]
        public pigeonname name { get; set; }

        public pigeon()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }

    [Serializable()]
    public class pcm
    {
        public string name { get; set; }

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
            helperFunctions.initializeNullProperties(this);
            helperFunctions.initializeDefaultValues(this);
        }

        public string getDisplayName(string propertyName, out helperFunctions.RefreshLevel refresh)
        {
            refresh = helperFunctions.RefreshLevel.parentHeader;
            if (string.IsNullOrEmpty(propertyName))
                refresh = helperFunctions.RefreshLevel.none;
            return string.Format("PCM ({0})", name);
        }
    }

    [Serializable()]
    [XmlInclude(typeof(TalonFX))]
    [XmlInclude(typeof(TalonSRX))]
    public class MotorController : baseRobotElementClass
    {
        [XmlIgnore]
        [Constant()]
        public string motorControllerType { get; protected set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "62")]
        public uintParameter canID { get; set; }

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

        public MotorController()
        {
            helperFunctions.initializeNullProperties(this, true);

            string temp = this.GetType().Name;
            int index = temp.LastIndexOf('_');

            motorControllerType = (index < 0) ? temp : temp.Substring(0, index);
            name = motorControllerType;

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

        override public List<string> generateObjectCreation()
        {
            string creation = string.Format("{0} = new {1}(\"{0}\",RobotElementNames::ROBOT_ELEMENT_NAMES::{2},{3},\"{4}\")",
                name,
                getImplementationName(),
                utilities.ListToString(generateElementNames()).ToUpper(),
                canID.value.ToString(),
                canBusName.ToString());

            return new List<string> { creation };
        }
    }

    [Serializable]
    public class baseRobotElementClass
    {
        [ConstantInMechInstance]
        public string name { get; set; }

        virtual public List<string> generateElementNames()
        {
            return new List<string> { string.Format("{0}_{1}", ToUnderscoreCase(generatorContext.theMechanism.name), ToUnderscoreCase(name)) };
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

        internal string ToUnderscoreCase(string str)
        {
            if (str.Contains("_"))
                return str;

            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
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
    }

    [Serializable()]
    [ImplementationName("DragonTalonFX")]
    [UserIncludeFile("hw/DragonTalonFX.h")]
    public class TalonFX : MotorController
    {
        public enum motorType { Falcon, Kraken }

        [DefaultValue(motorType.Falcon)]
        public motorType motor { get; set; }

        [Serializable]
        public class MotorConfigs : baseDataClass
        {
            public enum InvertedValue { CounterClockwise_Positive, Clockwise_Positive }
            public enum NeutralModeValue { Coast, Brake }

            [DefaultValue(0)]
            [Range(typeof(double), "0", "100")]
            [PhysicalUnitsFamily(physicalUnit.Family.percent)]
            [TunableParameter()]
            public doubleParameter deadbandPercent { get; set; }

            [DefaultValue(0)]
            [Range(typeof(double), "0", "30.0")]
            [PhysicalUnitsFamily(physicalUnit.Family.current)]
            [TunableParameter()]
            public doubleParameter peakMin { get; set; }

            [DefaultValue(0)]
            [Range(typeof(double), "0", "40.0")]
            [PhysicalUnitsFamily(physicalUnit.Family.current)]
            public doubleParameter peakMax { get; set; }

            [DefaultValue(InvertedValue.CounterClockwise_Positive)]
            public InvertedValue inverted { get; set; }

            [DefaultValue(NeutralModeValue.Coast)]
            public NeutralModeValue NeutralMode { get; set; }

            public MotorConfigs()
            {
                defaultDisplayName = "MotorConfigs";
            }
        }
        public MotorConfigs theMotorConfigs { get; set; }

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

        public TalonFX()
        {
        }

    }

    [Serializable()]
    [ImplementationName("DragonTalonSRX")]
    [UserIncludeFile("hw/DragonTalonSRX.h")]
    public class TalonSRX : MotorController
    {
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
    }




    #region enums
    [Serializable()]
    public enum CAN_BUS
    {
        rio,
    }





    [Serializable()]
    public enum pigeontype
    {
        pigeon1,
        pigeon2,
    }

    [Serializable()]
    public enum pigeonname
    {
        CENTER_OF_ROTATION,
    }

    [Serializable()]
    public enum limelightrotation
    {
        Angle_0_deg = 0,
        Angle_90_deg = 90,
        Angle_180_deg = 180,
        Angle_270_deg = 270,
    }


    [Serializable()]
    public enum limelightdefaultledmode
    {
        currentpipeline,
        off,
        blink,
        on,
    }


    [Serializable()]
    public enum limelightdefaultcammode
    {
        vision,
        drivercamera,
    }


    [Serializable()]
    public enum limelightstreammode
    {
        sidebyside,
        pipmain,
        pipsecondary,
    }


    [Serializable()]
    public enum limelightsnapshots
    {
        off,
        twopersec,
    }

    [Serializable()]
    public enum motormotorType
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
    public enum motorcontroller
    {

        TALONSRX,

        FALCON,

        BRUSHLESS_SPARK_MAX,

        BRUSHED_SPARK_MAX,
    }


    [Serializable()]
    public enum motorfeedbackDevice
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

    [Serializable()]
    public enum swervemoduletype
    {
        LEFT_FRONT,
        RIGHT_FRONT,
        LEFT_BACK,
        RIGHT_BACK,
    }


    [Serializable()]
    public enum chassistype
    {
        TANK,
        MECANUM,
        SWERVE,
    }


    [Serializable()]
    public enum chassiswheelSpeedCalcOption
    {
        WPI,
        ETHER,
        [XmlEnumAttribute("2910")]
        Item2910,
    }


    [Serializable()]
    public enum chassisposeEstimationOption
    {
        WPI,
        EULERCHASSIS,
        EULERWHEEL,
        POSECHASSIS,
        POSEWHEEL,
    }

    [Serializable()]
    public enum solenoidtype
    {
        CTREPCM,
        REVPH,
    }

    [Serializable()]
    public enum colorsensorport
    {
        kOnboard,
        kMXP,
    }

    [Serializable()]
    public enum cameraformat
    {
        KMJPEG,
        KYUYV,
        KRGB565,
        KBGR,
        KGRAY,
    }

    [Serializable()]
    public enum roborioorientation
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

    #endregion



    [Serializable()]
    public class controlData
    {
        public string name { get; set; }

        closedLoopControlParameters pid { get; set; }

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

        closedLoopControlParameters pid { get; set; }

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
            name = GetType().Name;
            helperFunctions.initializeNullProperties(this);
            helperFunctions.initializeDefaultValues(this);
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
    }




    [Serializable()]
    public class limelight
    {
        public string name { get; set; }

        [DefaultValue(0.0)]
        [TunableParameter()]
        public double mountingheight { get; set; }

        [DefaultValue(0.0)]
        [TunableParameter()]
        public double horizontaloffset { get; set; }

        [DefaultValue(0.0)]
        [TunableParameter()]
        public double mountingangle { get; set; }

        [DefaultValue(limelightrotation.Angle_0_deg)]
        [TunableParameter()]
        public limelightrotation rotation { get; set; }

        [DefaultValue(0.0)]
        [TunableParameter()]
        public double targetheight { get; set; }

        [DefaultValue(0.0)]
        [TunableParameter()]
        public double targetheight2 { get; set; }

        [DefaultValue(limelightdefaultledmode.currentpipeline)]
        [TunableParameter()]
        public limelightdefaultledmode defaultledmode { get; set; }

        [DefaultValue(limelightdefaultcammode.vision)]
        [TunableParameter()]
        public limelightdefaultcammode defaultcammode { get; set; }

        [DefaultValue(limelightstreammode.sidebyside)]
        [TunableParameter()]
        public limelightstreammode streammode { get; set; }

        [DefaultValue(limelightsnapshots.off)]
        [TunableParameter()]
        public limelightsnapshots snapshots { get; set; }

        [DefaultValue(0)]
        [TunableParameter()]
        public string crosshairx { get; set; }

        [DefaultValue(0)]
        [TunableParameter()]
        public string crosshairy { get; set; }

        [DefaultValue(0)]
        [TunableParameter()]
        public string secondcrosshairx { get; set; }

        [DefaultValue(0)]
        [TunableParameter()]
        public string secondcrosshairy { get; set; }

        public limelight()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }


    [Serializable()]
    public class chassis
    {
        public List<MotorController> motor { get; set; }

        public chassis()
        {
            motor = new List<MotorController>();
            swervemodule = new List<swervemodule>();

            helperFunctions.initializeDefaultValues(this);
        }

        public List<swervemodule> swervemodule { get; set; }

        [DefaultValue(ApplicationData.chassistype.TANK)]
        public chassistype type { get; set; }

        [TunableParameter()]
        public double wheelDiameter { get; set; }

        public double wheelBase { get; set; }

        public double track { get; set; }

        [DefaultValue(chassiswheelSpeedCalcOption.ETHER)]
        public chassiswheelSpeedCalcOption wheelSpeedCalcOption { get; set; }

        [DefaultValue(chassisposeEstimationOption.EULERCHASSIS)]
        public chassisposeEstimationOption poseEstimationOption { get; set; }

        public double maxVelocity { get; set; }

        public double maxAngularVelocity { get; set; }

        public double maxAcceleration { get; set; }

        public double maxAngularAcceleration { get; set; }
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
            name = GetType().Name;
            helperFunctions.initializeNullProperties(this);
            helperFunctions.initializeDefaultValues(this);
        }

        override public List<string> generateObjectCreation()
        {
            string creation = string.Format("{0} = new {1}(\"{0}\",RobotElementNames::ROBOT_ELEMENT_NAMES::{2},{3},{4},{5}({6}))",
                name,
                getImplementationName(),
                utilities.ListToString(generateElementNames()).ToUpper(),
                digitalId.value,
                reversed.value.ToString().ToLower(),
                generatorContext.theGeneratorConfig.getWPIphysicalUnitType(debouncetime.__units__),
                debouncetime.value
                );

            return new List<string> { creation };
        }
    }

    [Serializable()]
    public class swervemodule
    {
        public List<MotorController> motor { get; set; }

        public swervemodule()
        {
            motor = new List<MotorController>();

            helperFunctions.initializeDefaultValues(this);
        }

        public ctreCANcoder cancoder { get; set; }

        [DefaultValue(swervemoduletype.LEFT_FRONT)]
        public swervemoduletype type { get; set; }

        [DefaultValue(0.0)]
        [TunableParameter()]
        public double turn_p { get; set; }

        [DefaultValue(0.0)]
        [TunableParameter()]
        public double turn_i { get; set; }

        [DefaultValue(0.0)]
        [TunableParameter()]
        public double turn_d { get; set; }

        [DefaultValue(0.0)]
        [TunableParameter()]
        public double turn_f { get; set; }

        [DefaultValue(0.0)]
        [TunableParameter()]
        public double turn_nominal_val { get; set; }

        [DefaultValue(0.0)]
        [TunableParameter()]
        public double turn_peak_val { get; set; }

        [DefaultValue(0.0)]
        [TunableParameter()]
        public double turn_max_acc { get; set; }

        [DefaultValue(0.0)]
        [TunableParameter()]
        public double turn_cruise_vel { get; set; }

        [DefaultValue(1.0)]
        public uint countsOnTurnEncoderPerDegreesOnAngleSensor { get; set; }
    }


    [Serializable()]
    public class ctreCANcoder
    {
        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "62")]
        public uintParameter canID { get; set; }

        [DefaultValue(CAN_BUS.rio)]
        public CAN_BUS canBusName { get; set; }

        [DefaultValue(0D)]
        public double offset { get; set; }

        [DefaultValue(false)]
        public bool reverse { get; set; }

        public ctreCANcoder()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }


    [Serializable()]
    [ImplementationName("DragonSolenoid")]
    [UserIncludeFile("hw/DragonSolenoid.h")]
    public class solenoid : baseRobotElementClass
    {
        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "62")]
        public uintParameter CAN_ID { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "7")]
        public uintParameter channel { get; set; }

        [DefaultValue(false)]
        public boolParameter reversed { get; set; }

        [DefaultValue(solenoidtype.REVPH)]
        public solenoidtype type { get; set; }

        public solenoid()
        {
            name = this.GetType().Name;

            helperFunctions.initializeNullProperties(this);
            helperFunctions.initializeDefaultValues(this);
        }

        override public List<string> generateObjectCreation()
        {
            string creation = string.Format("{0} = new {1}(\"{0}\",RobotElementNames::ROBOT_ELEMENT_NAMES::{2},{3},frc::PneumaticsModuleType::{4},{5},{6})",
                name,
                getImplementationName(),
                utilities.ListToString(generateElementNames()).ToUpper(),
                CAN_ID.value,
                type,
                channel.value,
                reversed.value.ToString().ToLower()
                );

            return new List<string> { creation };
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
            name = GetType().Name;
            helperFunctions.initializeNullProperties(this);
            helperFunctions.initializeDefaultValues(this);
        }

        override public List<string> generateObjectCreation()
        {
            string creation = string.Format("{0} = new {1}(RobotElementNames::ROBOT_ELEMENT_NAMES::{2},{3},{4}({5}),{6}({7}))",
                name,
                getImplementationName(),
                utilities.ListToString(generateElementNames()).ToUpper(),
                Id.value,
                generatorContext.theGeneratorConfig.getWPIphysicalUnitType(minAngle.__units__),
                minAngle.value,
                generatorContext.theGeneratorConfig.getWPIphysicalUnitType(maxAngle.__units__),
                maxAngle.value
                );
            
            return new List<string> { creation };
        }
    }


    [Serializable()]
    [ImplementationName("DragonColorSensor")]
    [UserIncludeFile("hw/DragonColorSensor.h")]
    public class colorsensor : baseRobotElementClass
    {
        [DefaultValue(colorsensorport.kOnboard)]
        public colorsensorport port { get; set; }

        public colorsensor()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }



    [Serializable()]
    public class camera
    {
        [DefaultValue("0")]
        public string id { get; set; }

        [DefaultValue(ApplicationData.cameraformat.KMJPEG)]
        public cameraformat format { get; set; }

        [DefaultValue(640u)]
        public uint width { get; set; }

        [DefaultValue(480)]
        public string height { get; set; }

        [DefaultValue(30)]
        public uint fps { get; set; }

        [DefaultValue(false)]
        public bool thread { get; set; }

        public camera()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }

    [Serializable()]
    public class roborio
    {
        [DefaultValue(ApplicationData.roborioorientation.X_FORWARD_Y_LEFT)]
        [TunableParameter()]
        public roborioorientation orientation { get; set; }

        public roborio()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }




    [Serializable()]
    public class pwmultrasonic
    {
        [TunableParameter()]
        public string name { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "19")]
        public uint Id { get; set; }

        public pwmultrasonic()
        {
            name = GetType().Name;
            helperFunctions.initializeDefaultValues(this);
        }
    }

    [Serializable()]
    public class analogultrasonic
    {
        [TunableParameter()]
        public string name { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "7")]
        public uint id { get; set; }

        public analogultrasonic()
        {
            name = GetType().Name;
            helperFunctions.initializeDefaultValues(this);
        }
    }

    [Serializable()]
    public class lidar
    {
        [TunableParameter()]
        public string name { get; set; }

        [DefaultValue(0u)]
        [TunableParameter()]
        public uint inputpin { get; set; }

        [DefaultValue(0)]
        [TunableParameter()]
        public uint triggerpin { get; set; }

        public lidar()
        {
            name = GetType().Name;
            helperFunctions.initializeDefaultValues(this);
        }
    }

    [Serializable()]
    public class led
    {
        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "19")]
        public uint Id { get; set; }

        [DefaultValue(0u)]
        public uint number { get; set; }

        public led()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }


    [Serializable()]
    [XmlTypeAttribute("blinkin", Namespace = "http://team302.org/robot")]


    [XmlRootAttribute("blinkin", Namespace = "http://team302.org/robot")]
    public class blinkin
    {
        public string name { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "19")]
        public uint Id { get; set; }

        public blinkin()
        {
            name = GetType().Name;
            helperFunctions.initializeDefaultValues(this);
        }
    }



    [Serializable()]
    public class talontach
    {
        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "62")]
        public uintParameter CAN_ID { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "6")]
        public uint name { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "11")]
        public uint generalpin { get; set; }

        public talontach()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }
#endif

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
}

using Configuration;
using DataConfiguration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

// =================================== Rules =====================================
// A property named __units__ will be converted to the list of physical units
// A property named value__ will not be shown in the tree directly. Its value is shown in the parent node
// Attributes are only allowed on the standard types (uint, int, double, bool) and on doubleParameter, unitParameter, intParameter, boolParameter
// The attribute PhysicalUnitsFamily can only be applied on doubleParameter, uintParameter, intParameter, boolParameter
// A class can only contain one List of a particular type

namespace ApplicationData
{
    #region enums
    [Serializable()]
    public enum CAN_BUS
    {
        rio,
    }

    [Serializable()]
    public enum pdptype
    {
        CTRE,
        REV,
    }

    [Serializable()]
    public enum analogInputType
    {
        PRESSURE_GAUGE,
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
    [XmlInclude(typeof(Falcon_Motor))]
    [XmlInclude(typeof(TalonSRX_Motor))]
    public class motor
    {
        [XmlIgnore]
        [Constant()]
        public stringParameter motorType { get; protected set; }

        public stringParameter name { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "62")]
        public uintParameter CAN_ID { get; set; }

        public motor()
        {
            motorType = new stringParameter();
            name = new stringParameter();
            CAN_ID = new uintParameter();
            string temp = this.GetType().Name;
            motorType.value__ = temp.Substring(0, temp.LastIndexOf('_'));
            name.value__ = motorType.value__;

            helperFunctions.initializeDefaultValues(this);
        }
        public string getDisplayName()
        {
            return name.value__;
        }
    }

    [Serializable()]
    public class Falcon_Motor : motor
    {
        [DefaultValue(1.15)]
        [Range(typeof(double), "0", "62")]
        [PhysicalUnitsFamily(physicalUnit.Family.percent)]
        [TunableParameter()]
        public doubleParameter deadbandPercent { get; set; }

        [DefaultValue(5.55)]
        [Range(typeof(double), "0", "100")]
        [PhysicalUnitsFamily(physicalUnit.Family.percent)]
        [TunableParameter()]
        public doubleParameter deadband { get; set; }

        [DefaultValue(2.2)]
        [Range(typeof(double), "-1.0", "3.0")]
        [PhysicalUnitsFamily(physicalUnit.Family.current)]
        [TunableParameter()]
        public doubleParameter peakMin { get; set; }

        [DefaultValue(4.4)]
        [Range(typeof(double), "-10.0", "20.0")]
        [PhysicalUnitsFamily(physicalUnit.Family.current)]
        public doubleParameter peakMax { get; set; }

        public Falcon_Motor()
        {
        }
    }

    [Serializable()]
    public class TalonSRX_Motor : motor
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

        public TalonSRX_Motor()
        {
        }
    }


    [Serializable()]
    public partial class CAN_ID
    {
        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "62")]
        public uint value__ { get; set; }

        public CAN_ID()
        {
            helperFunctions.initializeDefaultValues(this);
        }

        public string getDisplayName(string instanceName, out helperFunctions.RefreshLevel refresh)
        {
            refresh = helperFunctions.RefreshLevel.parentHeader;

            return "CAN_ID (" + value__ + ")";
        }
    }

    [Serializable()]
    public partial class applicationData
    {
        public testClass testClass { get; set; }
        public List<parameter> parameter { get; set; }
        public List<motor> motor { get; set; }
        public List<pcm> pcm { get; set; }
        public pdp pdp { get; set; }
        public List<pigeon> pigeon { get; set; }
        public List<limelight> limelight { get; set; }
        public chassis chassis { get; set; }
        public List<mechanismInstance> mechanismInstance { get; set; }
        public List<camera> camera { get; set; }
        public List<roborio> roborio { get; set; }

        [DefaultValueAttribute(1u)]
        [RangeAttribute(typeof(uint), "1", "9999")]
        public uint robotID { get; set; }

        public applicationData()
        {
            testClass = new testClass();
            parameter = new List<parameter>();
            motor = new List<motor>();
            pcm = new List<pcm>();
            pigeon = new List<pigeon>();
            limelight = new List<limelight>();
            mechanismInstance = new List<mechanismInstance>();
            camera = new List<camera>();
            roborio = new List<roborio>();

            helperFunctions.initializeDefaultValues(this);
        }

        public string getDisplayName(string propertyName, out helperFunctions.RefreshLevel refresh)
        {
            refresh = helperFunctions.RefreshLevel.none;

            if (string.IsNullOrEmpty(propertyName))
                return string.Format("Robot #{0}", robotID);
            else if (propertyName == "testClass")
                return string.Format("{0} ({1}))", propertyName, testClass.name);
            else if (propertyName == "pdp")
                return string.Format("{0} ({1})", propertyName, pdp.type);

            return "robot class - incomplete getDisplayName";
        }
    }

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
    public partial class pdp
    {
        [DefaultValue(pdptype.CTRE)]
        public pdptype type { get; set; }

        public pdp()
        {
            helperFunctions.initializeDefaultValues(this);
        }

        public string getDisplayName()
        {
            return "Pdp (" + type.ToString() + ")";
        }
    }

    [Serializable()]
    public partial class pcm
    {
        public analogInput analogInput { get; set; }
        public CAN_ID canId { get; set; }

        public pcm()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }

    [Serializable()]
    public partial class analogInput
    {
        [DefaultValue(analogInputType.PRESSURE_GAUGE)]
        [TunableParameter()]
        public analogInputType type { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "7")]
        public uint analogId { get; set; }

        [DefaultValue(0D)]
        public double voltageMin { get; set; }

        [DefaultValue(5D)]
        public double voltageMax { get; set; }

        public double outputMin { get; set; }

        public double outputMax { get; set; }

        public analogInput()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }


    [Serializable()]
    public partial class pigeon
    {
        public CAN_ID canId { get; set; }

        [DefaultValue(CAN_BUS.rio)]
        [TunableParameter()]
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
    public partial class limelight
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
    public partial class chassis
    {
        public List<motor> motor { get; set; }

        public chassis()
        {
            motor = new List<motor>();
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
    public partial class digitalInput
    {
        public string name { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "25")]
        public uint digitalId { get; set; }

        [DefaultValue(false)]
        public bool reversed { get; set; }

        [DefaultValue(0D)]
        public double debouncetime { get; set; }

        public digitalInput()
        {
            name = GetType().Name;
            helperFunctions.initializeDefaultValues(this);
        }
    }

    [Serializable()]
    public partial class swervemodule
    {
        public List<motor> motor { get; set; }

        public swervemodule()
        {
            motor = new List<motor>();

            helperFunctions.initializeDefaultValues(this);
        }

        public cancoder cancoder { get; set; }

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
    public partial class cancoder
    {
        public CAN_ID canId { get; set; }

        [DefaultValue(CAN_BUS.rio)]
        public CAN_BUS canBusName { get; set; }

        [DefaultValue(0D)]
        public double offset { get; set; }

        [DefaultValue(false)]
        public bool reverse { get; set; }

        public cancoder()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }

    [Serializable()]
    public partial class mechanismInstance
    {
        [XmlIgnore]
        public object theTreeNode = null;

        public string name { get; set; }

        public mechanism mechanism { get; set; }

        public mechanismInstance()
        {
            name = "mechanismInstanceName";

            helperFunctions.initializeDefaultValues(this);
        }
    }


    [Serializable()]
    public partial class mechanism
    {
        [XmlIgnore]
        public object theTreeNode = null;

        public override string ToString()
        {
            return name;
        }

        public List<state> state { get; set; }
        public List<motor> motor { get; set; }
        public List<solenoid> solenoid { get; set; }
        public List<servo> servo { get; set; }
        public List<analogInput> analogInput { get; set; }
        public List<digitalInput> digitalInput { get; set; }
        public List<cancoder> cancoder { get; set; }
        public colorsensor colorsensor { get; set; }
        public List<closedLoopControlParameters> closedLoopControlParameters { get; set; }
        public string name { get; set; }

        public mechanism()
        {
            name = GetType().Name;

            state = new List<state>();
            motor = new List<motor>();
            solenoid = new List<solenoid>();
            servo = new List<servo>();
            analogInput = new List<analogInput>();
            digitalInput = new List<digitalInput>();
            cancoder = new List<cancoder>();
            closedLoopControlParameters = new List<closedLoopControlParameters>();

            helperFunctions.initializeDefaultValues(this);
        }
    }


    [Serializable()]
    public partial class solenoid
    {
        public CAN_ID canId { get; set; }

        [TunableParameter()]
        public string name { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "7")]
        public uint channel { get; set; }

        [DefaultValue(false)]
        public bool reversed { get; set; }

        [DefaultValue(solenoidtype.REVPH)]
        public solenoidtype type { get; set; }

        public solenoid()
        {
            name = this.GetType().Name;

            helperFunctions.initializeDefaultValues(this);
        }
    }





    [Serializable()]
    public partial class servo
    {
        public string name { get; set; }


        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "19")]
        public uint Id { get; set; }

        [DefaultValue("0.0")]
        public string minAngle { get; set; }

        [DefaultValue("360.0")]
        public string maxAngle { get; set; }

        public servo()
        {
            name = GetType().Name;
            helperFunctions.initializeDefaultValues(this);
        }
    }


    [Serializable()]
    public partial class colorsensor
    {
        [DefaultValue(colorsensorport.kOnboard)]
        public colorsensorport port { get; set; }

        public colorsensor()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }

    [Serializable()]
    public partial class closedLoopControlParameters
    {
        public string name { get; set; }

        [DefaultValue(0D)]
        [System.ComponentModel.Description("The proportional gain of the PID controller.")]
        [TunableParameter()]
        public double pGain { get; set; }

        [DefaultValue(0D)]
        [System.ComponentModel.Description("The integral gain of the PID controller.")]
        [TunableParameter()]
        public double iGain { get; set; }

        [DefaultValue(0D)]
        [System.ComponentModel.Description("The differential gain of the PID controller.")]
        [TunableParameter()]
        public double dGain { get; set; }

        [DefaultValue(0D)]
        [System.ComponentModel.Description("The feed forward gain of the PID controller.")]
        [TunableParameter()]
        public double fGain { get; set; }

        [DefaultValue(0D)]
        [TunableParameter()]
        public double iZone { get; set; }

        public closedLoopControlParameters()
        {
            name = GetType().Name;

            helperFunctions.initializeDefaultValues(this);
        }
    }


    [Serializable()]
    public partial class camera
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
    public partial class roborio
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
    public partial class topLevelAppDataElement
    {
        public List<applicationData> robot { get; set; }

        public List<mechanism> mechanism { get; set; }

        public topLevelAppDataElement()
        {
            robot = new List<applicationData>();
            mechanism = new List<mechanism>();

            helperFunctions.initializeDefaultValues(this);
        }

        public string getDisplayName()
        {
            return "robotVariants";
        }
    }


    [Serializable()]
    public partial class pwmultrasonic
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
    public partial class analogultrasonic
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
    public partial class lidar
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
    public partial class led
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
    public partial class blinkin
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
    public partial class talontach
    {
        public CAN_ID canId { get; set; }

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

    [Serializable()]
    public partial class testClass
    {
        public stringParameter name { get; set; }

        [DefaultValue(-4)]
        [Range(typeof(double), "-10", "10")]
        [PhysicalUnitsFamily(physicalUnit.Family.length)]
        [TunableParameter()]
        public doubleParameter aDouble { get; set; }

        [PhysicalUnitsFamily(physicalUnit.Family.mass)]
        [TunableParameter()]
        public doubleParameter anotherDouble { get; set; }

        public List<doubleParameterUserDefinedNonTunable> aListOfDoubles { get; set; }

        [TunableParameter()]
        public List<doubleParameterUserDefinedTunable> aListOfTunableDoubles { get; set; }


        [DefaultValue(-40)]
        [Range(typeof(int), "-100", "10")]
        [PhysicalUnitsFamily(physicalUnit.Family.mass)]
        [TunableParameter()]
        public intParameter anInt { get; set; }

        [PhysicalUnitsFamily(physicalUnit.Family.voltage)]
        [TunableParameter()]
        public intParameter anotherInt { get; set; }

        public List<intParameterUserDefinedNonTunable> aListOfInts { get; set; }

        [TunableParameter()]
        public List<intParameterUserDefinedTunable> aListOfTunableInts { get; set; }

        [DefaultValue(40)]
        [Range(typeof(int), "0", "43")]
        [PhysicalUnitsFamily(physicalUnit.Family.mass)]
        [TunableParameter()]
        public uintParameter aUint { get; set; }

        [PhysicalUnitsFamily(physicalUnit.Family.current)]
        [TunableParameter()]
        public uintParameter anotherUint { get; set; }

        public List<uintParameterUserDefinedNonTunable> aListOfUints { get; set; }

        public List<uintParameterUserDefinedTunable> aListOfTunableUints { get; set; }

        public boolParameter aBool { get; set; }

        [TunableParameter]
        public boolParameter aTunableBool { get; set; }

        public List<boolParameterUserDefinedNonTunable> aListOfNonTunableBools { get; set; }
        public List<boolParameterUserDefinedTunable> aListOfTunableBools { get; set; }


        public testClass()
        {
            name = new stringParameter();
            aDouble = new doubleParameter();
            anotherDouble = new doubleParameter();
            aListOfDoubles = new List<doubleParameterUserDefinedNonTunable>();
            aListOfTunableDoubles = new List<doubleParameterUserDefinedTunable>();

            anInt = new intParameter();
            anotherInt = new intParameter();
            aListOfInts = new List<intParameterUserDefinedNonTunable>();
            aListOfTunableInts = new List<intParameterUserDefinedTunable>();

            aUint = new uintParameter();
            anotherUint = new uintParameter();
            aListOfUints = new List<uintParameterUserDefinedNonTunable>();
            aListOfTunableUints = new List<uintParameterUserDefinedTunable>();

            aBool = new boolParameter();
            aTunableBool = new boolParameter();
            aListOfNonTunableBools = new List<boolParameterUserDefinedNonTunable>();
            aListOfTunableBools = new List<boolParameterUserDefinedTunable>();

            name.value__ = this.GetType().Name;
            helperFunctions.initializeDefaultValues(this);
        }

        public string getDisplayName(string propertyName, out helperFunctions.RefreshLevel refresh)
        {
            refresh = helperFunctions.RefreshLevel.none;

            if (string.IsNullOrEmpty(propertyName))
            {
                return name.value__;
            }
            else if (propertyName == "aDouble")
            {
                return aDouble.getDisplayName(propertyName, out refresh);
            }
            else
            {
                return propertyName;
            }
        }
    }

}
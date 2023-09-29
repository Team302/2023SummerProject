using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using System.Reflection;


namespace Robot
{
    #region enums
    [Serializable()]
    [XmlType("CAN_BUS", Namespace = "http://team302.org/robot")]
    public enum CAN_BUS
    {
        rio,
    }

    [Serializable()]
    [XmlType("pdptype", Namespace = "http://team302.org/robot")]
    public enum pdptype
    {
        CTRE,
        REV,
    }

    [Serializable()]
    [XmlType("analogInputtype", Namespace = "http://team302.org/robot")]
    public enum analogInputType
    {
        PRESSURE_GAUGE,
    }

    [Serializable()]
    [XmlType("pigeontype", Namespace = "http://team302.org/robot")]
    public enum pigeontype
    {
        pigeon1,
        pigeon2,
    }

    [Serializable()]
    [XmlType("pigeonname", Namespace = "http://team302.org/robot")]
    public enum pigeonname
    {
        CENTER_OF_ROTATION,
    }

    [Serializable()]
    [XmlType("limelightrotation", Namespace = "http://team302.org/robot")]
    public enum limelightrotation
    {
        Angle_0_deg = 0,
        Angle_90_deg = 90,
        Angle_180_deg = 180,
        Angle_270_deg = 270,
    }


    [Serializable()]
    [XmlType("limelightdefaultledmode", Namespace = "http://team302.org/robot")]
    public enum limelightdefaultledmode
    {
        currentpipeline,
        off,
        blink,
        on,
    }


    [Serializable()]
    [XmlType("limelightdefaultcammode", Namespace = "http://team302.org/robot")]
    public enum limelightdefaultcammode
    {
        vision,
        drivercamera,
    }


    [Serializable()]
    [XmlType("limelightstreammode", Namespace = "http://team302.org/robot")]
    public enum limelightstreammode
    {
        sidebyside,
        pipmain,
        pipsecondary,
    }


    [Serializable()]
    [XmlType("limelightsnapshots", Namespace = "http://team302.org/robot")]
    public enum limelightsnapshots
    {
        off,
        twopersec,
    }

    [Serializable()]
    [XmlTypeAttribute("motormotorType", Namespace = "http://team302.org/robot")]
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
    [XmlTypeAttribute("motorcontroller", Namespace = "http://team302.org/robot")]
    public enum motorcontroller
    {

        TALONSRX,

        FALCON,

        BRUSHLESS_SPARK_MAX,

        BRUSHED_SPARK_MAX,
    }


    [Serializable()]
    [XmlTypeAttribute("motorfeedbackDevice", Namespace = "http://team302.org/robot")]
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
    [XmlTypeAttribute("motorcurrentLimiting", Namespace = "http://team302.org/robot")]
    public enum motorcurrentLimiting
    {

        [XmlEnumAttribute("true")]
        Item_true,

        [XmlEnumAttribute("false")]
        Item_false,
    }


    [Serializable()]
    [XmlTypeAttribute("motorforwardlimitswitch", Namespace = "http://team302.org/robot")]
    public enum motorforwardlimitswitch
    {

        [XmlEnumAttribute("true")]
        Item_true,

        [XmlEnumAttribute("false")]
        Item_false,
    }


    [Serializable()]
    [XmlTypeAttribute("motorforwardlimitswitchopen", Namespace = "http://team302.org/robot")]
    public enum motorforwardlimitswitchopen
    {

        [XmlEnumAttribute("true")]
        Item_true,

        [XmlEnumAttribute("false")]
        Item_false,
    }


    [Serializable()]
    [XmlTypeAttribute("motorreverselimitswitch", Namespace = "http://team302.org/robot")]
    public enum motorreverselimitswitch
    {

        [XmlEnumAttribute("true")]
        Item_true,

        [XmlEnumAttribute("false")]
        Item_false,
    }


    [Serializable()]
    [XmlTypeAttribute("motorreverselimitswitchopen", Namespace = "http://team302.org/robot")]
    public enum motorreverselimitswitchopen
    {

        [XmlEnumAttribute("true")]
        Item_true,

        [XmlEnumAttribute("false")]
        Item_false,
    }


    [Serializable()]
    [XmlTypeAttribute("motorvoltageCompensationEnable", Namespace = "http://team302.org/robot")]
    public enum motorvoltageCompensationEnable
    {

        [XmlEnumAttribute("true")]
        Item_true,

        [XmlEnumAttribute("false")]
        Item_false,
    }

    [Serializable()]
    [XmlType("swervemoduletype", Namespace = "http://team302.org/robot")]
    public enum swervemoduletype
    {
        LEFT_FRONT,
        RIGHT_FRONT,
        LEFT_BACK,
        RIGHT_BACK,
    }


    [Serializable()]
    [XmlType("chassistype", Namespace = "http://team302.org/robot")]
    public enum chassistype
    {
        TANK,
        MECANUM,
        SWERVE,
    }


    [Serializable()]
    [XmlType("chassiswheelSpeedCalcOption", Namespace = "http://team302.org/robot")]
    public enum chassiswheelSpeedCalcOption
    {
        WPI,
        ETHER,
        [XmlEnumAttribute("2910")]
        Item2910,
    }


    [Serializable()]
    [XmlTypeAttribute("chassisposeEstimationOption", Namespace = "http://team302.org/robot")]
    public enum chassisposeEstimationOption
    {
        WPI,
        EULERCHASSIS,
        EULERWHEEL,
        POSECHASSIS,
        POSEWHEEL,
    }

    [Serializable()]
    [XmlTypeAttribute("solenoidreversed", Namespace = "http://team302.org/robot")]
    public enum solenoidreversed
    {

        [XmlEnumAttribute("true")]
        Item_true,

        [XmlEnumAttribute("false")]
        Item_false,
    }


    [Serializable()]
    [XmlType("solenoidtype", Namespace = "http://team302.org/robot")]
    public enum solenoidtype
    {
        CTREPCM,
        REVPH,
    }

    [Serializable()]
    [XmlType("colorsensorport", Namespace = "http://team302.org/robot")]
    public enum colorsensorport
    {
        kOnboard,
        kMXP,
    }

    [Serializable()]
    [XmlType("cameraformat", Namespace = "http://team302.org/robot")]
    public enum cameraformat
    {
        KMJPEG,
        KYUYV,
        KRGB565,
        KBGR,
        KGRAY,
    }


    [Serializable()]
    [XmlTypeAttribute("camerathread", Namespace = "http://team302.org/robot")]
    public enum camerathread
    {

        [XmlEnumAttribute("true")]
        Item_true,

        [XmlEnumAttribute("false")]
        Item_false,
    }

    [Serializable()]
    [XmlType("roborioorientation", Namespace = "http://team302.org/robot")]
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

    [Serializable()]
    [XmlType("pwmultrasonicname", Namespace = "http://team302.org/robot")]
    public enum pwmultrasonicname
    {
        front,
        back,
    }

    [Serializable()]
    [XmlType("analogultrasonicname", Namespace = "http://team302.org/robot")]
    public enum analogultrasonicname
    {
        front,
        back,
    }

    [Serializable()]
    [XmlType("lidarname", Namespace = "http://team302.org/robot")]
    public enum lidarname
    {
        front,
        back,
    }

    [Serializable()]
    [XmlType("blinkinname", Namespace = "http://team302.org/robot")]
    public enum blinkinname
    {
        front,
        back,
        top,
        bottom,
    }

    #endregion

    [Serializable()]
    [XmlInclude(typeof(Falcon_Motor))]
    [XmlInclude(typeof(TalonSRX_Motor))]
    public class motor
    {
        [XmlIgnore]
        [robotConstant()]
        public string motorType { get; protected set; }

        public string name { get; set; }

        public CAN_ID CAN_ID { get; set; }

        public motor()
        {
            CAN_ID = new CAN_ID();
            motorType = this.GetType().Name;
            motorType = motorType.Substring(0, motorType.LastIndexOf('_'));
            name = motorType;

            helperFunctions.initializeDefaultValues(this);
        }
    }

    [Serializable()]
    public class Falcon_Motor : motor
    {
        [DefaultValue(1.15)]
        [Range(typeof(double), "0", "62")]
        [robotParameter(true)]
        public double deadbandPercent { get; set; }

        [DefaultValue(2.2)]
        [Range(typeof(double), "-1.0", "3.0")]
        [robotParameter(false)]
        public double peakMin { get; set; }

        [DefaultValue(4.4)]
        [Range(typeof(double), "-10.0", "20.0")]
        public double peakMax { get; set; }

        public Falcon_Motor()
        {
        }
    }

    [Serializable()]
    public class TalonSRX_Motor : motor
    {
        [DefaultValue(1.1)]
        [Range(typeof(double), "0", "62")]
        [robotParameter(true)]
        public double deadbandPercent_ { get; set; }

        [DefaultValue(2.2)]
        [Range(typeof(double), "-1.0", "3.0")]
        public double peakMin_ { get; set; }

        [DefaultValue(4.4)]
        [Range(typeof(double), "-10.0", "20.0")]
        public double peakMax_ { get; set; }

        public TalonSRX_Motor()
        {
        }
    }


    [Serializable()]
    public partial class CAN_ID
    {
        [robotParameter(false)]
        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "62")]
        public uint value { get; set; }

        public CAN_ID()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }

    [Serializable()]
    public partial class robot
    {
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

        public robot()
        {
            motor = new List<motor>();
            pcm = new List<pcm>();
            pigeon = new List<pigeon>();
            limelight = new List<limelight>();
            mechanismInstance = new List<mechanismInstance>();
            camera = new List<camera>();
            roborio = new List<roborio>();

            helperFunctions.initializeDefaultValues(this);
        }
    }


    [Serializable()]
    public partial class pdp
    {
        [DefaultValue(pdptype.CTRE)]
        [robotParameter(false)]
        public pdptype type { get; set; }

        public pdp()
        {
            helperFunctions.initializeDefaultValues(this);
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
        [robotParameter(false)]
        public analogInputType type { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "7")]
        [robotParameter(false)]
        public uint analogId { get; set; }

        [DefaultValue(0D)]
        [robotParameter(false)]
        public double voltageMin { get; set; }

        [DefaultValue(5D)]
        [robotParameter(false)]
        public double voltageMax { get; set; }

        [robotParameter(false)]
        public double outputMin { get; set; }

        [robotParameter(false)]
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
        [robotParameter(false)]
        public CAN_BUS canBusName { get; set; }

        [DefaultValue("0.0")]
        [robotParameter(false)]
        public string rotation { get; set; }

        [DefaultValue(pigeontype.pigeon1)]
        [robotParameter(false)]
        public pigeontype type { get; set; }

        [DefaultValue(pigeonname.CENTER_OF_ROTATION)]
        [robotParameter(false)]
        public pigeonname name { get; set; }

        public pigeon()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }

    [Serializable()]
    public partial class limelight
    {
        [XmlAttributeAttribute("name")]
        [robotParameter(false)]
        public string name { get; set; }

        [DefaultValue(0.0)]
        [robotParameter(true)]
        public double mountingheight { get; set; }

        [DefaultValue(0.0)]
        [robotParameter(true)]
        public double horizontaloffset { get; set; }

        [DefaultValue(0.0)]
        [robotParameter(true)]
        public double mountingangle { get; set; }

        [DefaultValue(limelightrotation.Angle_0_deg)]
        [robotParameter(true)]
        public limelightrotation rotation { get; set; }

        [DefaultValue(0.0)]
        [robotParameter(true)]
        public double targetheight { get; set; }

        [DefaultValue(0.0)]
        [robotParameter(true)]
        public double targetheight2 { get; set; }

        [DefaultValue(limelightdefaultledmode.currentpipeline)]
        [robotParameter(false)]
        public limelightdefaultledmode defaultledmode { get; set; }

        [DefaultValue(limelightdefaultcammode.vision)]
        [robotParameter(false)]
        public limelightdefaultcammode defaultcammode { get; set; }

        [DefaultValue(limelightstreammode.sidebyside)]
        [robotParameter(false)]
        public limelightstreammode streammode { get; set; }

        [DefaultValue(limelightsnapshots.off)]
        [robotParameter(false)]
        public limelightsnapshots snapshots { get; set; }

        [DefaultValue(0)]
        [robotParameter(false)]
        public string crosshairx { get; set; }

        [DefaultValue(0)]
        [robotParameter(false)]
        public string crosshairy { get; set; }

        [DefaultValue(0)]
        [robotParameter(false)]
        public string secondcrosshairx { get; set; }

        [DefaultValue(0)]
        [robotParameter(false)]
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

        [DefaultValue(Robot.chassistype.TANK)]
        public chassistype type { get; set; }

        [robotParameter(false)]
        public double wheelDiameter { get; set; }

        [robotParameter(false)]
        public double wheelBase { get; set; }

        [robotParameter(false)]
        public double track { get; set; }

        [DefaultValue(chassiswheelSpeedCalcOption.ETHER)]
        [robotParameter(false)]
        public chassiswheelSpeedCalcOption wheelSpeedCalcOption { get; set; }

        [DefaultValue(chassisposeEstimationOption.EULERCHASSIS)]
        [robotParameter(false)]
        public chassisposeEstimationOption poseEstimationOption { get; set; }

        [robotParameter(false)]
        public double maxVelocity { get; set; }

        [robotParameter(false)]
        public double maxAngularVelocity { get; set; }

        [robotParameter(false)]
        public double maxAcceleration { get; set; }

        [robotParameter(false)]
        public double maxAngularAcceleration { get; set; }
    }

    [Serializable()]
    public partial class digitalInput
    {
        [DefaultValue("UNKNOWN")]
        [robotParameter(false)]
        public string name { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "25")]
        public uint digitalId { get; set; }

        [DefaultValue(false)]
        [robotParameter(false)]
        public bool reversed { get; set; }

        [DefaultValue(0D)]
        [robotParameter(false)]
        public double debouncetime { get; set; }

        public digitalInput()
        {
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
        [robotParameter(false)]
        public swervemoduletype type { get; set; }

        [DefaultValue(0.0)]
        [robotParameter(true)]
        public double turn_p { get; set; }

        [DefaultValue(0.0)]
        [robotParameter(true)]
        public double turn_i { get; set; }

        [DefaultValue(0.0)]
        [robotParameter(true)]
        public double turn_d { get; set; }

        [DefaultValue(0.0)]
        [robotParameter(true)]
        public double turn_f { get; set; }

        [DefaultValue(0.0)]
        [robotParameter(true)]
        public double turn_nominal_val { get; set; }

        [DefaultValue(0.0)]
        [robotParameter(true)]
        public double turn_peak_val { get; set; }

        [DefaultValue(0.0)]
        [robotParameter(true)]
        public double turn_max_acc { get; set; }

        [DefaultValue(0.0)]
        [robotParameter(true)]
        public double turn_cruise_vel { get; set; }

        [DefaultValue(1.0)]
        [robotParameter(true)]
        public uint countsOnTurnEncoderPerDegreesOnAngleSensor { get; set; }
    }


    [Serializable()]
    public partial class cancoder
    {
        public CAN_ID canId { get; set; }

        [DefaultValue(CAN_BUS.rio)]
        public CAN_BUS canBusName { get; set; }

        [DefaultValue(0D)]
        [robotParameter(false)]
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

        [robotParameter(false)]
        public string name { get; set; }

        // [DefaultValue(0u)]
        [Range(typeof(uint), "0", "7")]
        public uint channel { get; set; }

        [DefaultValue(false)]
        [robotParameter(false)]
        public bool reversed { get; set; }

        [DefaultValue(solenoidtype.REVPH)]
        [robotParameter(false)]
        public solenoidtype type { get; set; }

        public solenoid()
        {
            name = this.GetType().Name;

            helperFunctions.initializeDefaultValues(this);
        }
    }





    [Serializable()]
    [XmlTypeAttribute("servo", Namespace = "http://team302.org/robot")]


    [XmlRootAttribute("servo", Namespace = "http://team302.org/robot")]
    public partial class servo
    {

        private string _name = "UNKNOWN";

        [System.ComponentModel.DefaultValueAttribute("UNKNOWN")]
        [XmlAttributeAttribute("name")]
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private uint _pwmId = 0u;

        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 19.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "19")]
        [XmlAttributeAttribute("pwmId")]
        public uint pwmId
        {
            get
            {
                return _pwmId;
            }
            set
            {
                _pwmId = value;
            }
        }

        private string _minAngle = "0.0";

        [System.ComponentModel.DefaultValueAttribute("0.0")]
        [XmlAttributeAttribute("minAngle")]
        public string minAngle
        {
            get
            {
                return _minAngle;
            }
            set
            {
                _minAngle = value;
            }
        }

        private string _maxAngle = "360.0";

        [System.ComponentModel.DefaultValueAttribute("360.0")]
        [XmlAttributeAttribute("maxAngle")]
        public string maxAngle
        {
            get
            {
                return _maxAngle;
            }
            set
            {
                _maxAngle = value;
            }
        }

        public servo()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }


    [Serializable()]
    [XmlTypeAttribute("colorsensor", Namespace = "http://team302.org/robot")]


    [XmlRootAttribute("colorsensor", Namespace = "http://team302.org/robot")]
    public partial class colorsensor
    {

        private colorsensorport _port = Robot.colorsensorport.kOnboard;

        [System.ComponentModel.DefaultValueAttribute(Robot.colorsensorport.kOnboard)]
        [XmlAttributeAttribute("port")]
        public colorsensorport port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }

        public colorsensor()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }





    [Serializable()]
    [XmlTypeAttribute("closedLoopControlParameters", Namespace = "http://team302.org/robot")]


    [XmlRootAttribute("closedLoopControlParameters", Namespace = "http://team302.org/robot")]
    public partial class closedLoopControlParameters
    {
        public string name { get; set; }

        [DefaultValue(0D)]
        [robotParameter(true)]
        public double pGain { get; set; }

        [DefaultValue(0D)]
        [robotParameter(true)]
        public double iGain { get; set; }

        [DefaultValue(0D)]
        [robotParameter(true)]
        public double dGain { get; set; }

        [DefaultValue(0D)]
        [robotParameter(true)]
        public double fGain { get; set; }

        [DefaultValue(0D)]
        [robotParameter(true)]
        public double iZone { get; set; }

        public closedLoopControlParameters()
        {
            name = GetType().Name;

            helperFunctions.initializeDefaultValues(this);
        }
    }


    [Serializable()]
    [XmlTypeAttribute("camera", Namespace = "http://team302.org/robot")]


    [XmlRootAttribute("camera", Namespace = "http://team302.org/robot")]
    public partial class camera
    {

        private string _id = "0";

        [System.ComponentModel.DefaultValueAttribute("0")]
        [XmlAttributeAttribute("id")]
        public string id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        private cameraformat _format = Robot.cameraformat.KMJPEG;

        [System.ComponentModel.DefaultValueAttribute(Robot.cameraformat.KMJPEG)]
        [XmlAttributeAttribute("format")]
        public cameraformat format
        {
            get
            {
                return _format;
            }
            set
            {
                _format = value;
            }
        }

        private string _width = "640";

        [System.ComponentModel.DefaultValueAttribute("640")]
        [XmlAttributeAttribute("width")]
        public string width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        private string _height = "480";

        [System.ComponentModel.DefaultValueAttribute("480")]
        [XmlAttributeAttribute("height")]
        public string height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        private string _fps = "30";

        [System.ComponentModel.DefaultValueAttribute("30")]
        [XmlAttributeAttribute("fps")]
        public string fps
        {
            get
            {
                return _fps;
            }
            set
            {
                _fps = value;
            }
        }

        private camerathread _thread = Robot.camerathread.Item_false;

        [System.ComponentModel.DefaultValueAttribute(Robot.camerathread.Item_false)]
        [XmlAttributeAttribute("thread")]
        public camerathread thread
        {
            get
            {
                return _thread;
            }
            set
            {
                _thread = value;
            }
        }

        public camera()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }




    [Serializable()]
    [XmlTypeAttribute("roborio", Namespace = "http://team302.org/robot")]


    [XmlRootAttribute("roborio", Namespace = "http://team302.org/robot")]
    public partial class roborio
    {

        private roborioorientation _orientation = Robot.roborioorientation.X_FORWARD_Y_LEFT;

        [System.ComponentModel.DefaultValueAttribute(Robot.roborioorientation.X_FORWARD_Y_LEFT)]
        [XmlAttributeAttribute("orientation")]
        public roborioorientation orientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                _orientation = value;
            }
        }

        public roborio()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }





    [Serializable()]
    [XmlTypeAttribute("robotVariants", Namespace = "http://team302.org/robot")]


    [XmlRootAttribute("robotVariants", Namespace = "http://team302.org/robot")]
    public partial class robotVariants
    {

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<robot> _robot;

        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [XmlElementAttribute("robot")]
        public System.Collections.ObjectModel.Collection<robot> robot
        {
            get
            {
                return _robot;
            }
            private set
            {
                _robot = value;
            }
        }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<mechanism> _mechanism;

        [XmlElementAttribute("mechanism")]
        public System.Collections.ObjectModel.Collection<mechanism> mechanism
        {
            get
            {
                return _mechanism;
            }
            private set
            {
                _mechanism = value;
            }
        }

        public robotVariants()
        {
            this._robot = new System.Collections.ObjectModel.Collection<robot>();
            this._mechanism = new System.Collections.ObjectModel.Collection<mechanism>();

            helperFunctions.initializeDefaultValues(this);
        }
    }


    [Serializable()]
    [XmlTypeAttribute("pwmultrasonic", Namespace = "http://team302.org/robot")]


    [XmlRootAttribute("pwmultrasonic", Namespace = "http://team302.org/robot")]
    public partial class pwmultrasonic
    {

        private pwmultrasonicname _name = Robot.pwmultrasonicname.front;

        [System.ComponentModel.DefaultValueAttribute(Robot.pwmultrasonicname.front)]
        [XmlAttributeAttribute("name")]
        public pwmultrasonicname name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private uint _pwmId = 0u;

        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 19.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "19")]
        [XmlAttributeAttribute("pwmId")]
        public uint pwmId
        {
            get
            {
                return _pwmId;
            }
            set
            {
                _pwmId = value;
            }
        }

        public pwmultrasonic()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }



    [Serializable()]
    [XmlTypeAttribute("analogultrasonic", Namespace = "http://team302.org/robot")]


    [XmlRootAttribute("analogultrasonic", Namespace = "http://team302.org/robot")]
    public partial class analogultrasonic
    {

        private analogultrasonicname _name = Robot.analogultrasonicname.front;

        [System.ComponentModel.DefaultValueAttribute(Robot.analogultrasonicname.front)]
        [XmlAttributeAttribute("name")]
        public analogultrasonicname name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private uint _analogId = 0u;

        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 7.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "7")]
        [XmlAttributeAttribute("analogId")]
        public uint analogId
        {
            get
            {
                return _analogId;
            }
            set
            {
                _analogId = value;
            }
        }

        public analogultrasonic()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }



    [Serializable()]
    [XmlTypeAttribute("lidar", Namespace = "http://team302.org/robot")]


    [XmlRootAttribute("lidar", Namespace = "http://team302.org/robot")]
    public partial class lidar
    {

        private lidarname _name = Robot.lidarname.front;

        [System.ComponentModel.DefaultValueAttribute(Robot.lidarname.front)]
        [XmlAttributeAttribute("name")]
        public lidarname name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private string _inputpin = "0";

        [System.ComponentModel.DefaultValueAttribute("0")]
        [XmlAttributeAttribute("inputpin")]
        public string inputpin
        {
            get
            {
                return _inputpin;
            }
            set
            {
                _inputpin = value;
            }
        }

        private string _triggerpin = "0";

        [System.ComponentModel.DefaultValueAttribute("0")]
        [XmlAttributeAttribute("triggerpin")]
        public string triggerpin
        {
            get
            {
                return _triggerpin;
            }
            set
            {
                _triggerpin = value;
            }
        }

        public lidar()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }






    [Serializable()]
    [XmlTypeAttribute("led", Namespace = "http://team302.org/robot")]


    [XmlRootAttribute("led", Namespace = "http://team302.org/robot")]
    public partial class led
    {

        private uint _pwmId = 0u;

        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 19.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "19")]
        [XmlAttributeAttribute("pwmId")]
        public uint pwmId
        {
            get
            {
                return _pwmId;
            }
            set
            {
                _pwmId = value;
            }
        }

        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [XmlAttributeAttribute("number")]
        public string number { get; set; }

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

        private blinkinname _name = Robot.blinkinname.front;

        [System.ComponentModel.DefaultValueAttribute(Robot.blinkinname.front)]
        [XmlAttributeAttribute("name")]
        public blinkinname name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private uint _pwmId = 0u;

        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 19.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "19")]
        [XmlAttributeAttribute("pwmId")]
        public uint pwmId
        {
            get
            {
                return _pwmId;
            }
            set
            {
                _pwmId = value;
            }
        }

        public blinkin()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }




    [Serializable()]
    [XmlTypeAttribute("talontach", Namespace = "http://team302.org/robot")]


    [XmlRootAttribute("talontach", Namespace = "http://team302.org/robot")]
    public partial class talontach
    {

        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [XmlElementAttribute("canId")]
        public CAN_ID canId { get; set; }

        private uint _name = 0u;

        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 6.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "6")]
        [XmlAttributeAttribute("name")]
        public uint name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private uint _generalpin = 0u;

        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 11.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "11")]
        [XmlAttributeAttribute("generalpin")]
        public uint generalpin
        {
            get
            {
                return _generalpin;
            }
            set
            {
                _generalpin = value;
            }
        }

        public talontach()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }

    #region ====================== Attribute definitions
    // if isTunable == true, it means that live tuning over network tables is enabled
    // if isTunable == false, it means that after changing the value of the parameter, it will take effect on code regeneration and robot code build
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class robotParameterAttribute : Attribute
    {
        public bool isTunable { get; private set; }

        public robotParameterAttribute(bool isTunable)
        {
            this.isTunable = isTunable;
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class robotConstantAttribute : Attribute
    {
        public robotConstantAttribute()
        {
        }
    }

    #endregion

    static class helperFunctions
    {
        static public void initializeDefaultValues(object obj)
        {
            PropertyInfo[] PIs = obj.GetType().GetProperties();
            foreach (PropertyInfo pi in PIs)
            {
                DefaultValueAttribute dva = pi.GetCustomAttribute<DefaultValueAttribute>();
                if (dva != null)
                    pi.SetValue(obj, Convert.ChangeType(dva.Value, pi.PropertyType));
            }
        }
    }
}

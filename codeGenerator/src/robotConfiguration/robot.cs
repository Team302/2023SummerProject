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
        [constant()]
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
        [tunableParameter()]
        public double deadbandPercent { get; set; }

        [DefaultValue(2.2)]
        [Range(typeof(double), "-1.0", "3.0")]
        [tunableParameter()]
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
        [tunableParameter()]
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
        [tunableParameter()]
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
        [tunableParameter()]
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
        [tunableParameter()]
        public analogInputType type { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "7")]
        [tunableParameter()]
        public uint analogId { get; set; }

        [DefaultValue(0D)]
        [tunableParameter()]
        public double voltageMin { get; set; }

        [DefaultValue(5D)]
        [tunableParameter()]
        public double voltageMax { get; set; }

        [tunableParameter()]
        public double outputMin { get; set; }

        [tunableParameter()]
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
        [tunableParameter()]
        public CAN_BUS canBusName { get; set; }

        [DefaultValue("0.0")]
        [tunableParameter()]
        public string rotation { get; set; }

        [DefaultValue(pigeontype.pigeon1)]
        [tunableParameter()]
        public pigeontype type { get; set; }

        [DefaultValue(pigeonname.CENTER_OF_ROTATION)]
        [tunableParameter()]
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
        [tunableParameter()]
        public string name { get; set; }

        [DefaultValue(0.0)]
        [tunableParameter()]
        public double mountingheight { get; set; }

        [DefaultValue(0.0)]
        [tunableParameter()]
        public double horizontaloffset { get; set; }

        [DefaultValue(0.0)]
        [tunableParameter()]
        public double mountingangle { get; set; }

        [DefaultValue(limelightrotation.Angle_0_deg)]
        [tunableParameter()]
        public limelightrotation rotation { get; set; }

        [DefaultValue(0.0)]
        [tunableParameter()]
        public double targetheight { get; set; }

        [DefaultValue(0.0)]
        [tunableParameter()]
        public double targetheight2 { get; set; }

        [DefaultValue(limelightdefaultledmode.currentpipeline)]
        [tunableParameter()]
        public limelightdefaultledmode defaultledmode { get; set; }

        [DefaultValue(limelightdefaultcammode.vision)]
        [tunableParameter()]
        public limelightdefaultcammode defaultcammode { get; set; }

        [DefaultValue(limelightstreammode.sidebyside)]
        [tunableParameter()]
        public limelightstreammode streammode { get; set; }

        [DefaultValue(limelightsnapshots.off)]
        [tunableParameter()]
        public limelightsnapshots snapshots { get; set; }

        [DefaultValue(0)]
        [tunableParameter()]
        public string crosshairx { get; set; }

        [DefaultValue(0)]
        [tunableParameter()]
        public string crosshairy { get; set; }

        [DefaultValue(0)]
        [tunableParameter()]
        public string secondcrosshairx { get; set; }

        [DefaultValue(0)]
        [tunableParameter()]
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

        [tunableParameter()]
        public double wheelDiameter { get; set; }

        [tunableParameter()]
        public double wheelBase { get; set; }

        [tunableParameter()]
        public double track { get; set; }

        [DefaultValue(chassiswheelSpeedCalcOption.ETHER)]
        [tunableParameter()]
        public chassiswheelSpeedCalcOption wheelSpeedCalcOption { get; set; }

        [DefaultValue(chassisposeEstimationOption.EULERCHASSIS)]
        [tunableParameter()]
        public chassisposeEstimationOption poseEstimationOption { get; set; }

        [tunableParameter()]
        public double maxVelocity { get; set; }

        [tunableParameter()]
        public double maxAngularVelocity { get; set; }

        [tunableParameter()]
        public double maxAcceleration { get; set; }

        [tunableParameter()]
        public double maxAngularAcceleration { get; set; }
    }

    [Serializable()]
    public partial class digitalInput
    {
        [DefaultValue("UNKNOWN")]
        [tunableParameter()]
        public string name { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "25")]
        public uint digitalId { get; set; }

        [DefaultValue(false)]
        [tunableParameter()]
        public bool reversed { get; set; }

        [DefaultValue(0D)]
        [tunableParameter()]
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
        [tunableParameter()]
        public swervemoduletype type { get; set; }

        [DefaultValue(0.0)]
        [tunableParameter()]
        public double turn_p { get; set; }

        [DefaultValue(0.0)]
        [tunableParameter()]
        public double turn_i { get; set; }

        [DefaultValue(0.0)]
        [tunableParameter()]
        public double turn_d { get; set; }

        [DefaultValue(0.0)]
        [tunableParameter()]
        public double turn_f { get; set; }

        [DefaultValue(0.0)]
        [tunableParameter()]
        public double turn_nominal_val { get; set; }

        [DefaultValue(0.0)]
        [tunableParameter()]
        public double turn_peak_val { get; set; }

        [DefaultValue(0.0)]
        [tunableParameter()]
        public double turn_max_acc { get; set; }

        [DefaultValue(0.0)]
        [tunableParameter()]
        public double turn_cruise_vel { get; set; }

        [DefaultValue(1.0)]
        [tunableParameter()]
        public uint countsOnTurnEncoderPerDegreesOnAngleSensor { get; set; }
    }


    [Serializable()]
    public partial class cancoder
    {
        public CAN_ID canId { get; set; }

        [DefaultValue(CAN_BUS.rio)]
        public CAN_BUS canBusName { get; set; }

        [DefaultValue(0D)]
        [tunableParameter()]
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

        [tunableParameter()]
        public string name { get; set; }

        // [DefaultValue(0u)]
        [Range(typeof(uint), "0", "7")]
        public uint channel { get; set; }

        [DefaultValue(false)]
        [tunableParameter()]
        public bool reversed { get; set; }

        [DefaultValue(solenoidtype.REVPH)]
        [tunableParameter()]
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
        [tunableParameter()]
        public string name { get; set; }


        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "19")]
        public uint pwmId { get; set; }

        [DefaultValue("0.0")]
        [tunableParameter()]
        public string minAngle { get; set; }

        [DefaultValue("360.0")]
        [tunableParameter()]
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
        [tunableParameter()]
        public colorsensorport port { get; set; }

        public colorsensor()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }

    [Serializable()]
    public partial class closedLoopControlParameters
    {
        [tunableParameter()]
        public string name { get; set; }

        [DefaultValue(0D)]
        [tunableParameter()]
        public double pGain { get; set; }

        [DefaultValue(0D)]
        [tunableParameter()]
        public double iGain { get; set; }

        [DefaultValue(0D)]
        [tunableParameter()]
        public double dGain { get; set; }

        [DefaultValue(0D)]
        [tunableParameter()]
        public double fGain { get; set; }

        [DefaultValue(0D)]
        [tunableParameter()]
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
        [tunableParameter()]
        public string id { get; set; }

        [DefaultValue(Robot.cameraformat.KMJPEG)]
        [tunableParameter()]
        public cameraformat format { get; set; }

        [DefaultValue(640u)]
        [tunableParameter()]
        public uint width{get; set; }

        [DefaultValue(480)]
        [tunableParameter()]
        public string height{get;set; }

        [DefaultValue(30)]
        [tunableParameter()]
        public uint fps { get; set; }

        [DefaultValue(false)]
        [tunableParameter()]
        public bool thread{get;set; }

        public camera()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }

    [Serializable()]
    public partial class roborio
    {
        [DefaultValue(Robot.roborioorientation.X_FORWARD_Y_LEFT)]
        [tunableParameter()]
        public roborioorientation orientation { get; set; }

        public roborio()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }

    [Serializable()]
    public partial class robotVariants
    {
        public List<robot> robot { get; set; }

        public List<mechanism> mechanism { get; set; }

        public robotVariants()
        {
            robot = new List<robot>();
            mechanism = new List<mechanism>();

            helperFunctions.initializeDefaultValues(this);
        }
    }


    [Serializable()]
    public partial class pwmultrasonic
    {
        [tunableParameter()]
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
        [tunableParameter()]
        public string name { get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "7")]
        public uint id{get;set; }

        public analogultrasonic()
        {
            name = GetType().Name;
            helperFunctions.initializeDefaultValues(this);
        }
    }

    [Serializable()]
    public partial class lidar
    {
        [tunableParameter()]
        public string name{get; set;}

        [DefaultValue(0u)]
        [tunableParameter()]
        public uint inputpin { get; set; }

        [DefaultValue(0)]
        [tunableParameter()]
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
        public uint Id{get;set;}

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
        public string name{get;set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "19")]
        public uint Id{get;set;}

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
        public uint name{get; set; }

        [DefaultValue(0u)]
        [Range(typeof(uint), "0", "11")]
        public uint generalpin{get;set; }

        public talontach()
        {
            helperFunctions.initializeDefaultValues(this);
        }
    }

    #region ====================== Attribute definitions
    // if applied to a property, it means that live tuning over network tables is enabled
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class tunableParameterAttribute : Attribute
    {
        public tunableParameterAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class constantAttribute : Attribute
    {
        public constantAttribute()
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

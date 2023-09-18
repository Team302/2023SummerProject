using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Robot
{
    [Serializable]
    public class motorBase
    {
        [XmlIgnore]
        [robotConstant()]
        public string motorType { get; protected set; }
        
        [robotParameter(true)] 
        public string name { get; set; }
        
        public CAN_ID CAN_ID { get; set; }

        public motorBase() 
        {
            CAN_ID = new CAN_ID();
        }
    }

    [Serializable]
    public class Falcon_Motor : motorBase
    {
        [DefaultValue(1.1)]
        [Range(typeof(double), "0", "62")]
        [robotParameter(true)]
        public double deadbandPercent { get; set; }
        
        [DefaultValue(2.2)]
        [Range(typeof(double), "-1.0", "3.0")] 
        public double peakMin { get; set; }

        [DefaultValue(4.4)]
        [Range(typeof(double), "-10.0", "20.0")]
        public double peakMax { get; set; }

        public Falcon_Motor()
        {
            motorType = this.GetType().Name;
            motorType = motorType.Substring(0, motorType.LastIndexOf('_'));
        }
    }

    [Serializable]
    public class TalonSRX_Motor : motorBase
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
            motorType = this.GetType().Name;
            motorType = motorType.Substring(0, motorType.LastIndexOf('_'));
        }
    }

    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("CAN_ID", Namespace="http://team302.org/robot")]
    public partial class CAN_ID
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private uint _value = 0u;
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 62.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "62")]
        [System.Xml.Serialization.XmlAttributeAttribute("value")]
        public uint value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("CAN_BUS", Namespace="http://team302.org/robot")]
    public enum CAN_BUS
    {
        rio,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("doubleParameter", Namespace="http://team302.org/robot")]
    
    
    public partial class doubleParameter
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private double _value = 0D;
        
        [System.ComponentModel.DefaultValueAttribute(0D)]
        [System.Xml.Serialization.XmlAttributeAttribute("value")]
        public double value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }

    public partial class doubleParameter_
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private double _value = 0D;

        public double value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }

    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("doubleRestrictedParameter", Namespace="http://team302.org/robot")]
    
    
    public partial class doubleRestrictedParameter
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private double _value = 0D;
        
        /// <summary>
        /// <para xml:lang="en">Maximum inclusive value: 2.0.</para>
        /// <para xml:lang="en">Minimum inclusive value: -2.5.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0D)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(double), "-2.5", "2.0")]
        [System.Xml.Serialization.XmlAttributeAttribute("value")]
        public double value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("robot", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("robot", Namespace="http://team302.org/robot")]
    public partial class robot
    {
        public List<motorBase> motorBase { get; set; }
        public List<pcm> pcm { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("pdp")]
        public pdp pdp { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="robot" /> class.</para>
        /// </summary>
        public robot()
        {
            initialize();
            this.pcm = new List<pcm>();
            this._pigeon = new System.Collections.ObjectModel.Collection<pigeon>();
            this._limelight = new System.Collections.ObjectModel.Collection<limelight>();
            this._mechanismInstance = new System.Collections.ObjectModel.Collection<mechanismInstance>();
            this._camera = new System.Collections.ObjectModel.Collection<camera>();
            this._roborio = new System.Collections.ObjectModel.Collection<roborio>();
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<pigeon> _pigeon;
        
        [System.Xml.Serialization.XmlElementAttribute("pigeon")]
        public System.Collections.ObjectModel.Collection<pigeon> pigeon
        {
            get
            {
                return _pigeon;
            }
            private set
            {
                _pigeon = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<limelight> _limelight;
        
        [System.Xml.Serialization.XmlElementAttribute("limelight")]
        public System.Collections.ObjectModel.Collection<limelight> limelight
        {
            get
            {
                return _limelight;
            }
            private set
            {
                _limelight = value;
            }
        }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("chassis")]
        public chassis chassis { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<mechanismInstance> _mechanismInstance;
        
        [System.Xml.Serialization.XmlElementAttribute("mechanismInstance")]
        public System.Collections.ObjectModel.Collection<mechanismInstance> mechanismInstance
        {
            get
            {
                return _mechanismInstance;
            }
            private set
            {
                _mechanismInstance = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<camera> _camera;
        
        [System.Xml.Serialization.XmlElementAttribute("camera")]
        public System.Collections.ObjectModel.Collection<camera> camera
        {
            get
            {
                return _camera;
            }
            private set
            {
                _camera = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<roborio> _roborio;
        
        [System.Xml.Serialization.XmlElementAttribute("roborio")]
        public System.Collections.ObjectModel.Collection<roborio> roborio
        {
            get
            {
                return _roborio;
            }
            private set
            {
                _roborio = value;
            }
        }      
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private uint _robotID = 1u;
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 1.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999.</para>
        /// </summary>
        [DefaultValueAttribute(1u)]
        [RangeAttribute(typeof(uint), "1", "9999")]
        [XmlAttributeAttribute("robotID")]
        public uint robotID
        {
            get
            {
                return _robotID;
            }
            set
            {
                _robotID = value;
            }
        }
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("pdp", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("pdp", Namespace="http://team302.org/robot")]
    public partial class pdp
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private pdptype _type = Robot.pdptype.CTRE;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.pdptype.CTRE)]
        [System.Xml.Serialization.XmlAttributeAttribute("type")]
        public pdptype type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("pdptype", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum pdptype
    {
        
        CTRE,
        
        REV,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("pcm", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("pcm", Namespace="http://team302.org/robot")]
    public partial class pcm
    {
        
        [System.Xml.Serialization.XmlElementAttribute("analogInput")]
        public analogInput analogInput { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("canId")]
        public CAN_ID canId { get; set; }
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("analogInput", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("analogInput", Namespace="http://team302.org/robot")]
    public partial class analogInput
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private analogInputtype _type = Robot.analogInputtype.PRESSURE_GAUGE;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.analogInputtype.PRESSURE_GAUGE)]
        [System.Xml.Serialization.XmlAttributeAttribute("type")]
        public analogInputtype type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private uint _analogId = 0u;
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 7.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "7")]
        [System.Xml.Serialization.XmlAttributeAttribute("analogId")]
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private double _voltageMin = 0D;
        
        [System.ComponentModel.DefaultValueAttribute(0D)]
        [System.Xml.Serialization.XmlAttributeAttribute("voltageMin")]
        public double voltageMin
        {
            get
            {
                return _voltageMin;
            }
            set
            {
                _voltageMin = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private double _voltageMax = 5D;
        
        [System.ComponentModel.DefaultValueAttribute(5D)]
        [System.Xml.Serialization.XmlAttributeAttribute("voltageMax")]
        public double voltageMax
        {
            get
            {
                return _voltageMax;
            }
            set
            {
                _voltageMax = value;
            }
        }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("outputMin")]
        public double outputMin { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("outputMax")]
        public double outputMax { get; set; }
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("analogInputtype", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum analogInputtype
    {
        
        PRESSURE_GAUGE,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("pigeon", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("pigeon", Namespace="http://team302.org/robot")]
    public partial class pigeon
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("canId")]
        public CAN_ID canId { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private CAN_BUS _canBusName = Robot.CAN_BUS.rio;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.CAN_BUS.rio)]
        [System.Xml.Serialization.XmlAttributeAttribute("canBusName")]
        public CAN_BUS canBusName
        {
            get
            {
                return _canBusName;
            }
            set
            {
                _canBusName = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _rotation = "0.0";
        
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        [System.Xml.Serialization.XmlAttributeAttribute("rotation")]
        public string rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private pigeontype _type = Robot.pigeontype.pigeon1;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.pigeontype.pigeon1)]
        [System.Xml.Serialization.XmlAttributeAttribute("type")]
        public pigeontype type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private pigeonname _name = Robot.pigeonname.CENTER_OF_ROTATION;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.pigeonname.CENTER_OF_ROTATION)]
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
        public pigeonname name
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
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("pigeontype", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum pigeontype
    {
        
        pigeon1,
        
        pigeon2,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("pigeonname", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum pigeonname
    {
        
        CENTER_OF_ROTATION,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("limelight", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("limelight", Namespace="http://team302.org/robot")]
    public partial class limelight
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private limelightname _name = Robot.limelightname.MAINLIMELIGHT;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.limelightname.MAINLIMELIGHT)]
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
        public limelightname name
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private limelighttablename _tablename = Robot.limelighttablename.limelight;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.limelighttablename.limelight)]
        [System.Xml.Serialization.XmlAttributeAttribute("tablename")]
        public limelighttablename tablename
        {
            get
            {
                return _tablename;
            }
            set
            {
                _tablename = value;
            }
        }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("mountingheight")]
        public string mountingheight { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _horizontaloffset = "0.0";
        
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        [System.Xml.Serialization.XmlAttributeAttribute("horizontaloffset")]
        public string horizontaloffset
        {
            get
            {
                return _horizontaloffset;
            }
            set
            {
                _horizontaloffset = value;
            }
        }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("mountingangle")]
        public string mountingangle { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private limelightrotation _rotation = Robot.limelightrotation.Item0Period0;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.limelightrotation.Item0Period0)]
        [System.Xml.Serialization.XmlAttributeAttribute("rotation")]
        public limelightrotation rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
            }
        }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("targetheight")]
        public string targetheight { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("targetheight2")]
        public string targetheight2 { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private limelightdefaultledmode _defaultledmode = Robot.limelightdefaultledmode.currentpipeline;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.limelightdefaultledmode.currentpipeline)]
        [System.Xml.Serialization.XmlAttributeAttribute("defaultledmode")]
        public limelightdefaultledmode defaultledmode
        {
            get
            {
                return _defaultledmode;
            }
            set
            {
                _defaultledmode = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private limelightdefaultcammode _defaultcammode = Robot.limelightdefaultcammode.vision;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.limelightdefaultcammode.vision)]
        [System.Xml.Serialization.XmlAttributeAttribute("defaultcammode")]
        public limelightdefaultcammode defaultcammode
        {
            get
            {
                return _defaultcammode;
            }
            set
            {
                _defaultcammode = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private limelightstreammode _streammode = Robot.limelightstreammode.sidebyside;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.limelightstreammode.sidebyside)]
        [System.Xml.Serialization.XmlAttributeAttribute("streammode")]
        public limelightstreammode streammode
        {
            get
            {
                return _streammode;
            }
            set
            {
                _streammode = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private limelightsnapshots _snapshots = Robot.limelightsnapshots.off;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.limelightsnapshots.off)]
        [System.Xml.Serialization.XmlAttributeAttribute("snapshots")]
        public limelightsnapshots snapshots
        {
            get
            {
                return _snapshots;
            }
            set
            {
                _snapshots = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute("crosshairx")]
        public string crosshairx { get; set; }
        
        [System.Xml.Serialization.XmlAttributeAttribute("crosshairy")]
        public string crosshairy { get; set; }
        
        [System.Xml.Serialization.XmlAttributeAttribute("secondcrosshairx")]
        public string secondcrosshairx { get; set; }
        
        [System.Xml.Serialization.XmlAttributeAttribute("secondcrosshairy")]
        public string secondcrosshairy { get; set; }
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("limelightname", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum limelightname
    {
        
        MAINLIMELIGHT,
        
        SECONDARYLIMELIGHT,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("limelighttablename", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum limelighttablename
    {
        
        limelight,
        
        limelight2,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("limelightrotation", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum limelightrotation
    {
        
        [System.Xml.Serialization.XmlEnumAttribute("0.0")]
        Item0Period0,
        
        [System.Xml.Serialization.XmlEnumAttribute("90.0")]
        Item90Period0,
        
        [System.Xml.Serialization.XmlEnumAttribute("180.0")]
        Item180Period0,
        
        [System.Xml.Serialization.XmlEnumAttribute("270.0")]
        Item270Period0,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("limelightdefaultledmode", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum limelightdefaultledmode
    {
        
        currentpipeline,
        
        off,
        
        blink,
        
        on,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("limelightdefaultcammode", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum limelightdefaultcammode
    {
        
        vision,
        
        drivercamera,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("limelightstreammode", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum limelightstreammode
    {
        
        sidebyside,
        
        pipmain,
        
        pipsecondary,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("limelightsnapshots", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum limelightsnapshots
    {
        
        off,
        
        twopersec,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("chassis", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("chassis", Namespace="http://team302.org/robot")]
    public partial class chassis
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<motor> _motor;
        
        [System.Xml.Serialization.XmlElementAttribute("motor")]
        public System.Collections.ObjectModel.Collection<motor> motor
        {
            get
            {
                return _motor;
            }
            private set
            {
                _motor = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="chassis" /> class.</para>
        /// </summary>
        public chassis()
        {
            this._motor = new System.Collections.ObjectModel.Collection<motor>();
            this._swervemodule = new System.Collections.ObjectModel.Collection<swervemodule>();
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<swervemodule> _swervemodule;
        
        [System.Xml.Serialization.XmlElementAttribute("swervemodule")]
        public System.Collections.ObjectModel.Collection<swervemodule> swervemodule
        {
            get
            {
                return _swervemodule;
            }
            private set
            {
                _swervemodule = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private chassistype _type = Robot.chassistype.TANK;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.chassistype.TANK)]
        [System.Xml.Serialization.XmlAttributeAttribute("type")]
        public chassistype type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("wheelDiameter")]
        public string wheelDiameter { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("wheelBase")]
        public string wheelBase { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("track")]
        public string track { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private chassiswheelSpeedCalcOption _wheelSpeedCalcOption = Robot.chassiswheelSpeedCalcOption.ETHER;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.chassiswheelSpeedCalcOption.ETHER)]
        [System.Xml.Serialization.XmlAttributeAttribute("wheelSpeedCalcOption")]
        public chassiswheelSpeedCalcOption wheelSpeedCalcOption
        {
            get
            {
                return _wheelSpeedCalcOption;
            }
            set
            {
                _wheelSpeedCalcOption = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private chassisposeEstimationOption _poseEstimationOption = Robot.chassisposeEstimationOption.EULERCHASSIS;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.chassisposeEstimationOption.EULERCHASSIS)]
        [System.Xml.Serialization.XmlAttributeAttribute("poseEstimationOption")]
        public chassisposeEstimationOption poseEstimationOption
        {
            get
            {
                return _poseEstimationOption;
            }
            set
            {
                _poseEstimationOption = value;
            }
        }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("maxVelocity")]
        public string maxVelocity { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("maxAngularVelocity")]
        public string maxAngularVelocity { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("maxAcceleration")]
        public string maxAcceleration { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("maxAngularAcceleration")]
        public string maxAngularAcceleration { get; set; }
        
        [System.Xml.Serialization.XmlAttributeAttribute("networkTable")]
        public string networkTable { get; set; }
        
        [System.Xml.Serialization.XmlAttributeAttribute("controlFile")]
        public string controlFile { get; set; }
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("motor", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("motor", Namespace="http://team302.org/robot")]
    public partial class motor
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<digitalInput> _digitalInput;
        
        [System.Xml.Serialization.XmlElementAttribute("digitalInput")]
        public System.Collections.ObjectModel.Collection<digitalInput> digitalInput
        {
            get
            {
                return _digitalInput;
            }
            private set
            {
                _digitalInput = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="motor" /> class.</para>
        /// </summary>
        public motor()
        {
            initialize();
this._digitalInput = new System.Collections.ObjectModel.Collection<digitalInput>();
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private motormotorType _motorType = Robot.motormotorType.TALONSRX;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("motorType")]
        public motormotorType motorType
        {
            get
            {
                return _motorType;
            }
            set
            {
                _motorType = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private motorcontroller _controller = Robot.motorcontroller.TALONSRX;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("controller")]
        public motorcontroller controller
        {
            get
            {
                return _controller;
            }
            set
            {
                _controller = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private CAN_BUS _canBusName = Robot.CAN_BUS.rio;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("canBusName")]
        public CAN_BUS canBusName
        {
            get
            {
                return _canBusName;
            }
            set
            {
                _canBusName = value;
            }
        }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("canId")]
        public CAN_ID canId { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("pdpID")]
        public string pdpID { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private bool _inverted = false;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("inverted")]
        public bool inverted
        {
            get
            {
                return _inverted;
            }
            set
            {
                _inverted = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private bool _sensorInverted = false;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("sensorInverted")]
        public bool sensorInverted
        {
            get
            {
                return _sensorInverted;
            }
            set
            {
                _sensorInverted = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private motorfeedbackDevice _feedbackDevice = Robot.motorfeedbackDevice.NONE;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("feedbackDevice")]
        public motorfeedbackDevice feedbackDevice
        {
            get
            {
                return _feedbackDevice;
            }
            set
            {
                _feedbackDevice = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _countsPerRev = "0";
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("countsPerRev")]
        public string countsPerRev
        {
            get
            {
                return _countsPerRev;
            }
            set
            {
                _countsPerRev = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _gearRatio = "1";
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("gearRatio")]
        public string gearRatio
        {
            get
            {
                return _gearRatio;
            }
            set
            {
                _gearRatio = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private bool _brakeMode = false;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("brakeMode")]
        public bool brakeMode
        {
            get
            {
                return _brakeMode;
            }
            set
            {
                _brakeMode = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private int _motorIDtoFollow = -1;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("motorIDtoFollow")]
        public int motorIDtoFollow
        {
            get
            {
                return _motorIDtoFollow;
            }
            set
            {
                _motorIDtoFollow = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _name = "UNKNOWN";
        
        [System.ComponentModel.DefaultValueAttribute("UNKNOWN")]
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
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
        
        [System.Xml.Serialization.XmlAttributeAttribute("peakCurrentDuration")]
        public double peakCurrentDuration { get; set; }
        
        [System.Xml.Serialization.XmlAttributeAttribute("continuousCurrentLimit")]
        public double continuousCurrentLimit { get; set; }
        
        [System.Xml.Serialization.XmlAttributeAttribute("peakCurrentLimit")]
        public double peakCurrentLimit { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private motorcurrentLimiting _currentLimiting = Robot.motorcurrentLimiting.Item_false;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.motorcurrentLimiting.Item_false)]
        [System.Xml.Serialization.XmlAttributeAttribute("currentLimiting")]
        public motorcurrentLimiting currentLimiting
        {
            get
            {
                return _currentLimiting;
            }
            set
            {
                _currentLimiting = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private motorforwardlimitswitch _forwardlimitswitch = Robot.motorforwardlimitswitch.Item_false;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.motorforwardlimitswitch.Item_false)]
        [System.Xml.Serialization.XmlAttributeAttribute("forwardlimitswitch")]
        public motorforwardlimitswitch forwardlimitswitch
        {
            get
            {
                return _forwardlimitswitch;
            }
            set
            {
                _forwardlimitswitch = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private motorforwardlimitswitchopen _forwardlimitswitchopen = Robot.motorforwardlimitswitchopen.Item_true;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.motorforwardlimitswitchopen.Item_true)]
        [System.Xml.Serialization.XmlAttributeAttribute("forwardlimitswitchopen")]
        public motorforwardlimitswitchopen forwardlimitswitchopen
        {
            get
            {
                return _forwardlimitswitchopen;
            }
            set
            {
                _forwardlimitswitchopen = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private motorreverselimitswitch _reverselimitswitch = Robot.motorreverselimitswitch.Item_false;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.motorreverselimitswitch.Item_false)]
        [System.Xml.Serialization.XmlAttributeAttribute("reverselimitswitch")]
        public motorreverselimitswitch reverselimitswitch
        {
            get
            {
                return _reverselimitswitch;
            }
            set
            {
                _reverselimitswitch = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private motorreverselimitswitchopen _reverselimitswitchopen = Robot.motorreverselimitswitchopen.Item_true;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.motorreverselimitswitchopen.Item_true)]
        [System.Xml.Serialization.XmlAttributeAttribute("reverselimitswitchopen")]
        public motorreverselimitswitchopen reverselimitswitchopen
        {
            get
            {
                return _reverselimitswitchopen;
            }
            set
            {
                _reverselimitswitchopen = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private double _voltageCompensationSaturation = 12D;
        
        [System.ComponentModel.DefaultValueAttribute(12D)]
        [System.Xml.Serialization.XmlAttributeAttribute("voltageCompensationSaturation")]
        public double voltageCompensationSaturation
        {
            get
            {
                return _voltageCompensationSaturation;
            }
            set
            {
                _voltageCompensationSaturation = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private motorvoltageCompensationEnable _voltageCompensationEnable = Robot.motorvoltageCompensationEnable.Item_false;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.motorvoltageCompensationEnable.Item_false)]
        [System.Xml.Serialization.XmlAttributeAttribute("voltageCompensationEnable")]
        public motorvoltageCompensationEnable voltageCompensationEnable
        {
            get
            {
                return _voltageCompensationEnable;
            }
            set
            {
                _voltageCompensationEnable = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private uint _countsPerInch = 0u;
        
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.Xml.Serialization.XmlAttributeAttribute("countsPerInch")]
        public uint countsPerInch
        {
            get
            {
                return _countsPerInch;
            }
            set
            {
                _countsPerInch = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private uint _countsPerDegree = 0u;
        
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.Xml.Serialization.XmlAttributeAttribute("countsPerDegree")]
        public uint countsPerDegree
        {
            get
            {
                return _countsPerDegree;
            }
            set
            {
                _countsPerDegree = value;
            }
        }
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("digitalInput", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("digitalInput", Namespace="http://team302.org/robot")]
    public partial class digitalInput
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _name = "UNKNOWN";
        
        [System.ComponentModel.DefaultValueAttribute("UNKNOWN")]
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private uint _digitalId = 0u;
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 25.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "25")]
        [System.Xml.Serialization.XmlAttributeAttribute("digitalId")]
        public uint digitalId
        {
            get
            {
                return _digitalId;
            }
            set
            {
                _digitalId = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private digitalInputreversed _reversed = Robot.digitalInputreversed.Item_false;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.digitalInputreversed.Item_false)]
        [System.Xml.Serialization.XmlAttributeAttribute("reversed")]
        public digitalInputreversed reversed
        {
            get
            {
                return _reversed;
            }
            set
            {
                _reversed = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private double _debouncetime = 0D;
        
        [System.ComponentModel.DefaultValueAttribute(0D)]
        [System.Xml.Serialization.XmlAttributeAttribute("debouncetime")]
        public double debouncetime
        {
            get
            {
                return _debouncetime;
            }
            set
            {
                _debouncetime = value;
            }
        }
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("digitalInputreversed", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum digitalInputreversed
    {
        
        [System.Xml.Serialization.XmlEnumAttribute("true")]
        Item_true,
        
        [System.Xml.Serialization.XmlEnumAttribute("false")]
        Item_false,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("motormotorType", Namespace="http://team302.org/robot", AnonymousType=true)]
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
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("motorcontroller", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum motorcontroller
    {
        
        TALONSRX,
        
        FALCON,
        
        BRUSHLESS_SPARK_MAX,
        
        BRUSHED_SPARK_MAX,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("motorfeedbackDevice", Namespace="http://team302.org/robot", AnonymousType=true)]
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
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("motorcurrentLimiting", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum motorcurrentLimiting
    {
        
        [System.Xml.Serialization.XmlEnumAttribute("true")]
        Item_true,
        
        [System.Xml.Serialization.XmlEnumAttribute("false")]
        Item_false,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("motorforwardlimitswitch", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum motorforwardlimitswitch
    {
        
        [System.Xml.Serialization.XmlEnumAttribute("true")]
        Item_true,
        
        [System.Xml.Serialization.XmlEnumAttribute("false")]
        Item_false,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("motorforwardlimitswitchopen", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum motorforwardlimitswitchopen
    {
        
        [System.Xml.Serialization.XmlEnumAttribute("true")]
        Item_true,
        
        [System.Xml.Serialization.XmlEnumAttribute("false")]
        Item_false,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("motorreverselimitswitch", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum motorreverselimitswitch
    {
        
        [System.Xml.Serialization.XmlEnumAttribute("true")]
        Item_true,
        
        [System.Xml.Serialization.XmlEnumAttribute("false")]
        Item_false,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("motorreverselimitswitchopen", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum motorreverselimitswitchopen
    {
        
        [System.Xml.Serialization.XmlEnumAttribute("true")]
        Item_true,
        
        [System.Xml.Serialization.XmlEnumAttribute("false")]
        Item_false,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("motorvoltageCompensationEnable", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum motorvoltageCompensationEnable
    {
        
        [System.Xml.Serialization.XmlEnumAttribute("true")]
        Item_true,
        
        [System.Xml.Serialization.XmlEnumAttribute("false")]
        Item_false,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("swervemodule", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("swervemodule", Namespace="http://team302.org/robot")]
    public partial class swervemodule
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<motor> _motor;
        
        [System.Xml.Serialization.XmlElementAttribute("motor")]
        public System.Collections.ObjectModel.Collection<motor> motor
        {
            get
            {
                return _motor;
            }
            private set
            {
                _motor = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="swervemodule" /> class.</para>
        /// </summary>
        public swervemodule()
        {
            this._motor = new System.Collections.ObjectModel.Collection<motor>();
        }
        
        [System.Xml.Serialization.XmlElementAttribute("cancoder")]
        public cancoder cancoder { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private swervemoduletype _type = Robot.swervemoduletype.LEFT_FRONT;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.swervemoduletype.LEFT_FRONT)]
        [System.Xml.Serialization.XmlAttributeAttribute("type")]
        public swervemoduletype type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _turn_p = "0.0";
        
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        [System.Xml.Serialization.XmlAttributeAttribute("turn_p")]
        public string turn_p
        {
            get
            {
                return _turn_p;
            }
            set
            {
                _turn_p = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _turn_i = "0.0";
        
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        [System.Xml.Serialization.XmlAttributeAttribute("turn_i")]
        public string turn_i
        {
            get
            {
                return _turn_i;
            }
            set
            {
                _turn_i = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _turn_d = "0.0";
        
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        [System.Xml.Serialization.XmlAttributeAttribute("turn_d")]
        public string turn_d
        {
            get
            {
                return _turn_d;
            }
            set
            {
                _turn_d = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _turn_f = "0.0";
        
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        [System.Xml.Serialization.XmlAttributeAttribute("turn_f")]
        public string turn_f
        {
            get
            {
                return _turn_f;
            }
            set
            {
                _turn_f = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _turn_nominal_val = "0.0";
        
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        [System.Xml.Serialization.XmlAttributeAttribute("turn_nominal_val")]
        public string turn_nominal_val
        {
            get
            {
                return _turn_nominal_val;
            }
            set
            {
                _turn_nominal_val = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _turn_peak_val = "1.0";
        
        [System.ComponentModel.DefaultValueAttribute("1.0")]
        [System.Xml.Serialization.XmlAttributeAttribute("turn_peak_val")]
        public string turn_peak_val
        {
            get
            {
                return _turn_peak_val;
            }
            set
            {
                _turn_peak_val = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _turn_max_acc = "0.0";
        
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        [System.Xml.Serialization.XmlAttributeAttribute("turn_max_acc")]
        public string turn_max_acc
        {
            get
            {
                return _turn_max_acc;
            }
            set
            {
                _turn_max_acc = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _turn_cruise_vel = "0.0";
        
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        [System.Xml.Serialization.XmlAttributeAttribute("turn_cruise_vel")]
        public string turn_cruise_vel
        {
            get
            {
                return _turn_cruise_vel;
            }
            set
            {
                _turn_cruise_vel = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _countsOnTurnEncoderPerDegreesOnAngleSensor = "1.0";
        
        [System.ComponentModel.DefaultValueAttribute("1.0")]
        [System.Xml.Serialization.XmlAttributeAttribute("countsOnTurnEncoderPerDegreesOnAngleSensor")]
        public string countsOnTurnEncoderPerDegreesOnAngleSensor
        {
            get
            {
                return _countsOnTurnEncoderPerDegreesOnAngleSensor;
            }
            set
            {
                _countsOnTurnEncoderPerDegreesOnAngleSensor = value;
            }
        }
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("cancoder", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("cancoder", Namespace="http://team302.org/robot")]
    public partial class cancoder
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("canId")]
        public CAN_ID canId { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private CAN_BUS _canBusName = Robot.CAN_BUS.rio;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.CAN_BUS.rio)]
        [System.Xml.Serialization.XmlAttributeAttribute("canBusName")]
        public CAN_BUS canBusName
        {
            get
            {
                return _canBusName;
            }
            set
            {
                _canBusName = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private double _offset = 0D;
        
        [System.ComponentModel.DefaultValueAttribute(0D)]
        [System.Xml.Serialization.XmlAttributeAttribute("offset")]
        public double offset
        {
            get
            {
                return _offset;
            }
            set
            {
                _offset = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private cancoderreverse _reverse = Robot.cancoderreverse.Item_false;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.cancoderreverse.Item_false)]
        [System.Xml.Serialization.XmlAttributeAttribute("reverse")]
        public cancoderreverse reverse
        {
            get
            {
                return _reverse;
            }
            set
            {
                _reverse = value;
            }
        }
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("cancoderreverse", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum cancoderreverse
    {
        
        [System.Xml.Serialization.XmlEnumAttribute("true")]
        Item_true,
        
        [System.Xml.Serialization.XmlEnumAttribute("false")]
        Item_false,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("swervemoduletype", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum swervemoduletype
    {
        
        LEFT_FRONT,
        
        RIGHT_FRONT,
        
        LEFT_BACK,
        
        RIGHT_BACK,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("chassistype", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum chassistype
    {
        
        TANK,
        
        MECANUM,
        
        SWERVE,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("chassiswheelSpeedCalcOption", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum chassiswheelSpeedCalcOption
    {
        
        WPI,
        
        ETHER,
        
        [System.Xml.Serialization.XmlEnumAttribute("2910")]
        Item2910,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("chassisposeEstimationOption", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum chassisposeEstimationOption
    {
        
        WPI,
        
        EULERCHASSIS,
        
        EULERWHEEL,
        
        POSECHASSIS,
        
        POSEWHEEL,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("mechanismInstance", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("mechanismInstance", Namespace="http://team302.org/robot")]
    public partial class mechanismInstance
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _name = "mechanismInstanceName";
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("name")]
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
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("mechanism")]
        public mechanism mechanism { get; set; }
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("mechanism", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("mechanism", Namespace="http://team302.org/robot")]
    public partial class mechanism
    {
        public List<motorBase> motorBase { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<motor> _motor;
        
        [System.Xml.Serialization.XmlElementAttribute("motor")]
        public System.Collections.ObjectModel.Collection<motor> motor
        {
            get
            {
                return _motor;
            }
            private set
            {
                _motor = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="mechanism" /> class.</para>
        /// </summary>
        public mechanism()
        {
            this.motorBase = new List<motorBase>();

            this._motor = new System.Collections.ObjectModel.Collection<motor>();
            this._solenoid = new System.Collections.ObjectModel.Collection<solenoid>();
            this._servo = new System.Collections.ObjectModel.Collection<servo>();
            this._analogInput = new System.Collections.ObjectModel.Collection<analogInput>();
            this._digitalInput = new System.Collections.ObjectModel.Collection<digitalInput>();
            this._cancoder = new System.Collections.ObjectModel.Collection<cancoder>();
            this._closedLoopControlParameters = new System.Collections.ObjectModel.Collection<closedLoopControlParameters>();
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<solenoid> _solenoid;
        
        [System.Xml.Serialization.XmlElementAttribute("solenoid")]
        public System.Collections.ObjectModel.Collection<solenoid> solenoid
        {
            get
            {
                return _solenoid;
            }
            private set
            {
                _solenoid = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<servo> _servo;
        
        [System.Xml.Serialization.XmlElementAttribute("servo")]
        public System.Collections.ObjectModel.Collection<servo> servo
        {
            get
            {
                return _servo;
            }
            private set
            {
                _servo = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<analogInput> _analogInput;
        
        [System.Xml.Serialization.XmlElementAttribute("analogInput")]
        public System.Collections.ObjectModel.Collection<analogInput> analogInput
        {
            get
            {
                return _analogInput;
            }
            private set
            {
                _analogInput = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<digitalInput> _digitalInput;
        
        [System.Xml.Serialization.XmlElementAttribute("digitalInput")]
        public System.Collections.ObjectModel.Collection<digitalInput> digitalInput
        {
            get
            {
                return _digitalInput;
            }
            private set
            {
                _digitalInput = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<cancoder> _cancoder;
        
        [System.Xml.Serialization.XmlElementAttribute("cancoder")]
        public System.Collections.ObjectModel.Collection<cancoder> cancoder
        {
            get
            {
                return _cancoder;
            }
            private set
            {
                _cancoder = value;
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute("colorsensor")]
        public colorsensor colorsensor { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<closedLoopControlParameters> _closedLoopControlParameters;
        
        [System.Xml.Serialization.XmlElementAttribute("closedLoopControlParameters")]
        public System.Collections.ObjectModel.Collection<closedLoopControlParameters> closedLoopControlParameters
        {
            get
            {
                return _closedLoopControlParameters;
            }
            private set
            {
                _closedLoopControlParameters = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _name = "UNKNOWN";
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("name")]
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
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("solenoid", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("solenoid", Namespace="http://team302.org/robot")]
    public partial class solenoid
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("canId")]
        public CAN_ID canId { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _name = "UNKNOWN";
        
        [System.ComponentModel.DefaultValueAttribute("UNKNOWN")]
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private uint _channel = 0u;
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 7.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "7")]
        [System.Xml.Serialization.XmlAttributeAttribute("channel")]
        public uint channel
        {
            get
            {
                return _channel;
            }
            set
            {
                _channel = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private solenoidreversed _reversed = Robot.solenoidreversed.Item_false;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.solenoidreversed.Item_false)]
        [System.Xml.Serialization.XmlAttributeAttribute("reversed")]
        public solenoidreversed reversed
        {
            get
            {
                return _reversed;
            }
            set
            {
                _reversed = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private solenoidtype _type = Robot.solenoidtype.REVPH;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.solenoidtype.REVPH)]
        [System.Xml.Serialization.XmlAttributeAttribute("type")]
        public solenoidtype type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("solenoidreversed", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum solenoidreversed
    {
        
        [System.Xml.Serialization.XmlEnumAttribute("true")]
        Item_true,
        
        [System.Xml.Serialization.XmlEnumAttribute("false")]
        Item_false,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("solenoidtype", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum solenoidtype
    {
        
        CTREPCM,
        
        REVPH,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("servo", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("servo", Namespace="http://team302.org/robot")]
    public partial class servo
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _name = "UNKNOWN";
        
        [System.ComponentModel.DefaultValueAttribute("UNKNOWN")]
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private uint _pwmId = 0u;
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 19.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "19")]
        [System.Xml.Serialization.XmlAttributeAttribute("pwmId")]
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _minAngle = "0.0";
        
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        [System.Xml.Serialization.XmlAttributeAttribute("minAngle")]
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _maxAngle = "360.0";
        
        [System.ComponentModel.DefaultValueAttribute("360.0")]
        [System.Xml.Serialization.XmlAttributeAttribute("maxAngle")]
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
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("colorsensor", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("colorsensor", Namespace="http://team302.org/robot")]
    public partial class colorsensor
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private colorsensorport _port = Robot.colorsensorport.kOnboard;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.colorsensorport.kOnboard)]
        [System.Xml.Serialization.XmlAttributeAttribute("port")]
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
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("colorsensorport", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum colorsensorport
    {
        
        kOnboard,
        
        kMXP,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("closedLoopControlParameters", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("closedLoopControlParameters", Namespace="http://team302.org/robot")]
    public partial class closedLoopControlParameters
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _name = "UNKNOWN";
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("name")]
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
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("pGain")]
        public doubleParameter pGain { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("iGain")]
        public doubleParameter iGain { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("dGain")]
        public doubleParameter dGain { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("fGain")]
        public doubleParameter fGain { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("iZone")]
        public doubleParameter iZone { get; set; }
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("camera", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("camera", Namespace="http://team302.org/robot")]
    public partial class camera
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _id = "0";
        
        [System.ComponentModel.DefaultValueAttribute("0")]
        [System.Xml.Serialization.XmlAttributeAttribute("id")]
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private cameraformat _format = Robot.cameraformat.KMJPEG;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.cameraformat.KMJPEG)]
        [System.Xml.Serialization.XmlAttributeAttribute("format")]
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _width = "640";
        
        [System.ComponentModel.DefaultValueAttribute("640")]
        [System.Xml.Serialization.XmlAttributeAttribute("width")]
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _height = "480";
        
        [System.ComponentModel.DefaultValueAttribute("480")]
        [System.Xml.Serialization.XmlAttributeAttribute("height")]
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _fps = "30";
        
        [System.ComponentModel.DefaultValueAttribute("30")]
        [System.Xml.Serialization.XmlAttributeAttribute("fps")]
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private camerathread _thread = Robot.camerathread.Item_false;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.camerathread.Item_false)]
        [System.Xml.Serialization.XmlAttributeAttribute("thread")]
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
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("cameraformat", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum cameraformat
    {
        
        KMJPEG,
        
        KYUYV,
        
        KRGB565,
        
        KBGR,
        
        KGRAY,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("camerathread", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum camerathread
    {
        
        [System.Xml.Serialization.XmlEnumAttribute("true")]
        Item_true,
        
        [System.Xml.Serialization.XmlEnumAttribute("false")]
        Item_false,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("roborio", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("roborio", Namespace="http://team302.org/robot")]
    public partial class roborio
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private roborioorientation _orientation = Robot.roborioorientation.X_FORWARD_Y_LEFT;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.roborioorientation.X_FORWARD_Y_LEFT)]
        [System.Xml.Serialization.XmlAttributeAttribute("orientation")]
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
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("roborioorientation", Namespace="http://team302.org/robot", AnonymousType=true)]
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
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("robotVariants", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("robotVariants", Namespace="http://team302.org/robot")]
    public partial class robotVariants
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<robot> _robot;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("robot")]
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
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="robotVariants" /> class.</para>
        /// </summary>
        public robotVariants()
        {
            this._robot = new System.Collections.ObjectModel.Collection<robot>();
            this._mechanism = new System.Collections.ObjectModel.Collection<mechanism>();
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<mechanism> _mechanism;
        
        [System.Xml.Serialization.XmlElementAttribute("mechanism")]
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
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("pwmultrasonic", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("pwmultrasonic", Namespace="http://team302.org/robot")]
    public partial class pwmultrasonic
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private pwmultrasonicname _name = Robot.pwmultrasonicname.front;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.pwmultrasonicname.front)]
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private uint _pwmId = 0u;
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 19.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "19")]
        [System.Xml.Serialization.XmlAttributeAttribute("pwmId")]
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
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("pwmultrasonicname", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum pwmultrasonicname
    {
        
        front,
        
        back,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("analogultrasonic", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("analogultrasonic", Namespace="http://team302.org/robot")]
    public partial class analogultrasonic
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private analogultrasonicname _name = Robot.analogultrasonicname.front;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.analogultrasonicname.front)]
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private uint _analogId = 0u;
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 7.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "7")]
        [System.Xml.Serialization.XmlAttributeAttribute("analogId")]
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
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("analogultrasonicname", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum analogultrasonicname
    {
        
        front,
        
        back,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("lidar", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("lidar", Namespace="http://team302.org/robot")]
    public partial class lidar
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private lidarname _name = Robot.lidarname.front;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.lidarname.front)]
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _inputpin = "0";
        
        [System.ComponentModel.DefaultValueAttribute("0")]
        [System.Xml.Serialization.XmlAttributeAttribute("inputpin")]
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private string _triggerpin = "0";
        
        [System.ComponentModel.DefaultValueAttribute("0")]
        [System.Xml.Serialization.XmlAttributeAttribute("triggerpin")]
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
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("lidarname", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum lidarname
    {
        
        front,
        
        back,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("led", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("led", Namespace="http://team302.org/robot")]
    public partial class led
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private uint _pwmId = 0u;
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 19.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "19")]
        [System.Xml.Serialization.XmlAttributeAttribute("pwmId")]
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
        [System.Xml.Serialization.XmlAttributeAttribute("number")]
        public string number { get; set; }
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("blinkin", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("blinkin", Namespace="http://team302.org/robot")]
    public partial class blinkin
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private blinkinname _name = Robot.blinkinname.front;
        
        [System.ComponentModel.DefaultValueAttribute(Robot.blinkinname.front)]
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private uint _pwmId = 0u;
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 19.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "19")]
        [System.Xml.Serialization.XmlAttributeAttribute("pwmId")]
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
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("blinkinname", Namespace="http://team302.org/robot", AnonymousType=true)]
    public enum blinkinname
    {
        
        front,
        
        back,
        
        top,
        
        bottom,
    }
    
    
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("talontach", Namespace="http://team302.org/robot", AnonymousType=true)]
    
    
    [System.Xml.Serialization.XmlRootAttribute("talontach", Namespace="http://team302.org/robot")]
    public partial class talontach
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("canId")]
        public CAN_ID canId { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private uint _name = 0u;
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 6.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "6")]
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private uint _generalpin = 0u;
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 11.</para>
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0u)]
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "11")]
        [System.Xml.Serialization.XmlAttributeAttribute("generalpin")]
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
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class robotParameterAttribute : Attribute
    {
        public bool isTunable { get; private set; }

        public robotParameterAttribute(bool isTunable)
        {
            this.isTunable = isTunable;
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class robotConstantAttribute : Attribute
    {
        public robotConstantAttribute()
        {
        }
    }

}

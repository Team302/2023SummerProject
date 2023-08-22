using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Robot
{
    public partial class mechanism
    {
        [XmlIgnore]
        public object theTreeNode = null;

        public override string ToString()
        {
            return name;
        }
    }

    public partial class mechanismInstance
    {
        [XmlIgnore]
        public object theTreeNode = null;
    }

    public partial class closedLoopControlParameters
    {
        public closedLoopControlParameters()
        {
            utilities.initializeNullProperties(this);
        }
    }

    public partial class motor
    {
        public void initialize()
        {
            utilities.initializeNullProperties(this);
        }
    }

    public partial class robot
    {
        public void initialize()
        {
            utilities.initializeNullProperties(this);
        }
    }

    partial class pcm
    {
        public pcm()
        {
            utilities.initializeNullProperties(this);
        }
    }

    partial class pigeon
    {
        public pigeon()
        {
            utilities.initializeNullProperties(this);
        }
    }
    partial class cancoder
    {
        public cancoder()
        {
            utilities.initializeNullProperties(this);
        }
    }

    partial class solenoid
    {
        public solenoid()
        {
            utilities.initializeNullProperties(this);
        }
    }

    partial class talontach
    {
        public talontach()
        {
            utilities.initializeNullProperties(this);
        }
    }

    static class utilities
    {
        public static void initializeNullProperties(object obj)
        {
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();

            for (int i = 0; i < propertyInfos.Length; i++)
            {
                PropertyInfo pi = propertyInfos[i];
                if (pi.PropertyType != typeof(System.String))
                {
                    object theObj = pi.GetValue(obj);
                    if (theObj == null)
                    {
                        theObj = Activator.CreateInstance(pi.PropertyType);
                        pi.SetValue(obj, theObj);
                    }
                }
            }
        }
    }
}


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
            PropertyInfo[] propertyInfos = this.GetType().GetProperties();

            for (int i = 0; i < propertyInfos.Length; i++)
            {
                PropertyInfo pi = propertyInfos[i];
                object theObj = pi.GetValue(this);
                if (theObj == null)
                {
                    theObj = Activator.CreateInstance(pi.PropertyType);
                    pi.SetValue(this, theObj);
                }
            }
        }
    }

    public partial class robot
    {
        public void initialize()
        {
            if (pdp == null)
                pdp = new pdp(); ;

            //if (chassis == null)
            //    chassis = new chassis();
        }
    }
}


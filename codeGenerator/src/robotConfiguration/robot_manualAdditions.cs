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


    static class utilities
    {
        public static void initializeNullProperties(object obj)
        {
            initializeNullProperties(obj, false);
        }
        public static void initializeNullProperties(object obj, bool recursive)
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
                        if(recursive)
                            initializeNullProperties(theObj);
                    }
                }
            }
        }
    }
}


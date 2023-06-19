using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Configuration
{
    [Serializable]
    public class toolConfiguration
    {
        [XmlIgnore]
        public string configurationFullPath = "";

        public string rootOutputFolder = "";
        public string robotConfiguration = "";
        public List<string> robotConfigurations = new List<string>();
        public List<string> treeviewParentNameExtensions = new List<string>();

        public string templateMechanismCppPath = "";
        public string templateMechanismHPath = "";

        public string CopyrightNotice = "";
        public string GenerationNotice = "";

        public void loadDummyData()
        {
        }
        public override string ToString()
        {
            return "";
        }

        private void preSerialize()
        {
        }

        private void postSerialize()
        {
        }
        public void serialize(string rootPath)
        {
            var mySerializer = new XmlSerializer(typeof(toolConfiguration));
            using (var myFileStream = new FileStream(Path.Combine(rootPath, @"configuration.xml"), FileMode.Create))
            {
                mySerializer.Serialize(myFileStream, this);
            }
        }
        public toolConfiguration deserialize(string fullFilePathName)
        {
            var mySerializer = new XmlSerializer(typeof(toolConfiguration));

            using (var myFileStream = new FileStream(fullFilePathName, FileMode.Open))
            {
                toolConfiguration tc = (toolConfiguration)mySerializer.Deserialize(myFileStream);
                tc.configurationFullPath = fullFilePathName;
                return tc;
            }
        }
    }


}

﻿using System;
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

        public List<string> collectionBaseTypes = new List<string>();
        public List<string> tunableParameterTypes = new List<string>();
        public List<string> parameterTypes = new List<string>();

        public string templateMechanismCppPath = "";
        public string templateMechanismHPath = "";
        public string templateRobotDefinitionsCppPath = "";
        public string templateRobotDefinitionsHPath = "";


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
            // make the paths relative to the configuration file
            string rootPath = Path.GetDirectoryName(configurationFullPath);

            string temp = rootOutputFolder;
            rootOutputFolder = RelativePath(rootPath, rootOutputFolder);
            rootOutputFolder = temp;

            temp = robotConfiguration;
            robotConfiguration = RelativePath(rootPath, robotConfiguration);
            robotConfiguration = temp;
        }

        private void postSerialize()
        {

        }
        public void serialize(string rootPath)
        {
            preSerialize();

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

                postSerialize();

                return tc;
            }
        }

        public string RelativePath(string absPath, string relTo)
        {
            string[] absDirs = absPath.Split('\\');
            string[] relDirs = relTo.Split('\\');

            // Get the shortest of the two paths
            int len = absDirs.Length < relDirs.Length ? absDirs.Length :
            relDirs.Length;

            // Use to determine where in the loop we exited
            int lastCommonRoot = -1;
            int index;

            // Find common root
            for (index = 0; index < len; index++)
            {
                if (absDirs[index] == relDirs[index]) lastCommonRoot = index;
                else break;
            }

            // If we didn't find a common prefix then throw
            if (lastCommonRoot == -1)
            {
                throw new ArgumentException("Paths do not have a common base");
            }

            // Build up the relative path
            StringBuilder relativePath = new StringBuilder();

            // Add on the ..
            for (index = lastCommonRoot + 1; index < absDirs.Length; index++)
            {
                if (absDirs[index].Length > 0) relativePath.Append("..\\");
            }

            // Add on the folders
            for (index = lastCommonRoot + 1; index < relDirs.Length - 1; index++)
            {
                relativePath.Append(relDirs[index] + "\\");
            }
            relativePath.Append(relDirs[relDirs.Length - 1]);

            return relativePath.ToString();
        }
    }


}

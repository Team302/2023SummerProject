using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Configuration;
using robotConfiguration;
using Robot;
using StateData;
using System.Collections;
using System.Reflection;
using System.Diagnostics;

namespace CoreCodeGenerator
{
    public class codeGenerator_302Robotics : baseReportingClass
    {
        private string generatorConfigFullPathName = "";
        public enum MECHANISM_FILE_TYPE { MAIN, STATE, STATE_MGR }

        private robotConfig theRobotConfiguration = new robotConfig();
        private toolConfiguration theToolConfiguration = new toolConfiguration();

        private string getTemplateFullPath(string templatePath)
        {
            if(Path.IsPathRooted(templatePath)) 
            {
                return templatePath;
            }

            return Path.Combine(Path.GetDirectoryName(theToolConfiguration.configurationFullPath), templatePath);
        }

        private string loadTemplate(string templatePath) 
        {
            return File.ReadAllText(getTemplateFullPath(templatePath));
        }

        public void generate(robotConfig theRobotConfig, toolConfiguration generatorConfig)
        {
            theRobotConfiguration = theRobotConfig;
            theToolConfiguration = generatorConfig;

            string rootFolder = generatorConfig.rootOutputFolder;

            addProgress("Output will be placed at " + rootFolder);

            if (!Directory.Exists(rootFolder))
            {
                Directory.CreateDirectory(rootFolder);
                addProgress("Created output directory " + rootFolder);
            }
            else
            {
                addProgress("Output directory " + rootFolder + " already exists");
            }

            generateMechanismFiles();
        }

        private void generateMechanismFiles()
        {
            addProgress("Writing mechanism files...");
            List<string> mechMainFiles = new List<string>();
            List<string> mechStateFiles = new List<string>();
            List<string> mechStateMgrFiles = new List<string>();

            foreach (mechanism mech in theRobotConfiguration.theRobot.mechanism)
            {
                string templateString;
                string filePathName;

                createMechanismFolder(mech.type);

                #region Generate Cpp File
                templateString = loadTemplate(theToolConfiguration.templateMechanismCppPath);

                filePathName = getMechanismFullFilePathName(mech.type, theToolConfiguration.templateMechanismCppPath);

                File.WriteAllText(filePathName, templateString);
                #endregion

                #region Generate H File
                templateString = loadTemplate(theToolConfiguration.templateMechanismHPath);

                filePathName = getMechanismFullFilePathName(mech.type, theToolConfiguration.templateMechanismHPath);
                File.WriteAllText(filePathName, templateString);
                #endregion
            }
        }

        private void createMechanismFolder(string mechanismName)
        {
            Directory.CreateDirectory(getMechanismOutputPath(mechanismName));
        }

        private string getMechanismFullFilePathName(string mechanismName, string templateFilePath)
        {
            string filename = Path.GetFileName(templateFilePath);

            filename = filename.Replace("MECHANISM_NAME", mechanismName);

            return Path.Combine(getMechanismOutputPath(mechanismName), filename);
        }

        private string getMechanismOutputPath(string mechanismName)
        {
            return Path.Combine(theToolConfiguration.rootOutputFolder, "mechanisms", mechanismName);
        }
    }
}

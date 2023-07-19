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
            foreach (robot theRobot in theRobotConfiguration.theRobotVariants.robot)
            {
                foreach (mechanism mech in theRobot.mechanism)
                {
                    string filePathName;
                    string resultString;

                    string mechanismName = mech.name;

                    createMechanismFolder(mechanismName);

                    #region Generate Cpp File
                    resultString = loadTemplate(theToolConfiguration.templateMechanismCppPath);
                    filePathName = getMechanismFullFilePathName(mechanismName, theToolConfiguration.templateMechanismCppPath);

                    resultString = resultString.Replace("$$_COPYRIGHT_$$", theToolConfiguration.CopyrightNotice);
                    resultString = resultString.Replace("$$_GEN_NOTICE_$$", theToolConfiguration.GenerationNotice);
                    resultString = resultString.Replace("$$_INCLUDE_PATH_$$", getIncludePath(mechanismName));
                    resultString = resultString.Replace("$$_MECHANISM_NAME_$$", mechanismName);

                    #region Tunable Parameters
                    string allParameterReading = "";
                    foreach (closedLoopControlParameters cLCParams in mech.closedLoopControlParameters)
                    {
                        Type objType = cLCParams.GetType();

                        PropertyInfo[] propertyInfos = objType.GetProperties();

                        foreach (PropertyInfo pi in propertyInfos)
                        {
                            bool skip = (pi.Name == "name") || pi.Name.EndsWith("Specified");
                            if (!skip)
                                allParameterReading += string.Format("{0}_{1} = m_table.get()->GetNumber(\"{0}_{1}\", {2});{3}", cLCParams.name, pi.Name, pi.GetValue(cLCParams), Environment.NewLine);
                        }

                    }
                    resultString = resultString.Replace("$$_READ_TUNABLE_PARAMETERS_$$", allParameterReading);

                    string allParameterWriting = "";
                    foreach (closedLoopControlParameters cLCParams in mech.closedLoopControlParameters)
                    {
                        Type objType = cLCParams.GetType();

                        PropertyInfo[] propertyInfos = objType.GetProperties();

                        foreach (PropertyInfo pi in propertyInfos)
                        {
                            bool skip = (pi.Name == "name") || pi.Name.EndsWith("Specified");
                            if (!skip)
                                allParameterWriting += string.Format("{0}_{1} = m_table.get()->PutNumber(\"{0}_{1}\", {0}_{1});{2}", cLCParams.name, pi.Name, Environment.NewLine);
                        }

                    }
                    resultString = resultString.Replace("$$_PUSH_TUNABLE_PARAMETERS_$$", allParameterWriting);

                    #endregion

                    File.WriteAllText(filePathName, resultString);
                    #endregion

                    #region Generate H File
                    resultString = loadTemplate(theToolConfiguration.templateMechanismHPath);
                    filePathName = getMechanismFullFilePathName(mechanismName, theToolConfiguration.templateMechanismHPath);

                    resultString = resultString.Replace("$$_COPYRIGHT_$$", theToolConfiguration.CopyrightNotice);
                    resultString = resultString.Replace("$$_GEN_NOTICE_$$", theToolConfiguration.GenerationNotice);
                    resultString = resultString.Replace("$$_MECHANISM_NAME_$$", mechanismName);

                    //closed loop parameters
                    string allParameters = "";
                    foreach (closedLoopControlParameters cLCParams in mech.closedLoopControlParameters)
                    {
                        Type objType = cLCParams.GetType();

                        PropertyInfo[] propertyInfos = objType.GetProperties();

                        foreach (PropertyInfo pi in propertyInfos)
                        {
                            bool skip = (pi.Name == "name") || pi.Name.EndsWith("Specified");
                            if (!skip)
                                allParameters += string.Format("double {0}_{1} = {2};{3}", cLCParams.name, pi.Name, pi.GetValue(cLCParams), Environment.NewLine);
                        }

                    }
                    resultString = resultString.Replace("$$_TUNABLE_PARAMETERS_$$", allParameters);

                    File.WriteAllText(filePathName, resultString);
                    #endregion
                }
            }
        }

        private string getIncludePath(string mechanismName)
        {
            return getMechanismOutputPath(mechanismName).Replace(theToolConfiguration.rootOutputFolder, "").Replace(@"\", "/").TrimStart('/');
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

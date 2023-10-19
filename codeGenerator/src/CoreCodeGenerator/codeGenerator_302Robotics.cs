using Configuration;
using DataConfiguration;
using Robot;
using robotConfiguration;
using System;
using System.Collections.Generic;
using System.IO;

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

            generateRobotConfigs();
            //generateMechanismFiles();
            //generateRobotDefinitionFiles();
        }

        private void generateRobotConfigs()
        {
            addProgress("Writing robot configuration files...");

            foreach (robot theRobot in theRobotConfiguration.theRobotVariants.robot)
            {

                string configFolderPath = getPathInRoot("configs");
                string resultString = loadTemplate(theToolConfiguration.templateRobotConfigurationCppPath);
                string filePath = Path.Combine(configFolderPath, "RobotConfig" + theRobot.robotID.ToString() + ".cpp");


                #region Generate Cpp file
                resultString = resultString.Replace("$$COPYRIGHT$$", theToolConfiguration.CopyrightNotice);
                resultString = resultString.Replace("$$GENERATION_NOTICE$$", theToolConfiguration.GenerationNotice);

                resultString = resultString.Replace("$$ROBOT_ID$$", theRobot.robotID.ToString());

                string motorDefinition = "";
                string snippet = loadTemplate(theToolConfiguration.snippetsPath + "MOTOR_DEFINITION.txt");
                
                //snippet
                foreach (motor m in theRobot.chassis.motor)
                {
                    motorDefinition += snippet.Replace("$$NAME$$", m.name);
                }

                foreach(mechanismInstance mechanism in theRobot.mechanismInstance)
                {
                    foreach(motor m in mechanism.mechanism.motor)
                    {
                        motorDefinition += snippet.Replace("$$NAME$$", m.name);
                    }
                }

                resultString = resultString.Replace("$$MOTOR_DEFINITION$$", motorDefinition);

                File.WriteAllText(filePath, resultString);
                
                #endregion

                #region Generate H file
                resultString = loadTemplate(theToolConfiguration.templateRobotConfigurationHPath);
                filePath = Path.Combine(configFolderPath, "RobotConfig" + theRobot.robotID.ToString() + ".h");

                resultString = resultString.Replace("$$COPYRIGHT$$", theToolConfiguration.CopyrightNotice);
                resultString = resultString.Replace("$$GENERATION_NOTICE$$", theToolConfiguration.GenerationNotice);

                resultString = resultString.Replace("$$ROBOT_ID$$", theRobot.robotID.ToString());

                #region Mechanism Includes
                string tempString = "";
                string mechanismTemplate = "#include \"INCLUDE/decoratormods/NAME.h\"";
                foreach (mechanismInstance mech in theRobot.mechanismInstance)
                {
                    tempString += mechanismTemplate.Replace("INCLUDE", getIncludePath(mech.name)).Replace("NAME", mech.name);
                }

                resultString = resultString.Replace("$$MECH_INCLUDES$$", tempString);
                #endregion

                #region Motor Init
                string motorTemplate = "MOTOR_TYPE *m_motor-NAME = nullptr;" + Environment.NewLine;
                tempString = "";

                
                foreach(motor m in theRobot.chassis.motor)
                {
                    tempString += motorTemplate.Replace("MOTOR_TYPE", m.motorType).Replace("NAME", m.name);
                }

                foreach (mechanismInstance mech in theRobot.mechanismInstance)
                {
                    foreach (motor m in mech.mechanism.motor)
                    {
                        tempString += motorTemplate.Replace("MOTOR_TYPE", m.motorType).Replace("NAME", m.name);
                    }
                }

                resultString = resultString.Replace("$$MOTOR_INIT$$", tempString);
                #endregion

                #region Solenoid Init
                string solenoidTemplate = "DragonSolenoid m_solenoid-NAME = nullptr;" + Environment.NewLine;
                tempString = "";

                foreach (mechanismInstance mech in theRobot.mechanismInstance)
                {
                    foreach (solenoid s in mech.mechanism.solenoid)
                    {
                        tempString += solenoidTemplate.Replace("NAME", s.name);
                    }
                }

                resultString = resultString.Replace("$$SOLENOID_INIT$$", tempString);
                #endregion

                #region Cancoder Init
                string cancoderTemplate = "DragonCanCoder *m_cancoder-ID = nullptr;" + Environment.NewLine;
                tempString = "";

                foreach (mechanismInstance mechanism in theRobot.mechanismInstance)
                {
                    foreach(cancoder cancoder in mechanism.mechanism.cancoder)
                    {
                        tempString += cancoderTemplate.Replace("ID", cancoder.canId.ToString());
                    }
                }

                resultString = resultString.Replace("$$CANCODER_INIT$$", tempString);
                #endregion

                #region Mechanism Init
                string mechanismInitTemplate = "MECHANISM_TYPE *m_NAME = nullptr;" + Environment.NewLine;
                tempString = "";

                foreach(mechanismInstance mechanism in theRobot.mechanismInstance)
                {
                    tempString += mechanismInitTemplate.Replace("MECHANISM_TYPE", mechanism.mechanism.name).Replace("NAME", mechanism.name.ToLower());
                }

                resultString = resultString.Replace("$$MECHANISM_INIT$$", tempString);
                #endregion

                File.WriteAllText(filePath, resultString);
                #endregion 
            }
        }

        private void generateMechanismFiles()
        {
            addProgress("Writing mechanism files...");
            List<string> mechMainFiles = new List<string>();
            List<string> mechStateFiles = new List<string>();
            List<string> mechStateMgrFiles = new List<string>();
            foreach (robot theRobot in theRobotConfiguration.theRobotVariants.robot)
            {
                foreach(mechanismInstance mech in theRobot.mechanismInstance)
                {
                    string filePath;
                    string resultString;

                    string mechanismName = mech.name;

                    createMechanismFolder(mechanismName);

                    #region Generate 
                    #endregion
                }


                /*
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
                            bool skip = (pi.Name == "name");
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
                            bool skip = (pi.Name == "name");
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
                            bool skip = (pi.Name == "name");
                            if (!skip)
                                allParameters += string.Format("double {0}_{1} = {2};{3}", cLCParams.name, pi.Name, pi.GetValue(cLCParams), Environment.NewLine);
                        }

                    }
                    resultString = resultString.Replace("$$_TUNABLE_PARAMETERS_$$", allParameters);

                    File.WriteAllText(filePathName, resultString);
                    #endregion
                }
                */
            }
        }

        private void generateRobotDefinitionFiles()
        {
            /*
            #region H File
            addProgress("Writing robot definition files...");
            string contents = loadTemplate(theToolConfiguration.templateRobotDefinitionsHPath);
            string filePathName = getRobotDefinitionFilePath(Path.GetFileName(theToolConfiguration.templateRobotDefinitionsHPath));

            addProgress("Writing RobotDefinitions.h...");

            #region Notices
            contents = contents.Replace("$$_COPYRIGHT_$$", theToolConfiguration.CopyrightNotice);
            contents = contents.Replace("$$_GEN_NOTICE_$$", theToolConfiguration.GenerationNotice);
            #endregion

            #region Robot Variant Functions
            //# is robot id
            string functionTemplate = "void Get#Definition();";
            string replacement = "";

            foreach(robot bot in theRobotConfiguration.theRobotVariants.robot)
            {
                //this conditional makes sure the functions are on a new line after the first function
                replacement += replacement != "" ? "\n" : "" + functionTemplate.Replace("#", bot.robotID.ToString());
            }

            contents = contents.Replace("$$_ROBOT_VARIANT_CREATION_$$", replacement);
            #endregion

            #region Components Enum
            replacement = "";
            
            foreach(mechanism mech in theRobotConfiguration.theRobotVariants.mechanism)
            {
                //this conditional makes sure the functions are on a new line after the first function
                replacement += (replacement != "" ? "\n\t" : "") + mech.name + ",";
            }

            replacement = replacement.TrimEnd(',');

            contents = contents.Replace("$$_COMPONENTS_ENUM_$$", replacement);
            #endregion

            //write to RobotDefinitions.h
            File.WriteAllText(filePathName, contents);
            addProgress("Finished writing RobotDefinitions.h...");
            #endregion

            
            #region Cpp File
            contents = loadTemplate(theToolConfiguration.templateRobotDefinitionsCppPath);
            filePathName = getRobotDefinitionFilePath(Path.GetFileName(theToolConfiguration.templateRobotDefinitionsCppPath));

            addProgress("Writing RobotDefinitions.cpp...");

            #region Notices
            contents = contents.Replace("$$_COPYRIGHT_$$", theToolConfiguration.CopyrightNotice);
            contents = contents.Replace("$$_GEN_NOTICE_$$", theToolConfiguration.GenerationNotice);
            #endregion

            #region Includes
            replacement = "";

            foreach (mechanism mech in theRobotConfiguration.theRobotVariants.mechanism)
            {
                /// NOTE: We probably don't need to include mechanism and builder because builder should already have the mechanism included
                //string mechIncludeString = "#include <" + getIncludePath(mech.name) + "/" + mech.name + ".h>";
                string builderIncludeString = "#include <" + getIncludePath(mech.name) + "/" + mech.name + "Builder.h>";
                replacement += (replacement != "" ? "\n" : "") + builderIncludeString;
            }

            contents = contents.Replace("$$_INCLUDES_$$", replacement);
            #endregion

            #region Robot Defintion Switch Statement
            replacement = "switch(teamNumber)\n\t{";
            string replacementEnd = "\n\t\tdefault:\r\n\t\t\treturn Get302Defition();\r\n\t\t\tbreak;\n\t}\n";

            //# is robot id
            string caseTemplate = "\t\tcase #:\r\n\t\t\treturn Get#Definition();\n\t\t\tbreak;";

            foreach (robot bot in theRobotConfiguration.theRobotVariants.robot)
            {
                //this conditional makes sure the functions are on a new line after the first function
                replacement += (replacement != "" ? "\n" : "") + caseTemplate.Replace("#", bot.robotID.ToString());
            }

            contents = contents.Replace("$$_ROBOT_DEFINITION_SWITCH_$$", replacement + replacementEnd);
            #endregion

            //this is where the fucntions will be created to return a new robot definition
            #region Robot Definition Functions
            replacement = "";
            string vectorCreation = "\r\n\tstd::vector<std::pair<RobotDefinitions::Components, std::any>> components = new std::vector<std::pair<RobotDefinitions::Components, std::any>>();";
            string functionHeaderTemplate = "RobotDefinition* Get#Definition()\r\n{";
            string functionFooter = "\r\n\r\n\treturn new RobotDefinition(components);\n}";
            string mechanismTemplate = "\r\n\r\n\tMechanism *MECH = TYPEBuilder::GetBuilder()->CreateNewTYPE(args);\r\n\tmechs.emplace_back(std::make_pair(RobotDefinitions::Components::TYPE, MECH));";

            foreach (robot bot in theRobotConfiguration.theRobotVariants.robot)
            {
                replacement += (replacement != "" ? "\n\n" : "") + functionHeaderTemplate.Replace("#", bot.robotID.ToString());
                replacement += vectorCreation;

                foreach(mechanismInstance mechInstance in bot.mechanismInstance)
                {
                    replacement += mechanismTemplate.Replace("MECH", mechInstance.name).Replace("TYPE", mechInstance.mechanism.name);
                }

                //end with function footer
                replacement += functionFooter;
                ///TODO: Add sensors, pdh, pcm, etc.
            }

            contents = contents.Replace("$$_ROBOT_VARIANT_CREATION_FUNCTIONS_$$", replacement);
            #endregion

            //write to RobotDefinitions.cpp
            File.WriteAllText(filePathName, contents);
            addProgress("Finished writing RobotDefinitions.cpp...");
            #endregion
            */
        }

        private string getIncludePath(string mechanismName)
        {
            return getMechanismOutputPath(mechanismName).ToLower().Replace(theToolConfiguration.rootOutputFolder, "").Replace(@"\", "/").TrimStart('/');
        }

        private void createMechanismFolder(string mechanismName)
        {
            Directory.CreateDirectory(Path.Combine(getMechanismOutputPath(mechanismName), "generated"));
            Directory.CreateDirectory(Path.Combine(getMechanismOutputPath(mechanismName), "decoratormods"));
        }

        private string getMechanismFullFilePathName(string mechanismName, string templateFilePath)
        {
            string filename = Path.GetFileName(templateFilePath);

            filename = filename.Replace("MECHANISM_NAME", mechanismName);

            return Path.Combine(getMechanismOutputPath(mechanismName), filename);
        }

        private string getMechanismOutputPath(string mechanismName)
        {
            return Path.Combine("mechanisms", mechanismName);
        }

        private string getPathInRoot(string path)
        {
            return Path.Combine(theToolConfiguration.rootOutputFolder, path);
        }

        private string getRobotDefinitionFilePath(string filename)
        {
            //later we may add a folder for individual RobotDefinition files if we move away from creating them as functions
            return Path.Combine(getRobotDefinitionOutputPath(), filename);
        }

        private string getRobotDefinitionOutputPath()
        {
            //later we may add a folder for individual RobotDefinition files if we move away from creating them as functions
            return theToolConfiguration.rootOutputFolder;
        }
    }
}

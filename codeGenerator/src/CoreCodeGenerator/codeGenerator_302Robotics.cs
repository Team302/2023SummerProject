﻿using System;
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
            generateRobotDefinitionFiles();
        }

        private void generateMechanismFiles()
        {
            addProgress("Writing mechanism files...");
            List<string> mechMainFiles = new List<string>();
            List<string> mechStateFiles = new List<string>();
            List<string> mechStateMgrFiles = new List<string>();

            foreach(mechanism mech in theRobotConfiguration.theRobotVariants.mechanism)
            {
                string filePathName;
                string resultString;

                string mechanismName = mech.name;

                createMechanismFolder(mechanismName);

                #region Generate Mechanism Base Class

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
                            allParameterReading += (allParameterReading != "" ? "\n\t" : "") + string.Format("m_{0}_{1} = m_table.get()->GetNumber(\"{0}_{1}\", m_{0}_{1});", cLCParams.name, pi.Name);
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
                            allParameterWriting += (allParameterWriting != "" ? "\n\t" : "") + string.Format("m_table.get()->PutNumber(\"{0}_{1}\", m_{0}_{1});", cLCParams.name, pi.Name);
                    }

                }
                resultString = resultString.Replace("$$_PUSH_TUNABLE_PARAMETERS_$$", allParameterWriting);

                #endregion

                #region Member Variable Initialization
                string replacement = "";

                ///NOTE: May want to switch to using (mech.theTreeNode as TreeNode).Nodes to find all children and then add them here
                foreach (motor m in mech.motor)
                {
                    replacement += (replacement != "" ? "\n\t" : "") + "m_" + m.name + " = " + m.name + ";";
                }

                foreach (closedLoopControlParameters cLCParams in mech.closedLoopControlParameters)
                {
                    Type objType = cLCParams.GetType();

                    PropertyInfo[] propertyInfos = objType.GetProperties();

                    foreach (PropertyInfo pi in propertyInfos)
                    {
                        bool skip = (pi.Name == "name") || pi.Name.EndsWith("Specified");
                        if (!skip)
                            replacement += (replacement != "" ? "\n\t" : "") + string.Format("m_{0}_{1} = {0}_{1};", cLCParams.name, pi.Name, Environment.NewLine);
                    }
                }

                resultString = resultString.Replace("$$_MEMBER_VARIABLE_INITIALIZATION_$$", replacement);
                #endregion

                #region Constructor Args
                replacement = getMechanismConstructorArgs(mech);

                resultString = resultString.Replace("$$_CONSTRUCTOR_ARGS_$$", replacement);
                #endregion

                File.WriteAllText(filePathName, resultString);

                #endregion

                #region Generate H File
                resultString = loadTemplate(theToolConfiguration.templateMechanismHPath);
                filePathName = getMechanismFullFilePathName(mechanismName, theToolConfiguration.templateMechanismHPath);

                resultString = resultString.Replace("$$_COPYRIGHT_$$", theToolConfiguration.CopyrightNotice);
                resultString = resultString.Replace("$$_GEN_NOTICE_$$", theToolConfiguration.GenerationNotice);
                resultString = resultString.Replace("$$_MECHANISM_NAME_$$", mechanismName);

                #region Closed Loop Control Paramters
                string allParameters = "";
                foreach (closedLoopControlParameters cLCParams in mech.closedLoopControlParameters)
                {
                    Type objType = cLCParams.GetType();

                    PropertyInfo[] propertyInfos = objType.GetProperties();

                    foreach (PropertyInfo pi in propertyInfos)
                    {
                        bool skip = (pi.Name == "name") || pi.Name.EndsWith("Specified");
                        if (!skip)
                            allParameters += (allParameters!= "" ? "\n\t" : "") + string.Format("double m_{0}_{1};", cLCParams.name, pi.Name);
                    }

                }
                resultString = resultString.Replace("$$_TUNABLE_PARAMETERS_$$", allParameters);
                #endregion

                #region Constructor

                replacement = getMechanismConstructorArgs(mech);
                
                resultString = resultString.Replace("$$_CONSTRUCTOR_ARGS_$$", replacement);
                #endregion

                #region Component Member Variables
                ///NOTE: May want to switch to using (mech.theTreeNode as TreeNode).Nodes to find all children and then add them to these variable declarations
                // for now, just cycle through all the motors until a final mechbuilder design is reached
                replacement = "";
                foreach (motor m in mech.motor)
                {
                    ///NOTE: may need a switch statement for different values of m.controller if the enum doesn't match the class names in the code
                    replacement += (replacement != "" ? ", " : "") + m.controller.ToString() + " *m_" + m.name + ";";
                }
                resultString = resultString.Replace("$$_COMPONENTS_$$", replacement);

                #endregion

                #region Includes
                replacement = "";

                ///NOTE: May want to switch to using (mech.theTreeNode as TreeNode).Nodes to find all children and then include all of them
                foreach (motor m in mech.motor)
                {
                    replacement += (replacement != "" ? "\n" : "") + "#include <hw/" + m.controller.ToString() + ".h>";
                }

                resultString = resultString.Replace("$$_INCLUDES_$$", replacement);
                #endregion

                File.WriteAllText(filePathName, resultString);
                #endregion

                #endregion

                #region Generate Mechanism Builder

                #region Generate Cpp File
                resultString = loadTemplate(theToolConfiguration.templateMechBuilderCppPath);
                filePathName = getMechanismFullFilePathName(mechanismName, theToolConfiguration.templateMechBuilderCppPath);

                File.WriteAllText(filePathName, resultString);
                #endregion

                #region Generate H File
                resultString = loadTemplate(theToolConfiguration.templateMechBuilderHPath);
                filePathName = getMechanismFullFilePathName(mechanismName, theToolConfiguration.templateMechBuilderHPath);

                File.WriteAllText(filePathName, resultString);
                #endregion
                #endregion
            }
        }

        private string getMechanismConstructorArgs(mechanism mech)
        {
            string args = "";
            ///NOTE: May want to switch to using (mech.theTreeNode as TreeNode).Nodes to find all children and then add them to constructor
            ///this requires going to project tab, adding reference to System.Windows.Forms and then adding the using line at the top
            ///foreach(TreeNode in (mech.theTreeNode as TreeNode).Nodes)

            // for now, just cycle through all the motors until a final mechbuilder design is reached
            foreach (motor m in mech.motor)
            {
                ///NOTE: may need a switch statement for different values of m.controller if the enum doesn't match the class names in the code
                args += (args != "" ? ", " : "") + m.controller.ToString() + " " + m.name;
            }

            foreach (closedLoopControlParameters cLCParams in mech.closedLoopControlParameters)
            {
                Type objType = cLCParams.GetType();

                PropertyInfo[] propertyInfos = objType.GetProperties();

                foreach (PropertyInfo pi in propertyInfos)
                {
                    bool skip = (pi.Name == "name") || pi.Name.EndsWith("Specified");
                    if (!skip)
                        args += (args != "" ? ", " : "") + string.Format("double {0}_{1}", cLCParams.name, pi.Name, Environment.NewLine);
                }
            }

            return args;
        }

        private void generateRobotDefinitionFiles()
        {
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

            filename = filename.Replace("MECH", mechanismName);

            return Path.Combine(getMechanismOutputPath(mechanismName), filename);
        }

        private string getMechanismOutputPath(string mechanismName)
        {
            return Path.Combine(theToolConfiguration.rootOutputFolder, "mechanisms", mechanismName);
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

﻿using Configuration;
using DataConfiguration;
using ApplicationData;
using applicationConfiguration;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System;

namespace CoreCodeGenerator
{
    public class codeGenerator_302Robotics : baseReportingClass
    {
        string codeGeneratorVersion = "";

        public enum MECHANISM_FILE_TYPE { MAIN, STATE, STATE_MGR }

        private applicationDataConfig theRobotConfiguration = new applicationDataConfig();
        private toolConfiguration theToolConfiguration = new toolConfiguration();


        public void generate(applicationDataConfig theRobotConfig, toolConfiguration generatorConfig, string codeGenVersion)
        {
            codeGeneratorVersion = codeGenVersion;
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

            generateMiscFiles();
            generateMechanismFiles();

            //generateRobotDefinitionFiles();
        }

        private void generateMiscFiles()
        {
            codeTemplateFile cdf = theToolConfiguration.getTemplateInfo("RobotElementNames");
            string template = loadTemplate(cdf.templateFilePathName);

            StringBuilder sb = new StringBuilder();

            foreach (applicationData robot in theRobotConfiguration.theRobotVariants.Robots)
            {
                foreach (mechanismInstance mi in robot.mechanismInstances)
                {
                    foreach (MotorController mc in mi.mechanism.MotorControllers)
                        sb.AppendLine(getRobotElementName(mi, mc));

                }
            }

            template = template.Replace("$$_ROBOT_ELEMENT_NAMES_ENUMS_$$", sb.ToString().ToUpper());

            copyrightAndGenNoticeAndSave(getOutputFileFullPath(cdf.outputFilePathName), template);
        }

        private string getRobotElementName(mechanismInstance mi, MotorController mc)
        {
            return string.Format("{0}_{1},", ToUnderscoreCase(mi.name), ToUnderscoreCase(mc.name));
        }


        private void generateMechanismFiles()
        {
            addProgress("Writing mechanism files...");
            List<string> mechMainFiles = new List<string>();
            //List<string> mechStateFiles = new List<string>();
            //List<string> mechStateMgrFiles = new List<string>();
            foreach (applicationData theRobot in theRobotConfiguration.theRobotVariants.Robots)
            {
                foreach (mechanismInstance mechInst in theRobot.mechanismInstances)
                {
                    string filePathName;
                    string resultString;

                    string mechanismName = mechInst.name;

                    createMechanismFolder(mechanismName);
#if david
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
#endif
                }
            }
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

            foreach (applicationData bot in theRobotConfiguration.theRobotVariants.Robots)
            {
                //this conditional makes sure the functions are on a new line after the first function
                replacement += replacement != "" ? "\n" : "" + functionTemplate.Replace("#", bot.robotID.ToString());
            }

            contents = contents.Replace("$$_ROBOT_VARIANT_CREATION_$$", replacement);
            #endregion

            #region Components Enum
            replacement = "";

            foreach (mechanism mech in theRobotConfiguration.theRobotVariants.Mechanisms)
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

            foreach (mechanism mech in theRobotConfiguration.theRobotVariants.Mechanisms)
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

            foreach (applicationData bot in theRobotConfiguration.theRobotVariants.Robots)
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

            foreach (applicationData bot in theRobotConfiguration.theRobotVariants.Robots)
            {
                replacement += (replacement != "" ? "\n\n" : "") + functionHeaderTemplate.Replace("#", bot.robotID.ToString());
                replacement += vectorCreation;

                foreach (mechanismInstance mechInstance in bot.mechanismInstances)
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

            filename = filename.Replace("MECHANISM_NAME", mechanismName);

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

        #region Support functions
        private string getTemplateFullPath(string templatePath)
        {
            if (Path.IsPathRooted(templatePath))
            {
                return templatePath;
            }

            return Path.Combine(Path.GetDirectoryName(theToolConfiguration.configurationFullPath), templatePath);
        }

        private string getOutputFileFullPath(string filePath)
        {
            if (Path.IsPathRooted(filePath))
            {
                return filePath;
            }

            return Path.Combine(theToolConfiguration.rootOutputFolder, filePath);
        }

        private string loadTemplate(string templatePath)
        {
            return File.ReadAllText(getTemplateFullPath(templatePath));
        }

        private void copyrightAndGenNoticeAndSave(string outputFilePathName, string contents)
        {
            contents = contents.Replace("$$_Copyright_$$", theToolConfiguration.CopyrightNotice.Trim());
            contents = contents.Replace("$$_GEN_NOTICE_$$", getGenerationInfo());

            contents = astyle.AStyleCaller.beautify(contents, null);

            string outputFullFilePathName = getOutputFileFullPath(outputFilePathName);

            string currentText = "";
            string contentsWithoutGenInfo = "";
            bool writeFile = true;
            if (File.Exists(outputFullFilePathName))
            {
                currentText = File.ReadAllText(outputFullFilePathName);

                currentText = removeGenerationInfo(currentText);
                contentsWithoutGenInfo = removeGenerationInfo(contents);

                if (currentText == contentsWithoutGenInfo)
                    writeFile = false;
            }

            if (writeFile)
            {
                File.WriteAllText(outputFullFilePathName, contents);
                addProgress("Wrote " + outputFullFilePathName);
            }
            else
                addProgress("File content has not changed " + outputFullFilePathName);
        }

        private string removeGenerationInfo(string input)
        {
            int index = input.IndexOf("// Generated on");
            if (index >= 0)
            {
                int nextNewLineIndex = input.IndexOf("\n", index);
                return input.Remove(index, nextNewLineIndex - index);
            }

            return input;
        }

        private string getGenerationInfo()
        {
            string genNotice = theToolConfiguration.GenerationNotice.Trim().Replace("$CODE_GENERATOR_VERSION$", codeGeneratorVersion);
            genNotice = genNotice.Replace("$GENERATION_DATE$", DateTime.Now.ToLongDateString());
            genNotice = genNotice.Replace("$GENERATION_TIME$", DateTime.Now.ToLongTimeString());

            return genNotice;
        }

        private string ToUnderscoreCase(string str)
        {
            if (str.Contains("_"))
                return str;

            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
        #endregion

    }
}

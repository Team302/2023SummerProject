using Configuration;
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
        public bool cleanDecoratorModFolders { get; set; } = false;

        public enum MECHANISM_FILE_TYPE { MAIN, STATE, STATE_MGR }

        private applicationDataConfig theRobotConfiguration = new applicationDataConfig();
        private toolConfiguration theToolConfiguration = new toolConfiguration();

        public void generate(string codeGenVersion, applicationDataConfig theRobotConfig, toolConfiguration generatorConfig)
        {
            generate(codeGenVersion, theRobotConfig, generatorConfig, false);
        }
        public void clean(string codeGenVersion, applicationDataConfig theRobotConfig, toolConfiguration generatorConfig)
        {
            generate(codeGenVersion, theRobotConfig, generatorConfig, true);
        }
        private void generate(string codeGenVersion, applicationDataConfig theRobotConfig, toolConfiguration generatorConfig, bool cleanMode)
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
                addProgress("Output directory " + rootFolder + " already exists and therefore was not created.");
            }

            new MiscellaneousGenerator(codeGenVersion, theRobotConfig, generatorConfig, cleanMode, addProgress).generate();
            new MechanismGenerator(codeGenVersion, theRobotConfig, generatorConfig, cleanMode, addProgress).generate();
            new MechanismInstanceGenerator(codeGenVersion, theRobotConfig, generatorConfig, cleanMode, cleanDecoratorModFolders, addProgress).generate();
            new RobotConfigManagerGenerator(codeGenVersion, theRobotConfig, generatorConfig, cleanMode, addProgress).generate();
            new RobotConfigRobotSpecificGenerator(codeGenVersion, theRobotConfig, generatorConfig, cleanMode, addProgress).generate();
        }

#if david
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
#endif

    }
}

using applicationConfiguration;
using ApplicationData;
using Configuration;
using DataConfiguration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeGenerator
{
    internal class RobotConfigManagerGenerator : baseGenerator
    {
        internal RobotConfigManagerGenerator(string codeGeneratorVersion, applicationDataConfig theRobotConfiguration, toolConfiguration theToolConfiguration, showMessage displayProgress)
        : base(codeGeneratorVersion, theRobotConfiguration, theToolConfiguration)
        {
            setProgressCallback(displayProgress);
        }



        internal void generate()
        {
            string filePathName;
            string resultString;
            codeTemplateFile cdf;
            string template;

            addProgress("Writing Robot Configuration Manager files...");
#if david
            List<string> mechMainFiles = new List<string>();
            //List<string> mechStateFiles = new List<string>();
            //List<string> mechStateMgrFiles = new List<string>();
            foreach (applicationData robot in theRobotConfiguration.theRobotVariants.Robots)
            {
                generatorContext.theRobot = robot;
                foreach (mechanismInstance mi in robot.mechanismInstances)
                {
                    generatorContext.theMechanismInstance = mi;
                    generatorContext.theMechanism = mi.mechanism;

                    

                    string mechanismName = mi.name;

                    createMechanismFolder(mechanismName);

                    #region Generate Cpp File
                    cdf = theToolConfiguration.getTemplateInfo("MechanismInstance_cpp");
                    template = loadTemplate(cdf.templateFilePathName);

                    resultString = template;

                    resultString = resultString.Replace("$$_MECHANISM_INSTANCE_NAME_$$", mi.name);
                    resultString = resultString.Replace("$$_OBJECT_CREATION_$$", ListToString( generateMethod(mi, "generateObjectCreation"),";"));
                    resultString = resultString.Replace("$$_ELEMENT_INITIALIZATION_$$", ListToString(generateMethod(mi, "generateInitialization")));

                    filePathName = getMechanismFullFilePathName(mechanismName, cdf.outputFilePathName.Replace("MECHANISM_INSTANCE_NAME", mechanismName));
                    copyrightAndGenNoticeAndSave(filePathName, resultString);
                    #endregion

                    #region Generate H File
                    cdf = theToolConfiguration.getTemplateInfo("MechanismInstance_h");
                    template = loadTemplate(cdf.templateFilePathName);

                    resultString = template;

                    resultString = resultString.Replace("$$_MECHANISM_NAME_$$", mi.mechanism.name);
                    resultString = resultString.Replace("$$_MECHANISM_INSTANCE_NAME_$$", mi.name);

                    filePathName = getMechanismFullFilePathName(mechanismName, cdf.outputFilePathName.Replace("MECHANISM_INSTANCE_NAME", mechanismName));
                    copyrightAndGenNoticeAndSave(filePathName, resultString);
                    #endregion
                }
            }

#endif


            //case RobotIdentifier::EXAMPLE:
            //m_config = new RobotConfigExample();
            //break;
            #region Generate CPP File
            cdf = theToolConfiguration.getTemplateInfo("RobotConfigMgr_cpp");
            template = loadTemplate(cdf.templateFilePathName);

            generatorContext.clear();
            StringBuilder sb = new StringBuilder();
            foreach (applicationData robot in theRobotConfiguration.theRobotVariants.Robots)
            {
                generatorContext.theRobot = robot;
                sb.AppendLine(string.Format(@"case RobotIdentifier::{0}_{1}:
                                                m_config = new RobotConfig{0}_{1};
                                                break;", robot.name.ToUpper(), robot.robotID.value));
            }
            template = template.Replace("$$_ROBOT_CONFIGURATION_CREATION_$$", sb.ToString());

            copyrightAndGenNoticeAndSave(getOutputFileFullPath(cdf.outputFilePathName), template);
            #endregion

            #region Generate H File
            cdf = theToolConfiguration.getTemplateInfo("RobotConfigMgr_h");
            template = loadTemplate(cdf.templateFilePathName);

            generatorContext.clear();
            sb = new StringBuilder();
            foreach (applicationData robot in theRobotConfiguration.theRobotVariants.Robots)
            {
                generatorContext.theRobot = robot;
                sb.AppendLine(string.Format("{0}_{1},", robot.name, robot.robotID.value));
            }
            template = template.Replace("$$_ROBOT_CONFIGURATIONS_NAMES_ENUMS_$$", sb.ToString().ToUpper());

            copyrightAndGenNoticeAndSave(getOutputFileFullPath(cdf.outputFilePathName), template);
            #endregion
        }
    }
}

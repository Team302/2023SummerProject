﻿using applicationConfiguration;
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
    internal class MechanismInstanceGenerator : baseGenerator
    {
        internal MechanismInstanceGenerator(string codeGeneratorVersion, applicationDataConfig theRobotConfiguration, toolConfiguration theToolConfiguration, showMessage displayProgress)
        : base(codeGeneratorVersion, theRobotConfiguration, theToolConfiguration)
        {
            setProgressCallback(displayProgress);
        }



        internal void generate()
        {
            addProgress("Writing mechanism instance files...");
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

                    string filePathName;
                    string resultString;
                    codeTemplateFile cdf;
                    string template;

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
        }

        internal string getIncludePath(string mechanismName)
        {
            return getMechanismOutputPath(mechanismName).Replace(theToolConfiguration.rootOutputFolder, "").Replace(@"\", "/").TrimStart('/');
        }

        internal void createMechanismFolder(string mechanismName)
        {
            Directory.CreateDirectory(getMechanismOutputPath(mechanismName));
        }

        internal string getMechanismFullFilePathName(string mechanismName, string templateFilePath)
        {
            string filename = Path.GetFileName(templateFilePath);

            filename = filename.Replace("MECHANISM_NAME", mechanismName);

            return Path.Combine(getMechanismOutputPath(mechanismName), filename);
        }

        internal string getMechanismOutputPath(string mechanismName)
        {
            return Path.Combine(theToolConfiguration.rootOutputFolder, "mechanisms", mechanismName, "generated");
        }

    }
}

﻿using applicationConfiguration;
using ApplicationData;
using Configuration;
using DataConfiguration;
using System;
using System.CodeDom.Compiler;
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
        internal MechanismInstanceGenerator(string codeGeneratorVersion, applicationDataConfig theRobotConfiguration, toolConfiguration theToolConfiguration, bool cleanMode, bool cleanDecoratorModFolders, showMessage displayProgress)
        : base(codeGeneratorVersion, theRobotConfiguration, theToolConfiguration, cleanMode, cleanDecoratorModFolders)
        {
            setProgressCallback(displayProgress);
        }



        internal void generate()
        {
            addProgress((cleanMode ? "Erasing" : "Writing") + " mechanism instance files...");
            List<string> mechMainFiles = new List<string>();
            //List<string> mechStateFiles = new List<string>();
            //List<string> mechStateMgrFiles = new List<string>();
            List<string> mechInstanceNames = new List<string>();
            foreach (applicationData robot in theRobotConfiguration.theRobotVariants.Robots)
            {
                generatorContext.theRobot = robot;
                foreach (mechanismInstance mi in robot.mechanismInstances)
                {
                    if (!mechInstanceNames.Exists(n => n == mi.name))
                    {
                        mechInstanceNames.Add(mi.name);

                        generatorContext.theMechanismInstance = mi;
                        generatorContext.theMechanism = mi.mechanism;

                        string filePathName;
                        string resultString;
                        codeTemplateFile cdf;
                        string template;

                        string mechanismName = mi.name;


                        #region the generated files
                        createMechanismFolder(mechanismName, true);
                        createMechanismFolder(mechanismName, false);
                        #region Generate Cpp File
                        cdf = theToolConfiguration.getTemplateInfo("MechanismInstance_gen_cpp");
                        template = loadTemplate(cdf.templateFilePathName);

                        resultString = template;


                        resultString = resultString.Replace("$$_MECHANISM_TYPE_NAME_$$", ToUnderscoreCase(mi.name).ToUpper());
                        resultString = resultString.Replace("$$_MECHANISM_NAME_$$", mi.mechanism.name);
                        resultString = resultString.Replace("$$_MECHANISM_INSTANCE_NAME_$$", mi.name);
                        resultString = resultString.Replace("$$_OBJECT_CREATION_$$", ListToString(generateMethod(mi, "generateIndexedObjectCreation"), ";"));
                        resultString = resultString.Replace("$$_ADD_TO_MAPS_$$", ListToString(generateMethod(mi, "generateObjectAddToMaps"), ";"));
                        resultString = resultString.Replace("$$_STATE_CLASSES_INCLUDES_$$", ListToString(generateMethod(mi, "generateIncludes"), ""));

                        List<string> stateTransitions = new List<string>();
                        foreach (state s in mi.mechanism.states)
                        {
                            if (s.transitionsTo.Count > 0)
                            {
                                foreach (stringParameterConstInMechInstance transition in s.transitionsTo)
                                {
                                    stateTransitions.Add(String.Format("{0}State->RegisterTransitionState({1}State)", s.name, transition.value));
                                }
                            }
                            else
                            {
                                stateTransitions.Add(String.Format("{0}State->RegisterTransitionState({0}State)", s.name));
                            }
                        }

                        resultString = resultString.Replace("$$_STATE_TRANSITION_REGISTRATION_$$", ListToString(stateTransitions, ";"));


                        List<string> theUsings = generateMethod(mi, "generateUsings").Distinct().ToList();
                        resultString = resultString.Replace("$$_USING_DIRECTIVES_$$", ListToString(theUsings, ";"));

                        List<string> initCode = new List<string>
                        {
                            "if(false){}"
                        };
                        foreach (applicationData r in theRobotConfiguration.theRobotVariants.Robots)
                        {
                            mechanismInstance mis = r.mechanismInstances.Find(m => m.name == mi.name);
                            if (mis != null)
                            {
                                initCode.Add(string.Format("else if(RobotConfigMgr::RobotIdentifier::{0} == robotFullName)", r.getFullRobotName()));
                                initCode.Add("{");
                                initCode.AddRange(generateMethod(mis, "generateInitialization"));
                                initCode.Add("}");
                            }
                        }
                        resultString = resultString.Replace("$$_ELEMENT_INITIALIZATION_$$", ListToString(initCode));

                        filePathName = getMechanismFullFilePathName(mechanismName, cdf.outputFilePathName.Replace("MECHANISM_INSTANCE_NAME", mechanismName), true);
                        copyrightAndGenNoticeAndSave(filePathName, resultString);
                        #endregion

                        #region Generate H File
                        cdf = theToolConfiguration.getTemplateInfo("MechanismInstance_gen_h");
                        template = loadTemplate(cdf.templateFilePathName);

                        resultString = template;

                        resultString = resultString.Replace("$$_MECHANISM_NAME_$$", mi.mechanism.name);
                        resultString = resultString.Replace("$$_MECHANISM_INSTANCE_NAME_$$", mi.name);

                        List<string> enumList = new List<string>();
                        foreach (state s in mi.mechanism.states)
                        {
                            enumList.Add(String.Format("STATE_{0}", ToUnderscoreCase(s.name).ToUpper()));
                        }

                        resultString = resultString.Replace("$$_STATE_NAMES_$$", ListToString(enumList, ", "));

                        filePathName = getMechanismFullFilePathName(mechanismName, cdf.outputFilePathName.Replace("MECHANISM_INSTANCE_NAME", mechanismName), true);
                        copyrightAndGenNoticeAndSave(filePathName, resultString);
                        #endregion

                        #region Generate CPP baseStateGen File
                        cdf = theToolConfiguration.getTemplateInfo("BaseStateGen_cpp");
                        template = loadTemplate(cdf.templateFilePathName);

                        resultString = template;

                        resultString = resultString.Replace("$$_MECHANISM_NAME_$$", mi.mechanism.name);
                        resultString = resultString.Replace("$$_MECHANISM_INSTANCE_NAME_$$", mi.name);

                        filePathName = getMechanismFullFilePathName(mechanismName, cdf.outputFilePathName.Replace("MECHANISM_INSTANCE_NAME", mechanismName), true);
                        copyrightAndGenNoticeAndSave(filePathName, resultString);
                        #endregion

                        #region Generate H baseStateGen File
                        cdf = theToolConfiguration.getTemplateInfo("BaseStateGen_h");
                        template = loadTemplate(cdf.templateFilePathName);

                        resultString = template;

                        resultString = resultString.Replace("$$_MECHANISM_NAME_$$", mi.mechanism.name);
                        resultString = resultString.Replace("$$_MECHANISM_INSTANCE_NAME_$$", mi.name);

                        filePathName = getMechanismFullFilePathName(mechanismName, cdf.outputFilePathName.Replace("MECHANISM_INSTANCE_NAME", mechanismName), true);
                        copyrightAndGenNoticeAndSave(filePathName, resultString);
                        #endregion

                        #region Generate H StateGen Files
                        foreach (state s in mi.mechanism.states)
                        {
                            cdf = theToolConfiguration.getTemplateInfo("stateGen_h");
                            template = loadTemplate(cdf.templateFilePathName);

                            resultString = template;

                            resultString = resultString.Replace("$$_MECHANISM_NAME_$$", mi.mechanism.name);
                            resultString = resultString.Replace("$$_MECHANISM_INSTANCE_NAME_$$", mi.name);
                            resultString = resultString.Replace("$$_STATE_NAME_$$", s.name);

                            List<string> targetDecl = new List<string>();
                            foreach (doubleParameterUserDefinedTunableOnlyValueChangeableInMechInst d in s.doubleTargets)
                            {
                                targetDecl.Add(string.Format("{0} {1} = {0}({2})", generatorContext.theGeneratorConfig.getWPIphysicalUnitType(d.__units__), d.name, d.value));
                            }
                            foreach (boolParameterUserDefinedTunableOnlyValueChangeableInMechInst d in s.booleanTargets)
                            {
                                targetDecl.Add(string.Format("bool {0} = {1}", d.name, d.value.ToString().ToLower()));
                            }
                            resultString = resultString.Replace("$$_TARGET_DECLARATIONS_$$", ListToString(targetDecl,";"));
                            
                            filePathName = getMechanismFullFilePathName(mechanismName,
                                                                        cdf.outputFilePathName.Replace("MECHANISM_INSTANCE_NAME", mechanismName).Replace("STATE_NAME", s.name)
                                                                        , true);
                            copyrightAndGenNoticeAndSave(filePathName, resultString);
                        }
                        #endregion

                        #region Generate CPP StateGen Files
                        foreach (state s in mi.mechanism.states)
                        {
                            cdf = theToolConfiguration.getTemplateInfo("stateGen_cpp");
                            template = loadTemplate(cdf.templateFilePathName);

                            resultString = template;

                            resultString = resultString.Replace("$$_MECHANISM_NAME_$$", mi.mechanism.name);
                            resultString = resultString.Replace("$$_MECHANISM_INSTANCE_NAME_$$", mi.name);
                            resultString = resultString.Replace("$$_STATE_NAME_$$", s.name);

                            filePathName = getMechanismFullFilePathName(mechanismName,
                                                                        cdf.outputFilePathName.Replace("MECHANISM_INSTANCE_NAME", mechanismName).Replace("STATE_NAME", s.name)
                                                                        , true);
                            copyrightAndGenNoticeAndSave(filePathName, resultString);
                        }
                        #endregion

                        #endregion

                        #region The decorator mod files
                        createMechanismFolder(mechanismName, false);

                        #region Generate Cpp File
                        cdf = theToolConfiguration.getTemplateInfo("MechanismInstance_cpp");
                        template = loadTemplate(cdf.templateFilePathName);

                        resultString = template;

                        resultString = resultString.Replace("$$_MECHANISM_INSTANCE_NAME_$$", mi.name);

                        filePathName = getMechanismFullFilePathName(mechanismName, cdf.outputFilePathName.Replace("MECHANISM_INSTANCE_NAME", mechanismName), false);
                        copyrightAndGenNoticeAndSave(filePathName, resultString,true);
                        #endregion

                        #region Generate H File
                        cdf = theToolConfiguration.getTemplateInfo("MechanismInstance_h");
                        template = loadTemplate(cdf.templateFilePathName);

                        resultString = template;

                        resultString = resultString.Replace("$$_MECHANISM_INSTANCE_NAME_$$", mi.name);

                        filePathName = getMechanismFullFilePathName(mechanismName, cdf.outputFilePathName.Replace("MECHANISM_INSTANCE_NAME", mechanismName), false);
                        copyrightAndGenNoticeAndSave(filePathName, resultString, true);
                        #endregion

                        #region Generate H StateGen_Decorator Files
                        foreach (state s in mi.mechanism.states)
                        {
                            cdf = theToolConfiguration.getTemplateInfo("stateGen_Decorator_h");
                            template = loadTemplate(cdf.templateFilePathName);

                            resultString = template;

                            resultString = resultString.Replace("$$_MECHANISM_NAME_$$", mi.mechanism.name);
                            resultString = resultString.Replace("$$_MECHANISM_INSTANCE_NAME_$$", mi.name);
                            resultString = resultString.Replace("$$_STATE_NAME_$$", s.name);

                            filePathName = getMechanismFullFilePathName(mechanismName,
                                                                        cdf.outputFilePathName.Replace("MECHANISM_INSTANCE_NAME", mechanismName).Replace("STATE_NAME", s.name)
                                                                        , false);
                            copyrightAndGenNoticeAndSave(filePathName, resultString,true);
                        }
                        #endregion

                        #region Generate CPP StateGen_Decorator Files
                        foreach (state s in mi.mechanism.states)
                        {
                            cdf = theToolConfiguration.getTemplateInfo("stateGen_Decorator_cpp");
                            template = loadTemplate(cdf.templateFilePathName);

                            resultString = template;

                            resultString = resultString.Replace("$$_MECHANISM_NAME_$$", mi.mechanism.name);
                            resultString = resultString.Replace("$$_MECHANISM_INSTANCE_NAME_$$", mi.name);
                            resultString = resultString.Replace("$$_STATE_NAME_$$", s.name);

                            filePathName = getMechanismFullFilePathName(mechanismName,
                                                                        cdf.outputFilePathName.Replace("MECHANISM_INSTANCE_NAME", mechanismName).Replace("STATE_NAME", s.name)
                                                                        , false);
                            copyrightAndGenNoticeAndSave(filePathName, resultString,true);
                        }
                        #endregion

                        #endregion

                        if (cleanMode)
                        {
                            Directory.Delete(getMechanismOutputPath(mechanismName, true));
                            if (cleanDecoratorModFolders)
                            {
                                Directory.Delete(getMechanismOutputPath(mechanismName, false));
                                Directory.Delete(Path.Combine(getMechanismOutputPath(mechanismName, true), ".."));
                            }
                        }
                    }
                }
            }
        }

        internal string getIncludePath(string mechanismName, bool generated)
        {
            return getMechanismOutputPath(mechanismName, generated).Replace(theToolConfiguration.rootOutputFolder, "").Replace(@"\", "/").TrimStart('/');
        }

        internal void createMechanismFolder(string mechanismName, bool generated)
        {
            Directory.CreateDirectory(getMechanismOutputPath(mechanismName, generated));
        }

        internal string getMechanismFullFilePathName(string mechanismName, string templateFilePath, bool generated)
        {
            string filename = Path.GetFileName(templateFilePath);

            filename = filename.Replace("MECHANISM_NAME", mechanismName);

            return Path.Combine(getMechanismOutputPath(mechanismName, generated), filename);
        }

        internal string getMechanismOutputPath(string mechanismName, bool generated)
        {
            return Path.Combine(theToolConfiguration.rootOutputFolder, "mechanisms", mechanismName, generated ? "generated" : "decoratormods");
        }

    }
}

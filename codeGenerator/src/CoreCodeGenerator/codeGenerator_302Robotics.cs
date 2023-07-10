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
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics.Eventing.Reader;

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

                    string mechanismName = mech.mechanismName;

                    createMechanismFolder(mechanismName);
                    
                    #region Generate Cpp File
                    resultString = loadTemplate(theToolConfiguration.templateMechanismCppPath);
                    filePathName = getMechanismFullFilePathName(mechanismName, theToolConfiguration.templateMechanismCppPath);

                    resultString = resultString.Replace("$$_COPYRIGHT_$$", theToolConfiguration.CopyrightNotice);
                    resultString = resultString.Replace("$$_GEN_NOTICE_$$", theToolConfiguration.GenerationNotice);
                    resultString = resultString.Replace("$$_INCLUDE_PATH_$$", getIncludePath(mechanismName));
                    //foreach((string,string) pair in theToolConfiguration.mechanismReplacements)
                    //{
                    //    //string replacement = replace(pair.Item2, mech, theRobot);
                    //    //Debug.WriteLine(replacement);
                    //    //resultString = resultString.Replace(pair.Item1, replacement);
                    //}

                    //string replacement = "testing $$^mechanismName$$ testing";
                    string replacement = "testing $$^closedLoopControlParameters^name$$ testing";
                    string result = replace(replacement, mech, theRobot);
                    Debug.WriteLine("Result: " + result);

                    resultString = "";

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

        private string replace(string text, object curObject, robot currentRobot)
        {
            string marker = "$$";
            List<string> resultList = new List<string>() { text.Replace("$$", "") };

            //get the number of markers to properly split up the replacement string
            int count = countOccurencesInString(text, marker) + 1;

            string[] arr = text.Split(new string[] { marker }, StringSplitOptions.None);

            foreach (string s in arr)
            {
                if (s.Contains("^"))
                {
                    int numberOfReplacements = 0;
                    List<string> replacements = new List<string>();
                    List<string> tempStringList = new List<string>();

                    (numberOfReplacements, replacements) = findReplacements(s, curObject, currentRobot);
                    
                    foreach(string str in resultList)
                    {
                        for(int i = 0; i < numberOfReplacements; i++)
                        {
                            tempStringList.Add(str.Replace(s, replacements[i]));
                        }
                    }
                    resultList = tempStringList;
                }
            }

            //Format resultList into one long string
            string resultString = "";
            for(int i = 0; i < resultList.Count; i++)
            {
                if (i != 0) //we aren't on first replacement so we can go to a new line
                {
                    resultString += Environment.NewLine;
                }

                resultString += resultList[i];
            }

            return resultString;
        }
        
        private (int, List<string>) findReplacements(string str, object curObject, robot currentRobot)
        {
            int numberOfReplacements = 0;
            List<string> replacements = new List<string>();

            string[] arr = str.Split(new string[] { "^" }, StringSplitOptions.None);

            string[] excludes = str.Split(new string[] { "-" }, StringSplitOptions.None);

            ///Debugging
            //Debug.WriteLine("Current object name: " + curObject.GetType().Name);
            //Debug.WriteLine("String: " + str);

            if (arr[0] != "")
            {
                //not using default parent object (mechanism), need to find new parent
                findReplacements(str.Remove(0, arr[0].Length), findObject(arr[0], currentRobot), currentRobot); //this must always be currentRobot

                /// Debugging
                Debug.WriteLine("arr[0] != \"\"");
            }
            else
            {
                Debug.WriteLine("arr[0] == \"\"");
                if (excludes.Length > 1)
                {
                    for (int i = 1; i < excludes.Length; i++)
                    {
                        if (arr[1].Contains(excludes[i]) || curObject.GetType().Name.Contains(excludes[i]))
                        {
                            goto ifExcluded; //if we want to exclude the current element , skip past replacement code
                        }
                    }   
                }

                Debug.WriteLine("Arr[1]: " + arr[1]);

                switch (arr[1])
                {
                    case "ALL":
                        { 
                            if (isACollection(curObject))
                            {
                                foreach (object o in (curObject as IList))
                                {
                                    findReplacements(str.Remove(0, arr[1].Length), o, currentRobot);
                                }
                            }
                            break;
                        }

                    case "NAME":
                        { 
                            string name = curObject.GetType().Name;
                            replacements.Add(name);
                            numberOfReplacements++;
                            break;
                        }
                    case "VALUE":
                        {
                            string value = curObject.GetType().GetProperty(arr[1]).GetValue(curObject).ToString();
                            replacements.Add(value);
                            numberOfReplacements++;
                            break;
                        }

                    default:
                        {
                            if (isACollection(curObject))
                            {
                                foreach (object o in (curObject as IList))
                                {
                                    int num = 0;
                                    List<string> newReplacements = new List<string>();
                                    (num, newReplacements) = findReplacements(str, o, currentRobot);
                                    numberOfReplacements += num;
                                    foreach (string s in newReplacements)
                                        replacements.Add(s);
                                }
                            }
                            else if (curObject.GetType().GetProperty(arr[1]) != null)
                            {
                                object nextChild = curObject.GetType().GetProperty(arr[1]).GetValue(curObject);
                                if (isACollection(nextChild))
                                {
                                    int num = 0;
                                    List<string> newReplacements = new List<string>();
                                    (num, newReplacements) = findReplacements(str.Replace(arr[1], "").Remove(0, 1), nextChild, currentRobot);
                                    numberOfReplacements += num;
                                    foreach(string s in newReplacements)
                                        replacements.Add(s);
                                }
                                else
                                {
                                    Debug.WriteLine("Value: " + curObject.GetType().GetProperty(arr[1]).GetValue(curObject));
                                    string value = curObject.GetType().GetProperty(arr[1]).GetValue(curObject).ToString();
                                    replacements.Add(value);
                                    numberOfReplacements++;
                                }
                            }
                            else
                            {
                                string value = curObject.GetType().GetProperty(arr[1]).GetValue(curObject).ToString();
                                replacements.Add(value);
                                numberOfReplacements++;
                            }
                            break;
                        }
                }
            }
            ifExcluded:
            return (numberOfReplacements, replacements);
        }

        private object findObject(string str, object currentObject)
        {
            //string could be chassis/track (robot is implied)
            string marker = "/";

            string[] arr = str.Split(new string[] { marker }, StringSplitOptions.None);

            object result = null;
            if(isACollection(currentObject))
            {
                foreach (object o in currentObject as IList)
                {
                    if(o.GetType().Name == arr[0])
                        result = findObject(str.Remove(0, arr[0].Length), o);
                }
            }
            else
            {
                if(currentObject.GetType().Name == arr[0])
                {
                    return currentObject;
                }
            }
            
            //couldn't find specified object
            return null;
        }

        public static int countOccurencesInString(string text, string search)
        {
            int count = 0, minIndex = text.IndexOf(search, 0);
            while (minIndex != -1)
            {
                minIndex = text.IndexOf(search, minIndex + search.Length);
                count++;
            }
            return count;
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

        private bool isACollection(object obj)
        {
            return isACollection(obj.GetType());
        }

        private bool isACollection(Type t)
        {
            return ((t.Name == "Collection`1") && (t.Namespace == "System.Collections.ObjectModel"));
        }
    }
}

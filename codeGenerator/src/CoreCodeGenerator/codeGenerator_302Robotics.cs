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

                    /// Testing
                    //string str = "This is a test __^mechanismName__, now for the second test __^motor%canId__, now the third test __^motor%canBusName__ test is over";
                    string str = "This is a test __^mechanismName__, now for the second test __^closedLoopControlParameters%pGain__ test is over";

                    string marker = "__";

                    //plus 1 is to account for chunk at beginning before markers
                    //this gets the amount of markers ("__") in a string so that we can split up the string into it's different parts
                    int count = Regex.Matches(str, marker).Count + 1;

                    //will need to dynamically find 3 and 5 like in whiteboard
                    string[] array = str.Split(new string[] {marker}, count, StringSplitOptions.None);

                    string testResultString = "";
                    int timesToRun = 0;

                    #region Check for collection
                    //if we are access a collection, do separate logic for multiple lines
                    if (str.Contains("%"))
                    {
                        Type objType = mech.GetType();

                        PropertyInfo[] propertyInfos = objType.GetProperties();

                        foreach (string s in array)
                        {
                            //checks if we are trying to access a collection
                            if (s.Contains("%"))
                            {
                                //checks if accessing property of mechanism  may not need this if we're always accessing from this mech object
                                if (s.StartsWith("^"))
                                {
                                    string propertyName = s.Trim('^');

                                    //if we are trying to access a collection in some place, don't worry about that for now
                                    propertyName = propertyName.Split(new string[] { "%" }, 2, StringSplitOptions.None)[0];

                                    //find the property after the ^
                                    foreach (PropertyInfo pi in propertyInfos)
                                    {
                                        //if we are on the property that matches what we found from the string
                                        if (pi.Name == propertyName)
                                        {
                                            //check if property is a collection
                                            if (isACollection(pi.GetValue(mech)))
                                            {
                                                int collectionSize = (pi.GetValue(mech) as IList).Count;
                                                //check to make sure if multiple collections are being accessed, only repeat for lowest size to avoid index out of bounds
                                                //the 0 check is to make sure repeatsForCollection is set for the first collection found
                                                if (collectionSize < timesToRun || timesToRun == 0)
                                                    timesToRun = collectionSize;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else //we aren't accessing a collection, so only write one line
                    {
                        timesToRun = 1;
                    }
                    #endregion

                    #region Replace text however many times for a collection or just once
                    //write lines for how many times we need to run
                    for (int i = 0; i < timesToRun; i++)
                    {
                        Type objType = mech.GetType();

                        PropertyInfo[] propertyInfos = objType.GetProperties();
                        //go through each "chunk" that was separated by the markers ("__" in this test case)
                        foreach (string s in array)
                        {
                            //checks if accessing property of mechanism  may not need this if we're always accessing from this mech object
                            if (s.StartsWith("^"))
                            {
                                string propertyName = s.Trim('^');

                                //if we are trying to access a collection in some place, don't worry about that for now
                                if (s.IndexOf("%") != -1)
                                {
                                    int percentCount = Regex.Matches(propertyName, "%").Count + 1;
                                    propertyName = propertyName.Split(new string[] { "%" }, percentCount, StringSplitOptions.None)[0];
                                }

                                //find the property after the ^
                                foreach (PropertyInfo pi in propertyInfos)
                                {
                                    if (pi.Name == propertyName)
                                    {
                                        //check if property is a collection
                                        //we need to hijack testResultString here to have multiple lines for each element of collection
                                        if (isACollection(pi.GetValue(mech)))
                                        {
                                            IList list = (IList)pi.GetValue(mech);
                                            object element = list[i];

                                            Type collectionType = element.GetType();
                                            string collectionProperty = s.Split(new string[] { "%" }, 2, StringSplitOptions.None)[1];

                                            /// TODO: Add functionality to iterate through whole collection, getting all of its properties
                                            /// this can be done similar to closedLoopControlParameters like below
                                            /// Need to get a new propertyinfo of the type of collection we're accessing (motors or closedLoopControlParameters)
                                            /// then iterate through that and print out the value wanted from the string
                                            string value = element.GetType().GetProperty(collectionProperty).GetValue(element).ToString();
                                            testResultString += value;

                                        }
                                        else
                                            testResultString += pi.GetValue(mech).ToString();
                                    }
                                }
                            }
                            else
                            {
                                testResultString += s;
                            }
                        }
                        //make sure we don't put a new line after last element
                        if(i != (timesToRun -1))
                            testResultString += Environment.NewLine;
                    }
                    #endregion

                    Debug.WriteLine(testResultString);

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

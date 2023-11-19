using applicationConfiguration;
using ApplicationData;
using Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeGenerator
{
    internal class MechanismGenerator : baseGenerator
    {
        internal MechanismGenerator(string codeGeneratorVersion, applicationDataConfig theRobotConfiguration, toolConfiguration theToolConfiguration, showMessage displayProgress)
        : base(codeGeneratorVersion, theRobotConfiguration, theToolConfiguration)
        {
            setProgressCallback(displayProgress);
        }

        internal void generate()
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
            return Path.Combine(theToolConfiguration.rootOutputFolder, "mechanisms", mechanismName);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Robot;
using StateData;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

namespace robotConfiguration
{
    public class robotConfig : baseReportingClass
    {
        public robotVariants theRobotVariants = new robotVariants();
        public Dictionary<string, statedata> mechanismControlDefinition;
        public List<string> parameterTypes = new List<string>();

        public void load(string theRobotConfigFullPathFileName)
        {
            try
            {
                string rootRobotConfigFolder = Path.GetDirectoryName(theRobotConfigFullPathFileName);

                addProgress("Loading robot configuration " + theRobotConfigFullPathFileName);
                theRobotVariants = loadRobotConfiguration(theRobotConfigFullPathFileName);

                foreach (robot theRobot in theRobotVariants.robot)
                {
                    theRobot.initialize();

                    ValidationContext context = new ValidationContext(theRobot.pdp);
                    IList<ValidationResult> errors = new List<ValidationResult>();

                    addProgress("Validating Robot with ID " + theRobot.robotID);
                    if (!Validator.TryValidateObject(theRobot.pdp, context, errors, true))
                    {
                        addProgress("Error(s) found ");
                        //todo should the error be "fixed" without user intervention?
                        foreach (ValidationResult result in errors)
                            addProgress(result.ErrorMessage);
                    }
                    else
                        addProgress("Validation passed");
                }
            }
            catch (Exception ex)
            {
                progressCallback(ex.Message);
            }
        }

        public void save(string theRobotConfigFullPathFileName)
        {
            try
            {
                string rootRobotConfigFolder = Path.GetDirectoryName(theRobotConfigFullPathFileName);

                addProgress("Saving robot configuration " + theRobotConfigFullPathFileName);
                saveRobotConfiguration(theRobotConfigFullPathFileName);
            }
            catch (Exception ex)
            {
                progressCallback(ex.Message);
            }
        }

        robotVariants loadRobotConfiguration(string fullPathName)
        {
            robotVariants theRobotVariants;

            var mySerializer = new XmlSerializer(typeof(robotVariants));
            using (var myFileStream = new FileStream(fullPathName, FileMode.Open))
            {
                theRobotVariants = (robotVariants)mySerializer.Deserialize(myFileStream);
                myFileStream.Close();
            }

            for (int m = 0; m < theRobotVariants.mechanism.Count; m++)
            {
                string mechanismFullPath = Path.Combine(Path.GetDirectoryName(fullPathName), theRobotVariants.mechanism[m].name + ".xml");

                addProgress("Loading mechanism configuration " + mechanismFullPath);
                mySerializer = new XmlSerializer(typeof(mechanism));
                using (var myFileStream = new FileStream(mechanismFullPath, FileMode.Open))
                {
                    theRobotVariants.mechanism[m] = (mechanism)mySerializer.Deserialize(myFileStream);
                    myFileStream.Close();
                }
            }

            //try loading any additional mechanisms in cofiguration directory
            string[] files = Directory.GetFiles(Path.GetDirectoryName(fullPathName), "*.xml");
            foreach (string file in files)
            {
                if (theRobotVariants.mechanism.Any(p => p.name == Path.GetFileName(file).Replace(".xml", "")))
                {
                    //if we have previously loaded the mechanism, don't load it again
                    continue;
                }
                else
                {
                    string mechanismFullPath = Path.Combine(Path.GetDirectoryName(fullPathName), file);

                    string tempFile = File.ReadAllText(mechanismFullPath);

                    using (var myFileStream = new FileStream(mechanismFullPath, FileMode.Open))
                    {
                        //ignore configuration files
                        if(!tempFile.Contains("robotVariants") && !tempFile.Contains("toolConfiguration"))
                        {
                            mechanism tempMech = mySerializer.Deserialize(myFileStream) as mechanism;

                            //if we have two versions of a mechanism with the same name, append a nubmer to the end of the newest one
                            int numberOfSameNamedMechs = theRobotVariants.mechanism.Where(p => p.name == tempMech.name).Count();
                            if (numberOfSameNamedMechs > 0)
                            {
                                tempMech.name += numberOfSameNamedMechs;
                            }

                            theRobotVariants.mechanism.Add(tempMech);
                        }
                        myFileStream.Close();
                    }
                }
            }

            foreach (robot theRobot in theRobotVariants.robot)
            {
                foreach (mechanismInstance mi in theRobot.mechanismInstance)
                {
                    MergeMechanismParametersIntoStructure(loadMechanism(fullPathName, mi.mechanism.name), mi.mechanism);
                }
            }

            

            //foreach (robot theRobot in theRobotVariants.robot)
            //{
            //    for (int i = 0; i < theRobot.mechanismInstance.Count; i++)
            //    {
            //        mechanism mech = theRobot.mechanismInstance[i].mechanism;

            //        mySerializer = new XmlSerializer(typeof(mechanism));
            //        string mechanismFullPath = Path.Combine(Path.GetDirectoryName(fullPathName), mech.name + ".xml");

            //        addProgress("Loading mechanism configuration " + mechanismFullPath);
            //        using (var myFileStream = new FileStream(mechanismFullPath, FileMode.Open))
            //        {
            //            mech = (mechanism)mySerializer.Deserialize(myFileStream);
            //        }
            //    }
            //}
            return theRobotVariants;
        }

        private mechanism loadMechanism(string fullPathName, string mechanismName)
        {
            var mySerializer = new XmlSerializer(typeof(mechanism));

            string mechanismFullPath = Path.Combine(Path.GetDirectoryName(fullPathName), mechanismName + ".xml");

            using (var myFileStream = new FileStream(mechanismFullPath, FileMode.Open))
            {
                mechanism m = (mechanism)mySerializer.Deserialize(myFileStream);
                myFileStream.Close();
                return m;                
            }
        }

        private bool isACollection(object obj)
        {
            return isACollection(obj.GetType());
        }

        private bool isACollection(Type t)
        {
            return ((t.Name == "Collection`1") && (t.Namespace == "System.Collections.ObjectModel"));
        }

        bool isAParameterType(string typeName)
        {
            return parameterTypes.Contains(typeName);
        }



        /// <summary>
        /// Merges the structure and default values from structureSource to parametersSource
        /// </summary>
        /// <param name="structureSource"></param>
        /// <param name="parametersSource"></param>
        public void MergeMechanismParametersIntoStructure(object structureSource, object parametersSource)
        {
            if ((structureSource != null) && (parametersSource != null))
            {
                if (isACollection(structureSource))
                {
                    ICollection ics = structureSource as IList;
                    ICollection icp = parametersSource as IList;
                    foreach (var v in ics)
                    {
                        PropertyInfo[] propertyInfos = v.GetType().GetProperties();
                        PropertyInfo pi = propertyInfos.ToList().Find(p => p.Name == "name");
                        if (pi != null)
                        {
                            string structureName = pi.GetValue(v).ToString(); ;

                            foreach (var vParam in icp)
                            {
                                string s = pi.GetValue(vParam).ToString();
                                if ((s != null) && (s == structureName))
                                {
                                    MergeMechanismParametersIntoStructure(v, vParam);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Type objType = structureSource.GetType();

                    PropertyInfo[] propertyInfos = objType.GetProperties();

                    if(isAParameterType(objType.FullName))
                    {
                        PropertyInfo pi = propertyInfos.ToList().Find(p => p.Name == "value");
                        if (pi != null)
                        {
                            pi.SetValue(structureSource, pi.GetValue(parametersSource));
                        }
                    }
                    else if(objType.FullName == "System.String")
                    {

                    }
                    else
                    {
                        foreach (PropertyInfo pi in propertyInfos)
                        {
                            object theStructureObj = pi.GetValue(structureSource);
                            object theParametersObj = pi.GetValue(parametersSource);

                            if ((theStructureObj != null) && (theParametersObj != null) )
                            {
                                MergeMechanismParametersIntoStructure(theStructureObj, theParametersObj);
                            }
                        }
                    }

                    //if (!isAParameterType(objType.FullName) && (objType.FullName != "System.String") && (propertyInfos.Length > 0))
                    //{
                    //    // add its children
                    //    string previousName = "";
                    //    foreach (PropertyInfo pi in propertyInfos)
                    //    {
                    //        object theObj = pi.GetValue(obj);

                    //        //strings have to have some extra handling
                    //        if (pi.PropertyType.FullName == "System.String")
                    //        {
                    //            if (theObj == null)
                    //            {
                    //                theObj = "";
                    //                pi.SetValue(obj, "");
                    //            }
                    //        }

                    //        if (theObj != null)
                    //        {
                    //            if (pi.Name != previousName + "Specified")
                    //            {
                    //                AddNode(tn, theObj, pi.Name);
                    //                previousName = pi.Name;
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    // this means that this is a leaf node
                    //    leafNodeTag lnt = new leafNodeTag(obj.GetType(), nodeName, obj);
                    //    tn.Tag = lnt;
                    //}
                }
            }
        }

        public static mechanism DeepClone(mechanism obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (mechanism)formatter.Deserialize(ms);
            }
        }

        void saveRobotConfiguration(string fullPathName)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.NewLineOnAttributes = true;
            xmlWriterSettings.Indent = true;


            foreach (mechanism mech in theRobotVariants.mechanism)
            {
                string mechanismFullPath = Path.Combine(Path.GetDirectoryName(fullPathName), mech.name + ".xml");

                var mechSerializer = new XmlSerializer(typeof(mechanism));
                XmlWriter mechtw = XmlWriter.Create(mechanismFullPath, xmlWriterSettings);
                mechSerializer.Serialize(mechtw, mech);

                mechtw.Close();
            }

            // after saving the mechanisms into separate files, clear the list of mechanisms, except for the name
            // so that the mechanism xml is blank in the robot config file...will not lead to conflicts when  multiple people change
            // different mechanisms. Restore the list after saving to xml
            List<mechanism> tempList = new List<mechanism>();
            foreach (mechanism mech in theRobotVariants.mechanism)
                tempList.Add(mech);

            theRobotVariants.mechanism.Clear();

            foreach (mechanism mech in tempList)
            {
                mechanism temp = new mechanism();
                temp.name = mech.name;
                theRobotVariants.mechanism.Add(temp);
            }

            var robotSerializer = new XmlSerializer(typeof(robotVariants));
            XmlWriter tw = XmlWriter.Create(fullPathName, xmlWriterSettings);
            robotSerializer.Serialize(tw, theRobotVariants);

            tw.Close();

            theRobotVariants.mechanism.Clear();

            foreach (mechanism mech in tempList)
            {
                theRobotVariants.mechanism.Add(mech);
            }
        }

        statedata loadStateDataConfiguration(string fullPathName)
        {
            var mySerializer = new XmlSerializer(typeof(statedata));
            using (var myFileStream = new FileStream(fullPathName, FileMode.Open))
            {
                return (statedata)mySerializer.Deserialize(myFileStream);
            }
        }
        void saveStateDataConfiguration(string fullPathName, statedata obj)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.NewLineOnAttributes = true;
            xmlWriterSettings.Indent = true;

            var mySerializer = new XmlSerializer(typeof(statedata));
            XmlWriter tw = XmlWriter.Create(fullPathName, xmlWriterSettings);
            mySerializer.Serialize(tw, obj);
            tw.Close();
        }
    }

    public class baseReportingClass
    {
        public delegate void showMessage(string message);

        protected showMessage progressCallback;
        protected void addProgress(string info)
        {
            if (progressCallback != null)
                progressCallback(info);
        }
        public void setProgressCallback(showMessage callback)
        {
            progressCallback = callback;
        }
    }
}

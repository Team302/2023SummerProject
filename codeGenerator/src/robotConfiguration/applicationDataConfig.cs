﻿using ApplicationData;
using DataConfiguration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace applicationConfiguration
{
    public class applicationDataConfig : baseDataConfiguration
    {
        public topLevelAppDataElement theRobotVariants = new topLevelAppDataElement();

        override public void load(string theRobotConfigFullPathFileName)
        {
            try
            {
                string rootRobotConfigFolder = Path.GetDirectoryName(theRobotConfigFullPathFileName);

                addProgress("Loading robot configuration " + theRobotConfigFullPathFileName);
                theRobotVariants = loadRobotConfiguration(theRobotConfigFullPathFileName);

                // switch off validation... maybe switch it on once we understand how it works
                //foreach (applicationData theRobot in theRobotVariants.robot)
                //{
                //    ValidationContext context = new ValidationContext(theRobot.pdp);
                //    IList<ValidationResult> errors = new List<ValidationResult>();

                //    addProgress("Validating Robot with ID " + theRobot.robotID);
                //    if (!Validator.TryValidateObject(theRobot.pdp, context, errors, true))
                //    {
                //        addProgress("Error(s) found ");
                //        //todo should the error be "fixed" without user intervention?
                //        foreach (ValidationResult result in errors)
                //            addProgress(result.ErrorMessage);
                //    }
                //    else
                //        addProgress("Validation passed");
                //}
            }
            catch (Exception ex)
            {
                progressCallback(ex.Message);
            }
        }

        override public void save(string theRobotConfigFullPathFileName)
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

        private topLevelAppDataElement loadRobotConfiguration(string fullPathName)
        {
            topLevelAppDataElement theRobotVariants;

            var mySerializer = new XmlSerializer(typeof(topLevelAppDataElement));
            using (var myFileStream = new FileStream(fullPathName, FileMode.Open))
            {
                theRobotVariants = (topLevelAppDataElement)mySerializer.Deserialize(myFileStream);
            }

            for (int m = 0; m < theRobotVariants.mechanism.Count; m++)
            {
                string mechanismFullPath = Path.Combine(Path.GetDirectoryName(fullPathName), theRobotVariants.mechanism[m].name + ".xml");

                addProgress("Loading mechanism configuration " + mechanismFullPath);
                mySerializer = new XmlSerializer(typeof(mechanism));
                using (var myFileStream = new FileStream(mechanismFullPath, FileMode.Open))
                {
                    theRobotVariants.mechanism[m] = (mechanism)mySerializer.Deserialize(myFileStream);
                }
            }

            //try loading any additional mechanisms in cofiguration directory
            string[] files = Directory.GetFiles(Path.GetDirectoryName(fullPathName), "*.xml");
            foreach (string file in files)
            {
                if (theRobotVariants.mechanism.Any(p => p.name == Path.GetFileNameWithoutExtension(file)))
                {
                    //if we have previously loaded the mechanism, don't load it again
                    continue;
                }
                else
                {
                    string mechanismFullPath = Path.Combine(Path.GetDirectoryName(fullPathName), file);

                    string tempFile = File.ReadAllText(mechanismFullPath);

                    mechanism tempMech;

                    //ignore configuration files
                    if (!tempFile.Contains("topLevelAppDataElement") && !tempFile.Contains("toolConfiguration"))
                    {
                        mySerializer = new XmlSerializer(typeof(mechanism));

                        using (var myFileStream = new FileStream(mechanismFullPath, FileMode.Open))
                        {
                            tempMech = mySerializer.Deserialize(myFileStream) as mechanism;
                        }

                        //if we have two versions of a mechanism with the same name, append a number to the end of the newest one
                        int numberOfSameNamedMechs = theRobotVariants.mechanism.Where(p => p.name == tempMech.name).Count();
                        if (numberOfSameNamedMechs > 0)
                        {
                            tempMech.name += numberOfSameNamedMechs;
                        }

                        theRobotVariants.mechanism.Add(tempMech);
                    }
                }
            }

            foreach (applicationData theRobot in theRobotVariants.robot)
            {
                foreach (mechanismInstance mi in theRobot.mechanismInstance)
                {
                    MergeMechanismParametersIntoStructure(loadMechanism(fullPathName, mi.mechanism.name), mi.mechanism);
                }

                helperFunctions.initializeNullProperties(theRobot, true);
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
                return m;
            }
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

                    //if (isATunableParameterType(objType.FullName))
                    //{
                    //    PropertyInfo pi = propertyInfos.ToList().Find(p => p.Name == "value");
                    //    if (pi != null)
                    //    {
                    //        pi.SetValue(structureSource, pi.GetValue(parametersSource));
                    //    }
                    //}
                    //else if (isAParameterType(objType.FullName))
                    //{
                    //    PropertyInfo pi = propertyInfos.ToList().Find(p => p.Name == "value");
                    //    if (pi != null)
                    //    {
                    //        pi.SetValue(structureSource, pi.GetValue(parametersSource));
                    //    }
                    //}
                    //else
                    if ((objType.FullName == "System.String") || (objType.FullName == "System.DateTime"))
                    {

                    }
                    else
                    {
                        foreach (PropertyInfo pi in propertyInfos)
                        {
                            object theStructureObj = pi.GetValue(structureSource);
                            object theParametersObj = pi.GetValue(parametersSource);

                            if ((theStructureObj != null) && (theParametersObj != null))
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
                    //                AddNode(tn, theObj, pi.Name);
                    //                previousName = pi.Name;
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

        private void saveRobotConfiguration(string fullPathName)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.NewLineOnAttributes = true;
            xmlWriterSettings.Indent = true;


            foreach (mechanism mech in theRobotVariants.mechanism)
            {
                string mechanismFullPath = Path.Combine(Path.GetDirectoryName(fullPathName), mech.name + ".xml");

                addProgress("Writing mechanism file " + mechanismFullPath);
                try
                {
                    using (XmlWriter mechtw = XmlWriter.Create(mechanismFullPath, xmlWriterSettings))
                    {
                        var mechSerializer = new XmlSerializer(typeof(mechanism));
                        mechSerializer.Serialize(mechtw, mech);
                    }
                }
                catch(Exception ex)
                {
                    addProgress("Problem encountered while writing mechanism file " + mechanismFullPath);
                    addProgress(ex.ToString());
                }
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

            var robotSerializer = new XmlSerializer(typeof(topLevelAppDataElement));
            XmlWriter tw = XmlWriter.Create(fullPathName, xmlWriterSettings);
            robotSerializer.Serialize(tw, theRobotVariants);

            tw.Close();

            theRobotVariants.mechanism.Clear();

            foreach (mechanism mech in tempList)
            {
                theRobotVariants.mechanism.Add(mech);
            }
        }
    }
}
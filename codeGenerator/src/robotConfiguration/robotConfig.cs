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

namespace robotConfiguration
{
    public class robotConfig : baseReportingClass
    {
        public robotVariants theRobotVariants = new robotVariants();
        public Dictionary<string, statedata> mechanismControlDefinition;

        public void load(string theRobotConfigFullPathFileName)
        {
            try
            {
                string rootRobotConfigFolder = Path.GetDirectoryName(theRobotConfigFullPathFileName);

                addProgress("Loading robot configuration " + theRobotConfigFullPathFileName);
                theRobotVariants = loadRobotConfiguration(theRobotConfigFullPathFileName);

                foreach (robot theRobot in theRobotVariants.robot)
                {
                    if (theRobot.pdp == null)
                        theRobot.pdp = new pdp();

                    if (theRobot.chassis == null)
                        theRobot.chassis = new chassis();

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
            }

            foreach (robot theRobot in theRobotVariants.robot)
            {
                for (int i = 0; i < theRobot.mechanism.Count; i++)
                {
                    mechanism mech = theRobot.mechanism[i];

                    mySerializer = new XmlSerializer(typeof(mechanism));
                    string mechanismFullPath = Path.Combine(Path.GetDirectoryName(fullPathName), mech.mechanismName + ".xml");

                    addProgress("Loading mechanism configuration " + mechanismFullPath);
                    using (var myFileStream = new FileStream(mechanismFullPath, FileMode.Open))
                    {
                        mech = (mechanism)mySerializer.Deserialize(myFileStream);
                    }
                }
            }
            return theRobotVariants;
        }

        void saveRobotConfiguration(string fullPathName)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.NewLineOnAttributes = true;
            xmlWriterSettings.Indent = true;

            var mySerializer = new XmlSerializer(typeof(robotVariants));
            XmlWriter tw = XmlWriter.Create(fullPathName, xmlWriterSettings);
            mySerializer.Serialize(tw, theRobotVariants);

            tw.Close();
            foreach (robot theRobot in theRobotVariants.robot)
            {
                foreach (mechanism mech in theRobot.mechanism)
                {
                    string mechanismFullPath = Path.Combine(Path.GetDirectoryName(fullPathName), mech.mechanismName + ".xml");

                    mySerializer = new XmlSerializer(typeof(mechanism));
                    tw = XmlWriter.Create(mechanismFullPath, xmlWriterSettings);
                    mySerializer.Serialize(tw, mech);

                    tw.Close();
                }
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

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

namespace robotConfiguration
{
    public class robotConfig : baseReportingClass
    {
        public robot theRobot;
        public Dictionary<string, statedata> mechanismControlDefinition;

        public void load(string theRobotConfigFullPathFileName)
        {
            try
            {
                string rootRobotConfigFolder = Path.GetDirectoryName(theRobotConfigFullPathFileName);

                addProgress("Loading robot configuration " + theRobotConfigFullPathFileName);
                theRobot = loadRobotConfiguration(theRobotConfigFullPathFileName);

                if (theRobot.pdp == null)
                    theRobot.pdp = new pdp(); 
                
                if (theRobot.chassis == null)
                    theRobot.chassis = new chassis();

                mechanismControlDefinition = new Dictionary<string, statedata>();
                if (theRobot.mechanism != null)
                {
                    addProgress("Loading mechanism files...");
                }
            }
            catch(Exception ex)
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
            catch(Exception ex)
            {
                progressCallback(ex.Message);
            }
        }

        robot loadRobotConfiguration(string fullPathName)
        {
            robot theRobot;

            var mySerializer = new XmlSerializer(typeof(robot));
            using (var myFileStream = new FileStream(fullPathName, FileMode.Open))
            {
                 theRobot = (robot)mySerializer.Deserialize(myFileStream);
            }

            for(int i = 0; i < theRobot.mechanism.Count; i++)
            {
                mechanism mech = theRobot.mechanism[i];

                mySerializer = new XmlSerializer(typeof(mechanism));
                string mechanismFullPath = Path.Combine(Path.GetDirectoryName(fullPathName), mech.mechanismName + ".xml");
                
                using (var myFileStream = new FileStream(mechanismFullPath, FileMode.Open))
                {
                    mech = (mechanism)mySerializer.Deserialize(myFileStream);
                }
            }

            return theRobot;
        }

        void saveRobotConfiguration(string fullPathName)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.NewLineOnAttributes = true;
            xmlWriterSettings.Indent = true;

            var mySerializer = new XmlSerializer(typeof(robot));
            XmlWriter tw = XmlWriter.Create(fullPathName, xmlWriterSettings);
            mySerializer.Serialize(tw, theRobot);

            tw.Close();

            foreach (mechanism mech in theRobot.mechanism)
            {
                string mechanismFullPath = Path.Combine(Path.GetDirectoryName(fullPathName), mech.mechanismName + ".xml");

                mySerializer = new XmlSerializer(typeof(mechanism));
                tw = XmlWriter.Create(mechanismFullPath, xmlWriterSettings);
                mySerializer.Serialize(tw, mech);

                tw.Close();
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
            if( progressCallback != null)
                progressCallback(info);
        }
        public void setProgressCallback(showMessage callback)
        {
            progressCallback = callback;
        }
    }
}

using applicationConfiguration;
using ApplicationData;
using Configuration;
using DataConfiguration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeGenerator
{
    internal class MiscellaneousGenerator : baseGenerator
    {
        internal MiscellaneousGenerator(string codeGeneratorVersion, applicationDataConfig theRobotConfiguration, toolConfiguration theToolConfiguration, showMessage displayProgress)
                : base(codeGeneratorVersion, theRobotConfiguration, theToolConfiguration)
        {
            setProgressCallback(displayProgress);
        }
        internal void generate()
        {
            addProgress("Writing general files...");
            generate_RobotElementNames();
            generate_MechanismNames();
        }

        internal void generate_RobotElementNames()
        {
            addProgress("Writing RobotElementNames...");
            codeTemplateFile cdf = theToolConfiguration.getTemplateInfo("RobotElementNames");
            string template = loadTemplate(cdf.templateFilePathName);

            List<string> names = new List<string>();

            generatorContext.clear();
            foreach (mechanism mech in theRobotConfiguration.theRobotVariants.Mechanisms)
            {
                generatorContext.theMechanism = mech;
                names.AddRange(generateMethod(mech, "generateElementNames"));
            }

            generatorContext.clear();
            foreach (applicationData robot in theRobotConfiguration.theRobotVariants.Robots)
            {
                generatorContext.theRobot = robot;
                names.AddRange(generateMethod(robot, "generateElementNames").Distinct().ToList());
            }


            template = template.Replace("$$_ROBOT_ELEMENT_NAMES_ENUMS_$$", utilities.ListToString(names.Distinct().ToList(),",").ToUpper());
            
            copyrightAndGenNoticeAndSave(getOutputFileFullPath(cdf.outputFilePathName), template);
        }

        internal void generate_MechanismNames()
        {
            addProgress("Writing MechanismNames...");
            codeTemplateFile cdf = theToolConfiguration.getTemplateInfo("MechanismTypes");
            string template = loadTemplate(cdf.templateFilePathName);

            List<string> mechNames = new List<string>();
            foreach (applicationData robot in theRobotConfiguration.theRobotVariants.Robots)
            {
                foreach (mechanismInstance mi in robot.mechanismInstances)
                {
                    mechNames.Add(string.Format("{0}", getMechanismInstanceName(mi)));
                }
            }

            template = template.Replace("$$_MECHANISM_NAMES_ENUMS_$$", ListToString(mechNames.Distinct().ToList(), ",").ToUpper());

            copyrightAndGenNoticeAndSave(getOutputFileFullPath(cdf.outputFilePathName), template);
        }

        internal string getMechanismElementName(MotorController mc)
        {
            return string.Format("{0}", ToUnderscoreCase(mc.name));
        }

        internal string getMechanismInstanceName(mechanismInstance mi)
        {
            return string.Format("{0}", ToUnderscoreCase(mi.name));
        }
    }
}

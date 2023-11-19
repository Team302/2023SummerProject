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
            codeTemplateFile cdf = theToolConfiguration.getTemplateInfo("RobotElementNames");
            string template = loadTemplate(cdf.templateFilePathName);

            StringBuilder sb = new StringBuilder();

            foreach (applicationData robot in theRobotConfiguration.theRobotVariants.Robots)
            {
                foreach (mechanismInstance mi in robot.mechanismInstances)
                {
                    foreach (MotorController mc in mi.mechanism.MotorControllers)
                        sb.AppendLine(getRobotElementName(mi, mc));

                }
            }

            template = template.Replace("$$_ROBOT_ELEMENT_NAMES_ENUMS_$$", sb.ToString().ToUpper());

            copyrightAndGenNoticeAndSave(getOutputFileFullPath(cdf.outputFilePathName), template);
        }

        internal string getRobotElementName(mechanismInstance mi, MotorController mc)
        {
            return string.Format("{0}_{1},", ToUnderscoreCase(mi.name), ToUnderscoreCase(mc.name));
        }

    }
}

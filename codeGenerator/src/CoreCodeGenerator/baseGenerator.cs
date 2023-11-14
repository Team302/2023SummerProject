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
    internal class baseGenerator : baseReportingClass
    {
        internal string codeGeneratorVersion = "";
        internal applicationDataConfig theRobotConfiguration = new applicationDataConfig();
        internal toolConfiguration theToolConfiguration = new toolConfiguration();

        internal baseGenerator(string codeGeneratorVersion, applicationDataConfig theRobotConfiguration, toolConfiguration theToolConfiguration)
        {
            this.codeGeneratorVersion = codeGeneratorVersion;
            this.theRobotConfiguration = theRobotConfiguration;
            this.theToolConfiguration = theToolConfiguration;
        }

        internal string getTemplateFullPath(string templatePath)
        {
            if (Path.IsPathRooted(templatePath))
            {
                return templatePath;
            }

            return Path.Combine(Path.GetDirectoryName(theToolConfiguration.configurationFullPath), templatePath);
        }

        internal string getOutputFileFullPath(string filePath)
        {
            if (Path.IsPathRooted(filePath))
            {
                return filePath;
            }

            return Path.Combine(theToolConfiguration.rootOutputFolder, filePath);
        }

        internal string loadTemplate(string templatePath)
        {
            return File.ReadAllText(getTemplateFullPath(templatePath));
        }

        internal void copyrightAndGenNoticeAndSave(string outputFilePathName, string contents)
        {
            contents = contents.Replace("$$_Copyright_$$", theToolConfiguration.CopyrightNotice.Trim());
            contents = contents.Replace("$$_GEN_NOTICE_$$", getGenerationInfo());

            contents = astyle.AStyleCaller.beautify(contents, null);

            string outputFullFilePathName = getOutputFileFullPath(outputFilePathName);

            string currentText = "";
            string contentsWithoutGenInfo = "";
            bool writeFile = true;
            if (File.Exists(outputFullFilePathName))
            {
                currentText = File.ReadAllText(outputFullFilePathName);

                currentText = removeGenerationInfo(currentText);
                contentsWithoutGenInfo = removeGenerationInfo(contents);

                if (currentText == contentsWithoutGenInfo)
                    writeFile = false;
            }

            if (writeFile)
            {
                File.WriteAllText(outputFullFilePathName, contents);
                addProgress("Wrote " + outputFullFilePathName);
            }
            else
                addProgress("File content has not changed " + outputFullFilePathName);
        }

        internal string removeGenerationInfo(string input)
        {
            int index = input.IndexOf("// Generated on");
            if (index >= 0)
            {
                int nextNewLineIndex = input.IndexOf("\n", index);
                return input.Remove(index, nextNewLineIndex - index);
            }

            return input;
        }

        internal string getGenerationInfo()
        {
            string genNotice = theToolConfiguration.GenerationNotice.Trim().Replace("$CODE_GENERATOR_VERSION$", codeGeneratorVersion);
            genNotice = genNotice.Replace("$GENERATION_DATE$", DateTime.Now.ToLongDateString());
            genNotice = genNotice.Replace("$GENERATION_TIME$", DateTime.Now.ToLongTimeString());

            return genNotice;
        }

        internal string ToUnderscoreCase(string str)
        {
            if (str.Contains("_"))
                return str;

            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }
}
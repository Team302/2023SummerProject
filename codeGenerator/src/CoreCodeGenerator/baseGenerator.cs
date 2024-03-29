﻿using applicationConfiguration;
using ApplicationData;
using Configuration;
using DataConfiguration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeGenerator
{
    internal class baseGenerator : baseReportingClass
    {
        internal string codeGeneratorVersion = "";
        internal applicationDataConfig theRobotConfiguration = new applicationDataConfig();
        internal toolConfiguration theToolConfiguration = new toolConfiguration();
        public bool cleanMode { get; private set; }
        protected bool cleanDecoratorModFolders { get; set; } = false;

        internal baseGenerator(string codeGeneratorVersion, applicationDataConfig theRobotConfiguration, toolConfiguration theToolConfiguration, bool cleanMode)
        {
            initialize(codeGeneratorVersion, theRobotConfiguration, theToolConfiguration, cleanMode, false);
        }
        internal baseGenerator(string codeGeneratorVersion, applicationDataConfig theRobotConfiguration, toolConfiguration theToolConfiguration, bool cleanMode, bool cleanDecoratorModFolders)
        {
            initialize(codeGeneratorVersion, theRobotConfiguration, theToolConfiguration, cleanMode, cleanDecoratorModFolders);
        }

        private void initialize(string codeGeneratorVersion, applicationDataConfig theRobotConfiguration, toolConfiguration theToolConfiguration, bool cleanMode, bool cleanDecoratorModFolders)
        {
            this.codeGeneratorVersion = codeGeneratorVersion;
            this.theRobotConfiguration = theRobotConfiguration;
            this.theToolConfiguration = theToolConfiguration;
            generatorContext.theGeneratorConfig = theToolConfiguration;
            this.cleanMode = cleanMode;
            this.cleanDecoratorModFolders = cleanDecoratorModFolders;
        }
        protected string ListToString(List<string> list, string delimeter)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = list[i].Trim();
                if (!string.IsNullOrWhiteSpace(list[i]))
                    sb.AppendLine(string.Format("{0}{1}", list[i], delimeter));
                else
                    sb.AppendLine(string.Format("{0}", list[i]));
            }

            return sb.ToString();
        }
        protected string ListToString(List<string> list)
        {
            return ListToString(list, "");
        }

        internal List<string> generateMethod(object obj, string methodName)
        {
            MethodInfo mi = obj.GetType().GetMethod("generate");
            if (mi != null)
            {
                object[] parameters = new object[] { methodName };
                return (List<string>)mi.Invoke(obj, parameters);
            }

            return new List<string> { "" };
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
            copyrightAndGenNoticeAndSave(outputFilePathName, contents, false);
        }

        internal void copyrightAndGenNoticeAndSave(string outputFilePathName, string contents, bool doNotWriteIfExists)
        {
            string copyright = theToolConfiguration.CopyrightNotice.Trim();
            copyright = copyright.Replace(Environment.NewLine, "\n").Replace("\n", Environment.NewLine);

            string generationString = getGenerationInfo();
            generationString = generationString.Replace(Environment.NewLine, "\n").Replace("\n", Environment.NewLine);

            contents = contents.Replace("$$_COPYRIGHT_$$", copyright);
            contents = contents.Replace("$$_GEN_NOTICE_$$", generationString);
            contents = theToolConfiguration.EditorFormattingDisable.Trim() + Environment.NewLine + contents.TrimStart();

            contents = astyle.AStyleCaller.beautify(contents, null);

            string outputFullFilePathName = getOutputFileFullPath(outputFilePathName);

            string currentText = "";
            string contentsWithoutGenInfo = "";
            bool writeFile = true;
            if (File.Exists(outputFullFilePathName))
            {
                if (doNotWriteIfExists)
                {
                    writeFile = false;
                }
                else
                {
                    currentText = File.ReadAllText(outputFullFilePathName);

                    currentText = removeGenerationInfo(currentText);
                    contentsWithoutGenInfo = removeGenerationInfo(contents);

                    if (currentText == contentsWithoutGenInfo)
                        writeFile = false;
                }
            }

            if (cleanMode)
            {
                bool erase = true;

                if (Path.GetDirectoryName(outputFilePathName).Contains("decoratormods"))
                    erase = cleanDecoratorModFolders;

                if (erase)
                {
                    addProgress("Erasing " + outputFullFilePathName);
                    File.Delete(outputFullFilePathName);
                }
            }
            else
            {
                if (writeFile)
                {
                    File.WriteAllText(outputFullFilePathName, contents);
                    addProgress("\tWrote " + outputFullFilePathName);
                }
                else
                    addProgress("\tFile content has not changed or is a decoratorMod " + outputFullFilePathName);
            }
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

            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) && char.IsLower(str[i - 1]) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }
}
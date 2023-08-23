using Configuration;
using CoreCodeGenerator;
using robotConfiguration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using Robot;
using StateData;
using System.Collections;
using System.Collections.ObjectModel;
using System.Web;
using System.Drawing;
using System.Deployment.Application;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;

namespace FRCrobotCodeGen302
{
    public partial class MainForm : Form
    {
        toolConfiguration generatorConfig = new toolConfiguration();
        robotConfig theRobotConfiguration = new robotConfig();
        codeGenerator_302Robotics codeGenerator = new codeGenerator_302Robotics();
        bool needsSaving = false;
        bool loadRobotConfig = false;
        readonly string configurationCacheFile = Path.GetTempPath() + "DragonsCodeGeneratorCache.txt";

        const int treeIconIndex_lockedPadlock = 0;
        const int treeIconIndex_unlockedPadlock = 1;
        const int treeIconIndex_gear = 2;
        const int treeIconIndex_wrench = 3;

        public MainForm()
        {
            InitializeComponent();
            codeGenerator.setProgressCallback(addProgress);
            theRobotConfiguration.setProgressCallback(addProgress);
            clearNeedsSaving();

            splitContainer1.SplitterDistance = splitContainer1.Width - 180;

            valueComboBox.Location = valueNumericUpDown.Location;
            valueTextBox.Location = valueNumericUpDown.Location;

            this.Text += " Version " + ProductVersion;

            //try to load cached configuration.xml
            addProgress("Trying to load cached configuration.xml");
            try
            {
                if (File.Exists(configurationCacheFile))
                {
                    configurationFilePathNameTextBox.Text = File.ReadAllText(configurationCacheFile);
                    loadConfiguration(configurationFilePathNameTextBox.Text);
                    addProgress("Loaded cached configuration.xml");
                    robotConfigurationFileComboBox_TextChanged(null, null);
                }
                else
                {
                    addProgress("Cached configuration.xml does not exist, robot configuration will not be automatically loaded");
                }
            }
            catch (Exception ex)
            {
                addProgress("Issue encountered while loading the cached generator configuration file\r\n" + ex.ToString());
            }

            robotTreeView.ImageList = treeViewIcons;
        }

        private void addProgress(string info)
        {
            progressTextBox.AppendText(info + Environment.NewLine);
        }

        private string getTreeNodeDisplayName(object parentObject, object obj, string nodeName)
        {
            if (parentObject == null)
            {

            }
            else if (isACollection(obj))
            {
                if (!nodeName.EndsWith("s"))
                    nodeName += "s";
            }
            else
            {
                Type objType = obj.GetType();
                PropertyInfo[] properties = objType.GetProperties();

                string nodeValueString = "";
                if ((properties.Length == 0) || (objType.FullName == "System.String"))
                {
                    if (parentObject != null)
                    {
                        PropertyInfo prop = parentObject.GetType().GetProperty(nodeName, BindingFlags.Public | BindingFlags.Instance);
                        object value = null;
                        if (null != prop)
                        {
                            value = prop.GetValue(parentObject);
                            nodeValueString = value.ToString();
                        }
                    }
                }
                else if (isATunableParameterType(objType.FullName))
                {
                    object value = null;
                    value = properties[0].GetValue(obj);
                    nodeValueString = value.ToString();
                }
                else if (isAParameterType(objType.FullName))
                {
                    object value = null;
                    value = properties[0].GetValue(obj);
                    nodeValueString = value.ToString();
                }
                else
                {
                    nodeName = "";
                    foreach (string s in generatorConfig.treeviewParentNameExtensions)
                    {
                        PropertyInfo propertyInfo = properties.ToList().Find(p => p.Name == s);
                        if (propertyInfo != null)
                        {
                            if (propertyInfo.Name == "pwmId")
                            {
                                nodeName += "ID: " + propertyInfo.GetValue(obj).ToString() + ", ";
                            }
                            else if (propertyInfo.Name == "canId")
                            {
                                if ((propertyInfo.GetValue(obj) as Robot.CAN_ID) != null)
                                    nodeName += "ID: " + (propertyInfo.GetValue(obj) as Robot.CAN_ID).value.ToString() + ", ";
                            }
                            else
                            {
                                if (propertyInfo.GetValue(obj) != null)
                                {
                                    nodeName += propertyInfo.GetValue(obj).ToString() + ", ";
                                }
                                else
                                    nodeName += "UNKOWN_, ";
                            }
                        }
                    }

                    nodeName = nodeName.Trim();
                    nodeName = nodeName.Trim(',');
                    nodeName = nodeName.Trim();
                }

                if (String.IsNullOrEmpty(nodeName))
                    nodeName = objType.Name;

                if (objType == typeof(robot))
                {
                    robot tempBot = (robot)obj;
                    nodeName = "Robot #" + tempBot.robotID;
                }

                nodeName = getTreeNodeDisplayName(nodeValueString, nodeName);
            }

            return nodeName;
        }
        private string getTreeNodeDisplayName(string nodeValueString, string nodeName)
        {
            nodeValueString = nodeValueString.Trim();
            nodeValueString = nodeValueString.Trim(',');
            nodeValueString = nodeValueString.Trim();

            if (!string.IsNullOrEmpty(nodeValueString))
                nodeName += " (" + nodeValueString + ")";

            return nodeName;
        }


        private TreeNode AddNode(TreeNode parent, object obj, string nodeName)
        {
            // add this object to the tree
            string extendedNodeName = getTreeNodeDisplayName(parent == null ? null : parent.Tag, obj, nodeName);

            TreeNode tn = null;
            if (parent == null)
            {
                if (isACollection(obj))
                {
                    ICollection ic = obj as ICollection;
                    if (ic.Count > 0)
                        tn = robotTreeView.Nodes.Add(extendedNodeName);
                }
                else
                    tn = robotTreeView.Nodes.Add(extendedNodeName);
            }
            else
            {
                if (isACollection(obj))
                {
                    ICollection ic = obj as ICollection;
                    if (ic.Count > 0)
                        tn = parent.Nodes.Add(extendedNodeName);
                }
                else
                    tn = parent.Nodes.Add(extendedNodeName);
            }

            if (tn != null)
            {
                tn.Tag = obj;

                // if it is a collection, add an entry for each item
                if (isACollection(obj))
                {
                    ICollection ic = obj as ICollection;
                    if (ic.Count > 0)
                    {
                        int index = 0;
                        foreach (var v in ic)
                        {
                            AddNode(tn, v, v.GetType().Name + index);

                            index++;
                        }
                    }
                }
                else
                {
                    Type objType = obj.GetType();

                    PropertyInfo[] propertyInfos = objType.GetProperties();

                    if (obj is mechanismInstance)
                    {
                        ((mechanismInstance)obj).theTreeNode = tn;
                    }
                    else if (obj is mechanism)
                    {
                        ((mechanism)obj).theTreeNode = tn;
                    }

                    if (!isATunableParameterType(objType.FullName) && !isAParameterType(objType.FullName) && (objType.FullName != "System.String") && (propertyInfos.Length > 0))
                    {
                        // add its children
                        string previousName = "";
                        foreach (PropertyInfo pi in propertyInfos)
                        {
                            object theObj = pi.GetValue(obj);

                            //strings have to have some extra handling
                            if (pi.PropertyType.FullName == "System.String")
                            {
                                if (theObj == null)
                                {
                                    theObj = "";
                                    pi.SetValue(obj, "");
                                }
                            }

                            if (theObj != null)
                            {
                                if (pi.Name != previousName + "Specified")
                                {
                                    AddNode(tn, theObj, pi.Name);
                                    previousName = pi.Name;
                                }
                            }
                        }
                    }
                    else
                    {
                        // this means that this is a leaf node
                        leafNodeTag lnt = new leafNodeTag(obj.GetType(), nodeName, obj);
                        tn.Tag = lnt;

                        int imageIndex = treeIconIndex_unlockedPadlock;
                        if (isAParameterType(objType.FullName))
                            imageIndex = treeIconIndex_gear;
                        else if (isATunableParameterType(objType.FullName))
                            imageIndex = treeIconIndex_wrench;
                        else if (isPartOfAMechanismInaMechInstance(tn))
                            imageIndex = treeIconIndex_lockedPadlock;

                        tn.ImageIndex = imageIndex;
                        tn.SelectedImageIndex = imageIndex;


                    }
                }
            }

            return tn;
        }

        private void populateTree(robotConfig myRobot)
        {
            robotTreeView.Nodes.Clear();
            AddNode(null, myRobot.theRobotVariants, "Robot Variant");
            if (myRobot.theRobotVariants.robot.Count > 0)
                robotTreeView.Nodes[0].Expand();
        }

        private void repopulateMechanisms(mechanismInstance theMechanismInstance)
        {


        }

        bool isATunableParameterType(string typeName)
        {
            return generatorConfig.tunableParameterTypes.Contains(typeName);
        }

        bool isAParameterType(string typeName)
        {
            return generatorConfig.parameterTypes.Contains(typeName);
        }

        public void loadGeneratorConfig(string configurationFullPathName)
        {
            try
            {
                generatorConfig = (toolConfiguration)generatorConfig.deserialize(configurationFullPathName);
                if (generatorConfig.robotConfigurations.Count == 0)
                {
                    generatorConfig.robotConfigurations = new List<string>();
                    if (!string.IsNullOrEmpty(generatorConfig.robotConfiguration.Trim()))
                        generatorConfig.robotConfigurations.Add(generatorConfig.robotConfiguration.Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot load the generator configuration. " + ex.Message);
            }
        }

        public void saveGeneratorConfig(string configurationFullPathName)
        {
            try
            {
                generatorConfig.serialize(configurationFullPathName);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot save the generator configuration. " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                codeGenerator.generate(theRobotConfiguration, generatorConfig);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong. See below. \r\n\r\n" + ex.Message, "Code generator error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            var mySerializer = new XmlSerializer(typeof(robot));
            // To read the file, create a FileStream.
            var myFileStream = new FileStream(@"C:\GitRepos\2023SummerProject\codeGenerator\src\robotExample.xml", FileMode.Create);
            robot myRobot = new robot();
            // Call the Deserialize method and cast to the object type.
            mySerializer.Serialize(myFileStream, myRobot);

            mySerializer = new XmlSerializer(typeof(statedata));
            // To read the file, create a FileStream.
            myFileStream = new FileStream(@"C:\GitRepos\2023SummerProject\codeGenerator\src\mechExample.xml", FileMode.Create);
            statedata myStates = new statedata();
            // Call the Deserialize method and cast to the object type.
            mySerializer.Serialize(myFileStream, myStates);
        }

        private void configurationBrowseButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        configurationFilePathNameTextBox.Text = dlg.FileName;

                        loadConfiguration(configurationFilePathNameTextBox.Text);

                        //now that generator config has loaded succesfully, save to a temp file to save the desired config for future uses

                        File.WriteAllText(configurationCacheFile, configurationFilePathNameTextBox.Text);
                        addProgress("Wrote cached configuration.xml to: " + configurationCacheFile);
                    }
                }
            }
            catch (Exception ex)
            {
                addProgress("Issue encountered while loading the generator configuration file\r\n" + ex.ToString());
            }

            robotConfigurationFileComboBox_TextChanged(null, null);
        }

        private void loadConfiguration(string filePathName)
        {
            addProgress("Loading the generator configuration file " + filePathName);
            loadGeneratorConfig(filePathName);
            addProgress("Configuration file loaded.");

            loadRobotConfig = false;
            #region Load the Combobox with the robot configuration file list and select the first one
            robotConfigurationFileComboBox.Items.Clear();
            foreach (string f in generatorConfig.robotConfigurations)
            {
                string fullfilePath = Path.Combine(Path.GetDirectoryName(filePathName), f);
                fullfilePath = Path.GetFullPath(fullfilePath);
                robotConfigurationFileComboBox.Items.Add(fullfilePath);
            }
            if (robotConfigurationFileComboBox.Items.Count > 0)
                robotConfigurationFileComboBox.SelectedIndex = 0;
            #endregion

            generatorConfig.rootOutputFolder = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(filePathName), generatorConfig.rootOutputFolder));
            generatorConfig.robotConfiguration = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(filePathName), robotConfigurationFileComboBox.Text));
            loadRobotConfig = true;
        }

        private void robotConfigurationFileComboBox_TextChanged(object sender, EventArgs e)
        {
            if (loadRobotConfig)
            {
                try
                {
                    generatorConfig.robotConfiguration = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(configurationFilePathNameTextBox.Text), robotConfigurationFileComboBox.Text));

                    try
                    {
                        theRobotConfiguration.parameterTypes = generatorConfig.parameterTypes;
                        theRobotConfiguration.tunableParameterTypes = generatorConfig.tunableParameterTypes;
                        theRobotConfiguration.load(generatorConfig.robotConfiguration);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Issue encountered while loading the robot configuration file\r\n" + ex.ToString());
                    }

                    try
                    {
                        addProgress("Populating the robot configuration tree view.");
                        populateTree(theRobotConfiguration);
                        addProgress("... Tree view populated.");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Issue encountered while populating the robot configuration tree view\r\n" + ex.ToString());
                    }

                    configuredOutputFolderLabel.Text = generatorConfig.rootOutputFolder;

                    deleteTreeElementButton.Enabled = false;
                    clearNeedsSaving();
                }
                catch (Exception ex)
                {
                    addProgress(ex.ToString());
                }
            }
        }

        void setNeedsSaving()
        {
            needsSaving = true;
            saveConfigBbutton.Enabled = needsSaving;
        }

        void clearNeedsSaving()
        {
            needsSaving = false;
            saveConfigBbutton.Enabled = needsSaving;
        }

        List<robotElementType> getEmptyPossibleCollectionSubTypes(object obj)
        {
            List<robotElementType> types = new List<robotElementType>();


            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (isACollection(propertyInfo.PropertyType))
                {
                    ICollection ic = propertyInfo.GetValue(obj) as ICollection;
                    if (ic.Count == 0)
                        types.Add(new robotElementType(propertyInfo.PropertyType));
                }
                else if (propertyInfo.PropertyType == typeof(mechanismInstance))
                {
                    types.Add(new robotElementType(propertyInfo.PropertyType));
                }
                else if ((!propertyInfo.Name.EndsWith("Specified")) && (!propertyInfo.PropertyType.FullName.StartsWith("System.")))
                {
                    if (!isACollection(obj))
                    {
                        if (propertyInfo.GetValue(obj, null) == null)
                            types.Add(new robotElementType(propertyInfo.PropertyType));
                    }
                }
            }
            return types;
        }

        void hideAllValueEntryBoxes()
        {
            bool visible = false;

            valueComboBox.Visible = visible;
            valueNumericUpDown.Visible = visible;
            valueTextBox.Visible = visible;
        }
        void showValueComboBox()
        {
            hideAllValueEntryBoxes();
            valueComboBox.Visible = true;
        }

        void showValueNumericUpDown()
        {
            hideAllValueEntryBoxes();
            valueNumericUpDown.Visible = true;
        }

        void showValueTextBox()
        {
            hideAllValueEntryBoxes();
            valueTextBox.Visible = true;
        }


        TreeNode lastSelectedValueNode = null;
        TreeNode lastSelectedArrayNode = null;
        bool enableCallback = false;
        List<robotElementType> theCurrentElementPossibilities = new List<robotElementType>();
        private void robotTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            valueTextBox.Visible = false;
            valueComboBox.Visible = false;
            valueNumericUpDown.Visible = false;
            addTreeElementButton.Enabled = false;

            lastSelectedArrayNode = null;
            lastSelectedValueNode = null;

            bool isInaMechanismInstance = isPartOfAMechanismInaMechInstance(e.Node);
            deleteTreeElementButton.Enabled = isDeletable(e.Node) && !isInaMechanismInstance;

            if (e.Node.Tag != null)
            {
                bool visible_And_or_Enabled = false;

                theCurrentElementPossibilities = getEmptyPossibleCollectionSubTypes(e.Node.Tag);
                if ((theCurrentElementPossibilities.Count > 0) && (!isPartOfAMechanismInaMechInstance(e.Node)))
                {
                    visible_And_or_Enabled = true;

                    robotElementCheckedListBox.Items.Clear();
                    foreach (robotElementType t in theCurrentElementPossibilities)
                    {
                        Type elementType;
                        if (isACollection(t.t))
                            elementType = t.t.GetGenericArguments().Single();
                        else
                            elementType = t.t;

                        // Add the defined mechanisms as choices to add to a robot variant
                        if (elementType.Equals((new mechanismInstance()).GetType()))
                        {
                            foreach (mechanism m in theRobotConfiguration.theRobotVariants.mechanism)
                            {
                                robotElementCheckedListBox.Items.Add(m);
                            }
                        }
                        else
                        {
                            robotElementCheckedListBox.Items.Add(t);
                        }
                    }
                }

                robotElementCheckedListBox.Visible = visible_And_or_Enabled;
                addRobotElementLabel.Visible = visible_And_or_Enabled;
                addTreeElementButton.Enabled = visible_And_or_Enabled;


                if (isACollection(e.Node.Tag))
                {
                    lastSelectedArrayNode = e.Node;
                    addTreeElementButton.Enabled = !isInaMechanismInstance;
                }
                /*
                else if ((e.Node.Parent!=null) && (e.Node.Parent.Tag is robot))
                {
                    // do nothing
                }   */
                else if (e.Node.GetNodeCount(false) == 0)
                {
                    lastSelectedValueNode = e.Node;

                    leafNodeTag lnt = (leafNodeTag)(e.Node.Tag);

                    object value = null;
                    PropertyInfo prop = null;
                    bool allowEdit = false;
                    if (isATunableParameterType(lnt.type.FullName) || isAParameterType(lnt.type.FullName))
                    {
                        prop = ((leafNodeTag)lastSelectedValueNode.Tag).type.GetProperty("value", BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop)
                        {
                            value = prop.GetValue(((leafNodeTag)lastSelectedValueNode.Tag).obj);
                        }
                        allowEdit = isInaMechanismInstance;
                    }
                    else
                    {
                        prop = lastSelectedValueNode.Parent.Tag.GetType().GetProperty(lnt.name, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop)
                        {
                            value = prop.GetValue(lastSelectedValueNode.Parent.Tag);
                        }


                        allowEdit = !isInaMechanismInstance;
                    }

                    if (allowEdit)
                    {
                        enableCallback = false;
                        if (lnt.type.IsEnum)
                        {
                            showValueComboBox();
                            valueComboBox.Items.Clear();

                            string[] enumList = Enum.GetNames(lnt.type);
                            foreach (string en in enumList)
                                valueComboBox.Items.Add(en);

                            valueComboBox.SelectedIndex = valueComboBox.FindStringExact(value.ToString());
                        }
                        else if (value is uint || value is UInt32)
                        {
                            RangeAttribute ra = prop.GetCustomAttribute<RangeAttribute>();
                            if (ra == null)
                            {
                                valueNumericUpDown.Minimum = 0;
                                valueNumericUpDown.Maximum = 5000;
                            }
                            else
                            {
                                valueNumericUpDown.Minimum = Convert.ToInt32(ra.Minimum);
                                valueNumericUpDown.Maximum = Convert.ToInt32(ra.Maximum);
                            }


                            valueNumericUpDown.DecimalPlaces = 0;
                            valueNumericUpDown.Value = (uint)value;
                            showValueNumericUpDown();
                        }
                        else if (value is double)
                        {
                            RangeAttribute ra = prop.GetCustomAttribute<RangeAttribute>();
                            if (ra == null)
                            {
                                valueNumericUpDown.Minimum = Decimal.MinValue;
                                valueNumericUpDown.Maximum = Decimal.MaxValue;
                            }
                            else
                            {
                                valueNumericUpDown.Minimum = Convert.ToDecimal(ra.Minimum);
                                valueNumericUpDown.Maximum = Convert.ToDecimal(ra.Maximum);
                            }

                            valueNumericUpDown.DecimalPlaces = 5;
                            valueNumericUpDown.Value = Convert.ToDecimal(value);
                            showValueNumericUpDown();
                        }
                        else if (lastSelectedValueNode.Text == "controlFile")
                        {
                            showValueComboBox();
                            valueComboBox.Items.Clear();

                            string stateDataFilesPath = Path.Combine(Path.GetDirectoryName(generatorConfig.robotConfiguration), "states");

                            string[] files = Directory.GetFiles(stateDataFilesPath, "*.xml");
                            foreach (string f in files)
                                valueComboBox.Items.Add(Path.GetFileName(f));

                            valueComboBox.SelectedIndex = valueComboBox.FindStringExact(value.ToString());
                        }
                        else
                        {
                            showValueTextBox();
                            valueTextBox.Text = value.ToString();
                        }
                        enableCallback = true;
                    }
                }
                else
                {
                    lastSelectedValueNode = e.Node;
                }
            }
        }

        #region handle the events when values are changed
        private void valueComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (enableCallback)
            {
                if (lastSelectedValueNode != null)
                {
                    try
                    {
                        leafNodeTag lnt = (leafNodeTag)(lastSelectedValueNode.Tag);

                        PropertyInfo prop = lastSelectedValueNode.Parent.Tag.GetType().GetProperty(lnt.name, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            if (lastSelectedValueNode.Text == "controlFile")
                                prop.SetValue(lastSelectedValueNode.Parent.Tag, valueComboBox.Text);
                            else
                            {
                                prop.SetValue(lastSelectedValueNode.Parent.Tag, Enum.Parse(lnt.type, valueComboBox.Text));
                                //  lastSelectedValueNode.Parent.Text = getTreeNodeDisplayName(null, lastSelectedValueNode.Parent.Tag, lastSelectedValueNode.Parent.Tag.GetType().Name);
                            }

                            lastSelectedValueNode.Text = getTreeNodeDisplayName(valueComboBox.Text, lnt.name);

                            if (lastSelectedValueNode.Parent != null)
                            {
                                if (generatorConfig.treeviewParentNameExtensions.IndexOf(lnt.name) != -1)
                                    lastSelectedValueNode.Parent.Text = getTreeNodeDisplayName(lastSelectedValueNode.Parent.Parent.Tag, lastSelectedValueNode.Parent.Tag, valueTextBox.Text);
                            }

                            mechanism theMechanism;
                            if (isPartOfAMechanismTemplate(lastSelectedValueNode, out theMechanism))
                                updateMechInstancesFromMechTemplate(theMechanism);

                            setNeedsSaving();
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Failed to set " + lastSelectedValueNode.Text + " to " + valueComboBox.Text);
                    }
                }
            }
        }

        private void valueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (enableCallback)
            {
                if (lastSelectedValueNode != null)
                {
                    try
                    {
                        leafNodeTag lnt = (leafNodeTag)(lastSelectedValueNode.Tag);

                        Type t = lastSelectedValueNode.Tag.GetType();
                        PropertyInfo prop = lastSelectedValueNode.Parent.Tag.GetType().GetProperty(lnt.name, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            prop.SetValue(lastSelectedValueNode.Parent.Tag, valueTextBox.Text);
                        }

                        lastSelectedValueNode.Text = getTreeNodeDisplayName(valueTextBox.Text, lnt.name);

                        if (lastSelectedValueNode.Parent != null)
                        {
                            if (generatorConfig.treeviewParentNameExtensions.IndexOf(lnt.name) != -1)
                                lastSelectedValueNode.Parent.Text = getTreeNodeDisplayName(lastSelectedValueNode.Parent.Parent.Tag, lastSelectedValueNode.Parent.Tag, valueTextBox.Text);
                        }


                        mechanism theMechanism;
                        if (isPartOfAMechanismTemplate(lastSelectedValueNode, out theMechanism))
                            updateMechInstancesFromMechTemplate(theMechanism);

                        setNeedsSaving();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Failed to set " + lastSelectedValueNode.Text + " to " + valueTextBox.Text);
                    }
                }
            }
        }
        private void valueNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (enableCallback)
            {
                if (lastSelectedValueNode != null)
                {
                    try
                    {
                        leafNodeTag lnt = (leafNodeTag)(lastSelectedValueNode.Tag);

                        PropertyInfo prop;
                        if (isATunableParameterType(lnt.type.FullName) || isAParameterType(lnt.type.FullName))
                        {
                            prop = lnt.type.GetProperty("value", BindingFlags.Public | BindingFlags.Instance);
                            if (null != prop && prop.CanWrite)
                            {
                                if (prop.PropertyType.Name == "UInt")
                                    prop.SetValue(lnt.obj, (uint)valueNumericUpDown.Value);
                                else if (prop.PropertyType.Name == "UInt32")
                                    prop.SetValue(lnt.obj, (UInt32)valueNumericUpDown.Value);
                                else if (prop.PropertyType.Name == "Double")
                                    prop.SetValue(lnt.obj, (double)valueNumericUpDown.Value);
                            }
                        }
                        else
                        {
                            Type t = lastSelectedValueNode.Tag.GetType();
                            prop = lastSelectedValueNode.Parent.Tag.GetType().GetProperty(lnt.name, BindingFlags.Public | BindingFlags.Instance);
                            if (null != prop && prop.CanWrite)
                            {
                                if (lnt.obj is uint)
                                    prop.SetValue(lastSelectedValueNode.Parent.Tag, (uint)valueNumericUpDown.Value);
                                else if (lnt.obj is double)
                                    prop.SetValue(lastSelectedValueNode.Parent.Tag, (double)valueNumericUpDown.Value);
                            }
                        }


                        lastSelectedValueNode.Text = getTreeNodeDisplayName(valueNumericUpDown.Value.ToString(), lnt.name);

                        if (lastSelectedValueNode.Parent != null)
                        {
                            if (generatorConfig.treeviewParentNameExtensions.IndexOf(lnt.name) != -1)
                                lastSelectedValueNode.Parent.Text = getTreeNodeDisplayName(lastSelectedValueNode.Parent.Parent.Tag, lastSelectedValueNode.Parent.Tag, valueTextBox.Text);
                        }

                        mechanism theMechanism;
                        if (isPartOfAMechanismTemplate(lastSelectedValueNode, out theMechanism))
                            updateMechInstancesFromMechTemplate(theMechanism);

                        setNeedsSaving();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Failed to set " + lastSelectedValueNode.Text + " to " + valueNumericUpDown.Text);
                    }
                }
            }
        }
        #endregion
        private void saveConfigBbutton_Click(object sender, EventArgs e)
        {
            try
            {
                theRobotConfiguration.save(generatorConfig.robotConfiguration);
                //MessageBox.Show("File saved");
                addProgress("File saved");
                clearNeedsSaving();
            }
            catch (Exception ex)
            {
                addProgress(ex.Message);
            }
        }

        private void addTreeElementButton_Click(object sender, EventArgs e)
        {
            if (lastSelectedValueNode != null)
            {
                TreeNode tn = null;
                foreach (object robotElementObj in robotElementCheckedListBox.CheckedItems)
                {
                    tn = null;
                    Type elementType;

                    // first create a new element instance
                    if (isACollection(robotElementObj))
                        elementType = ((robotElementType)robotElementObj).t.GetGenericArguments().Single();
                    else
                        elementType = robotElementObj.GetType();

                    object obj;

                    // then find the collection of type robotElementObj.t within lastSelectedValueNode
                    PropertyInfo pi;
                    string name;
                    bool addToCollection = true;
                    if (robotElementObj is mechanism)
                    {
                        obj = Activator.CreateInstance((new mechanismInstance()).GetType());

                        name = "mechanismInstance";
                        ((mechanismInstance)obj).mechanism = robotConfig.DeepClone((mechanism)robotElementObj);
                    }
                    else if (isACollection(((robotElementType)robotElementObj).t))
                    {
                        elementType = ((robotElementType)robotElementObj).t.GetGenericArguments().Single();
                        obj = Activator.CreateInstance(elementType);

                        name = ((robotElementType)robotElementObj).ToString();
                    }
                    else
                    {
                        addToCollection = false;
                        obj = Activator.CreateInstance(((robotElementType)robotElementObj).t);
                        name = ((robotElementType)robotElementObj).ToString();
                    }

                    pi = lastSelectedValueNode.Tag.GetType().GetProperty(name);

                    object theObj = pi.GetValue(lastSelectedValueNode.Tag, null);

                    if (addToCollection)
                    {
                        // then add it to the collection
                        theObj.GetType().GetMethod("Add").Invoke(theObj, new object[] { obj });
                        int count = (int)theObj.GetType().GetProperty("Count").GetValue(theObj);
                        tn = AddNode(lastSelectedValueNode, theObj, name);
                    }
                    else
                    {
                        // it is not part of a collection, set the value only if it is null
                        if (theObj == null)
                        {
                            pi.SetValue(lastSelectedValueNode.Tag, obj);
                            tn = AddNode(lastSelectedValueNode, obj, name);
                        }
                    }

                    if (tn != null)
                    {
                        tn.EnsureVisible();
                        tn.Expand();
                    }


                    mechanism theMechanism;
                    if (isPartOfAMechanismTemplate(lastSelectedValueNode, out theMechanism))
                        updateMechInstancesFromMechTemplate(theMechanism);
                }

                if (tn != null)
                    robotTreeView.SelectedNode = tn;

                setNeedsSaving();
            }

            else if (lastSelectedArrayNode != null)
            {
                if (lastSelectedArrayNode.Text == "mechanismInstances")
                {
                    TreeNode tn = null;
                    foreach (object robotElementObj in robotElementCheckedListBox.CheckedItems) // there should only be mechanisms in the checkedItems list 
                    {
                        // first create a new element instance
                        if (robotElementObj is mechanism)
                        {
                            Type elementType = ((mechanism)robotElementObj).GetType();

                            object obj = Activator.CreateInstance((new mechanismInstance()).GetType());
                            ((mechanismInstance)obj).mechanism = robotConfig.DeepClone((mechanism)robotElementObj);

                            // then add it to the collection
                            lastSelectedArrayNode.Tag.GetType().GetMethod("Add").Invoke(lastSelectedArrayNode.Tag, new object[] { obj });
                            int count = (int)lastSelectedArrayNode.Tag.GetType().GetProperty("Count").GetValue(lastSelectedArrayNode.Tag);

                            AddNode(lastSelectedArrayNode, obj, elementType.Name + (count - 1));
                        }
                    }
                }
                else
                {
                    // first create a new instance
                    Type elementType = lastSelectedArrayNode.Tag.GetType().GetGenericArguments().Single();
                    object obj = Activator.CreateInstance(elementType);

                    // then add it to the collection
                    lastSelectedArrayNode.Tag.GetType().GetMethod("Add").Invoke(lastSelectedArrayNode.Tag, new object[] { obj });
                    int count = (int)lastSelectedArrayNode.Tag.GetType().GetProperty("Count").GetValue(lastSelectedArrayNode.Tag);

                    AddNode(lastSelectedArrayNode, obj, elementType.Name + (count - 1));
                }

                setNeedsSaving();

                mechanism theMechanism;
                if (isPartOfAMechanismTemplate(lastSelectedArrayNode, out theMechanism))
                    updateMechInstancesFromMechTemplate(theMechanism);
            }
        }

        void updateMechInstancesFromMechTemplate(mechanism theMechanism)
        {
            foreach (robot r in theRobotConfiguration.theRobotVariants.robot)
            {
                foreach (mechanismInstance mi in r.mechanismInstance)
                {
                    if (mi.mechanism.name == theMechanism.name)
                    {
                        mechanism m = robotConfig.DeepClone(theMechanism);

                        theRobotConfiguration.MergeMechanismParametersIntoStructure(m, mi.mechanism);

                        ((TreeNode)mi.mechanism.theTreeNode).Remove();

                        mi.mechanism = m;
                        mi.mechanism.theTreeNode = AddNode((TreeNode)mi.theTreeNode, mi.mechanism, mi.mechanism.name);
                    }
                }
            }
        }


        bool isPartOfAMechanismTemplate(TreeNode tn, out mechanism theTemplateMechanism)
        {
            List<object> lineage = new List<object>();
            string fullPath = tn.FullPath;

            if (tn != null)
            {
                lineage.Add(tn.Tag);
                while (tn.Parent != null)
                {
                    tn = tn.Parent;
                    lineage.Add(tn.Tag);
                }

                //this finds the index of the "closest" parent mechanism
                int indexOfMechanism = lineage.IndexOf(lineage.Where(x => x.GetType() == typeof(mechanism)).FirstOrDefault());

                if(indexOfMechanism != -1)
                {
                    theTemplateMechanism = (mechanism)lineage[indexOfMechanism];

                    //checks if the selected node is underneath the mechanisms collection.
                    //uses "EndsWith" function to make sure that the mechanisms collection is not considered part of a template
                    return fullPath.Contains("mechanisms") && !fullPath.EndsWith("mechanisms"); //this may be able to just return true, we should know that we are a child of a mechanism if index isn't -1
                }
            }

            theTemplateMechanism = null;
            return false;
        }

        bool isPartOfAMechanismInaMechInstance(TreeNode tn)
        {
            List<object> lineage = new List<object>();

            string fullPath = tn.FullPath;

            if (tn != null)
            {
                lineage.Add(tn.Tag);
                while (tn.Parent != null)
                {
                    tn = tn.Parent;
                    lineage.Add(tn.Tag);
                }

                //returns if mechanismInstances is part of the full path of a node
                //uses "EndsWith" function to make sure that the mechanismInstances collection is not considered part of an instance
                //that causes an issue where only one mechanismInstance can be added to a robot
                return fullPath.Contains("mechanismInstances") && !fullPath.EndsWith("mechanismInstances");
            }

            return false;
        }

        private bool isACollection(object obj)
        {
            return isACollection(obj.GetType());
        }

        private bool isACollection(Type t)
        {
            return ((t.Name == "Collection`1") && (t.Namespace == "System.Collections.ObjectModel"));
        }

        private void addStateDataButton_Click(object sender, EventArgs e)
        {
            string mechanismName = "";
            if (InputBox("Enter mechanism name", "Please enter a name for the mechanism file. A .xml extension will be added.", ref mechanismName) == DialogResult.OK)
            {
                statedata sd = new statedata();
                string filename = Path.GetFileNameWithoutExtension(mechanismName) + ".xml";

                theRobotConfiguration.mechanismControlDefinition.Add(filename, sd);
                AddNode(null, sd, filename);
            }
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (needsSaving)
            {
                DialogResult dlgRes = MessageBox.Show("Do you want to save changes?", "302 Code Generator", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dlgRes == DialogResult.Yes)
                {
                    saveConfigBbutton_Click(null, null);
                    clearNeedsSaving();
                }
                else if (dlgRes == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void clearReportButton_Click(object sender, EventArgs e)
        {
            progressTextBox.Clear();
        }
        private bool isDeletable(TreeNode tn)
        {
            TreeNode parent = tn.Parent;
            if (parent != null)
            {
                if (parent.Tag != null)
                    return isACollection(parent.Tag);
            }
            return false;
        }
        private void deleteTreeElementButton_Click(object sender, EventArgs e)
        {
            // The delete button will be disabled if the highlighted tree item cannot be deleted
            // Only a member of a collection can be deleted
            TreeNode tn = robotTreeView.SelectedNode;
            if (isDeletable(tn))
            {
                TreeNode parent = tn.Parent;
                if (parent != null)
                {
                    if ((parent.Tag != null) && (tn.Tag != null))
                    {

                        object theObjectToDelete = tn.Tag;
                        if (tn.Tag is leafNodeTag)
                            theObjectToDelete = ((leafNodeTag)tn.Tag).obj;

                        if (theObjectToDelete != null)
                        {
                            bool updateMechanismInstances = false;
                            mechanism theMechanism;
                            if (isPartOfAMechanismTemplate(tn, out theMechanism))
                                updateMechanismInstances = true;

                            parent.Tag.GetType().GetMethod("Remove").Invoke(parent.Tag, new object[] { theObjectToDelete });
                            tn.Remove();
                            setNeedsSaving();

                            if ((int)parent.Tag.GetType().GetProperty("Count").GetValue(parent.Tag) == 0)
                            {
                                parent.Remove();
                            }

                            if (updateMechanismInstances == true)
                                updateMechInstancesFromMechTemplate(theMechanism);

                        }
                    }
                }
            }
        }

        private void robotConfigurationFileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void createNewRobotVariantsConfigButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (generatorConfig == null)
                    throw new Exception("Please load a configuration file before creating a new robot variants file");

                SaveFileDialog dlg = new SaveFileDialog();
                dlg.AddExtension = true;
                dlg.DefaultExt = "xml";
                dlg.Filter = "Robot Variants Files | *.xml";

                if (!string.IsNullOrEmpty(generatorConfig.robotConfiguration))
                {
                    string path = Path.GetDirectoryName(configurationFilePathNameTextBox.Text);
                    if (Directory.Exists(path))
                        dlg.InitialDirectory = path;
                }

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    using (var myFileStream = new FileStream(dlg.FileName, FileMode.Create))
                    {
                        robotVariants newRobotVariantsConfig = new robotVariants();
                        newRobotVariantsConfig.robot.Add(new robot());

                        var mySerializer = new XmlSerializer(typeof(robotVariants));
                        mySerializer.Serialize(myFileStream, newRobotVariantsConfig);
                    }

                    Uri uriNewFile = new Uri(dlg.FileName);
                    Uri uriConfigFilePath = new Uri(configurationFilePathNameTextBox.Text);
                    string realtivePath = uriConfigFilePath.MakeRelativeUri(uriNewFile).ToString();
                    generatorConfig.robotConfigurations.Add(realtivePath);
                    robotConfigurationFileComboBox.Items.Add(dlg.FileName);
                    robotConfigurationFileComboBox.SelectedIndex = robotConfigurationFileComboBox.Items.Count - 1;

                    saveGeneratorConfig(Path.GetDirectoryName(configurationFilePathNameTextBox.Text));
                }
            }
            catch (Exception ex)
            {
                addProgress(ex.Message);
            }
        }
    }

    class leafNodeTag
    {
        public Type type { get; private set; }
        public string name { get; private set; }
        public object obj { get; private set; }

        public leafNodeTag(Type type, string name, object obj)
        {
            this.type = type;
            this.name = name;
            this.obj = obj;
        }
    }

    class robotElementType
    {
        public Type t;

        public robotElementType(Type t)
        {
            this.t = t;
        }
        public override string ToString()
        {
            string s = t.ToString();
            int indexlastDot = s.LastIndexOf('.');
            s = s.Substring(indexlastDot + 1);
            s = s.TrimEnd(']');

            return s;
        }
    }
}

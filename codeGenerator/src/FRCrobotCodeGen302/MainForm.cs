﻿using applicationConfiguration;
using ApplicationData;
using Configuration;
using CoreCodeGenerator;
using DataConfiguration;
using NetworkTablesUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace FRCrobotCodeGen302
{
    public partial class MainForm : Form
    {
        toolConfiguration generatorConfig = new toolConfiguration();
        applicationDataConfig theAppDataConfiguration = new applicationDataConfig();
        codeGenerator_302Robotics codeGenerator = new codeGenerator_302Robotics();
        NTViewer viewer;
        bool needsSaving = false;
        bool loadRobotConfig = false;
        readonly string configurationCacheFile = Path.GetTempPath() + "DragonsCodeGeneratorCache.txt";
        bool automationEnabled = false;

        const int treeIconIndex_lockedPadlock = 0;
        const int treeIconIndex_unlockedPadlock = 1;
        const int treeIconIndex_gear = 2;
        const int treeIconIndex_wrench = 3;

        public MainForm()
        {
            InitializeComponent();

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                if (args[1] == "enableAutomation")
                {
                    automationEnabled = true;
                    selectNodeButton.Enabled = true;
                    selectNodeButton.Visible = true;
                    getCheckBoxListItemsButton.Enabled = true;
                    getCheckBoxListItemsButton.Visible = true;
                    checkCheckBoxListItemButton.Enabled = true;
                    checkCheckBoxListItemButton.Visible = true;
                    getSelectedTreeElementPathButton.Enabled = true;
                    getSelectedTreeElementPathButton.Visible = true;

                    infoIOtextBox.Enabled = true;
                    infoIOtextBox.Visible = true;
                    selectedNodePathTextBox.Enabled = true;
                    selectedNodePathTextBox.Visible = true;
                }
            }

            codeGenerator.setProgressCallback(addProgress);
            theAppDataConfiguration.setProgressCallback(addProgress);
            clearNeedsSaving();

            splitContainer1.SplitterDistance = splitContainer1.Width - 180;

            valueNumericUpDown.Width -= physicalUnitsComboBox.Width;
            valueComboBox.Width = valueNumericUpDown.Width;
            valueTextBox.Width = valueNumericUpDown.Width;

            valueComboBox.Location = valueNumericUpDown.Location;
            valueTextBox.Location = valueNumericUpDown.Location;
            valueDatePicker.Location = valueNumericUpDown.Location;
            physicalUnitsComboBox.Location = new Point(valueNumericUpDown.Location.X + valueNumericUpDown.Width + 3, valueNumericUpDown.Location.Y);

            this.Text += " Version " + ProductVersion;

            //initialize NT viewer
            viewer = new NTViewer(tuningButton);

            if (!automationEnabled)
            {
                #region try to load cached configuration.xml
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
                #endregion
            }
            robotTreeView.ImageList = treeViewIcons;
        }

        private void addProgress(string info)
        {
            progressTextBox.AppendText(info + Environment.NewLine);
        }

        private TreeNode AddNode(TreeNode parent, object obj, string nodeName)
        {
            TreeNode tn = null;

            #region Add a new node if necessary
            TreeNodeCollection tnc = (parent == null) ? robotTreeView.Nodes : parent.Nodes;
            if (DataConfiguration.baseDataConfiguration.isACollection(obj))
            {
                ICollection ic = obj as ICollection;
                if (ic.Count > 0)
                    tn = tnc.Add(nodeName);
            }
            else
            {
                tn = tnc.Add(nodeName);
            }
            #endregion

            if (tn != null) // if a node has been added
            {
                tn.Tag = new nonLeafNodeTag(nodeName, obj);

                if (DataConfiguration.baseDataConfiguration.isACollection(obj)) // if it is a collection, add an entry for each item
                {
                    ICollection ic = obj as ICollection;
                    if (ic.Count > 0)
                    {
                        foreach (var v in ic)
                            AddNode(tn, v, v.GetType().Name);
                    }
                }
                else
                {
                    bool treatAsLeafNode = false;
                    PropertyInfo piToDisplay = null;

                    string unitsAsString = "";
                    physicalUnit.Family unitsFamily = physicalUnit.Family.unitless;

                    if (parent != null)
                    {
                        unitsAsString = GetUnitsShortName(nonLeafNodeTag.getObject(parent.Tag));
                        unitsFamily = GetTheUnitsFamilyName(parent, obj, nodeName);
                    }

                    #region Record the TreeNode in the actual object so that we do not have to back search for mechanisms
                    if (obj is mechanismInstance)
                        ((mechanismInstance)obj).theTreeNode = tn;
                    else if (obj is mechanism)
                        ((mechanism)obj).theTreeNode = tn;
                    #endregion

                    #region Determine which PropertyInfo to display
                    PropertyInfo piValue__ = obj.GetType().GetProperty("value__");
                    if (piValue__ != null)
                    {
                        //    // if the object contains a property called value__, we want to hide value__ and
                        //    // instead show the value of the object next to the parent
                        treatAsLeafNode = true;
                        //    piToDisplay = piValue__;
                    }
                    //else 
                    if ((parent != null) && (parent.Tag != null))
                    {
                        piToDisplay = nonLeafNodeTag.getObject(parent.Tag).GetType().GetProperties().ToList().Find(p => p.Name == nodeName);
                    }
                    #endregion

                    #region Get the available attributes
                    RangeAttribute range = null;
                    DefaultValueAttribute defaultValue = null;
                    bool isConstant = false;
                    bool isTunable = false;

                    if (piToDisplay != null)
                    {
                        if (unitsFamily == physicalUnit.Family.unitless)
                        {
                            PhysicalUnitsFamilyAttribute units = piToDisplay.GetCustomAttribute<PhysicalUnitsFamilyAttribute>(true);
                            unitsFamily = units == null ? physicalUnit.Family.unitless : units.family;
                        }

                        range = piToDisplay.GetCustomAttribute<RangeAttribute>();
                        defaultValue = piToDisplay.GetCustomAttribute<DefaultValueAttribute>();
                        isTunable = piToDisplay.GetCustomAttribute<TunableParameterAttribute>() != null;
                        isConstant = piToDisplay.GetCustomAttribute<ConstantAttribute>() != null;
                    }
                    #endregion

                    Type objType = obj.GetType();

                    if ((objType.FullName == "System.String") || (objType.FullName == "System.DateTime")) // these types need to be treated like a leaf node
                        treatAsLeafNode = true;

                    PropertyInfo[] propertyInfos = objType.GetProperties();
                    if (!treatAsLeafNode && (propertyInfos.Length > 0))
                    {
                        foreach (PropertyInfo pi in propertyInfos) // add its children
                        {
                            if (!(pi.Name.StartsWith("__") && pi.Name.EndsWith("__")))
                            {
                                object theObj = pi.GetValue(obj);

                                if (theObj != null)
                                    AddNode(tn, theObj, pi.Name);
                            }
                        }

                        tn.Text = getDisplayName(obj, "");
                    }
                    else
                    {
                        // this means that this is a leaf node
                        int imageIndex = treeIconIndex_unlockedPadlock;
                        if (isConstant)
                            imageIndex = treeIconIndex_lockedPadlock;
                        else if (isTunable)
                            imageIndex = treeIconIndex_wrench;
                        else if (isPartOfAMechanismInaMechInstance(tn))
                            imageIndex = treeIconIndex_lockedPadlock;

                        tn.ImageIndex = imageIndex;
                        tn.SelectedImageIndex = imageIndex;

                        leafNodeTag lnt = new leafNodeTag(obj.GetType(), nodeName, obj, isConstant, isTunable, unitsFamily, unitsAsString);
                        if (range != null)
                            lnt.setRange(Convert.ToDouble(range.Minimum), Convert.ToDouble(range.Maximum));
                        else
                            lnt.setRange(obj is uint ? Convert.ToDouble(0) : Convert.ToDouble(-10000), Convert.ToDouble(10000));

                        if (defaultValue != null)
                            lnt.setDefault(defaultValue.Value);
                        else
                            lnt.setDefault(0);

                        tn.Tag = lnt;

                        tn.Text = getDisplayName((piValue__ != null) ? obj : nonLeafNodeTag.getObject(parent.Tag), nodeName);
                    }
                }
            }

            return tn;
        }


        private string getDisplayName(object obj, string instanceName)
        {
            helperFunctions.RefreshLevel refresh;
            return getDisplayName(obj, instanceName, out refresh);
        }

        private string getDisplayName(object obj, string instanceName, out helperFunctions.RefreshLevel refresh)
        {
            refresh = helperFunctions.RefreshLevel.none;

            MethodInfo mi = obj.GetType().GetMethod("getDisplayName");
            if (mi != null)
            {
                string temp = "";
                ParameterInfo[] pi = mi.GetParameters();
                if (pi.Length == 0)
                    temp = (string)mi.Invoke(obj, new object[] { });
                else if (pi.Length == 2)
                {
                    helperFunctions.RefreshLevel tempRefresh = helperFunctions.RefreshLevel.none;
                    object[] parameters = new object[] { instanceName, tempRefresh };
                    temp = (string)mi.Invoke(obj, parameters);
                    refresh = (helperFunctions.RefreshLevel)parameters[1];
                }

                return temp;
            }

            return instanceName + " (" + obj.ToString() + ")";
        }

        private static physicalUnit.Family GetTheUnitsFamilyName(TreeNode parent, object obj, string originalNodeName)
        {
            physicalUnit.Family unitsFamily = physicalUnit.Family.unitless;
            PropertyInfo unitsFamilyPi = nonLeafNodeTag.getObject(parent.Tag).GetType().GetProperty("unitsFamily");
            if (unitsFamilyPi != null)
                unitsFamily = (physicalUnit.Family)unitsFamilyPi.GetValue(nonLeafNodeTag.getObject(parent.Tag)); // the units family is defined in a property as part of the class
            else
            {
                if ((parent != null) && (nonLeafNodeTag.getObject(parent.Tag) != null))
                {
                    PropertyInfo info = nonLeafNodeTag.getObject(parent.Tag).GetType().GetProperty(originalNodeName);
                    if (info != null)
                    {
                        PhysicalUnitsFamilyAttribute unitFamilyAttr = info.GetCustomAttribute<PhysicalUnitsFamilyAttribute>();
                        if (unitFamilyAttr != null)
                            unitsFamily = unitFamilyAttr.family; // the units family is defined as an attribute at the usage of the class type (at the instance definition)
                    }
                }
            }

            return unitsFamily;
        }

        private string GetUnitsShortName(object obj)
        {
            string unitsAsString = null;
            PropertyInfo unitsPi = obj.GetType().GetProperty("__units__");
            if (unitsPi != null)
            {
                unitsAsString = (string)unitsPi.GetValue(obj);
            }
            if (unitsAsString == null)
                unitsAsString = "";
            return unitsAsString;
        }

        private void populateTree(applicationDataConfig theApplicationDataConfig)
        {
            robotTreeView.Nodes.Clear();
            AddNode(null, theApplicationDataConfig.theRobotVariants, "Robot Variant");
            if (theApplicationDataConfig.theRobotVariants.Robots.Count > 0)
                robotTreeView.Nodes[0].Expand();
        }

        public void loadGeneratorConfig(string configurationFullPathName)
        {
            try
            {
                generatorConfig = generatorConfig.deserialize(configurationFullPathName);
                if (generatorConfig.appDataConfigurations.Count == 0)
                {
                    generatorConfig.appDataConfigurations = new List<string>();
                    if (!string.IsNullOrEmpty(generatorConfig.robotConfiguration.Trim()))
                        generatorConfig.appDataConfigurations.Add(generatorConfig.robotConfiguration.Trim());
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
                codeGenerator.generate(theAppDataConfiguration, generatorConfig);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong. See below. \r\n\r\n" + ex.Message, "Code generator error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                        if (!automationEnabled)
                        {
                            //now that generator config has loaded succesfully, save to a temp file to save the desired config for future uses
                            File.WriteAllText(configurationCacheFile, configurationFilePathNameTextBox.Text);
                            addProgress("Wrote cached configuration.xml to: " + configurationCacheFile);
                        }
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
            #region Load the Combobox with the robot configuration file list
            robotConfigurationFileComboBox.Items.Clear();
            foreach (string f in generatorConfig.appDataConfigurations)
            {
                string fullfilePath = Path.Combine(Path.GetDirectoryName(filePathName), f);
                fullfilePath = Path.GetFullPath(fullfilePath);
                robotConfigurationFileComboBox.Items.Add(fullfilePath);
            }
            #endregion

            generatorConfig.rootOutputFolder = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(filePathName), generatorConfig.rootOutputFolder));
            generatorConfig.robotConfiguration = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(filePathName), robotConfigurationFileComboBox.Text));
            loadRobotConfig = true;

            // select the config in the combobox after setting loadRobotConfig to true, otherwise robotConfigurationFileComboBox_TextChanged might fire before loadRobotConfig == true
            if (robotConfigurationFileComboBox.Items.Count > 0)
                robotConfigurationFileComboBox.SelectedIndex = 0;
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
                        theAppDataConfiguration.collectionBaseTypes = generatorConfig.collectionBaseTypes;
                        theAppDataConfiguration.load(generatorConfig.robotConfiguration);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Issue encountered while loading the robot configuration file\r\n" + ex.ToString());
                    }

                    try
                    {
                        addProgress("Populating the robot configuration tree view.");
                        populateTree(theAppDataConfiguration);
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

            if (obj is leafNodeTag)
                obj = ((leafNodeTag)obj).obj;
            else if (obj is nonLeafNodeTag)
                obj = nonLeafNodeTag.getObject(obj);
            else
                obj = null;

            if (theAppDataConfiguration.isASubClassedCollection(obj))
            {
                Type elementType = obj.GetType().GetGenericArguments().Single();
                List<Type> subTypes = Assembly.GetAssembly(elementType).GetTypes().Where(t => t.BaseType == elementType).ToList();
                foreach (Type type in subTypes)
                    addRobotElementType(type, types);
            }
            else
            {
                PropertyInfo[] propertyInfos = obj.GetType().GetProperties();

                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if (theAppDataConfiguration.isASubClassedCollection(propertyInfo.PropertyType))
                    {
                        ICollection ic = propertyInfo.GetValue(obj) as ICollection;
                        if (ic.Count == 0)
                        {
                            Type elementType = propertyInfo.PropertyType.GetGenericArguments().Single();
                            List<Type> subTypes = Assembly.GetAssembly(obj.GetType()).GetTypes().Where(t => t.BaseType == elementType).ToList();
                            foreach (Type type in subTypes)
                                addRobotElementType(type, types);
                        }
                    }
                    //else if (theRobotConfiguration.isASubClassedCollection(obj.GetType()))
                    //{
                    //    //Type elementType = propertyInfo.PropertyType.GetGenericArguments().Single();
                    //    //List<Type> subTypes = Assembly.GetAssembly(obj.GetType()).GetTypes().Where(t => t.BaseType == elementType).ToList();
                    //    //foreach (Type type in subTypes)
                    //    //    types.Add(new robotElementType(type));
                    //}
                    else if (DataConfiguration.baseDataConfiguration.isACollection(propertyInfo.PropertyType))
                    {
                        ICollection ic = propertyInfo.GetValue(obj) as ICollection;
                        if (ic.Count == 0)
                            addRobotElementType(propertyInfo.PropertyType, propertyInfo.Name, types);
                    }
                    else if (propertyInfo.PropertyType == typeof(mechanismInstance))
                    {
                        addRobotElementType(propertyInfo.PropertyType, types);
                    }
                    else if (!propertyInfo.PropertyType.FullName.StartsWith("System."))
                    {
                        if (!DataConfiguration.baseDataConfiguration.isACollection(obj))
                        {
                            if (propertyInfo.GetValue(obj, null) == null)
                                addRobotElementType(propertyInfo.PropertyType, types);
                        }
                    }
                }
            }

            return types;
        }

        void addRobotElementType(Type theType, string name, List<robotElementType> types)
        {
            NotUserAddableAttribute nuaa = theType.GetCustomAttribute<NotUserAddableAttribute>();
            if (nuaa == null)
            {
                if (name == null)
                    types.Add(new robotElementType(theType));
                else
                    types.Add(new robotElementType(theType, name));

            }
        }

        void addRobotElementType(Type theType, List<robotElementType> types)
        {
            addRobotElementType(theType, null, types);
        }

        void hideAllValueEntryBoxes()
        {
            bool visible = false;

            valueComboBox.Visible = visible;
            valueNumericUpDown.Visible = visible;
            valueTextBox.Visible = visible;
            valueDatePicker.Visible = visible;
        }
        string setPhysicalUnitsComboBox(physicalUnit.Family unitsFamily, string shortUnitsName)
        {
            string updatedUnits = null;

            physicalUnitsComboBox.Items.Clear();
            physicalUnitsComboBox.Visible = unitsFamily != physicalUnit.Family.unitless;
            if (physicalUnitsComboBox.Visible)
            {
                List<physicalUnit> unitsList = generatorConfig.physicalUnits.FindAll(p => p.family == unitsFamily);
                foreach (physicalUnit unit in unitsList)
                {
                    physicalUnitsComboBox.Items.Add(unit);
                }

                physicalUnit units = unitsList.Find(u => u.shortName == shortUnitsName);
                if (units != null)
                    physicalUnitsComboBox.SelectedIndex = physicalUnitsComboBox.FindStringExact(units.shortName);
                else if (unitsList.Count > 0)
                {
                    physicalUnitsComboBox.SelectedIndex = 0;
                    updatedUnits = unitsList[0].shortName;
                }
            }

            return updatedUnits;
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

        void showValueDatePicker()
        {
            hideAllValueEntryBoxes();
            valueDatePicker.Visible = true;
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
            physicalUnitsComboBox.Visible = false;
            addTreeElementButton.Enabled = false;

            lastSelectedArrayNode = null;
            lastSelectedValueNode = null;

            bool isInaMechanismInstance = isPartOfAMechanismInaMechInstance(e.Node);
            deleteTreeElementButton.Enabled = isDeletable(e.Node) && !isInaMechanismInstance;

            if (e.Node.Tag != null)
            {
                bool visible_And_or_Enabled = false;

                #region Add items which can be added as children of the currently selected item
                theCurrentElementPossibilities = getEmptyPossibleCollectionSubTypes(e.Node.Tag);
                if ((theCurrentElementPossibilities.Count > 0) && (!isPartOfAMechanismInaMechInstance(e.Node)))
                {
                    visible_And_or_Enabled = true;

                    robotElementCheckedListBox.Items.Clear();
                    foreach (robotElementType t in theCurrentElementPossibilities)
                    {
                        Type elementType;
                        if (DataConfiguration.baseDataConfiguration.isACollection(t.t))
                            elementType = t.t.GetGenericArguments().Single();
                        else
                            elementType = t.t;

                        // Add the defined mechanisms as choices to add to a robot variant
                        if (elementType.Equals((new mechanismInstance()).GetType()))
                        {
                            foreach (mechanism m in theAppDataConfiguration.theRobotVariants.Mechanisms)
                            {
                                robotElementType ret = new robotElementType(m.GetType(), m);

                                robotElementCheckedListBox.Items.Add(ret);
                            }
                        }
                        else
                        {
                            robotElementCheckedListBox.Items.Add(t);
                        }
                    }
                }
                #endregion

                robotElementCheckedListBox.Visible = visible_And_or_Enabled;
                addRobotElementLabel.Visible = visible_And_or_Enabled;
                addTreeElementButton.Enabled = visible_And_or_Enabled;

                object obj = e.Node.Tag is leafNodeTag ? ((leafNodeTag)e.Node.Tag).obj : nonLeafNodeTag.getObject(e.Node.Tag);
                if (DataConfiguration.baseDataConfiguration.isACollection(obj))
                {
                    lastSelectedArrayNode = e.Node;
                    addTreeElementButton.Enabled = !isInaMechanismInstance;
                }
                /*
                else if ((e.Node.Parent!=null) && (e.Node.Parent.Tag is robot))
                {
                    // do nothing
                }   */
                else if ((e.Node.GetNodeCount(false) == 0) && (e.Node.Parent != null))
                {
                    lastSelectedValueNode = e.Node;

                    leafNodeTag lnt = (leafNodeTag)(e.Node.Tag);

                    object value = null;
                    bool allowEdit = false;
                    bool isValue__ = false;
                    if (!lnt.isConstant)
                    {
                        PropertyInfo valueProp = ((leafNodeTag)lastSelectedValueNode.Tag).type.GetProperty("value__", BindingFlags.Public | BindingFlags.Instance);
                        if (valueProp != null)
                        {
                            isValue__ = true;
                            value = valueProp.GetValue(((leafNodeTag)lastSelectedValueNode.Tag).obj);
                        }
                        else
                            value = lnt.obj;

                        allowEdit = lnt.isTunable ? true : !isInaMechanismInstance;
                    }

                    enableCallback = false;
                    if ((lnt.name == "value") || isValue__)
                    {
                        string updatedUnits = setPhysicalUnitsComboBox(lnt.unitsFamily, lnt.physicalUnits);
                        if (!String.IsNullOrEmpty(updatedUnits))
                        {
                            PropertyInfo valueProp = ((leafNodeTag)lastSelectedValueNode.Tag).type.GetProperty("__units__", BindingFlags.Public | BindingFlags.Instance);
                            if (valueProp != null)
                            {
                                valueProp.SetValue(((leafNodeTag)lastSelectedValueNode.Tag).obj, updatedUnits);
                                lnt.physicalUnits = updatedUnits;
                                setNeedsSaving();
                            }
                        }
                    }
                    else
                        physicalUnitsComboBox.Visible = false;

                    if (allowEdit)
                    {
                        PropertyInfo valueStringList = ((leafNodeTag)lastSelectedValueNode.Tag).type.GetProperty("value_strings", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (valueStringList != null)
                        {
                            showValueComboBox();
                            valueComboBox.Items.Clear();

                            List<string> strings = (List<string>)valueStringList.GetValue(((leafNodeTag)lastSelectedValueNode.Tag).obj);
                            foreach (string en in strings)
                                valueComboBox.Items.Add(en);

                            valueComboBox.SelectedIndex = valueComboBox.FindStringExact(value.ToString());
                        }
                        else if (lnt.type.IsEnum)
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
                            valueNumericUpDown.Minimum = Convert.ToInt32(lnt.range.minRange);
                            valueNumericUpDown.Maximum = Convert.ToInt32(lnt.range.maxRange);

                            if ((uint)value < lnt.range.minRange || (uint)value > lnt.range.maxRange)
                            {
                                value = (uint)lnt.range.minRange;

                                PropertyInfo valueProp = ((leafNodeTag)lastSelectedValueNode.Tag).type.GetProperty("value__", BindingFlags.Public | BindingFlags.Instance);
                                if (valueProp != null)
                                    valueProp.SetValue(((leafNodeTag)lastSelectedValueNode.Tag).obj, value);
                            }
                            valueNumericUpDown.DecimalPlaces = 0;
                            valueNumericUpDown.Value = (uint)value;
                            showValueNumericUpDown();
                        }
                        else if (value is int || value is Int32)
                        {
                            valueNumericUpDown.Minimum = Convert.ToInt32(lnt.range.minRange);
                            valueNumericUpDown.Maximum = Convert.ToInt32(lnt.range.maxRange);

                            valueNumericUpDown.DecimalPlaces = 0;
                            valueNumericUpDown.Value = (int)value;
                            showValueNumericUpDown();
                        }
                        else if (value is double)
                        {
                            valueNumericUpDown.Minimum = Convert.ToDecimal(lnt.range.minRange);
                            valueNumericUpDown.Maximum = Convert.ToDecimal(lnt.range.maxRange);

                            valueNumericUpDown.DecimalPlaces = 5;
                            valueNumericUpDown.Value = Convert.ToDecimal(value);
                            showValueNumericUpDown();
                        }
                        else if (value is DateTime)
                        {
                            showValueDatePicker();
                            valueDatePicker.Value = (DateTime)value;
                        }
                        else if (value is bool)
                        {
                            showValueComboBox();
                            valueComboBox.Items.Clear();

                            valueComboBox.Items.Add(true.ToString());
                            valueComboBox.Items.Add(false.ToString());

                            valueComboBox.SelectedIndex = valueComboBox.FindStringExact(value.ToString());
                        }
                        else
                        {
                            showValueTextBox();
                            valueTextBox.Text = value.ToString();
                        }
                    }
                    enableCallback = true;
                }
                else
                {
                    lastSelectedValueNode = e.Node;
                }
            }
        }

        #region handle the events when values are changed
        private void physicalUnitsComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (enableCallback)
            {
                if (lastSelectedValueNode != null)
                {
                    try
                    {
                        leafNodeTag lnt = (leafNodeTag)(lastSelectedValueNode.Tag);

                        object parentObj = nonLeafNodeTag.getObject(lastSelectedValueNode.Parent.Tag);

                        PropertyInfo prop = null;
                        Object objToOperateOn = lnt.obj;
                        if (DataConfiguration.baseDataConfiguration.isACollection(parentObj))
                        {
                            Type elementType = parentObj.GetType().GetGenericArguments().Single();
                            prop = elementType.GetProperty("__units__", BindingFlags.Public | BindingFlags.Instance);
                        }
                        else
                        {
                            prop = objToOperateOn.GetType().GetProperty("__units__", BindingFlags.Public | BindingFlags.Instance);
                            if (prop == null)
                            {
                                objToOperateOn = parentObj;
                                prop = objToOperateOn.GetType().GetProperty("__units__", BindingFlags.Public | BindingFlags.Instance);
                            }
                        }


                        if ((prop != null) && prop.CanWrite)
                        {
                            prop.SetValue(objToOperateOn, physicalUnitsComboBox.Text);

                            lastSelectedValueNode.Text = getDisplayName(parentObj, lnt.name);

                            if (lastSelectedValueNode.Parent != null)
                                lastSelectedValueNode.Parent.Text = getDisplayName(nonLeafNodeTag.getObject(lastSelectedValueNode.Parent.Tag), "");

                            mechanism theMechanism;
                            if (isPartOfAMechanismTemplate(lastSelectedValueNode, out theMechanism))
                                updateMechInstancesFromMechTemplate(theMechanism);

                            setNeedsSaving();
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Failed to set the units of " + lastSelectedValueNode.Text + " to " + physicalUnitsComboBox.Text);
                    }
                }
            }
        }
        private void valueComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (enableCallback)
            {
                if (lastSelectedValueNode != null)
                {
                    try
                    {
                        leafNodeTag lnt = (leafNodeTag)(lastSelectedValueNode.Tag);

                        object obj = nonLeafNodeTag.getObject(lastSelectedValueNode.Parent.Tag);

                        PropertyInfo prop = null;
                        PropertyInfo valueStringList = null;

                        bool isValue__ = true;
                        if (DataConfiguration.baseDataConfiguration.isACollection(obj))
                        {
                            Type elementType = obj.GetType().GetGenericArguments().Single();
                            prop = elementType.GetProperty("value__", BindingFlags.Public | BindingFlags.Instance);
                            valueStringList = elementType.GetProperty("value_strings", BindingFlags.NonPublic | BindingFlags.Instance);
                        }
                        else
                        {
                            obj = lnt.obj;
                            prop = lnt.type.GetProperty("value__", BindingFlags.Public | BindingFlags.Instance);
                            if (prop == null)
                            {
                                isValue__ = false;
                                obj = nonLeafNodeTag.getObject(lastSelectedValueNode.Parent.Tag);
                                prop = obj.GetType().GetProperty(lnt.name, BindingFlags.Public | BindingFlags.Instance);
                            }
                        }


                        if ((prop != null) && (prop.CanWrite))
                        {
                            if (valueStringList == null)
                                if (isValue__)
                                    prop.SetValue(lnt.obj, valueComboBox.Text == "True");
                                else
                                {
                                    if (prop.PropertyType == typeof(bool))
                                        prop.SetValue(obj, valueComboBox.Text == "True");
                                    else
                                        prop.SetValue(obj, Enum.Parse(lnt.type, valueComboBox.Text));
                                }
                            else
                            {
                                prop.SetValue(lnt.obj, valueComboBox.Text);
                            }
                            if (!isValue__)
                            {
                                lnt.obj = prop.GetValue(obj);

                                if (prop.Name == "unitsFamily")
                                {
                                    physicalUnit.Family family = (physicalUnit.Family)Enum.Parse(typeof(physicalUnit.Family), lnt.obj.ToString());
                                    physicalUnit firstUnit = generatorConfig.physicalUnits.Find(p => p.family == family);

                                    // update the leafNodeTags
                                    TreeNodeCollection tnc = lastSelectedValueNode.Parent.Nodes;
                                    foreach (TreeNode node in tnc)
                                    {
                                        if ((node.Tag != null) && (node.Tag is leafNodeTag))
                                        {
                                            ((leafNodeTag)node.Tag).unitsFamily = family;
                                            ((leafNodeTag)node.Tag).physicalUnits = firstUnit == null ? "" : firstUnit.ToString();
                                        }
                                    }

                                    // update the __units__
                                    PropertyInfo pi = nonLeafNodeTag.getObject(lastSelectedValueNode.Parent.Tag).GetType().GetProperty("__units__");
                                    if (pi != null)
                                        pi.SetValue(nonLeafNodeTag.getObject(lastSelectedValueNode.Parent.Tag), firstUnit == null ? "" : firstUnit.ToString());
                                }
                            }
                        }

                        helperFunctions.RefreshLevel refresh;
                        if (isValue__)
                            lastSelectedValueNode.Text = getDisplayName(obj, lnt.name, out refresh);
                        else
                            lastSelectedValueNode.Text = getDisplayName(obj, prop.Name, out refresh);

                        if (lastSelectedValueNode.Parent != null)
                        {
                            if ((refresh == helperFunctions.RefreshLevel.parentHeader) || (refresh == helperFunctions.RefreshLevel.fullParent))
                                lastSelectedValueNode.Parent.Text = getDisplayName(nonLeafNodeTag.getObject(lastSelectedValueNode.Parent.Tag), "");

                            if (refresh == helperFunctions.RefreshLevel.fullParent)
                            {
                                TreeNodeCollection tnc = lastSelectedValueNode.Parent.Nodes;
                                foreach (TreeNode node in tnc)
                                {
                                    node.Text = getDisplayName(nonLeafNodeTag.getObject(node.Parent.Tag), ((leafNodeTag)node.Tag).name);
                                }
                            }
                        }

                        mechanism theMechanism;
                        if (isPartOfAMechanismTemplate(lastSelectedValueNode, out theMechanism))
                            updateMechInstancesFromMechTemplate(theMechanism);

                        setNeedsSaving();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Failed to set " + lastSelectedValueNode.Text + " to " + valueComboBox.Text);
                    }
                }
            }
        }

        private void valueDatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (enableCallback)
            {
                if (lastSelectedValueNode != null)
                {
                    try
                    {
                        leafNodeTag lnt = (leafNodeTag)(lastSelectedValueNode.Tag);

                        PropertyInfo prop = nonLeafNodeTag.getObject(lastSelectedValueNode.Parent.Tag).GetType().GetProperty(lnt.name, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            prop.SetValue(nonLeafNodeTag.getObject(lastSelectedValueNode.Parent.Tag), valueDatePicker.Value);
                        }

                        lastSelectedValueNode.Text = getDisplayName(lnt.obj, lnt.name);

                        if (lastSelectedValueNode.Parent != null)
                            lastSelectedValueNode.Parent.Text = getDisplayName(nonLeafNodeTag.getObject(lastSelectedValueNode.Parent.Tag), "");

                        mechanism theMechanism;
                        if (isPartOfAMechanismTemplate(lastSelectedValueNode, out theMechanism))
                            updateMechInstancesFromMechTemplate(theMechanism);

                        setNeedsSaving();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Failed to set " + lastSelectedValueNode.Text + " to " + valueDatePicker.Value.ToShortDateString());
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

                        bool isValue__ = true;
                        object obj = lnt.obj;
                        PropertyInfo prop = lnt.type.GetProperty("value__", BindingFlags.Public | BindingFlags.Instance);
                        if (prop == null)
                        {
                            isValue__ = false;
                            obj = nonLeafNodeTag.getObject(lastSelectedValueNode.Parent.Tag);
                            prop = obj.GetType().GetProperty(lnt.name, BindingFlags.Public | BindingFlags.Instance);
                        }

                        if ((prop != null) && (prop.CanWrite))
                        {
                            prop.SetValue(obj, valueTextBox.Text);

                            if (!isValue__)
                                lnt.obj = prop.GetValue(obj);

                            if (lnt.isTunable)
                            {
                                if (viewer != null)
                                    viewer.PushValue(valueTextBox.Text, NTViewer.ConvertFullNameToTuningKey(lnt.name));
                            }
                        }

                        lastSelectedValueNode.Text = getDisplayName(isValue__ ? obj : prop.GetValue(obj), lnt.name);

                        if (lastSelectedValueNode.Parent != null)
                            lastSelectedValueNode.Parent.Text = getDisplayName(nonLeafNodeTag.getObject(lastSelectedValueNode.Parent.Tag), "");

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

                        bool isValue__ = true;
                        object obj = lnt.obj;
                        PropertyInfo prop = lnt.type.GetProperty("value__", BindingFlags.Public | BindingFlags.Instance);
                        if (prop == null)
                        {
                            isValue__ = false;
                            obj = nonLeafNodeTag.getObject(lastSelectedValueNode.Parent.Tag);
                            prop = obj.GetType().GetProperty(lnt.name, BindingFlags.Public | BindingFlags.Instance);
                        }

                        if ((prop != null) && (prop.CanWrite))
                        {
                            if (prop.PropertyType.Name == "UInt")
                                prop.SetValue(obj, (uint)Math.Round(valueNumericUpDown.Value, valueNumericUpDown.DecimalPlaces, MidpointRounding.AwayFromZero));
                            else if (prop.PropertyType.Name == "UInt32")
                                prop.SetValue(obj, (UInt32)Math.Round(valueNumericUpDown.Value, valueNumericUpDown.DecimalPlaces, MidpointRounding.AwayFromZero));
                            else if (prop.PropertyType.Name == "Int")
                                prop.SetValue(obj, (int)Math.Round(valueNumericUpDown.Value, valueNumericUpDown.DecimalPlaces, MidpointRounding.AwayFromZero));
                            else if (prop.PropertyType.Name == "Int32")
                                prop.SetValue(obj, (Int32)Math.Round(valueNumericUpDown.Value, valueNumericUpDown.DecimalPlaces, MidpointRounding.AwayFromZero));
                            else if (prop.PropertyType.Name == "Double")
                            {
                                prop.SetValue(obj, (double)valueNumericUpDown.Value);
                            }

                            if (!isValue__)
                                lnt.obj = prop.GetValue(obj);

                            if (lnt.isTunable)
                            {
                                if (viewer != null)
                                    viewer.PushValue((double)valueNumericUpDown.Value, NTViewer.ConvertFullNameToTuningKey(lnt.name));
                            }
                        }

                        helperFunctions.RefreshLevel refresh;
                        if (isValue__)
                            lastSelectedValueNode.Text = getDisplayName(obj, lnt.name, out refresh);
                        else
                            lastSelectedValueNode.Text = getDisplayName(obj, prop.Name, out refresh);

                        if (lastSelectedValueNode.Parent != null)
                        {
                            if (refresh == helperFunctions.RefreshLevel.parentHeader)
                                lastSelectedValueNode.Parent.Text = getDisplayName(nonLeafNodeTag.getObject(lastSelectedValueNode.Parent.Tag), "");

                            if (refresh == helperFunctions.RefreshLevel.fullParent)
                            {
                                TreeNodeCollection tnc = lastSelectedValueNode.Parent.Nodes;
                                foreach (TreeNode node in tnc)
                                {
                                    helperFunctions.RefreshLevel refr;
                                    node.Text = getDisplayName(((leafNodeTag)node.Tag).obj, ((leafNodeTag)node.Tag).name, out refr);
                                }
                            }
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
                theAppDataConfiguration.save(generatorConfig.robotConfiguration);
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
                TreeNode mechanismInstancesNode = null;
                TreeNode tn = null;
                foreach (object robotElementObj in robotElementCheckedListBox.CheckedItems)
                {
                    tn = null;
                    object obj = null;

                    string name = "";
                    bool addToCollection = true;
                    if (theAppDataConfiguration.isDerivedFromGenericClass(((robotElementType)robotElementObj).t))
                    {
                        obj = Activator.CreateInstance(((robotElementType)robotElementObj).t);

                        Type baseType = (((robotElementType)robotElementObj).t).BaseType;
                        name = baseType.Name;
                    }
                    else if (((robotElementType)robotElementObj).t == typeof(mechanism))
                    {
                        obj = Activator.CreateInstance((new mechanismInstance()).GetType());

                        name = "mechanismInstance";
                        ((mechanismInstance)obj).mechanism = applicationDataConfig.DeepClone((mechanism)((robotElementType)robotElementObj).theObject);
                    }
                    else if (DataConfiguration.baseDataConfiguration.isACollection(((robotElementType)robotElementObj).t))
                    {
                        Type elementType = ((robotElementType)robotElementObj).t.GetGenericArguments().Single();
                        obj = Activator.CreateInstance(elementType);

                        // get the name of the property of this type
                        PropertyInfo[] pis = nonLeafNodeTag.getObject(lastSelectedValueNode.Tag).GetType().GetProperties();

                        PropertyInfo pi = pis.ToList().Find(p => p.Name == ((robotElementType)robotElementObj).name);
                        name = ((robotElementType)robotElementObj).name;
                        if (pi == null) // then try to search by the type
                        {
                            pi = pis.ToList().Find(p => p.PropertyType == ((robotElementType)robotElementObj).t);
                            name = (pi == null) ? ((robotElementType)robotElementObj).ToString() : pi.Name;
                        }
                    }
                    else
                    {
                        addToCollection = false;
                        obj = Activator.CreateInstance(((robotElementType)robotElementObj).t);
                        name = ((robotElementType)robotElementObj).ToString();
                    }


                    if (obj != null)
                    {
                        PropertyInfo[] pis = nonLeafNodeTag.getObject(lastSelectedValueNode.Tag).GetType().GetProperties();
                        PropertyInfo pi = nonLeafNodeTag.getObject(lastSelectedValueNode.Tag).GetType().GetProperty(name);

                        object theObj = pi.GetValue(nonLeafNodeTag.getObject(lastSelectedValueNode.Tag), null);

                        if (addToCollection)
                        {
                            // then add it to the collection
                            theObj.GetType().GetMethod("Add").Invoke(theObj, new object[] { obj });
                            int count = (int)theObj.GetType().GetProperty("Count").GetValue(theObj);
                            try
                            {
                                string nameStr = getDisplayName(obj, "");
                                PropertyInfo thisPi = obj.GetType().GetProperty("name");
                                if (thisPi != null)
                                    thisPi.SetValue(obj, nameStr);
                            }
                            catch { }

                            if (robotElementObj is mechanism)
                            {
                                if (mechanismInstancesNode == null)
                                {
                                    tn = AddNode(lastSelectedValueNode, theObj, name);
                                    mechanismInstancesNode = tn;
                                }
                                else
                                    tn = AddNode(mechanismInstancesNode, obj, name + (count - 1));
                            }
                            else
                            {
                                tn = AddNode(lastSelectedValueNode, theObj, name);
                            }
                        }
                        else
                        {
                            // it is not part of a collection, set the value only if it is null
                            if (theObj == null)
                            {
                                pi.SetValue(nonLeafNodeTag.getObject(lastSelectedValueNode.Tag), obj);
                                tn = AddNode(lastSelectedValueNode, obj, name);
                            }
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
                    foreach (object robotElementObj in robotElementCheckedListBox.CheckedItems) // there should only be mechanisms in the checkedItems list 
                    {
                        // first create a new element instance
                        if (robotElementObj is mechanism)
                        {
                            Type elementType = ((mechanism)robotElementObj).GetType();

                            object obj = Activator.CreateInstance((new mechanismInstance()).GetType());
                            ((mechanismInstance)obj).mechanism = applicationDataConfig.DeepClone((mechanism)robotElementObj);

                            // then add it to the collection
                            nonLeafNodeTag.getObject(lastSelectedArrayNode.Tag).GetType().GetMethod("Add").Invoke(lastSelectedArrayNode.Tag, new object[] { obj });
                            int count = (int)nonLeafNodeTag.getObject(lastSelectedArrayNode.Tag).GetType().GetProperty("Count").GetValue(lastSelectedArrayNode.Tag);

                            AddNode(lastSelectedArrayNode, obj, elementType.Name + (count - 1));
                        }
                    }
                }
                else
                {
                    // first create a new instance

                    if (theAppDataConfiguration.isASubClassedCollection(nonLeafNodeTag.getObject(lastSelectedArrayNode.Tag).GetType()))
                    {
                        foreach (object robotElementObj in robotElementCheckedListBox.CheckedItems)
                        {
                            Type elementType = ((robotElementType)robotElementObj).t;

                            object obj = Activator.CreateInstance(elementType);

                            // then add it to the collection
                            nonLeafNodeTag.getType(lastSelectedArrayNode.Tag).GetMethod("Add").Invoke(nonLeafNodeTag.getObject(lastSelectedArrayNode.Tag), new object[] { obj });
                            int count = (int)nonLeafNodeTag.getType(lastSelectedArrayNode.Tag).GetProperty("Count").GetValue(nonLeafNodeTag.getObject(lastSelectedArrayNode.Tag));

                            try
                            {
                                string nameStr = obj.GetType().GetProperty("name").GetValue(obj).ToString();
                                obj.GetType().GetProperty("name").SetValue(obj, nameStr + "_" + count);
                            }
                            catch { }

                            AddNode(lastSelectedArrayNode, obj, elementType.Name + (count - 1));
                        }
                    }
                    else
                    {
                        Type elementType = nonLeafNodeTag.getType(lastSelectedArrayNode.Tag).GetGenericArguments().Single();
                        object obj = Activator.CreateInstance(elementType);

                        // then add it to the collection
                        nonLeafNodeTag.getType(lastSelectedArrayNode.Tag).GetMethod("Add").Invoke(nonLeafNodeTag.getObject(lastSelectedArrayNode.Tag), new object[] { obj });
                        int count = (int)nonLeafNodeTag.getType(lastSelectedArrayNode.Tag).GetProperty("Count").GetValue(nonLeafNodeTag.getObject(lastSelectedArrayNode.Tag));

                        try
                        {
                            string nameStr = obj.GetType().GetProperty("name").GetValue(obj).ToString();
                            obj.GetType().GetProperty("name").SetValue(obj, nameStr + "_" + count);
                        }
                        catch { }

                        AddNode(lastSelectedArrayNode, obj, elementType.Name + (count - 1));
                    }
                }

                setNeedsSaving();

                mechanism theMechanism;
                if (isPartOfAMechanismTemplate(lastSelectedArrayNode, out theMechanism))
                    updateMechInstancesFromMechTemplate(theMechanism);
            }
        }

        void updateMechInstancesFromMechTemplate(mechanism theMechanism)
        {
            foreach (applicationData r in theAppDataConfiguration.theRobotVariants.Robots)
            {
                foreach (mechanismInstance mi in r.mechanismInstance)
                {
                    if (mi.mechanism.name == theMechanism.name)
                    {
                        mechanism m = applicationDataConfig.DeepClone(theMechanism);

                        theAppDataConfiguration.MergeMechanismParametersIntoStructure(m, mi.mechanism);

                        ((TreeNode)mi.mechanism.theTreeNode).Remove();

                        mi.mechanism = m;
                        mi.mechanism.theTreeNode = AddNode((TreeNode)mi.theTreeNode, mi.mechanism, mi.mechanism.name.value__);
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

                if (indexOfMechanism != -1)
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
            if (tn != null)
            {
                string fullPath = tn.FullPath;

                string mechInstancesName = @"\mechanismInstances\";
                int index = fullPath.IndexOf(mechInstancesName);
                if (index == -1)
                    return false;

                fullPath = fullPath.Substring(index + mechInstancesName.Length);

                string[] parts = fullPath.Split('\\');
                if (parts.Length <= 1)
                    return false;

                if (parts.Length > 2)
                    return true;

                if ("name (" + parts[0] + ")" == parts[1])
                    return false;

                return true;
            }

            return false;
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
                    return DataConfiguration.baseDataConfiguration.isACollection(nonLeafNodeTag.getObject(parent.Tag));
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
                        else if (tn.Tag is nonLeafNodeTag)
                            theObjectToDelete = ((nonLeafNodeTag)tn.Tag).obj;

                        if (theObjectToDelete != null)
                        {
                            bool updateMechanismInstances = false;
                            mechanism theMechanism;
                            if (isPartOfAMechanismTemplate(tn, out theMechanism))
                                updateMechanismInstances = true;

                            nonLeafNodeTag.getObject(parent.Tag).GetType().GetMethod("Remove").Invoke(nonLeafNodeTag.getObject(parent.Tag), new object[] { theObjectToDelete });
                            tn.Remove();
                            setNeedsSaving();

                            if ((int)nonLeafNodeTag.getObject(parent.Tag).GetType().GetProperty("Count").GetValue(nonLeafNodeTag.getObject(parent.Tag)) == 0)
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

        private void createNewAppDataConfigButton_Click(object sender, EventArgs e)
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
                        topLevelAppDataElement newAppDataConfig = new topLevelAppDataElement();

                        var mySerializer = new XmlSerializer(typeof(topLevelAppDataElement));
                        mySerializer.Serialize(myFileStream, newAppDataConfig);
                    }

                    Uri uriNewFile = new Uri(dlg.FileName);
                    Uri uriConfigFilePath = new Uri(configurationFilePathNameTextBox.Text);
                    string realtivePath = uriConfigFilePath.MakeRelativeUri(uriNewFile).ToString();
                    if (!generatorConfig.appDataConfigurations.Contains(realtivePath))
                        generatorConfig.appDataConfigurations.Add(realtivePath);

                    int index = robotConfigurationFileComboBox.Items.IndexOf(dlg.FileName);
                    if (index == -1)
                    {
                        index = robotConfigurationFileComboBox.Items.Add(dlg.FileName);
                    }
                    robotConfigurationFileComboBox.SelectedIndex = index;

                    saveGeneratorConfig(Path.GetDirectoryName(configurationFilePathNameTextBox.Text));
                }
            }
            catch (Exception ex)
            {
                addProgress(ex.Message);
            }
        }

        private TreeNode getNode(string fullPath)
        {
            TreeNode node = null;
            List<string> splitPath = fullPath.Split('\\').ToList();
            int currentIndex = 0;

            foreach (TreeNode tn in robotTreeView.Nodes)
            {
                if (tn.Text == splitPath[currentIndex])
                {
                    if (currentIndex == splitPath.Count - 1)
                        node = tn;
                    else
                        node = selectNode(splitPath, tn, ++currentIndex);
                    break;
                }
            }

            return node;
        }

        private TreeNode selectNode(List<string> splitPath, TreeNode currentNode, int currentIndex)
        {
            TreeNode node = null;

            foreach (TreeNode tn in currentNode.Nodes)
            {
                string name = tn.Text;

                if (name == splitPath[currentIndex])
                {
                    if (currentIndex == splitPath.Count - 1)
                        node = tn;
                    else
                        node = selectNode(splitPath, tn, ++currentIndex);
                    break;
                }
            }

            return node;
        }

        private void selectNodeButton_Click(object sender, EventArgs e)
        {
            string nodePath = infoIOtextBox.Text;
            TreeNode n = getNode(nodePath);

            if (n != null)
            {
                robotTreeView.SelectedNode = n;
                n.EnsureVisible();
            }
        }
        private void getCheckBoxListItemsButton_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (robotElementType ret in robotElementCheckedListBox.Items)
            {
                sb.Append("#" + ret.name);
            }

            infoIOtextBox.Text = sb.ToString();
        }
        private void getSelectedTreeElementPathButton_Click(object sender, EventArgs e)
        {
            infoIOtextBox.Text = robotTreeView.SelectedNode == null ? "" : robotTreeView.SelectedNode.FullPath;
        }
        private void checkCheckBoxListItemButton_Click(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(infoIOtextBox.Text);
                robotElementCheckedListBox.SetItemChecked(index, true);
            }
            catch
            {

            }
        }

        private void tuningButton_Click(object sender, EventArgs e)
        {
            if (viewer != null)
                viewer.ConnectToNetworkTables();
        }
    }

    class valueRange
    {
        public double minRange { get; set; }
        public double maxRange { get; set; }
    }

    class defaultValue
    {
        public object value { get; set; }
    }

    class nonLeafNodeTag
    {
        public string name { get; private set; }
        public object obj { get; set; }
        public nonLeafNodeTag(string name, object obj)
        {
            this.name = name;
            this.obj = obj;
        }

        static public object getObject(object nlnt)
        {
            if (nlnt == null)
                return null;

            return ((nonLeafNodeTag)nlnt).obj;
        }
        static public Type getType(object nlnt)
        {
            if (nlnt == null)
                return null;

            return ((nonLeafNodeTag)nlnt).obj.GetType();
        }
        static public string getName(object nlnt)
        {
            if (nlnt == null)
                return null;

            return ((nonLeafNodeTag)nlnt).name;
        }
    }

    class leafNodeTag
    {
        public Type type { get; private set; }
        public string name { get; private set; }
        public object obj { get; set; }
        public bool isConstant { get; private set; }
        public bool isTunable { get; private set; }
        public physicalUnit.Family unitsFamily { get; set; }
        public string physicalUnits { get; set; }
        public valueRange range { get; private set; }
        public defaultValue theDefault { get; private set; }

        public leafNodeTag(Type type, string name, object obj, bool isConstant, bool isTunable, physicalUnit.Family unitsFamily, string physicalUnits)
        {
            this.type = type;
            this.name = name;
            this.obj = obj;
            this.isConstant = isConstant;
            this.isTunable = isTunable;
            this.unitsFamily = unitsFamily;
            this.physicalUnits = physicalUnits;
            range = null;
            theDefault = null;
        }
        public void setRange(double min, double max)
        {
            range = new valueRange();
            range.minRange = min;
            range.maxRange = max;
        }
        public void setDefault(object value)
        {
            theDefault = new defaultValue();
            theDefault.value = value;
        }
    }

    class robotElementType
    {
        public Type t;
        public string name;
        public object theObject;

        public robotElementType(Type t)
        {
            this.t = t;

            string s = t.ToString();
            int indexlastDot = s.LastIndexOf('.');
            s = s.Substring(indexlastDot + 1);
            name = s.TrimEnd(']');
            theObject = null;
        }

        public robotElementType(Type t, string name)
        {
            this.t = t;
            this.name = name;
            theObject = null;
        }

        public robotElementType(Type t, mechanism m)
        {
            this.t = t;
            this.name = m.name.value__;
            theObject = m;
        }

        public override string ToString()
        {
            return name;
        }
    }
}

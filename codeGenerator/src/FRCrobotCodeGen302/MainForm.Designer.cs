
namespace FRCrobotCodeGen302
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.configurationBrowseButton = new System.Windows.Forms.Button();
            this.configurationFilePathNameTextBox = new System.Windows.Forms.TextBox();
            this.outputFolderLabel = new System.Windows.Forms.Label();
            this.configuredOutputFolderLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.robotConfigurationFileComboBox = new System.Windows.Forms.ComboBox();
            this.createNewRobotVariantsConfigButton = new System.Windows.Forms.Button();
            this.progressTextBox = new System.Windows.Forms.TextBox();
            this.theTabControl = new System.Windows.Forms.TabControl();
            this.tabMainPage = new System.Windows.Forms.TabPage();
            this.clearReportButton = new System.Windows.Forms.Button();
            this.tabConfigurationPage = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.robotTreeView = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.selectNodeButton = new System.Windows.Forms.Button();
            this.getSelectedTreeElementPathButton = new System.Windows.Forms.Button();
            this.getCheckBoxListItemsButton = new System.Windows.Forms.Button();
            this.checkCheckBoxListItemButton = new System.Windows.Forms.Button();
            this.infoIOtextBox = new System.Windows.Forms.TextBox();
            this.selectedNodePathTextBox = new System.Windows.Forms.TextBox();
            this.addRobotElementLabel = new System.Windows.Forms.Label();
            this.deleteTreeElementButton = new System.Windows.Forms.Button();
            this.addTreeElementButton = new System.Windows.Forms.Button();
            this.saveConfigBbutton = new System.Windows.Forms.Button();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.physicalUnitsTextBox = new System.Windows.Forms.TextBox();
            this.valueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.valueComboBox = new System.Windows.Forms.ComboBox();
            this.robotElementCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.treeViewIcons = new System.Windows.Forms.ImageList(this.components);
            this.tuningButton = new System.Windows.Forms.Button();
            this.configurationGroupBox.SuspendLayout();
            this.theTabControl.SuspendLayout();
            this.tabMainPage.SuspendLayout();
            this.tabConfigurationPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valueNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(681, 324);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 39);
            this.button1.TabIndex = 0;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(516, 335);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(67, 17);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // configurationBrowseButton
            // 
            this.configurationBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.configurationBrowseButton.Location = new System.Drawing.Point(700, 11);
            this.configurationBrowseButton.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.configurationBrowseButton.Name = "configurationBrowseButton";
            this.configurationBrowseButton.Size = new System.Drawing.Size(76, 28);
            this.configurationBrowseButton.TabIndex = 3;
            this.configurationBrowseButton.Text = "Browse";
            this.configurationBrowseButton.UseVisualStyleBackColor = true;
            this.configurationBrowseButton.Click += new System.EventHandler(this.configurationBrowseButton_Click);
            // 
            // configurationFilePathNameTextBox
            // 
            this.configurationFilePathNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.configurationFilePathNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.configurationFilePathNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.configurationFilePathNameTextBox.Location = new System.Drawing.Point(159, 12);
            this.configurationFilePathNameTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.configurationFilePathNameTextBox.Multiline = true;
            this.configurationFilePathNameTextBox.Name = "configurationFilePathNameTextBox";
            this.configurationFilePathNameTextBox.Size = new System.Drawing.Size(529, 27);
            this.configurationFilePathNameTextBox.TabIndex = 4;
            // 
            // outputFolderLabel
            // 
            this.outputFolderLabel.AutoSize = true;
            this.outputFolderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputFolderLabel.Location = new System.Drawing.Point(23, 31);
            this.outputFolderLabel.Name = "outputFolderLabel";
            this.outputFolderLabel.Size = new System.Drawing.Size(91, 17);
            this.outputFolderLabel.TabIndex = 5;
            this.outputFolderLabel.Text = "Output folder";
            // 
            // configuredOutputFolderLabel
            // 
            this.configuredOutputFolderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.configuredOutputFolderLabel.BackColor = System.Drawing.SystemColors.Control;
            this.configuredOutputFolderLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.configuredOutputFolderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.configuredOutputFolderLabel.Location = new System.Drawing.Point(145, 31);
            this.configuredOutputFolderLabel.Name = "configuredOutputFolderLabel";
            this.configuredOutputFolderLabel.Size = new System.Drawing.Size(606, 24);
            this.configuredOutputFolderLabel.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Gen config file";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(27, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Robot config";
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.configurationGroupBox.Controls.Add(this.robotConfigurationFileComboBox);
            this.configurationGroupBox.Controls.Add(this.configuredOutputFolderLabel);
            this.configurationGroupBox.Controls.Add(this.label2);
            this.configurationGroupBox.Controls.Add(this.outputFolderLabel);
            this.configurationGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.configurationGroupBox.Location = new System.Drawing.Point(9, 44);
            this.configurationGroupBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.configurationGroupBox.Size = new System.Drawing.Size(765, 106);
            this.configurationGroupBox.TabIndex = 8;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // robotConfigurationFileComboBox
            // 
            this.robotConfigurationFileComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.robotConfigurationFileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.robotConfigurationFileComboBox.FormattingEnabled = true;
            this.robotConfigurationFileComboBox.Location = new System.Drawing.Point(145, 65);
            this.robotConfigurationFileComboBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.robotConfigurationFileComboBox.Name = "robotConfigurationFileComboBox";
            this.robotConfigurationFileComboBox.Size = new System.Drawing.Size(607, 24);
            this.robotConfigurationFileComboBox.TabIndex = 8;
            this.robotConfigurationFileComboBox.SelectedIndexChanged += new System.EventHandler(this.robotConfigurationFileComboBox_SelectedIndexChanged);
            this.robotConfigurationFileComboBox.TextChanged += new System.EventHandler(this.robotConfigurationFileComboBox_TextChanged);
            // 
            // createNewRobotVariantsConfigButton
            // 
            this.createNewRobotVariantsConfigButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.createNewRobotVariantsConfigButton.Location = new System.Drawing.Point(200, 324);
            this.createNewRobotVariantsConfigButton.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.createNewRobotVariantsConfigButton.Name = "createNewRobotVariantsConfigButton";
            this.createNewRobotVariantsConfigButton.Size = new System.Drawing.Size(293, 39);
            this.createNewRobotVariantsConfigButton.TabIndex = 3;
            this.createNewRobotVariantsConfigButton.Text = "Create new robot variants configuration";
            this.createNewRobotVariantsConfigButton.UseVisualStyleBackColor = true;
            this.createNewRobotVariantsConfigButton.Click += new System.EventHandler(this.createNewRobotVariantsConfigButton_Click);
            // 
            // progressTextBox
            // 
            this.progressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressTextBox.Location = new System.Drawing.Point(9, 154);
            this.progressTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.progressTextBox.Multiline = true;
            this.progressTextBox.Name = "progressTextBox";
            this.progressTextBox.ReadOnly = true;
            this.progressTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.progressTextBox.Size = new System.Drawing.Size(765, 165);
            this.progressTextBox.TabIndex = 9;
            // 
            // theTabControl
            // 
            this.theTabControl.Controls.Add(this.tabMainPage);
            this.theTabControl.Controls.Add(this.tabConfigurationPage);
            this.theTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.theTabControl.Location = new System.Drawing.Point(0, 0);
            this.theTabControl.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.theTabControl.Name = "theTabControl";
            this.theTabControl.SelectedIndex = 0;
            this.theTabControl.Size = new System.Drawing.Size(788, 395);
            this.theTabControl.TabIndex = 10;
            // 
            // tabMainPage
            // 
            this.tabMainPage.Controls.Add(this.clearReportButton);
            this.tabMainPage.Controls.Add(this.createNewRobotVariantsConfigButton);
            this.tabMainPage.Controls.Add(this.button2);
            this.tabMainPage.Controls.Add(this.label1);
            this.tabMainPage.Controls.Add(this.button1);
            this.tabMainPage.Controls.Add(this.configurationFilePathNameTextBox);
            this.tabMainPage.Controls.Add(this.progressTextBox);
            this.tabMainPage.Controls.Add(this.configurationBrowseButton);
            this.tabMainPage.Controls.Add(this.configurationGroupBox);
            this.tabMainPage.Location = new System.Drawing.Point(4, 25);
            this.tabMainPage.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tabMainPage.Name = "tabMainPage";
            this.tabMainPage.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tabMainPage.Size = new System.Drawing.Size(780, 366);
            this.tabMainPage.TabIndex = 0;
            this.tabMainPage.Text = "Main";
            this.tabMainPage.UseVisualStyleBackColor = true;
            // 
            // clearReportButton
            // 
            this.clearReportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearReportButton.Location = new System.Drawing.Point(9, 324);
            this.clearReportButton.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.clearReportButton.Name = "clearReportButton";
            this.clearReportButton.Size = new System.Drawing.Size(164, 39);
            this.clearReportButton.TabIndex = 10;
            this.clearReportButton.Text = "Clear report window";
            this.clearReportButton.UseVisualStyleBackColor = true;
            this.clearReportButton.Click += new System.EventHandler(this.clearReportButton_Click);
            // 
            // tabConfigurationPage
            // 
            this.tabConfigurationPage.Controls.Add(this.splitContainer1);
            this.tabConfigurationPage.Location = new System.Drawing.Point(4, 25);
            this.tabConfigurationPage.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tabConfigurationPage.Name = "tabConfigurationPage";
            this.tabConfigurationPage.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tabConfigurationPage.Size = new System.Drawing.Size(780, 366);
            this.tabConfigurationPage.TabIndex = 1;
            this.tabConfigurationPage.Text = "Configuration";
            this.tabConfigurationPage.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 1);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.robotTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2MinSize = 180;
            this.splitContainer1.Size = new System.Drawing.Size(774, 364);
            this.splitContainer1.SplitterDistance = 251;
            this.splitContainer1.TabIndex = 1;
            // 
            // robotTreeView
            // 
            this.robotTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.robotTreeView.HideSelection = false;
            this.robotTreeView.Location = new System.Drawing.Point(0, 0);
            this.robotTreeView.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.robotTreeView.Name = "robotTreeView";
            this.robotTreeView.Size = new System.Drawing.Size(251, 364);
            this.robotTreeView.TabIndex = 0;
            this.robotTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.robotTreeView_AfterSelect);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tuningButton);
            this.panel1.Controls.Add(this.selectNodeButton);
            this.panel1.Controls.Add(this.getSelectedTreeElementPathButton);
            this.panel1.Controls.Add(this.getCheckBoxListItemsButton);
            this.panel1.Controls.Add(this.checkCheckBoxListItemButton);
            this.panel1.Controls.Add(this.infoIOtextBox);
            this.panel1.Controls.Add(this.selectedNodePathTextBox);
            this.panel1.Controls.Add(this.addRobotElementLabel);
            this.panel1.Controls.Add(this.deleteTreeElementButton);
            this.panel1.Controls.Add(this.addTreeElementButton);
            this.panel1.Controls.Add(this.saveConfigBbutton);
            this.panel1.Controls.Add(this.valueTextBox);
            this.panel1.Controls.Add(this.physicalUnitsTextBox);
            this.panel1.Controls.Add(this.valueNumericUpDown);
            this.panel1.Controls.Add(this.valueComboBox);
            this.panel1.Controls.Add(this.robotElementCheckedListBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(519, 364);
            this.panel1.TabIndex = 2;
            // 
            // selectNodeButton
            // 
            this.selectNodeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectNodeButton.Enabled = false;
            this.selectNodeButton.Location = new System.Drawing.Point(405, 205);
            this.selectNodeButton.Margin = new System.Windows.Forms.Padding(4);
            this.selectNodeButton.Name = "selectNodeButton";
            this.selectNodeButton.Size = new System.Drawing.Size(109, 32);
            this.selectNodeButton.TabIndex = 10;
            this.selectNodeButton.Text = "Select Node";
            this.selectNodeButton.UseVisualStyleBackColor = true;
            this.selectNodeButton.Visible = false;
            this.selectNodeButton.Click += new System.EventHandler(this.selectNodeButton_Click);
            // 
            // getSelectedTreeElementPathButton
            // 
            this.getSelectedTreeElementPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.getSelectedTreeElementPathButton.Enabled = false;
            this.getSelectedTreeElementPathButton.Location = new System.Drawing.Point(405, 114);
            this.getSelectedTreeElementPathButton.Margin = new System.Windows.Forms.Padding(4);
            this.getSelectedTreeElementPathButton.Name = "getSelectedTreeElementPathButton";
            this.getSelectedTreeElementPathButton.Size = new System.Drawing.Size(109, 32);
            this.getSelectedTreeElementPathButton.TabIndex = 10;
            this.getSelectedTreeElementPathButton.Text = "Get Selected node path";
            this.getSelectedTreeElementPathButton.UseVisualStyleBackColor = true;
            this.getSelectedTreeElementPathButton.Visible = false;
            this.getSelectedTreeElementPathButton.Click += new System.EventHandler(this.getSelectedTreeElementPathButton_Click);
            // 
            // getCheckBoxListItemsButton
            // 
            this.getCheckBoxListItemsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.getCheckBoxListItemsButton.Enabled = false;
            this.getCheckBoxListItemsButton.Location = new System.Drawing.Point(405, 175);
            this.getCheckBoxListItemsButton.Margin = new System.Windows.Forms.Padding(4);
            this.getCheckBoxListItemsButton.Name = "getCheckBoxListItemsButton";
            this.getCheckBoxListItemsButton.Size = new System.Drawing.Size(109, 32);
            this.getCheckBoxListItemsButton.TabIndex = 10;
            this.getCheckBoxListItemsButton.Text = "CheckList Items";
            this.getCheckBoxListItemsButton.UseVisualStyleBackColor = true;
            this.getCheckBoxListItemsButton.Visible = false;
            this.getCheckBoxListItemsButton.Click += new System.EventHandler(this.getCheckBoxListItemsButton_Click);
            // 
            // checkCheckBoxListItemButton
            // 
            this.checkCheckBoxListItemButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkCheckBoxListItemButton.Enabled = false;
            this.checkCheckBoxListItemButton.Location = new System.Drawing.Point(405, 138);
            this.checkCheckBoxListItemButton.Margin = new System.Windows.Forms.Padding(4);
            this.checkCheckBoxListItemButton.Name = "checkCheckBoxListItemButton";
            this.checkCheckBoxListItemButton.Size = new System.Drawing.Size(109, 32);
            this.checkCheckBoxListItemButton.TabIndex = 10;
            this.checkCheckBoxListItemButton.Text = "Check Item";
            this.checkCheckBoxListItemButton.UseVisualStyleBackColor = true;
            this.checkCheckBoxListItemButton.Visible = false;
            this.checkCheckBoxListItemButton.Click += new System.EventHandler(this.checkCheckBoxListItemButton_Click);
            // 
            // infoIOtextBox
            // 
            this.infoIOtextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoIOtextBox.Enabled = false;
            this.infoIOtextBox.Location = new System.Drawing.Point(4, 209);
            this.infoIOtextBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.infoIOtextBox.Name = "infoIOtextBox";
            this.infoIOtextBox.Size = new System.Drawing.Size(401, 22);
            this.infoIOtextBox.TabIndex = 9;
            this.infoIOtextBox.Visible = false;
            // 
            // selectedNodePathTextBox
            // 
            this.selectedNodePathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedNodePathTextBox.Enabled = false;
            this.selectedNodePathTextBox.Location = new System.Drawing.Point(4, 173);
            this.selectedNodePathTextBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.selectedNodePathTextBox.Name = "selectedNodePathTextBox";
            this.selectedNodePathTextBox.Size = new System.Drawing.Size(401, 22);
            this.selectedNodePathTextBox.TabIndex = 9;
            this.selectedNodePathTextBox.Visible = false;
            // 
            // addRobotElementLabel
            // 
            this.addRobotElementLabel.AutoSize = true;
            this.addRobotElementLabel.Location = new System.Drawing.Point(4, 5);
            this.addRobotElementLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.addRobotElementLabel.Name = "addRobotElementLabel";
            this.addRobotElementLabel.Size = new System.Drawing.Size(121, 16);
            this.addRobotElementLabel.TabIndex = 8;
            this.addRobotElementLabel.Text = "Select items to add";
            this.addRobotElementLabel.Visible = false;
            // 
            // deleteTreeElementButton
            // 
            this.deleteTreeElementButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteTreeElementButton.Location = new System.Drawing.Point(310, 296);
            this.deleteTreeElementButton.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.deleteTreeElementButton.Name = "deleteTreeElementButton";
            this.deleteTreeElementButton.Size = new System.Drawing.Size(204, 31);
            this.deleteTreeElementButton.TabIndex = 6;
            this.deleteTreeElementButton.Text = "Delete";
            this.deleteTreeElementButton.UseVisualStyleBackColor = true;
            this.deleteTreeElementButton.Click += new System.EventHandler(this.deleteTreeElementButton_Click);
            // 
            // addTreeElementButton
            // 
            this.addTreeElementButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addTreeElementButton.Enabled = false;
            this.addTreeElementButton.Location = new System.Drawing.Point(310, 264);
            this.addTreeElementButton.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.addTreeElementButton.Name = "addTreeElementButton";
            this.addTreeElementButton.Size = new System.Drawing.Size(204, 31);
            this.addTreeElementButton.TabIndex = 4;
            this.addTreeElementButton.Text = "Add";
            this.addTreeElementButton.UseVisualStyleBackColor = true;
            this.addTreeElementButton.Click += new System.EventHandler(this.addTreeElementButton_Click);
            // 
            // saveConfigBbutton
            // 
            this.saveConfigBbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveConfigBbutton.Location = new System.Drawing.Point(422, 329);
            this.saveConfigBbutton.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.saveConfigBbutton.Name = "saveConfigBbutton";
            this.saveConfigBbutton.Size = new System.Drawing.Size(92, 31);
            this.saveConfigBbutton.TabIndex = 2;
            this.saveConfigBbutton.Text = "Save";
            this.saveConfigBbutton.UseVisualStyleBackColor = true;
            this.saveConfigBbutton.Click += new System.EventHandler(this.saveConfigBbutton_Click);
            // 
            // valueTextBox
            // 
            this.valueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.valueTextBox.Location = new System.Drawing.Point(101, 241);
            this.valueTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.Size = new System.Drawing.Size(203, 22);
            this.valueTextBox.TabIndex = 0;
            this.valueTextBox.Visible = false;
            this.valueTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // physicalUnitsTextBox
            // 
            this.physicalUnitsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.physicalUnitsTextBox.Location = new System.Drawing.Point(262, 241);
            this.physicalUnitsTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.physicalUnitsTextBox.Name = "physicalUnitsTextBox";
            this.physicalUnitsTextBox.ReadOnly = true;
            this.physicalUnitsTextBox.Size = new System.Drawing.Size(65, 22);
            this.physicalUnitsTextBox.TabIndex = 0;
            this.physicalUnitsTextBox.Visible = false;
            // 
            // valueNumericUpDown
            // 
            this.valueNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.valueNumericUpDown.Location = new System.Drawing.Point(310, 237);
            this.valueNumericUpDown.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.valueNumericUpDown.Name = "valueNumericUpDown";
            this.valueNumericUpDown.Size = new System.Drawing.Size(204, 22);
            this.valueNumericUpDown.TabIndex = 3;
            this.valueNumericUpDown.Visible = false;
            this.valueNumericUpDown.ValueChanged += new System.EventHandler(this.valueNumericUpDown_ValueChanged);
            // 
            // valueComboBox
            // 
            this.valueComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.valueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.valueComboBox.FormattingEnabled = true;
            this.valueComboBox.Location = new System.Drawing.Point(101, 268);
            this.valueComboBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.valueComboBox.Name = "valueComboBox";
            this.valueComboBox.Size = new System.Drawing.Size(203, 24);
            this.valueComboBox.TabIndex = 1;
            this.valueComboBox.Visible = false;
            this.valueComboBox.SelectedValueChanged += new System.EventHandler(this.valueComboBox_SelectedValueChanged);
            // 
            // robotElementCheckedListBox
            // 
            this.robotElementCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.robotElementCheckedListBox.FormattingEnabled = true;
            this.robotElementCheckedListBox.Location = new System.Drawing.Point(3, 30);
            this.robotElementCheckedListBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.robotElementCheckedListBox.Name = "robotElementCheckedListBox";
            this.robotElementCheckedListBox.Size = new System.Drawing.Size(510, 157);
            this.robotElementCheckedListBox.TabIndex = 7;
            this.robotElementCheckedListBox.Visible = false;
            // 
            // treeViewIcons
            // 
            this.treeViewIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeViewIcons.ImageStream")));
            this.treeViewIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.treeViewIcons.Images.SetKeyName(0, "lock.ico");
            this.treeViewIcons.Images.SetKeyName(1, "lock-unlock.ico");
            this.treeViewIcons.Images.SetKeyName(2, "gear.ico");
            this.treeViewIcons.Images.SetKeyName(3, "wrench.ico");
            // 
            // tuningButton
            // 
            this.tuningButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tuningButton.BackColor = System.Drawing.Color.IndianRed;
            this.tuningButton.Location = new System.Drawing.Point(310, 331);
            this.tuningButton.Name = "tuningButton";
            this.tuningButton.Size = new System.Drawing.Size(106, 27);
            this.tuningButton.TabIndex = 12;
            this.tuningButton.Text = "Toggle Tuning";
            this.tuningButton.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 395);
            this.Controls.Add(this.theTabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.Name = "MainForm";
            this.Text = "Team 302 code generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.theTabControl.ResumeLayout(false);
            this.tabMainPage.ResumeLayout(false);
            this.tabMainPage.PerformLayout();
            this.tabConfigurationPage.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valueNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button configurationBrowseButton;
        private System.Windows.Forms.Button createNewRobotVariantsConfigButton;
        private System.Windows.Forms.TextBox configurationFilePathNameTextBox;
        private System.Windows.Forms.Label outputFolderLabel;
        private System.Windows.Forms.Label configuredOutputFolderLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.TextBox progressTextBox;
        private System.Windows.Forms.TabControl theTabControl;
        private System.Windows.Forms.TabPage tabMainPage;
        private System.Windows.Forms.TabPage tabConfigurationPage;
        private System.Windows.Forms.TreeView robotTreeView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ComboBox valueComboBox;
        private System.Windows.Forms.TextBox valueTextBox;
        private System.Windows.Forms.TextBox physicalUnitsTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button saveConfigBbutton;
        private System.Windows.Forms.NumericUpDown valueNumericUpDown;
        private System.Windows.Forms.Button addTreeElementButton;
        private System.Windows.Forms.ComboBox robotConfigurationFileComboBox;
        private System.Windows.Forms.Button clearReportButton;
        private System.Windows.Forms.Button deleteTreeElementButton;
        private System.Windows.Forms.CheckedListBox robotElementCheckedListBox;
        private System.Windows.Forms.Label addRobotElementLabel;
        private System.Windows.Forms.ImageList treeViewIcons;
        private System.Windows.Forms.Button selectNodeButton;
        private System.Windows.Forms.Button getSelectedTreeElementPathButton;
        private System.Windows.Forms.Button getCheckBoxListItemsButton;
        private System.Windows.Forms.Button checkCheckBoxListItemButton;
        private System.Windows.Forms.TextBox infoIOtextBox;
        private System.Windows.Forms.TextBox selectedNodePathTextBox;
        private System.Windows.Forms.Button tuningButton;
    }
}


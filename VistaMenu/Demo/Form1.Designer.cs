namespace VistaMenuDemo
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.mnuRename = new System.Windows.Forms.MenuItem();
            this.mnuAddFiles = new System.Windows.Forms.MenuItem();
            this.mnuAddFolder = new System.Windows.Forms.MenuItem();
            this.mnuNewFolder = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.mnuRemoveFolder = new System.Windows.Forms.MenuItem();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.mnuFile = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.mnuEdit = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.mnuTools = new System.Windows.Forms.MenuItem();
            this.mnuOptions = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.btnSetOptionsImage = new System.Windows.Forms.Button();
            this.btnClearOptionsImage = new System.Windows.Forms.Button();
            this.vistaMenu = new wyDay.Controls.VistaMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.vistaMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuRename,
            this.mnuAddFiles,
            this.mnuAddFolder,
            this.mnuNewFolder,
            this.menuItem12,
            this.mnuRemoveFolder});
            // 
            // mnuRename
            // 
            this.vistaMenu.SetImage(this.mnuRename, ((System.Drawing.Image)(resources.GetObject("mnuRename.Image"))));
            this.mnuRename.Index = 0;
            this.mnuRename.Text = "Rename";
            // 
            // mnuAddFiles
            // 
            this.vistaMenu.SetImage(this.mnuAddFiles, ((System.Drawing.Image)(resources.GetObject("mnuAddFiles.Image"))));
            this.mnuAddFiles.Index = 1;
            this.mnuAddFiles.Text = "Add Files";
            // 
            // mnuAddFolder
            // 
            this.vistaMenu.SetImage(this.mnuAddFolder, ((System.Drawing.Image)(resources.GetObject("mnuAddFolder.Image"))));
            this.mnuAddFolder.Index = 2;
            this.mnuAddFolder.Text = "Add Folder";
            // 
            // mnuNewFolder
            // 
            this.vistaMenu.SetImage(this.mnuNewFolder, ((System.Drawing.Image)(resources.GetObject("mnuNewFolder.Image"))));
            this.mnuNewFolder.Index = 3;
            this.mnuNewFolder.Text = "New Folder";
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 4;
            this.menuItem12.Text = "-";
            // 
            // mnuRemoveFolder
            // 
            this.vistaMenu.SetImage(this.mnuRemoveFolder, ((System.Drawing.Image)(resources.GetObject("mnuRemoveFolder.Image"))));
            this.mnuRemoveFolder.Index = 5;
            this.mnuRemoveFolder.Text = "Remove";
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFile,
            this.mnuEdit,
            this.mnuTools,
            this.menuItem8});
            // 
            // mnuFile
            // 
            this.mnuFile.Index = 0;
            this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem4,
            this.menuItem11,
            this.menuItem13,
            this.menuItem14,
            this.menuItem15,
            this.menuItem16,
            this.menuItem17});
            this.mnuFile.Text = "&File";
            // 
            // menuItem4
            // 
            this.vistaMenu.SetImage(this.menuItem4, ((System.Drawing.Image)(resources.GetObject("menuItem4.Image"))));
            this.menuItem4.Index = 0;
            this.menuItem4.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.menuItem4.Text = "&New Project";
            // 
            // menuItem11
            // 
            this.menuItem11.DefaultItem = true;
            this.vistaMenu.SetImage(this.menuItem11, ((System.Drawing.Image)(resources.GetObject("menuItem11.Image"))));
            this.menuItem11.Index = 1;
            this.menuItem11.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.menuItem11.Text = "&Open Project...";
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 2;
            this.menuItem13.Text = "&Close";
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 3;
            this.menuItem14.Text = "-";
            // 
            // menuItem15
            // 
            this.vistaMenu.SetImage(this.menuItem15, ((System.Drawing.Image)(resources.GetObject("menuItem15.Image"))));
            this.menuItem15.Index = 4;
            this.menuItem15.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.menuItem15.Text = "&Save";
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 5;
            this.menuItem16.Text = "-";
            // 
            // menuItem17
            // 
            this.menuItem17.Index = 6;
            this.menuItem17.Text = "E&xit";
            // 
            // mnuEdit
            // 
            this.mnuEdit.Index = 1;
            this.mnuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem6,
            this.menuItem7});
            this.mnuEdit.Text = "&Edit";
            // 
            // menuItem6
            // 
            this.vistaMenu.SetImage(this.menuItem6, ((System.Drawing.Image)(resources.GetObject("menuItem6.Image"))));
            this.menuItem6.Index = 0;
            this.menuItem6.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.menuItem6.Text = "&Undo";
            // 
            // menuItem7
            // 
            this.menuItem7.Enabled = false;
            this.vistaMenu.SetImage(this.menuItem7, ((System.Drawing.Image)(resources.GetObject("menuItem7.Image"))));
            this.menuItem7.Index = 1;
            this.menuItem7.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
            this.menuItem7.Text = "&Redo";
            // 
            // mnuTools
            // 
            this.mnuTools.Index = 2;
            this.mnuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuOptions});
            this.mnuTools.Text = "&Tools";
            // 
            // mnuOptions
            // 
            this.mnuOptions.Index = 0;
            this.mnuOptions.Text = "&Options...";
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 3;
            this.menuItem8.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem9,
            this.menuItem10,
            this.menuItem1,
            this.menuItem2});
            this.menuItem8.Text = "&Help";
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 0;
            this.menuItem9.RadioCheck = true;
            this.menuItem9.Text = "&Check for Updates";
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 1;
            this.menuItem10.Text = "-";
            // 
            // menuItem1
            // 
            this.vistaMenu.SetImage(this.menuItem1, ((System.Drawing.Image)(resources.GetObject("menuItem1.Image"))));
            this.menuItem1.Index = 2;
            this.menuItem1.Shortcut = System.Windows.Forms.Shortcut.F1;
            this.menuItem1.Text = "Online &Help";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 3;
            this.menuItem2.Text = "&About";
            // 
            // btnSetOptionsImage
            // 
            this.btnSetOptionsImage.Location = new System.Drawing.Point(12, 50);
            this.btnSetOptionsImage.Name = "btnSetOptionsImage";
            this.btnSetOptionsImage.Size = new System.Drawing.Size(177, 27);
            this.btnSetOptionsImage.TabIndex = 0;
            this.btnSetOptionsImage.Text = "Set \'Options\' menu item image";
            this.btnSetOptionsImage.UseVisualStyleBackColor = true;
            this.btnSetOptionsImage.Click += new System.EventHandler(this.btnSetOptionsImage_Click);
            // 
            // btnClearOptionsImage
            // 
            this.btnClearOptionsImage.Location = new System.Drawing.Point(12, 83);
            this.btnClearOptionsImage.Name = "btnClearOptionsImage";
            this.btnClearOptionsImage.Size = new System.Drawing.Size(177, 27);
            this.btnClearOptionsImage.TabIndex = 1;
            this.btnClearOptionsImage.Text = "Clear \'Options\' menu item image";
            this.btnClearOptionsImage.UseVisualStyleBackColor = true;
            this.btnClearOptionsImage.Click += new System.EventHandler(this.btnClearOptionsImage_Click);
            // 
            // vistaMenu
            // 
            this.vistaMenu.ContainerControl = this;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 132);
            this.Controls.Add(this.btnClearOptionsImage);
            this.Controls.Add(this.btnSetOptionsImage);
            this.Menu = this.mainMenu;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VistaMenu demo";
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.vistaMenu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem mnuRename;
        private System.Windows.Forms.MenuItem mnuAddFiles;
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem mnuFile;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem mnuEdit;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem mnuAddFolder;
        private System.Windows.Forms.MenuItem mnuNewFolder;
        private System.Windows.Forms.MenuItem menuItem12;
        private System.Windows.Forms.MenuItem mnuRemoveFolder;
        private System.Windows.Forms.MenuItem menuItem10;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem11;
        private System.Windows.Forms.MenuItem menuItem13;
        private System.Windows.Forms.MenuItem menuItem14;
        private System.Windows.Forms.MenuItem menuItem15;
        private System.Windows.Forms.MenuItem menuItem16;
        private System.Windows.Forms.MenuItem menuItem17;
        private System.Windows.Forms.MenuItem mnuTools;
        private System.Windows.Forms.MenuItem mnuOptions;
        private wyDay.Controls.VistaMenu vistaMenu;
        private System.Windows.Forms.Button btnSetOptionsImage;
        private System.Windows.Forms.Button btnClearOptionsImage;
    }
}


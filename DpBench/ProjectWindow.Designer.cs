namespace Paguru.DpBench
{
    partial class ProjectWindow
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
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnFilename = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnCamera = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnLens = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnAperture = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnExposure = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnIso = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnKeywords = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.contextMenuPhoto = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripProject = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupLevelEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.olvColumnFocalLength = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.contextMenuPhoto.SuspendLayout();
            this.contextMenuStripProject.SuspendLayout();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumnFilename);
            this.objectListView1.AllColumns.Add(this.olvColumnCamera);
            this.objectListView1.AllColumns.Add(this.olvColumnLens);
            this.objectListView1.AllColumns.Add(this.olvColumnAperture);
            this.objectListView1.AllColumns.Add(this.olvColumnExposure);
            this.objectListView1.AllColumns.Add(this.olvColumnFocalLength);
            this.objectListView1.AllColumns.Add(this.olvColumnIso);
            this.objectListView1.AllColumns.Add(this.olvColumnKeywords);
            this.objectListView1.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnFilename,
            this.olvColumnCamera,
            this.olvColumnLens,
            this.olvColumnAperture,
            this.olvColumnExposure,
            this.olvColumnFocalLength,
            this.olvColumnIso,
            this.olvColumnKeywords});
            this.objectListView1.ContextMenuStrip = this.contextMenuPhoto;
            this.objectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.GridLines = true;
            this.objectListView1.Location = new System.Drawing.Point(0, 0);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.Size = new System.Drawing.Size(625, 412);
            this.objectListView1.TabIndex = 1;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            this.objectListView1.SelectionChanged += new System.EventHandler(this.objectListView1_SelectionChanged);
            // 
            // olvColumnFilename
            // 
            this.olvColumnFilename.AspectName = "Filename";
            this.olvColumnFilename.CellPadding = null;
            this.olvColumnFilename.Groupable = false;
            this.olvColumnFilename.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColumnFilename.MinimumWidth = 100;
            this.olvColumnFilename.Text = "File";
            this.olvColumnFilename.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColumnFilename.Width = 100;
            // 
            // olvColumnCamera
            // 
            this.olvColumnCamera.AspectName = "Camera";
            this.olvColumnCamera.CellPadding = null;
            this.olvColumnCamera.MinimumWidth = 10;
            this.olvColumnCamera.Text = "Camera";
            // 
            // olvColumnLens
            // 
            this.olvColumnLens.AspectName = "Lens";
            this.olvColumnLens.CellPadding = null;
            this.olvColumnLens.DisplayIndex = 3;
            this.olvColumnLens.MinimumWidth = 10;
            this.olvColumnLens.Text = "Lens";
            // 
            // olvColumnAperture
            // 
            this.olvColumnAperture.AspectName = "Aperture";
            this.olvColumnAperture.CellPadding = null;
            this.olvColumnAperture.DisplayIndex = 2;
            this.olvColumnAperture.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColumnAperture.MinimumWidth = 10;
            this.olvColumnAperture.Text = "Aperture";
            this.olvColumnAperture.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // olvColumnExposure
            // 
            this.olvColumnExposure.AspectName = "Exposure";
            this.olvColumnExposure.CellPadding = null;
            this.olvColumnExposure.DisplayIndex = 5;
            this.olvColumnExposure.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColumnExposure.MinimumWidth = 10;
            this.olvColumnExposure.Text = "Exposure";
            this.olvColumnExposure.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // olvColumnIso
            // 
            this.olvColumnIso.AspectName = "Iso";
            this.olvColumnIso.CellPadding = null;
            this.olvColumnIso.DisplayIndex = 4;
            this.olvColumnIso.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColumnIso.MinimumWidth = 10;
            this.olvColumnIso.Text = "ISO";
            this.olvColumnIso.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // olvColumnKeywords
            // 
            this.olvColumnKeywords.AspectName = "Keywords";
            this.olvColumnKeywords.CellPadding = null;
            this.olvColumnKeywords.DisplayIndex = 6;
            this.olvColumnKeywords.MinimumWidth = 10;
            this.olvColumnKeywords.Text = "Keywords";
            // 
            // contextMenuPhoto
            // 
            this.contextMenuPhoto.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.contextMenuPhoto.Name = "contextMenuPhoto";
            this.contextMenuPhoto.Size = new System.Drawing.Size(153, 48);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // contextMenuStripProject
            // 
            this.contextMenuStripProject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFilesToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.groupLevelEditorToolStripMenuItem});
            this.contextMenuStripProject.Name = "contextMenuStrip1";
            this.contextMenuStripProject.Size = new System.Drawing.Size(172, 92);
            // 
            // addFilesToolStripMenuItem
            // 
            this.addFilesToolStripMenuItem.Name = "addFilesToolStripMenuItem";
            this.addFilesToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.addFilesToolStripMenuItem.Text = "Add File(s) ...";
            this.addFilesToolStripMenuItem.Click += new System.EventHandler(this.addFilesToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // groupLevelEditorToolStripMenuItem
            // 
            this.groupLevelEditorToolStripMenuItem.Name = "groupLevelEditorToolStripMenuItem";
            this.groupLevelEditorToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.groupLevelEditorToolStripMenuItem.Text = "Group Level Editor";
            this.groupLevelEditorToolStripMenuItem.Click += new System.EventHandler(this.groupLevelEditorToolStripMenuItem_Click);
            // 
            // olvColumnFocalLength
            // 
            this.olvColumnFocalLength.AspectName = "FocalLength";
            this.olvColumnFocalLength.CellPadding = null;
            this.olvColumnFocalLength.DisplayIndex = 7;
            this.olvColumnFocalLength.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColumnFocalLength.Text = "FocalLength";
            this.olvColumnFocalLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ProjectWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 412);
            this.Controls.Add(this.objectListView1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ProjectWindow";
            this.TabPageContextMenuStrip = this.contextMenuStripProject;
            this.Text = "ProjectWindow";
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.contextMenuPhoto.ResumeLayout(false);
            this.contextMenuStripProject.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumnFilename;
        private BrightIdeasSoftware.OLVColumn olvColumnAperture;
        private BrightIdeasSoftware.OLVColumn olvColumnCamera;
        private BrightIdeasSoftware.OLVColumn olvColumnLens;
        private BrightIdeasSoftware.OLVColumn olvColumnIso;
        private BrightIdeasSoftware.OLVColumn olvColumnExposure;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripProject;
        private System.Windows.Forms.ToolStripMenuItem addFilesToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuPhoto;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn olvColumnKeywords;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem groupLevelEditorToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn olvColumnFocalLength;
    }
}
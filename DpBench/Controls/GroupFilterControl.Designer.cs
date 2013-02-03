namespace Paguru.DpBench.Controls
{
    partial class GroupFilterControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBoxParameter = new System.Windows.Forms.ComboBox();
            this.buttonClose = new System.Windows.Forms.Label();
            this.SelectableValueListControl1 = new Paguru.DpBench.Controls.SelectableValueListControl();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // comboBoxParameter
            // 
            this.comboBoxParameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxParameter.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxParameter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxParameter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxParameter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxParameter.FormattingEnabled = true;
            this.comboBoxParameter.Location = new System.Drawing.Point(3, 4);
            this.comboBoxParameter.Name = "comboBoxParameter";
            this.comboBoxParameter.Size = new System.Drawing.Size(175, 21);
            this.comboBoxParameter.TabIndex = 1;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.AutoSize = true;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonClose.Image = global::Paguru.DpBench.Properties.Resources.x_21x21;
            this.buttonClose.Location = new System.Drawing.Point(188, 3);
            this.buttonClose.MinimumSize = new System.Drawing.Size(24, 24);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(24, 24);
            this.buttonClose.TabIndex = 2;
            this.toolTip1.SetToolTip(this.buttonClose, "delete group/filter");
            // 
            // SelectableValueListControl1
            // 
            this.SelectableValueListControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectableValueListControl1.Location = new System.Drawing.Point(3, 31);
            this.SelectableValueListControl1.Name = "SelectableValueListControl1";
            this.SelectableValueListControl1.Size = new System.Drawing.Size(205, 315);
            this.SelectableValueListControl1.TabIndex = 0;
            this.SelectableValueListControl1.Values = null;
            // 
            // GroupFilterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.comboBoxParameter);
            this.Controls.Add(this.SelectableValueListControl1);
            this.Name = "GroupFilterControl";
            this.Size = new System.Drawing.Size(212, 346);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public SelectableValueListControl SelectableValueListControl1;
        public System.Windows.Forms.ComboBox comboBoxParameter;
        public System.Windows.Forms.Label buttonClose;
        private System.Windows.Forms.ToolTip toolTip1;

    }
}

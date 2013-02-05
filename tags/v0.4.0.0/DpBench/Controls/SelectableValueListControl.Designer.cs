namespace Paguru.DpBench.Controls
{
    partial class SelectableValueListControl
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
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.labelNone = new System.Windows.Forms.Label();
            this.labelAll = new System.Windows.Forms.Label();
            this.labelUp = new System.Windows.Forms.Label();
            this.labelDown = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBox1.BackColor = System.Drawing.SystemColors.Window;
            this.checkedListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(1, 0);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(156, 255);
            this.checkedListBox1.TabIndex = 0;
            // 
            // labelNone
            // 
            this.labelNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNone.AutoSize = true;
            this.labelNone.Image = global::Paguru.DpBench.Properties.Resources.x_alt_16x16;
            this.labelNone.Location = new System.Drawing.Point(130, 267);
            this.labelNone.MinimumSize = new System.Drawing.Size(24, 24);
            this.labelNone.Name = "labelNone";
            this.labelNone.Size = new System.Drawing.Size(24, 24);
            this.labelNone.TabIndex = 9;
            this.toolTip1.SetToolTip(this.labelNone, "uncheck all");
            this.labelNone.Click += new System.EventHandler(this.labelNone_Click);
            // 
            // labelAll
            // 
            this.labelAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAll.AutoSize = true;
            this.labelAll.Image = global::Paguru.DpBench.Properties.Resources.check_alt_16x16;
            this.labelAll.Location = new System.Drawing.Point(100, 267);
            this.labelAll.MinimumSize = new System.Drawing.Size(24, 24);
            this.labelAll.Name = "labelAll";
            this.labelAll.Size = new System.Drawing.Size(24, 24);
            this.labelAll.TabIndex = 8;
            this.toolTip1.SetToolTip(this.labelAll, "check all");
            this.labelAll.Click += new System.EventHandler(this.labelAll_Click);
            // 
            // labelUp
            // 
            this.labelUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelUp.AutoSize = true;
            this.labelUp.Image = global::Paguru.DpBench.Properties.Resources.arrow_up_16x16;
            this.labelUp.Location = new System.Drawing.Point(4, 267);
            this.labelUp.MinimumSize = new System.Drawing.Size(24, 24);
            this.labelUp.Name = "labelUp";
            this.labelUp.Size = new System.Drawing.Size(24, 24);
            this.labelUp.TabIndex = 7;
            this.toolTip1.SetToolTip(this.labelUp, "move up (Crtl+Up)");
            this.labelUp.Click += new System.EventHandler(this.labelUp_Click);
            // 
            // labelDown
            // 
            this.labelDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDown.AutoSize = true;
            this.labelDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.labelDown.Image = global::Paguru.DpBench.Properties.Resources.arrow_down_16x16;
            this.labelDown.Location = new System.Drawing.Point(34, 267);
            this.labelDown.MinimumSize = new System.Drawing.Size(24, 24);
            this.labelDown.Name = "labelDown";
            this.labelDown.Size = new System.Drawing.Size(24, 24);
            this.labelDown.TabIndex = 6;
            this.toolTip1.SetToolTip(this.labelDown, "move down (Ctrl+Down)");
            this.labelDown.Click += new System.EventHandler(this.labelDown_Click);
            // 
            // SelectableValueListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelNone);
            this.Controls.Add(this.labelAll);
            this.Controls.Add(this.labelUp);
            this.Controls.Add(this.labelDown);
            this.Controls.Add(this.checkedListBox1);
            this.Name = "SelectableValueListControl";
            this.Size = new System.Drawing.Size(157, 294);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label labelDown;
        private System.Windows.Forms.Label labelUp;
        private System.Windows.Forms.Label labelAll;
        private System.Windows.Forms.Label labelNone;
    }
}

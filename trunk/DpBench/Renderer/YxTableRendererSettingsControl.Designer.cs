namespace Paguru.DpBench.Renderer
{
    partial class YxTableRendererSettingsControl
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
            this.radioButtonNoScale = new System.Windows.Forms.RadioButton();
            this.radioButtonScale = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericUpDownBBHeight = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownBBWidth = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownPadding = new System.Windows.Forms.NumericUpDown();
            this.linkLabelFont = new System.Windows.Forms.LinkLabel();
            this.buttonRender = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBBHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBBWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPadding)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButtonNoScale
            // 
            this.radioButtonNoScale.AutoSize = true;
            this.radioButtonNoScale.Location = new System.Drawing.Point(6, 19);
            this.radioButtonNoScale.Name = "radioButtonNoScale";
            this.radioButtonNoScale.Size = new System.Drawing.Size(40, 17);
            this.radioButtonNoScale.TabIndex = 0;
            this.radioButtonNoScale.TabStop = true;
            this.radioButtonNoScale.Text = "1:1";
            this.radioButtonNoScale.UseVisualStyleBackColor = true;
            // 
            // radioButtonScale
            // 
            this.radioButtonScale.AutoSize = true;
            this.radioButtonScale.Location = new System.Drawing.Point(6, 42);
            this.radioButtonScale.Name = "radioButtonScale";
            this.radioButtonScale.Size = new System.Drawing.Size(65, 17);
            this.radioButtonScale.TabIndex = 1;
            this.radioButtonScale.TabStop = true;
            this.radioButtonScale.Text = "shrink to";
            this.radioButtonScale.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(127, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "x";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numericUpDownBBHeight);
            this.groupBox1.Controls.Add(this.numericUpDownBBWidth);
            this.groupBox1.Controls.Add(this.radioButtonNoScale);
            this.groupBox1.Controls.Add(this.radioButtonScale);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 81);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tile size";
            // 
            // numericUpDownBBHeight
            // 
            this.numericUpDownBBHeight.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownBBHeight.Location = new System.Drawing.Point(142, 42);
            this.numericUpDownBBHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownBBHeight.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownBBHeight.Name = "numericUpDownBBHeight";
            this.numericUpDownBBHeight.Size = new System.Drawing.Size(52, 20);
            this.numericUpDownBBHeight.TabIndex = 6;
            this.numericUpDownBBHeight.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // numericUpDownBBWidth
            // 
            this.numericUpDownBBWidth.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownBBWidth.Location = new System.Drawing.Point(69, 42);
            this.numericUpDownBBWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownBBWidth.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownBBWidth.Name = "numericUpDownBBWidth";
            this.numericUpDownBBWidth.Size = new System.Drawing.Size(52, 20);
            this.numericUpDownBBWidth.TabIndex = 5;
            this.numericUpDownBBWidth.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(210, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Padding:";
            // 
            // numericUpDownPadding
            // 
            this.numericUpDownPadding.Location = new System.Drawing.Point(213, 20);
            this.numericUpDownPadding.Name = "numericUpDownPadding";
            this.numericUpDownPadding.Size = new System.Drawing.Size(43, 20);
            this.numericUpDownPadding.TabIndex = 7;
            this.numericUpDownPadding.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // linkLabelFont
            // 
            this.linkLabelFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelFont.Location = new System.Drawing.Point(213, 48);
            this.linkLabelFont.Name = "linkLabelFont";
            this.linkLabelFont.Size = new System.Drawing.Size(370, 37);
            this.linkLabelFont.TabIndex = 10;
            this.linkLabelFont.TabStop = true;
            this.linkLabelFont.Text = "Font";
            this.linkLabelFont.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelFont_LinkClicked);
            // 
            // buttonRender
            // 
            this.buttonRender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRender.Location = new System.Drawing.Point(508, 4);
            this.buttonRender.Name = "buttonRender";
            this.buttonRender.Size = new System.Drawing.Size(75, 23);
            this.buttonRender.TabIndex = 11;
            this.buttonRender.Text = "Render";
            this.buttonRender.UseVisualStyleBackColor = true;
            this.buttonRender.Click += new System.EventHandler(this.buttonRender_Click);
            // 
            // YxTableRendererSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.buttonRender);
            this.Controls.Add(this.linkLabelFont);
            this.Controls.Add(this.numericUpDownPadding);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Name = "YxTableRendererSettingsControl";
            this.Size = new System.Drawing.Size(586, 92);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBBHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBBWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPadding)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonNoScale;
        private System.Windows.Forms.RadioButton radioButtonScale;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownBBHeight;
        private System.Windows.Forms.NumericUpDown numericUpDownBBWidth;
        private System.Windows.Forms.NumericUpDown numericUpDownPadding;
        private System.Windows.Forms.LinkLabel linkLabelFont;
        private System.Windows.Forms.Button buttonRender;
    }
}

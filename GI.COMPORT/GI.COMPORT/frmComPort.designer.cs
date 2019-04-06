namespace GI.COMPORT
{
    partial class frmComPort
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
            this.button1 = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.lblPortName = new System.Windows.Forms.Label();
            this.lblCon = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 47);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start Reading";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbLog
            // 
            this.tbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLog.BackColor = System.Drawing.SystemColors.WindowText;
            this.tbLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLog.ForeColor = System.Drawing.Color.White;
            this.tbLog.Location = new System.Drawing.Point(12, 65);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(406, 196);
            this.tbLog.TabIndex = 2;
            this.tbLog.WordWrap = false;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(310, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(106, 47);
            this.button2.TabIndex = 1;
            this.button2.Text = "Stop Reading";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblPortName
            // 
            this.lblPortName.AutoSize = true;
            this.lblPortName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPortName.Location = new System.Drawing.Point(135, 16);
            this.lblPortName.Name = "lblPortName";
            this.lblPortName.Size = new System.Drawing.Size(51, 16);
            this.lblPortName.TabIndex = 3;
            this.lblPortName.Text = "label1";
            // 
            // lblCon
            // 
            this.lblCon.AutoSize = true;
            this.lblCon.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCon.Location = new System.Drawing.Point(135, 39);
            this.lblCon.Name = "lblCon";
            this.lblCon.Size = new System.Drawing.Size(51, 16);
            this.lblCon.TabIndex = 4;
            this.lblCon.Text = "label1";
            // 
            // frmComPort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 273);
            this.Controls.Add(this.lblCon);
            this.Controls.Add(this.lblPortName);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmComPort";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "COM Port";
            this.Load += new System.EventHandler(this.ComPort_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        protected System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lblPortName;
        private System.Windows.Forms.Label lblCon;
    }
}
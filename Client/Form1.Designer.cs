namespace Client
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
            this.ConnectButton = new System.Windows.Forms.Button();
            this.ConnectStatus = new System.Windows.Forms.TextBox();
            this.GetXmlButton = new System.Windows.Forms.Button();
            this.XmlText = new System.Windows.Forms.TextBox();
            this.ConnectionChecker = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(549, 9);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectButton.TabIndex = 0;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // ConnectStatus
            // 
            this.ConnectStatus.Location = new System.Drawing.Point(497, 38);
            this.ConnectStatus.Name = "ConnectStatus";
            this.ConnectStatus.Size = new System.Drawing.Size(195, 20);
            this.ConnectStatus.TabIndex = 1;
            this.ConnectStatus.Text = "Not Connected";
            // 
            // GetXmlButton
            // 
            this.GetXmlButton.Location = new System.Drawing.Point(295, 12);
            this.GetXmlButton.Name = "GetXmlButton";
            this.GetXmlButton.Size = new System.Drawing.Size(75, 23);
            this.GetXmlButton.TabIndex = 2;
            this.GetXmlButton.Text = "Get Songs";
            this.GetXmlButton.UseVisualStyleBackColor = true;
            this.GetXmlButton.Click += new System.EventHandler(this.GetXmlButton_Click);
            // 
            // XmlText
            // 
            this.XmlText.Location = new System.Drawing.Point(13, 12);
            this.XmlText.Multiline = true;
            this.XmlText.Name = "XmlText";
            this.XmlText.Size = new System.Drawing.Size(276, 417);
            this.XmlText.TabIndex = 3;
            // 
            // ConnectionChecker
            // 
            this.ConnectionChecker.Enabled = true;
            this.ConnectionChecker.Interval = 500;
            this.ConnectionChecker.Tick += new System.EventHandler(this.ConnectionChecker_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 441);
            this.Controls.Add(this.XmlText);
            this.Controls.Add(this.GetXmlButton);
            this.Controls.Add(this.ConnectStatus);
            this.Controls.Add(this.ConnectButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.TextBox ConnectStatus;
        private System.Windows.Forms.Button GetXmlButton;
        private System.Windows.Forms.TextBox XmlText;
        private System.Windows.Forms.Timer ConnectionChecker;
    }
}


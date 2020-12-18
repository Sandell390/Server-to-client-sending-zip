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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ConnectButton = new System.Windows.Forms.Button();
            this.ConnectStatus = new System.Windows.Forms.TextBox();
            this.DownloadFiles = new System.Windows.Forms.Button();
            this.ConnectionChecker = new System.Windows.Forms.Timer(this.components);
            this.fileBox = new System.Windows.Forms.ListBox();
            this.GetFiles = new System.Windows.Forms.Button();
            this.ipServerText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ConnectButton
            // 
            this.ConnectButton.Enabled = false;
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
            this.ConnectStatus.Location = new System.Drawing.Point(497, 64);
            this.ConnectStatus.Name = "ConnectStatus";
            this.ConnectStatus.Size = new System.Drawing.Size(195, 20);
            this.ConnectStatus.TabIndex = 1;
            this.ConnectStatus.Text = "Not Connected";
            // 
            // DownloadFiles
            // 
            this.DownloadFiles.Enabled = false;
            this.DownloadFiles.Location = new System.Drawing.Point(295, 12);
            this.DownloadFiles.Name = "DownloadFiles";
            this.DownloadFiles.Size = new System.Drawing.Size(101, 23);
            this.DownloadFiles.TabIndex = 2;
            this.DownloadFiles.Text = "Download Files";
            this.DownloadFiles.UseVisualStyleBackColor = true;
            this.DownloadFiles.Click += new System.EventHandler(this.DownloadFiles_Click);
            // 
            // ConnectionChecker
            // 
            this.ConnectionChecker.Enabled = true;
            this.ConnectionChecker.Interval = 500;
            // 
            // fileBox
            // 
            this.fileBox.FormattingEnabled = true;
            this.fileBox.Location = new System.Drawing.Point(12, 12);
            this.fileBox.Name = "fileBox";
            this.fileBox.Size = new System.Drawing.Size(277, 420);
            this.fileBox.TabIndex = 4;
            // 
            // GetFiles
            // 
            this.GetFiles.Enabled = false;
            this.GetFiles.Location = new System.Drawing.Point(296, 408);
            this.GetFiles.Name = "GetFiles";
            this.GetFiles.Size = new System.Drawing.Size(75, 23);
            this.GetFiles.TabIndex = 5;
            this.GetFiles.Text = "Refresh";
            this.GetFiles.UseVisualStyleBackColor = true;
            this.GetFiles.Click += new System.EventHandler(this.GetFiles_Click);
            // 
            // ipServerText
            // 
            this.ipServerText.Location = new System.Drawing.Point(497, 38);
            this.ipServerText.Name = "ipServerText";
            this.ipServerText.Size = new System.Drawing.Size(195, 20);
            this.ipServerText.TabIndex = 6;
            this.ipServerText.Text = "Enter server ip";
            this.ipServerText.TextChanged += new System.EventHandler(this.ipServerText_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 441);
            this.Controls.Add(this.ipServerText);
            this.Controls.Add(this.GetFiles);
            this.Controls.Add(this.fileBox);
            this.Controls.Add(this.DownloadFiles);
            this.Controls.Add(this.ConnectStatus);
            this.Controls.Add(this.ConnectButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "The Super Sender Inator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.TextBox ConnectStatus;
        private System.Windows.Forms.Button DownloadFiles;
        private System.Windows.Forms.Timer ConnectionChecker;
        private System.Windows.Forms.ListBox fileBox;
        private System.Windows.Forms.Button GetFiles;
        private System.Windows.Forms.TextBox ipServerText;
    }
}


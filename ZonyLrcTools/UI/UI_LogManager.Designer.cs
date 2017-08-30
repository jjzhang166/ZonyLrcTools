namespace ZonyLrcTools.UI
{
    partial class UI_LogManager
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
            this.label_FileViewer = new System.Windows.Forms.Label();
            this.listBox_FileViewer = new System.Windows.Forms.ListBox();
            this.listBox_LogItem = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label_FileViewer
            // 
            this.label_FileViewer.AutoSize = true;
            this.label_FileViewer.Location = new System.Drawing.Point(12, 10);
            this.label_FileViewer.Name = "label_FileViewer";
            this.label_FileViewer.Size = new System.Drawing.Size(58, 13);
            this.label_FileViewer.TabIndex = 0;
            this.label_FileViewer.Text = "日志文件:";
            // 
            // listBox_FileViewer
            // 
            this.listBox_FileViewer.FormattingEnabled = true;
            this.listBox_FileViewer.Location = new System.Drawing.Point(12, 26);
            this.listBox_FileViewer.Name = "listBox_FileViewer";
            this.listBox_FileViewer.Size = new System.Drawing.Size(214, 420);
            this.listBox_FileViewer.TabIndex = 1;
            this.listBox_FileViewer.SelectedIndexChanged += new System.EventHandler(this.listBox_FileViewer_SelectedIndexChanged);
            this.listBox_FileViewer.DoubleClick += new System.EventHandler(this.listBox_FileViewer_DoubleClick);
            // 
            // listBox_LogItem
            // 
            this.listBox_LogItem.FormattingEnabled = true;
            this.listBox_LogItem.Location = new System.Drawing.Point(232, 26);
            this.listBox_LogItem.Name = "listBox_LogItem";
            this.listBox_LogItem.Size = new System.Drawing.Size(397, 420);
            this.listBox_LogItem.TabIndex = 2;
            this.listBox_LogItem.DoubleClick += new System.EventHandler(this.listBox_LogItem_DoubleClick);
            // 
            // UI_LogManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 459);
            this.Controls.Add(this.listBox_LogItem);
            this.Controls.Add(this.listBox_FileViewer);
            this.Controls.Add(this.label_FileViewer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UI_LogManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "日志管理器";
            this.Load += new System.EventHandler(this.UI_LogManager_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_FileViewer;
        private System.Windows.Forms.ListBox listBox_FileViewer;
        private System.Windows.Forms.ListBox listBox_LogItem;
    }
}
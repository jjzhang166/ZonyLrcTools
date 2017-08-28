namespace LibSingleLyricSearch
{
    partial class UI_SearchWindow
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_SongName = new System.Windows.Forms.TextBox();
            this.textBox_Artist = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_Search = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_DownLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.button_PreviousPage = new System.Windows.Forms.Button();
            this.button_NextPage = new System.Windows.Forms.Button();
            this.listView_LyricList = new LibPlug.UI.ListViewNF();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "搜索结果:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "歌曲名:";
            // 
            // textBox_SongName
            // 
            this.textBox_SongName.Location = new System.Drawing.Point(59, 7);
            this.textBox_SongName.Name = "textBox_SongName";
            this.textBox_SongName.Size = new System.Drawing.Size(98, 20);
            this.textBox_SongName.TabIndex = 3;
            // 
            // textBox_Artist
            // 
            this.textBox_Artist.Location = new System.Drawing.Point(210, 7);
            this.textBox_Artist.Name = "textBox_Artist";
            this.textBox_Artist.Size = new System.Drawing.Size(98, 20);
            this.textBox_Artist.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(175, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "歌手:";
            // 
            // button_Search
            // 
            this.button_Search.Location = new System.Drawing.Point(314, 5);
            this.button_Search.Name = "button_Search";
            this.button_Search.Size = new System.Drawing.Size(75, 25);
            this.button_Search.TabIndex = 6;
            this.button_Search.Text = "搜索";
            this.button_Search.UseVisualStyleBackColor = true;
            this.button_Search.Click += new System.EventHandler(this.button_Search_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_DownLoad});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(99, 26);
            // 
            // ToolStripMenuItem_DownLoad
            // 
            this.ToolStripMenuItem_DownLoad.Name = "ToolStripMenuItem_DownLoad";
            this.ToolStripMenuItem_DownLoad.Size = new System.Drawing.Size(98, 22);
            this.ToolStripMenuItem_DownLoad.Text = "下载";
            this.ToolStripMenuItem_DownLoad.Click += new System.EventHandler(this.ToolStripMenuItem_DownLoad_Click);
            // 
            // button_PreviousPage
            // 
            this.button_PreviousPage.Location = new System.Drawing.Point(99, 243);
            this.button_PreviousPage.Name = "button_PreviousPage";
            this.button_PreviousPage.Size = new System.Drawing.Size(75, 23);
            this.button_PreviousPage.TabIndex = 7;
            this.button_PreviousPage.Text = "上一页";
            this.button_PreviousPage.UseVisualStyleBackColor = true;
            this.button_PreviousPage.Click += new System.EventHandler(this.button_PreviousPage_Click);
            // 
            // button_NextPage
            // 
            this.button_NextPage.Location = new System.Drawing.Point(283, 243);
            this.button_NextPage.Name = "button_NextPage";
            this.button_NextPage.Size = new System.Drawing.Size(75, 23);
            this.button_NextPage.TabIndex = 7;
            this.button_NextPage.Text = "下一页";
            this.button_NextPage.UseVisualStyleBackColor = true;
            this.button_NextPage.Click += new System.EventHandler(this.button_NextPage_Click);
            // 
            // listView_LyricList
            // 
            this.listView_LyricList.AllowDrop = true;
            this.listView_LyricList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView_LyricList.ContextMenuStrip = this.contextMenuStrip1;
            this.listView_LyricList.Location = new System.Drawing.Point(12, 49);
            this.listView_LyricList.Name = "listView_LyricList";
            this.listView_LyricList.Size = new System.Drawing.Size(427, 188);
            this.listView_LyricList.TabIndex = 0;
            this.listView_LyricList.UseCompatibleStateImageBehavior = false;
            this.listView_LyricList.View = System.Windows.Forms.View.Details;
            this.listView_LyricList.ItemActivate += new System.EventHandler(this.listView_LyricList_ItemActivate);
            this.listView_LyricList.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView_LyricList_DragEnter);
            this.listView_LyricList.DragOver += new System.Windows.Forms.DragEventHandler(this.listView_LyricList_DragOver);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "歌曲名";
            this.columnHeader1.Width = 194;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "歌手";
            this.columnHeader2.Width = 223;
            // 
            // UI_SearchWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 276);
            this.Controls.Add(this.button_NextPage);
            this.Controls.Add(this.button_PreviousPage);
            this.Controls.Add(this.button_Search);
            this.Controls.Add(this.textBox_Artist);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_SongName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView_LyricList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "UI_SearchWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "歌词搜索";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LibPlug.UI.ListViewNF listView_LyricList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_SongName;
        private System.Windows.Forms.TextBox textBox_Artist;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_Search;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_DownLoad;
        private System.Windows.Forms.Button button_PreviousPage;
        private System.Windows.Forms.Button button_NextPage;
    }
}
namespace diplom
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.редагуванняToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.балкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.зосередженаСилаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.моментToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.виглядToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.показатиСховатиДеформаціюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.властивостіМатеріалуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.силаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.додатиСилуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.прибратиСилуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.моментToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.додатиМоментToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.прибратиМоментToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.типКріпленняToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.балкаМіжСтінамиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.балкаМіжШарнірамиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(0, 24);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(1172, 579);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.редагуванняToolStripMenuItem,
            this.виглядToolStripMenuItem,
            this.типКріпленняToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1172, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// редагуванняToolStripMenuItem
			// 
			this.редагуванняToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.балкаToolStripMenuItem,
            this.зосередженаСилаToolStripMenuItem,
            this.моментToolStripMenuItem});
			this.редагуванняToolStripMenuItem.Name = "редагуванняToolStripMenuItem";
			this.редагуванняToolStripMenuItem.Size = new System.Drawing.Size(87, 20);
			this.редагуванняToolStripMenuItem.Text = "Редагування";
			// 
			// балкаToolStripMenuItem
			// 
			this.балкаToolStripMenuItem.Name = "балкаToolStripMenuItem";
			this.балкаToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.балкаToolStripMenuItem.Text = "Балка";
			this.балкаToolStripMenuItem.Click += new System.EventHandler(this.балкаToolStripMenuItem_Click);
			// 
			// зосередженаСилаToolStripMenuItem
			// 
			this.зосередженаСилаToolStripMenuItem.Name = "зосередженаСилаToolStripMenuItem";
			this.зосередженаСилаToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.зосередженаСилаToolStripMenuItem.Text = "Зосереджена сила";
			this.зосередженаСилаToolStripMenuItem.Click += new System.EventHandler(this.зосередженаСилаToolStripMenuItem_Click);
			// 
			// моментToolStripMenuItem
			// 
			this.моментToolStripMenuItem.Name = "моментToolStripMenuItem";
			this.моментToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.моментToolStripMenuItem.Text = "Момент";
			this.моментToolStripMenuItem.Click += new System.EventHandler(this.моментToolStripMenuItem_Click);
			// 
			// виглядToolStripMenuItem
			// 
			this.виглядToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.показатиСховатиДеформаціюToolStripMenuItem,
            this.властивостіМатеріалуToolStripMenuItem,
            this.силаToolStripMenuItem,
            this.моментToolStripMenuItem1});
			this.виглядToolStripMenuItem.Name = "виглядToolStripMenuItem";
			this.виглядToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
			this.виглядToolStripMenuItem.Text = "Вигляд";
			// 
			// показатиСховатиДеформаціюToolStripMenuItem
			// 
			this.показатиСховатиДеформаціюToolStripMenuItem.Name = "показатиСховатиДеформаціюToolStripMenuItem";
			this.показатиСховатиДеформаціюToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.показатиСховатиДеформаціюToolStripMenuItem.Text = "Показати деформацію";
			this.показатиСховатиДеформаціюToolStripMenuItem.Click += new System.EventHandler(this.показатиСховатиДеформаціюToolStripMenuItem_Click);
			// 
			// властивостіМатеріалуToolStripMenuItem
			// 
			this.властивостіМатеріалуToolStripMenuItem.Name = "властивостіМатеріалуToolStripMenuItem";
			this.властивостіМатеріалуToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.властивостіМатеріалуToolStripMenuItem.Text = "Властивості матеріалу";
			this.властивостіМатеріалуToolStripMenuItem.Click += new System.EventHandler(this.властивостіМатеріалуToolStripMenuItem_Click);
			// 
			// силаToolStripMenuItem
			// 
			this.силаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.додатиСилуToolStripMenuItem,
            this.прибратиСилуToolStripMenuItem});
			this.силаToolStripMenuItem.Name = "силаToolStripMenuItem";
			this.силаToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.силаToolStripMenuItem.Text = "Сила";
			// 
			// додатиСилуToolStripMenuItem
			// 
			this.додатиСилуToolStripMenuItem.Name = "додатиСилуToolStripMenuItem";
			this.додатиСилуToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
			this.додатиСилуToolStripMenuItem.Text = "Додати силу";
			this.додатиСилуToolStripMenuItem.Click += new System.EventHandler(this.додатиСилуToolStripMenuItem_Click);
			// 
			// прибратиСилуToolStripMenuItem
			// 
			this.прибратиСилуToolStripMenuItem.Name = "прибратиСилуToolStripMenuItem";
			this.прибратиСилуToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
			this.прибратиСилуToolStripMenuItem.Text = "Прибрати силу";
			this.прибратиСилуToolStripMenuItem.Click += new System.EventHandler(this.прибратиСилуToolStripMenuItem_Click);
			// 
			// моментToolStripMenuItem1
			// 
			this.моментToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.додатиМоментToolStripMenuItem,
            this.прибратиМоментToolStripMenuItem});
			this.моментToolStripMenuItem1.Name = "моментToolStripMenuItem1";
			this.моментToolStripMenuItem1.Size = new System.Drawing.Size(198, 22);
			this.моментToolStripMenuItem1.Text = "Момент";
			// 
			// додатиМоментToolStripMenuItem
			// 
			this.додатиМоментToolStripMenuItem.Name = "додатиМоментToolStripMenuItem";
			this.додатиМоментToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.додатиМоментToolStripMenuItem.Text = "Додати момент";
			this.додатиМоментToolStripMenuItem.Click += new System.EventHandler(this.додатиМоментToolStripMenuItem_Click);
			// 
			// прибратиМоментToolStripMenuItem
			// 
			this.прибратиМоментToolStripMenuItem.Name = "прибратиМоментToolStripMenuItem";
			this.прибратиМоментToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.прибратиМоментToolStripMenuItem.Text = "Прибрати момент";
			this.прибратиМоментToolStripMenuItem.Click += new System.EventHandler(this.прибратиМоментToolStripMenuItem_Click);
			// 
			// типКріпленняToolStripMenuItem
			// 
			this.типКріпленняToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.балкаМіжСтінамиToolStripMenuItem,
            this.балкаМіжШарнірамиToolStripMenuItem});
			this.типКріпленняToolStripMenuItem.Name = "типКріпленняToolStripMenuItem";
			this.типКріпленняToolStripMenuItem.Size = new System.Drawing.Size(98, 20);
			this.типКріпленняToolStripMenuItem.Text = "Тип кріплення";
			// 
			// балкаМіжСтінамиToolStripMenuItem
			// 
			this.балкаМіжСтінамиToolStripMenuItem.Name = "балкаМіжСтінамиToolStripMenuItem";
			this.балкаМіжСтінамиToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
			this.балкаМіжСтінамиToolStripMenuItem.Text = "Балка між стінами";
			this.балкаМіжСтінамиToolStripMenuItem.Click += new System.EventHandler(this.балкаМіжСтінамиToolStripMenuItem_Click);
			// 
			// балкаМіжШарнірамиToolStripMenuItem
			// 
			this.балкаМіжШарнірамиToolStripMenuItem.Name = "балкаМіжШарнірамиToolStripMenuItem";
			this.балкаМіжШарнірамиToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
			this.балкаМіжШарнірамиToolStripMenuItem.Text = "Балка між шарнірами";
			this.балкаМіжШарнірамиToolStripMenuItem.Click += new System.EventHandler(this.балкаМіжШарнірамиToolStripMenuItem_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(1172, 603);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "Form1";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem редагуванняToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem балкаToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem зосередженаСилаToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem виглядToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem показатиСховатиДеформаціюToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem властивостіМатеріалуToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem типКріпленняToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem балкаМіжСтінамиToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem балкаМіжШарнірамиToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem моментToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem силаToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem додатиСилуToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem прибратиСилуToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem моментToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem додатиМоментToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem прибратиМоментToolStripMenuItem;
	}
}


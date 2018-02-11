namespace ArrivalPrediction.UI
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
			this.labelInfo = new System.Windows.Forms.Label();
			this.buttonPlay = new System.Windows.Forms.Button();
			this.buttonStopPolling = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(12, 71);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(806, 276);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// labelInfo
			// 
			this.labelInfo.AutoSize = true;
			this.labelInfo.Location = new System.Drawing.Point(371, 25);
			this.labelInfo.Name = "labelInfo";
			this.labelInfo.Size = new System.Drawing.Size(208, 25);
			this.labelInfo.TabIndex = 1;
			this.labelInfo.Text = "Polling in progress...";
			// 
			// buttonPlay
			// 
			this.buttonPlay.Enabled = false;
			this.buttonPlay.Location = new System.Drawing.Point(257, 9);
			this.buttonPlay.Name = "buttonPlay";
			this.buttonPlay.Size = new System.Drawing.Size(108, 56);
			this.buttonPlay.TabIndex = 2;
			this.buttonPlay.Text = "Play";
			this.buttonPlay.UseVisualStyleBackColor = true;
			this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
			// 
			// buttonStopPolling
			// 
			this.buttonStopPolling.Location = new System.Drawing.Point(12, 9);
			this.buttonStopPolling.Name = "buttonStopPolling";
			this.buttonStopPolling.Size = new System.Drawing.Size(239, 56);
			this.buttonStopPolling.TabIndex = 3;
			this.buttonStopPolling.Text = "Stop polling";
			this.buttonStopPolling.UseVisualStyleBackColor = true;
			this.buttonStopPolling.Click += new System.EventHandler(this.buttonStopPolling_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(2390, 1259);
			this.Controls.Add(this.buttonStopPolling);
			this.Controls.Add(this.buttonPlay);
			this.Controls.Add(this.labelInfo);
			this.Controls.Add(this.pictureBox1);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label labelInfo;
		private System.Windows.Forms.Button buttonPlay;
		private System.Windows.Forms.Button buttonStopPolling;
	}
}


namespace mario
{
	partial class SpriteForm
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose( bool disposing )
		{
			if (disposing && ( components != null ))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.animationTimer = new System.Windows.Forms.Timer(this.components);
			this.eventTimer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// animationTimer
			// 
			this.animationTimer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// eventTimer
			// 
			this.eventTimer.Interval = 5000;
			this.eventTimer.Tick += new System.EventHandler(this.eventTimer_Tick);
			// 
			// MarioSprite
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(109, 99);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MarioSprite";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer animationTimer;
		private System.Windows.Forms.Timer eventTimer;
	}
}


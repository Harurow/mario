using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace mario
{
	public partial class MainForm : Form
	{
		List<SpriteForm> sprites = new List<SpriteForm>();

		public MainForm()
		{
			InitializeComponent();
		}

		protected override void OnLoad( EventArgs e )
		{
			base.OnLoad(e);

			Add();

			Visible = false;
		}

		protected override void OnMouseClick( MouseEventArgs e )
		{
			base.OnMouseClick(e);

			if (e.Button == MouseButtons.Left)
			{
				Add();
			}
			else if (e.Button == MouseButtons.Right)
			{
				Kill();
			}
		}

		public void Add()
		{
			var sprite = new SpriteForm();
			sprite.Closed += SpriteOnClosed;
			sprites.Add(sprite);
			sprite.Show();
		}

		public void Kill()
		{
			foreach (var sprite in sprites)
			{
				sprite.Kill();
			}
		}

		private void SpriteOnClosed(object sender, EventArgs eventArgs)
		{
			sprites.Remove((SpriteForm) sender);
		}
	}
}

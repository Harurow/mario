using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace mario
{
	public partial class MainForm : Form
	{
		private readonly List<SpriteForm> _sprites = new List<SpriteForm>();

		public MainForm()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			for (int i = 0; i < Program.MarioCount; i++)
			{
				Add();
			}
		}

		protected override void OnMouseClick(MouseEventArgs e)
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
			sprite.FormClosed += SpriteOnClosed;
			_sprites.Add(sprite);
			sprite.Show(this);
		}

		public void Kill()
		{
			foreach (var sprite in _sprites)
			{
				sprite.Kill();
			}
		}

		private void SpriteOnClosed(object sender, EventArgs eventArgs)
		{
			var sprite = (SpriteForm)sender;
			_sprites.Remove(sprite);
			sprite.FormClosed -= SpriteOnClosed;

			if (_sprites.Count == 0)
			{
				Close();
			}
		}

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			base.OnClosing(e);

			if (_sprites.Count != 0)
			{
				Kill();
				e.Cancel = true;
			}
			else
			{
				e.Cancel = false;
			}
		}
	}
}

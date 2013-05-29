using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace mario
{
	public sealed partial class SpriteForm : Form
	{
		private Random _rand = new Random();
		private Mario _mario = new Mario();

		public SpriteForm()
		{
			InitializeComponent();

			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.StandardDoubleClick, false);
			TopMost = true;

			BackColor = Color.Black;
			TransparencyKey = Color.Black;
		}

		protected override void OnLoad( EventArgs e )
		{
			base.OnLoad(e);

			Size = _mario.Size;

			Rectangle desk = Screen.GetWorkingArea(this);
			Location = new Point(0, desk.Height - Size.Height);

			int interval = 30;
			Random rand = new Random();
			int num = rand.Next(100);
			if (num < 10)
			{
				interval = 33;
			}
			else if (num < 25)
			{
				interval = 35;
			}
			else if (num == 99)
			{
				interval = 40;
			}

			animationTimer.Interval = interval;
			animationTimer.Enabled = true;
			eventTimer.Enabled = true;

			SystemEvents.SessionEnding += SystemEventsOnSessionEnding;
		}

		private void SystemEventsOnSessionEnding(object sender, SessionEndingEventArgs e)
		{
			SystemEvents.SessionEnding -= SystemEventsOnSessionEnding;
			_mario.Kill();
		}

		protected override void OnPaint( PaintEventArgs e )
		{
			base.OnPaint(e);
			_mario.Draw(e.Graphics, ClientRectangle);
		}

		private void timer_Tick( object sender, EventArgs e )
		{
			int xMove, yMove;
			_mario.Animate(out xMove, out yMove);

			Location = new Point(Location.X + xMove, Location.Y - yMove);

			Invalidate();

			Rectangle desk = Screen.GetWorkingArea(this);
			if (xMove > 0 && desk.Right < Location.X)
			{
				Location = new Point(-_mario.Size.Width, Location.Y);
			}
			else if (xMove < 0 && Location.X < desk.Left - _mario.Size.Width)
			{
				Location = new Point(desk.Right, Location.Y);
			}
			if (yMove != 0 && desk.Bottom < Location.Y)
			{
				Close();
			}
		}

		protected override void OnMouseClick( MouseEventArgs e )
		{
			base.OnMouseClick(e);

			if (_mario.IsStop)
			{
				_mario.Kill();
			}
		}

		private void MarioAction()
		{
			int rand = _rand.Next(100);
			if (_mario.IsRunning)
			{
				if (rand < 2)
				{
					_mario.Kill();
				}
				else if (rand < 25)
				{
					_mario.Turn();
				}
				else
				{
					_mario.Jump();
				}
			}
		}

		protected override void OnMouseEnter( EventArgs e )
		{
			base.OnMouseEnter(e);

			if (ModifierKeys == Keys.Control)
			{
				_mario.Turn();
			}
			else
			{
				MarioAction();
			}
		}

		private void eventTimer_Tick( object sender, EventArgs e )
		{
			if (_rand.Next(100) < 30)
			{
				MarioAction();
			}
		}

		public void Kill()
		{
			_mario.Kill();
		}
	}
}

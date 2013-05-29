using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace mario
{
	public sealed partial class SpriteForm : Form
	{
		private Mario _mario = new Mario();
		private int _zoom = 10;

		public SpriteForm()
		{
			InitializeComponent();

			DoubleBuffered = true;

			_zoom = Program.Zoom;
		}

		protected override void OnLoad( EventArgs e )
		{
			base.OnLoad(e);

			Size = new Size(_mario.Size.Width * _zoom,
			                _mario.Size.Height * _zoom);

			Rectangle desk = Screen.GetWorkingArea(this);
			Location = new Point(0, desk.Height - Size.Height);

			int interval = 35;
			int num = Program.Rand.Next(100);
			if (num < 50)
			{
				interval += num/4;
			}

			animationTimer.Interval = interval;
			animationTimer.Enabled = true;
			eventTimer.Interval = 2500;
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

		private void EventTimerTick( object sender, EventArgs e )
		{
			if (Program.Rand.Next(100) < 25)
			{
				MarioAction();
			}
		}

		private void AnimationTimerTick( object sender, EventArgs e )
		{
			int xMove, yMove;
			_mario.Animate(out xMove, out yMove);

			var point = new Point(Location.X + xMove * _zoom, Location.Y - yMove * _zoom);

			Rectangle desk = Screen.GetWorkingArea(this);
			if (xMove > 0 && desk.Right < point.X)
			{
				point = new Point(-Size.Width, point.Y);
			}
			else if (xMove < 0 && point.X < desk.Left - Size.Width)
			{
				point = new Point(desk.Right, point.Y);
			}

			Invalidate();
			Location = point;

			if (yMove != 0 && desk.Bottom < point.Y)
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
			int rand = Program.Rand.Next(100);
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

			if ((ModifierKeys & Keys.Control) == Keys.Control)
			{
				_mario.Turn();
			}
			else
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

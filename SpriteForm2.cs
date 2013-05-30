using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace mario
{
	public sealed partial class SpriteForm2 : Form
	{
		private List<Mario> _marios = new List<Mario>();
		private int _zoom = 1;

		public SpriteForm2()
		{
			InitializeComponent();

			DoubleBuffered = true;

			_zoom = Program.Zoom;
		}

		protected override void OnLoad( EventArgs e )
		{
			base.OnLoad(e);

			Rectangle desk = Screen.GetWorkingArea(Point.Empty);

			Size = new Size(desk.Size.Width, Math.Min(desk.Size.Height, 16 * _zoom * 8));
			Location = new Point(desk.Location.X, desk.Bottom - Size.Height);

			for (int i = 0; i < Program.MarioCount; i++)
			{
				var m = new Mario {XHint = 0};
				m.YHint = Size.Height - m.Size.Height*_zoom;
				_marios.Add(m);
			}

			animationTimer.Interval = 35;
			eventTimer.Interval = 2500;
			animationTimer.Enabled = true;
			eventTimer.Enabled = true;

			SystemEvents.SessionEnding += SystemEventsOnSessionEnding;
		}

		private void SystemEventsOnSessionEnding(object sender, SessionEndingEventArgs e)
		{
			SystemEvents.SessionEnding -= SystemEventsOnSessionEnding;
			foreach (var mario in _marios)
			{
				mario.Kill();
			}
		}

		private Rectangle CalcMarioArea(Mario m)
		{
			return new Rectangle(m.XHint, m.YHint, m.Size.Width * _zoom, m.Size.Height * _zoom);
		}

		protected override void OnPaint( PaintEventArgs e )
		{
			base.OnPaint(e);

			foreach (var mario in _marios)
			{
				mario.Draw(e.Graphics, CalcMarioArea(mario));
			}
		}

		private void EventTimerTick( object sender, EventArgs e )
		{
			foreach (var mario in _marios)
			{
				if (Program.Rand.Next(100) < 25)
				{
					MarioAction(mario);
				}
			}
		}

		private void AnimationTimerTick( object sender, EventArgs e )
		{
			for (int i = 0; i < _marios.Count; i++)
			{
				var m = _marios[i];

				Invalidate(CalcMarioArea(m));

				int xMove, yMove;
				m.Animate(out xMove, out yMove);

				var point = new Point(m.XHint + xMove * _zoom, m.YHint - yMove * _zoom);

				Rectangle bounds = ClientRectangle;
				if (xMove > 0 && bounds.Right < point.X)
				{
					point = new Point(-m.Size.Width * _zoom, point.Y);
				}
				else if (xMove < 0 && point.X < bounds.Left - m.Size.Width * _zoom)
				{
					point = new Point(bounds.Right, point.Y);
				}
				m.XHint = point.X;
				m.YHint = point.Y;
				if (yMove != 0 && bounds.Bottom < point.Y)
				{
					_marios.RemoveAt(i);
					i--;
				}

				Invalidate(CalcMarioArea(m));
			}

			if (_marios.Count == 0)
			{
				Close();
				return;
			}

			Point pt = PointToClient(MousePosition);
			for (int i = 0; i < _marios.Count; i++)
			{
				var m = _marios[i];
				var r = CalcMarioArea(m);
				if (r.Contains(pt))
				{
					if ((ModifierKeys & Keys.Control) == Keys.Control)
					{
						m.Turn();
					}
					else
					{
						MarioAction(m);
					}
				}
			}
		}

		protected override void OnMouseClick( MouseEventArgs e )
		{
			base.OnMouseClick(e);

			for (int i = _marios.Count - 1; i >= 0; i--)
			{
				var m = _marios[i];
				var r = CalcMarioArea(m);
				if (m.IsStop && r.Contains(e.Location))
				{
					m.Kill();
				}
			}
		}

		private void MarioAction(Mario m)
		{
			int rand = Program.Rand.Next(100);
			if (m.IsRunning)
			{
				if (rand < 2)
				{
					m.Kill();
				}
				else if (rand < 25)
				{
					m.Turn();
				}
				else
				{
					m.Jump();
				}
			}
		}
	}
}

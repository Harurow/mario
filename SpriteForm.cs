using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace mario
{
	public sealed partial class SpriteForm : Form
	{
		private readonly Mario _mario = new Mario();
		private readonly int _zoom = 1;

		public SpriteForm()
		{
			InitializeComponent();

			DoubleBuffered = true;
			SetStyle(ControlStyles.StandardClick, true);
			SetStyle(ControlStyles.Selectable, false);

			_zoom = Program.Zoom;
		}

		protected override void OnLoad( EventArgs e )
		{
			base.OnLoad(e);

			Size = new Size(_mario.Size.Width * _zoom,
			                _mario.Size.Height * _zoom);

			Rectangle desk = Screen.GetWorkingArea(this);
			Location = new Point(0, desk.Height - Size.Height);

			animationTimer.Interval = 35;
			eventTimer.Interval = 2500;
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
			bool invalidate = _mario.Animate(out xMove, out yMove);

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

			if (invalidate)
			{
				Invalidate();
			}

			Location = point;

			if (yMove != 0 && desk.Bottom < point.Y)
			{
				Close();
			}
		}

		protected override void OnMouseClick( MouseEventArgs e )
		{
			base.OnMouseClick(e);

			if (_mario.IsStop && e.Button == MouseButtons.Left)
			{
				if (ModifierKeys == (Keys.Alt | Keys.Control | Keys.Shift))
				{
					foreach (Form f in Application.OpenForms)
					{
						var mf = f as MainForm;
						if (mf != null)
						{
							mf.Kill();
							break;
						}
					}
				}
				else
				{
					_mario.Kill();
				}
			}
			else if (_mario.IsStop && e.Button == MouseButtons.Right)
			{
				_mario.AddPauseCount(120);
				string talk = _mario.IsMario ? "It's me! Mario!" : "It's me! Luigi!";
				string text = string.Format("{0}\nMove:{1:#,##0}pixels\nJump:{2:#,##0}\nTurn:{3:#,##0}",
								talk, _mario.MovePixels, _mario.JumpCount, _mario.TurnCount);
				var tt = new ToolTip {IsBalloon = true};
				tt.Show(text, this);
			}
		}

		private void MarioAction()
		{
			int rand = Program.Rand.Next(100);
			if (_mario.IsRunning)
			{
				if (rand < 2 && Program.AutoDeath)
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

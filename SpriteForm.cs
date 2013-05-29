using System;
using System.Drawing;
using System.Windows.Forms;

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

			animationTimer.Interval = 30;
			animationTimer.Enabled = true;
			eventTimer.Enabled = true;
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
				_mario.Dead();
			}
		}

		private void MarioAction()
		{
			int rand = _rand.Next(100);
			if (_mario.IsRunning)
			{
				if (rand < 2)
				{
					_mario.Dead();
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
	}
}

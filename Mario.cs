using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace mario
{
	public class Mario
	{
		private static readonly Point RStand = new Point(6, 0);
		private static readonly Point RWalk1 = new Point(7, 0);
		private static readonly Point RWalk2 = new Point(8, 0);
		private static readonly Point RWalk3 = new Point(9, 0);
		private static readonly Point RBrake = new Point(10, 0);
		private static readonly Point RJump = new Point(11, 0);
		private static readonly Point LStand = new Point(5, 0);
		private static readonly Point LWalk1 = new Point(4, 0);
		private static readonly Point LWalk2 = new Point(3, 0);
		private static readonly Point LWalk3 = new Point(2, 0);
		private static readonly Point LBrake = new Point(1, 0);
		private static readonly Point LJump = new Point(0, 0);
		private static readonly Point Death = new Point(12, 0);

		private static readonly int[] JumpStep = new[] { 10, 9, 8, 7, 6, 5, 5, 4, 3, 3, 2, 2, 1, 1, 0 };
		private static readonly int[] BrakeStep = new[] { 5, 4, 4, 5, 4, 4, 3, 4, 3, 2, 3, 2, 0, 1, 0 };
		private static readonly int[] DeathStep = new[] { 8, 7, 6, 5, 5, 4, 3, 3, 2, 2, 1, 1, 0, 0, -1, -1, -2, -2, -3, -3, -4, -5, -5, -6, -7, -8, -9, -10 };

		private Bitmap _sprite;
		private int _animeState;
		private Rectangle _srcRect;
		private int _pauseCount = 20;
		private bool _moveRight = true;
		private int _speed = 5;

		public Size Size
		{
			get { return _srcRect.Size; }
		}

		public bool IsStop
		{
			get { return _animeState == 0; }
		}

		public bool IsRunning
		{
			get { return 0 < _animeState && _animeState < 100; }
		}

		public bool IsJumping
		{
			get { return 100 <= _animeState && _animeState < 200; }
		}

		public bool IsTruning
		{
			get { return 200 <= _animeState && _animeState < 300; }
		}

		public bool IsDying
		{
			get { return 300 <= _animeState && _animeState < 400; }
		}

		public bool Jump()
		{
			if (IsRunning)
			{
				_animeState = 100;
				return true;
			}
			return false;
		}

		public bool Turn()
		{
			if (IsRunning)
			{
				_animeState = 200;
				return true;
			}
			return false;
		}

		public void Kill()
		{
			if (!IsDying)
			{
				_animeState = 300;
			}
		}

		public Mario()
		{
			_sprite = Properties.Resources.MarioSprites;
			if (Program.Rand.Next(100) < 8)
			{
				ToLuigi();
			}
			_srcRect = GetRect(RStand);

			if (Program.Rand.Next(100) < 10)
			{
				_speed++;
				if (Program.Rand.Next(100) < 10)
				{
					_speed++;
				}
			}
			else if (Program.Rand.Next(100) < 20)
			{
				_speed--;
				if (Program.Rand.Next(100) < 10)
				{
					_speed--;
				}
			}

			_pauseCount += Program.Rand.Next(20);
		}

		private void ToLuigi()
		{
			Color hair = Color.FromArgb(0xAC, 0x7C, 0x00);
			Color cap = Color.FromArgb(0xf8, 0x38, 0x00);

			Color hair2 = Color.FromArgb(0x00, 0xAC, 0x7C);
			Color cap2 = Color.FromArgb(0xff, 0xff, 0xff);

			var bmp = (Bitmap)_sprite.Clone();
			var palette = bmp.Palette;
			for (int i = 0; i < palette.Entries.Length; i++)
			{
				Color col = palette.Entries[i];
				if (col == hair)
				{
					palette.Entries[i] = hair2;
				}
				else if (col == cap)
				{
					palette.Entries[i] = cap2;
				}
			}
			bmp.Palette = palette;
			_sprite = bmp;
			_speed += 3;
		}

		private Rectangle GetRect( Point pt )
		{
			return GetRect(pt.X, pt.Y);
		}

		private Rectangle GetRect( int x, int y )
		{
			const int cx = 13;
			const int cy = 1;

			int w = _sprite.Width / cx;
			int h = _sprite.Height / cy;

			return new Rectangle(w * x, h * y, w, h);
		}

		public void Draw(Graphics g, Rectangle rect)
		{
			g.InterpolationMode = InterpolationMode.NearestNeighbor;
			g.DrawImage(_sprite, rect, _srcRect, GraphicsUnit.Pixel);
		}

		public void Animate(out int x, out int y)
		{
			int xMove = _moveRight ? _speed : -_speed;
			int yMove = 0;

			if (_animeState == 0)
			{
				// standing
				xMove = 0;
				if (--_pauseCount == 0)
				{
					_animeState++;
					_pauseCount = 10;

					_pauseCount += Program.Rand.Next(50);
				}
				_srcRect = GetRect(_moveRight ? RStand : LStand);
			}
			else if (_animeState == 1)
			{
				// walk 1
				_animeState++;
				_srcRect = GetRect(_moveRight ? RWalk1 : LWalk1);
			}
			else if (_animeState == 2 || _animeState == 4)
			{
				// walk 2
				_animeState++;
				_srcRect = GetRect(_moveRight ? RWalk2 : LWalk2);
			}
			else if (_animeState == 3)
			{
				// walk 3
				_animeState++;
				_srcRect = GetRect(_moveRight ? RWalk3 : LWalk3);
			}
			else if (_animeState == 5)
			{
				// walk 1
				_animeState = 2;
				_srcRect = GetRect(_moveRight ? RWalk1 : LWalk1);
			}
			else if (100 <= _animeState && _animeState < 200)
			{
				// ジャンプ
				int jstep = _animeState - 100;
				if (jstep < JumpStep.Length)
				{
					yMove = JumpStep[jstep];
					_animeState++;
					_srcRect = GetRect(_moveRight ? RJump : LJump);
				}
				else
				{
					jstep -= JumpStep.Length;
					if (jstep < JumpStep.Length)
					{
						yMove = -JumpStep[JumpStep.Length - 1 - jstep];
						_animeState++;
					}
					else
					{
						_srcRect = GetRect(_moveRight ? RWalk1: LWalk1);
						_animeState = 1;
					}
				}
			}
			else if (200 <= _animeState && _animeState < 300)
			{
				// ブレーキ
				int bstep = _animeState - 200;
				if (bstep < BrakeStep.Length)
				{
					xMove = BrakeStep[bstep] * ( _moveRight ? 1 : -1 );
					_animeState++;
					_srcRect = GetRect(!_moveRight ? RBrake : LBrake);
				}
				else
				{
					_moveRight = !_moveRight;
					_animeState = 0;
					xMove = 0;
				}
			}
			else if (300 <= _animeState)
			{
				// 退場
				int dstep = _animeState - 300;
				xMove = 0;
				if (dstep < DeathStep.Length)
				{
					yMove = DeathStep[dstep];
					_animeState++;
				}
				else
				{
					yMove = DeathStep[DeathStep.Length-1];
				}
				_srcRect = GetRect(Death);
			}

			x = xMove;
			y = yMove;
		}
	}
}
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace mario
{
	public class Mario
	{
		private const int StopFrame = -1;
		private const int RunFrame = 0;
		private const int JumpFrame = 100;
		private const int TurnFrame = 200;
		private const int DeadFrame = 300;
		private const int AnimationFrames = 100;

		private static readonly Point RStand = new Point(6, 0);
		private static readonly Point RRun1 = new Point(7, 0);
		private static readonly Point RRun2 = new Point(8, 0);
		private static readonly Point RRun3 = new Point(9, 0);
		private static readonly Point RBrake = new Point(10, 0);
		private static readonly Point RJump = new Point(11, 0);
		private static readonly Point LStand = new Point(5, 0);
		private static readonly Point LRun1 = new Point(4, 0);
		private static readonly Point LRun2 = new Point(3, 0);
		private static readonly Point LRun3 = new Point(2, 0);
		private static readonly Point LBrake = new Point(1, 0);
		private static readonly Point LJump = new Point(0, 0);
		private static readonly Point Death = new Point(12, 0);

		private static readonly Point[] RightRunStep = new[] { RRun1, RRun2, RRun3, RRun2 };
		private static readonly Point[] LeftRunStep = new[] { LRun1, LRun2, LRun3, LRun2 };
		private static readonly int[] JumpStep = new[] { 15, 12, 10, 8, 6, 4, 2, 1 };
		private static readonly int[] BrakeStep = new[] {5, 4, 5, 3, 4, 2, 0, 1};
		private static readonly int[] DeathStep = new[] {8, 6, 6, 5, 4, 2, 1, 0, -1, -2, -4, -5, -6, -6, -8, -10, -13, -16};

		private Bitmap _sprite;
		private int _animeState = StopFrame;
		private Rectangle _srcRect;
		private int _pauseCount = 20;
		private bool _moveRight = true;
		private int _speed = 5;
		private bool _mario = true;

		public Size Size
		{
			get { return _srcRect.Size; }
		}

		public int XHint { get; set; }
		public int YHint { get; set; }

		#region status

		public bool IsStop
		{
			get { return _animeState == StopFrame; }
		}

		public bool IsRunning
		{
			get { return RunFrame < _animeState && _animeState < RunFrame + AnimationFrames; }
		}

		public bool IsJumping
		{
			get { return JumpFrame <= _animeState && _animeState < JumpFrame + AnimationFrames; }
		}

		public bool IsTruning
		{
			get { return TurnFrame <= _animeState && _animeState < TurnFrame + AnimationFrames; }
		}

		public bool IsDying
		{
			get { return DeadFrame <= _animeState && _animeState < DeadFrame + AnimationFrames; }
		}

		public bool IsMario
		{
			get { return _mario; }
		}

		#endregion

		#region info

		public long MovePixels { get; private set; }
		public long JumpCount { get; private set; }
		public long TurnCount { get; private set; }

		#endregion

		#region action

		public void AddPauseCount(int pause)
		{
			_pauseCount += pause;
		}

		public bool Jump()
		{
			if (IsRunning || IsStop)
			{
				_animeState = JumpFrame;
				JumpCount++;
				return true;
			}
			return false;
		}

		public bool Run()
		{
			if (IsStop)
			{
				_animeState = RunFrame;
				return true;
			}
			return false;
		}

		public bool Turn()
		{
			if (IsRunning)
			{
				_animeState = TurnFrame;
				TurnCount++;
				return true;
			}
			return false;
		}

		public void Kill()
		{
			if (!IsDying)
			{
				_animeState = DeadFrame;
			}
		}

		#endregion

		public void UpdateSpeed()
		{
			_speed = _mario ? 5 : 8;

			if (Program.Rand.Next(100) < 15)
			{
				_speed++;
				if (Program.Rand.Next(100) < 10)
				{
					_speed++;
				}
			}
			else if (Program.Rand.Next(100) < 30)
			{
				_speed--;
				if (Program.Rand.Next(100) < 10)
				{
					_speed--;
				}
			}
		}

		public Mario()
		{
			_sprite = Properties.Resources.MarioSprites;
#if DEBUG
			if (Program.Rand.Next(100) < 50)
			{
				ToLuigi();
			}
#else
			if (Program.Rand.Next(100) < 8)
			{
				ToLuigi();
			}
#endif
			_srcRect = GetImageRect(RStand);
			UpdateSpeed();
			_pauseCount += Program.Rand.Next(20);
		}

		private void ToLuigi()
		{
			Color hair = Color.FromArgb(0xAC, 0x7C, 0x00);
			Color cap = Color.FromArgb(0xf8, 0x38, 0x00);

			Color hair2 = Color.FromArgb(0x00, 0xAC, 0x7C);
			Color cap2 = Color.FromArgb(0xff, 0xff, 0xff);

			var bmp = (Bitmap) _sprite.Clone();
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
			_mario = false;
		}

		private Rectangle GetImageRect(Point pt)
		{
			const int cx = 13;
			const int cy = 1;

			int w = _sprite.Width/cx;
			int h = _sprite.Height/cy;

			return new Rectangle(w*pt.X, h*pt.Y, w, h);
		}

		public void Draw(Graphics g, Rectangle rect)
		{
			g.InterpolationMode = InterpolationMode.NearestNeighbor;
			g.DrawImage(_sprite, rect, _srcRect, GraphicsUnit.Pixel);
		}

		public bool Animate(out int x, out int y)
		{
			int xMove = 0, yMove = 0;

			var lastRect = _srcRect;

			if (_animeState == StopFrame)
			{
				// 起立
				if (--_pauseCount == 0)
				{
					UpdateSpeed();

					_animeState = RunFrame;
					_pauseCount = 10;
					_pauseCount += Program.Rand.Next(50);

					if (Program.Rand.Next(100) < 10)
					{
						_pauseCount = 1;
					}
				}

				_srcRect = GetImageRect(_moveRight ? RStand : LStand);
			}
			else if (RunFrame <= _animeState && _animeState < RunFrame + AnimationFrames)
			{
				// 走る
				xMove = _moveRight ? _speed : -_speed;

				int wstep = _animeState - RunFrame;
				_srcRect = GetImageRect(_moveRight ? RightRunStep[wstep] : LeftRunStep[wstep]);
				if (wstep == RightRunStep.Length - 1)
				{
					_animeState = RunFrame;
				}
				else
				{
					_animeState++;
				}
			}
			else if (JumpFrame <= _animeState && _animeState < JumpFrame + AnimationFrames)
			{
				// ジャンプ
				xMove = _moveRight ? _speed : -_speed;

				int jstep = _animeState - JumpFrame;
				if (jstep < JumpStep.Length)
				{
					yMove = JumpStep[jstep];
					if (!IsMario && jstep%2 == 0)
					{
						yMove++;
						if (jstep%3 == 0)
						{
							yMove++;
						}
					}
					_animeState++;
					_srcRect = GetImageRect(_moveRight ? RJump : LJump);
				}
				else
				{
					jstep -= JumpStep.Length;
					if (jstep < JumpStep.Length)
					{
						yMove = -JumpStep[JumpStep.Length - 1 - jstep];
						if (!IsMario && jstep%2 == 0)
						{
							yMove--;
							if (jstep%3 == 0)
							{
								yMove--;
							}
						}
						_animeState++;
					}
					else
					{
						_srcRect = GetImageRect(_moveRight ? RightRunStep[0] : LeftRunStep[0]);
						_animeState = RunFrame;
					}
				}
			}
			else if (TurnFrame <= _animeState && _animeState < TurnFrame + AnimationFrames)
			{
				// ブレーキ
				int bstep = _animeState - TurnFrame;
				if (bstep < BrakeStep.Length)
				{
					xMove = BrakeStep[bstep]*(_moveRight ? 1 : -1);

					if (!IsMario && bstep%2 == 0)
					{
						xMove += _moveRight ? 1 : -1;
						if (bstep%3 == 0)
						{
							xMove += _moveRight ? 1 : -1;
						}
					}

					_animeState++;
					_srcRect = GetImageRect(!_moveRight ? RBrake : LBrake);
				}
				else
				{
					_moveRight = !_moveRight;
					_animeState = StopFrame;
					xMove = 0;
				}
			}
			else if (DeadFrame <= _animeState && _animeState < DeadFrame + AnimationFrames)
			{
				// 退場
				int dstep = _animeState - DeadFrame;
				if (dstep < DeathStep.Length)
				{
					yMove = DeathStep[dstep];
					_animeState++;
				}
				else
				{
					yMove = DeathStep[DeathStep.Length - 1];
				}
				_srcRect = GetImageRect(Death);
			}

			x = xMove;
			y = yMove;

			MovePixels += Math.Abs(xMove);

			return lastRect.Location != _srcRect.Location;
		}
	}
}
using System;
using System.Windows.Forms;

namespace mario
{
	public static class Program
	{
		public static Random Rand = new Random();
		public static int Zoom = 1;
		public static int MarioCount = 1;
		public static bool AutoDeath = true;

		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			bool oneWindow = false;
			for (int i = 0; i < args.Length; i++)
			{
				string arg = args[i].ToLower();
				if (arg.StartsWith("/z:"))
				{
					int zoom;
					if (int.TryParse(arg.Substring(3), out zoom))
					{
						if (1 < zoom && zoom <= 16)
						{
							Zoom = zoom;
						}
					}
				}
				if (arg.StartsWith("/m:"))
				{
					int marios;
					if (int.TryParse(arg.Substring(3), out marios))
					{
						MarioCount = Math.Min(32, Math.Max(1, marios));
					}
				}
				if (arg == "/onewindow")
				{
					oneWindow = true;
				}
				if (arg == "/nonautodeath")
				{
					AutoDeath = false;
				}
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			if (oneWindow)
			{
				Application.Run(new SpriteForm2());
			}
			else
			{
				Application.Run(new MainForm());
			}
		}
	}
}

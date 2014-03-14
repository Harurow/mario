using System;
using System.Text;
using System.Windows.Forms;

namespace mario
{
	public static class Program
	{
		public static Random Rand = new Random();
		public static int Zoom = 1;
		public static int MarioCount = 1;
		public static bool AutoDeath = true;
		public static bool ShowUsage = false;
		public static string Message = string.Empty;

		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Console.OpenStandardOutput();
			bool oneWindow = false;
			for (int i = 0; i < args.Length; i++)
			{
				string arg = args[i].ToLower();

				if (arg.StartsWith("-"))
				{
					arg = "/" + arg.Substring(1);
				}

				if (arg.StartsWith("/z:"))
				{
					int zoom;
					if (int.TryParse(arg.Substring("/z:".Length), out zoom))
					{
						if (1 < zoom && zoom <= 16)
						{
							Zoom = zoom;
							continue;
						}
					}
				}
				if (arg.StartsWith("/m:"))
				{
					int marios;
					if (int.TryParse(arg.Substring("/m:".Length), out marios))
					{
						if (1 < marios && marios <= 256)
						{
							MarioCount = marios;
							continue;
						}
					}
				}
				if (arg == "/onewindow")
				{
					oneWindow = true;
					continue;
				}
				if (arg == "/nonautodeath")
				{
					AutoDeath = false;
					continue;
				}
				if (arg.StartsWith("/msg:"))
				{
					Message = arg.Substring("/msg:".Length);
					AutoDeath = false;
					continue;
				}

				ShowUsage = true;
			}

			if (ShowUsage)
			{
				var sb = new StringBuilder();
				sb.AppendLine("Mario.exe usage");
				sb.AppendLine("Mario.exe [/z:<1-16>]     zoom ratio");
				sb.AppendLine("          [/m:<1-256>]    number of mario");
				sb.AppendLine("          [/onewindow]    one window mode");
				sb.AppendLine("          [/nonautodeath] disable auto death");
				sb.AppendLine("          [/msg:<text>]   mario message");

				MessageBox.Show(sb.ToString(), "Mario", MessageBoxButtons.OK);
				return;
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

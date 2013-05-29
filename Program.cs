using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace mario
{
	public static class Program
	{
		public static Random Rand = new Random();
		public static int Zoom = 1;
		public static int MarioCount = 1;

		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].ToLower().StartsWith("/z:"))
				{
					int zoom;
					if (int.TryParse(args[i].Substring(3), out zoom))
					{
						if (1 < zoom && zoom <= 16)
						{
							Zoom = zoom;
						}
					}
				}
				if (args[i].ToLower().StartsWith("/m:"))
				{
					int marios;
					if (int.TryParse(args[i].Substring(3), out marios))
					{
						if (1 < marios && marios <= 32)
						{
							MarioCount = marios;
						}
					}
				}
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}

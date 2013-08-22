using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace kibicom.tlib
{
	public delegate void josi_f_done(object args);

	public class t_args
	{
		public josi_f_done fdone;
	}
	public class t_uti
    {

        #region функции обратного вызова

        public static void f_fdone(t_args args)
		{
			if (args.fdone != null)
			{
				args.fdone(args);
			}
		}

		public static void f_fdone(t args)
		{
			if (args["f_done"].f_f<t_f<t,t>>() != null)
			{
				args["f_done"].f_f<t_f<t, t>>()(args);
			}
		}

		public static void f_fdone(t_f<t,t> f_done, t args)
		{
			if (f_done != null)
			{
				f_fdone(args);
			}
		}

        #endregion функции обратного вызова

        #region работа с графикой

        public static void f_draw_text(Graphics g, string str, Font font, Brush text_brush, Rectangle rect)
		{
			
			g.DrawString
			(
				str,
				font,
				text_brush,
				rect,
				StringFormat.GenericDefault
			);
		}

		public static void f_draw_text(Graphics g, string str, Font font, Brush text_brush, Brush shadow_brush, Rectangle rect)
		{
			g.DrawString
			(
				str,
				font,
				shadow_brush,
				new Rectangle(rect.X + 1, rect.Y + 1, rect.Width, rect.Height),
				StringFormat.GenericDefault
			);
			g.DrawString
			(
				str,
				font,
				text_brush,
				rect,
				StringFormat.GenericDefault
			);
		}


		public static void f_dtaw_text_shadow(System.Windows.Forms.DrawItemEventArgs e, string str)
		{
			Graphics g = e.Graphics;
			string s = str;
			RectangleF rect = e.Bounds;
			Font font = e.Font;
			StringFormat format = StringFormat.GenericDefault;
			float dpi = g.DpiY;
			using (GraphicsPath path = GetStringPath(s, dpi, rect, font, format))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				RectangleF off = rect;
				off.Offset(1, 1);

				using (GraphicsPath offPath = GetStringPath(s, dpi, off, font, format))
				{
					//Brush b = new SolidBrush(Color.FromArgb(100, 0, 0, 0));
					Brush b = new SolidBrush(ColorTranslator.FromHtml("#000"));
					g.FillPath(b, offPath);
					b.Dispose();
				}

				g.FillPath(Brushes.White, path);
				g.DrawPath(Pens.White, path);
			}
		}

		static GraphicsPath GetStringPath(string s, float dpi, RectangleF rect, Font font, StringFormat format)
		{
			GraphicsPath path = new GraphicsPath();
			// Convert font size into appropriate coordinates
			float emSize = dpi * font.SizeInPoints / 72;
			path.AddString(s, font.FontFamily, (int)font.Style, emSize, rect, format);

			return path;
		}

		#endregion работа с графикой

		#region работа со строками

		public static string fjoin(string itm1, char sep, string itm2)
		{
			if (itm1 != "" && itm2 != "")
			{
				return itm1 + sep.ToString() + itm2;
			}
			if (itm1 == "")
			{
				return itm2;
			}
			if (itm2 == "")
			{
				return itm1;
			}
			return "err";
		}	

		#endregion работа со строками

		#region работа с датами

		public static int f_udt(DateTime dt)
		{
			int dtcre_ut = (int)(dt.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
			return dtcre_ut;
		}

		public static int f_udt(string dt_str)
		{
			DateTime dtcre = new DateTime();
			DateTime.TryParse(dt_str, out dtcre);
			int dtcre_ut = (int)(dtcre.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
			return dtcre_ut;
		}

		public static string f_mssql_dt(DateTime dt)
		{
			string mssql_dt = dt.ToString("yyyy-MM-dd HH:mm:ss");
			return mssql_dt;
		}

		public static string f_mssql_dt(string dt_str)
		{
			if (dt_str == "") return dt_str;
			DateTime dt = new DateTime();
			DateTime.TryParse(dt_str, out dt);
			string mssql_dt = dt.ToString("yyyy-MM-dd HH:mm:ss");
			return mssql_dt;
		}

		public static string f_mysql_dt(DateTime dt)
		{
			string mssql_dt = dt.ToString("yyyyMMddHHmmss");
			return mssql_dt;
		}

		public static string f_mysql_dt(string dt_str)
		{
			DateTime dt = new DateTime();
			DateTime.TryParse(dt_str, out dt);
			string mssql_dt = dt.ToString("yyyyMMddHHmmss");
			return mssql_dt;
		}

		#endregion работа с датами

	}
}

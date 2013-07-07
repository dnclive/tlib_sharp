using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace kibicom.tlib
{
	/// <summary>
	/// <para>f_when promise engine</para>
	/// </summary>
	public partial class t
	{
		/// <summary>
		/// <para>f_when promise engine</para>
		/// <para>_</para>
		/// <para>PARAMS</para>
		/// <para>server_________________Server address {ip|name}/{server_instance}</para>
		/// <para>db_name_________________Data base name</para>
		/// <para>login__________________Login</para>
		/// <para>pass___________________pass</para>
		/// <para>timeout___________________timeout responce</para>
		/// <para>_</para>
		/// <para>RETURN</para>
		/// <para>good mood</para>
		/// </summary>
		public t f_when(string f_name, t_f<t, t> f_arg)
		{
			//добавляем функцию в массив вызываемых когда _f_done true
			this["f_when"][f_name]["f_arr"].Add(f_arg);

			if (this["f_when"][f_name]["done"].f_def(false).f_bool())
			{
				foreach (t f in (IList<t>)this["f_when"][f_name]["f_arr"])
				{
					//MessageBox.Show(f.val.GetType().FullName);
					t.f_f(f.f_f(), this["f_when"][f_name]["f_args"].f_def(this));
				}

				this["f_when"][f_name]["f_arr"].Clear();
			}

			return this;
		}

		public t f_when_done(t args)
		{
			string f_name = args["f_name"].f_str();
			t f_args = args["f_args"];
			this["f_when"][f_name]["done"].f_set(true);

			foreach (t f in (IList<t>)this["f_when"][f_name]["f_arr"])
			{
				t.f_f(f.f_f(), f_args);
			}

			this["f_when"][f_name]["f_arr"].Clear();

			return this;
		}

	}
}

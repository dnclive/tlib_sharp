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
		static public t f_when_cre(t args, object self)
		{
			t ret = new t()
			{
				{"args", args},
				{"self", self},
			};

			//регистритуем переданные в аргументах callbackи
			foreach (KeyValuePair<string, t> args_i in (IDictionary<string, t>) args)
			{
				if (args_i.Value.val != null)
				{
					//MessageBox.Show(args_i.Value.val.GetType().ToString());
				}
				//if (args_i)
				{

				}
			}

			return ret;
		}

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
			this["f_when"][f_name]["f_args"].f_set("when", this);

			if (this["f_when"][f_name]["done"].f_def(false).f_bool())
			{
				foreach (t f in (IList<t>)this["f_when"][f_name]["f_arr"])
				{
					//MessageBox.Show(f.val.GetType().FullName);
					t.f_f(f.f_f(), this["f_when"][f_name]["f_args"]);
				}

				this["f_when"][f_name]["f_arr"].Clear();
			}

			return this;
		}

		/// <summary>
		/// <para>f_when_done call when we cal call f_name function</para>
		/// <para>_</para>
		/// <para>PARAMS</para>
		/// <para>f_name_________________name of function what we can call</para>
		/// <para>f_args_________________args for calling this function</para>
		/// <para>_</para>
		/// <para>RETURN</para>
		/// <para>self</para>
		/// </summary>
		public t f_when_done(t args)
		{
			string f_name = args["f_name"].f_str();
			t f_args = args["f_args"];
			this["f_when"][f_name]["done"].f_set(true);
			this["f_when"][f_name]["f_args"] = f_args.f_set("when", this);

			foreach (t f in (IList<t>)this["f_when"][f_name]["f_arr"])
			{
				t.f_f(f.f_f(), this["f_when"][f_name]["f_args"]);
			}

			this["f_when"][f_name]["f_arr"].Clear();

			return this;
		}

		/// <summary>
		/// <para>f_when_done call when we cal call f_name function</para>
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
		public t_f<t, t> f_when_done_f(string f_name)
		{
			return new t_f<t, t>(delegate(t args2)
			{
				this.f_when_done(new t()
				{
					{"f_name", f_name},
					{"f_args", args2}
				});

				return new t();
			});
		}

		/// <summary>
		/// <para>f_fail</para>
		/// <para>_</para>
		/// <para>PARAMS</para>
		/// <para>message_________________сообщение будет переданно как args.err.message</para>
		/// <para>ex______________________исключение args.err.ex</para>
		/// <para>_</para>
		/// <para>RETURN</para>
		/// <para>self</para>
		/// </summary>
		public t f_fail(t args,  string message)
		{
			//добавляем message в err
			args["err"].f_set("message", message);
			return args;

		}


	}
}

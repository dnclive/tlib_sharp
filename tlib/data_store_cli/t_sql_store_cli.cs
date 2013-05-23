using System;
using System.Collections.Generic;
using System.Text;

namespace kibicom.tlib.data_store_cli
{
	public class t_sql_store_cli : t, i_sql_data_store<t_sql_store_cli>
	{
		/// <summary>
		/// <para>connect to ms sql server</para>
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
		public virtual t_sql_store_cli f_connect(t args)
		{
			return this;
		}

		/// <summary>
		/// <para>set current database for current open connection</para>
		/// <para>_</para>
		/// <para>PARAMS</para>
		/// <para>db_name_________________Data Base name</para>
		/// <para>_</para>
		/// <para>RETURN</para>
		/// <para>good mood</para>
		/// </summary>
		public virtual t_sql_store_cli f_set_db(t args)
		{
			return this;
		}

		/// <summary>
		/// <para>execute no queyr sql command</para>
		/// <para>_</para>
		/// <para>PARAMS</para>
		/// <para>cmd_________________Select sql command text</para>
		/// <para>_</para>
		/// <para>RETURN</para>
		/// <para></para>
		/// </summary>
		public virtual t_sql_store_cli f_exec_cmd(t args)
		{
			return this;
		}

		/// <summary>
		/// <para>execute select query and return DataTable</para>
		/// <para>_</para>
		/// <para>PARAMS</para>
		/// <para>cmd_________________Select sql command text</para>
		/// <para>tab_name____________Name for returning table</para>
		/// <para>f_done______________Callback function</para>
		/// <para>_</para>
		/// <para>RETURN</para>
		/// <para>tab_________________requested table</para>
		/// </summary>
		public virtual t_sql_store_cli f_select(t args)
		{
			return this;
		}

		public virtual t f_make_ins_query(t args)
		{
			return new t();
		}

		public virtual t f_dispose(t args)
		{
			return new t();
		}
	}
}

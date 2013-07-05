using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.OleDb;
using System.Windows.Forms;

namespace kibicom.tlib.data_store_cli
{
	public class t_oledb_cli : t_sql_store_cli
	{

		public t_oledb_cli()
		{
			this["is_connected"] = new t(false);
		}

		public t_oledb_cli(t args)
		{
			f_connect(args);

			t.f_f("f_done", args);

		}

		/// <summary>
		/// <para>connect to dbf</para>
		/// <para>_</para>
		/// <para>PARAMS</para>
		/// <para>location_________________folder with contains dbf file</para>
		/// <para>db_file_name_____________dbf file name</para>
		/// <para>login__________________Login</para>
		/// <para>pass___________________pass</para>
		/// <para>_</para>
		/// <para>RETURN</para>
		/// <para>good mood</para>
		/// </summary>
		public override t_sql_store_cli f_connect(t args)
		{

			if (args == null)
			{
				return this;
			}

			//входные параметры
			string location = args["location"].f_def(this["location"].f_str()).f_def("").f_str();
			//string server_name = args["server_name"].f_str();
			//string db_file_name = args["db_file_name"].f_def("").f_str();
			string login = args["login"].f_def(this["login"].f_str()).f_def("").f_str();
			string pass = args["pass"].f_def(this["pass"].f_str()).f_def("").f_str();

			bool conn_keep_open = args["conn_keep_open"].f_def(true).f_bool();

			//если уже подключен то выходим
			//и входные параметры те же
			if (this["is_connected"].f_def(false).f_bool() &&
				(this["location"].f_str() == location || location == "") //&&
				//this["db_file_name"].f_str()==db_file_name&
				//this["login"].f_str() == login || login != "" &&
				//this["pass"].f_str()==pass || pass!="")
				)
			{
				return this;
			}

			//формируем строку подключения, без указания конкретной БД
			string sql_conn_str = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
									@"Data Source=" +
									(location == "" ? "" : location) + ";" +
				//(db_file_name==""?"":db_file_name)+";"+
									@"Extended Properties=dBASE IV;User ID=Admin;Password=;";//+
									//@"Connect Timeout=30;";

			//sql_conn_str = "Provider=SQLOLEDB;OLE DB Services=-4;Data Source="+location+";Integrated Security=SSPI;";

			//создаем подключение
			OleDbConnection sql_conn = new OleDbConnection(sql_conn_str);

			

			//выносим в global нашего объекта
			this["sql_conn_str"] = new t(sql_conn_str);
			this["sql_conn"] = new t(sql_conn);
			this["location"] = new t(location);
			//this["server_name"] = new t(server_name);
			//this["db_file_name"] = new t(db_file_name);
			this["login"] = new t(login);
			this["pass"] = new t(pass);

			//пробуем поднять содединение...
			try
			{
				//MessageBox.Show(sql_conn.ConnectionTimeout.ToString());
				sql_conn.Open();
				this["is_connected"].f_val(true);

				//if (!conn_keep_open)
				{
					sql_conn.Close();
					//this["is_connected"].f_val(false);
				}
			}
			catch (Exception ex)
			{
				this["is_connected"].f_val(false);

				ex.Data.Add("args", args);

				t.f_f(args["f_fail"].f_f(), new t() 
				{ 
					{ "message", "connection failed" },
					{ "ex", ex}
				});
				return this;
			}


			//вызываем f_done и сообщаем что все ок
			t.f_f(args["f_done"].f_f(), new t() { { "message", "connection is ok" } });

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
		public override t_sql_store_cli f_set_db(t args)
		{
			OleDbConnection conn = this["sql_conn"].f_val<OleDbConnection>();

			//если уже установлена необходимая БД просто уходим
			if (this["db_name"].f_str() == args["db_name"].f_str())
			{
				return this;
			}

			Console.WriteLine("this:" + this["db_name"].f_str());
			Console.WriteLine("args:" + args["db_name"].f_str());

			//заменяем текущее значение db_name, если переданое не null
			string db_name = this.f_replace("db_name", args["db_name"])["db_name"].f_str();

			Console.WriteLine("this:" + this["db_name"].f_str());
			Console.WriteLine("args:" + args["db_name"].f_str());
			Console.WriteLine("result:" + db_name);

			if (db_name != "")
			{
				conn.ChangeDatabase(db_name);
			}

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
		public override t_sql_store_cli f_exec_cmd(t args)
		{
			string cmd_text = args["cmd"].f_str();
			bool conn_keep_open = args["conn_keep_open"].f_def(false).f_bool();

			OleDbConnection conn = f_connect(args)["sql_conn"].f_val<OleDbConnection>();
			//OleDbConnection conn = args["sql_conn"].f_val<OleDbConnection>();

			bool is_connected = this["is_connected"].f_def(false).f_bool();

			OleDbCommand cmd = new OleDbCommand(cmd_text, conn);

			//MessageBox.Show(is_connected.ToString());
			//MessageBox.Show(conn_keep_open.ToString());

			try
			{
				//if (!is_connected || conn.State != ConnectionState.Open)
				if (conn.State != ConnectionState.Open)
				{
					conn.Open();
				}

				cmd.Prepare();
				int cmd_exec_cnt = cmd.ExecuteNonQuery();

				if (!conn_keep_open)
				{
					conn.Close();
				}

				//вызываем f_done
				t.f_fdone(args);

			}
			catch (Exception ex)
			{
				conn.Close();

				ex.Data.Add("args", args);

				t.f_f(args["f_fail"].f_f(), args.f_add(true, new t() 
				{ 
					{ "message", "connection failed" },
					{ "ex", ex}
				}));
			}

			//chb_db.Items.Add();

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
		public override t_sql_store_cli f_select(t args)
		{

			string cmd_text = args["cmd"].f_str();
			string tab_name = args["tab_name"].f_str();
			string query = args["each"]["query"].f_str();
			string sort = args["each"]["sort"].f_str();

			//OleDbConnection conn = this["sql_conn"].f_val<OleDbConnection>();
			OleDbConnection conn = f_connect(args)["sql_conn"].f_val<OleDbConnection>();
			//t_f<t, t> f_done = args["f_done"].f_f<t_f>();

			conn.Open();

			//выбираем нужную БД
			f_set_db(args);

			Console.WriteLine(conn.Database);

			//создаем адаптек для запроса
			OleDbDataAdapter ad = new OleDbDataAdapter(cmd_text, conn);

			//создаем таблицу для результата
			DataTable tab = new DataTable(tab_name);

			//получаем данные, заполняем таблицу
			ad.Fill(tab);

			conn.Close();

			//вкидываем в принятые параметры полеченную таблицу и возвращаем результат
			//в функцию обратного вызова
			args["tab"] = new t(tab);

			//если переданана функция перебора элементов результата (строк таблицы)
			if (args["f_each"] != null)
			{

				int i = -1;
				DataRow[] dr_arr = tab.Select(query, sort);
				foreach (DataRow dr in dr_arr)
				{
					i++;
					//добавляем к входным параметрам текущую строку и индекс этой строки
					//и вызываем f_each
					f_f("f_each", args.f_add(true, new t()
					{
						{	//добавляем к each переданному свои значения
							"each", args["each"].f_add(true, new t()
							{
								{"item",	dr},
								{"index",	i},
								{"count", dr_arr.Length}
							})
						}
					}));
				}

			}

			args.f_drop("each");

			//вызываем f_done
			t.f_fdone(args);

			return this;
		}

		public override t f_make_ins_query(t args)
		{
			DataTable tab = args["tab"].f_def(new DataTable()).f_val<DataTable>();
			string tab_name = args["tab_name"].f_def("").f_str();

			DataRow[] dr_arr = args["dr_arr"].f_def(tab.Select()).f_val<DataRow[]>();

			//string set_date_format_sql = "SET DATEFORMAT ymd \r\n";
			//string set_language_sql = "SET LANGUAGE Russian \r\n";
			string ins_sql_str = "";
			string ins_sql_head = "";
			string vals = "";
			int oper_dr_cnt = 0;
			foreach (DataRow dr in dr_arr)
			{
				//insert _table_name_
				string ins_dr_sql = " insert into " + tab_name;

				//собираем имена колонок таблицы
				// ( col1, col2, col3...)
				vals = "";
				foreach (DataColumn cl in tab.Columns)
				{
					vals = t_uti.fjoin(vals, ',', cl.ColumnName);
				}
				ins_dr_sql += " ( " + vals + " ) ";
				ins_sql_head = ins_dr_sql;
				//MessageBox.Show(vals);
				//собираем значения
				// values ( val1, val2, val3...)
				vals = "";
				foreach (DataColumn cl in tab.Columns)
				{
					vals = t_uti.fjoin(vals, ',', t_sql_builder.f_db_val(dr, cl));
				}

				ins_dr_sql += " values ( " + vals + " ) ; ";
				if (ins_sql_str == "")
				{
					ins_sql_str += ins_sql_head + " values ( " + vals + " ) ";
				}
				else
				{
					ins_sql_str +=  ", ( " + vals + " ) ";
				}
				
				//MessageBox.Show(vals);
				//MessageBox.Show(ins_sql_str);
				//break;
				oper_dr_cnt++;

				t.f_f("f_each", args.f_dub_mix(true, new t()
				{
					{"query", ins_dr_sql}
				}));

			}

			//string query = set_date_format_sql + set_language_sql + ins_sql_str;
			string query = ins_sql_str+";";

			t.f_f("f_done", args.f_add(true, new t()
			{
				{"query", query}
			}));

			return new t(){{"query",query}};
		}

		public override t f_dispose(t args)
		{
			OleDbConnection conn = f_connect(args)["sql_conn"].f_val<OleDbConnection>();

			if (conn == null)
			{
				return new t();
			}

			if (conn.State == ConnectionState.Open)
			{
				conn.Close();
			}

			return new t();
		}

	}
}

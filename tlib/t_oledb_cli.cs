using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.OleDb;

namespace tlib
{
	public class t_oledb_cli:t
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
		/// <para>db_file_name_________________dbf file name</para>
		/// <para>login__________________Login</para>
		/// <para>pass___________________pass</para>
		/// <para>_</para>
		/// <para>RETURN</para>
		/// <para>good mood</para>
		/// </summary>
		public t_oledb_cli f_connect(t args)
		{

			if (args == null)
			{
				return this;
			}

			//входные параметры
			string location = args["location"].f_def("").f_str();
			//string server_name = args["server_name"].f_str();
			//string db_file_name = args["db_file_name"].f_def("").f_str();
			string login = args["login"].f_def("").f_str();
			string pass = args["pass"].f_def("").f_str();

			//если уже подключен то выходим
			//и входные параметры те же
			if (this["is_connected"].f_def(false).f_val<bool>() &&
				this["location"].f_str()==location && location!="" //&&
				//this["db_file_name"].f_str()==db_file_name&
				//this["login"].f_str() == login && login != "" &&
				//this["pass"].f_str()==pass && pass!="")
				)
			{
				return this;
			}

			//формируем строку подключения, без указания конкретной БД
			string sql_conn_str =	@"Provider=Microsoft.Jet.OLEDB.4.0;"+
									@"Data Source="+
									(location==""?"":location)+";"+
									//(db_file_name==""?"":db_file_name)+";"+
									@"Extended Properties=dBASE IV;User ID=Admin;Password=;";

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
				sql_conn.Open();
				sql_conn.Close();
			}
			catch (SqlException sex)
			{
				this["is_connected"].f_val(false);
				t.f_f(args["f_fail"].f_f(), new t() 
				{ 
					{ "message", "connection failed" },
					{ "ex", sex}
				});
				return this;
			}

			this["is_connected"].f_val(true);

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
		public string f_set_db(t args)
		{
			SqlConnection conn = this["sql_conn"].f_val<SqlConnection>();

			//если уже установлена необходимая БД просто уходим
			if (this["db_name"].f_str() == args["db_name"].f_str())
			{
				return this["db_name"].f_str();
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

			return db_name;
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
		public t_oledb_cli f_exec_cmd(t args)
		{
			string cmd_text = args["cmd"].f_str();

			OleDbConnection conn = f_connect(args)["sql_conn"].f_val<OleDbConnection>();

			OleDbCommand cmd = new OleDbCommand(cmd_text, conn);

			try
			{

				conn.Open();

				int cmd_exec_cnt = cmd.ExecuteNonQuery();

				conn.Close();

				//вызываем f_done
				t.f_fdone(args);

			}
			catch (Exception ex)
			{
				conn.Close();
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
		public void f_select(t args)
		{

			string cmd_text = args["cmd"].f_str();
			string tab_name = args["tab_name"].f_str();
			string query = args["each"]["query"].f_str();
			string sort = args["each"]["sort"].f_str();

			SqlConnection conn = this["sql_conn"].f_val<SqlConnection>();

			//t_f<t, t> f_done = args["f_done"].f_f<t_f>();




			conn.Open();

			//выбираем нужную БД
			f_set_db(args);

			Console.WriteLine(conn.Database);

			//создаем адаптек для запроса
			SqlDataAdapter ad = new SqlDataAdapter(cmd_text, conn);

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
								{"index",	i}
							})
						}
					}));
				}

			}
			
			args.f_drop("each");

			//вызываем f_done
			t.f_fdone(args);

			return;
		}

		public string f_make_ins_query(t args)
		{
			DataTable tab = args["tab"].f_def(new DataTable()).f_val<DataTable>();
			string tab_name = args["tab_name"].f_def("").f_str();

			//string set_date_format_sql = "SET DATEFORMAT ymd \r\n";
			//string set_language_sql = "SET LANGUAGE Russian \r\n";
			string ins_sql_str = "";
			string vals = "";
			int oper_dr_cnt = 0;
			foreach (DataRow dr in tab.Rows)
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
				//MessageBox.Show(vals);
				//собираем значения
				// values ( val1, val2, val3...)
				vals = "";
				foreach (DataColumn cl in tab.Columns)
				{
					vals = t_uti.fjoin(vals, ',', t_sql_builder.f_db_val(dr, cl));
				}

				ins_dr_sql += " values ( " + vals + " ) ; ";

				ins_sql_str += ins_dr_sql+" \r\n";
				//MessageBox.Show(vals);
				//MessageBox.Show(ins_sql_str);
				//break;
				oper_dr_cnt++;

				t.f_f("f_each", args.f_add(true, new t()
				{
					{"query", ins_dr_sql}
				}));

			}

			//string query = set_date_format_sql + set_language_sql + ins_sql_str;
			string query = ins_sql_str;

			t.f_f("f_done", args.f_add(true, new t()
			{
				{"query", query}
			}));

			return query;
		}

	}
}

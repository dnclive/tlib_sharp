using System;
using System.Collections.Generic;
using System.Text;
using Community.CsharpSqlite.SQLiteClient;
using System.Data;
using kibicom.tlib;

namespace kibicom.tlib
{
	public class t_sqlite_cli:t
	{

		bool is_blocked = false;

		public t_sqlite_cli()
		{
			this["is_connected"] = new t(false);
		}

		public t_sqlite_cli(t args)
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
		public t_sqlite_cli f_connect(t args)
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
			string sql_conn_str = string.Format("Version=3,uri=file:{0}", location);

			//создаем подключение
			SqliteConnection sql_conn = new SqliteConnection(sql_conn_str);


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

				if (!conn_keep_open)
				{
					sql_conn.Close();
					this["is_connected"].f_val(false);
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
		public string f_set_db(t args)
		{
			SqliteConnection conn = this["sql_conn"].f_val<SqliteConnection>();

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
		/// <para>execute no query sql command</para>
		/// <para>_</para>
		/// <para>PARAMS</para>
		/// <para>cmd_________________Select sql command text</para>
		/// <para>_</para>
		/// <para>RETURN</para>
		/// <para></para>
		/// </summary>
		public t_sqlite_cli f_exec_cmd(t args)
		{
			string cmd_text = args["cmd"].f_str();
			bool conn_keep_open = args["conn_keep_open"].f_def(false).f_bool();

			bool block = args["block"].f_def(false).f_bool();

			if (block)
			{
				//f_exec_cmd - блокирующий
				lock (this)
				{
					if (!is_blocked)
					{
						is_blocked = true;
					}
				}
			}

			SqliteConnection conn = f_connect(args)["sql_conn"].f_val<SqliteConnection>();
			//OleDbConnection conn = args["sql_conn"].f_val<OleDbConnection>();

			bool is_connected = this["is_connected"].f_def(false).f_bool();

			SqliteCommand cmd = new SqliteCommand(cmd_text, conn);

			//MessageBox.Show(is_connected.ToString());
			//MessageBox.Show(conn_keep_open.ToString());

			try
			{
				//if (!is_connected || conn.State != ConnectionState.Open)
				//if (conn.State != ConnectionState.Open)
				{
					conn.Open();
				}

				//cmd.Prepare();
				int cmd_exec_cnt = cmd.ExecuteNonQuery();

				string err= cmd.GetLastError();

				//if (!conn_keep_open)
				{
					conn.Close();
				}

				//вызываем f_done
				t.f_fdone(args);

			}
			catch (Exception ex)
			{
				conn.Close();

				string sql_err = cmd.GetLastError();

				ex.Data.Add("args", args);

				t.f_f(args["f_fail"].f_f(), args.f_add(true, new t() 
				{ 
					{ "message", sql_err },
					{ "ex", ex}
				}));

			}

			//chb_db.Items.Add();

			is_blocked = false;

			return this;
		}

		public void f_select(t args)
		{

			string cmd_text = args["cmd"].f_str();
			string tab_name = args["tab_name"].f_str();
			string query = args["each"]["query"].f_str();
			string sort = args["each"]["sort"].f_str();

			SqliteConnection conn = this["sql_conn"].f_val<SqliteConnection>();

			//t_f<t, t> f_done = args["f_done"].f_f<t_f>();

			//conn.Open();

			//выбираем нужную БД
			f_set_db(args);

			Console.WriteLine(conn.Database);

			//создаем адаптер для запроса
			SqliteDataAdapter ad = new SqliteDataAdapter(cmd_text, conn);


			//создаем таблицу для результата
			DataTable tab = new DataTable(tab_name);

			//получаем данные, заполняем таблицу
			ad.Fill(tab);

			//conn.Close();

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


	}
}

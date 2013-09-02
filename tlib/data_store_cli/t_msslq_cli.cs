using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Windows.Forms;

namespace kibicom.tlib.data_store_cli
{
	public class t_msslq_cli : t_sql_store_cli
	{

		public t_msslq_cli()
		{

		}

		public t_msslq_cli(t args)
		{
			this["server"]=args["server"].f_def("");
			this["server_name"] = args["server_name"].f_def("");
			this["db_name"] = args["db_name"].f_def("");
			this["login"] = args["login"].f_def("");
			this["pass"] = args["pass"].f_def("");
			this["timeout"] = args["timeout"].f_def(300);
			this["sql_conn"] = args["conn"];
		}

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
		public override t_sql_store_cli f_connect(t args)
		{

			if (args == null)
			{
				return this;
			}

			if (!this["sql_conn"].f_is_empty())
			{
				this["is_connected"].f_val(true);
				return this;
			}

			//входные параметры
			string server = args["server"].f_def(this["server"].f_str()).f_def("").f_str();
			string server_name = args["server_name"].f_def(this["server_name"].f_str()).f_def("").f_str();
			string db_name = args["db_name"].f_def(this["db_name"].f_str()).f_def("").f_str();
			string login = args["login"].f_def(this["login"].f_str()).f_def("").f_str();
			string pass = args["pass"].f_def(this["pass"].f_str()).f_def("").f_str();
			string timeout = args["timeout"].f_def(this["timeout"].f_def(300).f_int()).f_str();

			SqlConnection conn = this["sql_conn"].f_def_set(args["conn"].f_val<SqlConnection>()).f_def(new SqlConnection()).f_val<SqlConnection>();

			bool reconnect = args["reconnect"].f_def(this["reconnect"].f_str()).f_def(true).f_bool();

			bool is_connected=this["is_connected"].f_def(false).f_bool();

			//если подключение содержит строку подключения и нельзя переподключаться то считаем что уже подключены
			if (conn.ConnectionString != ""&& !reconnect)
			{
				this["is_connected"].f_val(true);
				return this;
			}

			//если уже подключениы и нельзя переподключаться то возвращаемся
			if (is_connected && !reconnect)
			{
				return this;
			}

			//если уже подключен то выходим
			//и входные параметры те же
			if (is_connected &&
				(this["server"].f_str() == server || server == "") &&
				(this["server_name"].f_str() == server_name || server_name == "") &&
				(this["db_name"].f_str() == db_name || db_name == "") &&
				(this["login"].f_str() == login || login == "") &&
				(this["pass"].f_str() == pass || pass == ""))
			{
				return this;
			}

			//формируем строку подключения, без указания конкретной БД
			string sql_conn_str = "Server=" + server +
									(server_name == "" ? "" : "\\" + server_name) +
									(db_name == "" ? "" : ";Database=" + db_name) +
									";User Id=" + login +
									";Password=" + pass+
									";Connection Timeout="+timeout;
			
			//создаем подключение
			SqlConnection sql_conn = new SqlConnection(sql_conn_str);

			//sql_conn.ConnectionTimeout = timeout;

			//выносим в global нашего объекта
			this["sql_conn_str"] = new t(sql_conn_str);
			this["sql_conn"] = new t(sql_conn);
			this["server"] = new t(server);
			this["server_name"] = new t(server_name);
			this["db_name"] = new t(db_name);
			this["login"] = new t(login);
			this["pass"] = new t(pass);

			//пробуем поднять содединение...
			try
			{
				sql_conn.Open();
				sql_conn.Close();

				this["is_connected"].f_val(true);
			}
			catch (Exception ex)
			{
				this["is_connected"].f_val(false);

				ex.Data.Add("args", args);

				t.f_f(args["f_fail"].f_f(), this.f_add(true, new t() 
				{ 
					{ "message", "connection failed" },
					{ "ex", ex}
				}));
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
			SqlConnection conn = this["sql_conn"].f_val<SqlConnection>();

			//если уже установлена необходимая БД или не перенадна новая просто уходим
			if (this["db_name"].f_str() == args["db_name"].f_str() || args["db_name"].f_str() == "")
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
				try
				{
					conn.ChangeDatabase(db_name);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
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
		public t_sql_store_cli f_exec_cmd_(t args)
		{
			string cmd_text = args["cmd"].f_str();
			SqlConnection conn = this["sql_connection"].f_val<SqlConnection>();

			SqlCommand cmd = new SqlCommand(cmd_text, conn);


			return this;

			//chb_db.Items.Add();
		}
		public override t_sql_store_cli f_exec_cmd(t args)
		{
			string cmd_text = args["cmd"].f_str();
			bool conn_keep_open = args["conn_keep_open"].f_def(false).f_bool();
			bool exec_scalar = args["exec_scalar"].f_def(true).f_bool();
			bool transact = args["transact"].f_def(false).f_bool();
			bool rollback_fail=args["rollbakc_fail"].f_def(true).f_bool();
			bool commit_done = args["commit_done"].f_def(true).f_bool();

			SqlConnection conn = f_connect(args)["sql_conn"].f_val<SqlConnection>();

			//OleDbConnection conn = args["sql_conn"].f_val<OleDbConnection>();

			bool is_connected = this["is_connected"].f_def(false).f_bool();

			if (!cmd_text.ToLower().Contains("dateformat"))
			{
				string set_date_format_sql = "SET DATEFORMAT ymd \r\n";
				string set_language_sql = "SET LANGUAGE Russian \r\n";
				cmd_text=set_date_format_sql+set_language_sql+cmd_text;
			}

			SqlCommand cmd = new SqlCommand(cmd_text, conn);

			SqlTransaction tran = null;

			//MessageBox.Show(is_connected.ToString());
			//MessageBox.Show(conn_keep_open.ToString());

			try
			{
				//if (!is_connected || conn.State != ConnectionState.Open)
				if (conn.State != ConnectionState.Open)
				{
					conn.Open();
				}

				object res_cnt=null;
				cmd.Prepare();

				//если нужно откатывать при неудаче открываем транзакцию
				if (transact)
				{
					tran=conn.BeginTransaction();
					cmd.Transaction = tran;
				}
				
				if (exec_scalar)
				{
					res_cnt = cmd.ExecuteScalar();
				}
				else
				{
					res_cnt = cmd.ExecuteNonQuery();
				}

				//если нужно откатывать при неудаче и мы здесь
				//и f_done то подтверждаем транзакцию
				if (transact&&args["f_done"].f_is_empty())
				{
					tran.Commit();
				}


				if (!args["f_done"].f_is_empty())
				{
					//вызываем f_done
					t f_done_res = t.f_f("f_done", args.f_add(true, new t() { { "res_cnt", res_cnt } }));

					//если fdone вернул подтверждение транзакции или не вернул ничего то подтверждаем
					if (transact)
					{
						if (f_done_res["commit"].f_def(true).f_bool() )
						{
							tran.Commit();
						}
						else
						{
							tran.Rollback();
						}
					}
				}
				else
				{
					//если fdone вернул подтверждение транзакции или не вернул ничего то подтверждаем
					if (transact)
					{
						if (commit_done)
						{
							tran.Commit();
						}
						else
						{
							tran.Rollback();
						}
					}
				}

				if (!conn_keep_open)
				{
					conn.Close();
				}

				this["res_cnt"].f_set(res_cnt);
			}
			catch (Exception ex)
			{
				conn.Close();

				//ex.Data.Add("args", args);

				t f_fail_res=t.f_f(args["f_fail"].f_f(), args.f_add(true, new t() 
				{ 
					{ "message", "connection failed" },
					{ "ex", ex }
				}));

				//если f_fail вернул подтверждение транзакции или не вернул ничего то подтверждаем
				if (f_fail_res["commit"].f_def(rollback_fail).f_bool()&&transact)
				{
					tran.Commit();
				}
				else
				{
					tran.Rollback();
				}

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
			int timeout=args["timeout"].f_def(60).f_int();
			bool conn_keep_open = args["conn_keep_open"].f_def(false).f_bool();

			//OleDbConnection conn = f_connect(args)["sql_conn"].f_val<OleDbConnection>();
			SqlConnection conn = f_connect(new t().f_add(true, args).f_drop(new List<string>() { "f_done", "f_fail" }))
									["sql_conn"].f_val<SqlConnection>();

			//если соединиться не удалось вызываем fail и прекращаем работу
			if (!this["is_connected"].f_bool())
			{
				t.f_f(args["f_fail"].f_f(), args.f_add(false, this));
				return this;
			}

			//t_f<t, t> f_done = args["f_done"].f_f<t_f>();
			try
			{
				if (conn.State != ConnectionState.Open)
				{
					conn.Open();
				}

				//выбираем нужную БД
				f_set_db(args);

				Console.WriteLine(conn.Database);

				//создаем адаптер для запроса
				SqlDataAdapter ad = new SqlDataAdapter(cmd_text, conn);

				//устанавливаем timout команды
				ad.SelectCommand.CommandTimeout = timeout;

				//создаем таблицу для результата
				DataTable tab = new DataTable(tab_name);

				//получаем данные, заполняем таблицу
				ad.Fill(tab);

				if (!conn_keep_open)
				{
					conn.Close();
				}

				conn.Close();

				//вкидываем в принятые параметры полученную таблицу и возвращаем результат
				//в функцию обратного вызова
				args["tab"] = new t(tab);

				//если передананая функция перебора элементов результата (строк таблицы)
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

			}
			catch (Exception ex)
			{
				conn.Close();

				ex.Data.Add("args", args);

				t.f_f(args["f_fail"].f_f(), args.f_add(true, new t() 
				{ 
					{ "message", "query execution failed\r\n\r\n"+cmd_text },
					{ "ex", ex}
				}));
			}

			return this;
		}

		public override t_sql_store_cli f_store_tab(t args)
		{
			DataTable tab = args["tab"].f_val<DataTable>();

			f_make_ins_query(new t()
			{
				{"tab", tab},
				{
					"f_done", new t_f<t,t>(delegate (t args1)
					{
						string query = args1["query"].f_str();

						f_exec_cmd(new t()
						{
							//запрос блокирует клиента
							{"block", true},
							{"cmd", query},
							{"f_done", args["f_done"].f_f()},
							{"f_fail", args["f_fail"].f_f()},
							{
								"f_done_", new t_f<t,t>(delegate (t args2)
								{

									return new t();
								})
							},
							{
								"f_fail_", new t_f<t,t>(delegate (t args2)
								{

									return new t();
								})
							}
						});

						return new t();
					})
				},
				{
					"f_fail", new t_f<t,t>(delegate (t args1)
					{

						return new t();
					})
				},
			});

			return this;
		}

		/// <summary>
		/// <para>put (insert or update) table or row in store not use deleted engine</para>
		/// <para>_</para>
		/// <para>PARAMS</para>
		/// <para>cmd_________________Select sql command text</para>
		/// <para>tab_name____________Name for returning table</para>
		/// <para>f_done______________Callback function</para>
		/// <para>_</para>
		/// <para>RETURN</para>
		/// <para>tab_________________requested table</para>
		/// </summary>
		public t_sql_store_cli f_put_store(t args)
		{
			DataTable tab = args["tab"].f_val<DataTable>();

			f_make_ins_query(new t()
			{
				{"tab", tab},
				{
					"f_done", new t_f<t,t>(delegate (t args1)
					{
						string query = args1["query"].f_str();

						f_exec_cmd(new t()
						{
							//запрос блокирует клиента
							{"block", true},
							{"cmd", query},
							{
								"f_done", new t_f<t,t>(delegate (t args2)
								{

									return new t();
								})
							},
							{
								"f_fail", new t_f<t,t>(delegate (t args2)
								{

									return new t();
								})
							}
						});

						return new t();
					})
				},
				{
					"f_fail", new t_f<t,t>(delegate (t args1)
					{

						return new t();
					})
				},
			});

			return this;
		}

		/// <summary>
		/// <para>put (insert or update) table or row to store using deleted engine (de)</para>
		/// <para>_</para>
		/// <para>PARAMS</para>
		/// <para>cmd_________________Select sql command text</para>
		/// <para>tab_name____________Name for returning table</para>
		/// <para>f_done______________Callback function</para>
		/// <para>_</para>
		/// <para>RETURN</para>
		/// <para>tab_________________requested table</para>
		/// </summary>
		public t_sql_store_cli f_put_store_de(t args)
		{
			DataTable tab = args["tab"].f_val<DataTable>();
			DataRow row = args["row"].f_val<DataRow>();
			t t_tab = args["t_tab"];
			t t_row = args["t_row"];
			string tab_name = args["tab_name"].f_def(tab.TableName).f_str();
			string key_name = args["key_name"].f_def("id").f_str();

			if (tab == null)
			{
				tab = new DataTable(tab_name);
				if (row != null)
				{

				}
				else if (!t_row.f_is_empty())
				{

				}
			}

			

			f_make_ins_query(new t()
			{
				{"tab", tab},
				{"tab_name", tab_name},
				{"key_name", key_name},
				{"row_state", "added"},
				{
					"f_done", new t_f<t,t>(delegate (t args1)
					{
						string query = args1["query"].f_str();
						int processed_dr_cnt = args1["processed_dr_cnt"].f_int();
						
						//выполняем команды вставки
						f_store_put_exec_de(new t()
						{
							{"query", query},
							{"exprected_res_cnt", processed_dr_cnt},
							{
								"f_done", new t_f<t,t>(delegate (t args2)
								{

									//принимаем изменения в таблице
									tab.AcceptChanges();

									//err = 5000;
									//удаляем новые строки (которых еще не было в базе)
									foreach (DataRow dr in tab.Rows)
									{
										if (dr["deleted"] != DBNull.Value && dr.RowState == DataRowState.Unchanged)
										{
											dr.Delete();
										}
									}

									//err = 5000;
									//еще раз принимаем изменения
									tab.AcceptChanges();

									t.f_f("f_done", args.f_add(true, new t() { { "res_cnt", processed_dr_cnt } }));

									return new t();
								})
							}
						});

						return new t();

					})
				}
			});

			f_make_upd_query(new t()
			{
				{"tab", tab},
				{"tab_name", tab_name},
				{"id_key_name", key_name},
				{"row_state", "modify"},
				{
					"f_done", new t_f<t,t>(delegate (t args1)
					{
						string query = args1["query"].f_str();
						int processed_dr_cnt = args1["processed_dr_cnt"].f_int();

						//выполняем команды вставки
						f_store_put_exec_de(new t()
						{
							{"query", query},
							{"exprected_res_cnt", processed_dr_cnt},
							{
								"f_done", new t_f<t,t>(delegate (t args2)
								{

									//принимаем изменения в таблице
									tab.AcceptChanges();


									return new t();
								})
							}
						});

						return new t();

					})
				}
			});

			return this;
		}

		public override t f_make_ins_query(t args)
		{
			DataTable tab = args["tab"].f_val<DataTable>();
			string tab_name = args["tab_name"].f_str();
			string row_state = args["row_state"].f_str();

			t res = new t();

			string set_date_format_sql = "SET DATEFORMAT ymd \r\n";
			string set_language_sql = "SET LANGUAGE Russian \r\n";
			string ins_sql_str = "";
			string vals = "";
			int oper_dr_cnt = 0;
			int processed_dr_cnt = 0;
			foreach (DataRow dr in tab.Rows)
			{
				if (!row_state.ToLower().Contains(dr.RowState.ToString().ToLower())&&row_state!="")
				{
					continue;
				}

				//текущая строка попадает в запрос
				processed_dr_cnt++;

				//insert _table_name_
				ins_sql_str += " insert " + tab.TableName;



				//собираем имена колонок таблицы
				// ( col1, col2, col3...)
				vals = "";
				foreach (DataColumn cl in tab.Columns)
				{
					vals = t_uti.fjoin(vals, ',', cl.ColumnName);
				}
				ins_sql_str += " ( " + vals + " ) ";
				//MessageBox.Show(vals);
				//собираем значения
				// values ( val1, val2, val3...)
				vals = "";
				foreach (DataColumn cl in tab.Columns)
				{
					vals = t_uti.fjoin(vals, ',', t_sql_builder.f_db_val(dr, cl));
				}
				ins_sql_str += " values ( " + vals + " ) ; \r\n";
				//MessageBox.Show(vals);
				//MessageBox.Show(ins_sql_str);
				//break;
				oper_dr_cnt++;
			}

			string query = set_date_format_sql + set_language_sql + ins_sql_str;

			t.f_f("f_done", args.f_add(true, new t()
			{
				{"query", query},
				{"processed_dr_cnt", processed_dr_cnt},
			}));

			return new t()
			{
				{"query",query},
				{"processed_dr_cnt", processed_dr_cnt},
			};
		}

		static public t f_make_upd_query(t args)
		{
			DataTable tab = args["tab"].f_val<DataTable>();
			string tab_name = args["tab_name"].f_str();
			string id_key_name = args["id_key_name"].f_str();
			string row_state = args["row_state"].f_str();

			string set_date_format_sql = "SET DATEFORMAT ymd \r\n";
			string set_language_sql = "SET LANGUAGE Russian \r\n";
			string upd_sql_str = "";
			string vals = "";
			int oper_dr_cnt = 0;
			int processed_dr_cnt = 0;
			foreach (DataRow dr in tab.Rows)
			{

				if (!row_state.ToLower().Contains(dr.RowState.ToString().ToLower()) && row_state != "")
				{
					continue;
				}

				//текущая строка попадает в запрос
				processed_dr_cnt++;

				//собираем измененные значения
				// set col1=val1, col2=val2...
				vals = "";
				foreach (DataColumn cl in tab.Columns)
				{
					vals = t_uti.fjoin(vals, ',', cl.ColumnName + "=" + t_sql_builder.f_db_val(dr, cl));
				}

				//собираем строку обновления
				upd_sql_str += " update " + tab.TableName + " set " + vals + " where " + id_key_name + "=" + dr[id_key_name];

				//MessageBox.Show(upd_sql_str);

				//MessageBox.Show(" update "+tab.TableName+" set "+vals+" where "+id_key+"="+dr[id_key]);

				//break;
				oper_dr_cnt++;
			}

			string query = set_date_format_sql + set_language_sql + upd_sql_str;

			if (oper_dr_cnt > 0)
			{
				t.f_f("f_done", args.f_add(true, new t()
				{
					{"query", query},
					{"processed_dr_cnt", processed_dr_cnt},
				}));
			}
			else
			{
				t.f_f("f_fail", args.f_add(true, new t()
				{
					{
						"err", new t()
						{
							{ "message", "нет строк для обновления"}
						}
					}
				}));
			}

			return new t() { { "query", query }, { "processed_dr_cnt", processed_dr_cnt }, };
		}

		public t f_store_put_exec_de(t args)
		{
			string query = args["query"].f_str();
			int exprected_res_cnt = args["exprected_res_cnt"].f_int();

			try
			{
				int r_cnt = 0;
				//выполняем запрос
				if (query != "")
				{
					r_cnt += f_exec_cmd(new t()
					{
						{"cmd" , query},
						{"rollback_fail", true},
						{"exec_scalar", false},
						{
							"f_done", new t_f<t,t>(delegate (t args2)
							{
								int res_cnt = args2["res_cnt"].f_int();

								if (res_cnt==exprected_res_cnt)
								{
									t.f_f(args["f_done"].f_f(), args2);


									return new t(){{"commit", true}};
								}

								return new t() { { "commit", false } };
								
							})
						},
						{
							"f_fail", new t_f<t,t>(delegate (t args2)
							{
								
								t.f_f(args["f_fail"].f_f(), args2);

								return new t() { { "commit", false } };
								
							})
						}
					})["res_cnt"].f_int();
				}

				//если количество удачно сохраненных строк не соответствует количеству всего строк
				
				if (r_cnt != exprected_res_cnt)
				{
					throw (new Exception("Внимание!!! Не все данные были успешно сохранены!!! Обратитесь к администратору системы."));
				}
				
			}
			catch (Exception ex)
			{
				//как правило сохранить не удается если дублируется PK
				//здесь необходимо сделать проверку на эту ошибку

				//откатываем сделанные изменения
				//db.command.Transaction.Rollback();

				//тоже глючная функция
				try
				{
					//команда обновления генератора
					//db.command.CommandText = "exec dbo.sys_update_generator";
					//db.command.ExecuteNonQuery();
				}
				catch (Exception ex1)
				{

				}

				//MessageBox.Show("При сохранении заказа произошла ошибка, \r\n часть данных сохранить не удалось. \r\n Расчитайте заказ повторно - это исправит проблему!");

				//db.CloseDB();

				//пробуем еще раз сохранить данные
				//f_2_store(tab, id_key);

				//return;

			}

			//если запросы выполнены успешно

			//db.CloseDB();

			//err = 4000;
			

			return new t();
		}

		//преобразует таблицу из t в DataTable
		public t f_t_2_data_table(t args)
		{



			return new t();
		}

		public override t f_dispose(t args)
		{
			SqlConnection conn = this["sql_conn"].f_val<SqlConnection>();

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

		/*
		public void f_2_store(DataTable tab, string id_key)
		{
			string set_date_format_sql = "SET DATEFORMAT ymd \r\n";
			string set_language_sql = "SET LANGUAGE Russian \r\n";
			string upd_sql_str = "";
			string ins_sql_str = "";
			string vals = "";
			int oper_dr_cnt = 0;
			//MessageBox.Show(tab.Rows.Count.ToString());
			foreach (DataRow dr in tab.Rows)
			{
				//MessageBox.Show(dr.RowState.ToString()+"\r\n"+dr[id_key]);
				if (dr.RowState == DataRowState.Added)
				{
					//insert _table_name_
					ins_sql_str += " insert " + tab.TableName;



					//собираем имена колонок таблицы
					// ( col1, col2, col3...)
					vals = "";
					foreach (DataColumn cl in tab.Columns)
					{
						vals = fjoin(vals, ',', cl.ColumnName);
					}
					ins_sql_str += " ( " + vals + " ) ";
					//MessageBox.Show(vals);
					//собираем значения
					// values ( val1, val2, val3...)
					vals = "";
					foreach (DataColumn cl in tab.Columns)
					{
						vals = fjoin(vals, ',', f_db_val(dr, cl));
					}
					ins_sql_str += " values ( " + vals + " ) ; \r\n";
					//MessageBox.Show(vals);
					//MessageBox.Show(ins_sql_str);
					//break;
					oper_dr_cnt++;
				}
				if (dr.RowState == DataRowState.Modified)
				{

					//собираем измененные значения
					// set col1=val1, col2=val2...
					vals = "";
					foreach (DataColumn cl in tab.Columns)
					{
						vals = fjoin(vals, ',', cl.ColumnName + "=" + f_db_val(dr, cl));
					}

					//собираем строку обновления
					upd_sql_str += " update " + tab.TableName + " set " + vals + " where " + id_key + "=" + dr[id_key];

					//MessageBox.Show(upd_sql_str);

					//MessageBox.Show(" update "+tab.TableName+" set "+vals+" where "+id_key+"="+dr[id_key]);

					//break;
					oper_dr_cnt++;
				}

			}

			db.OpenDB();

			try
			{


				int r_cnt = 0;
				err = 1000;
				//выполняем запросы обновления
				if (upd_sql_str != "")
				{
					db.command.CommandText = set_date_format_sql + set_language_sql + upd_sql_str;
					r_cnt += db.command.ExecuteNonQuery();
				}

				err = 2000;
				//выполняем запросы вставки
				if (ins_sql_str != "")
				{
					db.command.CommandText = set_date_format_sql + set_language_sql + ins_sql_str;
					r_cnt += db.command.ExecuteNonQuery();
				}

				if (r_cnt != oper_dr_cnt)
				{
					throw (new Exception("Внимание!!! Не все данные были успешно сохранены!!! Обратитесь к администратору системы."));
				}



			}
			catch (Exception ex)
			{
				//как правило сохранить не удается если дублируется PK
				//здесь необходимо сделать проверку на эту ошибку

				//откатываем сделанные изменения
				db.command.Transaction.Rollback();

				//тоже глючная функция
				try
				{
					//команда обновления генератора
					db.command.CommandText = "exec dbo.sys_update_generator";
					db.command.ExecuteNonQuery();
				}
				catch (Exception ex1)
				{

				}

				MessageBox.Show("При сохранении заказа произошла ошибка, \r\n часть данных сохранить не удалось. \r\n Расчитайте заказ повторно - это исправит проблему!");

				db.CloseDB();

				//пробуем еще раз сохранить данные
				//f_2_store(tab, id_key);

				return;

			}

			//если запросы выполнены успешно

			db.CloseDB();

			err = 4000;
			//принимаем изменения в таблице
			tab.AcceptChanges();

			err = 5000;
			//удаляем новые строки (которых еще не было в базе)
			foreach (DataRow dr in tab.Rows)
			{
				if (dr["deleted"] != DBNull.Value && dr.RowState == DataRowState.Unchanged)
				{
					dr.Delete();
				}
			}

			err = 5000;
			//еще раз принимаем изменения
			tab.AcceptChanges();

		}
		*/


	}
}

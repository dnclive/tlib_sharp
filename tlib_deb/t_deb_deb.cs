using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

using kibicom.tlib;
using kibicom.tlib.data_store_cli;

namespace kibicom.tlib_deb
{
	class t_deb_deb:t
	{

		static t f_check_all()
		{
			bool ut_res=true;

			ut_res&=f_deb_deb(new t())["ut_res"].f_val<bool>();

			return new t() { { "ut_res", ut_res } };
		}

		/// <summary>
		/// Test for t_deb.f_set_deb_group(string group)
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		static public t f_set_deb_group_deb(t args)
		{

			//empty group
			Console.WriteLine("set deb_group, call f_set_deb_group group=''");

			string group = "";
			t_deb.f_set_deb_group(group);

			Console.WriteLine(t_deb.f_deb_group());




			return new t();
		}

		static public t f_set_context_info_deb(t args)
		{

			//empty group
			Console.WriteLine("test come debug messages out");

			t_deb.f_set_context_info(new t()
			{
				{"show_file", false},
				{"show_f_name", false},
				{"show_line", true},
			});

			Console.WriteLine("");




			return new t();
		}

		static public t f_deb_deb(t args)
		{

			bool ut_res = true;


			#region ut_1
			Console.WriteLine("UT\tmod:t_deb\tf:f_deb");

			//включаем вывод сообщений только группы unit_test
			t_deb.f_set_deb_group("unit_test");

			//включаем только вывод номера строки вызова f_deb
			t_deb.f_set_context_info(new t()
			{
				{"show_file", false},
				{"show_f_name", false},
				{"show_line", true},
			});
			
			t_deb.f_deb("main", "{0}:{1}:{2}","123", 456, 56.34);

			t_deb.f_deb("unit_test", "{0}:{1}:{2}", "123", 456, 56.34);

			t_deb.f_deb("other_group", "{0}:{1}:{2}", "123", 456, 56.34);

			ut_res &= true;

			Console.Write("\r\n\r\n");

			#endregion ut_1


			#region ut_2

			Console.WriteLine("UT\tmod:t_deb\tf:f_deb");

			//включаем вывод сообщений только группы unit_test
			t_deb.f_set_deb_group("unit_test_2");

			//включаем только вывод номера строки вызова f_deb и название метода
			t_deb.f_set_context_info(new t()
			{
				{"show_file", false},
				{"show_f_name", true},
				{"show_line", true},
			});

			t_deb.f_deb("main", "{0}:{1}:{2}", "123", 456, 56.34);

			t_deb.f_deb("unit_test", "{0}:{1}:{2}", "123", 456, 56.34);

			t_deb.f_deb("other_group", "{0}:{1}:{2}", "123", 456, 56.34);

			t_deb.f_deb("unit_test_2", "Тест группы 2. Status:{0}", "successed");

			ut_res &= true;

			Console.Write("\r\n\r\n");

			#endregion ut_2

			return new t(){{"ut_res",ut_res}};
		}


		static public t f_deb_deb_3(t args)
		{

			bool ut_res = true;

			#region ut_1
			Console.WriteLine("UT\tmod:t_deb\tf:f_deb");

			//включаем вывод сообщений только группы unit_test
			t_deb.f_set_deb_group("unit_test");

			//включаем только вывод номера строки вызова f_deb
			t_deb.f_set_context_info(new t()
			{
				{"show_file", false},
				{"show_f_name", false},
				{"show_line", true},
			});

			t var = new t()
			{
				{"key1", "val1"},
				{"key3", "val1"}
			};

			string var_json = var.f_json()["json_str"].f_str();

			t_deb.f_deb("main", "{0}", var_json);

			//t_deb.f_deb("unit_test", "{0}:{1}:{2}", "123", 456, 56.34);

			//t_deb.f_deb("other_group", "{0}:{1}:{2}", "123", 456, 56.34);

			ut_res &= true;

			Console.Write("\r\n\r\n");

			#endregion ut_1


			return new t() { { "ut_res", ut_res } };
		}

		#region deb sqlite_cli

		static public t f_deb_sqlite_cli(t args)
		{
			bool ut_res = true;

			#region ut_1
			Console.WriteLine("UT\tmod:t_sqlite_cli\tf:f_deb");

			//включаем вывод сообщений только группы unit_test
			t_deb.f_set_deb_group("unit_test");

			//включаем только вывод номера строки вызова f_deb
			t_deb.f_set_context_info(new t()
			{
				{"show_file", false},
				{"show_f_name", false},
				{"show_line", true},
			});

			//создаем клиента, подключаемся
			t_sqlite_cli cli = new t_sqlite_cli(new t()
			{
				{"location", "sqlite_db_test.db"},
				{"conn_keep_open", true}
			});

			//создаем таблицу
			cli.f_exec_cmd(new t()
			{
				{"cmd","CREATE TABLE TEST_TABLE ( COLA INTEGER, COLB TEXT, COLC DATETIME )"},
				{
					"f_done", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} created successfull...", "TEST_TABLE");

						return new t();
					})
				},
				{
					"f_fail", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} create is fail...", "TEST_TABLE");

						return new t();
					})
				}
			});

			//добавляем строку
			cli.f_exec_cmd(new t()
			{
				{"cmd","INSERT INTO TEST_TABLE ( COLA, COLB, COLC ) VALUES (123,'ABC','2008-12-31 18:19:20' )"},
				{
					"f_done", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} created successfull...", "TEST_TABLE");

						return new t();
					})
				},
				{
					"f_fail", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} create is fail...", "TEST_TABLE");

						return new t();
					})
				}
			});

			//еще одну
			cli.f_exec_cmd(new t()
			{
				{"cmd","INSERT INTO TEST_TABLE ( COLA, COLB, COLC ) VALUES (124,'DEF', '2009-11-16 13:35:36' )"},
				{
					"f_done", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} created successfull...", "TEST_TABLE");

						return new t();
					})
				},
				{
					"f_fail", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} create is fail...", "TEST_TABLE");

						return new t();
					})
				}
			});

			//сразу несколько
			cli.f_exec_cmd(new t()
			{
				{"cmd","INSERT INTO TEST_TABLE ( COLA, COLB, COLC ) VALUES (124,'DEF', '2009-11-20 13:35:36' );\r\n"+
						"INSERT INTO TEST_TABLE ( COLA, COLB, COLC ) VALUES (127,'TRU', '2009-11-25 13:35:36' );\r\n"},
				{
					"f_done", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} created successfull...", "TEST_TABLE");

						return new t();
					})
				},
				{
					"f_fail", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} create is fail...", "TEST_TABLE");

						return new t();
					})
				}
			});

			//***выборка

			cli.f_select(new t()
			{
				{"cmd","SELECT COLA, COLB, COLC FROM TEST_TABLE"},
				{
					"f_done", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} select successfull...", "TEST_TABLE");

						DataTable tab = args1["tab"].f_val<DataTable>();

						foreach(DataRow dr in tab.Rows)
						{
							t_deb.f_deb("unit_test", "{0}   {1}   {2}", dr["cola"].ToString(), dr["colb"].ToString(), dr["colc"].ToString());
						}

						return new t();
					})
				},
				{
					"f_fail", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} create is fail...", "TEST_TABLE");

						return new t();
					})
				}
			});

			t_deb.f_deb("unit_test", "{0}", cli["is_connected"].f_str());

			//t_deb.f_deb("unit_test", "{0}:{1}:{2}", "123", 456, 56.34);

			//t_deb.f_deb("other_group", "{0}:{1}:{2}", "123", 456, 56.34);

			ut_res &= true;

			Console.Write("\r\n\r\n");

			#endregion ut_1


			return new t() { { "ut_res", ut_res } };
		}

		static public t f_deb_sqlite_cli_3(t args)
		{
			bool ut_res = true;

			#region ut_1
			Console.WriteLine("UT\tmod:t_sqlite_cli\tf:f_deb");

			//включаем вывод сообщений только группы unit_test
			t_deb.f_set_deb_group("unit_test");

			//включаем только вывод номера строки вызова f_deb
			t_deb.f_set_context_info(new t()
			{
				{"show_file", false},
				{"show_f_name", false},
				{"show_line", true},
			});

			File.Delete("sqlite_db_test.db");

			//создаем клиента, подключаемся
			t_sqlite_cli cli = new t_sqlite_cli(new t()
			{
				{"location", "kibicom_wd_josi.db"},
				{"conn_keep_open", true}
			});

			//создаем таблицу tab_customer
			cli.f_exec_cmd(new t()
			{
				{"cmd",@"CREATE TABLE IF NOT EXISTS tab_customer (
						id_tab INTEGER PRIMARY KEY AUTOINCREMENT,
						  id INTEGER,
						  dtcre INTEGER,
						  deleted INTEGER,
						  id_login INTEGER,
						  name TEXT,
						  fio TEXT,
						  phone TEXT,
						  email TEXT,
						  site TEXT,
						  wd_idcustomer INTEGER
						)"},
				{
					"f_done", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} created successfull...", "TEST_TABLE");

						return new t();
					})
				},
				{
					"f_fail", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} create is fail...", "TEST_TABLE");

						return new t();
					})
				}
			});

			//tab_address
			cli.f_exec_cmd(new t()
			{
				{"cmd",@"CREATE TABLE IF NOT EXISTS tab_address (
						id_tab INTEGER PRIMARY KEY AUTOINCREMENT,
						  id INTEGER,
						  dtcre INTEGER,
						  deleted INTEGER,
						  id_login INTEGER,
						  name TEXT
						)"},
				{
					"f_done", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} created successfull...", "TEST_TABLE");

						return new t();
					})
				},
				{
					"f_fail", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} create is fail...", "TEST_TABLE");

						return new t();
					})
				}
			});

			return new t();

			//добавляем строку
			cli.f_exec_cmd(new t()
			{
				{"cmd","INSERT INTO TEST_TABLE ( COLA, COLB, COLC ) VALUES (123,'ABC','2008-12-31 18:19:20' )"},
				{
					"f_done", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} created successfull...", "TEST_TABLE");

						return new t();
					})
				},
				{
					"f_fail", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} create is fail...", "TEST_TABLE");

						return new t();
					})
				}
			});

			//еще одну
			cli.f_exec_cmd(new t()
			{
				{"cmd","INSERT INTO TEST_TABLE ( COLA, COLB, COLC ) VALUES (124,'DEF', '2009-11-16 13:35:36' )"},
				{
					"f_done", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} created successfull...", "TEST_TABLE");

						return new t();
					})
				},
				{
					"f_fail", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} create is fail...", "TEST_TABLE");

						return new t();
					})
				}
			});

			//сразу несколько
			cli.f_exec_cmd(new t()
			{
				{"cmd","INSERT INTO TEST_TABLE ( COLA, COLB, COLC ) VALUES (124,'DEF', '2009-11-20 13:35:36' );\r\n"+
						"INSERT INTO TEST_TABLE ( COLA, COLB, COLC ) VALUES (127,'TRU', '2009-11-25 13:35:36' );\r\n"},
				{
					"f_done", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} created successfull...", "TEST_TABLE");

						return new t();
					})
				},
				{
					"f_fail", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} create is fail...", "TEST_TABLE");

						return new t();
					})
				}
			});

			//***выборка

			cli.f_select(new t()
			{
				{"cmd","SELECT COLA, COLB, COLC FROM TEST_TABLE"},
				{
					"f_done", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} select successfull...", "TEST_TABLE");

						DataTable tab = args1["tab"].f_val<DataTable>();

						foreach(DataRow dr in tab.Rows)
						{
							t_deb.f_deb("unit_test", "{0}   {1}   {2}", dr["cola"].ToString(), dr["colb"].ToString(), dr["colc"].ToString());
						}

						return new t();
					})
				},
				{
					"f_fail", new t_f<t,t>(delegate (t args1)
					{

						t_deb.f_deb("unit_test", "table {0} create is fail...", "TEST_TABLE");

						return new t();
					})
				}
			});

			t_deb.f_deb("unit_test", "{0}", cli["is_connected"].f_str());

			//t_deb.f_deb("unit_test", "{0}:{1}:{2}", "123", 456, 56.34);

			//t_deb.f_deb("other_group", "{0}:{1}:{2}", "123", 456, 56.34);

			ut_res &= true;

			Console.Write("\r\n\r\n");

			#endregion ut_1


			return new t() { { "ut_res", ut_res } };
		}



		#endregion deb sqlite_cli

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kibicom.tlib;

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

	}
}

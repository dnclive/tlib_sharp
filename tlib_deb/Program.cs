using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kibicom.tlib;

namespace kibicom.tlib_deb
{
	class Program
	{
		static void Main(string[] args)
		{
			//test t_deb
			//t_deb_deb.f_deb_deb(new t());

			//t_deb_deb.f_deb_deb_3(new t());

			//тестирование клиента sqlite
			//t_deb_deb.f_deb_sqlite_cli(new t());

			//создание базы kibicom_wd_josi.db
			t_deb_deb.f_deb_sqlite_cli_3(new t());

			Console.ReadLine();
		}
	}
}

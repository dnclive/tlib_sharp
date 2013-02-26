using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace tlib
{

	#region tdeb класс отладки позволяет накапливать и выводить отладочные сообщения

	//класс сообщения объекты этого класса добавляются в вписки tdeb.warns и tdeb.errs
	//в качестве сообщений
	public class tdebmess
	{
		public string group;		//отладочная группа сообщения
		public string mess;			//текст сообщения

		//конструктор, создает сообщение с указанным текстом mess, в указанной группе group
		public tdebmess(string group, string mess)
		{
			this.group = group;
			this.mess = mess;
		}
	}

	//отладочный класс
	//позволяет накапливать и выводить сообщения определенной отладочной группы
	//ВНИМАНИЕ класс статический, все его методы вызываются без создания объекта
	//класса
	public class tdeb										//отладочный класс
	{
		//отладка
		public static string deb_group = "deb";				//отладочная группа по умолчанию
		public static string mark = "nerocalc";				//метка ошибки
		public static bool debug = true;						//отладка - включена/выключена
		public static int err;								//метка точки ошибки
		public static List<string> warns;					//список предупреждений
		public static List<string> errs;					//список ошибок

		public static List<tdebmess> debmess;				//список предупреждений с группами

		//инициализация статического класса
		static tdeb()
		{
			//debug=deb;
			err = 0;
			warns = new List<string>();	//создаем пустой список предупреждений
			errs = new List<string>();	//создаем пустой список ошибок

			debmess = new List<tdebmess>();	//создаем универсальный список отладочных сообщений

		}

		public static int fdeb(string mess)
		{
			int err = 1;

			if (!debug) return err;				//если отладка выключена ничего не делаем

			warns.Add("_deb_" + mess);			//добавляем сообщение

			return err;
		}

		//*** добавляем универсальное сообщение mess сообщение в указанную группу group
		public static int fdeb(string group, string mess)
		{
			int err = 1;

			if (!debug) return err;				//если отладка выключена ничего не делаем

			//если текущая группа (выводимая, сообщения которой будут отображены)
			//не соответствует переданной сюда то нет смысла добавлять сообщение в список
			//оно все равно не будет выведено пользователю
			if (!deb_group.ToLower().Contains(group.ToLower())) return err;

			//иначе добавляем сообщение в список
			debmess.Add(new tdebmess(group, mess));

			return err;
		}

		//вывод отладочных сообщений
		public static int fshow()
		{
			int err = 1;
			if (!debug) return err;				//если отладка выключена ничего не делаем
			//вывод предупреждений и ошибок

			foreach (string swarn in warns)
			{
				//Atechnology.ecad.Calc.SystemScript.RunCalc.AddWarning(swarn, mark, 0);
			}
			warns.Clear();

			foreach (string serr in errs)
			{
				//Atechnology.ecad.Calc.SystemScript.RunCalc.AddError(serr, mark, 0, 0);
			}
			errs.Clear();


			return err;
		}

		//вывод универсальных отладочных сообещний в стандартное окно WinDraw
		public static int fshowdebmess(string group)
		{
			int err = 1;
			if (!debug) return err;				//если отладка выключена ничего не делаем
			//вывод предупреждений и ошибок

			foreach (tdebmess debmessi in debmess)
			{
				//MessageBox.Show("\r\n\r\n\r\n\r\n"+group.ToLower());
				if (group.ToLower().Contains(debmessi.group.ToLower()))
				{
					//	pb.fadd_warn(eqkvl3.fitmkey("head").fval(), eqkvl3.fitmkey("mess").fval(), 
					//		help_href);
					//Atechnology.ecad.Calc.SystemScript.RunCalc.AddWarning("_deb_" + debmessi.group + "_" + debmessi.mess, mark, 0);
				}
			}
			debmess.Clear();

			return err;
		}

		//*** вывод универсальных отладочных сообещний в расчетное окно NeroCalc
		//group - группа сообщения относящиеся к которой необходимо показать
		//pb - ссылка на окно расчета
		public static int fshowdebmess(string group, fpb pb)
		{
			int err = 1;
			if (!debug) return err;				//если отладка выключена ничего не делаем
			//вывод предупреждений и ошибок
			int i = 0;	//перебираем сообщения в списке
			foreach (tdebmess debmessi in debmess)
			{
				//если отладочная группа текущего сообщения соответствует запрошенной
				if (group.ToLower().Contains(debmessi.group.ToLower()))
				{
					//добавляем сообщение в окно расчета
					pb.fadd_warn(mark, "_deb_" + debmessi.group + "_" + debmessi.mess, "");

					//добавляем сообщение в стандартное окно расчета
					//Atechnology.ecad.Calc.SystemScript.RunCalc.AddWarning("_deb_" + debmessi.group + "_" + debmessi.mess, mark, 0);
				}
			}

			//очищаем список отладочных сообещний
			debmess.Clear();

			return err;
		}

		//добавление сообщения-ошибки в стандартное окно
		public static int ferrmes(string group, string message, int numpos)
		{
			if (!debug) return 1;				//если отладка выключена ничего не делаем
			//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n"+eqkvl3.fitmkey("mess").fval(),"WinDraw", 
			//	System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
			//Atechnology.ecad.Calc.SystemScript.RunCalc.AddError(message, group, 0, numpos);
			return 1;
		}

		//добавление сообщения-предупреждения в стандартное окно
		public static int fwarnmes(string group, string message, int numpos)
		{
			if (!debug) return 1;				//если отладка выключена ничего не делаем
			//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n"+eqkvl3.fitmkey("mess").fval(),"WinDraw", 
			//	System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
			//Atechnology.ecad.Calc.SystemScript.RunCalc.AddWarning(message, group, numpos);
			return 1;
		}

		//очистка всех списков 
		public static int fclear()
		{
			warns.Clear();
			errs.Clear();
			debmess.Clear();

			return 1;
		}
	}

	#endregion tdeb класс отладки позволяет накапливать и выводить отладочные сообщения
	
}

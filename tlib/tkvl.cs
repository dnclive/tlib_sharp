using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tlib
{
	#region trange

	//диапазон значений
	public class trange
	{
		public double low;							//нижняя граница
		public double top;							//верхняя граница
		public string err = "";					//ошибки

		//параметры
		public char sepopen = '[';				//открывающая скобка
		public char sepval = ',';					//разделитель занчений
		public char sepclose = ']';				//закрывающая скобка

		public trange(string val)
		{
			int pst = 0;							//0 - ищем открывающую скобку 1 - значение 2 - разделители
			//3 - выход
			string sval = "";
			foreach (char ch in val)
			{
				if (ch == sepopen && pst == 0)		//ищем открывающую скобку
				{
					pst = 2;						//переходим в состояние поиска нижнего предела - 1
				}

				if (ch == sepval && pst == 1)			//ищем разделитель значений
				{
					if (!double.TryParse(sval.Replace('.', ','), out low))//пробуем получить число нижнего предела
					{
						err = "Не возможно распознать начало диапазона...";
					}
					sval = "";					//очищаем текущее строковое значение
					pst = 2;						//переходим в состояние поиска значения - 1
				}
				if (ch == sepclose && pst == 1)		//ищем закрывающую скобку
				{
					if (!double.TryParse(sval.Replace('.', ','), out top))//пробуем получить значение
					{
						err = "Не возможно распознать конец диапазона...";
					}
					sval = "";					//очищаем текущее строковое значение
					pst = 3;						//переходим в состояние выхода
				}
				if (pst == 1)				//состояния запоминания значений
				{
					sval += ch;
				}
				if (pst == 3)						//анализ закончен выходим из перебора
				{
					break;
				}
				if (pst == 2)						//пропускаем разделители и скобки - 2
				{
					pst = 1;
				}
			}
		}

		public bool finrange(string val)
		{
			bool eq = false;								//результат сравнения

			double ival;								//значение конвертированное в целое	

			//val.Replace('.',',');

			if (!double.TryParse(val.Replace('.', ','), out ival))//если конвертация не удалась то выдаем ошибку
			{
				if (err == "")							//если ошибки в этом диапазоне не было то пишем
				{
					err = "ошибка конвертации значения в целое...";
					return eq;							//возвращаем отрицательный результат
				}
			}

			if (ival > low && ival < top)						//если значение попадает в диапазон
			{
				eq = true;								//возвращаем положительный результат
			}

			return eq;
		}

		public string frange(int low, int top)
		{
			this.low = low;
			this.top = top;
			return sepopen + low.ToString() + sepval + top.ToString() + sepclose;
		}

		public double[] frange()
		{
			return new double[] { low, top };
		}

	}

	#endregion trange

	#region enum

	//диапазон значений
	public class tenum
	{
		public List<string> items = new List<string>();//список элементов перечисления
		public string err = "";					//ошибки

		//параметры
		public char sepopen = '{';				//открывающая скобка
		public char sepval = ',';					//разделитель занчений
		public char sepclose = '}';				//закрывающая скобка

		public tenum(string val)
		{
			int pst = 0;							//0 - начало 1 - значение 2 - разделители 3 - выход
			string sval = "";

			foreach (char ch in val)
			{
				//MessageBox.Show(ch.ToString());
				if (ch == sepopen && pst == 0)		//ищем открывающую скобку
				{
					pst = 2;						//переходим в состояние поиска элементов - 1
				}
				if (ch == sepval && pst == 1)			//ищем разделитель значений
				{
					//MessageBox.Show(sval);
					items.Add(sval);			//добавляем очередное значение в список элементов
					sval = "";					//очищаем текущее строковое значение
					pst = 2;						//пропускаем разделитель
				}
				if (ch == sepclose && pst == 1)		//ищем закрывающую скобку
				{
					//MessageBox.Show(sval);
					items.Add(sval);			//добавляем последнее значение в список элементов
					sval = "";					//очищаем текущее строковое значение
					pst = 3;						//пропускаем разделитель
				}
				if (pst == 1)						//состояния запоминания значений
				{
					sval += ch;
				}
				if (pst == 2)
				{
					pst = 1;
				}
				if (pst == 3)
				{
					break;
				}
			}
			if (sval != "")
			{
				err = "Возможно отсутствует закрывающая скобка перечисления...";
			}
		}

		public string fadditm(string val)
		{
			items.Add(val);
			string sen = "" + sepopen;
			foreach (string itm in items)
			{
				sen += itm + sepval;
			}
			sen = sen.Substring(0, sen.Length - 1) + sepclose;
			return sen;
		}

		public bool fcontains(string val)
		{
			bool eq = false;						//результат сравнения

			foreach (string v in this.items)
			{

				if (v == val)						//если попадается равное значение то...
				{
					eq = true;					//возвращаем положительный результат
					break;						//далее нет смысла искать
				}

			}

			return eq;							//возврат результата
		}

		public bool fequal(tenum items)
		{
			bool eq = false;

			return eq;
		}

	}

	#endregion enum

	#region tsep

	public class tsep
	{

		public string sep;										//строка последовательности разделителей

		//разделители	
		public char cgr;										//разделитель колекций групп 
		public char gr;											//разделитель невзаимосвязанных групп
		public char kvl;										//разделитель список*список
		public char or;											//разделитель элеметнов ИЛИ-группы
		public char and;										//разделитель элемениов И-группы
		public char kv;											//разделитель ключа и значения
		public char i = ',';										//разделитель элементов
		public char rgopen = '[';									//начало диапазона
		public char rgclose = ']';								//конец диапазона
		public char enopen = '{';									//начало перечисления
		public char enclose = '}';								//конец перечисления

		public tsep()
		{
			sep = "#%^|&:";

			//берем известные разделители
			if (sep.Contains("#")) cgr = '#';
			if (sep.Contains("%")) gr = '%';
			if (sep.Contains("^")) kvl = '^';
			if (sep.Contains("|")) or = '|';
			if (sep.Contains("&")) and = '&';
			if (sep.Contains(":")) kv = ':';
		}

		public int fsepind(char sep)
		{
			int i = 0;
			while (i < this.sep.Length)
			{
				if (this.sep[i++] == sep) return i - 1;
			}
			return -1;
		}

		public static bool f_1_up_2(char sep1, char sep2)
		{
			tsep sep = new tsep();
			if (sep.fsepind(sep1) < sep.fsepind(sep2))
			{
				return true;
			}
			return false;
		}

	}

	#endregion tsep

	#region tkvl

	public class tkvl
	{
		//key:val
		public string key = "";									//ключ
		public string val = "";									//значение
		public string vtyp;										//тип значения атома
		public trange range;									//диапазон
		public tenum en;										//перечисление

		//relat
		public List<tkvl> list;									//список элементов ядра содержит атомы
		//public string sep;										//последовательность разделителей
		public char rel;										//логическая связь элементов разделенная &
		public string mark;										//метка элемента позволяет найти его в списке по метке

		public tuti uti = new tuti();							//утилиты

		public string err; 										//ошибка

		//параметры
		//разделители

		public tsep sep;										//разделитель

		//public char sepgr='%';									//разделитель невзаимосвязанных групп
		//public char sepkvl='^';									//разделитель ключ:значение*список
		//public char sepor='|';									//разделитель элеметнов ИЛИ-группы
		//public char sepand='&';									//разделитель элемениов И-группы
		//public char sepkv=':';									//разделитель ключа и значения
		//public char sepi=',';									//разделитель элементов
		//public char seprgopen='[';								//начало диапазона
		//public char seprgclose=']';								//конец диапазона
		//public char sepenopen='{';								//начало перечисления
		//public char sepenclose='}';								//конец перечисления

		//отладка
		//public tdeb deb=new tdeb(true, "nerocalc");

		public tkvl()
		{
			//items=new List<tatom>();
			sep = new tsep();										//создаем разделители

			rel = sep.sep[0];										//запоминаем свой разделитель 
			//sepi=rel;
			//rel=sep.sep[0];									//верхний разделитель
			list = new List<tkvl>();								//создаем список
		}

		public tkvl(char rel)
			: this()
		{
			this.rel = rel;
		}

		public tkvl(string str)
			: this()
		{
			tdeb.fdeb("tkvl", "tkvl" + str);

			//return tkvl(rel

			//tdeb.fdeb("tkvl_создание объекта"+s+"|"+sep[i].ToString());
			//замена функции делителя строки-множества на строки-элементы
			//string[] kvl=str.Split(rel);							//делим строку по разделителю
			string[] kvl = uti.fsplit(str, rel);						//делим строку по разделителю
			//MessageBox.Show("\r\n\r\n\r\n\r\n\r"+rel.ToString()+"   "+kvl.Length.ToString());
			if (rel == sep.kv)										//если разделитель ключ:значение
			{
				//tdeb.fdeb("tkvl_sepkv"+s);
				fkeyval(str);										//парсим ключ значение
				//rel=sep.i;										//запоминаем свой разделитель 
				return;												//прекращаем разбор 
			}
			if (rel != sep.kv)
			{
				foreach (string kv in kvl)							//иначе строка делится логическим разделителем
				{
					//tdeb.fdeb("tkvl_seplogic"+kv);
					this.list.Add(new tkvl(kv, sep.fsepind(rel) + 1));//создаем следующий узел и парсим его содержимое
					//по следующему разделителю

				}
			}

		}

		public tkvl(char rel, string str)
			: this()
		{
			this.rel = rel;
			//sep.i=rel;

			tdeb.fdeb("tkvl", "tkvl___" + str);

			//tdeb.fdeb("tkvl_создание объекта"+s+"|"+sep[i].ToString());

			//замена функции делителя строки-множества на строки-элементы
			//string[] kvl=str.Split(rel);							//делим строку по разделителю
			string[] kvl = uti.fsplit(str, rel);						//делим строку по разделителю

			if (rel == sep.kv)										//если разделитель ключ:значение
			{
				//tdeb.fdeb("tkvl_sepkv"+s);
				fkeyval(str);										//парсим ключ значение
				rel = sep.i;											//запоминаем свой разделитель 
				return;												//прекращаем разбор 
			}
			if (rel != sep.kv)
			{
				foreach (string kv in kvl)							//иначе строка делится логическим разделителем
				{
					//tdeb.fdeb("tkvl_seplogic"+kv);
					//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n\r\n"+sep.sep[sep.fsepind(rel)+1].ToString());
					this.list.Add(new tkvl(kv, sep.fsepind(rel) + 1));//создаем следующий узел и парсим его содержимое
					//по следующему разделителю

				}
			}
		}

		public tkvl(string str, int sepind)
			: this()
		{
			tdeb.fdeb("tkvl", "tkvl" + str);
			//MessageBox.Show("\r\n\r\n\r\n\r\\r\n\r\n видно!!!"+str+sep.sep[sepind].ToString());
			//sep.i=sep.sep[sepind];								//берем первый разделитель
			rel = sep.sep[sepind];											//запоминаем свой разделитель 
			//tdeb.fdeb("tkvl_создание объекта"+s+"|"+sep[i].ToString());
			//замена функции делителя строки-множества на строки-элементы
			//string[] kvl=str.Split(rel);							//делим строку по разделителю
			string[] kvl = uti.fsplit(str, rel);						//делим строку по разделителю
			//MessageBox.Show("\r\n\r\n\r\n\r\n\r"+str+"    "+rel.ToString()+"   "+kvl.Length.ToString());
			if (rel == sep.kv)									//если разделитель ключ:значение
			{
				//tdeb.fdeb("tkvl_sepkv"+s);
				fkeyval(str);										//парсим ключ значение
				//rel=sepi;										//запоминаем свой разделитель 
				return;											//прекращаем разбор 
			}
			if (rel != sep.kv)
			{
				foreach (string kv in kvl)						//иначе строка делится логическим разделителем
				{
					//tdeb.fdeb("tkvl_seplogic"+kv);
					this.list.Add(new tkvl(kv, sepind + 1));		//создаем следующий узел и парсим его содержимое
					//по следующему разделителю

				}
			}
		}

		public tkvl(string key, string val)
			: this()
		{
			fkeyval(key, val);
		}

		public string fjoin(string itm1, char sep, string itm2)
		{
			if (itm1 != "" && itm2 != "")
			{
				return itm1 + sep.ToString() + itm2;
			}
			if (itm1 == "")
			{
				return itm2;
			}
			if (itm2 == "")
			{
				return itm1;
			}
			return "err";
		}

		public bool fparsekv()
		{
			//анализ ключ:значение
			//значение строка
			vtyp = "str";											//изначально считаем что значение атомарно

			//регион
			if (val.Contains(sep.rgopen.ToString()) &&			//наличие открывающей скобки
				val.Contains(sep.i.ToString()) &&					//наличие разделителя границ диапазона
				val.Contains(sep.rgclose.ToString()))			//наличие закрывающей скобки
			{
				range = new trange(val);							//анализируем значение-диапазон
				if (range.err != "")
				{
					err = "key:" + key + " | " + range.err;
				}
				if (range.err == "")
				{
					vtyp = "rg";
				}
			}

			//перечисление
			if (val.Contains(sep.enopen.ToString()) &&			//наличие начала перечисления
				val.Contains(sep.enclose.ToString()))			//наличие конча перечисления
			{
				en = new tenum(val);								//анализируем значение-перечисление

				if (en.err != "")
				{
					err = "key:" + key + " | " + en.err;
				}
				if (en.err == "")
				{
					vtyp = "en";
				}
			}

			return true;
		}

		public string fkeyval(string keyval)
		{
			if (keyval != null)
			{
				//замена функции делителя строки-множества на строки-элементы
				//string[] kv=keyval.Split(sep.kv);							//делим строку по разделителю
				string[] kv = uti.fsplit(keyval, rel);						//делим строку по разделителю
				//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n"+keyval);
				if (kv.Length == 1)
				{
					this.key = uti.finjesc(kv[0], rel);
				}
				if (kv.Length == 2)
				{
					this.key = uti.finjesc(kv[0], rel);
					this.val = uti.finjesc(kv[1], rel);
				}
				fparsekv();
			}

			return uti.fjoin(uti.finjesc(this.key, sep.kv), sep.kv, uti.finjesc(this.val, sep.kv));
		}

		public string fkeyval(string key, string val)
		{
			if (key != null && val != null)
			{
				this.key = key;
				this.val = val;
				this.rel = sep.kv;
				fparsekv();
			}
			return uti.fjoin(uti.finjesc(this.key, sep.kv), sep.kv, uti.finjesc(this.val, sep.kv));
		}

		public string fkeyval()
		{
			string str = "";
			if (this.key == "")
			{
				str = this.val;
			}
			if (this.val == "")
			{
				str = this.key;
			}
			if (this.key != "" && this.val != "")
			{
				//str=this.key+sep.kv.ToString()+this.val;
				str = uti.fjoin(uti.finjesc(this.key, sep.kv), sep.kv, uti.finjesc(this.val, sep.kv));
			}
			//MessageBox.Show("\r\n\r\n\r\n\r\n\r"+str);
			return str;
		}

		public string flist()
		{
			//tdeb.fdeb("flist_"+key+"|"+val);
			string str = "";
			int i = 0;
			while (i < list.Count)							//получаем строку списка
			{
				str += list[i].ToString();
				if (i < list.Count - 1)
				{
					str += rel.ToString();
				}
				i++;									//индексация
			}

			return str;
		}

		public string fkvl()
		{

			string kvstr = fkeyval();						//ключ:значение			

			tdeb.fdeb("tkvl", "fkvl_kvstr_");

			string str = this.ToString();						//строка списка

			if (kvstr != "" && list.Count > 0)				//если строка key:val не пустая и список не пустой
			{
				kvstr += sep.kvl.ToString();				//добавляем разделитель
			}

			kvstr += str;									//и строку списка

			return kvstr;									//возвращаем
		}

		public string ToString()
		{
			//tdeb.fdeb("tostring_"+key+"|"+val);


			//для целей отладки часто пльзовался этой фукнцией что очень снижает скорость работы!
			//по окончании отладки отключаем ее работу!!!
			//return "";	

			string str = "";

			string lstr = "";								//строка списка
			int i = 0;
			while (i < list.Count)							//получаем строку списка
			{
				//lstr+=uti.fjoin(lstr, rel,list[i].ToString());
				lstr = uti.fjoin(lstr, rel, uti.finjesc(list[i].ToString(), rel));
				//if (i<list.Count-1)
				//{
				//	lstr+=rel.ToString();
				//}
				i++;									//индексация
			}


			string keyval = fkeyval();					//строка своего key:val

			if (keyval != "" && lstr != "")
			{
				//str=keyval+sep.kvl.ToString()+lstr;
				str = uti.fjoin(uti.finjesc(keyval, sep.kvl), sep.kvl, lstr);
			}

			if (keyval != "" && lstr == "")
			{
				str = keyval;
			}

			if (keyval == "" && lstr != "")
			{
				str = lstr;
			}

			return str;
		}

		public string fstr()
		{
			//tdeb.fdeb("tostring_"+key+"|"+val);


			//для целей отладки часто пльзовался этой фукнцией что очень снижает скорость работы!
			//по окончании отладки отключаем ее работу!!!
			//return "";	

			string str = "";

			string lstr = "";								//строка списка
			int i = 0;
			while (i < list.Count)							//получаем строку списка
			{
				//lstr+=uti.fjoin(lstr, rel,list[i].ToString());
				lstr = uti.fjoin(lstr, rel, uti.finjesc(list[i].fstr(), rel));
				//if (i<list.Count-1)
				//{
				//	lstr+=rel.ToString();
				//}
				i++;									//индексация
			}


			string keyval = fkeyval();					//строка своего key:val

			if (keyval != "" && lstr != "")
			{
				//str=keyval+sep.kvl.ToString()+lstr;
				str = uti.fjoin(uti.finjesc(keyval, sep.kvl), sep.kvl, lstr);
			}

			if (keyval != "" && lstr == "")
			{
				str = keyval;
			}

			if (keyval == "" && lstr != "")
			{
				str = lstr;
			}

			return str;
		}

		public string fval(string val)
		{
			if (val != null)
			{
				this.val = val;
				//fkeyval(null);
			}
			return fval();
		}

		public string fkey(string key)
		{
			if (key != null)
			{
				this.key = key;
			}
			return fkey();
		}

		public string fval()
		{
			return uti.finjesc(this.val, rel);
		}

		public string fkey()
		{
			return uti.finjesc(this.key, rel);
		}

		public List<tkvl> fflat(char sep)
		{
			//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n"+sep+rel+this.sep.fsepind(sep).ToString()+this.sep.fsepind(rel).ToString());
			List<tkvl> lkvl = new List<tkvl>();
			if (this.sep.fsepind(sep) < this.sep.fsepind(rel))
			{
				return lkvl;
			}
			//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n"+sep+rel+(this.sep.fsepind(sep)>this.sep.fsepind(rel)+1).ToString());
			if (this.sep.fsepind(sep) > this.sep.fsepind(rel) + 1)
			{
				//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n"+sep+rel+(this.sep.fsepind(sep)>this.sep.fsepind(rel)+1).ToString());
				foreach (tkvl kvl in this.list)
				{
					List<tkvl> rkvl = kvl.fflat(sep);
					foreach (tkvl rkvli in rkvl)
					{
						//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n"+sep+rel+kvl.ToString()+kvl.rel);
						lkvl.Add(rkvli);
					}
				}
				return lkvl;
			}
			if (this.sep.fsepind(sep) == this.sep.fsepind(rel) + 1)
			{
				foreach (tkvl kvl in this.list)
				{
					//List<tkvl> rkvl=kvl.fflat(sep);
					//foreach(tkvl rkvli in rkvl)
					//{
					//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n"+sep+rel+kvl.ToString()+kvl.rel);
					lkvl.Add(kvl);
					//}
				}
			}
			return lkvl;
		}

		public string frange(int low, int top)
		{
			if (vtyp == "rg")
			{
				val = range.frange(low, top);
				fkeyval(null);
				return val;
			}
			return "";
		}

		public double[] frange()
		{
			if (vtyp == "rg")
			{
				return range.frange();
			}
			return new double[0];
		}

		public double flow()
		{
			if (vtyp == "rg")
			{
				return range.low;
			}
			return 0;
		}

		public double ftop()
		{
			if (vtyp == "rg")
			{
				return range.top;
			}
			return 0;
		}

		public List<string> fen()
		{
			if (vtyp == "en")
				return en.items;
			return new List<string>(0);
		}

		public string fen(string itm)
		{
			val = en.fadditm(itm);
			fkeyval(null);
			return val;
		}

		public tkvl fmark(string mark)						//присваивает элементу метку
		{
			this.mark = mark.ToLower();
			return this;
		}

		public tkvl fbymark(string mark)
		{
			tkvl marked_kvl = new tkvl(this.rel);
			foreach (tkvl kvl in this.list)
			{
				if (kvl.mark == mark.ToLower())
				{
					marked_kvl.fadd(kvl);
					//return kvl;
				}
			}
			return marked_kvl;
		}

		public tkvl fbymark(string[] mark)
		{
			tkvl kvl = new tkvl(list[3].rel);
			//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r"+list[3].rel.ToString());
			foreach (string marki in mark)
			{
				tkvl kvlmark = fbymark(marki);

				foreach (tkvl kvli in kvlmark.list)
				{
					kvl.fadd(kvli);

				}
			}
			return kvl;
		}

		public tkvl fadd(string itm)
		{
			tkvl kvl = new tkvl(itm, sep.fsepind(this.rel) + 1);
			this.list.Add(kvl);
			return kvl;
		}

		public tkvl fadd(tkvl kvl)
		{
			//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r"+this.list.Count.ToString()+kvl.rel.ToString());
			this.list.Add(kvl);

			return kvl;
		}

		public tkvl fadd(string key, string val)
		{
			tkvl kvl = new tkvl(key, val);
			this.list.Add(kvl);
			return kvl;
		}

		public tkvl fitmkey(string key)
		{
			foreach (tkvl kvl in this.list)
			{
				if (kvl.fkey() == key)
				{
					return kvl;
				}
			}
			return null;
		}

		public tkvl fitmval(string val)
		{
			foreach (tkvl kvl in this.list)
			{
				if (kvl.fval() == val)
				{
					return kvl;
				}
			}
			return null;
		}

		public string fadditm(string itm)
		{
			val = en.fadditm(itm);
			fkeyval(null);
			return val;
		}

		public string fadd2list(string itm)					//добавляем элемент 
		{
			this.list.Add(new tkvl(itm));
			return itm;
		}

		public tkvl fadd2list(tkvl itm)					//добавляем элемент 
		{
			this.list.Add(itm);
			return itm;
		}

	}

	#endregion tatom

	#region tkveq

	public class tkveq
	{
		string err = "";
		string eqstr = "";									//строка эквивалентности
		bool eq;											//эквивалентность

		public bool feq(tkvl kvl1, tkvl kvl2, string opt)
		{
			return true;
		}

		public string fcompare(tkvl kvl1, tkvl kvl2)			//сравнение пар - результат вид соответствия - строка
		{
			//bool eq=false;									//результат сравнения
			string eqstr = "not equal";							//полагаем что не эквивалентны

			if (kvl1.key != kvl2.key)								//если не равны ключи пар то нет смысла сравнивать значения
			{
				return eqstr;										//возвращаем отричательный результат - НЕ ЭКВИВАЛЕНТНЫ!!!
			}

			//атомарное
			if (kvl1.vtyp == "str")								//если свое атомарное
			{
				eqstr = fstr2any(kvl1, kvl2);								//сравниваем свое атомарное с переданным
			}

			//диапазоны
			if (kvl1.vtyp == "rg")									//если свое значение диапазон
			{
				eqstr = frg2any(kvl1, kvl2);									//проверяем попадание значения в диапазон
			}

			//перечисления
			if (kvl1.vtyp == "en")									//если свое значение перечисление
			{
				eqstr = fen2any(kvl1, kvl2);									//проверяем попадание значения в диапазон
			}

			return eqstr;
		}

		//сравнители

		public string fstr2any(tkvl kvl1, tkvl kvl2)
		{
			string eq = "not equal";				//результат сравнения

			if (kvl2.vtyp == "str")
			{
				if (kvl1.val == kvl2.val)			//если значения совпадают
				{
					eq = "equal";					//результат положительный - ЭКВИВАЛЕНТНЫ
				}
			}

			if (kvl2.vtyp == "rg")
			{
				int ival;
				if (!int.TryParse(kvl1.val, out ival))
				{
					if (err == "")
					{
						err = "key:" + kvl1.key + " | ошибка преобразования строки в целое";
					}
					eq = "not equal";
					return eq;
				}

				//свое атомарное значение попадает в переданный диапазон
				if (ival >= kvl2.range.low && ival <= kvl2.range.top)
					eq = "contained";
			}

			if (kvl2.vtyp == "en")
			{
				foreach (string itm in kvl2.en.items)
				{
					if (kvl1.val == itm)				//если значения совпадают
					{
						eq = "contained";				//результат положительный - содержится
						break;
					}
				}
			}

			return eq;								//возвращаем
		}

		public string frg2any(tkvl kvl1, tkvl kvl2)
		{
			string eq = "not equal";						//результат сравнения

			if (kvl2.vtyp == "str")
			{
				eq = "not contain";						//не содержит
				int ival;
				if (!int.TryParse(kvl2.val, out ival))
				{
					if (err == "")
					{
						err = "key:" + kvl2.key + " | ошибка преобразования строки в целое";
					}
					eq = "not contain";
					return eq;
				}

				//переданное атомарное значение попадает в свой диапазон
				if (ival >= kvl1.range.low && ival <= kvl1.range.top)
					eq = "contains";
			}

			if (kvl2.vtyp == "rg")
			{
				eq = "not cross";						//не пересекаются

				//диапазоны пересекаются
				if (!(kvl1.range.low > kvl2.range.top || kvl1.range.top < kvl2.range.low))
					eq = "cross";

				//переданный диапазон - подмножество своего
				if (kvl1.range.low < kvl2.range.low && kvl1.range.top > kvl2.range.top)
					eq = "contains";

				//свой диапазон - подмножество переданного
				if (kvl1.range.low > kvl2.range.low && kvl1.range.top < kvl2.range.top)
					eq = "contained";

				//диапазоны эквивалентны - равны границы
				if (kvl1.range.low == kvl2.range.low && kvl1.range.top == kvl2.range.top)
					eq = "equal";
			}

			if (kvl2.vtyp == "en")
			{
				eq = "not cross";										//не пересекаются

				bool eqi = true;
				foreach (string itm in kvl2.en.items)
				{
					int ival;
					if (!int.TryParse(itm, out ival))
					{
						continue;									//если элемент не целое берем следующий
					}
					if (ival >= kvl1.range.low && ival <= kvl1.range.top)	//сравниваем
					{
						eq = "cross";									//если хотя бы одно значение из перечисления попадает в
						//диапазон то пары пересекаются
					}
					eqi &= ival >= kvl1.range.low && ival <= kvl1.range.top;
				}
				if (eqi) eq = "contains";								//если все значения перечисления попадают в диапазон
				//то диапазон содержит перечисление
			}

			return eq;								//возвращаем
		}

		public string fen2any(tkvl kvl1, tkvl kvl2)
		{
			string eq = "not equal";						//результат сравнения

			if (kvl2.vtyp == "str")
			{
				eq = "not contain";						//не содержит

				foreach (string itm in kvl1.en.items)
				{
					if (kvl2.val == itm)				//если значения совпадают
					{
						eq = "contains";				//результат положительный - содержится
						break;
					}
				}
			}

			if (kvl2.vtyp == "rg")
			{
				eq = "not cross";										//не пересекаются

				bool eqi = true;
				foreach (string itm in kvl1.en.items)
				{
					int ival;
					if (!int.TryParse(itm, out ival))
					{
						continue;										//если элемент не целое берем следующий
					}
					if (ival >= kvl2.range.low && ival <= kvl2.range.top)	//сравниваем
					{
						eq = "cross";										//если хотя бы одно значение из перечисления попадает в
						//диапазон то пары пересекаются
					}
					eqi &= ival >= kvl2.range.low && ival <= kvl2.range.top;
				}
				if (eqi) eq = "contained";								//если все значения перечисления попадают в диапазон
				//то диапазон содержит перечисление
			}

			if (kvl2.vtyp == "en")
			{
				eq = "not cross";											//не пересекаются

				bool eqi = false;											//соответствие как минимум одного элемента
				bool[] kvl1eq = new bool[kvl1.en.items.Count];			//соответствия элементов своего перечисления
				bool[] kvl2eq = new bool[kvl2.en.items.Count];			//соответствия элементов переданного перечисления
				bool kvl1eqs = true;										//соответствие всех элементов своего перечисления
				bool kvl2eqs = true;										//соответствие элеметов переданного перечисления
				int i = 0;												//счетчик переданных элементов
				int j = 0;												//счетчик своих элементов
				foreach (string iitm in kvl2.en.items)						//перебираем переданные элементы
				{
					j = 0;
					foreach (string mitm in kvl1.en.items)				//перебираем свои
					{
						if (iitm == mitm)									//сравниваем 
						{
							kvl2eq[i] = true;								//совпадение переданного элемента
							kvl1eq[j] = true;								//совпадение своего элемента
							eqi = true;									//значения совпали

							eq = "cross";									//если хотя бы одно значение из перечисления попадает в
							//диапазон то пары пересекаются
						}
						j++;
					}
					kvl2eqs &= kvl2eq[i];										//соответствие элеметов переданного перечисления
					i++;
				}

				//анализируем совпадения элементов своего перечисления
				foreach (bool eqitm in kvl1eq)
				{
					kvl1eqs &= eqitm;
				}

				if (kvl1eqs)											//если все элементы своего совпали то свое содержится
					eq = "contained";										//в пеереданном
				if (kvl2eqs)												//если все элементы переданного совпали то переданное 
					eq = "contains";										//содержится в своем
				if (kvl2eqs && kvl1eqs && kvl1.en.items.Count == kvl2.en.items.Count)//если для каждого элемента обоих перечислений есть 
					eq = "equal";											//совпадения то перечисления эквивалентны

			}

			return eq;													//возвращаем
		}

	}

	#endregion tkveq

	#region teq

	public delegate int teq_feq_each_f(ref tkvl kvl1, ref tkvl kvl2, tkvl eqkvl);

	public class teq
	{
		string err = "";

		public tkvl kvl1;									//список 1
		public tkvl kvl2;									//список 2

		public tsep sep = new tsep();							//Объект разделителей

		public string eqstr = "not equal";					//строковое значение эквивалентности
		public tkvl kvl;									//собственный список

		public tkvl eqkvl;										//список совпавших пар...

		public teq(tkvl kvl1, tkvl kvl2)
		{
			this.kvl1 = kvl1;
			this.kvl2 = kvl2;
		}

		public teq(tkvl kvl)
		{
			this.kvl1 = kvl;
		}

		public teq feq()
		{
			//string eqstr="not equal";
			//tdeb.fdeb("feq_and_   kvl1 "+kvl1.ToString()+" kvl2 "+kvl2.ToString());
			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.kv)					//если оба элемента пары key:val
			{
				eqstr = fcompare(kvl1, kvl2);								//сравниваем как два key:val
				//MessageBox.Show("\r\n\r\n\r\n\r\n kvl1 \r\n"+kvl1.ToString()+
				//	"\r\nkvl2 \r\n"+kvl2.ToString()+
				//	"\r\neq "+eqstr+
				//	"\r\nrel1 "+kvl1.rel.ToString()+
				//	"\r\nrel2 "+kvl2.rel.ToString());
			}

			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.and)					//если сравниваемые - И-группы
			{
				bool eqand = true;
				foreach (tkvl kv2 in kvl2.list)
				{
					bool eqor = false;
					foreach (tkvl kv1 in kvl1.list)
					{
						string eqs = new teq(kv1, kv2).feq().eqstr;
						tdeb.fdeb("feq", "feq_and_   kv1 " + kv1.fkeyval() + " kv2 " + kv2.fkeyval() + eqs);
						eqor |= ((eqs == "equal" || eqs.Contains("contain")) && !eqs.Contains("not"));
					}
					eqand &= eqor;
					//Оптимизация сравнеий если получили eqand false то дальше можно не сравнивать...
					if (!eqand) { break; }
				}

				if (eqand)
				{
					//tdeb.fdeb("feq_and_   eqand "+eqand.ToString());
					eqstr = "contains";
				}
			}

			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.or)					//если сравнениваемые - ИЛИ-группы
			{
				tdeb.fdeb("feq", "feq_or_   kv1 " + kvl1.list.Count.ToString());
				bool eqor1 = false;
				foreach (tkvl kv2 in kvl2.list)							//каждую или группу первого списка
				{
					bool eqor2 = false;
					foreach (tkvl kv1 in kvl1.list)						//с каждой или группой второго списка
					{
						string eqs = new teq(kv1, kv2).feq().eqstr;
						tdeb.fdeb("feq", "feq_or_   kv1 " + kv1.fkeyval() + " kv2 " + kv2.fkeyval() + eqs);
						eqor2 |= (eqs == "equal" || eqs == "contains");
						///MessageBox.Show("\r\n\r\n\r\n\r\n kvl1 \r\n"+kv1.ToString()+
						//	"\r\nkvl2 \r\n"+kv2.ToString()+
						//	"\r\neq "+eqs+
						//	"\r\nrel1 "+kv1.rel.ToString()+
						//	"\r\nrel2 "+kv2.rel.ToString());
					}
					eqor1 |= eqor2;
					//Оптимизация сравнеий если получили eqor true то дальше можно не сравнивать...
					if (eqor1) { break; }
				}
				if (eqor1)
				{

					//tdeb.fdeb("feq_and_   eqor1 "+eqor1.ToString());
					eqstr = "contains";
				}

			}

			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.kvl)					//если сравнениваемые - связанные ИЛИ-группы
			{
				bool eq = false;											//логическая эквивалентность списков

				eqstr = new teq(kvl1.list[1], kvl2.list[1]).feq().eqstr;	//строка эквивалентности списков
				eq = (eqstr == "equal" || eqstr == "contains");					//эквивалентно если первый аргумент
				//равен второму либо содержит его либо содержится в нем
				tdeb.fdeb("feq", "feq_*_ " + kvl1.list[1].rel + " kv2 " + kvl2.list[1].rel + eqstr);

				if (eq)													//если логически эевивалентны
				{
					kvl = new tkvl(sep.kvl);								//создаем список уровня relat

					//teq eqmix=new teq(kvl1.list[0], kvl2.list[0]).fmix();
					//tdeb.fdeb("*****mix   "+eqmix.kvl.ToString());
					//Запоминаем исходные списки			
					kvl.fadd(kvl1);							//0
					kvl.fadd(kvl2);							//1
					//объедененный список параметров		//2
					//все параметры входящие во второй список со значениями из первого если значение во втором пустое
					kvl.fadd(new teq(kvl1.list[0], kvl2.list[0]).fmix().kvl);	//запоминаем первые элементы сравниваемых списков
					//kvl.fadd(kvl2.list[0]);									//как его элементы

					//объедененный список параметров		//3
					//все параметры входящие во второй и первый список доминирующие значения из второго списка
					//содержит только пары из kvl1(модель) и значения из kvl2(конфигурация)
					//для тех пар значения в kvl2 у которых ==""
					kvl.fadd(new teq(kvl1.list[0], kvl2.list[0]).fmix().kvl);	//запоминаем первые элементы сравниваемых списков
					//kvl.fadd(kvl2.list[0]);									//как его элементы

					//список для рассчета формул			//4
					//содержит все значения из списка модели(1сп) и материала(2сп) значения из модели приоритетны
					//содержит только пары из kvl2(конфигурация) и значения из kvl1(модель) 
					//для тех пар значения в kvl1 у которых ==""
					kvl.fadd(new teq(kvl2.list[0], kvl1.list[0]).fjoin().kvl);	//запоминаем первые элементы сравниваемых списков

					//MessageBox.Show("kvl2.0\r\n"+kvl2.list[0].ToString());

					//MessageBox.Show("kvl1.0\r\n"+kvl1.list[0].ToString());

					//MessageBox.Show(new teq(kvl2.list[0], kvl1.list[0]).fjoin().kvl.fstr());
					//MessageBox.Show(new teq(kvl1.list[0], kvl2.list[0]).fjoin().kvl.fstr());

					//MessageBox.Show("join\r\n"+(new teq(kvl2.list[0], kvl1.list[0])).fjoin().kvl.ToString());

				}

			}

			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.gr)					//если сравнениваемые - перечисление связанных групп
			{
				kvl = new tkvl(sep.gr);									//создаем свой список
				bool eq = false;											//логическая эквивалентность списков
				foreach (tkvl kv2 in kvl2.list)							//каждую связанную группу
				{
					foreach (tkvl kv1 in kvl1.list)						//с каждой связанной группой
					{

						teq eqkvl = new teq(kv1, kv2).feq();				//результат сравнения
						eqstr = eqkvl.eqstr;								//строка результата сравнения
						eq = (eqstr == "equal" || eqstr == "contains");			//логический результат сравнения
						tdeb.fdeb("feq", "feq_%_   kv1 " + kv1.rel + " kv2 " + kv2.rel + eqstr);
						if (eq)											//если логически эквивалентны
						{
							kvl.fadd(eqkvl.kvl);						//добавляем связанную группу совпавших групп
						}												//в свой список	
					}
				}
			}

			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.cgr)					//если сравнениваемые - перечисление связанных групп
			{
				kvl = new tkvl(sep.cgr);									//создаем свой список
				bool eq = false;											//логическая эквивалентность списков
				foreach (tkvl kv2 in kvl2.list)							//каждую связанную группу
				{
					foreach (tkvl kv1 in kvl1.list)						//с каждой связанной группой
					{

						teq eqkvl = new teq(kv1, kv2).feq();				//результат сравнения
						eqstr = eqkvl.eqstr;								//строка результата сравнения
						eq = (eqstr == "equal" || eqstr == "contains");			//логический результат сравнения
						tdeb.fdeb("feq", "feq_#_   kv1 " + kv1.rel + " kv2 " + kv2.rel + eqstr);
						if (eq)											//если логически эквивалентны
						{
							kvl.fadd(eqkvl.kvl);						//добавляем связанную группу совпавших групп
						}												//в свой список	
					}
				}
			}

			//tdeb.fdeb("feq_and_   eqstr "+eqstr+kvl1.rel.ToString());
			return this;
		}



		public teq feq_each(teq_feq_each_f each_f)
		{


			//если индекс первого списка выше индекса второго то перебираем элементы первого списка
			//и для каждого элемента вызываем новый feq_each, то есть мы постепенно понижаем уровень
			//первого списка пока не уровняем его с уровнем второго списка
			//
			if (tsep.f_1_up_2(kvl1.rel, kvl2.rel))
			{
				foreach (tkvl kv1 in kvl1.list)
				{
					teq eq = new teq(kv1, kvl2).feq_each(each_f);
				}
			}

			//string eqstr="not equal";
			//tdeb.fdeb("feq_and_   kvl1 "+kvl1.ToString()+" kvl2 "+kvl2.ToString());
			//if (kvl1.rel==kvl2.rel&&kvl1.rel==sep.kv)					//если оба элемента пары key:val
			//{
			//	eqstr=fcompare(kvl1, kvl2);								//сравниваем как два key:val
			//}


			//если же мы получили на входе два множества одинакового уровня
			//то переходим к их сравнения (так как мы можем сравнивать
			//множества одного уровня)
			//и если их сравнениеговорит о том что они эквивалентны
			//мы вызываем each_f и передаем ей совпавшие kvl и вид эквивалентности

			if (kvl1.rel == kvl2.rel)					//если сравниваемые - И-группы
			{
				bool eqand = true;
				foreach (tkvl kv2 in kvl2.list)
				{
					bool eqor = false;
					foreach (tkvl kv1 in kvl1.list)
					{

						string eqs = new teq(kv1, kv2).feq().eqstr;
						//MessageBox.Show(eqs);
						//MessageBox.Show("\r\n\r\n\r\n\r\n kvl1 \r\n"+kvl1.ToString()+
						//				"\r\nkvl2 \r\n"+kvl2.ToString()+
						//				"\r\neq "+eqs);
						//MessageBox.Show("\r\n\r\n\r\n\r\n kvl1 \r\n"+kv1.ToString()+
						//	"\r\nkvl2 \r\n"+kv2.ToString()+
						//	"\r\neq "+eqs+
						//	"\r\nrel1 "+kv1.rel.ToString()+
						//	"\r\nrel2 "+kv2.rel.ToString());
						//tdeb.fdeb("each","feq_and_   kv1 "+eqs);
						//tdeb.fdeb("feq","feq_and_   kv1 "+kv1.fkeyval()+" kv2 "+kv2.fkeyval()+eqs);
						eqor |= ((eqs == "equal" || eqs.Contains("contain")) && !eqs.Contains("not"));
					}
					eqand &= eqor;
					//Оптимизация сравнеий если получили eqand false то дальше можно не сравнивать...
					if (!eqand) { break; }
				}

				if (eqand)
				{
					//tdeb.fdeb("feq_and_   eqand "+eqand.ToString());
					each_f(ref kvl1, ref kvl2, kvl1);
					//eqstr="contains";
				}
			}

			return null;

			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.or)					//если сравнениваемые - ИЛИ-группы
			{
				tdeb.fdeb("feq", "feq_or_   kv1 " + kvl1.list.Count.ToString());
				bool eqor1 = false;
				foreach (tkvl kv2 in kvl2.list)							//каждую или группу первого списка
				{
					bool eqor2 = false;
					foreach (tkvl kv1 in kvl1.list)						//с каждой или группой второго списка
					{
						string eqs = new teq(kv1, kv2).feq().eqstr;
						tdeb.fdeb("feq", "feq_or_   kv1 " + kv1.fkeyval() + " kv2 " + kv2.fkeyval() + eqs);
						eqor2 |= (eqs == "equal" || eqs == "contains");
					}
					eqor1 |= eqor2;
					//Оптимизация сравнеий если получили eqor true то дальше можно не сравнивать...
					if (eqor1) { break; }
				}
				if (eqor1)
				{
					//tdeb.fdeb("feq_and_   eqor1 "+eqor1.ToString());
					eqstr = "contains";
				}
			}

			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.kvl)					//если сравнениваемые - связанные ИЛИ-группы
			{
				bool eq = false;											//логическая эквивалентность списков

				eqstr = new teq(kvl1.list[1], kvl2.list[1]).feq().eqstr;	//строка эквивалентности списков
				eq = (eqstr == "equal" || eqstr == "contains");					//эквивалентно если первый аргумент
				//равен второму либо содержит его либо содержится в нем
				tdeb.fdeb("feq", "feq_*_ " + kvl1.list[1].rel + " kv2 " + kvl2.list[1].rel + eqstr);

				if (eq)													//если логически эевивалентны
				{
					kvl = new tkvl(sep.kvl);								//создаем список уровня relat

					//teq eqmix=new teq(kvl1.list[0], kvl2.list[0]).fmix();
					//tdeb.fdeb("*****mix   "+eqmix.kvl.ToString());
					//Запоминаем исходные списки			
					kvl.fadd(kvl1);							//0
					kvl.fadd(kvl2);							//1
					//объедененный список параметров		//2
					//все параметры входящие во второй список со значениями из первого если значение во втором пустое
					kvl.fadd(new teq(kvl1.list[0], kvl2.list[0]).fmix().kvl);	//запоминаем первые элементы сравниваемых списков
					//kvl.fadd(kvl2.list[0]);									//как его элементы

					//объедененный список параметров		//3
					//все параметры входящие во второй и первый список доминирующие значения из второго списка
					kvl.fadd(new teq(kvl1.list[0], kvl2.list[0]).fjoin().kvl);	//запоминаем первые элементы сравниваемых списков
					//kvl.fadd(kvl2.list[0]);									//как его элементы

					//список для рассчета формул			//4
					//содержит все значения из списка модели(1сп) и материала(2сп) значения из модели приоритетны
					kvl.fadd(new teq(kvl2.list[0], kvl1.list[0]).fjoin().kvl);	//запоминаем первые элементы сравниваемых списков



				}

			}

			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.gr)					//если сравнениваемые - перечисление связанных групп
			{
				kvl = new tkvl(sep.gr);									//создаем свой список
				bool eq = false;											//логическая эквивалентность списков
				foreach (tkvl kv2 in kvl2.list)							//каждую связанную группу
				{
					foreach (tkvl kv1 in kvl1.list)						//с каждой связанной группой
					{

						teq eqkvl = new teq(kv1, kv2).feq();				//результат сравнения
						eqstr = eqkvl.eqstr;								//строка результата сравнения
						eq = (eqstr == "equal" || eqstr == "contains");			//логический результат сравнения
						tdeb.fdeb("feq", "feq_%_   kv1 " + kv1.rel + " kv2 " + kv2.rel + eqstr);
						if (eq)											//если логически эквивалентны
						{
							kvl.fadd(eqkvl.kvl);						//добавляем связанную группу совпавших групп
						}												//в свой список	
					}
				}
			}

			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.cgr)					//если сравнениваемые - перечисление связанных групп
			{
				kvl = new tkvl(sep.cgr);									//создаем свой список
				bool eq = false;											//логическая эквивалентность списков
				foreach (tkvl kv2 in kvl2.list)							//каждую связанную группу
				{
					foreach (tkvl kv1 in kvl1.list)						//с каждой связанной группой
					{

						teq eqkvl = new teq(kv1, kv2).feq();				//результат сравнения
						eqstr = eqkvl.eqstr;								//строка результата сравнения
						eq = (eqstr == "equal" || eqstr == "contains");			//логический результат сравнения
						tdeb.fdeb("feq", "feq_#_   kv1 " + kv1.rel + " kv2 " + kv2.rel + eqstr);
						if (eq)											//если логически эквивалентны
						{
							kvl.fadd(eqkvl.kvl);						//добавляем связанную группу совпавших групп
						}												//в свой список	
					}
				}
			}

			//tdeb.fdeb("feq_and_   eqstr "+eqstr+kvl1.rel.ToString());

			//each_f(kvl1, kvl2, 

			return this;
		}


		public teq fmix()
		{
			//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n"+ kvl1.rel);

			if (kvl1.rel == sep.gr)					//если сравнениваемые - перечисление связанных групп
			{
				kvl = new tkvl();
				foreach (tkvl kv1 in kvl1.list)							//каждую связанную группу
				{
					teq eq = new teq(kv1).fmix();							//результат сравнения
					//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\naksdjfiwjef"+eq.kvl.list[0].ToString());
					tdeb.fdeb("feq", "feq_mix_%_   kv1" + eq.kvl.ToString());
					kvl.fadd(eq.kvl);
					//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n"+kvl.ToString());
				}
				return this;
			}

			if (kvl1.rel == sep.kvl)					//если сравнениваемые - связанные ИЛИ-группы
			{
				kvl = new tkvl('^');

				teq eq = new teq(kvl1.list[0], kvl1.list[1]).fmix();		//строка эквивалентности списков
				kvl.fadd(eq.kvl);
				//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n1111"+kvl.ToString());
				return this;
			}

			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.or)					//ИЛИ-группы
			{

				kvl = new tkvl('|');
				teq eq = new teq(kvl1.list[0], kvl2.list[0]).fmix();

				kvl.fadd(eq.kvl);
				//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n2222"+eq.kvl.ToString());
				//tdeb.fdeb("feq_mix_and_   kv1 "+kvl1.list[0].ToString()+" kv2 "+kvl2.list[0].ToString());
			}

			//в итоговый массив попадают все пары из kvl2 и только те значения из kvl1
			//которые в kvl2 пустые
			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.and)					//И-группы
			{
				kvl = new tkvl('&');
				foreach (tkvl kv2 in kvl2.list)							//значения из второго списка приоритетны
				{														//поэтому сначала переносим все значения
					kvl.fadd(new tkvl(kv2.fkey(), kv2.fval()));			//из второго масива а потом отсутствующие из первого
				}
				/*
				foreach(tkvl kv1 in kvl1.list)
				{
					bool eqor=false;
					foreach(tkvl kv2 in kvl2.list)
					{
						eqor|=kv2.fkey()==kv1.fkey();
					}
					if (!eqor) kvl.fadd(kv1);
				}
				*/
				//переносим из kvl1 в итоговый массив значения по пустым ключам
				int i = 0;
				while (i < kvl.list.Count)
				{
					foreach (tkvl kv1 in kvl1.list)
					{
						if (kvl.list[i].fkey() == kv1.fkey() && kvl.list[i].fval() == "")
						{
							kvl.list[i].fval(kv1.fval());
						}
					}
					i++;
				}
				//eqand&=eqor;


			}

			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.kv)										//если оба элемента пары key:val
			{

				tdeb.fdeb("feq", "teq_mix_kv_   " + kvl1.ToString() + kvl2.ToString());
				kvl = new tkvl(':');
				if (kvl1.key == kvl2.key)
				{
					kvl.key = kvl1.key;									//запоминаем ключ


					if (kvl1.fval() != "") kvl.fval(kvl2.fval());			//если второе значение задано берем его
					if (kvl1.fval() != "") kvl.fval(kvl1.fval());			//если первое выражение заданно то берем его
					//таким образом если заданы оба то возьмется первое
				}
			}



			//tdeb.fdeb("feq_and_   eqstr "+eqstr+kvl1.rel.ToString());

			//MessageBox.Show("mix\r\nkvl1 "+kvl1.rel+"\r\n"+kvl1.ToString()+
			//	"\r\nkvl2 "+kvl2.rel+"\r\n"+kvl2.ToString()+
			//	"\r\nkvl\r\n"+kvl.fstr());

			return this;
		}

		public teq fjoin()
		{
			//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n"+ kvl1.rel);
			//MessageBox.Show("kvl1 "+kvl1.rel+"\r\n"+kvl1.ToString()+
			//	"\r\nkvl2"+kvl2.rel+"\r\n"+kvl2.ToString());

			if (kvl1.rel == sep.gr)					//если сравнениваемые - перечисление связанных групп
			{
				kvl = new tkvl();
				foreach (tkvl kv1 in kvl1.list)							//каждую связанную группу
				{
					teq eq = new teq(kv1).fmix();							//результат сравнения
					//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\naksdjfiwjef"+eq.kvl.list[0].ToString());
					tdeb.fdeb("feq", "feq_mix_%_   kv1" + eq.kvl.ToString());
					kvl.fadd(eq.kvl);
					//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n"+kvl.ToString());
				}
				return this;
			}

			if (kvl1.rel == sep.kvl)					//если сравнениваемые - связанные ИЛИ-группы
			{
				kvl = new tkvl('^');

				teq eq = new teq(kvl1.list[0], kvl1.list[1]).fjoin();		//строка эквивалентности списков
				kvl.fadd(eq.kvl);
				//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n1111"+kvl.ToString());
				return this;
			}

			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.or)					//ИЛИ-группы
			{

				kvl = new tkvl('|');
				teq eq = new teq(kvl1.list[0], kvl2.list[0]).fjoin();

				kvl.fadd(eq.kvl);
				//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n2222\r\n"+eq.kvl.ToString());
				//tdeb.fdeb("feq_mix_and_   kv1 "+kvl1.list[0].ToString()+" kv2 "+kvl2.list[0].ToString());
			}

			//берет все пары из kvl2
			//затем заменяет
			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.and)					//И-группы
			{
				kvl = new tkvl('&');
				foreach (tkvl kv2 in kvl2.list)							//значения из второго списка приоритетны
				{														//поэтому сначала переносим все значения
					//kvl.fadd(kv2);										//из второго масива а потом отсутствующие из первого
					kvl.fadd(new tkvl(kv2.fkey(), kv2.fval()));
				}
				//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\nsep.and kvl2\r\n"+kvl2.ToString());
				//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\nsep.and kvl1\r\n"+kvl1.ToString());
				//MessageBox.Show("kvl1\r\n"+kvl1.ToString()+
				//					"\r\nkvl2\r\n"+kvl2.ToString());
				foreach (tkvl kv1 in kvl1.list)
				{
					bool eqor = false;
					foreach (tkvl kv2 in kvl2.list)
					{
						eqor |= kv2.fkey() == kv1.fkey();
						//MessageBox.Show("kvl1\r\n"+kvl1.ToString()+
						//				"\r\nkvl2\r\n"+kvl2.ToString()+
						//				"\r\nkv1\r\n"+kv1.ToString()+
						//				"\r\nkv1.key\r\n"+kv1.fkey()+
						//				"\r\nkv1.val\r\n"+kv1.fval()+
						//				"\r\nkv2\r\n"+kv2.ToString());
					}
					if (!eqor) kvl.fadd(kv1);
				}
				/*
				int i=0;
				while(i<kvl.list.Count)
				{
					foreach(tkvl kv1 in kvl1.list)
					{
						if (kvl.list[i].fkey()==kv1.fkey()&&kvl.list[i].fval()=="")
						{
							kvl.list[i].fval(kvl.fval());
						}
					}
					i++;
				}
				*/
				//eqand&=eqor;


			}

			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.kv)										//если оба элемента пары key:val
			{

				tdeb.fdeb("feq", "teq_mix_kv_   " + kvl1.ToString() + kvl2.ToString());
				kvl = new tkvl(':');
				if (kvl1.key == kvl2.key)
				{
					kvl.key = kvl1.key;									//запоминаем ключ


					if (kvl1.fval() != "") kvl.fval(kvl2.fval());			//если второе значение задано берем его
					if (kvl1.fval() != "") kvl.fval(kvl1.fval());			//если первое выражение заданно то берем его
					//таким образом если заданы оба то возьмется первое
				}
			}



			//tdeb.fdeb("feq_and_   eqstr "+eqstr+kvl1.rel.ToString());

			//MessageBox.Show("join\r\nkvl1 "+kvl1.rel+"\r\n"+kvl1.ToString()+
			//				"\r\nkvl2 "+kvl2.rel+"\r\n"+kvl2.ToString()+
			//				"\r\nkvl\r\n"+kvl.fstr());

			return this;
		}

		public teq fcontains()
		{
			//string eqstr="not equal";

			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.kv)					//если оба элемента пары key:val
			{
				//MessageBox.Show("***teq_fcontains_   kv"+eqstr);
				eqstr = fcompare(kvl1, kvl2);								//сравниваем как два key:val
				//tdeb.fdeb("**********************teq_fcontains_   kv"+eqstr);
			}
			//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n"+kvl1.rel+kvl2.rel);
			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.and)					//если сравниваемые - И-группы
			{

				bool eqand = true;
				foreach (tkvl kv2 in kvl2.list)
				{

					bool eqor = false;
					foreach (tkvl kv1 in kvl1.list)
					{
						string eqs = new teq(kv1, kv2).fcontains().eqstr;
						//tdeb.fdeb("feq_and_   kv1 "+kv1.fkeyval()+" kv2 "+kv2.fkeyval()+eqs);
						eqor |= (eqs == "equal" || eqs == "eqkey" || eqs.Contains("contain"));
					}
					eqand &= eqor;
					tdeb.fdeb("feq", "feq_and_   eqand " + eqor.ToString() + "\r\n" + kvl1.ToString() + "\r\n" + kvl2.ToString());
				}
				tdeb.fdeb("feq", "feq_and_   eqand " + eqand.ToString());
				if (eqand)
				{
					tdeb.fdeb("feq", "feq_and_   eqand " + eqand.ToString());
					eqstr = "contains";
				}
			}

			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.or)					//если сравнениваемые - ИЛИ-группы
			{
				tdeb.fdeb("feq", "feq_or_   kv1 " + kvl1.list.Count.ToString());
				bool eqor1 = false;
				foreach (tkvl kv2 in kvl2.list)							//каждую или группу первого списка
				{
					bool eqor2 = false;
					foreach (tkvl kv1 in kvl1.list)						//с каждой или группой второго списка
					{
						string eqs = new teq(kv1, kv2).fcontains().eqstr;
						tdeb.fdeb("feq", "feq_or_   kv1 " + kv1.fkeyval() + " kv2 " + kv2.fkeyval() + eqs);
						eqor2 |= (eqs == "equal" || eqs == "contains");
					}
					eqor1 |= eqor2;
				}
				if (eqor1)
				{
					//tdeb.fdeb("feq_and_   eqor1 "+eqor1.ToString());
					eqstr = "contains";
				}
			}

			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.kvl)					//если сравнениваемые - связанные ИЛИ-группы
			{
				bool eq = false;											//логическая эквивалентность списков

				eqstr = new teq(kvl1.list[1], kvl2.list[1]).fcontains().eqstr;	//строка эквивалентности списков
				eq = (eqstr == "equal" || eqstr == "contains");					//эквивалентно если первый аргумент
				//равен второму либо содержит его
				tdeb.fdeb("feq", "feq_*_ " + kvl1.list[1].rel + " kv2 " + kvl2.list[1].rel + eqstr);

				if (eq)													//если логически эевивалентны
				{
					kvl = new tkvl('^');									//создаем список уровня relat

					//teq eqmix=new teq(kvl1.list[0], kvl2.list[0]).fmix();
					//tdeb.fdeb("*****mix   "+eqmix.kvl.ToString());
					//объедененный список параметров
					kvl.fadd(new teq(kvl1.list[0], kvl2.list[0]).fmix().kvl);//запоминаем первые элементы сравниваемых списков
					//kvl.fadd(kvl2.list[0]);								//как его элементы
				}

			}

			if (kvl1.rel == kvl2.rel && kvl1.rel == sep.gr)					//если сравнениваемые - перечисление связанных групп
			{
				kvl = new tkvl();											//создаем свой список
				bool eq = false;											//логическая эквивалентность списков
				foreach (tkvl kv2 in kvl2.list)							//каждую связанную группу
				{
					foreach (tkvl kv1 in kvl1.list)						//с каждой связанной группой
					{

						teq eqkvl = new teq(kv1, kv2).fcontains();				//результат сравнения
						eqstr = eqkvl.eqstr;								//строка результата сравнения
						eq = (eqstr == "equal" || eqstr == "contains");			//логический результат сравнения
						tdeb.fdeb("feq", "feq_%_   kv1 " + kv1.rel + " kv2 " + kv2.rel + eqstr);
						if (eq)											//если логически эквивалентны
						{
							kvl.fadd(eqkvl.kvl);						//добавляем связанную группу совпавших групп
						}												//в свой список	
					}
				}
			}

			//tdeb.fdeb("feq_and_   eqstr "+eqstr+kvl1.rel.ToString());
			return this;
		}

		public string fcompare(tkvl kvl1, tkvl kvl2)			//сравнение пар - результат вид соответствия - строка
		{
			//bool eq=false;									//результат сравнения
			string eqstr = "not equal";							//полагаем что не эквивалентны

			if (kvl1.key != kvl2.key)								//если не равны ключи пар то нет смысла сравнивать значения
			{
				return eqstr;										//возвращаем отричательный результат - НЕ ЭКВИВАЛЕНТНЫ!!!
			}
			//tdeb.fdeb("__teq___fcompare___"+kvl1.vtyp+kvl2.vtyp+((kvl1.fval()==""||kvl2.fval()=="")&&kvl1.fkey()==kvl2.fkey()).ToString());
			if ((kvl1.fval() == "" || kvl2.fval() == "") && kvl1.fkey() == kvl2.fkey())
			{
				tdeb.fdeb("feq", "__teq___fcompare___" + kvl1.vtyp + kvl2.vtyp);
				eqstr = "eqkey";
				return eqstr;
			}

			//атомарное
			if (kvl1.vtyp == "str")								//если свое атомарное
			{
				eqstr = fstr2any(kvl1, kvl2);								//сравниваем свое атомарное с переданным
			}

			//диапазоны
			if (kvl1.vtyp == "rg")									//если свое значение диапазон
			{
				eqstr = frg2any(kvl1, kvl2);									//проверяем попадание значения в диапазон
			}

			//перечисления
			if (kvl1.vtyp == "en")									//если свое значение перечисление
			{
				eqstr = fen2any(kvl1, kvl2);									//проверяем попадание значения в диапазон
			}

			return eqstr;
		}

		//сравнители

		public string fstr2any(tkvl kvl1, tkvl kvl2)
		{
			string eq = "not equal";				//результат сравнения

			if (kvl2.vtyp == "str")
			{
				if (kvl2.val.Contains("~"))
				{
					string[] kvl2i = kvl2.val.Split('~');
					bool eqq = true;
					foreach (string kvl2ii in kvl2i)
					{
						if (kvl2ii != "")
						{
							eqq &= kvl2ii != "" && kvl1.val.ToLower().Contains(kvl2ii);
							tdeb.fdeb("огран отл", "kvl2ii_ " + kvl2ii + "   kvl1.val " + kvl1.val + "   eq " + eqq.ToString());
						}
					}
					if (eqq) eq = "equal";
				}
				if (kvl2.val.StartsWith("!"))
				{
					string notval = kvl2.val.Substring(1, kvl2.val.Length - 1);
					if (notval != kvl1.val)
					{
						eq = "equal";
					}
				}
				if (kvl1.val == kvl2.val)			//если значения совпадают
				{
					eq = "equal";					//результат положительный - ЭКВИВАЛЕНТНЫ
				}
			}

			if (kvl2.vtyp == "rg")
			{
				double ival;
				if (!double.TryParse(kvl1.val, out ival))
				{
					if (err == "")
					{
						err = "key:" + kvl1.key + " | ошибка преобразования строки в целое";
					}
					eq = "not equal";
					return eq;
				}

				//свое атомарное значение попадает в переданный диапазон
				if (ival >= kvl2.range.low && ival <= kvl2.range.top)
					eq = "contained";
			}

			if (kvl2.vtyp == "en")
			{
				foreach (string itm in kvl2.en.items)
				{
					if (kvl1.val == itm)				//если значения совпадают
					{
						eq = "contained";				//результат положительный - содержится
						break;
					}
				}
			}

			return eq;								//возвращаем
		}

		public string frg2any(tkvl kvl1, tkvl kvl2)
		{
			string eq = "not equal";						//результат сравнения

			if (kvl2.vtyp == "str")
			{
				eq = "not contain";						//не содержит
				int ival;
				if (!int.TryParse(kvl2.val, out ival))
				{
					if (err == "")
					{
						err = "key:" + kvl2.key + " | ошибка преобразования строки в целое";
					}
					eq = "not contain";
					return eq;
				}

				//переданное атомарное значение попадает в свой диапазон
				if (ival >= kvl1.range.low && ival <= kvl1.range.top)
					eq = "contains";
			}

			if (kvl2.vtyp == "rg")
			{
				eq = "not cross";						//не пересекаются

				//диапазоны пересекаются
				if (!(kvl1.range.low > kvl2.range.top || kvl1.range.top < kvl2.range.low))
					eq = "cross";

				//переданный диапазон - подмножество своего
				if (kvl1.range.low < kvl2.range.low && kvl1.range.top > kvl2.range.top)
					eq = "contains";

				//свой диапазон - подмножество переданного
				if (kvl1.range.low > kvl2.range.low && kvl1.range.top < kvl2.range.top)
					eq = "contained";

				//диапазоны эквивалентны - равны границы
				if (kvl1.range.low == kvl2.range.low && kvl1.range.top == kvl2.range.top)
					eq = "equal";
			}

			if (kvl2.vtyp == "en")
			{
				eq = "not cross";										//не пересекаются

				bool eqi = true;
				foreach (string itm in kvl2.en.items)
				{
					int ival;
					if (!int.TryParse(itm, out ival))
					{
						continue;									//если элемент не целое берем следующий
					}
					if (ival >= kvl1.range.low && ival <= kvl1.range.top)	//сравниваем
					{
						eq = "cross";									//если хотя бы одно значение из перечисления попадает в
						//диапазон то пары пересекаются
					}
					eqi &= ival >= kvl1.range.low && ival <= kvl1.range.top;
				}
				if (eqi) eq = "contains";								//если все значения перечисления попадают в диапазон
				//то диапазон содержит перечисление
			}

			return eq;								//возвращаем
		}

		public string fen2any(tkvl kvl1, tkvl kvl2)
		{
			string eq = "not equal";						//результат сравнения

			if (kvl2.vtyp == "str")
			{
				eq = "not contain";						//не содержит

				foreach (string itm in kvl1.en.items)
				{
					if (kvl2.val == itm)				//если значения совпадают
					{
						eq = "contains";				//результат положительный - содержится
						break;
					}
				}
			}

			if (kvl2.vtyp == "rg")
			{
				eq = "not cross";										//не пересекаются

				bool eqi = true;
				foreach (string itm in kvl1.en.items)
				{
					int ival;
					if (!int.TryParse(itm, out ival))
					{
						continue;										//если элемент не целое берем следующий
					}
					if (ival >= kvl2.range.low && ival <= kvl2.range.top)	//сравниваем
					{
						eq = "cross";										//если хотя бы одно значение из перечисления попадает в
						//диапазон то пары пересекаются
					}
					eqi &= ival >= kvl2.range.low && ival <= kvl2.range.top;
				}
				if (eqi) eq = "contained";								//если все значения перечисления попадают в диапазон
				//то диапазон содержит перечисление
			}

			if (kvl2.vtyp == "en")
			{
				eq = "not cross";											//не пересекаются

				bool eqi = false;											//соответствие как минимум одного элемента
				bool[] kvl1eq = new bool[kvl1.en.items.Count];			//соответствия элементов своего перечисления
				bool[] kvl2eq = new bool[kvl2.en.items.Count];			//соответствия элементов переданного перечисления
				bool kvl1eqs = true;										//соответствие всех элементов своего перечисления
				bool kvl2eqs = true;										//соответствие элеметов переданного перечисления
				int i = 0;												//счетчик переданных элементов
				int j = 0;												//счетчик своих элементов
				foreach (string iitm in kvl2.en.items)						//перебираем переданные элементы
				{
					j = 0;
					foreach (string mitm in kvl1.en.items)				//перебираем свои
					{
						if (iitm == mitm)									//сравниваем 
						{
							kvl2eq[i] = true;								//совпадение переданного элемента
							kvl1eq[j] = true;								//совпадение своего элемента
							eqi = true;									//значения совпали

							eq = "cross";									//если хотя бы одно значение из перечисления попадает в
							//диапазон то пары пересекаются
						}
						j++;
					}
					kvl2eqs &= kvl2eq[i];										//соответствие элеметов переданного перечисления
					i++;
				}

				//анализируем совпадения элементов своего перечисления
				foreach (bool eqitm in kvl1eq)
				{
					kvl1eqs &= eqitm;
				}

				if (kvl1eqs)											//если все элементы своего совпали то свое содержится
					eq = "contained";										//в пеереданном
				if (kvl2eqs)												//если все элементы переданного совпали то переданное 
					eq = "contains";										//содержится в своем
				if (kvl2eqs && kvl1eqs && kvl1.en.items.Count == kvl2.en.items.Count)//если для каждого элемента обоих перечислений есть 
					eq = "equal";											//совпадения то перечисления эквивалентны

			}

			return eq;													//возвращаем
		}

		public bool feqstr(string val1, string val2)
		{
			if (val2.Contains("~"))
			{
				string[] kvl2i = val2.Split('~');
				bool eqq = true;
				foreach (string kvl2ii in kvl2i)
				{
					if (kvl2ii != "")
					{
						eqq &= kvl2ii != "" && val1.ToLower().Contains(kvl2ii);
						tdeb.fdeb("огран отл", "kvl2ii_ " + kvl2ii + "   kvl1.val " + val1 + "   eq " + eqq.ToString());
					}
				}
				//if (eqq) eq="equal";
				return eqq;
			}
			if (val2.StartsWith("!"))
			{
				string notval = val2.Substring(1, val2.Length - 1);
				if (notval != val1)
				{
					//eq="equal";
					return true;
				}
			}
			if (val1 == val2)			//если значения совпадают
			{
				//eq="equal";			//результат положительный - ЭКВИВАЛЕНТНЫ
				return true;
			}
			return false;
		}

	}

	#endregion teq

	#region tfsep

	public class tfsep
	{
		public List<string> fsep = new List<string>();

		public tfsep()
		{
			fsep.Add("+-");
			fsep.Add("*/");
		}
	}

	#endregion tfsep

	#region tf

	public class tf
	{
		public string sep = "+-*/?";

		public char mysep;

		public string fstr;
		public tkvl kvl;

		public List<string> itml = new List<string>();
		public List<double> itmresl = new List<double>();

		public List<string> itmtypl = new List<string>();

		public tf(string fstr, tkvl kvl)
		{
			this.fstr = fstr;
			this.kvl = kvl;
		}

		public string funhooks(string fstr_hooks)
		{
			//MessageBox.Show("unhooks|"+fstr_hooks+"|");
			if (fstr_hooks.StartsWith("(") && fstr_hooks.EndsWith(")"))
			{
				int pass = 0;
				int j = 0;
				int open_hook_poss = -1;
				foreach (char ch in fstr_hooks)
				{
					//char nch=fstr_hooks[j+1];

					if (ch == '(')
					{
						pass++;
						open_hook_poss = j;
					}
					if (ch == ')')
					{
						pass--;
					}
					j++;
				}
				if (pass == 0 && open_hook_poss == 0)
				{
					return funhooks(fstr_hooks.Substring(1, fstr_hooks.Length - 2));
				}

			}

			return fstr_hooks;
		}

		public string ff()
		{
			int err = 1;
			double calc = 0;
			tdeb.fdeb("tf", "___ff___fstr_" + fstr);
			//calc=Convert.ToDouble(fstr);
			if (double.TryParse(fstr, out calc))
			{
				return calc.ToString();
			}
			//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n key"+fstr+"|"+(kvl.fitmkey(fstr)==null).ToString());
			//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n list"+kvl.list[1].rel.ToString());
			if (kvl.fitmkey(fstr) != null)
			{
				return new tf(kvl.fitmkey(fstr).fval(), kvl).ff();
			}

			fstr = funhooks(fstr);
			//MessageBox.Show("___"+fstr);
			//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n"+itml[0].ToString()+itml[1].ToString()+itml[2].ToString());
			bool isexp = false;

			foreach (char sch in sep)
			{
				int pass = 0;
				foreach (char ch in fstr)
				{
					if (ch == '(')
					{
						pass++;
					}
					if (ch == ')')
					{
						pass--;
					}

					if (pass == 0 && ch == sch)
					{
						mysep = sch;
						fsplitch(sch);
						isexp = true;
						break;
					}
				}
				if (itml.Count > 0) break;
				//MessageBox.Show(itml.Count.ToString());
				/*
				if(fstr.Contains(sch.ToString()))
				{
				mysep=sch;
				fsplitch(sch);
				isexp=true;
				break;
				}
				*/
			}
			//если переданная строка не выражение и не ключ, то просто возвращаем ее
			if (!isexp)
			{
				return fstr;
			}


			//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n"+mysep.ToString()+itml[0].ToString()+"|"+itml[1].ToString()+"|"+itml[2].ToString());

			//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n"+mysep.ToString()+"|"+itml[0].ToString()+"|"+itml[1].ToString());


			//return 0;
			int i = 0;
			while (i < itml.Count)
			{
				//MessageBox.Show("itml.len "+itml[i]);
				if (itml[i] != mysep.ToString())
				{
					itml[i] = new tf(itml[i], kvl).ff().ToString();
					//itmresl.Add();
				}
				i++;
			}

			i = 0;
			/*
			while (i<itml.Count)
			{
				if (itml[i]!=mysep.ToString())
				{
					itml[i]=new tf(itm, kvl).ff().ToString();
					//itmresl.Add();
				}
			}
			*/
			//fcalc();


			//foreach(

			return fcalc().ToString();
		}

		public double fcalc()
		{
			int i = 0;
			double calc = 0;

			if (mysep == '+')
			{
				calc = 0;
			}
			if ((mysep == '*' || mysep == '/' || mysep == '?' || mysep == '-') && itml.Count > 2)
			{
				calc = Convert.ToDouble(itml[0]);
				i = 2;
			}
			if ((mysep == '*' || mysep == '/' || mysep == '?' || mysep == '-') && itml.Count <= 2)
			{
				calc = 0;
			}
			//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n"+itml[0].ToString()+itml[1].ToString());//+itml[2].ToString());
			while (i < itml.Count)
			{
				if (mysep == '+')
				{
					calc += Convert.ToDouble(itml[i]);
				}

				if (mysep == '-')
				{
					calc -= Convert.ToDouble(itml[i]);
				}

				if (mysep == '*')
				{
					calc *= Convert.ToDouble(itml[i]);
				}

				if (mysep == '/')
				{
					calc /= Convert.ToDouble(itml[i]);
				}

				if (mysep == '?')
				{
					//MessageBox.Show(calc.ToString());
					//MessageBox.Show(itml[i].ToString());
					//MessageBox.Show((calc/Convert.ToDouble(itml[i])).ToString());
					//целочисленное деление
					//округляем в большую сторону
					double ost = calc - calc % Convert.ToDouble(itml[i]);
					calc = (Convert.ToDouble(itml[i]) + ost) / Convert.ToDouble(itml[i]);
				}

				i += 2;
			}

			return calc;
		}

		public int fsplit()
		{
			int err = 1;

			string st = "";
			string wst = "";
			bool chst = false;
			int pass = 0;

			char ch = fstr[0];
			//char wch='';

			string itm = "";

			int i = 0;
			while (i < fstr.Length)
			{
				ch = fstr[i];

				//определяторы состояния
				if (sep.Contains(ch.ToString()))
				{
					wst = "sep";
				}
				if (!sep.Contains(ch.ToString()))
				{
					wst = "itm";
				}




				if (pass != 0)
				{
					wst = "itm";
				}
				if (ch == '(')
				{
					pass++;
				}
				if (ch == ')')
				{
					pass--;
				}

				if (i + 1 == fstr.Length)
				{
					wst = "end";
					itm += ch;
				}

				chst = st != wst;
				//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n\r\n"+itm+wst+st);
				//смена состояния
				/*
				if(st=="itm"&&(wst=="sep"||wst=="end"))
				{
				tdeb.fdeb("tf_itm____  ch"+ch+"|tim|"+itm+"|st|"+st+"|wst|"+wst);
				itml.Add(itm);
				itmtypl.Add("itm");
				itm="";
				}
				//смена состояния
				if(st=="sep"&&(wst=="itm"||wst=="end"))
				{
				itml.Add(itm);
				itmtypl.Add("sep");
				itm="";
				}*/

				if (chst)
				{
					tdeb.fdeb("tf", "tf_itm____  ch" + ch + "|tim|" + itm + "|st|" + st + "|wst|" + wst);
					itml.Add(itm);
					itm = "";
				}

				//tdeb.fdeb("tf_____  ch"+ch+"|tim|"+itm+"|st|"+st+"|wst|"+wst);

				itm += ch;
				st = wst;
				i++;
			}

			return err;
		}

		public int fsplitch(char sp)
		{
			int err = 1;

			string st = "itm";
			string wst = "";
			bool chst = false;
			int pass = 0;

			char ch = fstr[0];
			//char wch='';

			string itm = "";

			int i = 0;
			while (i < fstr.Length)
			{
				ch = fstr[i];
				//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n"+"|i|"+i.ToString());
				//определяторы состояния
				if (sp == ch)
				{
					wst = "sep";
				}
				if (sp != ch)
				{
					wst = "itm";
				}




				if (pass != 0)
				{
					wst = "itm";
				}
				if (ch == '(')
				{
					pass++;
				}
				if (ch == ')')
				{
					pass--;
				}
				/*
				if (i+1==fstr.Length)
				{
					wst="end";
					itm+=ch;
				}
				*/

				chst = st != wst;
				tdeb.fdeb("tf", "tf_itm_tf____  ch" + ch + "|tim|" + itm + "|st|" + st + "|wst|" + wst + "|chst|" + chst.ToString() + "|i|" + i.ToString());
				//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n\r\n"+itm+wst+st);
				//смена состояния
				/*
				if(st=="itm"&&(wst=="sep"||wst=="end"))
				{
				tdeb.fdeb("tf_itm____  ch"+ch+"|tim|"+itm+"|st|"+st+"|wst|"+wst);
				itml.Add(itm);
				itmtypl.Add("itm");
				itm="";
				}
				//смена состояния
				if(st=="sep"&&(wst=="itm"||wst=="end"))
				{
				itml.Add(itm);
				itmtypl.Add("sep");
				itm="";
				}*/

				if (chst)
				{
					tdeb.fdeb("tf", "tf_itm____  ch" + ch + "|tim|" + itm + "|st|" + st + "|wst|" + wst);

					/*
					if (MessageBox.Show("Встретил скобки.\r\n"+
						"\r\nfstr="+fstr+
						"\r\nitm="+itm+
						"\r\nch="+ch.ToString()+
						"\r\nch="+mysep.ToString()+
						"\r\nПродолжить?", "ff",
						System.Windows.Forms.MessageBoxButtons.YesNo,
						System.Windows.Forms.MessageBoxIcon.Warning)==DialogResult.No)
					{
						itm="";
						return 0;
					}
					*/
					/*
					if (itm.StartsWith("(")&&itm.EndsWith(")"))
					{
						itm=itm.Substring(1, itm.Length-2);
					}
					*/

					if (itm == "") itml.Add("0"); else itml.Add(itm);
					itm = "";
				}

				if (i + 1 == fstr.Length)
				{

					itm += fstr[i];
					tdeb.fdeb("tf", "tf_itm____  ch" + ch + "|tim|" + itm + "|st|" + st + "|wst|" + wst);

					/*
					if (MessageBox.Show("Встретил скобки.\r\n"+
						"\r\nfstr="+fstr+
						"\r\nitm="+itm+
						"\r\nch="+ch.ToString()+
						"\r\nch="+mysep.ToString()+
						"\r\nПродолжить?", "ff",
						System.Windows.Forms.MessageBoxButtons.YesNo,
						System.Windows.Forms.MessageBoxIcon.Warning)==DialogResult.No)
					{
						itm="";
						return 0;
					}
					*/
					/*
					if (itm.StartsWith("(")&&itm.EndsWith(")"))
					{
						itm=itm.Substring(1, itm.Length-2);
					}
					*/

					if (itm == "") itml.Add("0"); else itml.Add(itm);
					itml.Add(itm);
					itm = "";
				}


				//tdeb.fdeb("tf_____  ch"+ch+"|tim|"+itm+"|st|"+st+"|wst|"+wst);

				itm += ch;
				st = wst;
				i++;
				//MessageBox.Show("\r\n\r\n\r\n\r\n\r\n\r\n"+"|st|"+st+"|wst|"+wst+"|i|"+i.ToString());
			}

			return err;
		}
	}

	#endregion tf
}

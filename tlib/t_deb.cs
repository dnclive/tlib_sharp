using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace kibicom.tlib
{
	public class t_deb:t
	{

		static t args=new t();

//отладка влючена если отладочная версия
#if DEBUG
		static bool debug=true;
#else
		static bool debug=false;
#endif

		static public t f_set_deb_group(string group)
		{
			t_deb.args["deb_group"] = new t(group);

			return null;
		}
		
		static public t f_set_context_info(t args)
		{
			t res=new t()
			{
				{"show_file", t_deb.args["show_file"].f_set(args["show_file"].f_def(true).f_val()).f_bool()},
				{"show_f_name", t_deb.args["show_f_name"].f_set(args["show_f_name"].f_def(true).f_val()).f_bool()},
				{"show_line", t_deb.args["show_line"].f_set(args["show_line"].f_def(true).f_val()).f_bool()},
			};
			
			return res;
		}

		static public t f_deb(string group, string fmt, params object[] args)
		{
			if (!debug) return null;
			string deb_group = t_deb.args["deb_group"].f_def_set("main").f_str();

			bool show_file=t_deb.args["show_file"].f_def_set("true").f_bool();
			bool show_f_name = t_deb.args["show_f_name"].f_def_set("true").f_bool();
			bool show_line=t_deb.args["show_line"].f_def_set("true").f_bool();

			if (group != deb_group) return null;

			StackFrame callStack = new StackFrame(1, true);

			string info_fmt = (show_file ? "{0}:" : "") + (show_f_name ? "{1}:" : "") + (show_line ? "{2}" : "");


			Console.WriteLine(info_fmt, callStack.GetFileName(), callStack.GetMethod(), callStack.GetFileLineNumber());

			Console.WriteLine(fmt, args);
			
			return null;
		}


		static public t f_deb3(string group, string file, int line, string fmt, params object[] args)
		{
			if (!debug) return null;
			string deb_group = t_deb.args["deb_group"].f_def_set("main").f_str();
			
			if (group!=deb_group) return null;

			StackFrame callStack = new StackFrame(1, true);

			//va_list ap;        /* указывает на очередной безымянный аргумент */ 
			//char *p, *sval;
			//char chval;
			//int ival;
			//void *pval;
			//double dval;

			//printf("%s:%i:", file, line);	//вывод файла и номера строки в которых 
											//вызвано отладочное сообщение

			Console.WriteLine("{0}:{1}", file, line);

			Console.WriteLine(fmt,args);

			return null;
			//va_start(ap, fmt); /* устанавливает ap на 1-й безымянный аргумент */ 
			//for (p=fmt; *p; p++)
			int i=0;
			int arg_i=0;
			foreach(char ch in fmt)
			{
				//if (*p !='%')
				if (ch!='%')
				{
					//putchar(*p);
					continue;
				}
				//printf("tdeb_fdeb   %c\r\n", *p);
				switch (fmt[i+1]) 
				{
					case 'i':
						//ival = (int)args[arg_i];
						Console.Write("%i", (int)args[arg_i]);
						break;
					case 'd':
						//ival = (double)ap;
						Console.Write ("%d", (double)args[arg_i]);
						break;
					case 'f':
						//dval = va_arg(ap, double);
						Console.Write("%f", (double)args[arg_i]);
						break;
					case 's':
						//for (sval = va_arg(ap, char *); *sval; sval++)
						//	putchar(*sval);
						Console.Write("%s", (string)args[arg_i]);
						break;
					case 'c':
						//for (sval = va_arg(ap, char *); *sval; sval++)
						//	putchar(*sval);
						//chval=va_arg(ap, char);
						//printf("%c", chval);
						Console.Write("%c", (string)args[arg_i]);
						break;
					//case 'p':
						//pval = va_arg(ap, void *);
						//printf ("%p", pval);
						//Console.Write("%p", (string)args[arg_i]);
						//break;
					default:
						//putchar(*p);
						Console.Write("%c", fmt[i+1]);
						break;
				}

				i++;
				arg_i++;
			}
			//va_end(ap); /* очистка, когда все сделано */

			return null; 
		}

		static public string f_deb_group()
		{
			return t_deb.args["deb_group"].f_def_set("main").f_str();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace kibicom.tlib
{
	public class t_deb:t
	{

		public t_deb f_set_deb_group(string group)
		{
			this["deb_group"] = new t(group);

			return this;
		}

		public t_deb tdeb_fdeb3(string group, string file, int line, string fmt, params object[] args)
		{		
			string deb_group=this["deb_group"].f_def_set("main").f_str();
			
			if (group!=deb_group) return this;

			//va_list ap;        /* указывает на очередной безымянный аргумент */ 
			//char *p, *sval;
			//char chval;
			//int ival;
			//void *pval;
			//double dval;

			//printf("%s:%i:", file, line);	//вывод файла и номера строки в которых 
											//вызвано отладочное сообщение

			Console.WriteLine("%s:%i", file, line);

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

			return this; 
		}

		public string f_deb_group()
		{
			return this["deb_group"].f_def_set("main").f_str();
		}
	}
}

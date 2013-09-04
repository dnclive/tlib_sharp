using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace kibicom.tlib
{
	public partial class t
	{
		public t f_json()
		{
			string self_json_str = "";

			//если текущий t - массив ключ:значений
			if (key_val_arr.Count > 0)
			{
				
				foreach (KeyValuePair<string, t> item in key_val_arr)
				{
					//json текущего элемента
					string json_item = "\"" + item.Key + "\":" + item.Value.f_json().f_get("json_str").f_str();
					
					//присоединяем очередной элемент к всей строке
					self_json_str=t_uti.fjoin(self_json_str, ',', json_item);
				}

				return new t(){{"json_str","{" + self_json_str + "}"}};

			}

			//если массив [index]:значений
			if (val_arr.Count > 0)
			{

				foreach (t item in val_arr)
				{
					//json текущего элемента
					string json_item = item.f_json().f_get("json_str").f_str();

					//присоединяем очередной элемент к всей строке
					self_json_str = t_uti.fjoin(self_json_str, ',', json_item);
				}

				return new t() { { "json_str", "[" + self_json_str + "]" } };

			}

			//если он сам значение
			if (val != null)
			{
				if (this.f_str() == "False" || this.f_str() == "True")
				{
					return new t() { { "json_str", "" + this.f_str().ToLower() + "" } };
				}
				return new t() { { "json_str", "\"" + this.f_str().Replace("\"", "\\\"") + "\"" } };
			}



			return new t() { { "json_str", "null" } };
		}

		static public t f_dict_2_t(t args)
		{
			try
			{
				object dict = args["dict"].f_val();
				t res = new t();
				if (dict == null)
				{
					return res;
				}
				//MessageBox.Show(dict.GetType().FullName);
				if (dict.GetType() == typeof(Dictionary<string, object>))
				{
					foreach (KeyValuePair<string, object> dict_item in (Dictionary<string, object>)dict)
					{
						res[dict_item.Key] = f_dict_2_t(new t() { { "dict", dict_item.Value } });
						//Dictionary<string, object> tab_payment = ((Dictionary<string, object>)order.Value)["tab_order"];
						//перебираем платежи по текущему заказу
						//foreach (KeyValuePair<string, object> order in )
					}
				}
				else if (dict.GetType() == typeof(ArrayList))
				{
					foreach (object dict_item in (ArrayList)dict)
					{
						res.Add(f_dict_2_t(new t() { { "dict", dict_item } }));
						//Dictionary<string, object> tab_payment = ((Dictionary<string, object>)order.Value)["tab_order"];
						//перебираем платежи по текущему заказу
						//foreach (KeyValuePair<string, object> order in )
					}
				}
				else
				{
					res.f_set(dict);
				}

				return res;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			return new t();
		}

	}
}

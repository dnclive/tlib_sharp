using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace kibicom.tlib
{
	public class t_dot
	{
		static public object f_get_val_from_json_obj(object json_obj, string dot_key)
		{
			//Dictionary<string, object> json_obj_dic=null;
			//ArrayList json_obj_arr=null;

			string dot_keyi = f_get_top_dot_keyi(dot_key);
			string dot_key_tail = f_get_dot_key_tail(dot_key);

			if (dot_keyi == "")
			{
				return json_obj;
			}

			int num_dot_keyi;
			if (int.TryParse(dot_keyi, out num_dot_keyi))
			{
				ArrayList json_obj_arr = (ArrayList)json_obj;



				return f_get_val_from_json_obj(json_obj_arr[num_dot_keyi], dot_key_tail);

				/*
				foreach (object json_obj_arri in json_obj_arr)
				{
					f_get_val_from_json_obj(json_obj_arri, dot_key_tail);	
				}
				*/
			}
			else
			{

				Dictionary<string, object> json_obj_dic = (Dictionary<string, object>)json_obj;

				return f_get_val_from_json_obj(json_obj_dic[dot_keyi], dot_key_tail);
			}
			return null;
		}

		static public string f_get_top_dot_keyi(string dot_key)
		{
			int doti = dot_key.IndexOf('.');
			return doti < 0 ? dot_key : dot_key.Substring(0, dot_key.IndexOf('.'));
		}

		static public string f_get_dot_key_tail(string dot_key)
		{
			int doti = dot_key.IndexOf('.');
			return doti < 0 ? "" : dot_key.Substring(doti + 1, dot_key.Length - doti - 1);
		}
	}
}

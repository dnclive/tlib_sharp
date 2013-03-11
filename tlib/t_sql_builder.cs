using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;

namespace kibicom.tlib
{
	class t_sql_builder
	{
		static public string f_make_ins_query(t args)
		{
			DataTable tab = args["tab"].f_val<DataTable>();

			string set_date_format_sql = "SET DATEFORMAT ymd \r\n";
			string set_language_sql = "SET LANGUAGE Russian \r\n";
			string ins_sql_str = "";
			string vals = "";
			int oper_dr_cnt = 0;
			foreach (DataRow dr in tab.Rows)
			{
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
					vals = t_uti.fjoin(vals, ',', f_db_val(dr, cl));
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
				{"query", query}
			}));

			return query;
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

			dbconn._db.OpenDB();

			try
			{


				int r_cnt = 0;
				err = 1000;
				//выполняем запросы обновления
				if (upd_sql_str != "")
				{
					dbconn._db.command.CommandText = set_date_format_sql + set_language_sql + upd_sql_str;
					r_cnt += dbconn._db.command.ExecuteNonQuery();
				}

				err = 2000;
				//выполняем запросы вставки
				if (ins_sql_str != "")
				{
					dbconn._db.command.CommandText = set_date_format_sql + set_language_sql + ins_sql_str;
					r_cnt += dbconn._db.command.ExecuteNonQuery();
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
				dbconn._db.command.Transaction.Rollback();

				//тоже глючная функция
				try
				{
					//команда обновления генератора
					dbconn._db.command.CommandText = "exec dbo.sys_update_generator";
					dbconn._db.command.ExecuteNonQuery();
				}
				catch (Exception ex1)
				{

				}

				MessageBox.Show("При сохранении заказа произошла ошибка, \r\n часть данных сохранить не удалось. \r\n Расчитайте заказ повторно - это исправит проблему!");

				dbconn._db.CloseDB();

				//пробуем еще раз сохранить данные
				//f_2_store(tab, id_key);

				return;

			}

			//если запросы выполнены успешно

			dbconn._db.CloseDB();

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

		static public string f_db_val(DataRow dr, DataColumn cl)
		{
			//if (cl.ColumnName=="deleted")
			//{
			//	MessageBox.Show(dr[cl.ColumnName].ToString()+"\r\n"+cl.DataType.ToString());
			//}
			if (dr[cl.ColumnName] == DBNull.Value)
			{
				return "null";
			}
			if (cl.DataType == typeof(DateTime))
			{
				return "'" + DateTime.Parse(dr[cl.ColumnName].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "'";
				//return "'"+DateTime.Parse(dr[cl.ColumnName].ToString()).ToUniversalTime()+"'";	
			}
			if (cl.DataType == typeof(String))
			{
				return "'" + dr[cl.ColumnName].ToString() + "'";
			}
			if (cl.DataType == typeof(int) ||
					cl.DataType == typeof(Int16) ||
					cl.DataType == typeof(Int32) ||
					cl.DataType == typeof(Int64) ||
					cl.DataType == typeof(double) ||
					cl.DataType == typeof(float) ||
					cl.DataType == typeof(decimal)
				)
			{
				return dr[cl.ColumnName].ToString().Replace(',', '.');
			}
			if (cl.DataType == typeof(System.Guid))
			{
				return "'" + dr[cl.ColumnName].ToString() + "'";
			}
			if (cl.DataType== typeof(System.Byte[]))
			{
				return "'" + "0x" + BitConverter.ToString((byte[])dr[cl.ColumnName]).Replace("-", "") + "'";
			}
			else
			{
				throw new Exception("f_db_val " + cl.DataType + " not processing");
			}
			return "";
		}

		

	}
}

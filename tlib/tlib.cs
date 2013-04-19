using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;

namespace kibicom.tlib
{

	#region делигаты

	//public delegate void hwr_delegate(t_josi_store_req_args args);

	public delegate void t_f();
	//public delegate void t_f<T>();
	public delegate TResult t_f<TResult>();
	public delegate TResult t_f<T, TResult>(T args);

	//public delegate void josi_store_resp(Dictionary<string, object> res);



	#endregion делигаты

	class t_val<T>
	{
		public T val;

		public t_val(T val)
		{
			this.val = val;
		}

		public T f_val()
		{
			return val;
		}
	}

	//определяет как смешиваются структуры
	enum t_mix { replace, empty_only };

	//класс представляет собой универсальный динамический, структурируемый тип данных
	//где каждый элемент может быть строкой, числом, массивом, структурой, функцией и тд.
	[Serializable]
	public class t : IDictionary<string, t>, IList<t>
	{
		//массив именованных элементов (аля {key:value, key1:val1})
		private Dictionary<string, t> key_val_arr = new Dictionary<string, t>();

		//массив индексируемых объектов (аля обычный массив [val1, val2, val3, ...])
		private List<t> val_arr = new List<t>();

		//значение элемента если это строка целое, и тд любой значащий тип
		public object val = null;

		public Type val_type;

		//значение если это делегат, те функция
		public Delegate f = null;

		//флаг определяющий тип текущего объекта массив, структура, значение, функция
		//на данный момент не используется...
		bool is_struct;
		bool is_array;
		bool is_val;
		bool is_f;

		//true если данных t не несет значения
		bool is_val_empty = true;

		#region конструкторы

		public t()
		{

		}

		public t(object val)
		{
			this.val = val;
			is_val_empty = false;
		}

		public t(Delegate f)
		{
			this.f = f;
			is_val_empty = false;
		}

		#region adding and setting value

		public t f_add(bool replace, t val)
		{
			//сливаем key_val_arr
			foreach (KeyValuePair<string, t> item in (IDictionary<string, t>)val)
			{
				if (replace)
				{
					key_val_arr[item.Key] = item.Value;
					continue;
				}
				if (!key_val_arr.ContainsKey(item.Key))
				{
					key_val_arr[item.Key] = item.Value;
				}
			}

			//сливаем val_arr
			foreach (t item in (IList<t>)val)
			{
				val_arr.Add(item);
			}

			//заменяем значение если нужно
			if (replace)
			{
				this.val = val.val;
			}

			return this;

		}

		public t f_dub_mix(bool replace, t val)
		{
			return new t().f_add(true, this).f_add(replace, val);
		}

		/// <summary>
		/// <para>replace current value if passed val is not null</para>
		/// <para>_</para>
		/// <para>PARAMS</para>
		/// <para>val_________________new val</para>
		/// <para>_</para>
		/// <para>RETURN</para>
		/// <para>val</para>
		/// </summary>
		/// данный код пока выключаем поскольку череват ошибками
		public t f_replace_(t val)
		{
			if (val != null)
			{
				//переносим значения
				this.key_val_arr = val.key_val_arr;
				this.val_arr = val.val_arr;
				this.val = val.val;
				this.val_type = val.val_type;
				this.f = val.f;
			}
			return this;
		}

		/// <summary>
		/// <para>replace item value if passed item is not null</para>
		/// <para>_</para>
		/// <para>PARAMS</para>
		/// <para>val_________________new val</para>
		/// <para>_</para>
		/// <para>RETURN</para>
		/// <para>val</para>
		/// </summary>
		/// заменяет свой элемент если новое предлагаемое значение не пусто
		public t f_replace(string key, t item)
		{
			Console.WriteLine("item.val " + item.val.ToString());
			//если передаваемое занчение пустое
			if ((item.val == null || item.val.ToString() == "") && item.f == null)
			{
				return this;
			}

			this[key] = item;
			return this;
		}

		#endregion adding and setting value

		#endregion конструкторы

		#region полечуние значения

		//на удаление!!!
		public t_f<t, t> f_f<D>()
		{
			//return Delegate.CreateDelegate(
			return (t_f<t, t>)f;
		}

		public t_f<t, t> f_f()
		{
			//return Delegate.CreateDelegate(
			return (t_f<t, t>)f;
		}

		/*возвращает объект*/
		public T f_val<T>()
		{
			//if (val == null) return val;
			//MessageBox.Show(typeof(T).ToString());
			if (typeof(T).ToString() == "System.Boolean")
			{
				if (val == null || val.ToString() == "False")
				{
					return (T)Convert.ChangeType(false, typeof(T));
				}
				else
				{
					return (T)Convert.ChangeType(true, typeof(T));
				}

			}

			try
			{
				return (T)val;
			}
			catch (Exception ex)
			{

				ex.Data.Add("t", this);
				ex.Data.Add("val", val);
				ex.Data.Add("args", ex);
				//MessageBox.Show(val.ToString() + "\r\n"+ex.Message);
				throw (ex);
			}

		}

		public object f_val(object val)
		{

			//Type generic = typeof(List<>);
			//Type specific = generic.MakeGenericType(val.GetType());
			//System.Reflection.ConstructorInfo ci = specific.GetConstructor(Type.EmptyTypes);
			//this.val = ci.Invoke(new object[] { });

			//Type tp=typeof(t_val<>);
			//Type tp2 = val.GetType();
			//Type tp3 = tp2.MakeGenericType(new System.Type[] { tp2 });
			//System.Type gtp = Type.MakeGenericType(new System.Type[] { tp });
			//Activator.CreateInstance(typeof(T), args)
			//t_val<tp3> new_val=new t_val<tp3>(val);

			this.val = val;

			return this.val;
		}

		public object f_val()
		{

			//Type generic = typeof(List<>);
			//Type specific = generic.MakeGenericType(val.GetType());
			//System.Reflection.ConstructorInfo ci = specific.GetConstructor(Type.EmptyTypes);
			//this.val = ci.Invoke(new object[] { });

			//Type tp=typeof(t_val<>);
			//Type tp2 = val.GetType();
			//Type tp3 = tp2.MakeGenericType(new System.Type[] { tp2 });
			//System.Type gtp = Type.MakeGenericType(new System.Type[] { tp });
			//Activator.CreateInstance(typeof(T), args)
			//t_val<tp3> new_val=new t_val<tp3>(val);
			return val;
		}

		public T f_val<T>(T val)
		{
			this.val = val;
			return (T)this.val;
		}

		//значение по умолчанию
		public t f_def(object val)
		{
			if (this.val == null&&this.val_arr.Count==0&&this.key_val_arr.Count==0)
			{
				t new_t = new t(this.val);
				new_t.val = val;
				return new_t;
			}
			else
			{
				return this;
			}
		}

		public t f_def_get(object val)
		{
			if (this.val == null)
			{
				t new_t = new t(this.val);
				new_t.val = val;
				return new_t;
			}
			else
			{
				return this;
			}
		}

		public t f_def_set(object val)
		{
			if (this.val == null)
			{
				this.val = val;
			}
			return this;
		}

		//значение пустое
		//требует переработки
		public bool f_is_empty()
		{
			if ((val == null || val == "") && f == null)
			{
				return true;
			}

			return false;
		}

		//задать значение элемента
		public t f_set(string key, object val)
		{
			if (val.GetType().Name != "System.t")
			{
				val = new t(val);
			}
			this[key] = (t)val;
			return this;
		}

		//задать свое значение val
		public t f_set(object val)
		{
			this.val = val;
			return this;
		}



		/*возвращает строку*/
		public string f_str()
		{
			if (val == null)
			{
				return "";
			}
			if (val.GetType().Name == "System.String")
			{
				return (string)val;
			}
			else
			{
				return val.ToString();
			}
		}

		override public string ToString()
		{
			return f_str();
		}

		/*возвращает целое*/
		public int f_int()
		{
			return (int)val;
		}

		/*возвращает double*/
		public double f_double()
		{
			return (double)val;
		}

		/*возвращает float*/
		public float f_float()
		{
			return (float)val;
		}

		/*возвращает bool*/
		public bool f_bool()
		{
			return (bool)val;
		}


		/*** возврат значений ***/

		public t f_last()
		{
			if (val_arr.Count==0)
			{
				return new t();
			}
			else
			{
				return val_arr[val_arr.Count - 1];
			}
		}

		public t f_last(int count)
		{
			if (val_arr.Count<count)
			{
				return new t().f_add(true, this);
			}
			else
			{
				t last_list=new t();
				int i=val_arr.Count-count;
				while(i<val_arr.Count-1)
				{
					last_list.Add(this[i]);
				}
				return last_list;
			}
		}

		public t f_top(int count)
		{
			if (val_arr.Count < count)
			{
				return new t().f_add(true, this);
			}
			else
			{
				t last_list = new t();
				int i = 0;
				while (i < count-1)
				{
					last_list.Add(this[i]);
				}
				return last_list;
			}
		}

		#endregion получение значения

		#region работа с val int

		public t f_inc()
		{
			if (val.GetType().Name == "System.Int")
			{
				var int_val = (int)this.val;
				int_val++;
				this.val = int_val;
			}
			return this;
		}

		public t f_dec()
		{
			if (val.GetType().Name == "System.Int")
			{
				var int_val = (int)this.val;
				int_val--;
				this.val = int_val;
			}
			return this;
		}

		#endregion работа с val int

		#region implicit преобразование
		/*
		public static implicit operator t(object val)
		{
			return new t(val);
		}
		
		public static explicit operator object(t val)
		{
			return val.f_val<object>();
		}
		*/
		#endregion implicit преобразование

		#region удаление значения

		//удаляет элемент текущего объекта
		public t f_drop(string key)
		{
			t item = this[key];
			Remove(key);
			return this;
		}

		//удаляет переданный массив
		public t f_drop(List<string> key_arr)
		{
			foreach (string key in key_arr)
			{
				Remove(key);
			}

			return this;
		}

		#endregion удаление значения

		#region idictionary

		public t this[string key]
		{
			get
			{
				//MessageBox.Show(key_val_arr.ContainsKey(key).ToString());


				if (key_val_arr.ContainsKey(key))
				{
					return (t)key_val_arr[key];
				}
				else
				{
					t new_t = new t();
					key_val_arr.Add(key, new_t);
					return new_t;
				}
			}
			set
			{
				//MessageBox.Show(key_val_arr.ContainsKey(key).ToString());
				//Console.WriteLine(key);
				//Console.WriteLine(value.GetType().ToString());

				//берем переданное значение
				t tval = null;

				//если тим значения не tlib.t
				//преобразуем к нему
				if (value.GetType().ToString() != "kibicom.tlib.t")
				{
					tval = new t(value);
				}
				else
				{
					tval = value;
				}
				if (key_val_arr.ContainsKey(key))
				{
					key_val_arr[key] = tval;
				}
				else
				{
					key_val_arr.Add(key, tval);
				}
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return key_val_arr.Keys;
			}
		}

		public ICollection<t> Values
		{
			get
			{
				return key_val_arr.Values;
			}
		}

		public int Count
		{
			get
			{
				return val_arr.Count+key_val_arr.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public void Add(string key, t val)
		{
			key_val_arr.Add(key, val);
		}

		public void Add(string key, object val)
		{
			//KeyValuePair<string, t_res> new_item=new KeyValuePair<string, t_res>(key, null);
			//new_item.val = val;
			key_val_arr.Add(key, new t(val));
		}

		public void Add(string key, Delegate f)
		{
			//KeyValuePair<string, t_res> new_item=new KeyValuePair<string, t_res>(key, null);
			//new_item.val = val;
			key_val_arr.Add(key, new t(f));
		}

		public void Add(KeyValuePair<string, t> item)
		{
			key_val_arr.Add(item.Key, item.Value);
		}

		public bool Remove(string key)
		{
			return key_val_arr.Remove(key);
		}

		public bool Remove(KeyValuePair<string, t> item)
		{
			return key_val_arr.Remove(item.Key);
		}

		public bool TryGetValue(string key, out t val)
		{
			return key_val_arr.TryGetValue(key, out val);
		}

		public void Clear()
		{
			key_val_arr.Clear();
		}

		public bool ContainsKey(string key)
		{
			return key_val_arr.ContainsKey(key);
		}

		public bool Contains(KeyValuePair<string, t> item)
		{
			return false;
		}

		public void CopyTo(KeyValuePair<string, t>[] key_val_arr, int pos)
		{

		}

		public bool ContainsValue(t val)
		{
			return key_val_arr.ContainsValue(val);
		}

		public IEnumerator<KeyValuePair<string, t>> GetEnumerator_id()
		{
			return key_val_arr.GetEnumerator();
		}

		IEnumerator<KeyValuePair<string, t>> IEnumerable<KeyValuePair<string, t>>.GetEnumerator()
		{

			return GetEnumerator_id();
		}

		#endregion idictionary

		#region ilist

		public t this[int key]
		{
			get
			{
				//MessageBox.Show(val_arr.Count.ToString());
				if (val_arr.Count > key)
				{
					return val_arr[key];
				}
				else
				{
					return (new t(""));
				}
			}
			set
			{
				//MessageBox.Show(val_arr.Count.ToString());
				if (val_arr.Count > key)
				{
					val_arr[key] = value;
				}
				else
				{
					//val_arr.Add(value);
				}
			}
		}

		public void Add(t item)
		{
			val_arr.Add(item);
		}

		public void Add(object item)
		{
			val_arr.Add(new t(item));
		}

		public void Insert(int index, t item)
		{
			val_arr.Insert(index, item);
		}

		public void Insert(int index, object item)
		{
			val_arr.Insert(index, new t(item));
		}

		public int IndexOf(t item)
		{
			return val_arr.IndexOf(item);
		}

		public bool Contains(t item)
		{
			return val_arr.Contains(item);
		}

		public void CopyTo(t[] val_arr, int pos)
		{
			val_arr.CopyTo(val_arr, pos);
		}

		public bool Remove(t item)
		{
			return val_arr.Remove(item);
		}

		public void RemoveAt(int index)
		{
			val_arr.RemoveAt(index);
		}

		public IEnumerator<t> GetEnumerator_il()
		{
			return val_arr.GetEnumerator();
		}

		IEnumerator<t> IEnumerable<t>.GetEnumerator()
		{

			return GetEnumerator_il();
		}

		#endregion ilist

		#region enumerators

		/*
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return (IEnumerator<IDictionary<string, object>>)key_val_arr.GetEnumerator();
        }
        */

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator_il();
		}

		#endregion enumerators

		#region функции обратного вызова

		public static void f_f(string f_name, t args)
		{
			if (args == null) return;
			if (args[f_name].f_f<t_f<t, t>>() != null)
			{
				args[f_name].f_f<t_f<t, t>>()(args);
			}
		}

		public static void f_f(t_f<t, t> f, t args)
		{
			if (f != null)
			{
				f(args);
			}
		}

		#region выполнено

		public static void f_fdone(t args)
		{
			if (args["f_done"].f_f<t_f<t, t>>() != null)
			{
				args["f_done"].f_f<t_f<t, t>>()(args);
			}
		}

		public static void f_fdone(t_f<t, t> f_done, t args)
		{
			if (f_done != null)
			{
				f_fdone(args);
			}
		}

		#endregion выполнено

		#region перебор элементов

		public static void f_feach(t args)
		{
			if (args["f_each"].f_f<t_f<t, t>>() != null)
			{
				args["f_each"].f_f<t_f<t, t>>()(args);
			}
		}

		public static void f_feach(t_f<t, t> f_each, t args)
		{
			if (f_each != null)
			{
				f_feach(args);
			}
		}

		#endregion перебор элементов

		#endregion функции обратного вызова

		#region async

		delegate t d_f_async_self(object obj, string f_name, t args);

		public t f_async(string f_name, t args)
		{

			//MethodInfo mi = this.GetType().GetMethod(f_name,
			//			System.Reflection.BindingFlags.Public
			//			| System.Reflection.BindingFlags.Instance);

			//this.GetType().InvokeMember(

			//t.f_async((t_f<t, t>)mi, args);

			//mi.Invoke();


			//
			d_f_async_self f_async_self = new d_f_async_self(t.f_async_self);

			IAsyncResult result = f_async_self.BeginInvoke(this, f_name, args, new AsyncCallback(f_async_done), null);

			return null;
		}

		static t f_async_self(object obj, string f_name, t args)
		{

			obj.GetType().GetMethod(f_name).Invoke(obj, new object[] { args });

			return new t();
		}

		static t f_async(object obj, t_f<t, t> f, t args)
		{
			IAsyncResult result = f.BeginInvoke(args, new AsyncCallback(f_async_done), null);

			return new t();
		}

		static void f_async_done(IAsyncResult ar)
		{
			// Retrieve the delegate.
			AsyncResult result = (AsyncResult)ar;
			t_f<t, t> caller = (t_f<t, t>)result.AsyncDelegate;

			// Retrieve the format string that was passed as state 
			// information.
			t args = (t)ar.AsyncState;

			// Define a variable to receive the value of the out parameter.
			// If the parameter were ref rather than out then it would have to
			// be a class-level field so it could also be passed to BeginInvoke.
			int threadId = 0;

			// Call EndInvoke to retrieve the results.
			t relust = caller.EndInvoke(result);

			// Use the format string to format the output message.
			//Console.WriteLine(formatString, threadId, returnValue);
		}

		#endregion async

		#region seq complex

		//массив цепочек
		static t chain_arr = new t();

		static public t f_chain(t args)
		{
			string seq_name = args["seq_name"].f_def("main").f_str();
			bool async = args["async"].f_def(false).f_val<bool>();

			//перебираем переданные функции
			foreach (t f in (IList<t>)args["seq"])
			{
				chain_arr[seq_name].f_def_set(new t()
				{
					{"name",			seq_name},
					{"is_seq",			true},
					{"current_index",	0},
					{"all_done",		false}

				})["items"].Add(new t(f));
			}

			//не реализована возможность асинхронного вызова

			//текущая цепочка
			t chain=chain_arr[seq_name];

			//текущая функция для вызова
			int index=chain["current_index"].f_def(0).f_val<int>();

			//вызываем первую функцию цепочки
			chain["items"][index].f_f().Invoke(args);

			return null;
		}

		static public t f_chain_next(string chain_name)
		{
			//текущая цепочка
			t chain = chain_arr[chain_name];

			//текущая функция для вызова
			int index = chain["current_index"].f_def(0).f_val<int>();

			//вызываем следующую функцию
			chain["items"][index++].f_f().Invoke(new t());

			chain["current_index"].f_val(index);

			return null;
		}

		#endregion seq complex

	}

}

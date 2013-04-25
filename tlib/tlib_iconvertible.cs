using System;
using System.Collections.Generic;
using System.Text;

namespace kibicom.tlib
{
	//реализация IConvertible
	public partial class tlib: IConvertible
	{

		public TypeCode GetTypeCode()
		{
			return TypeCode.Object;
		}

		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return true;
		}

		double GetDoubleValue()
		{
			return new double();
		}

		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return new byte();
		}

		char IConvertible.ToChar(IFormatProvider provider)
		{
			return new char();
		}

		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return new DateTime();
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return new decimal();
		}

		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return new double();
		}

		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return new short();
		}

		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return new int();
		}

		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return new long();
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return new sbyte();
		}

		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return new float();
		}

		string IConvertible.ToString(IFormatProvider provider)
		{
			return "";
		}

		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			return Convert.ChangeType(this,conversionType);
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return new ushort();
		}

		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return new uint();
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return new ulong();
		}

	}
}

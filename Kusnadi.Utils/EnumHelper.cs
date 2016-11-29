using System;

namespace Kusnadi.Utils
{
	public class EnumHelper<T> where T : struct, IConvertible
	{
		public T? GetValue(string enumName, StringComparison comparisonMode = StringComparison.InvariantCultureIgnoreCase)
		{
			return Enum.GetValue<T>(enumName, comparisonMode);
		}

		public T? GetValue(long enumIntrinsicValue)
		{
			return Enum.GetValue<T>(enumIntrinsicValue);
		}

		public U GetAttribute<U>(System.Enum enumValue, bool inherit = false) where U : Attribute
		{
			return Enum.GetAttribute<U>(enumValue, inherit);
		}

		public string GetDescription(System.Enum enumValue)
		{
			return Enum.GetDescription(enumValue);
		}
	}
}

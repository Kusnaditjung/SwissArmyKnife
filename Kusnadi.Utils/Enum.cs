using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Kusnadi.Utils
{
    public static class Enum
    {
		public static T? GetValue<T>(this string enumName, StringComparison comparisonMode = StringComparison.InvariantCultureIgnoreCase) where T : struct, IConvertible
		{
			Type enumType = typeof(T);
			if (!enumType.IsEnum)			
				throw new Exception("Generic T must be an Enumeration type.");

			Array enumValues = global::System.Enum.GetValues(enumType);

			foreach(object enumValue in enumValues)			
				if (string.Equals(enumValue.ToString(), enumName, comparisonMode))
					return (T)enumValue;			

			return null;			
		}

		public static T? GetValue<T>(this long enumIntrinsicValue) where T : struct, IConvertible
		{
			Type enumType = typeof(T);
			if (!enumType.IsEnum)			
				throw new Exception("Generic T must be an Enumeration type.");		
			
			Array enumValues = global::System.Enum.GetValues(enumType);
			
			foreach (object enumValue in enumValues)
			{
				var longValue = (long) Convert.ChangeType(enumValue, typeof(long));

				if (longValue == enumIntrinsicValue)							
					return (T)enumValue;				
			}
			
			return null;
		}

		public static T GetAttribute<T>(this global::System.Enum enumValue, bool inherit = false) where T : Attribute
		{
			Type enumType = enumValue.GetType();			

			MemberInfo[] memInfo = enumType.GetMember(enumValue.ToString());
			object[] attributes = memInfo.First().GetCustomAttributes(typeof(T), inherit);

			if (!attributes.Any())
				return null;

			return (T)attributes.First();
		}

		public static string GetDescription(this global::System.Enum enumValue)
		{
			var descriptionAttribute = GetAttribute<DescriptionAttribute>(enumValue);

			if (descriptionAttribute == null)
				return null;

			return descriptionAttribute.Description;
		}
	}
}

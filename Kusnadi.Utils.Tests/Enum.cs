using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kusnadi.Utils;
using Kusnadi.Utils.Tests.Fakes;

namespace Kusnadi.Utils.Tests
{
	[TestClass]
	public class Enum
	{
		[TestMethod]
		public void GetValue_PassInvalidEnumName_ReturnNull()
		{
			var enumValue =  "test".GetValue<MessageChoice>();
			Assert.IsNull(enumValue);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void GetValue_PassNoneEnumTypeForReturnType_ThrowException()
		{
			var enumValue = "test".GetValue<Int32>();			
		}

		[TestMethod]
		public void GetValue_PassInvalidEnumNameAccordingComparisonMode_ReturnNull()
		{
			var enumValue = "sendsms".GetValue<MessageChoice>(StringComparison.Ordinal);
			Assert.IsNull(enumValue);
		}

		[TestMethod]
		public void GetValue_PassValidEnumName_ReturnAssociatedEnumValue()
		{
			var enumValue = "sendsms".GetValue<MessageChoice>();
			Assert.AreEqual(MessageChoice.SendSms, enumValue);
		}

		[TestMethod]
		public void GetValue_PassInvalidEnumValue_ReturnNull()
		{
			var enumValue = Utils.Enum.GetValue<MessageChoice>(100);
			Assert.IsNull(enumValue);
		}

		[TestMethod]
		public void GetValue_PassValidEnumIntrinsicValue_ReturnAssociatedEnumValue()
		{
			var enumValue = Utils.Enum.GetValue<MessageChoice>(3);
			Assert.AreEqual(MessageChoice.SendEmail, enumValue);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void GetValue_PassNoneEnumTypeForReturnTypeWithValidEnumIntrinsicValue_ThrowException()
		{
			var enumValue = Utils.Enum.GetValue<Int32>(3);
		}

		[TestMethod]		
		public void GetAttribute_PassEnumValueWithoutCorrectAttribute_ReturnNull()
		{
			var attribute = MessageChoice.SendEmail.GetAttribute<DataAttribute>();
			Assert.IsNull(attribute);
		}

		[TestMethod]
		public void GetAttribute_PassEnumValueWithCorrectAttribute_ReturnAssociatedAttribute()
		{
			var attribute = MessageChoice.GetEmail.GetAttribute<DataAttribute>();
			Assert.IsNotNull(attribute);
			Assert.AreEqual("Get Email", attribute.Description);
		}

		[TestMethod]
		public void GetDescription_WhenEnumHaveNoDescription_ReturnNull()
		{
			string description = MessageChoice.GetEmail.GetDescription();
			Assert.IsNull(description);			
		}

		[TestMethod]
		public void GetDescription_WhenHasDescription_ReturnDescription()
		{
			string description = MessageChoice.SendSms.GetDescription();
			Assert.AreEqual("Send Sms", description);
		}
	}
}

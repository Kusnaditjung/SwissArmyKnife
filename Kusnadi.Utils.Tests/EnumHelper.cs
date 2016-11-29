using Kusnadi.Utils.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Kusnadi.Utils.Tests
{
	[TestClass]
	public class EnumHelper
	{
		[TestMethod]
		public void GetValue_PassInvalidEnumName_ReturnNull()
		{
			var enumHelper = new EnumHelper<MessageChoice>();
			var enumValue =  enumHelper.GetValue("test");						
			Assert.IsNull(enumValue);
		}

		[TestMethod]		
		[ExpectedException(typeof(Exception))]
		public void GetValue_PassNoneEnumTypeForReturnType_ThrowException()
		{
			var enumHelper = new EnumHelper<Int32>();
			var enumValue = enumHelper.GetValue("test");			
		}

		[TestMethod]
		public void GetValue_PassInvalidEnumNameAccordingComparisonMode_ReturnNull()
		{
			var enumHelper = new EnumHelper<MessageChoice>();
			var enumValue = enumHelper.GetValue("sendsms", StringComparison.Ordinal);			
			Assert.IsNull(enumValue);
		}

		[TestMethod]
		public void GetValue_PassValidEnumName_ReturnAssociatedEnumValue()
		{
			var enumHelper = new EnumHelper<MessageChoice>();
			var enumValue = enumHelper.GetValue("sendsms");			
			Assert.AreEqual(MessageChoice.SendSms, enumValue);
		}

		[TestMethod]
		public void GetValue_PassInvalidEnumValue_ReturnNull()
		{
			var enumHelper = new EnumHelper<MessageChoice>();
			var enumValue = enumHelper.GetValue(100);
			Assert.IsNull(enumValue);
		}

		[TestMethod]
		public void GetValue_PassValidEnumIntrinsicValue_ReturnAssociatedEnumValue()
		{
			var enumHelper = new EnumHelper<MessageChoice>();
			var enumValue = enumHelper.GetValue(3);
			Assert.AreEqual(MessageChoice.SendEmail, enumValue);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void GetValue_PassNoneEnumTypeForReturnTypeWithValidEnumIntrinsicValue_ThrowException()
		{
			var enumHelper = new EnumHelper<Int32>();
			var enumValue = enumHelper.GetValue(3);
		}

		[TestMethod]
		public void GetAttribute_PassEnumValueWithoutCorrectAttribute_ReturnNull()
		{
			var enumHelper = new EnumHelper<MessageChoice>();
			var attribute = enumHelper.GetAttribute<DataAttribute>(MessageChoice.SendEmail);
			Assert.IsNull(attribute);
		}

		[TestMethod]
		public void GetAttribute_PassEnumValueWithCorrectAttribute_ReturnAssociatedAttribute()
		{
			var enumHelper = new EnumHelper<MessageChoice>();
			var attribute = enumHelper.GetAttribute<DataAttribute>(MessageChoice.GetEmail);
			Assert.IsNotNull(attribute);
			Assert.AreEqual("Get Email", attribute.Description);
		}

		[TestMethod]
		public void GetDescription_WhenEnumHaveNoDescription_ReturnNull()
		{
			var enumHelper = new EnumHelper<MessageChoice>();
			string description = enumHelper.GetDescription(MessageChoice.GetEmail);
			Assert.IsNull(description);
		}

		[TestMethod]
		public void GetDescription_WhenHasDescription_ReturnDescription()
		{
			var enumHelper = new EnumHelper<MessageChoice>();
			string description = enumHelper.GetDescription(MessageChoice.SendSms);
			Assert.AreEqual("Send Sms", description);
		}
	}
}

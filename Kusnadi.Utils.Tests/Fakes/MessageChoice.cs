using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kusnadi.Utils.Tests.Fakes
{
	public enum MessageChoice
	{
		[Description("Send Sms")]
		SendSms = 1,
		SendEmail = 3,
		[Data("Get Email", 3)]
		GetEmail = 6,
		GetSms = 8
	}
}

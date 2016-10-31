using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kusnadi.Utils.Tests.Fakes
{
	public class DataAttribute : Attribute
	{
		public string Description { get; private set; }
		public int Id { get; private set; }
		public DataAttribute(string description, int id)
		{
			Description = description;
			Id = id;
		}
	}
}

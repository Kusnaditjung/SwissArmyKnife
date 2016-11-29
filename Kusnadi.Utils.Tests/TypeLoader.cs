using Kusnadi.Utils.Tests.Fakes;
using Kusnadi.Utils.Tests.Fakes.TypeLoader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace Kusnadi.Utils.Tests
{
	[TestClass]
	public class TypeLoader
	{
		[TestMethod]
		public void Types_WhenPassAssembly_ReturnCorrectTypes()
		{
			var assemblyMock = new Moq.Mock<_Assembly>();
			assemblyMock.Setup(item => item.GetTypes())				
				.Returns(new Type[] { typeof(ClassA), typeof(ConcreteInterfaceA) });
			assemblyMock.Setup(item => item.GetObjectData(It.IsAny<SerializationInfo>(), It.IsAny<StreamingContext>()));

			var typeLoader = new Kusnadi.Utils.TypeLoader();
			typeLoader.LoadFromAssembly(assemblyMock.Object);
			IEnumerable<Type> types = typeLoader.Types;

			Assert.AreEqual(2, types.Count());
			Assert.IsTrue(types.Any(item => item.Equals(typeof(ClassA))));
			Assert.IsTrue(types.Any(item => item.Equals(typeof(ConcreteInterfaceA))));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void loadFromAssembly_WhenAssemblyIsNull_ThrowNullArgumentException()
		{
			Assembly assembly = null;
			var typeLoader = new Kusnadi.Utils.TypeLoader();
			typeLoader.LoadFromAssembly(assembly);

		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructing_PassNullAppDomain_ThrowNullArgumentException()
		{
			AppDomain appDomain = null;
			var typeLoader = new Kusnadi.Utils.TypeLoader();
			typeLoader.LoadFromAppDomain(appDomain);
		}

		[TestMethod]
		public void GetTypesImplementingType_PassTypeWithExistingImplementation_ReturnTypes()
		{			
			var typeLoader = new Kusnadi.Utils.TypeLoader();
			typeLoader.LoadFromCurrentAppDomain();

			//implementing abstract class
			var types = typeLoader.GetTypesImplementingType<AbstractA>();
			Assert.AreEqual(1, types.Count());
			Assert.AreEqual(types.First().Name, "ConcreteAbstractA");

			//implementing interface
			types = typeLoader.GetTypesImplementingType<InterfaceA>();
			Assert.AreEqual(1, types.Count());
			Assert.AreEqual(types.First().Name, "ConcreteInterfaceA");

			//implementing normal class
			types = typeLoader.GetTypesImplementingType<ClassA>();
			Assert.AreEqual(2, types.Count());
			Assert.IsTrue(types.Any(item => item.Name == "ConcreteConcreteClassA" || item.Name == "ConcreteClassA"));

			types = typeLoader.GetTypesImplementingType<ConcreteClassA>();
			Assert.AreEqual(1, types.Count());
			Assert.IsTrue(types.First().Name == "ConcreteConcreteClassA");
		}

		[TestMethod]
		public void GetTypesImplementingType_PassTypeWithExistingImplementationWithFlagIsTrue_ReturnTypes()
		{
			var typeLoader = new Kusnadi.Utils.TypeLoader();
			typeLoader.LoadFromCurrentAppDomain();

			//implementing normal class, return also the type
			var types = typeLoader.GetTypesImplementingType<ClassA>(true);
			Assert.AreEqual(3, types.Count());
			Assert.IsTrue(types.Any(item => item.Name == "ConcreteConcreteClassA" || item.Name == "ConcreteClassA" || item.Name == "ClassA"));

			types = typeLoader.GetTypesImplementingType<ConcreteClassA>(true);
			Assert.AreEqual(2, types.Count());
			Assert.IsTrue(types.Any(item => item.Name == "ConcreteConcreteClassA" || item.Name == "ConcreteClassA"));

			types = typeLoader.GetTypesImplementingType<ConcreteConcreteClassA>(true);
			Assert.AreEqual(1, types.Count());
			Assert.IsTrue(types.First().Name == "ConcreteConcreteClassA");

			//implementing abtract, return only implementation type
			types = typeLoader.GetTypesImplementingType<AbstractA>(true);
			Assert.AreEqual(1, types.Count());
			Assert.IsTrue(types.First().Name == "ConcreteAbstractA");

			//implementing interface, return only implemtation type
			types = typeLoader.GetTypesImplementingType<InterfaceA>(true);
			Assert.AreEqual(1, types.Count());
			Assert.IsTrue(types.First().Name == "ConcreteInterfaceA");
		}

		[TestMethod]
		public void GetTypesImplementingType_PassTypeWithNoExistingImplementation_ReturnEmpty()
		{
			var typeLoader = new Kusnadi.Utils.TypeLoader();

			//pass non existing implementation Types, with class
			var types = typeLoader.GetTypesImplementingType<ClassB>();
			Assert.AreEqual(0, types.Count());

			//pass non existing implementation Types, interface
			types = typeLoader.GetTypesImplementingType<InterfaceB>();
			Assert.AreEqual(0, types.Count());

			//pass non existing implementation Types, with abstract class
			types = typeLoader.GetTypesImplementingType<AbstractB>();
			Assert.AreEqual(0, types.Count());
		}

		[TestMethod]
		public void GetTypesWithAttribute_ReturnAllInstantiableTypes()
		{
			var typeLoader = new Kusnadi.Utils.TypeLoader();
			typeLoader.LoadFromCurrentAppDomain();

			var types = typeLoader.GetTypesWithAttribute<DataAttribute>();

			Assert.AreEqual(5, types.Count());
			Assert.IsTrue(types.Any(item => item.Name == "ClassA"));
			Assert.IsTrue(types.Any(item => item.Name == "ConcreteClassA"));
			Assert.IsTrue(types.Any(item => item.Name == "ConcreteConcreteClassA"));			
			Assert.IsTrue(types.Any(item => item.Name == "ConcreteAbstractA"));			
			Assert.IsTrue(types.Any(item => item.Name == "ConcreteInterfaceA"));
		}

		[TestMethod]
		public void GetTypesWithAttribute_WithFlagIsTrue_ReturnAllTypes()
		{
			var typeLoader = new Kusnadi.Utils.TypeLoader();
			typeLoader.LoadFromCurrentAppDomain();

			var types = typeLoader.GetTypesWithAttribute<DataAttribute>(true);

			Assert.AreEqual(7, types.Count());
			Assert.IsTrue(types.Any(item => item.Name == "ClassA"));
			Assert.IsTrue(types.Any(item => item.Name == "ConcreteClassA"));
			Assert.IsTrue(types.Any(item => item.Name == "ConcreteConcreteClassA"));		
			Assert.IsTrue(types.Any(item => item.Name == "ConcreteAbstractA"));			
			Assert.IsTrue(types.Any(item => item.Name == "ConcreteInterfaceA"));
			Assert.IsTrue(types.Any(item => item.Name == "AbstractA"));
			Assert.IsTrue(types.Any(item => item.Name == "InterfaceA"));
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Kusnadi.Utils
{
	public class TypeLoader
	{
		private readonly List<Type> _types;
		public TypeLoader() 
		{
			_types = new List<Type>();
		}

		public void LoadFromCurrentAppDomain()
		{
			LoadFromAppDomain(AppDomain.CurrentDomain);
		}

		public void LoadFromAppDomain(_AppDomain appDomain)
		{
			if (appDomain == null)
				throw new ArgumentNullException("appDomain");

			_types.AddRange(appDomain
				.GetAssemblies()
				.SelectMany(item => item.GetTypes()));
		}

		public void LoadFromAssembly(_Assembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			_types.AddRange(assembly.GetTypes());			
		}


		public void LoadFromCurrentPath(SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			LoadFromAbsolutePath(currentPath, searchOption);
		}

		public void LoadFromRelativePath(string relativePath, SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string absolutePath = Path.Combine(currentPath, relativePath);

			LoadFromAbsolutePath(absolutePath, searchOption);
		}

		public void LoadFromAbsolutePath(string absolutePath, SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			if (string.IsNullOrEmpty(absolutePath))
				throw new ArgumentException("absolutePath is null or empty");

			string assemblyFileFilter = "*.exe|*.dll";

			foreach (string assemblyFileName in GetFilesWithMultipleFilter(absolutePath, assemblyFileFilter, searchOption))
			{
				try
				{
					var types = Assembly.LoadFrom(assemblyFileName).GetTypes();
					_types.AddRange(types);
				}
				catch
				{
				}
			}
		}

		private static string[] GetFilesWithMultipleFilter(string path, string filters, SearchOption searchOption)
		{
			return filters
				.Split('|')
				.SelectMany(singleFilter => Directory.GetFiles(path, singleFilter, searchOption))
				.ToArray();
		}

		public IEnumerable<Type> Types
		{
			get
			{
				return _types;
			}
		}

		public IEnumerable<Type> GetTypesWithAttribute<T>(bool includeNonInstantiableType = false) where T : Attribute
		{
			return 				
				_types
				.Where(item =>
				{					
					bool isInstantiableType = item.IsClass && !item.IsAbstract;

					bool hasTheAttribute = item.GetCustomAttributes(typeof(T), true).Length > 0;
					bool hasInterfacesWithTheAttribute = item
							.GetInterfaces()
							.Any(interfaceItem =>
								interfaceItem
								.GetCustomAttributes(typeof(T), true)
								.Length > 0);

					return
						(hasTheAttribute || hasInterfacesWithTheAttribute)
						&&
						(includeNonInstantiableType || isInstantiableType );
				});
		}		

		public IEnumerable<Type> GetTypesImplementingType<T>(bool includeTypeIfInstantiable = false) where T : class
		{
			Type type = typeof(T);

			return _types
				.Where(item => 
					type.IsAssignableFrom(item) 
					&& (!item.Equals(type) || (includeTypeIfInstantiable && type.IsClass && !type.IsAbstract)));
		}
	}
}

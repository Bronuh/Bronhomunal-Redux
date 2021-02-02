using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bronuh
{
	static class InterfaceExecutor
	{
		public static void Execute(string interfaceName, string methodName)
		{
			List<Type> types = new List<Type>(Assembly.GetExecutingAssembly().GetTypes());
			foreach (Type type in types)
			{
				List<Type> interfaces = new List<Type>(type.GetInterfaces());
				if (interfaces.Find(i => i.Name == interfaceName) != null)
				{
					type.GetMethod(methodName).Invoke(Activator.CreateInstance(type), new object[] { });
				}
			}
		}


		public static void Execute(Type interfaceType, string methodName)
		{
			List<Type> types = new List<Type>(Assembly.GetExecutingAssembly().GetTypes());
			foreach (Type type in types)
			{
				List<Type> interfaces = new List<Type>(type.GetInterfaces());
				if (interfaces.Find(i => i == interfaceType) != null)
				{
					type.GetMethod(methodName).Invoke(Activator.CreateInstance(type), new object[] { });
				}
			}
		}
	}
}

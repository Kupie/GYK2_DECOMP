using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LazyBearTechnology
{
	// Token: 0x02000182 RID: 386
	public static class LazyReflection
	{
		// Token: 0x06000892 RID: 2194 RVA: 0x0002A1E8 File Offset: 0x000283E8
		public static List<Type> GetAllTypes()
		{
			List<Type> list = new List<Type>();
			foreach (Assembly assembly in from a in AppDomain.CurrentDomain.GetAssemblies()
				where a.FullName.StartsWith("Assembly-CSharp")
				select a)
			{
				list.AddRange(assembly.GetTypes());
			}
			return list;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0002A26C File Offset: 0x0002846C
		public static List<Type> GetAllDerivedClasses<T>() where T : class
		{
			Type baseType = typeof(T);
			return (from t in LazyReflection.GetAllTypes()
				where t != baseType && baseType.IsAssignableFrom(t)
				select t).ToList<Type>();
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0002A2AC File Offset: 0x000284AC
		public static List<MethodInfo> GetAllMethodsWithAttribute<T>(BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) where T : Attribute
		{
			List<MethodInfo> list = new List<MethodInfo>();
			foreach (Type type in LazyReflection.GetAllTypes())
			{
				foreach (MethodInfo methodInfo in type.GetMethods(flags))
				{
					if (methodInfo.GetCustomAttributes(typeof(LazyUITestAttribute), false).Any<object>())
					{
						list.Add(methodInfo);
					}
				}
			}
			return list;
		}
	}
}

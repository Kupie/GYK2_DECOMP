using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x02000753 RID: 1875
public static class InterceptorCache
{
	// Token: 0x060030B9 RID: 12473 RVA: 0x000E8920 File Offset: 0x000E6B20
	static InterceptorCache()
	{
		Type[] types = Assembly.GetExecutingAssembly().GetTypes();
		for (int i = 0; i < types.Length; i++)
		{
			foreach (MethodInfo methodInfo in types[i].GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				NetworkMethodAttribute customAttribute = methodInfo.GetCustomAttribute<NetworkMethodAttribute>();
				if (customAttribute != null)
				{
					MethodInfo method = customAttribute.TargetType.GetMethod(customAttribute.MethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (method != null)
					{
						Type[] baseGenericTypeArguments = InterceptorCache.GetBaseGenericTypeArguments(customAttribute.TargetType, typeof(CommandTargeted<>));
						if (baseGenericTypeArguments.Length > 1)
						{
							throw new ArgumentException(string.Format("Unsupported having more than 1 generic arguments for type [{0}]", customAttribute.TargetType));
						}
						Type type = ((baseGenericTypeArguments.Length == 1) ? baseGenericTypeArguments[0] : null);
						InterceptorCache.cache[methodInfo] = new InterceptorInfo(customAttribute.TargetType, method, type);
					}
				}
			}
		}
	}

	// Token: 0x060030BA RID: 12474 RVA: 0x000E8A10 File Offset: 0x000E6C10
	public static InterceptorInfo GetInterceptorInfo(MethodInfo method)
	{
		InterceptorInfo interceptorInfo;
		if (!InterceptorCache.cache.TryGetValue(method, out interceptorInfo))
		{
			return null;
		}
		return interceptorInfo;
	}

	// Token: 0x060030BB RID: 12475 RVA: 0x000E8A30 File Offset: 0x000E6C30
	private static Type[] GetBaseGenericTypeArguments(Type derivedType, Type baseGenericType)
	{
		while (derivedType != null && derivedType != typeof(object))
		{
			if ((derivedType.IsGenericType ? derivedType.GetGenericTypeDefinition() : derivedType) == baseGenericType)
			{
				return derivedType.GetGenericArguments();
			}
			derivedType = derivedType.BaseType;
		}
		return null;
	}

	// Token: 0x04002759 RID: 10073
	private static readonly Dictionary<MethodInfo, InterceptorInfo> cache = new Dictionary<MethodInfo, InterceptorInfo>();
}

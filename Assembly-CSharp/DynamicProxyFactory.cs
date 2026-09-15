using System;
using Castle.DynamicProxy;

// Token: 0x02000752 RID: 1874
public static class DynamicProxyFactory
{
	// Token: 0x060030B6 RID: 12470 RVA: 0x000E88B4 File Offset: 0x000E6AB4
	static DynamicProxyFactory()
	{
		IInterceptor[] array = new NetworkInterceptor[]
		{
			new NetworkInterceptor()
		};
		DynamicProxyFactory.defaultInterceptors = array;
	}

	// Token: 0x060030B7 RID: 12471 RVA: 0x000E88E0 File Offset: 0x000E6AE0
	public static T CreateClass<T>(T targetClassCreateFrom, params IInterceptor[] interceptors) where T : class
	{
		return DynamicProxyFactory.proxyGenerator.CreateClassProxyWithTarget<T>(targetClassCreateFrom, (interceptors.Length != 0) ? interceptors : DynamicProxyFactory.defaultInterceptors);
	}

	// Token: 0x060030B8 RID: 12472 RVA: 0x000E88F9 File Offset: 0x000E6AF9
	public static TProxy CreateClassTargeted<TClass, TProxy>(TClass targetClassCreateFrom, params IInterceptor[] interceptors) where TClass : class where TProxy : class, IProxyClassTargeted<TClass>, TClass
	{
		TProxy tproxy = DynamicProxyFactory.CreateClass<TProxy>(targetClassCreateFrom as TProxy, interceptors);
		tproxy.Target = targetClassCreateFrom;
		return tproxy;
	}

	// Token: 0x04002757 RID: 10071
	private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();

	// Token: 0x04002758 RID: 10072
	private static readonly IInterceptor[] defaultInterceptors;
}

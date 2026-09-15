using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using LinqTools;

// Token: 0x02000755 RID: 1877
public class NetworkInterceptor : IInterceptor
{
	// Token: 0x060030C1 RID: 12481 RVA: 0x000E8ACB File Offset: 0x000E6CCB
	public void SetContextData(params object[] contextData)
	{
		this.contextData = contextData.ToList<object>();
	}

	// Token: 0x060030C2 RID: 12482 RVA: 0x000E8ADC File Offset: 0x000E6CDC
	public void Intercept(IInvocation invocation)
	{
		InterceptorInfo interceptorInfo = InterceptorCache.GetInterceptorInfo(invocation.MethodInvocationTarget ?? invocation.Method);
		if (interceptorInfo != null)
		{
			object obj = ((interceptorInfo.BaseGenericArgumentType != null) ? Activator.CreateInstance(interceptorInfo.TargetType, new object[] { invocation.InvocationTarget }) : Activator.CreateInstance(interceptorInfo.TargetType));
			int num = interceptorInfo.TargetMethod.GetParameters().Length - invocation.Arguments.Length;
			object[] array = new object[invocation.Arguments.Length + num];
			Array.Copy(invocation.Arguments, 0, array, 0, invocation.Arguments.Length);
			for (int i = 0; i < this.contextData.Count; i++)
			{
				int num2 = invocation.Arguments.Length + i;
				array[num2] = this.contextData[i];
			}
			interceptorInfo.TargetMethod.Invoke(obj, array);
			return;
		}
		invocation.Proceed();
	}

	// Token: 0x0400275D RID: 10077
	private List<object> contextData = new List<object>();
}

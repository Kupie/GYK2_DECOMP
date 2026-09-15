using System;

// Token: 0x02000771 RID: 1905
[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class NetworkMethodAttribute : Attribute
{
	// Token: 0x17000797 RID: 1943
	// (get) Token: 0x06003185 RID: 12677 RVA: 0x000EA8E9 File Offset: 0x000E8AE9
	public Type TargetType { get; }

	// Token: 0x17000798 RID: 1944
	// (get) Token: 0x06003186 RID: 12678 RVA: 0x000EA8F1 File Offset: 0x000E8AF1
	public string MethodName { get; }

	// Token: 0x17000799 RID: 1945
	// (get) Token: 0x06003187 RID: 12679 RVA: 0x000EA8F9 File Offset: 0x000E8AF9
	public object[] Parameters { get; }

	// Token: 0x06003188 RID: 12680 RVA: 0x000EA901 File Offset: 0x000E8B01
	public NetworkMethodAttribute(Type targetType, string methodName, params object[] parameters)
	{
		this.TargetType = targetType;
		this.MethodName = methodName;
		this.Parameters = parameters;
	}
}

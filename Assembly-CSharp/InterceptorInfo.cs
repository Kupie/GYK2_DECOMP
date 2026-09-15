using System;
using System.Reflection;

// Token: 0x02000754 RID: 1876
public class InterceptorInfo
{
	// Token: 0x17000776 RID: 1910
	// (get) Token: 0x060030BC RID: 12476 RVA: 0x000E8A83 File Offset: 0x000E6C83
	public Type TargetType { get; }

	// Token: 0x17000777 RID: 1911
	// (get) Token: 0x060030BD RID: 12477 RVA: 0x000E8A8B File Offset: 0x000E6C8B
	public MethodInfo TargetMethod { get; }

	// Token: 0x17000778 RID: 1912
	// (get) Token: 0x060030BE RID: 12478 RVA: 0x000E8A93 File Offset: 0x000E6C93
	public Type BaseGenericArgumentType { get; }

	// Token: 0x060030BF RID: 12479 RVA: 0x000E8A9B File Offset: 0x000E6C9B
	public InterceptorInfo(Type targetType, MethodInfo targetMethod, Type baseGenericArgumentType)
	{
		this.TargetType = targetType;
		this.TargetMethod = targetMethod;
		this.BaseGenericArgumentType = baseGenericArgumentType;
	}
}

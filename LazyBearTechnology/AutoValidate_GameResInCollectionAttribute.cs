using System;
using System.Diagnostics;

namespace LazyBearTechnology
{
	// Token: 0x020000E9 RID: 233
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	[Conditional("UNITY_EDITOR")]
	[Conditional("BALANCE_PARSER")]
	public class AutoValidate_GameResInCollectionAttribute : AutoValidateAttribute
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x00016C59 File Offset: 0x00014E59
		public Type TypeOfBalanceBaseObject { get; }

		// Token: 0x06000427 RID: 1063 RVA: 0x00016C61 File Offset: 0x00014E61
		public AutoValidate_GameResInCollectionAttribute(Type typeOfBalanceBaseObject)
		{
			this.TypeOfBalanceBaseObject = typeOfBalanceBaseObject;
		}
	}
}

using System;
using System.Diagnostics;

namespace LazyBearTechnology
{
	// Token: 0x020000EA RID: 234
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	[Conditional("UNITY_EDITOR")]
	[Conditional("BALANCE_PARSER")]
	public class AutoValidate_IdInCollection : AutoValidateAttribute
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x00016C70 File Offset: 0x00014E70
		public Type TypeOfBalanceBaseObject { get; }

		// Token: 0x06000429 RID: 1065 RVA: 0x00016C78 File Offset: 0x00014E78
		public AutoValidate_IdInCollection(Type typeOfBalanceBaseObject)
		{
			this.TypeOfBalanceBaseObject = typeOfBalanceBaseObject;
		}
	}
}

using System;
using System.Diagnostics;

namespace LazyBearTechnology
{
	// Token: 0x020000EB RID: 235
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	[Conditional("UNITY_EDITOR")]
	[Conditional("BALANCE_PARSER")]
	public class AutoValidate_StrListInCollectionAttribute : AutoValidateAttribute
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x00016C87 File Offset: 0x00014E87
		public Type TypeOfBalanceBaseObject { get; }

		// Token: 0x0600042B RID: 1067 RVA: 0x00016C8F File Offset: 0x00014E8F
		public AutoValidate_StrListInCollectionAttribute(Type typeOfBalanceBaseObject)
		{
			this.TypeOfBalanceBaseObject = typeOfBalanceBaseObject;
		}
	}
}

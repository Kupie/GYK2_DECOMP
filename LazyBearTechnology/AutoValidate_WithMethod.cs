using System;
using System.Diagnostics;

namespace LazyBearTechnology
{
	// Token: 0x020000EC RID: 236
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	[Conditional("UNITY_EDITOR")]
	[Conditional("BALANCE_PARSER")]
	public class AutoValidate_WithMethod : AutoValidateAttribute
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x00016C9E File Offset: 0x00014E9E
		public string MethodName { get; }

		// Token: 0x0600042D RID: 1069 RVA: 0x00016CA6 File Offset: 0x00014EA6
		public AutoValidate_WithMethod(string methodName)
		{
			this.MethodName = methodName;
		}
	}
}

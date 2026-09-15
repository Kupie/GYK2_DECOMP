using System;

namespace LazyBearTechnology
{
	// Token: 0x020000ED RID: 237
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class LazyExpressionPureValueTypeAttribute : Attribute
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x00016CB5 File Offset: 0x00014EB5
		public PureValueType PureValueType { get; }

		// Token: 0x0600042F RID: 1071 RVA: 0x00016CBD File Offset: 0x00014EBD
		public LazyExpressionPureValueTypeAttribute(PureValueType pureValueType)
		{
			this.PureValueType = pureValueType;
		}
	}
}

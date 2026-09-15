using System;

namespace LazyBearTechnology
{
	// Token: 0x020000E7 RID: 231
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class AutoParseAttribute : Attribute
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x00016C2B File Offset: 0x00014E2B
		public string Column { get; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x00016C33 File Offset: 0x00014E33
		public object DefaultValue { get; }

		// Token: 0x06000424 RID: 1060 RVA: 0x00016C3B File Offset: 0x00014E3B
		public AutoParseAttribute(string column)
		{
			this.Column = column;
			this.DefaultValue = null;
		}
	}
}

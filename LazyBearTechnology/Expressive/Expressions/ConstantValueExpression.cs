using System;
using System.Collections.Generic;

namespace Expressive.Expressions
{
	// Token: 0x0200009B RID: 155
	internal class ConstantValueExpression : IExpression
	{
		// Token: 0x0600027E RID: 638 RVA: 0x0000E18E File Offset: 0x0000C38E
		internal ConstantValueExpression(object value)
		{
			this.value = value;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000E19D File Offset: 0x0000C39D
		public object Evaluate(IDictionary<string, object> variables)
		{
			return this.value;
		}

		// Token: 0x040000B4 RID: 180
		private readonly object value;
	}
}

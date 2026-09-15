using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x0200007F RID: 127
	internal class TruncateFunction : FunctionBase
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000D647 File Offset: 0x0000B847
		public override string Name
		{
			get
			{
				return "Truncate";
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000D64E File Offset: 0x0000B84E
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Truncate(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}

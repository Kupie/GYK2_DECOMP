using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x02000076 RID: 118
	internal class PIFunction : FunctionBase
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000D22A File Offset: 0x0000B42A
		public override string Name
		{
			get
			{
				return "PI";
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000D231 File Offset: 0x0000B431
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 0, 0);
			return 3.141592653589793;
		}
	}
}

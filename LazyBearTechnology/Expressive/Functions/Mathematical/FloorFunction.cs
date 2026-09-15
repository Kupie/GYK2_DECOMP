using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x02000072 RID: 114
	internal class FloorFunction : FunctionBase
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000D128 File Offset: 0x0000B328
		public override string Name
		{
			get
			{
				return "Floor";
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000D12F File Offset: 0x0000B32F
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Floor(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}

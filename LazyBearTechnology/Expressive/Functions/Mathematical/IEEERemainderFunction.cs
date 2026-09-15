using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x02000073 RID: 115
	internal class IEEERemainderFunction : FunctionBase
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000D15F File Offset: 0x0000B35F
		public override string Name
		{
			get
			{
				return "IEEERemainder";
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000D166 File Offset: 0x0000B366
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 2, 2);
			return Math.IEEERemainder(Convert.ToDouble(parameters[0].Evaluate(base.Variables)), Convert.ToDouble(parameters[1].Evaluate(base.Variables)));
		}
	}
}

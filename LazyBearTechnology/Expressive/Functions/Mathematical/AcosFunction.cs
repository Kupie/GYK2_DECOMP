using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x0200006A RID: 106
	internal class AcosFunction : FunctionBase
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000CF0E File Offset: 0x0000B10E
		public override string Name
		{
			get
			{
				return "Acos";
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000CF15 File Offset: 0x0000B115
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Acos(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}

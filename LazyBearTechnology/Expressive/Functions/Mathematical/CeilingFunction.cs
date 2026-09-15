using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x0200006D RID: 109
	internal class CeilingFunction : FunctionBase
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000CFB3 File Offset: 0x0000B1B3
		public override string Name
		{
			get
			{
				return "Ceiling";
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000CFBC File Offset: 0x0000B1BC
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj is double)
			{
				return Math.Ceiling((double)obj);
			}
			if (obj is decimal)
			{
				return Math.Ceiling((decimal)obj);
			}
			return Math.Ceiling(Convert.ToDouble(obj));
		}
	}
}

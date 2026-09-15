using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x02000078 RID: 120
	internal class RandomFunction : FunctionBase
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000D29C File Offset: 0x0000B49C
		public override string Name
		{
			get
			{
				return "Random";
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000D2A4 File Offset: 0x0000B4A4
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 2, 2);
			object obj = parameters[0].Evaluate(base.Variables);
			object obj2 = parameters[1].Evaluate(base.Variables);
			Random random = new Random(DateTime.UtcNow.Millisecond);
			if (obj is int && obj2 is int)
			{
				return random.Next((int)obj, (int)obj2);
			}
			if (obj is double || obj2 is double)
			{
				double num = random.NextDouble();
				double num2 = Convert.ToDouble(obj);
				double num3 = Convert.ToDouble(obj2) - num2;
				return num2 + num3 * num;
			}
			return null;
		}
	}
}

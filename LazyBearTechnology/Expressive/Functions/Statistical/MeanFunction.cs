using System;
using System.Collections;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Helpers;

namespace Expressive.Functions.Statistical
{
	// Token: 0x02000064 RID: 100
	internal class MeanFunction : FunctionBase
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000C82B File Offset: 0x0000AA2B
		public override string Name
		{
			get
			{
				return "Mean";
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000C832 File Offset: 0x0000AA32
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, -1, 1);
			return MeanFunction.Evaluate(parameters, base.Variables);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000C84C File Offset: 0x0000AA4C
		internal static object Evaluate(IExpression[] parameters, IDictionary<string, object> variables)
		{
			int num = 0;
			object obj = 0;
			int i = 0;
			while (i < parameters.Length)
			{
				IExpression expression = parameters[i];
				int num2 = 1;
				object obj2 = expression.Evaluate(variables);
				IEnumerable enumerable = obj2 as IEnumerable;
				if (enumerable != null)
				{
					int num3 = 0;
					object obj3 = 0;
					foreach (object obj4 in enumerable)
					{
						if (obj4 != null)
						{
							num3++;
							obj3 = Numbers.Add(obj3, obj4);
						}
					}
					num2 = num3;
					obj2 = obj3;
					goto IL_008F;
				}
				if (obj2 != null)
				{
					goto IL_008F;
				}
				IL_009D:
				i++;
				continue;
				IL_008F:
				obj = Numbers.Add(obj, obj2);
				num += num2;
				goto IL_009D;
			}
			return Convert.ToDouble(obj) / (double)num;
		}
	}
}

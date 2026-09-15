using System;
using System.Collections;
using Expressive.Expressions;
using Expressive.Helpers;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x0200007D RID: 125
	internal class SumFunction : FunctionBase
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0000D524 File Offset: 0x0000B724
		public override string Name
		{
			get
			{
				return "Sum";
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000D52C File Offset: 0x0000B72C
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, -1, 1);
			object obj = 0;
			for (int i = 0; i < parameters.Length; i++)
			{
				object obj2 = parameters[i].Evaluate(base.Variables);
				IEnumerable enumerable = obj2 as IEnumerable;
				if (enumerable != null)
				{
					object obj3 = 0;
					foreach (object obj4 in enumerable)
					{
						obj3 = Numbers.Add(obj3 ?? 0, obj4 ?? 0);
					}
					obj2 = obj3;
				}
				obj = Numbers.Add(obj ?? 0, obj2 ?? 0);
			}
			return obj;
		}
	}
}

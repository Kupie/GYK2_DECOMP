using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Expressive.Expressions;
using Expressive.Helpers;

namespace Expressive.Functions.Logical
{
	// Token: 0x02000081 RID: 129
	internal class InFunction : FunctionBase
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000D6CA File Offset: 0x0000B8CA
		public override string Name
		{
			get
			{
				return "In";
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000D6D4 File Offset: 0x0000B8D4
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, -1, 2);
			bool flag = false;
			object parameter = parameters[0].Evaluate(base.Variables);
			Func<object, bool> <>9__0;
			for (int i = 1; i < parameters.Length; i++)
			{
				object obj = parameters[i].Evaluate(base.Variables);
				ICollection collection = obj as ICollection;
				if (collection != null)
				{
					IEnumerable<object> enumerable = collection.Cast<object>();
					Func<object, bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = (object innerValue) => Comparison.CompareUsingMostPreciseType(parameter, innerValue, context) == 0);
					}
					if (enumerable.Any(func))
					{
						flag = true;
						break;
					}
				}
				else
				{
					flag = Comparison.CompareUsingMostPreciseType(parameter, obj, context) == 0;
					if (flag)
					{
						break;
					}
				}
			}
			return flag;
		}
	}
}

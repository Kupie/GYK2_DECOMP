using System;
using System.Collections;
using System.Linq;
using Expressive.Expressions;
using Expressive.Helpers;

namespace Expressive.Functions.Relational
{
	// Token: 0x02000067 RID: 103
	internal class MaxFunction : FunctionBase
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000CB90 File Offset: 0x0000AD90
		public override string Name
		{
			get
			{
				return "Max";
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000CB98 File Offset: 0x0000AD98
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, -1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			IEnumerable enumerable = obj as IEnumerable;
			if (enumerable != null)
			{
				obj = MaxFunction.Max(enumerable, context);
			}
			if (obj == null)
			{
				return null;
			}
			foreach (IExpression expression in parameters.Skip(1))
			{
				object obj2 = expression.Evaluate(base.Variables);
				IEnumerable enumerable2 = obj2 as IEnumerable;
				if (enumerable2 != null)
				{
					obj2 = MaxFunction.Max(enumerable2, context);
				}
				if (obj2 == null)
				{
					return null;
				}
				obj = ((Comparison.CompareUsingMostPreciseType(obj, obj2, context) > 0) ? obj : obj2);
			}
			return obj;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000CC4C File Offset: 0x0000AE4C
		private static object Max(IEnumerable enumerable, Context context)
		{
			object obj = null;
			foreach (object obj2 in enumerable)
			{
				if (obj2 == null)
				{
					return null;
				}
				if (obj == null)
				{
					obj = obj2;
				}
				else
				{
					obj = ((Comparison.CompareUsingMostPreciseType(obj, obj2, context) > 0) ? obj : obj2);
				}
			}
			return obj;
		}
	}
}

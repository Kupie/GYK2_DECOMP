using System;
using System.Collections;
using System.Linq;
using Expressive.Expressions;
using Expressive.Helpers;

namespace Expressive.Functions.Relational
{
	// Token: 0x02000068 RID: 104
	internal class MinFunction : FunctionBase
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000CCC4 File Offset: 0x0000AEC4
		public override string Name
		{
			get
			{
				return "Min";
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000CCCC File Offset: 0x0000AECC
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, -1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			IEnumerable enumerable = obj as IEnumerable;
			if (enumerable != null)
			{
				obj = MinFunction.Min(enumerable, context);
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
					obj2 = MinFunction.Min(enumerable2, context);
				}
				obj = ((Comparison.CompareUsingMostPreciseType(obj, obj2, context) < 0) ? obj : obj2);
				if (obj == null)
				{
					return null;
				}
			}
			return obj;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000CD80 File Offset: 0x0000AF80
		private static object Min(IEnumerable enumerable, Context context)
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
					obj = ((Comparison.CompareUsingMostPreciseType(obj, obj2, context) < 0) ? obj : obj2);
				}
			}
			return obj;
		}
	}
}

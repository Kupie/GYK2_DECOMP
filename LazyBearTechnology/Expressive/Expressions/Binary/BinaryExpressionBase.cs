using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Expressive.Exceptions;

namespace Expressive.Expressions.Binary
{
	// Token: 0x020000A5 RID: 165
	public abstract class BinaryExpressionBase : IExpression
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000E344 File Offset: 0x0000C544
		protected Context Context { get; }

		// Token: 0x06000294 RID: 660 RVA: 0x0000E34C File Offset: 0x0000C54C
		protected BinaryExpressionBase(IExpression lhs, IExpression rhs, Context context)
		{
			this.leftHandSide = lhs;
			this.Context = context;
			this.rightHandSide = rhs;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000E36C File Offset: 0x0000C56C
		public object Evaluate(IDictionary<string, object> variables)
		{
			if (this.leftHandSide == null)
			{
				throw new MissingParticipantException("The left hand side of the operation is missing.");
			}
			if (this.rightHandSide == null)
			{
				throw new MissingParticipantException("The right hand side of the operation is missing.");
			}
			object obj = this.leftHandSide.Evaluate(variables);
			return this.EvaluateImpl(obj, this.rightHandSide, variables);
		}

		// Token: 0x06000296 RID: 662
		protected abstract object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables);

		// Token: 0x06000297 RID: 663 RVA: 0x0000E3BC File Offset: 0x0000C5BC
		public static object EvaluateAggregates(object lhsResult, IExpression rhs, IDictionary<string, object> variables, Func<object, object, object> resultSelector)
		{
			if (rhs == null)
			{
				throw new ArgumentNullException("rhs");
			}
			if (resultSelector == null)
			{
				throw new ArgumentNullException("resultSelector");
			}
			IList<object> list = new List<object>();
			IList<object> list2 = new List<object>();
			object obj = rhs.Evaluate(variables);
			if (!(lhsResult is ICollection) && !(obj is ICollection))
			{
				return resultSelector(lhsResult, obj);
			}
			ICollection collection = lhsResult as ICollection;
			if (collection != null)
			{
				foreach (object obj2 in collection)
				{
					list.Add(obj2);
				}
			}
			ICollection collection2 = obj as ICollection;
			if (collection2 != null)
			{
				foreach (object obj3 in collection2)
				{
					list2.Add(obj3);
				}
			}
			object[] array = null;
			if (list.Count == list2.Count)
			{
				IList<object> list3 = new List<object>();
				for (int i = 0; i < list.Count; i++)
				{
					list3.Add(resultSelector(list[i], list2[i]));
				}
				array = list3.ToArray<object>();
			}
			else if (list.Count == 0)
			{
				IList<object> list4 = new List<object>();
				for (int j = 0; j < list2.Count; j++)
				{
					list4.Add(resultSelector(lhsResult, list2[j]));
				}
				array = list4.ToArray<object>();
			}
			else if (list2.Count == 0)
			{
				IList<object> list5 = new List<object>();
				for (int k = 0; k < list.Count; k++)
				{
					list5.Add(resultSelector(list[k], obj));
				}
				array = list5.ToArray<object>();
			}
			return array;
		}

		// Token: 0x040000BB RID: 187
		private readonly IExpression leftHandSide;

		// Token: 0x040000BC RID: 188
		private readonly IExpression rightHandSide;
	}
}

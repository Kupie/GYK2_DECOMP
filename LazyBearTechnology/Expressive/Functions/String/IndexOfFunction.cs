using System;
using System.Collections;
using Expressive.Expressions;

namespace Expressive.Functions.String
{
	// Token: 0x0200005C RID: 92
	internal class IndexOfFunction : FunctionBase
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000C32C File Offset: 0x0000A52C
		public override string Name
		{
			get
			{
				return "IndexOf";
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000C334 File Offset: 0x0000A534
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, -1, 2);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			if (obj is string)
			{
				string text = obj.ToString();
				obj = parameters[1].Evaluate(base.Variables);
				if (obj == null)
				{
					return null;
				}
				string text2 = obj.ToString();
				if (parameters.Length > 2)
				{
					int num = Convert.ToInt32(parameters[2].Evaluate(base.Variables));
					return text.IndexOf(text2, num, context.EqualityStringComparison);
				}
				return text.IndexOf(text2, context.EqualityStringComparison);
			}
			else
			{
				IEnumerable enumerable = obj as IEnumerable;
				if (enumerable != null)
				{
					int num2 = 0;
					obj = parameters[1].Evaluate(base.Variables);
					foreach (object obj2 in enumerable)
					{
						if (obj2 is IExpression)
						{
							obj2 = (obj2 as IExpression).Evaluate(base.Variables);
						}
						if (obj2 != null)
						{
							string text3 = obj2 as string;
							if (text3 != null)
							{
								string text4 = obj as string;
								if (text4 != null && text3.Equals(text4, context.EqualityStringComparison))
								{
									return num2;
								}
							}
							else if (obj.Equals(obj2))
							{
								return num2;
							}
						}
						num2++;
					}
					return -1;
				}
				if (!(obj is IComparable))
				{
					return -1;
				}
				IComparable comparable = obj as IComparable;
				obj = parameters[1].Evaluate(base.Variables);
				if (comparable.CompareTo(obj) == 0)
				{
					return 0;
				}
				return -1;
			}
		}
	}
}

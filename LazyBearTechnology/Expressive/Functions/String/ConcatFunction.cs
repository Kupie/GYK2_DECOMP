using System;
using System.Collections;
using System.Text;
using Expressive.Expressions;

namespace Expressive.Functions.String
{
	// Token: 0x02000059 RID: 89
	internal class ConcatFunction : FunctionBase
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000C162 File Offset: 0x0000A362
		public override string Name
		{
			get
			{
				return "Concat";
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000C16C File Offset: 0x0000A36C
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, -1, 1);
			StringBuilder stringBuilder = new StringBuilder();
			this.Evaluate(stringBuilder, parameters, context);
			return stringBuilder.ToString();
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000C198 File Offset: 0x0000A398
		protected virtual void Evaluate(StringBuilder sb, IEnumerable parameters, Context context)
		{
			foreach (object obj in parameters)
			{
				IExpression expression = obj as IExpression;
				object obj2;
				if (expression != null)
				{
					obj2 = expression.Evaluate(base.Variables);
				}
				else
				{
					obj2 = obj;
				}
				if (obj2 != null)
				{
					if (obj2 is string)
					{
						sb.Append(obj2);
					}
					else
					{
						IEnumerable enumerable = obj2 as IEnumerable;
						if (enumerable != null)
						{
							this.Evaluate(sb, enumerable, context);
						}
						else
						{
							string text = obj2.ToString();
							sb.Append(text);
						}
					}
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;

namespace Expressive.Expressions.Unary.Logical
{
	// Token: 0x020000A2 RID: 162
	internal class NotExpression : UnaryExpressionBase
	{
		// Token: 0x0600028D RID: 653 RVA: 0x0000E2AD File Offset: 0x0000C4AD
		public NotExpression(IExpression expression)
			: base(expression)
		{
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000E2B8 File Offset: 0x0000C4B8
		public override object Evaluate(IDictionary<string, object> variables)
		{
			object obj = this.expression.Evaluate(variables);
			if (obj == null)
			{
				return null;
			}
			if (obj is bool)
			{
				bool flag = (bool)obj;
				return !flag;
			}
			return !Convert.ToBoolean(obj);
		}
	}
}

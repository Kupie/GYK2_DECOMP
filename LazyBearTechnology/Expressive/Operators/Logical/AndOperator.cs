using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Logical;

namespace Expressive.Operators.Logical
{
	// Token: 0x02000047 RID: 71
	internal class AndOperator : OperatorBase
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000781F File Offset: 0x00005A1F
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "&&", "and" };
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00007837 File Offset: 0x00005A37
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new AndExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007845 File Offset: 0x00005A45
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.And;
		}
	}
}

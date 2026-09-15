using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Multiplicative;

namespace Expressive.Operators.Multiplicative
{
	// Token: 0x02000045 RID: 69
	internal class ModulusOperator : OperatorBase
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000158 RID: 344 RVA: 0x000077BB File Offset: 0x000059BB
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "%", "mod" };
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000077D3 File Offset: 0x000059D3
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new ModulusExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000077E1 File Offset: 0x000059E1
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.Modulus;
		}
	}
}

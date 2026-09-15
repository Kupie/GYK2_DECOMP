using System;
using System.Collections.Generic;

namespace Expressive.Expressions
{
	// Token: 0x0200009D RID: 157
	public interface IExpression
	{
		// Token: 0x06000282 RID: 642
		object Evaluate(IDictionary<string, object> variables);
	}
}

using System;
using System.Collections.Generic;
using Expressive.Expressions;

namespace Expressive.Functions
{
	// Token: 0x02000058 RID: 88
	public interface IFunction
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001AE RID: 430
		// (set) Token: 0x060001AF RID: 431
		IDictionary<string, object> Variables { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001B0 RID: 432
		string Name { get; }

		// Token: 0x060001B1 RID: 433
		object Evaluate(IExpression[] parameters, Context context);
	}
}

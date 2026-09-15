using System;
using System.Collections.Generic;

namespace Expressive.Expressions
{
	// Token: 0x0200009C RID: 156
	internal class FunctionExpression : IExpression
	{
		// Token: 0x06000280 RID: 640 RVA: 0x0000E1A5 File Offset: 0x0000C3A5
		internal FunctionExpression(string name, Func<IExpression[], IDictionary<string, object>, object> function, IExpression[] parameters)
		{
			this.name = name;
			this.function = function;
			this.parameters = parameters;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000E1C2 File Offset: 0x0000C3C2
		public object Evaluate(IDictionary<string, object> variables)
		{
			return this.function(this.parameters, variables);
		}

		// Token: 0x040000B5 RID: 181
		private readonly Func<IExpression[], IDictionary<string, object>, object> function;

		// Token: 0x040000B6 RID: 182
		private readonly string name;

		// Token: 0x040000B7 RID: 183
		private readonly IExpression[] parameters;
	}
}

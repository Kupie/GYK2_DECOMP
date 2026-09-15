using System;
using System.Collections.Generic;

namespace Expressive.Expressions
{
	// Token: 0x0200009F RID: 159
	internal class VariableExpression : IExpression
	{
		// Token: 0x06000285 RID: 645 RVA: 0x0000E206 File Offset: 0x0000C406
		internal VariableExpression(string variableName)
		{
			this.variableName = variableName;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000E218 File Offset: 0x0000C418
		public object Evaluate(IDictionary<string, object> variables)
		{
			object obj;
			if (variables == null || !variables.TryGetValue(this.variableName, out obj))
			{
				throw new ArgumentException("The variable '" + this.variableName + "' has not been supplied.");
			}
			IExpression expression = obj as IExpression;
			if (expression != null)
			{
				return expression.Evaluate(variables);
			}
			return obj;
		}

		// Token: 0x040000B9 RID: 185
		private readonly string variableName;
	}
}

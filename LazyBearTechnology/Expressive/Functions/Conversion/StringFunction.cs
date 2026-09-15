using System;
using Expressive.Expressions;

namespace Expressive.Functions.Conversion
{
	// Token: 0x0200009A RID: 154
	internal sealed class StringFunction : FunctionBase
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000E113 File Offset: 0x0000C313
		public override string Name
		{
			get
			{
				return "String";
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000E11C File Offset: 0x0000C31C
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, -1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			if (parameters.Length > 1)
			{
				string text = parameters[1].Evaluate(base.Variables) as string;
				if (text != null)
				{
					return string.Format(context.CurrentCulture, "{0:" + text + "}", obj);
				}
			}
			return obj.ToString();
		}
	}
}

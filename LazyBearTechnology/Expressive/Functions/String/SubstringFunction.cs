using System;
using Expressive.Expressions;

namespace Expressive.Functions.String
{
	// Token: 0x02000062 RID: 98
	internal class SubstringFunction : FunctionBase
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001CE RID: 462 RVA: 0x0000C77C File Offset: 0x0000A97C
		public override string Name
		{
			get
			{
				return "Substring";
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000C784 File Offset: 0x0000A984
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 3, 3);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			string text;
			if (obj is string)
			{
				text = (string)obj;
			}
			else
			{
				text = obj.ToString();
			}
			int num = (int)parameters[1].Evaluate(base.Variables);
			int num2 = (int)parameters[2].Evaluate(base.Variables);
			if (text == null)
			{
				return null;
			}
			return text.Substring(num, num2);
		}
	}
}

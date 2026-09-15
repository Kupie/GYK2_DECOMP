using System;
using System.Text.RegularExpressions;
using Expressive.Expressions;

namespace Expressive.Functions.String
{
	// Token: 0x02000060 RID: 96
	internal class RegexFunction : FunctionBase
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000C6BE File Offset: 0x0000A8BE
		public override string Name
		{
			get
			{
				return "Regex";
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000C6C5 File Offset: 0x0000A8C5
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 2, 2);
			return new Regex(parameters[1].Evaluate(base.Variables) as string).IsMatch(parameters[0].Evaluate(base.Variables) as string);
		}
	}
}

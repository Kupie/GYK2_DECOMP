using System;
using Expressive.Expressions;

namespace Expressive.Functions.String
{
	// Token: 0x0200005F RID: 95
	internal class PadRightFunction : FunctionBase
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000C606 File Offset: 0x0000A806
		public override string Name
		{
			get
			{
				return "PadRight";
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000C610 File Offset: 0x0000A810
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 3, 3);
			object obj = parameters[0].Evaluate(base.Variables);
			object obj2 = parameters[1].Evaluate(base.Variables);
			if (obj == null || obj2 == null)
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
			int num = Convert.ToInt32(obj2);
			object obj3 = parameters[2].Evaluate(base.Variables);
			char c = ' ';
			if (obj3 is char)
			{
				c = (char)obj3;
			}
			else if (obj3 is string)
			{
				c = ((string)obj3)[0];
			}
			return text.PadRight(num, c);
		}
	}
}

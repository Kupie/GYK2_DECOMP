using System;
using Expressive.Expressions;

namespace Expressive.Functions.String
{
	// Token: 0x0200005E RID: 94
	internal class PadLeftFunction : FunctionBase
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000C550 File Offset: 0x0000A750
		public override string Name
		{
			get
			{
				return "PadLeft";
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000C558 File Offset: 0x0000A758
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
			return text.PadLeft(num, c);
		}
	}
}

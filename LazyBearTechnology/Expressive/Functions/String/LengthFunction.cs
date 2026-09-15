using System;
using Expressive.Expressions;

namespace Expressive.Functions.String
{
	// Token: 0x0200005D RID: 93
	internal class LengthFunction : FunctionBase
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000C4F0 File Offset: 0x0000A6F0
		public override string Name
		{
			get
			{
				return "Length";
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000C4F8 File Offset: 0x0000A6F8
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			string text = obj as string;
			if (text != null)
			{
				return text.Length;
			}
			return obj.ToString().Length;
		}
	}
}

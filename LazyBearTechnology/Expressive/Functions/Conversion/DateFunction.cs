using System;
using Expressive.Expressions;

namespace Expressive.Functions.Conversion
{
	// Token: 0x02000095 RID: 149
	internal sealed class DateFunction : FunctionBase
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0000DF5F File Offset: 0x0000C15F
		public override string Name
		{
			get
			{
				return "Date";
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000DF68 File Offset: 0x0000C168
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
				string text = obj as string;
				if (text != null)
				{
					string text2 = parameters[1].Evaluate(base.Variables) as string;
					if (text2 != null)
					{
						return DateTime.ParseExact(text, text2, context.CurrentCulture);
					}
				}
			}
			return Convert.ToDateTime(obj, context.CurrentCulture);
		}
	}
}

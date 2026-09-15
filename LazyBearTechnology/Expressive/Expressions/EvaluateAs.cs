using System;
using System.Collections.Generic;

namespace Expressive.Expressions
{
	// Token: 0x020000A0 RID: 160
	public static class EvaluateAs
	{
		// Token: 0x06000287 RID: 647 RVA: 0x0000E266 File Offset: 0x0000C466
		public static string EvaluateAsString(this IExpression e, IDictionary<string, object> variables)
		{
			return Convert.ToString(e.Evaluate(variables));
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000E274 File Offset: 0x0000C474
		public static int EvaluateAsInt(this IExpression e, IDictionary<string, object> variables)
		{
			return Convert.ToInt32(e.Evaluate(variables));
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000E282 File Offset: 0x0000C482
		public static bool EvaluateAsBoolean(this IExpression e, IDictionary<string, object> variables)
		{
			return Convert.ToBoolean(e.Evaluate(variables));
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000E290 File Offset: 0x0000C490
		public static float EvaluateAsFloat(this IExpression e, IDictionary<string, object> variables)
		{
			return Convert.ToSingle(e.Evaluate(variables));
		}
	}
}

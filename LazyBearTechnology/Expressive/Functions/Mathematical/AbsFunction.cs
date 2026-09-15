using System;
using Expressive.Expressions;
using Expressive.Helpers;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x02000069 RID: 105
	internal class AbsFunction : FunctionBase
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000CDF8 File Offset: 0x0000AFF8
		public override string Name
		{
			get
			{
				return "Abs";
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000CE00 File Offset: 0x0000B000
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj != null)
			{
				switch (TypeHelper.GetTypeCode(obj))
				{
				case TypeCode.SByte:
					return Math.Abs(Convert.ToSByte(obj));
				case TypeCode.Int16:
					return Math.Abs(Convert.ToInt16(obj));
				case TypeCode.UInt16:
					return Math.Abs((int)Convert.ToUInt16(obj));
				case TypeCode.Int32:
					return Math.Abs(Convert.ToInt32(obj));
				case TypeCode.UInt32:
					return Math.Abs((long)((ulong)Convert.ToUInt32(obj)));
				case TypeCode.Int64:
					return Math.Abs(Convert.ToInt64(obj));
				case TypeCode.Single:
					return Math.Abs(Convert.ToSingle(obj));
				case TypeCode.Double:
					return Math.Abs(Convert.ToDouble(obj));
				case TypeCode.Decimal:
					return Math.Abs(Convert.ToDecimal(obj));
				}
			}
			return null;
		}
	}
}

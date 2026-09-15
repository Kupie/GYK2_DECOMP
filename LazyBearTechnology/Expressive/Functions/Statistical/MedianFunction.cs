using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Expressive.Expressions;

namespace Expressive.Functions.Statistical
{
	// Token: 0x02000065 RID: 101
	internal class MedianFunction : FunctionBase
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000C92C File Offset: 0x0000AB2C
		public override string Name
		{
			get
			{
				return "Median";
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000C934 File Offset: 0x0000AB34
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, -1, 1);
			IList<decimal> list = new List<decimal>();
			int i = 0;
			while (i < parameters.Length)
			{
				object obj = parameters[i].Evaluate(base.Variables);
				IEnumerable enumerable = obj as IEnumerable;
				if (enumerable != null)
				{
					using (IEnumerator enumerator = enumerable.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							MedianFunction.AddValue(obj2, list);
						}
						goto IL_006F;
					}
					goto IL_0068;
				}
				goto IL_0068;
				IL_006F:
				i++;
				continue;
				IL_0068:
				MedianFunction.AddValue(obj, list);
				goto IL_006F;
			}
			return MedianFunction.Median(list.ToArray<decimal>());
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000C9DC File Offset: 0x0000ABDC
		private static void AddValue(object value, IList<decimal> decimalValues)
		{
			if (value == null)
			{
				return;
			}
			decimalValues.Add(Convert.ToDecimal(value));
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000C9F0 File Offset: 0x0000ABF0
		private static decimal Median(IEnumerable<decimal> xs)
		{
			List<decimal> list = xs.OrderBy((decimal x) => x).ToList<decimal>();
			double num = (double)(list.Count - 1) / 2.0;
			return (list[(int)num] + list[(int)(num + 0.5)]) / 2m;
		}
	}
}

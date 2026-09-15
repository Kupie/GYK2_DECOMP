using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Expressive.Expressions;

namespace Expressive.Functions.Statistical
{
	// Token: 0x02000066 RID: 102
	internal class ModeFunction : FunctionBase
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001DD RID: 477 RVA: 0x0000CA6E File Offset: 0x0000AC6E
		public override string Name
		{
			get
			{
				return "Mode";
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000CA78 File Offset: 0x0000AC78
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, -1, 1);
			IList<object> list = new List<object>();
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
							list.Add(obj2);
						}
						goto IL_007E;
					}
					goto IL_0076;
				}
				goto IL_0076;
				IL_007E:
				i++;
				continue;
				IL_0076:
				list.Add(obj);
				goto IL_007E;
			}
			IEnumerable<IGrouping<object, object>> enumerable2 = from v in list
				group v by v;
			int maxCount = enumerable2.Max((IGrouping<object, object> g) => g.Count<object>());
			return enumerable2.First((IGrouping<object, object> g) => g.Count<object>() == maxCount).Key;
		}
	}
}

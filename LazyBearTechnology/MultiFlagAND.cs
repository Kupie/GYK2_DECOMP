using System;
using System.Collections.Generic;

namespace LazyBearTechnology
{
	// Token: 0x02000188 RID: 392
	public class MultiFlagAND<T> : MultiFlagBase<T> where T : Enum
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x0002A568 File Offset: 0x00028768
		protected override bool DefaultValueForMissingFlag
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0002A56B File Offset: 0x0002876B
		protected override bool IsExceptMatch(bool flagValue)
		{
			return !flagValue;
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x0002A571 File Offset: 0x00028771
		protected override bool ExceptResultOnMatch
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x0002A574 File Offset: 0x00028774
		protected override bool ExceptResultDefault
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0002A577 File Offset: 0x00028777
		protected override bool IsComparedFlagsMatch(bool flagValue)
		{
			return !flagValue;
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x0002A57D File Offset: 0x0002877D
		protected override bool ComparedFlagsResultOnMatch
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x0002A580 File Offset: 0x00028780
		protected override bool ComparedFlagsResultDefault
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0002A584 File Offset: 0x00028784
		protected override void CalcResult()
		{
			bool flag = true;
			foreach (KeyValuePair<T, bool> keyValuePair in this.flags)
			{
				flag &= keyValuePair.Value;
				if (!flag)
				{
					break;
				}
			}
			base.SetResultFlagAndNotify(flag);
		}
	}
}

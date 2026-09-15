using System;
using System.Collections.Generic;

namespace LazyBearTechnology
{
	// Token: 0x0200018A RID: 394
	public class MultiFlagOR<T> : MultiFlagBase<T> where T : Enum
	{
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0002A88D File Offset: 0x00028A8D
		protected override bool DefaultValueForMissingFlag
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0002A890 File Offset: 0x00028A90
		protected override bool IsExceptMatch(bool flagValue)
		{
			return flagValue;
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0002A893 File Offset: 0x00028A93
		protected override bool ExceptResultOnMatch
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x0002A896 File Offset: 0x00028A96
		protected override bool ExceptResultDefault
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0002A899 File Offset: 0x00028A99
		protected override bool IsComparedFlagsMatch(bool flagValue)
		{
			return flagValue;
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x0002A89C File Offset: 0x00028A9C
		protected override bool ComparedFlagsResultOnMatch
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x0002A89F File Offset: 0x00028A9F
		protected override bool ComparedFlagsResultDefault
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0002A8A4 File Offset: 0x00028AA4
		protected override void CalcResult()
		{
			bool flag = false;
			foreach (KeyValuePair<T, bool> keyValuePair in this.flags)
			{
				flag |= keyValuePair.Value;
				if (flag)
				{
					break;
				}
			}
			base.SetResultFlagAndNotify(flag);
		}
	}
}

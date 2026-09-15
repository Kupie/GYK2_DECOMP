using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000F7 RID: 247
	[Serializable]
	public class GameResInt
	{
		// Token: 0x0600047D RID: 1149 RVA: 0x000179E0 File Offset: 0x00015BE0
		public int Get(string id)
		{
			int num = this.ids.IndexOf(id);
			if (num != -1)
			{
				return this.amounts[num];
			}
			return 0;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00017A0C File Offset: 0x00015C0C
		public void Add(string id, int count = 1)
		{
			int num = this.ids.IndexOf(id);
			if (num == -1)
			{
				this.amounts.Add(count);
				this.ids.Add(id);
				return;
			}
			List<int> list = this.amounts;
			int num2 = num;
			list[num2] += count;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00017A5C File Offset: 0x00015C5C
		public bool Remove(string id, int count = 1)
		{
			int num = this.ids.IndexOf(id);
			if (num == -1)
			{
				return false;
			}
			int num2 = this.amounts[num];
			if (count > num2)
			{
				this.RemoveAll(id);
				return false;
			}
			if (count == num2)
			{
				this.RemoveAll(id);
				return true;
			}
			this.amounts[num] = num2 - count;
			return true;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00017AB4 File Offset: 0x00015CB4
		public void RemoveAll(string id)
		{
			int num = this.ids.IndexOf(id);
			if (num == -1)
			{
				return;
			}
			this.amounts.RemoveAt(num);
			this.ids.RemoveAt(num);
		}

		// Token: 0x0400021A RID: 538
		[SerializeField]
		private List<string> ids = new List<string>();

		// Token: 0x0400021B RID: 539
		[SerializeField]
		private List<int> amounts = new List<int>();
	}
}

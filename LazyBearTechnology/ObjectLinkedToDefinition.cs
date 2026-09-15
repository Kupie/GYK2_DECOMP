using System;

namespace LazyBearTechnology
{
	// Token: 0x020000F8 RID: 248
	[Serializable]
	public abstract class ObjectLinkedToDefinition<T> where T : BalanceBaseObject
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x00017B09 File Offset: 0x00015D09
		public T Definition
		{
			get
			{
				if (this.cachedId != this.id)
				{
					this.definition = GameBalanceBase.Instance.GetData<T>(this.id);
					this.cachedId = this.id;
				}
				return this.definition;
			}
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00017B46 File Offset: 0x00015D46
		public ObjectLinkedToDefinition()
		{
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00017B59 File Offset: 0x00015D59
		public ObjectLinkedToDefinition(string id)
		{
			this.id = id;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00017B73 File Offset: 0x00015D73
		public override string ToString()
		{
			return base.ToString() + " (" + this.id + ")";
		}

		// Token: 0x0400021C RID: 540
		public string id;

		// Token: 0x0400021D RID: 541
		private T definition;

		// Token: 0x0400021E RID: 542
		private string cachedId = "";
	}
}

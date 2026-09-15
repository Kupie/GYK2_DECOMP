using System;

namespace LazyBearTechnology
{
	// Token: 0x020000F2 RID: 242
	[Serializable]
	public class GameResAtom : IStringIntSet
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x00017617 File Offset: 0x00015817
		public static GameResAtom Empty
		{
			get
			{
				return new GameResAtom(string.Empty, 0);
			}
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00017624 File Offset: 0x00015824
		public GameResAtom()
		{
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0001762C File Offset: 0x0001582C
		public GameResAtom(GameResAtom source)
		{
			this.type = source.type;
			this.value = source.value;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0001764C File Offset: 0x0001584C
		public GameResAtom(string type, int value)
		{
			this.type = type;
			this.value = (float)value;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00017663 File Offset: 0x00015863
		public GameResAtom(string type, float value)
		{
			this.type = type;
			this.value = value;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00017679 File Offset: 0x00015879
		public bool IsEmpty()
		{
			return this.type == string.Empty || this.value == 0f;
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0001769C File Offset: 0x0001589C
		public override string ToString()
		{
			return string.Format("[t={0}, v={1}]", this.type, this.value);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000176B9 File Offset: 0x000158B9
		public void Set(string id, int v)
		{
			this.type = id;
			this.value = (float)v;
		}

		// Token: 0x0400020C RID: 524
		public string type;

		// Token: 0x0400020D RID: 525
		public float value;
	}
}

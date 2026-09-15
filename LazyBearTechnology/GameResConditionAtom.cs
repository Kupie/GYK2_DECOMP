using System;

namespace LazyBearTechnology
{
	// Token: 0x020000F4 RID: 244
	[Serializable]
	public class GameResConditionAtom
	{
		// Token: 0x06000472 RID: 1138 RVA: 0x0001787A File Offset: 0x00015A7A
		public GameResConditionAtom()
		{
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00017882 File Offset: 0x00015A82
		public GameResConditionAtom(GameResConditionAtom source)
		{
			this.type = source.type;
			this.value = source.value;
			this.condition = source.condition;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x000178AE File Offset: 0x00015AAE
		public GameResConditionAtom(string type, int value, GameResCondition.Condition condition)
		{
			this.type = type;
			this.value = (float)value;
			this.condition = condition;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x000178CC File Offset: 0x00015ACC
		public GameResConditionAtom(string type, float value, GameResCondition.Condition condition)
		{
			this.type = type;
			this.value = value;
			this.condition = condition;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x000178E9 File Offset: 0x00015AE9
		public override string ToString()
		{
			return string.Format("[{0} {1} {2}]", this.type, GameResCondition.ConditionToString(this.condition), this.value);
		}

		// Token: 0x04000211 RID: 529
		public string type;

		// Token: 0x04000212 RID: 530
		public float value;

		// Token: 0x04000213 RID: 531
		public GameResCondition.Condition condition;
	}
}

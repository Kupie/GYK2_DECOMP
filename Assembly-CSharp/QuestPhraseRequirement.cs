using System;
using LazyBearTechnology;

// Token: 0x02000204 RID: 516
[Serializable]
public class QuestPhraseRequirement
{
	// Token: 0x04000EC5 RID: 3781
	public QuestPhraseRequirement.Requirement requirement;

	// Token: 0x04000EC6 RID: 3782
	public QuestPhraseRequirement.Entity entity;

	// Token: 0x04000EC7 RID: 3783
	public ItemCount itemCount;

	// Token: 0x04000EC8 RID: 3784
	public GameResAtom gameResAtom;

	// Token: 0x04000EC9 RID: 3785
	public string dayNumber;

	// Token: 0x04000ECA RID: 3786
	public string order;

	// Token: 0x02000205 RID: 517
	public enum Requirement
	{
		// Token: 0x04000ECC RID: 3788
		Price,
		// Token: 0x04000ECD RID: 3789
		Lock,
		// Token: 0x04000ECE RID: 3790
		Day,
		// Token: 0x04000ECF RID: 3791
		Order
	}

	// Token: 0x02000206 RID: 518
	public enum Entity
	{
		// Token: 0x04000ED1 RID: 3793
		Item,
		// Token: 0x04000ED2 RID: 3794
		GameResAtom,
		// Token: 0x04000ED3 RID: 3795
		Day,
		// Token: 0x04000ED4 RID: 3796
		Order
	}
}

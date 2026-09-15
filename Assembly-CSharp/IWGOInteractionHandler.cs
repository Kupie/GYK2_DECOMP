using System;

// Token: 0x02000673 RID: 1651
public interface IWGOInteractionHandler : IInteractionHandler
{
	// Token: 0x06002B8C RID: 11148
	IWGOInteractionHandler Init(Wgo wgo);

	// Token: 0x06002B8D RID: 11149
	void OnInteractionTargetEnter(PlayerController interactor);

	// Token: 0x06002B8E RID: 11150
	bool Interact(PlayerController interactor);

	// Token: 0x06002B8F RID: 11151
	bool HasInteraction(PlayerController interactor);

	// Token: 0x06002B90 RID: 11152
	bool Interact2(PlayerController interactor);

	// Token: 0x06002B91 RID: 11153
	bool HasInteraction2(PlayerController interactor);

	// Token: 0x06002B92 RID: 11154
	ItemType GetRequiredInteractionToolType();
}

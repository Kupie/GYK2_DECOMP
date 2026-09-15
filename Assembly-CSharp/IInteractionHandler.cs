using System;

// Token: 0x02000631 RID: 1585
public interface IInteractionHandler
{
	// Token: 0x06002A36 RID: 10806
	void OnInteractionTargetEnter();

	// Token: 0x06002A37 RID: 10807
	void OnInteractionTargetExit();

	// Token: 0x06002A38 RID: 10808
	InteractionInfos GetInteractionInfos();

	// Token: 0x06002A39 RID: 10809
	bool Interact();

	// Token: 0x06002A3A RID: 10810
	bool HasInteraction();

	// Token: 0x06002A3B RID: 10811
	bool Interact2();

	// Token: 0x06002A3C RID: 10812
	bool HasInteraction2();
}

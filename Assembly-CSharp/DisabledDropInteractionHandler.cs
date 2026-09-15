using System;

// Token: 0x0200062F RID: 1583
public class DisabledDropInteractionHandler : IInteractionHandler
{
	// Token: 0x06002A24 RID: 10788 RVA: 0x00002318 File Offset: 0x00000518
	public void OnInteractionTargetEnter()
	{
	}

	// Token: 0x06002A25 RID: 10789 RVA: 0x00002318 File Offset: 0x00000518
	public void OnInteractionTargetExit()
	{
	}

	// Token: 0x06002A26 RID: 10790 RVA: 0x000C73C5 File Offset: 0x000C55C5
	public InteractionInfos GetInteractionInfos()
	{
		return new InteractionInfos();
	}

	// Token: 0x06002A27 RID: 10791 RVA: 0x00028294 File Offset: 0x00026494
	public bool Interact()
	{
		return false;
	}

	// Token: 0x06002A28 RID: 10792 RVA: 0x00028294 File Offset: 0x00026494
	public bool HasInteraction()
	{
		return false;
	}

	// Token: 0x06002A29 RID: 10793 RVA: 0x00028294 File Offset: 0x00026494
	public bool Interact2()
	{
		return false;
	}

	// Token: 0x06002A2A RID: 10794 RVA: 0x00028294 File Offset: 0x00026494
	public bool HasInteraction2()
	{
		return false;
	}
}

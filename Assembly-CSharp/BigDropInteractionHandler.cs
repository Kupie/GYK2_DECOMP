using System;
using LazyBearTechnology;

// Token: 0x0200062E RID: 1582
public class BigDropInteractionHandler : IInteractionHandler
{
	// Token: 0x06002A1A RID: 10778 RVA: 0x000C72E6 File Offset: 0x000C54E6
	public BigDropInteractionHandler Init(DropView drop)
	{
		this.drop = drop;
		return this;
	}

	// Token: 0x06002A1B RID: 10779 RVA: 0x000C72F0 File Offset: 0x000C54F0
	public virtual void OnInteractionTargetEnter()
	{
		DropView dropView = this.drop;
		if (((dropView != null) ? dropView.Data : null) == null || UIObjectBubbleManager.Instance == null)
		{
			return;
		}
		UIObjectBubbleManager.Instance.PutOnTopTargetId = this.drop.Data.UniqueId;
	}

	// Token: 0x06002A1C RID: 10780 RVA: 0x000C732E File Offset: 0x000C552E
	public virtual void OnInteractionTargetExit()
	{
		if (UIObjectBubbleManager.Instance == null)
		{
			return;
		}
		UIObjectBubbleManager.Instance.PutOnTopTargetId = SGuid.Empty;
	}

	// Token: 0x06002A1D RID: 10781 RVA: 0x000C734D File Offset: 0x000C554D
	public virtual InteractionInfos GetInteractionInfos()
	{
		return new InteractionInfos(new InteractionInfo(string.Empty));
	}

	// Token: 0x06002A1E RID: 10782 RVA: 0x000C7360 File Offset: 0x000C5560
	public virtual bool Interact()
	{
		DropData data = this.drop.Data;
		Item item = ((data != null) ? data.Item : null);
		if (data == null || item == null)
		{
			return false;
		}
		MainGame.Instance.dropSystem.RemoveDrop(data, data.WorldId);
		MainGame.PlayerData.AddOverheadItem(item);
		return true;
	}

	// Token: 0x06002A1F RID: 10783 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public virtual bool HasInteraction()
	{
		return true;
	}

	// Token: 0x06002A20 RID: 10784 RVA: 0x00028294 File Offset: 0x00026494
	public virtual bool Interact2()
	{
		return false;
	}

	// Token: 0x06002A21 RID: 10785 RVA: 0x00028294 File Offset: 0x00026494
	public virtual bool HasInteraction2()
	{
		return false;
	}

	// Token: 0x06002A22 RID: 10786 RVA: 0x000C73B0 File Offset: 0x000C55B0
	protected string LocalizeHintWithActionIcon(string hintId, GameKey gameKey)
	{
		return ControllerIconLibrary.GetIconId(gameKey, null, true) + LLBase.L(hintId);
	}

	// Token: 0x04002321 RID: 8993
	protected DropView drop;
}

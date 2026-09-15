using System;

// Token: 0x020005E3 RID: 1507
[Serializable]
public class PlantOrder : OrderBase
{
	// Token: 0x060027D9 RID: 10201 RVA: 0x000BA0A0 File Offset: 0x000B82A0
	public PlantOrder(SGuid targetWgoUniqueId, Item item, bool isStarGroupItem = false)
		: base(targetWgoUniqueId, item)
	{
		this.isStarGroupItem = isStarGroupItem;
	}

	// Token: 0x060027DA RID: 10202 RVA: 0x00002318 File Offset: 0x00000518
	public override void ExecuteOrder(IOrderExecutor orderExecutor)
	{
	}

	// Token: 0x060027DB RID: 10203 RVA: 0x000BA0B1 File Offset: 0x000B82B1
	public override int GetPriority()
	{
		return 10;
	}

	// Token: 0x060027DC RID: 10204 RVA: 0x000BA0B8 File Offset: 0x000B82B8
	public override bool CanOrderBeExecuted(IOrderExecutor orderExecutor, out string reasonIfNot)
	{
		reasonIfNot = (orderExecutor.HasItem(base.Item) ? string.Empty : "player_talk_not_enough_items");
		return true;
	}

	// Token: 0x060027DD RID: 10205 RVA: 0x000B9F11 File Offset: 0x000B8111
	public override string GetInteractionHint()
	{
		return string.Empty;
	}

	// Token: 0x060027DE RID: 10206 RVA: 0x000B9F11 File Offset: 0x000B8111
	public override string GetStatusIcon()
	{
		return string.Empty;
	}

	// Token: 0x040021B6 RID: 8630
	public bool isStarGroupItem;
}

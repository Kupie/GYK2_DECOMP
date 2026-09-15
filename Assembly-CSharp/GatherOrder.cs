using System;

// Token: 0x020005DF RID: 1503
[Serializable]
public class GatherOrder : OrderBase
{
	// Token: 0x060027BD RID: 10173 RVA: 0x000B9D6C File Offset: 0x000B7F6C
	public GatherOrder(SGuid targetWgoUniqueId, Item item)
		: base(targetWgoUniqueId, item)
	{
	}

	// Token: 0x060027BE RID: 10174 RVA: 0x00002318 File Offset: 0x00000518
	public override void ExecuteOrder(IOrderExecutor orderExecutor)
	{
	}

	// Token: 0x060027BF RID: 10175 RVA: 0x000B9EF0 File Offset: 0x000B80F0
	public override bool CanOrderBeExecuted(IOrderExecutor orderExecutor, out string reasonIfNot)
	{
		bool flag = true;
		reasonIfNot = (flag ? string.Empty : "player_talk_not_enough_items");
		return flag;
	}

	// Token: 0x060027C0 RID: 10176 RVA: 0x000B9F11 File Offset: 0x000B8111
	public override string GetInteractionHint()
	{
		return string.Empty;
	}

	// Token: 0x060027C1 RID: 10177 RVA: 0x000B9F11 File Offset: 0x000B8111
	public override string GetStatusIcon()
	{
		return string.Empty;
	}
}

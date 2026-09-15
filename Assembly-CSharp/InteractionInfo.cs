using System;

// Token: 0x02000633 RID: 1587
public class InteractionInfo
{
	// Token: 0x06002A41 RID: 10817 RVA: 0x000C75F4 File Offset: 0x000C57F4
	public InteractionInfo()
	{
	}

	// Token: 0x06002A42 RID: 10818 RVA: 0x000C760A File Offset: 0x000C580A
	public InteractionInfo(string text)
	{
		this.text = text;
	}

	// Token: 0x06002A43 RID: 10819 RVA: 0x000C7627 File Offset: 0x000C5827
	public InteractionInfo(string text, string customIconId)
		: this(text)
	{
		this.customIconId = customIconId;
	}

	// Token: 0x06002A44 RID: 10820 RVA: 0x000C7638 File Offset: 0x000C5838
	public InteractionInfo(string text, ItemType equippedItemType, bool isItemEquipped, TalentDef assignedTalent, int masteryLock, bool isEnoughMastery)
	{
		this.text = text;
		this.equippedItemType = equippedItemType;
		this.isItemEquipped = isItemEquipped;
		this.assignedTalent = assignedTalent;
		this.masteryLock = masteryLock;
		this.isEnoughMastery = isEnoughMastery;
	}

	// Token: 0x06002A45 RID: 10821 RVA: 0x000C7688 File Offset: 0x000C5888
	public InteractionInfo(string text, string customIconId, ItemType equippedItemType, bool isItemEquipped, TalentDef assignedTalent, int masteryLock, bool isEnoughMastery)
	{
		this.text = text;
		this.equippedItemType = equippedItemType;
		this.isItemEquipped = isItemEquipped;
		this.assignedTalent = assignedTalent;
		this.masteryLock = masteryLock;
		this.isEnoughMastery = isEnoughMastery;
		this.customIconId = customIconId;
	}

	// Token: 0x04002323 RID: 8995
	public string text;

	// Token: 0x04002324 RID: 8996
	public ItemType equippedItemType;

	// Token: 0x04002325 RID: 8997
	public bool isItemEquipped = true;

	// Token: 0x04002326 RID: 8998
	public TalentDef assignedTalent;

	// Token: 0x04002327 RID: 8999
	public int masteryLock;

	// Token: 0x04002328 RID: 9000
	public bool isEnoughMastery = true;

	// Token: 0x04002329 RID: 9001
	public string customIconId;
}

using System;
using LazyBearTechnology;

// Token: 0x0200086C RID: 2156
public class UIHotBarWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000830 RID: 2096
	// (get) Token: 0x06003729 RID: 14121 RVA: 0x0010A9EB File Offset: 0x00108BEB
	// (set) Token: 0x0600372A RID: 14122 RVA: 0x0010A9F3 File Offset: 0x00108BF3
	public PlayerData PlayerData { get; private set; }

	// Token: 0x17000831 RID: 2097
	// (get) Token: 0x0600372B RID: 14123 RVA: 0x0010A9FC File Offset: 0x00108BFC
	// (set) Token: 0x0600372C RID: 14124 RVA: 0x0010AA04 File Offset: 0x00108C04
	public bool IsUsable { get; private set; }

	// Token: 0x17000832 RID: 2098
	// (get) Token: 0x0600372D RID: 14125 RVA: 0x0010AA0D File Offset: 0x00108C0D
	// (set) Token: 0x0600372E RID: 14126 RVA: 0x0010AA15 File Offset: 0x00108C15
	public Item PinnableItem { get; private set; }

	// Token: 0x0600372F RID: 14127 RVA: 0x0010AA1E File Offset: 0x00108C1E
	public UIHotBarWidgetData(GameSave gameSave, bool isUsable = false, Item pinnableItem = null)
	{
		this.PlayerData = gameSave.playerData;
		this.IsUsable = isUsable;
		this.PinnableItem = pinnableItem;
	}
}

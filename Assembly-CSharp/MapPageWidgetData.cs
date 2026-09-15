using System;
using LazyBearTechnology;

// Token: 0x02000948 RID: 2376
public class MapPageWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000973 RID: 2419
	// (get) Token: 0x06003EAB RID: 16043 RVA: 0x0012B4BD File Offset: 0x001296BD
	// (set) Token: 0x06003EAC RID: 16044 RVA: 0x0012B4C5 File Offset: 0x001296C5
	public PlayerData PlayerData { get; set; }

	// Token: 0x17000974 RID: 2420
	// (get) Token: 0x06003EAD RID: 16045 RVA: 0x0012B4CE File Offset: 0x001296CE
	// (set) Token: 0x06003EAE RID: 16046 RVA: 0x0012B4D6 File Offset: 0x001296D6
	public bool MilestonesInteractable { get; private set; }

	// Token: 0x17000975 RID: 2421
	// (get) Token: 0x06003EAF RID: 16047 RVA: 0x0012B4DF File Offset: 0x001296DF
	// (set) Token: 0x06003EB0 RID: 16048 RVA: 0x0012B4E7 File Offset: 0x001296E7
	public string CurrentMilestone { get; private set; }

	// Token: 0x06003EB1 RID: 16049 RVA: 0x0012B4F0 File Offset: 0x001296F0
	public MapPageWidgetData(GameSave gameSave, bool milestonesInteractable, string currentMilestone)
	{
		this.PlayerData = gameSave.playerData;
		this.MilestonesInteractable = milestonesInteractable;
		this.CurrentMilestone = currentMilestone;
	}
}

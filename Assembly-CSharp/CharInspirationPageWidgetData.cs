using System;
using LazyBearTechnology;

// Token: 0x02000914 RID: 2324
public class CharInspirationPageWidgetData : LazyWidgetDataBase
{
	// Token: 0x1700092E RID: 2350
	// (get) Token: 0x06003D31 RID: 15665 RVA: 0x00124B41 File Offset: 0x00122D41
	// (set) Token: 0x06003D32 RID: 15666 RVA: 0x00124B49 File Offset: 0x00122D49
	public TalentData TalentData { get; private set; }

	// Token: 0x1700092F RID: 2351
	// (get) Token: 0x06003D33 RID: 15667 RVA: 0x00124B52 File Offset: 0x00122D52
	// (set) Token: 0x06003D34 RID: 15668 RVA: 0x00124B5A File Offset: 0x00122D5A
	public TalentSystemData TalentSystemData { get; private set; }

	// Token: 0x06003D35 RID: 15669 RVA: 0x00124B64 File Offset: 0x00122D64
	public CharInspirationPageWidgetData(TalentSystemData talentSystemData, string talentId = null)
	{
		this.TalentSystemData = talentSystemData;
		string text = ((!string.IsNullOrEmpty(talentId)) ? talentId : ((!string.IsNullOrEmpty(CharInspirationPageWidgetData.lastShownTalentId)) ? CharInspirationPageWidgetData.lastShownTalentId : "talent_orange"));
		this.SwitchTalent(text);
	}

	// Token: 0x06003D36 RID: 15670 RVA: 0x00002318 File Offset: 0x00000518
	public void FillFromTalentsData(TalentSystemData talentSystemData, string talentId = null)
	{
	}

	// Token: 0x06003D37 RID: 15671 RVA: 0x00124BA9 File Offset: 0x00122DA9
	public void SwitchTalent(string talentId)
	{
		this.TalentData = this.TalentSystemData.GetTalentBranch(talentId);
		CharInspirationPageWidgetData.lastShownTalentId = this.TalentData.id;
	}

	// Token: 0x06003D38 RID: 15672 RVA: 0x00124BD0 File Offset: 0x00122DD0
	public void SubscribeEvents()
	{
		if (!this.subscribedEvents)
		{
			this.TalentSystemData.OnTalentExpChanged += this.onTalentExpChanged;
			this.TalentSystemData.OnInspirationProgressChanged += this.onInspirationProgressChanged;
			this.TalentSystemData.OnInspirationCompleted += this.onInspirationCompleted;
			this.TalentSystemData.OnTalentLevelPurchased += this.onTalentLevelPurchased;
			this.TalentSystemData.OnInspirationPurchased += this.onInspirationPurchased;
			this.subscribedEvents = true;
		}
	}

	// Token: 0x06003D39 RID: 15673 RVA: 0x00124C44 File Offset: 0x00122E44
	public void UnsubscribeEvents()
	{
		if (this.subscribedEvents)
		{
			this.TalentSystemData.OnTalentExpChanged -= this.onTalentExpChanged;
			this.TalentSystemData.OnInspirationProgressChanged -= this.onInspirationProgressChanged;
			this.TalentSystemData.OnInspirationCompleted -= this.onInspirationCompleted;
			this.TalentSystemData.OnTalentLevelPurchased -= this.onTalentLevelPurchased;
			this.TalentSystemData.OnInspirationPurchased -= this.onInspirationPurchased;
			this.subscribedEvents = false;
		}
	}

	// Token: 0x0400300A RID: 12298
	private const string DEFAULT_TALENT_ID_TO_DRAW = "talent_orange";

	// Token: 0x0400300B RID: 12299
	public TalentSystemData.DelTalentExpChanged onTalentExpChanged;

	// Token: 0x0400300C RID: 12300
	public TalentSystemData.DelTalentLevelPurchased onTalentLevelPurchased;

	// Token: 0x0400300D RID: 12301
	public Action<string> onInspirationPurchased;

	// Token: 0x0400300E RID: 12302
	public Action<string> onInspirationProgressChanged;

	// Token: 0x0400300F RID: 12303
	public Action<string> onInspirationCompleted;

	// Token: 0x04003010 RID: 12304
	private static string lastShownTalentId;

	// Token: 0x04003013 RID: 12307
	private bool subscribedEvents;
}

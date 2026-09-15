using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x0200090A RID: 2314
public class CharacterWindowData : LazyWidgetDataBase
{
	// Token: 0x17000927 RID: 2343
	// (get) Token: 0x06003CDA RID: 15578 RVA: 0x00123024 File Offset: 0x00121224
	// (set) Token: 0x06003CDB RID: 15579 RVA: 0x0012302C File Offset: 0x0012122C
	public PlayerData PlayerData { get; private set; }

	// Token: 0x17000928 RID: 2344
	// (get) Token: 0x06003CDC RID: 15580 RVA: 0x00123035 File Offset: 0x00121235
	// (set) Token: 0x06003CDD RID: 15581 RVA: 0x0012303D File Offset: 0x0012123D
	public CharacterWindowData.CharPage Page { get; private set; }

	// Token: 0x17000929 RID: 2345
	// (get) Token: 0x06003CDE RID: 15582 RVA: 0x00123046 File Offset: 0x00121246
	// (set) Token: 0x06003CDF RID: 15583 RVA: 0x0012304E File Offset: 0x0012124E
	public CharMainPageWidgetData CharMainPageWidgetData { get; private set; }

	// Token: 0x1700092A RID: 2346
	// (get) Token: 0x06003CE0 RID: 15584 RVA: 0x00123057 File Offset: 0x00121257
	// (set) Token: 0x06003CE1 RID: 15585 RVA: 0x0012305F File Offset: 0x0012125F
	public CharInspirationPageWidgetData InspirationPageWidgetData { get; private set; }

	// Token: 0x1700092B RID: 2347
	// (get) Token: 0x06003CE2 RID: 15586 RVA: 0x00123068 File Offset: 0x00121268
	// (set) Token: 0x06003CE3 RID: 15587 RVA: 0x00123070 File Offset: 0x00121270
	public MapPageWidgetData MapPageWidgetData { get; private set; }

	// Token: 0x1700092C RID: 2348
	// (get) Token: 0x06003CE4 RID: 15588 RVA: 0x00123079 File Offset: 0x00121279
	// (set) Token: 0x06003CE5 RID: 15589 RVA: 0x00123081 File Offset: 0x00121281
	public string FocusOnQuest { get; set; }

	// Token: 0x1700092D RID: 2349
	// (get) Token: 0x06003CE6 RID: 15590 RVA: 0x0012308A File Offset: 0x0012128A
	// (set) Token: 0x06003CE7 RID: 15591 RVA: 0x00123092 File Offset: 0x00121292
	public bool HasAvailableActionOnInspirationPage { get; private set; }

	// Token: 0x06003CE8 RID: 15592 RVA: 0x0012309C File Offset: 0x0012129C
	public CharacterWindowData(GameSave gameSave, CharacterWindowData.CharPage page = CharacterWindowData.CharPage.Main, string inspirationTalentId = null)
	{
		this.PlayerData = gameSave.playerData;
		this.SetPage(page);
		this.CharMainPageWidgetData = new CharMainPageWidgetData(gameSave);
		this.InspirationPageWidgetData = new CharInspirationPageWidgetData(gameSave.talentSystemData, inspirationTalentId);
		this.MapPageWidgetData = new MapPageWidgetData(gameSave, false, string.Empty);
		this.UpdateHasAvailableActionOnInspirationPageStatus("");
	}

	// Token: 0x06003CE9 RID: 15593 RVA: 0x00123100 File Offset: 0x00121300
	public void SubscribeEvents()
	{
		if (!this.subscribedEvents)
		{
			MainGame.Instance.GameSave.talentSystemData.OnInspirationPurchased += this.UpdateHasAvailableActionOnInspirationPageStatus;
			MainGame.Instance.GameSave.talentSystemData.OnInspirationCompleted += this.UpdateHasAvailableActionOnInspirationPageStatus;
			this.subscribedEvents = true;
		}
	}

	// Token: 0x06003CEA RID: 15594 RVA: 0x0012315C File Offset: 0x0012135C
	public void UnsubscribeEvents()
	{
		if (this.subscribedEvents)
		{
			MainGame.Instance.GameSave.talentSystemData.OnInspirationPurchased -= this.UpdateHasAvailableActionOnInspirationPageStatus;
			MainGame.Instance.GameSave.talentSystemData.OnInspirationCompleted -= this.UpdateHasAvailableActionOnInspirationPageStatus;
			this.subscribedEvents = false;
		}
	}

	// Token: 0x06003CEB RID: 15595 RVA: 0x001231B8 File Offset: 0x001213B8
	public void UpdateHasAvailableActionOnInspirationPageStatus(string id = "")
	{
		this.HasAvailableActionOnInspirationPage = false;
		using (List<TalentData>.Enumerator enumerator = MainGame.Instance.GameSave.talentSystemData.talentData.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasAvailableInspirationActionIndicator)
				{
					this.HasAvailableActionOnInspirationPage = true;
					break;
				}
			}
		}
		Action<CharacterWindowData.CharPage, bool> action = this.onPageStatusChanged;
		if (action == null)
		{
			return;
		}
		action(CharacterWindowData.CharPage.Inspiration, this.HasAvailableActionOnInspirationPage);
	}

	// Token: 0x06003CEC RID: 15596 RVA: 0x00123240 File Offset: 0x00121440
	public void SetPage(CharacterWindowData.CharPage page)
	{
		this.Page = page;
		this.PlayerData.lastOpenedPage = page;
	}

	// Token: 0x06003CED RID: 15597 RVA: 0x00123255 File Offset: 0x00121455
	public void UpdateTalentInInspirationWidgetData(string talentId)
	{
		this.InspirationPageWidgetData.SwitchTalent(talentId);
	}

	// Token: 0x04002FCB RID: 12235
	public Action<CharacterWindowData.CharPage, bool> onPageStatusChanged;

	// Token: 0x04002FD3 RID: 12243
	private bool subscribedEvents;

	// Token: 0x0200090B RID: 2315
	public enum CharPage
	{
		// Token: 0x04002FD5 RID: 12245
		Undefined,
		// Token: 0x04002FD6 RID: 12246
		Main,
		// Token: 0x04002FD7 RID: 12247
		TechTree,
		// Token: 0x04002FD8 RID: 12248
		Inspiration,
		// Token: 0x04002FD9 RID: 12249
		QuestTree,
		// Token: 0x04002FDA RID: 12250
		Map
	}
}

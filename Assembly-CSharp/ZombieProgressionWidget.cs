using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000A82 RID: 2690
public class ZombieProgressionWidget : LazyWidget<ZombieProgressionWidgetData>
{
	// Token: 0x17000B1E RID: 2846
	// (get) Token: 0x0600494C RID: 18764 RVA: 0x0015A8DC File Offset: 0x00158ADC
	public TalentTabButtonsContainer TalentTabButtonsContainer
	{
		get
		{
			return this.talentTabButtonsContainer;
		}
	}

	// Token: 0x0600494D RID: 18765 RVA: 0x0015A8E4 File Offset: 0x00158AE4
	public override void Init()
	{
		base.Init();
		this.talentTabButtonsContainer.Init(delegate(string talentId)
		{
			this.data.SwitchTalent(talentId);
			this.RedrawTalentLevelUpsWidget();
		}, this.canvas);
	}

	// Token: 0x0600494E RID: 18766 RVA: 0x0015A909 File Offset: 0x00158B09
	public override void Redraw()
	{
		base.Redraw();
		this.talentTabButtonsContainer.Draw(this.data.ZombieTalentData.id, true);
		this.RedrawTalentLevelUpsWidget();
	}

	// Token: 0x0600494F RID: 18767 RVA: 0x0015A933 File Offset: 0x00158B33
	private void RedrawTalentLevelUpsWidget()
	{
		this.talentLevelUpsWidgetData = new TalentLevelUpsWidgetData(this.data.ZombieWgoData, this.data.ZombieTalentData);
		this.talentLevelUpsWidget.Draw(this.talentLevelUpsWidgetData);
	}

	// Token: 0x06004950 RID: 18768 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003923 RID: 14627
	[SerializeField]
	private TalentLevelUpsWidget talentLevelUpsWidget;

	// Token: 0x04003924 RID: 14628
	[SerializeField]
	private Canvas canvas;

	// Token: 0x04003925 RID: 14629
	[SerializeField]
	private TalentTabButtonsContainer talentTabButtonsContainer;

	// Token: 0x04003926 RID: 14630
	private TalentLevelUpsWidgetData talentLevelUpsWidgetData;
}

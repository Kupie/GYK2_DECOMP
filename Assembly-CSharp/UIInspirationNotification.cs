using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000886 RID: 2182
public class UIInspirationNotification : UIBaseNotification
{
	// Token: 0x17000851 RID: 2129
	// (get) Token: 0x06003804 RID: 14340 RVA: 0x0010E2BC File Offset: 0x0010C4BC
	// (set) Token: 0x06003805 RID: 14341 RVA: 0x0010E2C4 File Offset: 0x0010C4C4
	public string InspirationTalentId { get; set; }

	// Token: 0x17000852 RID: 2130
	// (get) Token: 0x06003806 RID: 14342 RVA: 0x0010E2CD File Offset: 0x0010C4CD
	// (set) Token: 0x06003807 RID: 14343 RVA: 0x0010E2D5 File Offset: 0x0010C4D5
	public string InspirationId { get; set; }

	// Token: 0x06003808 RID: 14344 RVA: 0x0010E2E0 File Offset: 0x0010C4E0
	public override void Draw()
	{
		this.nameLabel.text = LLBase.L(this.InspirationId);
		InspirationDef data = GameBalance.Me.GetData<InspirationDef>(this.InspirationId);
		this.inspirationIcon.sprite = data.Icon;
		this.inspirationIcon.BlueColorReplace(this.iconOutlineColor);
		this.iconFrame.sprite = this.framesSprites[data.lvl - 1];
		this.talentIconLabel.text = this.InspirationTalentId.FontIcon();
		this.currentViewData = this.viewDatas.Find((UIInspirationNotification.TalentViewData d) => d.talentId == this.InspirationTalentId);
		this.backgroundRight.sprite = this.currentViewData.rightBack;
		this.backgroundLeft.sprite = this.currentViewData.leftBack;
		LazyAudio.Play("unlock");
	}

	// Token: 0x06003809 RID: 14345 RVA: 0x0010E3BA File Offset: 0x0010C5BA
	public override void ReleaseToPool()
	{
		LazyPooler.ReleaseObject<UIInspirationNotification>(this);
	}

	// Token: 0x04002C9C RID: 11420
	[SerializeField]
	private TextMeshProUGUI nameLabel;

	// Token: 0x04002C9D RID: 11421
	[SerializeField]
	private TextMeshProUGUI talentIconLabel;

	// Token: 0x04002C9E RID: 11422
	[SerializeField]
	private Image inspirationIcon;

	// Token: 0x04002C9F RID: 11423
	[SerializeField]
	private Image iconFrame;

	// Token: 0x04002CA0 RID: 11424
	[SerializeField]
	private Color iconOutlineColor;

	// Token: 0x04002CA1 RID: 11425
	[SerializeField]
	private Image backgroundRight;

	// Token: 0x04002CA2 RID: 11426
	[SerializeField]
	private Image backgroundLeft;

	// Token: 0x04002CA3 RID: 11427
	[SerializeField]
	private Sprite[] framesSprites;

	// Token: 0x04002CA4 RID: 11428
	[SerializeField]
	private List<UIInspirationNotification.TalentViewData> viewDatas = new List<UIInspirationNotification.TalentViewData>();

	// Token: 0x04002CA5 RID: 11429
	private UIInspirationNotification.TalentViewData currentViewData;

	// Token: 0x02000887 RID: 2183
	[Serializable]
	private class TalentViewData
	{
		// Token: 0x04002CA8 RID: 11432
		public string talentId;

		// Token: 0x04002CA9 RID: 11433
		public Sprite rightBack;

		// Token: 0x04002CAA RID: 11434
		public Sprite leftBack;
	}
}

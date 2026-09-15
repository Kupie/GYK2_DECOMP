using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008CB RID: 2251
public class UITalentIcon : MonoBehaviour
{
	// Token: 0x06003ACB RID: 15051 RVA: 0x00118C04 File Offset: 0x00116E04
	public void Draw(TalentDef talentDef, int masteryLock, bool isEnoughMastery)
	{
		this.masteryLockLabel.gameObject.SetActive(!isEnoughMastery);
		this.masteryLockTextStyle.SetTextStyle(isEnoughMastery ? this.normalStyle : this.lockStyle);
		this.masteryLockLabel.text = masteryLock.ToString();
		this.DrawTalentIcon(talentDef);
		base.gameObject.SetActive(true);
	}

	// Token: 0x06003ACC RID: 15052 RVA: 0x00118C68 File Offset: 0x00116E68
	public void Draw(TalentDef talentDef, int masteryLock, bool isEnoughMastery, bool isStar)
	{
		if (talentDef == null)
		{
			this.Hide();
			return;
		}
		this.masteryLockLabel.gameObject.SetActive(true);
		this.masteryLockLabel.text = masteryLock.ToString();
		this.masteryLockTextStyle.SetTextStyle(isStar ? this.startStyle : (isEnoughMastery ? this.normalStyle : this.lockStyle));
		this.DrawTalentIcon(talentDef);
		base.gameObject.SetActive(true);
	}

	// Token: 0x06003ACD RID: 15053 RVA: 0x00118CE0 File Offset: 0x00116EE0
	public void Draw(TalentDef talentDef, string masteryValue)
	{
		if (talentDef == null)
		{
			this.Hide();
			return;
		}
		this.masteryLockLabel.gameObject.SetActive(true);
		this.masteryLockLabel.text = masteryValue;
		this.masteryLockTextStyle.SetTextStyle(this.normalStyle);
		this.DrawTalentIcon(talentDef);
		base.gameObject.SetActive(true);
	}

	// Token: 0x06003ACE RID: 15054 RVA: 0x00027874 File Offset: 0x00025A74
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06003ACF RID: 15055 RVA: 0x00118D38 File Offset: 0x00116F38
	private void DrawTalentIcon(TalentDef talentDef)
	{
		this.talentIconImage.text = talentDef.id.FontIcon();
	}

	// Token: 0x04002E69 RID: 11881
	[SerializeField]
	private TextMeshProUGUI talentIconImage;

	// Token: 0x04002E6A RID: 11882
	[SerializeField]
	private TextMeshProUGUI masteryLockLabel;

	// Token: 0x04002E6B RID: 11883
	[SerializeField]
	private TextStyleComponent masteryLockTextStyle;

	// Token: 0x04002E6C RID: 11884
	[SerializeField]
	private TextStyle normalStyle;

	// Token: 0x04002E6D RID: 11885
	[SerializeField]
	private TextStyle lockStyle;

	// Token: 0x04002E6E RID: 11886
	[SerializeField]
	private TextStyle startStyle;
}

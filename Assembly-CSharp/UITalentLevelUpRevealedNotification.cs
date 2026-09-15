using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000890 RID: 2192
public class UITalentLevelUpRevealedNotification : UIBaseNotification
{
	// Token: 0x17000860 RID: 2144
	// (get) Token: 0x06003846 RID: 14406 RVA: 0x0010E8BC File Offset: 0x0010CABC
	// (set) Token: 0x06003847 RID: 14407 RVA: 0x0010E8C4 File Offset: 0x0010CAC4
	public string TalentLevelUpId { get; set; }

	// Token: 0x06003848 RID: 14408 RVA: 0x0010E8D0 File Offset: 0x0010CAD0
	public override void Draw()
	{
		TalentLevelUpDef data = GameBalance.Me.GetData<TalentLevelUpDef>(this.TalentLevelUpId);
		PerkDef data2 = GameBalance.Me.GetData<PerkDef>(data.linkedPerk);
		this.perkIcon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(data2.IconId, "i_p-artist");
		this.perkIcon.BlueColorReplace(this.toReplace);
		this.label.text = LLBase.L("ui_perks") + ": " + LLBase.L(data2.id);
	}

	// Token: 0x06003849 RID: 14409 RVA: 0x0010E95A File Offset: 0x0010CB5A
	public override void ReleaseToPool()
	{
		LazyPooler.ReleaseObject<UITalentLevelUpRevealedNotification>(this);
	}

	// Token: 0x04002CCB RID: 11467
	[SerializeField]
	private Image perkIcon;

	// Token: 0x04002CCC RID: 11468
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x04002CCD RID: 11469
	[SerializeField]
	private Color toReplace = new Color(1f, 1f, 1f, 0f);
}

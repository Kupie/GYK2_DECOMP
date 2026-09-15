using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000891 RID: 2193
public class UITechRepUnlockedNotification : UIBaseNotification
{
	// Token: 0x17000861 RID: 2145
	// (get) Token: 0x0600384B RID: 14411 RVA: 0x0010E989 File Offset: 0x0010CB89
	// (set) Token: 0x0600384C RID: 14412 RVA: 0x0010E991 File Offset: 0x0010CB91
	public string TechId { get; set; }

	// Token: 0x0600384D RID: 14413 RVA: 0x0010E99C File Offset: 0x0010CB9C
	public override void Draw()
	{
		TechDef data = GameBalance.Me.GetData<TechDef>(this.TechId);
		string text = string.Format("tech_tab_{0}", data.tab);
		this.tabIcon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(text, "i_tech_tree_tab_placeholder");
		this.tabIcon.BlueColorReplace(this.toReplace);
		this.label.text = LLBase.L("ui_notification_tech_disponible") + " " + LLBase.L(this.TechId);
	}

	// Token: 0x0600384E RID: 14414 RVA: 0x0010EA26 File Offset: 0x0010CC26
	public override void ReleaseToPool()
	{
		LazyPooler.ReleaseObject<UITechRepUnlockedNotification>(this);
	}

	// Token: 0x04002CCF RID: 11471
	[SerializeField]
	private Image tabIcon;

	// Token: 0x04002CD0 RID: 11472
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x04002CD1 RID: 11473
	[SerializeField]
	private Color toReplace = new Color(1f, 1f, 1f, 0f);
}

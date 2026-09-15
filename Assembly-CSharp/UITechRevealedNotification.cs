using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000892 RID: 2194
public class UITechRevealedNotification : UIBaseNotification
{
	// Token: 0x17000862 RID: 2146
	// (get) Token: 0x06003850 RID: 14416 RVA: 0x0010EA55 File Offset: 0x0010CC55
	// (set) Token: 0x06003851 RID: 14417 RVA: 0x0010EA5D File Offset: 0x0010CC5D
	public string TechId { get; set; }

	// Token: 0x17000863 RID: 2147
	// (get) Token: 0x06003852 RID: 14418 RVA: 0x0010EA66 File Offset: 0x0010CC66
	// (set) Token: 0x06003853 RID: 14419 RVA: 0x0010EA6E File Offset: 0x0010CC6E
	public bool IsTechUnlocked { get; set; }

	// Token: 0x06003854 RID: 14420 RVA: 0x0010EA78 File Offset: 0x0010CC78
	public override void Draw()
	{
		TechDef data = GameBalance.Me.GetData<TechDef>(this.TechId);
		string text = string.Format("tech_tab_{0}", data.tab);
		this.tabIcon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(text, "i_tech_tree_tab_placeholder");
		this.tabIcon.BlueColorReplace(this.toReplace);
		this.label.text = (this.IsTechUnlocked ? (LLBase.L("ui_notification_tech_added") + " " + LLBase.L(this.TechId)) : (LLBase.L("ui_notification_tech_revealed") + " " + LLBase.L(this.TechId)));
		if (this.IsTechUnlocked)
		{
			LazyAudio.PlayAndForget("unlock");
		}
	}

	// Token: 0x06003855 RID: 14421 RVA: 0x0010EB3D File Offset: 0x0010CD3D
	public override void ReleaseToPool()
	{
		LazyPooler.ReleaseObject<UITechRevealedNotification>(this);
	}

	// Token: 0x04002CD3 RID: 11475
	[SerializeField]
	private Image tabIcon;

	// Token: 0x04002CD4 RID: 11476
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x04002CD5 RID: 11477
	[SerializeField]
	private Color toReplace = new Color(1f, 1f, 1f, 0f);
}

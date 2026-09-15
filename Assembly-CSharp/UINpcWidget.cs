using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000896 RID: 2198
public class UINpcWidget : LazyWidget<UINpcWidgetData>
{
	// Token: 0x06003889 RID: 14473 RVA: 0x0010F8E4 File Offset: 0x0010DAE4
	public override void Redraw()
	{
		base.Redraw();
		this.portrait.sprite = this.data.WgoDef.Portrait;
		this.npcNameLabel.text = LLBase.L(this.data.WgoDef.id);
		int npcrep = MainGame.Instance.GameSave.playerData.GetNPCRep(this.data.WgoDef.repResName);
		GameResIconConfig configForRes = GameResDisplayConfig.GetConfigForRes(this.data.WgoDef.repResName, GameResIconType.Common);
		if (configForRes == null)
		{
			this.repIcon.text = "icon_smile02".FontIcon();
		}
		else
		{
			this.repIcon.text = configForRes.iconName.FontIcon();
		}
		this.repLabel.text = npcrep.ToString();
		this.progressBar.value = (float)npcrep / 100f;
		base.transform.parent.gameObject.SetActive(true);
		this.portrait.BlueColorReplace(this.toReplace);
	}

	// Token: 0x0600388A RID: 14474 RVA: 0x0010F9EE File Offset: 0x0010DBEE
	public override void Hide()
	{
		base.transform.parent.gameObject.SetActive(false);
	}

	// Token: 0x0600388B RID: 14475 RVA: 0x0010FA08 File Offset: 0x0010DC08
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new UINpcWidgetData
		{
			NpcId = "npc_larry"
		});
	}

	// Token: 0x04002CE5 RID: 11493
	[SerializeField]
	private Image portrait;

	// Token: 0x04002CE6 RID: 11494
	[SerializeField]
	private TextMeshProUGUI npcNameLabel;

	// Token: 0x04002CE7 RID: 11495
	[SerializeField]
	private TextMeshProUGUI repLabel;

	// Token: 0x04002CE8 RID: 11496
	[SerializeField]
	private TextMeshProUGUI repIcon;

	// Token: 0x04002CE9 RID: 11497
	[SerializeField]
	private Slider progressBar;

	// Token: 0x04002CEA RID: 11498
	[SerializeField]
	private Color toReplace = new Color(1f, 1f, 1f, 0f);
}

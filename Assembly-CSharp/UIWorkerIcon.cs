using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008CD RID: 2253
public class UIWorkerIcon : MonoBehaviour
{
	// Token: 0x170008DC RID: 2268
	// (get) Token: 0x06003AD4 RID: 15060 RVA: 0x00118E62 File Offset: 0x00117062
	public RectTransform WorkerIconParent
	{
		get
		{
			return this.workerIconParent;
		}
	}

	// Token: 0x06003AD5 RID: 15061 RVA: 0x00118E6C File Offset: 0x0011706C
	private void TryInitMaterials()
	{
		if (!this.materialsInitialized)
		{
			this.materialsInitialized = true;
			this.hed.material = new Material(this.hed.material);
			this.hed2.material = new Material(this.hed2.material);
			this.bdy.material = new Material(this.bdy.material);
			this.bdy2.material = new Material(this.bdy2.material);
			this.hrs.material = new Material(this.hrs.material);
			this.hrs2.material = new Material(this.hrs2.material);
			this.arms.material = new Material(this.arms.material);
			this.arms2.material = new Material(this.arms2.material);
			this.brd.material = new Material(this.brd.material);
			this.brd2.material = new Material(this.brd2.material);
			this.bdyOver.material = new Material(this.bdyOver.material);
			this.bdyOver2.material = new Material(this.bdyOver2.material);
			this.stn.material = new Material(this.stn.material);
			this.stn2.material = new Material(this.stn2.material);
		}
	}

	// Token: 0x06003AD6 RID: 15062 RVA: 0x00119008 File Offset: 0x00117208
	public void Show(IWorker worker, SkinPresetGK2 skinPreset, TalentDef talentDef)
	{
		if (talentDef == null)
		{
			this.SetTalentActive(false);
			base.gameObject.SetActive(true);
			return;
		}
		this.ShowSkin(skinPreset);
		UIWorkerIcon.TalentViewData talentViewData = this.viewDatas.Find((UIWorkerIcon.TalentViewData d) => d.talentId == talentDef.id);
		if (talentViewData == null)
		{
			Debug.LogError("No view data for talent " + talentDef.id);
			this.SetTalentActive(false);
			return;
		}
		this.SetTalentActive(true);
		this.masteryValueLabel.text = worker.GetMasteryLevelForTalentBranch(talentDef.id, null).ToString();
		this.talentIconLabel.text = talentDef.id.FontIcon();
		this.talentBackImage.sprite = talentViewData.backSprite;
		this.masteryValueTextStyleComponent.SetTextStyle(talentViewData.talentStyle);
		this.EnsureMasteryMouseTooltip("tt_craft_mastery");
	}

	// Token: 0x06003AD7 RID: 15063 RVA: 0x001190F8 File Offset: 0x001172F8
	public void SetTalentValue(TalentDef talentDef, int value)
	{
		this.SetTalentActive(true);
		UIWorkerIcon.TalentViewData talentViewData = this.viewDatas.Find((UIWorkerIcon.TalentViewData d) => d.talentId == talentDef.id);
		if (talentViewData == null)
		{
			Debug.LogError("No view data for talent " + talentDef.id);
			this.SetTalentActive(false);
			return;
		}
		this.SetTalentActive(true);
		this.masteryValueLabel.text = value.ToString();
		this.talentIconLabel.text = talentDef.id.FontIcon();
		this.talentBackImage.sprite = talentViewData.backSprite;
		this.masteryValueTextStyleComponent.SetTextStyle(talentViewData.talentStyle);
		this.EnsureMasteryMouseTooltip("tt_garden_mastery");
	}

	// Token: 0x06003AD8 RID: 15064 RVA: 0x001191B7 File Offset: 0x001173B7
	public void ShowWithoutTalent([CanBeNull] IWorker worker, SkinPresetGK2 skinPreset)
	{
		this.SetTalentActive(false);
		this.ShowSkin(skinPreset);
		base.gameObject.SetActive(true);
	}

	// Token: 0x06003AD9 RID: 15065 RVA: 0x00027874 File Offset: 0x00025A74
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06003ADA RID: 15066 RVA: 0x001191D3 File Offset: 0x001173D3
	private void SetTalentActive(bool active)
	{
		this.talentParent.SetActive(active);
		this.workerIconParent.anchoredPosition = (this.talentParent.activeSelf ? this.workerIconPositionWithTalent : this.workerIconPositionWithoutTalent);
	}

	// Token: 0x06003ADB RID: 15067 RVA: 0x00119208 File Offset: 0x00117408
	private void EnsureMasteryMouseTooltip(string tooltipId)
	{
		if (this.talentBackImage == null)
		{
			return;
		}
		UIMouseTooltip.Attach(this.talentBackImage.gameObject, tooltipId, null, false, false, default(UIMouseTooltipEdges), default(Vector2), null);
		this.talentIconLabel.raycastTarget = false;
		this.masteryValueLabel.raycastTarget = false;
	}

	// Token: 0x06003ADC RID: 15068 RVA: 0x00119264 File Offset: 0x00117464
	private void ShowSkin(SkinPresetGK2 skinPreset)
	{
		this.TryInitMaterials();
		this.hed.gameObject.SetActive(false);
		this.bdy.gameObject.SetActive(false);
		this.brd.gameObject.SetActive(false);
		this.arms.gameObject.SetActive(false);
		this.hrs.gameObject.SetActive(false);
		this.bdyOver.gameObject.SetActive(false);
		this.stn.gameObject.SetActive(false);
		if (skinPreset != null)
		{
			UIWorkerIcon.ApplySkinPart(this.hed, this.hed2, string.Format("{0}{1}", skinPreset.head.id, "_hed_static_down"), skinPreset, skinPreset.head);
			UIWorkerIcon.ApplySkinPart(this.bdy, this.bdy2, string.Format("{0}{1}", skinPreset.body.id, "_bdy_static_down"), skinPreset, skinPreset.body);
			if (skinPreset.body.id == 1002)
			{
				UIWorkerIcon.ApplySkinPart(this.bdyOver, string.Format("{0}{1}", skinPreset.body.id, "_bdy_over_static_down"));
			}
			if (skinPreset.isPlayerPreset)
			{
				UIWorkerIcon.ApplySkinPart(this.brd, this.brd2, string.Format("{0}{1}", skinPreset.beard.id, "_brd_static_down"), skinPreset, skinPreset.beard);
				UIWorkerIcon.ApplySkinPart(this.hrs, this.hrs2, string.Format("{0}{1}", skinPreset.hairstyle.id, "_hrs_static_down"), skinPreset, skinPreset.hairstyle);
				UIWorkerIcon.ApplySkinPart(this.arms, this.arms2, string.Format("{0}{1}", skinPreset.body.id, "_arm_static_down"), skinPreset, skinPreset.arms);
			}
			else
			{
				UIWorkerIcon.ApplySkinPart(this.stn, "1001_stn_static_down");
			}
		}
		base.gameObject.SetActive(true);
		this.hed2.gameObject.SetActive(this.hed.gameObject.activeSelf);
		this.hed2.sprite = this.hed.sprite;
		this.brd2.gameObject.SetActive(this.brd.gameObject.activeSelf);
		this.brd2.sprite = this.brd.sprite;
		this.hrs2.gameObject.SetActive(this.hrs.gameObject.activeSelf);
		this.hrs2.sprite = this.hrs.sprite;
		this.bdy2.gameObject.SetActive(this.bdy.gameObject.activeSelf);
		this.bdy2.sprite = this.bdy.sprite;
		this.arms2.gameObject.SetActive(this.arms.gameObject.activeSelf);
		this.arms2.sprite = this.arms.sprite;
		this.bdyOver2.gameObject.SetActive(this.bdyOver.gameObject.activeSelf);
		this.bdyOver2.sprite = this.bdyOver.sprite;
		this.stn2.gameObject.SetActive(this.stn.gameObject.activeSelf);
		this.stn2.sprite = this.stn.sprite;
	}

	// Token: 0x06003ADD RID: 15069 RVA: 0x001195EC File Offset: 0x001177EC
	private static void ApplySkinPart(Image image, string spriteName)
	{
		bool flag = LazySingletonSO<EasySpritesCollection>.Instance.HasSprite(spriteName);
		image.gameObject.SetActive(flag);
		if (!flag)
		{
			return;
		}
		image.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(spriteName, null);
	}

	// Token: 0x06003ADE RID: 15070 RVA: 0x00119627 File Offset: 0x00117827
	private static void ApplySkinPart(Image image, Image colorImage, string spriteName, SkinPresetGK2 skinPreset, SkinPresetPartGK2 part)
	{
		UIWorkerIcon.ApplySkinPart(image, spriteName);
		if (image.gameObject.activeSelf)
		{
			skinPreset.TryToApply(colorImage, colorImage.name, part);
		}
	}

	// Token: 0x04002E72 RID: 11890
	private const string BODY_SPRITE_PART_WITHOUT_ID = "_bdy_static_down";

	// Token: 0x04002E73 RID: 11891
	private const string ARMS_SPRITE_PART_WITHOUT_ID = "_arm_static_down";

	// Token: 0x04002E74 RID: 11892
	private const string HEAD_SPRITE_PART_WITHOUT_ID = "_hed_static_down";

	// Token: 0x04002E75 RID: 11893
	private const string BEARD_SPRITE_PART_WITHOUT_ID = "_brd_static_down";

	// Token: 0x04002E76 RID: 11894
	private const string HAIRSTYLE_SPRITE_PART_WITHOUT_ID = "_hrs_static_down";

	// Token: 0x04002E77 RID: 11895
	private const string BODY_OVER_SPRITE_PART_WITHOUT_ID = "_bdy_over_static_down";

	// Token: 0x04002E78 RID: 11896
	private const string STONE_SPRITE_ID = "1001_stn_static_down";

	// Token: 0x04002E79 RID: 11897
	[SerializeField]
	private GameObject talentParent;

	// Token: 0x04002E7A RID: 11898
	[SerializeField]
	private RectTransform workerIconParent;

	// Token: 0x04002E7B RID: 11899
	[SerializeField]
	private Vector2 workerIconPositionWithTalent = new Vector2(0f, 8f);

	// Token: 0x04002E7C RID: 11900
	[SerializeField]
	private Vector2 workerIconPositionWithoutTalent = new Vector2(0f, 2f);

	// Token: 0x04002E7D RID: 11901
	[SerializeField]
	private Image talentBackImage;

	// Token: 0x04002E7E RID: 11902
	[SerializeField]
	private TextMeshProUGUI talentIconLabel;

	// Token: 0x04002E7F RID: 11903
	[SerializeField]
	private TextMeshProUGUI masteryValueLabel;

	// Token: 0x04002E80 RID: 11904
	[SerializeField]
	private TextStyleComponent masteryValueTextStyleComponent;

	// Token: 0x04002E81 RID: 11905
	[SerializeField]
	private List<UIWorkerIcon.TalentViewData> viewDatas = new List<UIWorkerIcon.TalentViewData>();

	// Token: 0x04002E82 RID: 11906
	[SerializeField]
	private Image hed;

	// Token: 0x04002E83 RID: 11907
	[SerializeField]
	private Image brd;

	// Token: 0x04002E84 RID: 11908
	[SerializeField]
	private Image hrs;

	// Token: 0x04002E85 RID: 11909
	[SerializeField]
	private Image bdy;

	// Token: 0x04002E86 RID: 11910
	[SerializeField]
	private Image arms;

	// Token: 0x04002E87 RID: 11911
	[SerializeField]
	private Image hed2;

	// Token: 0x04002E88 RID: 11912
	[SerializeField]
	private Image brd2;

	// Token: 0x04002E89 RID: 11913
	[SerializeField]
	private Image hrs2;

	// Token: 0x04002E8A RID: 11914
	[SerializeField]
	private Image bdy2;

	// Token: 0x04002E8B RID: 11915
	[SerializeField]
	private Image arms2;

	// Token: 0x04002E8C RID: 11916
	[SerializeField]
	private Image stn;

	// Token: 0x04002E8D RID: 11917
	[SerializeField]
	private Image stn2;

	// Token: 0x04002E8E RID: 11918
	[SerializeField]
	private Image bdyOver;

	// Token: 0x04002E8F RID: 11919
	[SerializeField]
	private Image bdyOver2;

	// Token: 0x04002E90 RID: 11920
	private bool materialsInitialized;

	// Token: 0x020008CE RID: 2254
	[Serializable]
	private class TalentViewData
	{
		// Token: 0x04002E91 RID: 11921
		public string talentId;

		// Token: 0x04002E92 RID: 11922
		public Sprite backSprite;

		// Token: 0x04002E93 RID: 11923
		public TextStyle talentStyle;
	}
}

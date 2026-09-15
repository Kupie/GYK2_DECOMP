using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008D7 RID: 2263
public class WorldZoneWidget : LazyWidget<WorldZoneWidgetData>
{
	// Token: 0x06003AFA RID: 15098 RVA: 0x00119BD0 File Offset: 0x00117DD0
	public override void Redraw()
	{
		bool flag = this.data.WorldZoneData != null;
		bool insideTown = this.data.InsideTown;
		this.townSubZoneParent.gameObject.SetActive(false);
		this.worldZoneLabel.text = "";
		this.worldZoneLabel.ForceMeshUpdate(true, true);
		if (insideTown)
		{
			bool flag2 = this.data.TownSubZone != null;
			base.gameObject.SetActive(true);
			string text = "reputation-citizens".FontIcon() + "\u2060" + this.qualityEnoughStyle.ApplyStyleToString(string.Format("{0}", MainGame.Instance.GameSave.townSystem.Quality), false, true);
			this.worldZoneLabel.SetText(LLBase.L("town_zone") + " " + text);
			this.worldZoneLabel.ForceMeshUpdate(true, true);
			if (flag2)
			{
				this.townSubZoneParent.gameObject.SetActive(true);
				this.townSubZoneLabel.text = LLBase.L(this.data.TownSubZone.id);
			}
			((RectTransform)base.transform).RefreshContentFitter();
			if (flag2)
			{
				if (this.townSubZoneParent.sizeDelta.y > this.subTownImageSizeBorder)
				{
					this.subTownImage.sprite = this.subTownSpr2;
					return;
				}
				this.subTownImage.sprite = this.subTownSpr1;
				return;
			}
		}
		else
		{
			WorldZoneData worldZoneData = this.data.WorldZoneData;
			WorldZoneDef.DisplayType displayType = ((worldZoneData != null) ? worldZoneData.Definition.displayType : WorldZoneDef.DisplayType.None);
			if (flag && displayType != WorldZoneDef.DisplayType.Hidden)
			{
				base.gameObject.SetActive(true);
				string text2 = LLBase.L("wz_" + this.data.WorldZoneData.id);
				if (this.data.WorldZoneData.IsContainer && (!(this.data.WorldZoneData.id == "resurrection") || MainGame.PlayerData.GetResInt("zombies_limit_mechanic") != 0) && displayType != WorldZoneDef.DisplayType.None)
				{
					text2 += " ";
					if (this.data.WorldZoneData.GetTotalQuality() >= 0f)
					{
						text2 += this.data.WorldZoneData.GetQualityString(this.qualityEnoughStyle);
					}
					else
					{
						text2 += this.data.WorldZoneData.GetQualityString(this.qualityNotEnoughStyle);
					}
				}
				this.worldZoneLabel.SetText(text2);
				this.worldZoneLabel.ForceMeshUpdate(true, true);
			}
			else
			{
				base.gameObject.SetActive(false);
			}
			((RectTransform)base.transform).RefreshContentFitter();
		}
	}

	// Token: 0x06003AFB RID: 15099 RVA: 0x00119E78 File Offset: 0x00118078
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new WorldZoneWidgetData(new WorldZoneData("493_dev_playground_test", "PortArea", Vector3.zero, default(Rect))));
	}

	// Token: 0x04002E9D RID: 11933
	[SerializeField]
	private TextMeshProUGUI worldZoneLabel;

	// Token: 0x04002E9E RID: 11934
	[SerializeField]
	private TextMeshProUGUI townSubZoneLabel;

	// Token: 0x04002E9F RID: 11935
	[SerializeField]
	private RectTransform townSubZoneParent;

	// Token: 0x04002EA0 RID: 11936
	[SerializeField]
	private Image subTownImage;

	// Token: 0x04002EA1 RID: 11937
	[SerializeField]
	private Sprite subTownSpr1;

	// Token: 0x04002EA2 RID: 11938
	[SerializeField]
	private Sprite subTownSpr2;

	// Token: 0x04002EA3 RID: 11939
	[SerializeField]
	private float subTownImageSizeBorder = 20f;

	// Token: 0x04002EA4 RID: 11940
	[SerializeField]
	private TextStyle qualityEnoughStyle;

	// Token: 0x04002EA5 RID: 11941
	[SerializeField]
	private TextStyle qualityNotEnoughStyle;
}

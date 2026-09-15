using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000926 RID: 2342
public class TalentLevelUpsWidget : LazyWidget<TalentLevelUpsWidgetData>
{
	// Token: 0x06003DB8 RID: 15800 RVA: 0x00126E45 File Offset: 0x00125045
	public override void Init()
	{
		base.Init();
		TalentLevelUpDef.Link();
	}

	// Token: 0x06003DB9 RID: 15801 RVA: 0x00126E54 File Offset: 0x00125054
	public override void Redraw()
	{
		base.Redraw();
		this.HideCurrentElements();
		this.HideConnectors();
		if (this.data.ZombieWgoData == null)
		{
			this.currentViewData = this.viewDatas.Find((TalentLevelUpsWidget.TalentViewData d) => d.talentId == this.data.TalentData.id);
		}
		foreach (TalentLevelUpDef talentLevelUpDef in this.data.LevelUps)
		{
			TalentLevelUpWidgetData talentLevelUpWidgetData;
			if (talentLevelUpDef.isZombiePerk)
			{
				talentLevelUpWidgetData = new TalentLevelUpWidgetData(talentLevelUpDef, this.data.ZombieWgoData.GetLevelUpState(talentLevelUpDef), this.data.ZombieWgoData, new Action(this.Redraw));
			}
			else
			{
				talentLevelUpWidgetData = new TalentLevelUpWidgetData(talentLevelUpDef, this.data.TalentData.GetLevelUpState(talentLevelUpDef), this.currentViewData.pointIconId);
			}
			talentLevelUpWidgetData.localPosition = new Vector2(talentLevelUpDef.TreePos.x * this.elementOffset.x + this.edgeOffset.x, talentLevelUpDef.TreePos.y * this.elementOffset.y * -1f - this.edgeOffset.y);
			talentLevelUpWidgetData.downConnectorPos = talentLevelUpWidgetData.localPosition + this.connectorPortOffsetDown;
			talentLevelUpWidgetData.upConnectorPos = talentLevelUpWidgetData.localPosition + this.connectorPortOffsetUp;
			talentLevelUpWidgetData.leftConnectorPos = talentLevelUpWidgetData.localPosition + this.connectorPortOffsetLeft;
			talentLevelUpWidgetData.rightConnectorPos = talentLevelUpWidgetData.localPosition + this.connectorPortOffsetRight;
			TalentLevelUpWidget elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<TalentLevelUpWidget>(this.content);
			elementFromPool.gameObject.name = talentLevelUpDef.id;
			elementFromPool.transform.localScale = Vector3.one;
			((RectTransform)elementFromPool.transform).sizeDelta = this.elementSize;
			elementFromPool.Draw(talentLevelUpWidgetData);
			this.displayedTalentLevelUps.Add(elementFromPool);
		}
		((RectTransform)base.transform).RefreshContentFitter();
		for (int i = 0; i < this.displayedTalentLevelUps.Count; i++)
		{
			TalentLevelUpDef def = this.displayedTalentLevelUps[i].Data.Def;
			List<TalentLevelUpDef> childDefinitionList = def.childDefinitionList;
			for (int j = 0; j < childDefinitionList.Count; j++)
			{
				TalentLevelUpDef talentLevelUpDef2 = childDefinitionList[j];
				TalentLevelUpsConnector elementFromPool2 = UIPrefabsPooler.Instance.GetElementFromPool<TalentLevelUpsConnector>(this.content);
				TalentLevelUpsConnector elementFromPool3 = UIPrefabsPooler.Instance.GetElementFromPool<TalentLevelUpsConnector>(this.content);
				this.connectors.Add(elementFromPool2);
				this.connectors.Add(elementFromPool3);
				TalentLevelUpWidget talentLevelUpWidget = this.FindByDefinition(talentLevelUpDef2);
				if (talentLevelUpWidget == null)
				{
					Debug.LogError(string.Concat(new string[] { "Can't find child element:[", talentLevelUpDef2.id, "] for:[", def.id, "]" }));
				}
				elementFromPool2.Draw(this.displayedTalentLevelUps[i], talentLevelUpWidget, false);
				elementFromPool3.Draw(this.displayedTalentLevelUps[i], talentLevelUpWidget, true);
				TalentConnectorType talentConnectorType = elementFromPool2.TalentConnectorType;
				if (talentConnectorType != TalentConnectorType.Active)
				{
					if (talentConnectorType != TalentConnectorType.Inactive)
					{
						throw new ArgumentOutOfRangeException();
					}
					elementFromPool2.transform.SetParent(this.inactiveConnectorsContent.transform);
				}
				else
				{
					elementFromPool2.transform.SetParent(this.activeConnectorsContent.transform);
				}
				elementFromPool2.transform.SetAsFirstSibling();
				elementFromPool3.transform.SetParent(this.backgroundConnectorsContent.transform);
				elementFromPool3.transform.SetAsFirstSibling();
			}
		}
		this.backgroundConnectorsContent.transform.SetAsLastSibling();
		this.inactiveConnectorsContent.transform.SetAsLastSibling();
		this.activeConnectorsContent.transform.SetAsLastSibling();
		this.data.onTalentLevelPurchased = new TalentSystemData.DelTalentLevelPurchased(this.OnTalentLevelUpPurchased);
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06003DBA RID: 15802 RVA: 0x00127268 File Offset: 0x00125468
	private TalentLevelUpWidget FindByDefinition(TalentLevelUpDef def)
	{
		foreach (TalentLevelUpWidget talentLevelUpWidget in this.displayedTalentLevelUps)
		{
			if (talentLevelUpWidget.Data.Def == def)
			{
				return talentLevelUpWidget;
			}
		}
		Debug.Log("Cannot find element for definition " + def.id);
		return null;
	}

	// Token: 0x06003DBB RID: 15803 RVA: 0x001272E0 File Offset: 0x001254E0
	public override void Hide()
	{
		base.Hide();
		this.data.onTalentLevelPurchased = null;
	}

	// Token: 0x06003DBC RID: 15804 RVA: 0x0010A599 File Offset: 0x00108799
	private void OnTalentLevelUpPurchased(string talentId, string levelUpId)
	{
		this.Redraw();
	}

	// Token: 0x06003DBD RID: 15805 RVA: 0x001272F4 File Offset: 0x001254F4
	private void HideCurrentElements()
	{
		for (int i = this.displayedTalentLevelUps.Count - 1; i >= 0; i--)
		{
			TalentLevelUpWidget talentLevelUpWidget = this.displayedTalentLevelUps[i];
			talentLevelUpWidget.gameObject.SetActive(false);
			talentLevelUpWidget.ClearCallbacks();
			UIPrefabsPooler.Instance.ReleaseElementToPool<TalentLevelUpWidget>(talentLevelUpWidget);
		}
		this.displayedTalentLevelUps.Clear();
	}

	// Token: 0x06003DBE RID: 15806 RVA: 0x00127350 File Offset: 0x00125550
	private void HideConnectors()
	{
		foreach (TalentLevelUpsConnector talentLevelUpsConnector in this.connectors)
		{
			talentLevelUpsConnector.gameObject.SetActive(false);
			UIPrefabsPooler.Instance.ReleaseElementToPool<TalentLevelUpsConnector>(talentLevelUpsConnector);
		}
		this.connectors.Clear();
	}

	// Token: 0x06003DBF RID: 15807 RVA: 0x001273C0 File Offset: 0x001255C0
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new TalentLevelUpsWidgetData(MainGame.Instance.GameSave.talentSystemData.GetTalentBranch("talent_orange")));
	}

	// Token: 0x04003082 RID: 12418
	[SerializeField]
	private RectTransform content;

	// Token: 0x04003083 RID: 12419
	private List<TalentLevelUpWidget> displayedTalentLevelUps = new List<TalentLevelUpWidget>();

	// Token: 0x04003084 RID: 12420
	[Space]
	[SerializeField]
	private Vector2 edgeOffset = new Vector2(10f, 10f);

	// Token: 0x04003085 RID: 12421
	[SerializeField]
	private Vector2 elementOffset = new Vector2(210f, 80f);

	// Token: 0x04003086 RID: 12422
	[SerializeField]
	private Vector2 elementSize = new Vector2(32f, 32f);

	// Token: 0x04003087 RID: 12423
	[SerializeField]
	private GameObject backgroundConnectorsContent;

	// Token: 0x04003088 RID: 12424
	[SerializeField]
	private GameObject activeConnectorsContent;

	// Token: 0x04003089 RID: 12425
	[SerializeField]
	private GameObject inactiveConnectorsContent;

	// Token: 0x0400308A RID: 12426
	[SerializeField]
	private Vector2 connectorPortOffsetUp = new Vector2(0f, 16f);

	// Token: 0x0400308B RID: 12427
	[SerializeField]
	private Vector2 connectorPortOffsetDown = new Vector2(0f, -16f);

	// Token: 0x0400308C RID: 12428
	[SerializeField]
	private Vector2 connectorPortOffsetRight = new Vector2(16f, 0f);

	// Token: 0x0400308D RID: 12429
	[SerializeField]
	private Vector2 connectorPortOffsetLeft = new Vector2(-16f, 0f);

	// Token: 0x0400308E RID: 12430
	[SerializeField]
	private List<TalentLevelUpsWidget.TalentViewData> viewDatas = new List<TalentLevelUpsWidget.TalentViewData>();

	// Token: 0x0400308F RID: 12431
	private TalentLevelUpsWidget.TalentViewData currentViewData;

	// Token: 0x04003090 RID: 12432
	private List<TalentLevelUpsConnector> connectors = new List<TalentLevelUpsConnector>();

	// Token: 0x04003091 RID: 12433
	private Action onQueueChanged;

	// Token: 0x02000927 RID: 2343
	[Serializable]
	private class TalentViewData
	{
		// Token: 0x04003092 RID: 12434
		public string talentId;

		// Token: 0x04003093 RID: 12435
		public string pointIconId;
	}
}

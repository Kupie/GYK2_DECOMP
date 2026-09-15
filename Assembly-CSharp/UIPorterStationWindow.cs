using System;
using System.Collections.Generic;
using LazyBearTechnology;
using LinqTools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A09 RID: 2569
public class UIPorterStationWindow : LazyWindow<UIPorterStationWindowData>
{
	// Token: 0x06004524 RID: 17700 RVA: 0x0014707C File Offset: 0x0014527C
	public override void Init()
	{
		base.Init();
		SmoothMouseWheelScroll.EnsureForItem(base.GetComponentInChildren<ScrollRect>(true), 42f);
	}

	// Token: 0x06004525 RID: 17701 RVA: 0x00147098 File Offset: 0x00145298
	public override void Redraw()
	{
		base.Redraw();
		foreach (UIPorterStationItemCell uiporterStationItemCell in this.drawnCells)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<UIPorterStationItemCell>(uiporterStationItemCell);
		}
		this.drawnCells.Clear();
		BuildingDef buildingDef = GameBalance.Me.GetDataOrNull<BuildingDef>(this.data.Station.id + "_p");
		if (buildingDef == null)
		{
			buildingDef = GameBalance.Me.GetDataOrNull<BuildingDef>(this.data.Station.id + "_s");
			if (buildingDef == null)
			{
				this.uiInfoWidget.Draw(new UIInfoWidgetData(this.data.Station, "i_b_grn_zombie_delivery", false));
			}
			else
			{
				this.uiInfoWidget.Draw(new UIInfoWidgetData(this.data.Station, buildingDef.BuildResultIcon, false));
			}
		}
		else
		{
			this.uiInfoWidget.Draw(new UIInfoWidgetData(this.data.Station, buildingDef.BuildResultIcon, false));
		}
		if (this.data.PorterStationDef.items.Count > 0)
		{
			this.noItemsLabel.gameObject.SetActive(false);
			this.grid.gameObject.SetActive(true);
			using (List<NeedItemData>.Enumerator enumerator2 = this.data.PorterStationDef.items.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					NeedItemData needItemData = enumerator2.Current;
					UIPorterStationItemCell elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UIPorterStationItemCell>(base.transform);
					elementFromPool.Draw(new Item(needItemData.id, 1), this.data.Station.GetGameResInt(needItemData.id) == 1, this.data.OnItemCellPress);
					this.drawnCells.Add(elementFromPool);
				}
				goto IL_01F5;
			}
		}
		this.noItemsLabel.gameObject.SetActive(true);
		this.grid.gameObject.SetActive(false);
		IL_01F5:
		this.grid.Draw(this.drawnCells.Select((UIPorterStationItemCell go) => go.RectTransform).ToList<RectTransform>());
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x06004526 RID: 17702 RVA: 0x00147308 File Offset: 0x00145508
	[LazyUITest]
	protected override void TestDraw()
	{
		WgoData wgoData = new WgoData("porter_station_carrier", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId);
		this.Open(new UIPorterStationWindowData(wgoData));
	}

	// Token: 0x040035FD RID: 13821
	[SerializeField]
	private TextMeshProUGUI noItemsLabel;

	// Token: 0x040035FE RID: 13822
	[SerializeField]
	private FlexibleHorizontalSizeGridLayoutGroup grid;

	// Token: 0x040035FF RID: 13823
	[SerializeField]
	private UIInfoWidget uiInfoWidget;

	// Token: 0x04003600 RID: 13824
	private List<UIPorterStationItemCell> drawnCells = new List<UIPorterStationItemCell>();
}

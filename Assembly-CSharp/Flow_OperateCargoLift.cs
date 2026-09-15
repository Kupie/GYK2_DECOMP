using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

// Token: 0x020004CE RID: 1230
[Name("Operate Cargo Lift", 0)]
[Category("Game")]
public class Flow_OperateCargoLift : GKCustomFlowNodeWithWgoData
{
	// Token: 0x06002083 RID: 8323 RVA: 0x00099CE0 File Offset: 0x00097EE0
	protected override void RegisterPorts()
	{
		base.RegisterPorts();
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SendItemsToCargo), "");
		this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		this.result = base.AddValueOutput<bool>("result".CapitalizeFirst(), () => this.success, "");
	}

	// Token: 0x06002084 RID: 8324 RVA: 0x00099D5C File Offset: 0x00097F5C
	private void SendItemsToCargo(Flow flow)
	{
		WgoData wgoData = base.GetWgoData();
		if (wgoData == null)
		{
			this.success = false;
			this.@out.Call(flow);
			return;
		}
		if (!string.IsNullOrEmpty(wgoData.GameResStr.Get("target_storage_wgo", "")))
		{
			this.success = false;
			this.@out.Call(flow);
			return;
		}
		PorterStationDef data = GameBalance.Me.GetData<PorterStationDef>(wgoData.id);
		if (data == null)
		{
			Debug.LogError(string.Format("CargoLift {0} has no PorterStationDef configured!", wgoData.UniqueId));
			this.success = false;
			this.@out.Call(flow);
			return;
		}
		WgoData wgoData2 = null;
		if (data.customTargets != null && data.customTargets.Count > 0)
		{
			foreach (string text in data.customTargets)
			{
				if (!string.IsNullOrEmpty(text))
				{
					List<WgoData> wgoDataList = MainGame.WorldData.GetWgoDataList(text);
					if (wgoDataList != null && wgoDataList.Count > 0)
					{
						wgoData2 = wgoDataList[0];
						break;
					}
				}
			}
		}
		if (wgoData2 == null)
		{
			Debug.LogError("[Flow_OperateCargoLift]: Target storage wgo was not found!");
			this.success = false;
			this.@out.Call(flow);
			return;
		}
		int inventorySize = wgoData.Inventory.Data.InventorySize;
		if (inventorySize <= 0)
		{
			Debug.LogError(string.Format("[{0}]: CargoLift {1} has invalid inventory size!", "Flow_OperateCargoLift", wgoData.UniqueId));
			this.success = false;
			this.@out.Call(flow);
			return;
		}
		wgoData.Inventory.Clear();
		bool flag = false;
		int num = 0;
		foreach (NeedItemData needItemData in data.items)
		{
			if (wgoData.GetGameResInt(needItemData.id) == 1)
			{
				WorldZoneData worldZoneData = wgoData.WorldZoneData;
				if (worldZoneData == null)
				{
					Debug.LogWarning(string.Format("[{0}]: CargoLift {1} is not in any world zone!", "Flow_OperateCargoLift", wgoData.UniqueId));
				}
				else
				{
					MultiInventory multiInventory = new MultiInventory(worldZoneData, null, false);
					int totalCount = multiInventory.GetTotalCount(needItemData.id);
					if (totalCount <= 0)
					{
						Debug.LogWarning(string.Concat(new string[] { "[Flow_OperateCargoLift]: Item ", needItemData.id, " not found in zone ", worldZoneData.id, "!" }));
					}
					else
					{
						int num2 = inventorySize - num;
						if (num2 <= 0)
						{
							break;
						}
						int num3 = Mathf.Min(totalCount, num2);
						List<Item> list = multiInventory.RemoveItemByCount(needItemData.id, num3);
						if (list.Count > 0)
						{
							foreach (Item item in list)
							{
								wgoData.Inventory.AddItemToInventory(item, null, false);
								num += item.Count;
							}
							flag = true;
						}
						else
						{
							Debug.LogWarning("[Flow_OperateCargoLift]: Failed to remove item " + needItemData.id + " from zone MultiInventory!");
						}
					}
				}
			}
		}
		if (flag)
		{
			wgoData.SetCustomAnimationTrigger("");
			wgoData.GameResStr.Set("target_storage_wgo", wgoData2.id);
		}
		this.success = flag;
		this.@out.Call(flow);
	}

	// Token: 0x04001D21 RID: 7457
	private FlowInput @in;

	// Token: 0x04001D22 RID: 7458
	private FlowOutput @out;

	// Token: 0x04001D23 RID: 7459
	private ValueOutput<bool> result;

	// Token: 0x04001D24 RID: 7460
	private bool success;
}

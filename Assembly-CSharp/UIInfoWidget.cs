using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200089D RID: 2205
public class UIInfoWidget : LazyWidget<UIInfoWidgetData>
{
	// Token: 0x1700086E RID: 2158
	// (get) Token: 0x060038CD RID: 14541 RVA: 0x00110D81 File Offset: 0x0010EF81
	// (set) Token: 0x060038CE RID: 14542 RVA: 0x00110D88 File Offset: 0x0010EF88
	public static bool IsDisabled { get; set; }

	// Token: 0x1700086F RID: 2159
	// (get) Token: 0x060038CF RID: 14543 RVA: 0x00110D90 File Offset: 0x0010EF90
	public Sprite Icon
	{
		get
		{
			return this.icon.sprite;
		}
	}

	// Token: 0x060038D0 RID: 14544 RVA: 0x00110D9D File Offset: 0x0010EF9D
	public override void Draw(UIInfoWidgetData data)
	{
		if (UIInfoWidget.IsDisabled)
		{
			this.Hide();
			return;
		}
		base.Draw(data);
	}

	// Token: 0x060038D1 RID: 14545 RVA: 0x00110DB4 File Offset: 0x0010EFB4
	public override void Redraw()
	{
		base.Redraw();
		this.SubscrubeToWgoEvents();
		this.header.text = this.data.Header;
		this.icon.sprite = this.data.Icon;
		this.UpdateBackgroundIcon();
		this.UpdateDescription(null);
		this.UpdateWorkerIcon();
	}

	// Token: 0x060038D2 RID: 14546 RVA: 0x00110E0C File Offset: 0x0010F00C
	public override void Hide()
	{
		base.Hide();
		this.UnsubscribeFromWgoEvents();
	}

	// Token: 0x060038D3 RID: 14547 RVA: 0x00110E1C File Offset: 0x0010F01C
	private void SubscrubeToWgoEvents()
	{
		if (this.data == null)
		{
			return;
		}
		if (this.data.CraftComponent == null)
		{
			return;
		}
		ICraftable craftableObject = this.data.CraftComponent.CraftableObject;
		if (((craftableObject != null) ? craftableObject.CraftableObjectInventory : null) == null)
		{
			return;
		}
		if (!this.subscribedWgoEvents)
		{
			this.data.CraftComponent.CraftableObject.CraftableObjectInventory.OnItemsAdd += this.UpdateDescription;
			this.data.CraftComponent.CraftableObject.CraftableObjectInventory.OnItemsRemove += this.UpdateDescription;
			this.subscribedWgoEvents = true;
		}
	}

	// Token: 0x060038D4 RID: 14548 RVA: 0x00110EBC File Offset: 0x0010F0BC
	private void UnsubscribeFromWgoEvents()
	{
		if (this.data == null)
		{
			return;
		}
		if (this.data.CraftComponent == null)
		{
			return;
		}
		if (this.data.CraftComponent.CraftableObject.CraftableObjectInventory == null)
		{
			return;
		}
		if (this.subscribedWgoEvents)
		{
			this.data.CraftComponent.CraftableObject.CraftableObjectInventory.OnItemsAdd -= this.UpdateDescription;
			this.data.CraftComponent.CraftableObject.CraftableObjectInventory.OnItemsRemove -= this.UpdateDescription;
			this.subscribedWgoEvents = false;
		}
	}

	// Token: 0x060038D5 RID: 14549 RVA: 0x00110F54 File Offset: 0x0010F154
	private void UpdateWorkerIcon()
	{
		this.workerIcon.transform.parent.parent.gameObject.SetActive(false);
		this.workerIconGamepad.transform.parent.parent.gameObject.SetActive(false);
		this.workerIcon.Hide();
		this.workerIconGamepad.Hide();
		if (this.data.CraftComponent != null && this.data.CraftComponent.HasCraftsByBalance && this.data.Worker != null)
		{
			SkinPresetGK2 skinPresetGK = null;
			ZombieWgoData zombieWgoData = this.data.Worker as ZombieWgoData;
			if (zombieWgoData != null)
			{
				Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(zombieWgoData.UniqueId);
				if (wgoViewGlobal != null)
				{
					skinPresetGK = wgoViewGlobal.MainWgoPart.AnimationComponent.SkinPreset;
				}
			}
			else
			{
				PlayerController playerController = this.data.Worker as PlayerController;
				if (playerController != null)
				{
					skinPresetGK = playerController.View.PlayerAnimation.SkinPreset;
				}
			}
			if (LazyInput.IsGamepadActive)
			{
				this.workerIconGamepad.Show(this.data.Worker, skinPresetGK, GameBalance.Me.GetData<TalentDef>(this.data.WgoData.Definition.talent));
				this.workerIconGamepad.transform.parent.parent.gameObject.SetActive(true);
				return;
			}
			this.workerIcon.Show(this.data.Worker, skinPresetGK, GameBalance.Me.GetData<TalentDef>(this.data.WgoData.Definition.talent));
			this.workerIcon.transform.parent.parent.gameObject.SetActive(true);
		}
	}

	// Token: 0x060038D6 RID: 14550 RVA: 0x00111104 File Offset: 0x0010F304
	public void UpdateDescription(List<Item> items = null)
	{
		bool flag = this.TryShowLabeledWorldZoneQuality();
		if (!flag)
		{
			this.description.text = this.data.Description;
			if (!this.IsEmbalmTable())
			{
				if (string.IsNullOrEmpty(this.data.Description))
				{
					if (!string.IsNullOrEmpty(this.data.WorldZoneQuality))
					{
						TextMeshProUGUI textMeshProUGUI = this.description;
						textMeshProUGUI.text += this.worldZoneQualityStyle.ApplyStyleToString(this.data.WorldZoneQuality, false, true);
					}
				}
				else if (!string.IsNullOrEmpty(this.data.WorldZoneQuality))
				{
					TextMeshProUGUI textMeshProUGUI2 = this.description;
					textMeshProUGUI2.text = textMeshProUGUI2.text + ": " + this.worldZoneQualityStyle.ApplyStyleToString(this.data.WorldZoneQuality, false, true);
				}
			}
		}
		this.descriptionFuel.transform.parent.gameObject.SetActive(false);
		this.descriptionFuel2.transform.parent.gameObject.SetActive(false);
		this.descriptionTick.transform.parent.gameObject.SetActive(false);
		this.description.gameObject.SetActive(!string.IsNullOrEmpty(this.description.text));
		this.descriptionFuel.transform.parent.parent.gameObject.SetActive(false);
		this.descriptionFuel2.transform.parent.parent.gameObject.SetActive(false);
		if (this.data.CraftComponent == null)
		{
			return;
		}
		bool flag2 = flag;
		if (!flag && (this.data.CraftComponent.HasCraftsByBalance || this.data.WgoData.Definition.interactionType == WGODef.InteractionType.Survey || this.data.WgoData.Definition.interactionType == WGODef.InteractionType.Alchemy || this.data.WgoData.Definition.interactionType == WGODef.InteractionType.Autopsy))
		{
			if (this.data.CraftComponent.AvailableCrafts.Count > 0 && this.data.CraftComponent.AvailableCrafts[0].isFuelCraft)
			{
				ItemDef itemDef = this.data.CraftComponent.AvailableCrafts[0].FuelItemDef;
				string text = string.Format("{0}{1}/{2}", itemDef.id.FontIcon(), this.data.WgoData.Inventory.Data.GetTotalCountInInventory(itemDef.id, null, false), this.data.WgoData.Definition.emptyCellStackCount * this.data.WgoData.Inventory.Data.InventorySize);
				flag2 = this.TryShowLabeledFuelHeader(itemDef, text);
				if (!flag2)
				{
					this.descriptionFuel.transform.parent.gameObject.SetActive(true);
					this.descriptionFuel.transform.parent.parent.gameObject.SetActive(true);
					this.descriptionFuel.text = text;
					this.description.gameObject.SetActive(false);
					this.AttachHeaderFuelTooltip(itemDef, this.descriptionFuel.transform.parent.gameObject);
				}
			}
			else if (!string.IsNullOrEmpty(this.data.WgoData.Definition.fuelItemId))
			{
				ItemDef itemDef = this.data.WgoData.Definition.FuelItemDef;
				string text2 = string.Format("{0}{1}", itemDef.id.FontIcon(), this.data.WgoData.GetCraftableMultiInventory(this.data.ExcludePlayerFromMultiinventoryWhenCountItemsForFuel).GetTotalCount(itemDef.id));
				flag2 = this.TryShowLabeledFuelHeader(itemDef, text2);
				if (!flag2)
				{
					this.descriptionFuel.transform.parent.gameObject.SetActive(true);
					this.descriptionFuel.transform.parent.parent.gameObject.SetActive(true);
					this.descriptionFuel.text = text2;
					this.description.gameObject.SetActive(false);
					this.AttachHeaderFuelTooltip(itemDef, this.descriptionFuel.transform.parent.gameObject);
					if (!string.IsNullOrEmpty(this.data.WgoData.Definition.fuelItemId2))
					{
						itemDef = this.data.WgoData.Definition.FuelItemDef2;
						this.descriptionFuel2.transform.parent.gameObject.SetActive(true);
						this.descriptionFuel2.transform.parent.parent.gameObject.SetActive(true);
						this.descriptionFuel2.text = string.Format("{0}{1}", itemDef.id.FontIcon(), this.data.WgoData.GetCraftableMultiInventory(this.data.ExcludePlayerFromMultiinventoryWhenCountItemsForFuel).GetTotalCount(itemDef.id));
						this.description.gameObject.SetActive(false);
						this.AttachHeaderFuelTooltip(itemDef, this.descriptionFuel2.transform.parent.gameObject);
					}
				}
			}
		}
		if (this.data.ShowTickDuration)
		{
			if (this.data.ShowTickDuration && this.data.CraftComponent != null && this.data.CraftComponent.HasCraftsByBalance && this.data.CraftComponent.AvailableCrafts[0].isAuto)
			{
				this.descriptionTick.transform.parent.gameObject.SetActive(true);
				TimeSpan timeSpan = TimeSpan.FromSeconds((double)this.data.WgoData.Definition.autocraftTickDuration.EvaluateFloat(this.data.WgoData));
				this.descriptionTick.text = "cell_time".FontIcon() + timeSpan.ToString("m\\:ss");
				this.descriptionFuel.transform.parent.parent.gameObject.SetActive(true);
				if (!flag2)
				{
					this.description.gameObject.SetActive(false);
				}
				this.AttachTickLengthTooltip();
				return;
			}
			this.descriptionTick.transform.parent.gameObject.SetActive(false);
		}
	}

	// Token: 0x060038D7 RID: 14551 RVA: 0x00111760 File Offset: 0x0010F960
	private bool IsEmbalmTable()
	{
		WgoData wgoData = this.data.WgoData;
		if (wgoData == null)
		{
			return false;
		}
		WGODef definition = wgoData.Definition;
		WGODef.InteractionType? interactionType = ((definition != null) ? new WGODef.InteractionType?(definition.interactionType) : null);
		WGODef.InteractionType interactionType2 = WGODef.InteractionType.Embalm;
		return (interactionType.GetValueOrDefault() == interactionType2) & (interactionType != null);
	}

	// Token: 0x060038D8 RID: 14552 RVA: 0x001117B4 File Offset: 0x0010F9B4
	private bool TryShowLabeledWorldZoneQuality()
	{
		if (this.IsEmbalmTable() || string.IsNullOrEmpty(this.data.WorldZoneQuality))
		{
			return false;
		}
		string worldZoneQualityHeaderId = this.GetWorldZoneQualityHeaderId();
		if (string.IsNullOrEmpty(worldZoneQualityHeaderId))
		{
			return false;
		}
		string text = this.worldZoneQualityStyle.ApplyStyleToString(this.data.WorldZoneQuality, false, true);
		this.description.text = (string.IsNullOrEmpty(this.data.Description) ? (UIInfoWidget.FormatLabeledHeader(worldZoneQualityHeaderId) + " " + text) : (this.data.Description + ": " + text));
		this.description.gameObject.SetActive(true);
		this.AttachLabeledHeaderTooltip(worldZoneQualityHeaderId);
		return true;
	}

	// Token: 0x060038D9 RID: 14553 RVA: 0x00111868 File Offset: 0x0010FA68
	private bool TryShowLabeledFuelHeader(ItemDef fuelItemDef, string fuelValue)
	{
		string labeledFuelHeaderId = this.GetLabeledFuelHeaderId(fuelItemDef);
		if (string.IsNullOrEmpty(labeledFuelHeaderId) || string.IsNullOrEmpty(fuelValue))
		{
			return false;
		}
		this.description.text = UIInfoWidget.FormatLabeledHeader(labeledFuelHeaderId) + " " + this.worldZoneQualityStyle.ApplyStyleToString(fuelValue, false, true);
		this.description.gameObject.SetActive(true);
		this.AttachLabeledHeaderTooltip(labeledFuelHeaderId);
		return true;
	}

	// Token: 0x060038DA RID: 14554 RVA: 0x001118D4 File Offset: 0x0010FAD4
	private void AttachLabeledHeaderTooltip(string headerLocId)
	{
		if (headerLocId != "hdr_factory_gears")
		{
			return;
		}
		UIMouseTooltip component = this.description.transform.parent.GetComponent<UIMouseTooltip>();
		if (component != null)
		{
			global::UnityEngine.Object.Destroy(component);
		}
		UIMouseTooltip.Attach(this.description.gameObject, "tt_factory_gears", null, true, false, default(UIMouseTooltipEdges), default(Vector2), null);
	}

	// Token: 0x060038DB RID: 14555 RVA: 0x00111940 File Offset: 0x0010FB40
	private static string FormatLabeledHeader(string headerLocId)
	{
		return LLBase.L(headerLocId).TrimEnd(new char[] { ' ', ':' }) + ":";
	}

	// Token: 0x060038DC RID: 14556 RVA: 0x00111968 File Offset: 0x0010FB68
	private string GetWorldZoneQualityHeaderId()
	{
		WgoData wgoData = this.data.WgoData;
		WorldZoneData worldZoneData = ((wgoData != null) ? wgoData.WorldZoneData : null);
		if (worldZoneData == null)
		{
			WgoData wgoData2 = this.data.WgoData;
			if (wgoData2 != null)
			{
				wgoData2.TryGetNearestBuilderWorldZone(out worldZoneData);
			}
		}
		string text;
		if (worldZoneData == null)
		{
			text = null;
		}
		else
		{
			WorldZoneDef definition = worldZoneData.Definition;
			text = ((definition != null) ? definition.qualityIcon : null);
		}
		string qualityIconHeaderId = UIInfoWidget.GetQualityIconHeaderId(text);
		if (!string.IsNullOrEmpty(qualityIconHeaderId))
		{
			return qualityIconHeaderId;
		}
		WgoData wgoData3 = this.data.WgoData;
		bool flag;
		if (wgoData3 == null)
		{
			flag = false;
		}
		else
		{
			WGODef definition2 = wgoData3.Definition;
			WGODef.InteractionType? interactionType = ((definition2 != null) ? new WGODef.InteractionType?(definition2.interactionType) : null);
			WGODef.InteractionType interactionType2 = WGODef.InteractionType.Autopsy;
			flag = (interactionType.GetValueOrDefault() == interactionType2) & (interactionType != null);
		}
		if (flag)
		{
			return "hdr_autopsy_bodies";
		}
		return null;
	}

	// Token: 0x060038DD RID: 14557 RVA: 0x00111A24 File Offset: 0x0010FC24
	private string GetLabeledFuelHeaderId(ItemDef fuelItemDef)
	{
		WgoData wgoData = this.data.WgoData;
		bool flag;
		if (wgoData == null)
		{
			flag = false;
		}
		else
		{
			WGODef definition = wgoData.Definition;
			WGODef.InteractionType? interactionType = ((definition != null) ? new WGODef.InteractionType?(definition.interactionType) : null);
			WGODef.InteractionType interactionType2 = WGODef.InteractionType.Autopsy;
			flag = (interactionType.GetValueOrDefault() == interactionType2) & (interactionType != null);
		}
		if (flag)
		{
			return "hdr_autopsy_bodies";
		}
		string qualityIconHeaderId = UIInfoWidget.GetQualityIconHeaderId((fuelItemDef != null) ? fuelItemDef.id : null);
		if (!string.IsNullOrEmpty(qualityIconHeaderId))
		{
			return qualityIconHeaderId;
		}
		return UIInfoWidget.GetQualityIconHeaderId((fuelItemDef != null) ? fuelItemDef.qualityIcon : null);
	}

	// Token: 0x060038DE RID: 14558 RVA: 0x00111AAC File Offset: 0x0010FCAC
	private static string GetQualityIconHeaderId(string icon)
	{
		if (icon == "gear")
		{
			return "hdr_factory_gears";
		}
		if (icon == "corpse" || icon == "body")
		{
			return "hdr_autopsy_bodies";
		}
		return null;
	}

	// Token: 0x060038DF RID: 14559 RVA: 0x00111AE4 File Offset: 0x0010FCE4
	private void AttachHeaderFuelTooltip(ItemDef fuelItemDef, GameObject chip)
	{
		if (fuelItemDef == null || chip == null)
		{
			return;
		}
		if (fuelItemDef.id == "fire")
		{
			UIMouseTooltip.Attach(chip, "tt_craft_fire", null, true, false, default(UIMouseTooltipEdges), default(Vector2), null);
			return;
		}
		if (fuelItemDef.id == "alchemy_flask")
		{
			UIMouseTooltip.Attach(chip, "tt_alchemy_flasks", null, true, false, default(UIMouseTooltipEdges), default(Vector2), null);
		}
		WgoData wgoData = this.data.WgoData;
		bool flag;
		if (wgoData == null)
		{
			flag = true;
		}
		else
		{
			WGODef definition = wgoData.Definition;
			WGODef.InteractionType? interactionType = ((definition != null) ? new WGODef.InteractionType?(definition.interactionType) : null);
			WGODef.InteractionType interactionType2 = WGODef.InteractionType.Survey;
			flag = !((interactionType.GetValueOrDefault() == interactionType2) & (interactionType != null));
		}
		if (flag)
		{
			return;
		}
		if (fuelItemDef.id == "faith")
		{
			UIMouseTooltip.Attach(chip, "tt_study_faith", null, true, false, default(UIMouseTooltipEdges), default(Vector2), "faith");
			return;
		}
		if (fuelItemDef.id == "science")
		{
			UIMouseTooltip.Attach(chip, "tt_study_science", null, true, false, default(UIMouseTooltipEdges), default(Vector2), "science");
		}
	}

	// Token: 0x060038E0 RID: 14560 RVA: 0x00111C28 File Offset: 0x0010FE28
	private void AttachTickLengthTooltip()
	{
		UIMouseTooltip.Attach(this.descriptionTick.transform.parent.gameObject, "tt_craft_tick_length", null, true, false, default(UIMouseTooltipEdges), default(Vector2), null);
	}

	// Token: 0x060038E1 RID: 14561 RVA: 0x00111C6C File Offset: 0x0010FE6C
	public void TurnOnTickDuration()
	{
		this.descriptionTick.transform.parent.gameObject.SetActive(true);
		this.descriptionFuel.transform.parent.parent.gameObject.SetActive(true);
		this.description.gameObject.SetActive(false);
		this.AttachTickLengthTooltip();
	}

	// Token: 0x060038E2 RID: 14562 RVA: 0x00111CCC File Offset: 0x0010FECC
	private void UpdateBackgroundIcon()
	{
		if (this.data == null)
		{
			return;
		}
		if (this.data.WgoData != null && this.data.WgoData.Definition != null && this.data.DefineIconBackgroundFromWgo && this.background != null)
		{
			if (this.data.WgoData != null)
			{
				if (this.data.WgoData.Definition.craftIconColor > 0 || this.data.WgoData.Definition.conveyorType != ConveyorElementType.None)
				{
					this.background.sprite = this.conveyorBgIcon;
					return;
				}
				this.background.sprite = this.defaultBgIcon;
				return;
			}
			else
			{
				this.background.sprite = this.defaultBgIcon;
			}
		}
	}

	// Token: 0x060038E3 RID: 14563 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04002D37 RID: 11575
	private const string FIRE_ITEM_ID = "fire";

	// Token: 0x04002D38 RID: 11576
	[SerializeField]
	private TextMeshProUGUI header;

	// Token: 0x04002D39 RID: 11577
	[SerializeField]
	private TextMeshProUGUI description;

	// Token: 0x04002D3A RID: 11578
	[SerializeField]
	private TextMeshProUGUI descriptionFuel;

	// Token: 0x04002D3B RID: 11579
	[SerializeField]
	private TextMeshProUGUI descriptionFuel2;

	// Token: 0x04002D3C RID: 11580
	[SerializeField]
	private TextMeshProUGUI descriptionTick;

	// Token: 0x04002D3D RID: 11581
	[SerializeField]
	private Image icon;

	// Token: 0x04002D3E RID: 11582
	[SerializeField]
	private Image background;

	// Token: 0x04002D3F RID: 11583
	[SerializeField]
	private UIWorkerIcon workerIcon;

	// Token: 0x04002D40 RID: 11584
	[SerializeField]
	private UIWorkerIcon workerIconGamepad;

	// Token: 0x04002D41 RID: 11585
	[SerializeField]
	private TextStyle worldZoneQualityStyle;

	// Token: 0x04002D42 RID: 11586
	[Space]
	[SerializeField]
	private Sprite defaultBgIcon;

	// Token: 0x04002D43 RID: 11587
	[SerializeField]
	private Sprite conveyorBgIcon;

	// Token: 0x04002D44 RID: 11588
	private bool subscribedWgoEvents;
}

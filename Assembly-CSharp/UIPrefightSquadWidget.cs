using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020009CB RID: 2507
public class UIPrefightSquadWidget : LazyWidget<UIPrefightSquadWidgetData>
{
	// Token: 0x17000A1A RID: 2586
	// (get) Token: 0x060042C5 RID: 17093 RVA: 0x0013D1B5 File Offset: 0x0013B3B5
	public UIPrefightSquadWidgetData Data
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x060042C6 RID: 17094 RVA: 0x0013D1C0 File Offset: 0x0013B3C0
	public override void Init()
	{
		base.Init();
		this.buttonDefault.onClick.AddListener(new UnityAction(this.OnPressedDefault));
		this.buttonTurnedOn.onClick.AddListener(new UnityAction(this.OnPressedTurnedOn));
		UIMouseTooltip.Attach(this.powerLabel.gameObject, "tt_prefight_3", null, true, false, default(UIMouseTooltipEdges), default(Vector2), null);
	}

	// Token: 0x060042C7 RID: 17095 RVA: 0x0013D238 File Offset: 0x0013B438
	public override void Redraw()
	{
		base.Redraw();
		this.data.OnRedraw = new Action(this.Redraw);
		this.gamepadNavigationItem.SetCallbacks(new UnityAction(this.Empty), new UnityAction(this.Empty), new UnityAction(this.Empty));
		this.powerLabel.text = string.Format("{0}{1}", "barracks".FontIcon(), this.data.SquadPower);
		if (this.data.WgoData == null || !this.data.HasAnyFighterInSquad)
		{
			this.shading.gameObject.SetActive(true);
			this.icon.gameObject.SetActive(false);
			this.iconLocked.gameObject.SetActive(true);
			this.squadWeaponsLabel.text = "squad_equip_icon-no_equip".FontIcon() ?? "";
			this.buttonDefault.interactable = false;
			this.buttonDefault.gameObject.SetActive(true);
			this.buttonTurnedOn.gameObject.SetActive(false);
			GameObject[] array = this.turnedOnSelection;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			if (this.data.IsMercenary)
			{
				this.buttonDefault.gameObject.SetActive(false);
				this.turnedOnMercenaries.SetActive(true);
				return;
			}
		}
		else
		{
			this.icon.gameObject.SetActive(true);
			this.iconLocked.gameObject.SetActive(false);
			this.shading.gameObject.SetActive(false);
			this.squadWeaponsLabel.text = string.Empty;
			for (int j = 0; j < this.data.FightersWeapons.Count; j++)
			{
				if (this.data.FightersWeapons[j] == ItemType.None || this.data.FightersArmors[j] == ItemType.None)
				{
					TextMeshProUGUI textMeshProUGUI = this.squadWeaponsLabel;
					textMeshProUGUI.text += "squad_equip_icon-no_equip".FontIcon();
				}
				else if (this.data.FightersWeapons[j] == ItemType.Pike)
				{
					TextMeshProUGUI textMeshProUGUI2 = this.squadWeaponsLabel;
					textMeshProUGUI2.text += "squad_equip_icon-spear".FontIcon();
				}
				else if (this.data.FightersWeapons[j] == ItemType.Bow)
				{
					TextMeshProUGUI textMeshProUGUI3 = this.squadWeaponsLabel;
					textMeshProUGUI3.text += "squad_equip_icon-arrow".FontIcon();
				}
			}
			if (this.data.IsTurnedOn)
			{
				this.buttonTurnedOn.interactable = !this.data.IsMercenary;
				if (this.buttonTurnedOn.interactable)
				{
					this.gamepadNavigationItem.SetCallbacks(new UnityAction(this.buttonTurnedOn.ForceOnEnter), new UnityAction(this.buttonTurnedOn.ForceOnExit), new UnityAction(this.buttonTurnedOn.ForceOnClick));
				}
				this.buttonDefault.gameObject.SetActive(false);
				this.buttonTurnedOn.gameObject.SetActive(true);
				GameObject[] array = this.turnedOnSelection;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(true);
				}
			}
			else
			{
				this.buttonDefault.interactable = this.data.CanBeTurnedOn;
				if (this.buttonDefault.interactable)
				{
					this.gamepadNavigationItem.SetCallbacks(new UnityAction(this.buttonDefault.ForceOnEnter), new UnityAction(this.buttonDefault.ForceOnExit), new UnityAction(this.buttonDefault.ForceOnClick));
				}
				this.buttonTurnedOn.gameObject.SetActive(false);
				this.buttonDefault.gameObject.SetActive(true);
				GameObject[] array = this.turnedOnSelection;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(false);
				}
			}
			if (this.data.IsMercenary)
			{
				this.buttonDefault.gameObject.SetActive(false);
				this.buttonTurnedOn.gameObject.SetActive(false);
				this.turnedOnMercenaries.SetActive(true);
			}
		}
	}

	// Token: 0x060042C8 RID: 17096 RVA: 0x00002318 File Offset: 0x00000518
	private void Empty()
	{
	}

	// Token: 0x060042C9 RID: 17097 RVA: 0x0013D656 File Offset: 0x0013B856
	private void OnPressedDefault()
	{
		Action<UIPrefightSquadWidget> onPressedDefault = this.data.OnPressedDefault;
		if (onPressedDefault == null)
		{
			return;
		}
		onPressedDefault(this);
	}

	// Token: 0x060042CA RID: 17098 RVA: 0x0013D66E File Offset: 0x0013B86E
	private void OnPressedTurnedOn()
	{
		Action<UIPrefightSquadWidget> onPressedTurnedOn = this.data.OnPressedTurnedOn;
		if (onPressedTurnedOn == null)
		{
			return;
		}
		onPressedTurnedOn(this);
	}

	// Token: 0x060042CB RID: 17099 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003418 RID: 13336
	[SerializeField]
	private LazyButton buttonDefault;

	// Token: 0x04003419 RID: 13337
	[SerializeField]
	private LazyButton buttonTurnedOn;

	// Token: 0x0400341A RID: 13338
	[SerializeField]
	private GameObject turnedOnMercenaries;

	// Token: 0x0400341B RID: 13339
	[SerializeField]
	private Image icon;

	// Token: 0x0400341C RID: 13340
	[SerializeField]
	private Image iconLocked;

	// Token: 0x0400341D RID: 13341
	[SerializeField]
	private TextMeshProUGUI powerLabel;

	// Token: 0x0400341E RID: 13342
	[SerializeField]
	private TextMeshProUGUI squadWeaponsLabel;

	// Token: 0x0400341F RID: 13343
	[SerializeField]
	private GameObject[] turnedOnSelection;

	// Token: 0x04003420 RID: 13344
	[SerializeField]
	private GameObject shading;

	// Token: 0x04003421 RID: 13345
	[SerializeField]
	private GamepadNavigationItem gamepadNavigationItem;
}

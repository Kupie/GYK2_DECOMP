using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000943 RID: 2371
public class PerkWidget : LazyWidget<PerkWidgetData>
{
	// Token: 0x1700096A RID: 2410
	// (get) Token: 0x06003E78 RID: 15992 RVA: 0x0012A100 File Offset: 0x00128300
	public PerkData PerkData
	{
		get
		{
			return this.data.PerkData;
		}
	}

	// Token: 0x1700096B RID: 2411
	// (get) Token: 0x06003E79 RID: 15993 RVA: 0x0012A10D File Offset: 0x0012830D
	public GamepadNavigationItem GamepadNavigationItem
	{
		get
		{
			this.TryInitGamepadNavigationItem();
			return this.gamepadNavigationItem;
		}
	}

	// Token: 0x06003E7A RID: 15994 RVA: 0x0012A11C File Offset: 0x0012831C
	public override void Redraw()
	{
		base.Redraw();
		this.icon.sprite = this.data.PerkData.Definition.Icon;
		if (this.icon.sprite == null)
		{
			this.icon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("i_placeholder", null);
		}
		this.icon.SetNativeSize();
		this.onPress = this.data.OnPress;
		this.onOver = this.data.OnOver;
		this.onOut = this.data.OnOut;
		if (this.dbgImageText != null)
		{
			this.dbgImageText.text = this.data.PerkData.id;
			this.dbgImageText.gameObject.SetActive(this.icon.sprite == null);
		}
		this.blackout.gameObject.SetActive(!this.data.IsActive);
		this.durationLabel.gameObject.SetActive(!this.data.PerkData.Definition.hiddenTimer);
		this.durationLabel.text = PerkSystemData.GetFormattedDuration(this.data.PerkData.currentDuration);
	}

	// Token: 0x06003E7B RID: 15995 RVA: 0x0012A26B File Offset: 0x0012846B
	public void ClearCallbacks()
	{
		this.onPress = null;
		this.onOver = null;
		this.onOut = null;
	}

	// Token: 0x06003E7C RID: 15996 RVA: 0x0012A284 File Offset: 0x00128484
	private void Awake()
	{
		this.button.onDown.AddListener(new UnityAction(this.OnPress));
		this.button.onEnter.AddListener(new UnityAction(this.OnOver));
		this.button.onExit.AddListener(new UnityAction(this.OnOut));
		this.selectionFrame.SetActive(false);
		this.TryInitGamepadNavigationItem();
	}

	// Token: 0x06003E7D RID: 15997 RVA: 0x0012A2F8 File Offset: 0x001284F8
	private void TryInitGamepadNavigationItem()
	{
		if (this.gamepadNavigationItem == null)
		{
			this.gamepadNavigationItem = base.GetComponent<GamepadNavigationItem>();
			if (this.gamepadNavigationItem != null)
			{
				this.gamepadNavigationItem.SetCallbacks(new UnityAction(this.button.ForceOnEnter), new UnityAction(this.button.ForceOnExit), new UnityAction(this.button.ForceOnClick));
			}
		}
	}

	// Token: 0x06003E7E RID: 15998 RVA: 0x0012A36B File Offset: 0x0012856B
	private void OnPress()
	{
		Action<PerkWidgetData> action = this.onPress;
		if (action == null)
		{
			return;
		}
		action(this.data);
	}

	// Token: 0x06003E7F RID: 15999 RVA: 0x0012A383 File Offset: 0x00128583
	private void OnOver()
	{
		this.selectionFrame.SetActive(true);
		UITooltip.ShowPerkWidget(this);
		Action<PerkWidgetData> action = this.onOver;
		if (action == null)
		{
			return;
		}
		action(this.data);
	}

	// Token: 0x06003E80 RID: 16000 RVA: 0x0012A3AD File Offset: 0x001285AD
	private void OnOut()
	{
		this.selectionFrame.SetActive(false);
		UITooltip.Hide();
		Action<PerkWidgetData> action = this.onOut;
		if (action == null)
		{
			return;
		}
		action(this.data);
	}

	// Token: 0x06003E81 RID: 16001 RVA: 0x0012A3D6 File Offset: 0x001285D6
	private void OnDisable()
	{
		this.selectionFrame.SetActive(false);
		if (UITooltip.IsTooltipShowingAtTarget(base.transform as RectTransform))
		{
			UITooltip.HideImmediately();
		}
	}

	// Token: 0x06003E82 RID: 16002 RVA: 0x0012A3FB File Offset: 0x001285FB
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new PerkWidgetData(new PerkData("buff_alchomaster"), true, null, null, null));
	}

	// Token: 0x04003114 RID: 12564
	[SerializeField]
	private LazyButton button;

	// Token: 0x04003115 RID: 12565
	[SerializeField]
	private Image icon;

	// Token: 0x04003116 RID: 12566
	[SerializeField]
	private Image blackout;

	// Token: 0x04003117 RID: 12567
	[SerializeField]
	private GameObject selectionFrame;

	// Token: 0x04003118 RID: 12568
	[SerializeField]
	protected TextMeshProUGUI durationLabel;

	// Token: 0x04003119 RID: 12569
	[Space]
	[SerializeField]
	private TextMeshProUGUI dbgImageText;

	// Token: 0x0400311A RID: 12570
	private Action<PerkWidgetData> onPress;

	// Token: 0x0400311B RID: 12571
	private Action<PerkWidgetData> onOver;

	// Token: 0x0400311C RID: 12572
	private Action<PerkWidgetData> onOut;

	// Token: 0x0400311D RID: 12573
	private GamepadNavigationItem gamepadNavigationItem;
}

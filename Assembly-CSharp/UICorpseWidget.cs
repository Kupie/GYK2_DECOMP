using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020009DF RID: 2527
public class UICorpseWidget : LazyWidget<UICorpseWidgetData>
{
	// Token: 0x06004399 RID: 17305 RVA: 0x00141B04 File Offset: 0x0013FD04
	public override void Init()
	{
		base.Init();
		this.exhumeButton.onClick.AddListener(new UnityAction(this.OnButtonPressed));
		this.exhumeButton.onNotInteractableEnter.AddListener(new UnityAction(this.OnNonInteractableOver));
		this.exhumeButton.onNotInteractableExit.AddListener(new UnityAction(this.OnOut));
		UIMouseTooltip.Attach(this.redSkullsValue.transform.parent.gameObject, "tt_grave_2", null, false, false, new UIMouseTooltipEdges(0f, 0f, -20f, -20f), new Vector2(0f, -62f), null);
	}

	// Token: 0x0600439A RID: 17306 RVA: 0x00141BB7 File Offset: 0x0013FDB7
	private void OnEnable()
	{
		LazyInput.OnInputChanged += this.OnInputChanged;
		LazyInput.OnActiveGamepadChangedEvent += this.OnInputChanged;
	}

	// Token: 0x0600439B RID: 17307 RVA: 0x00141BDB File Offset: 0x0013FDDB
	private void OnDisable()
	{
		LazyInput.OnInputChanged -= this.OnInputChanged;
		LazyInput.OnActiveGamepadChangedEvent += this.OnInputChanged;
	}

	// Token: 0x0600439C RID: 17308 RVA: 0x00141C00 File Offset: 0x0013FE00
	public override void Redraw()
	{
		base.Redraw();
		this.redSkullsValue.text = ((this.data.CollarRedSkullsLimit >= 0) ? string.Format("{0}{1}/{2}", "rskull".FontIcon(), this.data.RedSkulls, this.data.CollarRedSkullsLimit) : string.Format("{0}{1}", "rskull".FontIcon(), this.data.RedSkulls));
		this.whiteSkullsValue.text = string.Format("{0}{1}", "skull".FontIcon(), this.data.WhiteSkulls);
		this.exhumeButton.interactable = this.data.ButtonInteractable && !this.data.IsEmpty;
		this.bodyImage.gameObject.SetActive(false);
		this.zombieImage.gameObject.SetActive(false);
		if (LazyInput.IsGamepadActive)
		{
			this.gameKeyTipLabel.text = new LazyGameKeyTip(this.data.GameKeyToExhume, this.data.ButtonText, this.exhumeButton.interactable, true, false).ToString();
		}
		this.headerLabel.text = this.data.HeaderText;
		this.descriptionLabel.text = this.data.DescriptionText;
		this.buttonLabel.text = this.data.ButtonText;
		if (this.data.IsEmpty)
		{
			this.inactiveStyle.ApplyStyle(this.headerLabel, false, null, null, null);
			this.redSkullsValue.gameObject.SetActive(false);
			this.whiteSkullsValue.gameObject.SetActive(false);
			this.noBodyObj.SetActive(true);
			this.collarCell.gameObject.SetActive(false);
			return;
		}
		this.activeStyle.ApplyStyle(this.headerLabel, false, null, null, null);
		this.redSkullsValue.gameObject.SetActive(true);
		this.whiteSkullsValue.gameObject.SetActive(true);
		this.noBodyObj.SetActive(false);
		if (this.data.IsZombie)
		{
			if (this.data.ZombieWgoData.Collar.IsEmpty)
			{
				this.collarCell.DrawEmpty();
				Debug.LogError(string.Format("No collar on the zombie:[{0}]", this.data.ZombieWgoData));
			}
			else
			{
				this.collarCell.Draw(this.data.ZombieWgoData.Collar, ItemRelatedWidgetState.NotSet, true);
			}
			this.collarCell.UIItemCell.ClearCallbacks();
			this.collarCell.gameObject.SetActive(true);
			this.zombieImage.gameObject.SetActive(true);
			SkinPresetGK2 presetForWgoData = ZombieSkinHelper.GetPresetForWgoData(this.data.ZombieWgoData, "zombie_worker");
			this.zombieImage.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(string.Format("i_zombie_{0}", presetForWgoData.head.id), "i_body");
			presetForWgoData.TryToApply(this.zombieImage, this.zombieImage.name, presetForWgoData.head);
			return;
		}
		this.bodyImage.gameObject.SetActive(true);
		this.bodyImage.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.data.IconId, "i_body");
		this.collarCell.gameObject.SetActive(false);
	}

	// Token: 0x0600439D RID: 17309 RVA: 0x00141FA3 File Offset: 0x001401A3
	public void OnButtonPressed()
	{
		Action onButtonPressed = this.data.OnButtonPressed;
		if (onButtonPressed == null)
		{
			return;
		}
		onButtonPressed();
	}

	// Token: 0x0600439E RID: 17310 RVA: 0x00141FBC File Offset: 0x001401BC
	private void OnInputChanged()
	{
		if (LazyInput.IsGamepadActive)
		{
			this.gameKeyTipLabel.text = new LazyGameKeyTip(this.data.GameKeyToExhume, this.data.ButtonText, this.exhumeButton.interactable, true, false).ToString();
		}
	}

	// Token: 0x0600439F RID: 17311 RVA: 0x00142008 File Offset: 0x00140208
	private void OnNonInteractableOver()
	{
		if (this.data.IsEmpty || this.data.ButtonInteractable)
		{
			return;
		}
		Action<LazyButton> onNonInteractableButtonOver = this.data.OnNonInteractableButtonOver;
		if (onNonInteractableButtonOver == null)
		{
			return;
		}
		onNonInteractableButtonOver(this.exhumeButton);
	}

	// Token: 0x060043A0 RID: 17312 RVA: 0x001080F0 File Offset: 0x001062F0
	private void OnOut()
	{
		UITooltip.Hide();
	}

	// Token: 0x060043A1 RID: 17313 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x040034C5 RID: 13509
	[SerializeField]
	private Image bodyImage;

	// Token: 0x040034C6 RID: 13510
	[SerializeField]
	private Image zombieImage;

	// Token: 0x040034C7 RID: 13511
	[SerializeField]
	private TextStyle activeStyle;

	// Token: 0x040034C8 RID: 13512
	[SerializeField]
	private TextStyle inactiveStyle;

	// Token: 0x040034C9 RID: 13513
	[SerializeField]
	private LazyButton exhumeButton;

	// Token: 0x040034CA RID: 13514
	[SerializeField]
	private TextMeshProUGUI headerLabel;

	// Token: 0x040034CB RID: 13515
	[SerializeField]
	private GameObject noBodyObj;

	// Token: 0x040034CC RID: 13516
	[SerializeField]
	private TextMeshProUGUI buttonLabel;

	// Token: 0x040034CD RID: 13517
	[SerializeField]
	private TextMeshProUGUI gameKeyTipLabel;

	// Token: 0x040034CE RID: 13518
	[SerializeField]
	private TextMeshProUGUI descriptionLabel;

	// Token: 0x040034CF RID: 13519
	[SerializeField]
	private TextMeshProUGUI redSkullsValue;

	// Token: 0x040034D0 RID: 13520
	[SerializeField]
	private TextMeshProUGUI whiteSkullsValue;

	// Token: 0x040034D1 RID: 13521
	[SerializeField]
	private UIFixedTypeItemCell collarCell;
}

using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200087A RID: 2170
public class UIEnergySanityBar : LazyWidget<UIEnergySanityBarData>
{
	// Token: 0x06003778 RID: 14200 RVA: 0x0010BA86 File Offset: 0x00109C86
	public override void Init()
	{
		this.currentEnergySlider.value = 1f;
		this.currentInsanitySlider.value = 0f;
		this.AttachHudIconTooltips();
	}

	// Token: 0x06003779 RID: 14201 RVA: 0x0010BAB0 File Offset: 0x00109CB0
	public void AttachHudIconTooltips()
	{
		if (this.energyIcon == null)
		{
			this.energyIcon = base.transform.Find("EnergyIcon");
		}
		if (this.insanityIcon == null)
		{
			this.insanityIcon = base.transform.Find("InsanityIcon");
		}
		if (this.energyIcon != null)
		{
			this.energyIcon.SetAsLastSibling();
			UIMouseTooltip.Attach(this.energyIcon.gameObject, "hud_energy", null, true, false, UIMouseTooltipEdges.All(2f), default(Vector2), "energy");
		}
		if (this.insanityIcon != null)
		{
			this.insanityIcon.SetAsLastSibling();
			UIMouseTooltip.Attach(this.insanityIcon.gameObject, "hud_insanity", null, true, false, UIMouseTooltipEdges.All(2f), default(Vector2), "insanity");
		}
	}

	// Token: 0x0600377A RID: 14202 RVA: 0x0010BB99 File Offset: 0x00109D99
	protected override void SetData(UIEnergySanityBarData data)
	{
		base.SetData(data);
		data.onFillValueChanged = new Action(this.UpdateEnergyBars);
	}

	// Token: 0x0600377B RID: 14203 RVA: 0x0010BBB4 File Offset: 0x00109DB4
	public override void Redraw()
	{
		base.Redraw();
		this.UpdateEnergyBars();
	}

	// Token: 0x0600377C RID: 14204 RVA: 0x0010BBC4 File Offset: 0x00109DC4
	private void UpdateEnergyBars()
	{
		this.currentEnergySlider.value = this.data.EnergyFillValue;
		this.energyFillImage.sprite = ((this.currentEnergySlider.value > this.lowBorderValue) ? this.defaultEnergyFillSprite : this.lowEnergyFillSprite);
		this.currentInsanitySlider.value = this.data.InsanityFillValue;
		this.insanityFillImage.sprite = ((this.currentInsanitySlider.value > this.lowBorderValue) ? this.defaultInsanityFillSprite : this.lowInsanityFillSprite);
		this.energyBlick.gameObject.SetActive(this.currentEnergySlider.value > this.lowBorderValue);
		this.insanityBlick.gameObject.SetActive(this.currentInsanitySlider.value > this.lowBorderValue);
		if (this.currentEnergySlider.value <= 0f)
		{
			this.currentEnergySlider.value = 0f;
		}
		else if (this.currentEnergySlider.value <= this.disableBorderValue)
		{
			this.currentEnergySlider.value = this.disableBorderValue;
		}
		if (this.currentInsanitySlider.value <= 0f)
		{
			this.currentInsanitySlider.value = 0f;
			return;
		}
		if (this.currentInsanitySlider.value <= this.disableBorderValue)
		{
			this.currentInsanitySlider.value = this.disableBorderValue;
		}
	}

	// Token: 0x0600377D RID: 14205 RVA: 0x0010BD2A File Offset: 0x00109F2A
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new UIEnergySanityBarData(MainGame.Instance.GameSave));
	}

	// Token: 0x04002C25 RID: 11301
	[SerializeField]
	private Slider currentEnergySlider;

	// Token: 0x04002C26 RID: 11302
	[SerializeField]
	private Slider currentInsanitySlider;

	// Token: 0x04002C27 RID: 11303
	[SerializeField]
	private Image energyFillImage;

	// Token: 0x04002C28 RID: 11304
	[SerializeField]
	private Image insanityFillImage;

	// Token: 0x04002C29 RID: 11305
	[SerializeField]
	private Sprite defaultEnergyFillSprite;

	// Token: 0x04002C2A RID: 11306
	[SerializeField]
	private Sprite lowEnergyFillSprite;

	// Token: 0x04002C2B RID: 11307
	[SerializeField]
	private Sprite defaultInsanityFillSprite;

	// Token: 0x04002C2C RID: 11308
	[SerializeField]
	private Sprite lowInsanityFillSprite;

	// Token: 0x04002C2D RID: 11309
	[SerializeField]
	private GameObject energyBlick;

	// Token: 0x04002C2E RID: 11310
	[SerializeField]
	private GameObject insanityBlick;

	// Token: 0x04002C2F RID: 11311
	[SerializeField]
	private float lowBorderValue;

	// Token: 0x04002C30 RID: 11312
	[SerializeField]
	private float disableBorderValue = 0.058f;

	// Token: 0x04002C31 RID: 11313
	[SerializeField]
	private Transform energyIcon;

	// Token: 0x04002C32 RID: 11314
	[SerializeField]
	private Transform insanityIcon;
}

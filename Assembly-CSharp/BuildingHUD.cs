using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000878 RID: 2168
public class BuildingHUD : LazyWidget<BuildingHUDData>
{
	// Token: 0x06003771 RID: 14193 RVA: 0x0010B8A0 File Offset: 0x00109AA0
	public override void Init()
	{
		base.Init();
		LazyInput.OnInputChanged += this.UpdateHints;
	}

	// Token: 0x06003772 RID: 14194 RVA: 0x0010B8B9 File Offset: 0x00109AB9
	public override void Redraw()
	{
		this.UpdateHints();
	}

	// Token: 0x06003773 RID: 14195 RVA: 0x0010B8C4 File Offset: 0x00109AC4
	private void UpdateHints()
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		if (this.data == null)
		{
			return;
		}
		if (this.data.isTargetCanBeRotated)
		{
			this.rotationHint.transform.parent.gameObject.SetActive(true);
			this.rotationHintGamepad.gameObject.SetActive(true);
		}
		else
		{
			this.rotationHint.transform.parent.gameObject.SetActive(false);
			this.rotationHintGamepad.gameObject.SetActive(false);
		}
		if (LazyInput.IsGamepadActive)
		{
			this.gamepadParent.SetActive(true);
			this.mouseParent.SetActive(false);
		}
		else
		{
			this.gamepadParent.SetActive(false);
			this.mouseParent.SetActive(true);
		}
		this.buildHintGamepad.text = ControllerIconLibrary.GetIconId(GameKey.Build, null, true) + LLBase.L("ui_build");
		this.rotationHintGamepad.text = ControllerIconLibrary.GetIconId(GameKey.Rotate, null, true) + LLBase.L("ui_rotate");
		this.exitHintGamepad.text = ControllerIconLibrary.GetIconId(GameKey.Back, null, true) + LLBase.L("ui_build_mode_exit");
		this.buildHint.text = LLBase.L("ui_build");
		this.rotationHint.text = LLBase.L("ui_rotate");
		this.exitHint.text = LLBase.L("ui_build_mode_exit");
		((RectTransform)base.transform).RefreshContentFitterAndDisable();
	}

	// Token: 0x06003774 RID: 14196 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04002C18 RID: 11288
	[SerializeField]
	private TextMeshProUGUI buildHint;

	// Token: 0x04002C19 RID: 11289
	[SerializeField]
	private TextMeshProUGUI rotationHint;

	// Token: 0x04002C1A RID: 11290
	[SerializeField]
	private TextMeshProUGUI exitHint;

	// Token: 0x04002C1B RID: 11291
	[SerializeField]
	private TextMeshProUGUI buildHintGamepad;

	// Token: 0x04002C1C RID: 11292
	[SerializeField]
	private TextMeshProUGUI rotationHintGamepad;

	// Token: 0x04002C1D RID: 11293
	[SerializeField]
	private TextMeshProUGUI exitHintGamepad;

	// Token: 0x04002C1E RID: 11294
	[SerializeField]
	private GameObject gamepadParent;

	// Token: 0x04002C1F RID: 11295
	[SerializeField]
	private GameObject mouseParent;
}

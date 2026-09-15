using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020009B7 RID: 2487
public class UIDialogInputWindow : LazyWindow<DialogInputWindowData>
{
	// Token: 0x06004240 RID: 16960 RVA: 0x0013AB9C File Offset: 0x00138D9C
	public override void Init()
	{
		base.Init();
		this.actionButton.onClick.AddListener(new UnityAction(this.OnActionButtonClicked));
		this.amountInputField.onValidateInput = delegate(string text, int index, char addedChar)
		{
			if (!"0123456789".Contains(addedChar))
			{
				return '\0';
			}
			return addedChar;
		};
	}

	// Token: 0x06004241 RID: 16961 RVA: 0x0013ABF5 File Offset: 0x00138DF5
	protected override void SetData(DialogInputWindowData data)
	{
		base.SetData(data);
		this.onActionButtonClicked = data.OnButtonPressed;
	}

	// Token: 0x06004242 RID: 16962 RVA: 0x0013AC0C File Offset: 0x00138E0C
	public override void Redraw()
	{
		base.Redraw();
		this.label.text = LLBase.L(this.data.HeaderText ?? "");
		this.actionButtonText.text = this.data.ButtonText;
	}

	// Token: 0x06004243 RID: 16963 RVA: 0x0013AC59 File Offset: 0x00138E59
	public override void Hide()
	{
		base.Hide();
		this.onActionButtonClicked = null;
	}

	// Token: 0x06004244 RID: 16964 RVA: 0x0013AC68 File Offset: 0x00138E68
	private void OnActionButtonClicked()
	{
		Action<string, float> action = this.onActionButtonClicked;
		if (action != null)
		{
			float num;
			action(this.idInputField.text, float.TryParse(this.amountInputField.text, out num) ? num : 0f);
		}
		this.Close();
	}

	// Token: 0x06004245 RID: 16965 RVA: 0x0013ACB3 File Offset: 0x00138EB3
	protected override void TestDraw()
	{
		this.Draw(new DialogInputWindowData("Test", "OK", null));
	}

	// Token: 0x040033AD RID: 13229
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x040033AE RID: 13230
	[SerializeField]
	private TMP_InputField idInputField;

	// Token: 0x040033AF RID: 13231
	[SerializeField]
	private TMP_InputField amountInputField;

	// Token: 0x040033B0 RID: 13232
	[SerializeField]
	private LazyButton actionButton;

	// Token: 0x040033B1 RID: 13233
	[SerializeField]
	private TextMeshProUGUI actionButtonText;

	// Token: 0x040033B2 RID: 13234
	private Action<string, float> onActionButtonClicked;
}

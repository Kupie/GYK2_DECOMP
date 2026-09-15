using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A41 RID: 2625
public class UITutorialListItemWidget : LazyWidget<UITutorialListItemWidgetData>
{
	// Token: 0x060046CB RID: 18123 RVA: 0x0014F114 File Offset: 0x0014D314
	private void Awake()
	{
		if (this.button != null)
		{
			this.button.onClick.RemoveAllListeners();
			this.button.onEnter.RemoveAllListeners();
			this.button.onExit.RemoveAllListeners();
			this.button.onClick.AddListener(new UnityAction(this.OnPressed));
			this.button.onEnter.AddListener(new UnityAction(this.OnOver));
			this.button.onExit.AddListener(new UnityAction(this.OnOut));
			this.button.SetCallbacksIntoGamepadNavigationItem();
		}
	}

	// Token: 0x060046CC RID: 18124 RVA: 0x0014F1C1 File Offset: 0x0014D3C1
	public override void Redraw()
	{
		base.Redraw();
		if (this.label != null)
		{
			this.label.text = LLBase.L(this.data.TutorialId);
		}
	}

	// Token: 0x060046CD RID: 18125 RVA: 0x0014F1F2 File Offset: 0x0014D3F2
	private void OnPressed()
	{
		Action<string> onPressed = this.data.OnPressed;
		if (onPressed == null)
		{
			return;
		}
		onPressed(this.data.TutorialId);
	}

	// Token: 0x060046CE RID: 18126 RVA: 0x0014F214 File Offset: 0x0014D414
	private void OnOver()
	{
		if (this.selectionFrame != null)
		{
			this.selectionFrame.gameObject.SetActive(true);
		}
	}

	// Token: 0x060046CF RID: 18127 RVA: 0x0014F235 File Offset: 0x0014D435
	private void OnOut()
	{
		if (this.selectionFrame != null)
		{
			this.selectionFrame.gameObject.SetActive(false);
		}
	}

	// Token: 0x060046D0 RID: 18128 RVA: 0x0014F235 File Offset: 0x0014D435
	private void OnDisable()
	{
		if (this.selectionFrame != null)
		{
			this.selectionFrame.gameObject.SetActive(false);
		}
	}

	// Token: 0x060046D1 RID: 18129 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003740 RID: 14144
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x04003741 RID: 14145
	[SerializeField]
	private Image selectionFrame;

	// Token: 0x04003742 RID: 14146
	[SerializeField]
	private LazyButton button;
}

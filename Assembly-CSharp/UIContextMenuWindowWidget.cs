using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000973 RID: 2419
public class UIContextMenuWindowWidget : LazyWidget<UIContextMenuWindowWidgetData>
{
	// Token: 0x06003FB9 RID: 16313 RVA: 0x00131150 File Offset: 0x0012F350
	private void Awake()
	{
		this.button.onClick.RemoveAllListeners();
		this.button.onClick.AddListener(new UnityAction(this.OnClick));
		this.button.onEnter.AddListener(new UnityAction(this.OnOver));
		this.button.onExit.AddListener(new UnityAction(this.OnOut));
	}

	// Token: 0x06003FBA RID: 16314 RVA: 0x001311C1 File Offset: 0x0012F3C1
	public override void Redraw()
	{
		base.Redraw();
		this.label.text = this.data.name;
		this.selector.SetActive(false);
		this.button.interactable = this.data.enabled;
	}

	// Token: 0x06003FBB RID: 16315 RVA: 0x00131201 File Offset: 0x0012F401
	private void OnClick()
	{
		Action callback = this.data.callback;
		if (callback == null)
		{
			return;
		}
		callback();
	}

	// Token: 0x06003FBC RID: 16316 RVA: 0x00131218 File Offset: 0x0012F418
	private void OnOver()
	{
		this.selector.SetActive(true);
	}

	// Token: 0x06003FBD RID: 16317 RVA: 0x00131226 File Offset: 0x0012F426
	private void OnOut()
	{
		this.selector.SetActive(false);
	}

	// Token: 0x06003FBE RID: 16318 RVA: 0x00131226 File Offset: 0x0012F426
	private void OnDisable()
	{
		this.selector.SetActive(false);
	}

	// Token: 0x06003FBF RID: 16319 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003228 RID: 12840
	[SerializeField]
	private LazyButton button;

	// Token: 0x04003229 RID: 12841
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x0400322A RID: 12842
	[SerializeField]
	private GameObject selector;
}

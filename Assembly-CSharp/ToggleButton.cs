using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000836 RID: 2102
public class ToggleButton : MonoBehaviour
{
	// Token: 0x17000807 RID: 2055
	// (get) Token: 0x060035C2 RID: 13762 RVA: 0x0010259C File Offset: 0x0010079C
	public LazyButton LazyButton
	{
		get
		{
			return this.lazyButton;
		}
	}

	// Token: 0x060035C3 RID: 13763 RVA: 0x001025A4 File Offset: 0x001007A4
	public void Init(Func<bool> getter, Action onClick)
	{
		this.getter = getter;
		this.lazyButton.onClick.RemoveAllListeners();
		this.lazyButton.onClick.AddListener(delegate
		{
			Action onClick2 = onClick;
			if (onClick2 != null)
			{
				onClick2();
			}
			this.Refresh();
		});
		this.Refresh();
		this.SetInteractable(true);
	}

	// Token: 0x060035C4 RID: 13764 RVA: 0x00102605 File Offset: 0x00100805
	public void SetInteractable(bool isInteractable)
	{
		this.lazyButton.interactable = isInteractable;
	}

	// Token: 0x060035C5 RID: 13765 RVA: 0x00102613 File Offset: 0x00100813
	private void Update()
	{
		this.Refresh();
	}

	// Token: 0x060035C6 RID: 13766 RVA: 0x0010261B File Offset: 0x0010081B
	private void Refresh()
	{
		if (this.getter != null)
		{
			this.toggle.isOn = this.getter();
		}
	}

	// Token: 0x04002B0E RID: 11022
	[SerializeField]
	private LazyButton lazyButton;

	// Token: 0x04002B0F RID: 11023
	[SerializeField]
	private Toggle toggle;

	// Token: 0x04002B10 RID: 11024
	private Func<bool> getter;
}

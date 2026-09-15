using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000957 RID: 2391
public abstract class TechTreeElementBaseWidget : LazyWidget<TechTreeElementBaseWidgetData>
{
	// Token: 0x06003F0F RID: 16143 RVA: 0x0012DCCF File Offset: 0x0012BECF
	public override void Init()
	{
		base.Init();
		this.OnDeselect();
	}

	// Token: 0x06003F10 RID: 16144 RVA: 0x0012DCE0 File Offset: 0x0012BEE0
	public override void Redraw()
	{
		this.button.onExit.RemoveAllListeners();
		this.button.onExit.AddListener(new UnityAction(this.OnDeselect));
		this.button.onEnter.RemoveAllListeners();
		this.button.onEnter.AddListener(new UnityAction(this.OnSelect));
		this.button.onClick.RemoveAllListeners();
		this.button.onClick.AddListener(new UnityAction(this.OnClicked));
		base.Redraw();
	}

	// Token: 0x06003F11 RID: 16145 RVA: 0x0012DD77 File Offset: 0x0012BF77
	protected void OnClicked()
	{
		Action<TechTreeElementBaseWidgetData> onTechClicked = this.data.onTechClicked;
		if (onTechClicked != null)
		{
			onTechClicked(this.data);
		}
		this.OnDeselect();
		this.Redraw();
	}

	// Token: 0x06003F12 RID: 16146 RVA: 0x0012DDA1 File Offset: 0x0012BFA1
	private void OnDisable()
	{
		this.OnDeselect();
	}

	// Token: 0x06003F13 RID: 16147 RVA: 0x0012DDA9 File Offset: 0x0012BFA9
	private void OnSelect()
	{
		this.selection.gameObject.SetActive(true);
	}

	// Token: 0x06003F14 RID: 16148 RVA: 0x0012DDBC File Offset: 0x0012BFBC
	private void OnDeselect()
	{
		this.selection.gameObject.SetActive(false);
	}

	// Token: 0x0400319C RID: 12700
	[SerializeField]
	protected Image selection;

	// Token: 0x0400319D RID: 12701
	[SerializeField]
	protected GameObject hiddenObj;

	// Token: 0x0400319E RID: 12702
	[SerializeField]
	protected GameObject visibleObj;

	// Token: 0x0400319F RID: 12703
	[SerializeField]
	protected GameObject availableObj;

	// Token: 0x040031A0 RID: 12704
	[SerializeField]
	protected GameObject unlockedObj;

	// Token: 0x040031A1 RID: 12705
	public LazyButton button;
}

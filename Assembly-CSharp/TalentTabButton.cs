using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200092D RID: 2349
public class TalentTabButton : MonoBehaviour
{
	// Token: 0x17000957 RID: 2391
	// (get) Token: 0x06003DEB RID: 15851 RVA: 0x00127FDA File Offset: 0x001261DA
	public string TalentId
	{
		get
		{
			return this.talentId;
		}
	}

	// Token: 0x06003DEC RID: 15852 RVA: 0x00127FE4 File Offset: 0x001261E4
	public void Init(string talentId, Action onPressAction)
	{
		this.talentId = talentId;
		this.onPressAction = onPressAction;
		this.button.onDown.AddListener(new UnityAction(this.HandlePress));
		this.button.onExit.RemoveAllListeners();
		this.button.onExit.AddListener(new UnityAction(this.OnDeselect));
		this.button.onEnter.RemoveAllListeners();
		this.button.onEnter.AddListener(new UnityAction(this.OnSelect));
	}

	// Token: 0x06003DED RID: 15853 RVA: 0x00128078 File Offset: 0x00126278
	public virtual void UpdateState(bool isActive, Canvas parentCanvas)
	{
		this.isActive = isActive;
		if (isActive)
		{
			this.backgroundImage.sprite = this.activeBackSprite;
		}
		else
		{
			this.backgroundImage.sprite = this.inactiveBackSprite;
		}
		this.DrawTalentIcon();
		this.button.interactable = !isActive;
		this.UpdateSorting(parentCanvas);
	}

	// Token: 0x06003DEE RID: 15854 RVA: 0x001280CF File Offset: 0x001262CF
	protected virtual void DrawTalentIcon()
	{
		this.icon.Draw(this.talentId, this.isActive);
	}

	// Token: 0x06003DEF RID: 15855 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void SetActionIndicatorState(bool isActive)
	{
	}

	// Token: 0x06003DF0 RID: 15856 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void DrawMasteryValue()
	{
	}

	// Token: 0x06003DF1 RID: 15857 RVA: 0x001280E8 File Offset: 0x001262E8
	public void UpdateSorting(Canvas parentCanvas)
	{
		this.canvas.sortingOrder = parentCanvas.sortingOrder + (this.isActive ? 1 : (-1));
	}

	// Token: 0x06003DF2 RID: 15858 RVA: 0x00128108 File Offset: 0x00126308
	private void HandlePress()
	{
		LazyAudio.PlayAndForget("tab_click");
		Action action = this.onPressAction;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06003DF3 RID: 15859 RVA: 0x00128124 File Offset: 0x00126324
	protected virtual void OnDisable()
	{
		if (this.button.interactable)
		{
			this.OnDeselect();
		}
	}

	// Token: 0x06003DF4 RID: 15860 RVA: 0x00128139 File Offset: 0x00126339
	protected virtual void OnSelect()
	{
		this.backgroundImage.sprite = this.inactiveBackSpriteSelected;
	}

	// Token: 0x06003DF5 RID: 15861 RVA: 0x0012814C File Offset: 0x0012634C
	protected virtual void OnDeselect()
	{
		this.backgroundImage.sprite = this.inactiveBackSprite;
	}

	// Token: 0x040030C1 RID: 12481
	[SerializeField]
	protected LazyButton button;

	// Token: 0x040030C2 RID: 12482
	[SerializeField]
	protected TalentIcon icon;

	// Token: 0x040030C3 RID: 12483
	[SerializeField]
	protected Canvas canvas;

	// Token: 0x040030C4 RID: 12484
	[SerializeField]
	protected Image backgroundImage;

	// Token: 0x040030C5 RID: 12485
	[SerializeField]
	protected Sprite activeBackSprite;

	// Token: 0x040030C6 RID: 12486
	[SerializeField]
	protected Sprite inactiveBackSprite;

	// Token: 0x040030C7 RID: 12487
	[SerializeField]
	protected Sprite inactiveBackSpriteSelected;

	// Token: 0x040030C8 RID: 12488
	protected string talentId;

	// Token: 0x040030C9 RID: 12489
	protected Action onPressAction;

	// Token: 0x040030CA RID: 12490
	protected bool isActive;
}

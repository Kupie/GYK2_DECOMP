using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000900 RID: 2304
public class BuildingWindowTab : MonoBehaviour
{
	// Token: 0x17000911 RID: 2321
	// (get) Token: 0x06003C49 RID: 15433 RVA: 0x001202CA File Offset: 0x0011E4CA
	public string TabId
	{
		get
		{
			return this.tabId;
		}
	}

	// Token: 0x06003C4A RID: 15434 RVA: 0x001202D4 File Offset: 0x0011E4D4
	public void Init(string tabId, Action<BuildingWindowTab> onPressAction)
	{
		this.tabId = tabId;
		this.onPressAction = onPressAction;
		if (!this.isInitialized)
		{
			this.isInitialized = true;
			this.button.onDown.AddListener(new UnityAction(this.HandlePress));
			this.button.onExit.RemoveAllListeners();
			this.button.onExit.AddListener(new UnityAction(this.OnDeselect));
			this.button.onEnter.RemoveAllListeners();
			this.button.onEnter.AddListener(new UnityAction(this.OnSelect));
		}
	}

	// Token: 0x06003C4B RID: 15435 RVA: 0x00120374 File Offset: 0x0011E574
	public void UpdateState(bool isActive, Canvas parentCanvas)
	{
		this.isActive = isActive;
		if (isActive)
		{
			Image[] array = this.backgroundImages;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].sprite = this.activeBackSprite;
			}
		}
		else
		{
			Image[] array = this.backgroundImages;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].sprite = this.inactiveBackSprite;
			}
		}
		foreach (Image image in this.imageIcons)
		{
			image.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(isActive ? (this.tabId + "_active") : (this.tabId + "_inactive"), null);
			image.SetNativeSize();
		}
		this.button.interactable = !isActive;
		this.UpdateSorting(parentCanvas);
	}

	// Token: 0x06003C4C RID: 15436 RVA: 0x0012043C File Offset: 0x0011E63C
	public void UpdateSorting(Canvas parentCanvas)
	{
		this.canvas.sortingOrder = parentCanvas.sortingOrder + 5 + (this.isActive ? 1 : (-1));
	}

	// Token: 0x06003C4D RID: 15437 RVA: 0x0012045E File Offset: 0x0011E65E
	private void HandlePress()
	{
		Action<BuildingWindowTab> action = this.onPressAction;
		if (action == null)
		{
			return;
		}
		action(this);
	}

	// Token: 0x06003C4E RID: 15438 RVA: 0x00120471 File Offset: 0x0011E671
	private void OnDisable()
	{
		if (this.button.interactable)
		{
			this.OnDeselect();
		}
	}

	// Token: 0x06003C4F RID: 15439 RVA: 0x00120488 File Offset: 0x0011E688
	private void OnSelect()
	{
		Image[] array = this.backgroundImages;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].sprite = this.inactiveBackSpriteSelected;
		}
		foreach (Image image in this.imageIcons)
		{
			image.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.tabId + "_active", null);
			image.SetNativeSize();
		}
	}

	// Token: 0x06003C50 RID: 15440 RVA: 0x001204F8 File Offset: 0x0011E6F8
	private void OnDeselect()
	{
		Image[] array = this.backgroundImages;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].sprite = this.inactiveBackSprite;
		}
		foreach (Image image in this.imageIcons)
		{
			image.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.tabId + "_inactive", null);
			image.SetNativeSize();
		}
	}

	// Token: 0x04002F6C RID: 12140
	[SerializeField]
	private LazyButton button;

	// Token: 0x04002F6D RID: 12141
	[SerializeField]
	private Canvas canvas;

	// Token: 0x04002F6E RID: 12142
	[SerializeField]
	private GameObject activeObject;

	// Token: 0x04002F6F RID: 12143
	[SerializeField]
	private GameObject inactiveObject;

	// Token: 0x04002F70 RID: 12144
	[SerializeField]
	private Image[] imageIcons;

	// Token: 0x04002F71 RID: 12145
	[SerializeField]
	private Image[] backgroundImages;

	// Token: 0x04002F72 RID: 12146
	[SerializeField]
	private Sprite activeBackSprite;

	// Token: 0x04002F73 RID: 12147
	[SerializeField]
	private Sprite inactiveBackSprite;

	// Token: 0x04002F74 RID: 12148
	[SerializeField]
	private Sprite inactiveBackSpriteSelected;

	// Token: 0x04002F75 RID: 12149
	private Action<BuildingWindowTab> onPressAction;

	// Token: 0x04002F76 RID: 12150
	private bool isActive;

	// Token: 0x04002F77 RID: 12151
	private string tabId;

	// Token: 0x04002F78 RID: 12152
	private bool isInitialized;
}

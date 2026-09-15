using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020008C6 RID: 2246
public class UITabButton : MonoBehaviour
{
	// Token: 0x06003AB2 RID: 15026 RVA: 0x00118578 File Offset: 0x00116778
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnTabClicked));
	}

	// Token: 0x06003AB3 RID: 15027 RVA: 0x00118596 File Offset: 0x00116796
	public void Init(Action<string> onButtonClicked)
	{
		this.onButtonClicked = onButtonClicked;
	}

	// Token: 0x06003AB4 RID: 15028 RVA: 0x0011859F File Offset: 0x0011679F
	private void OnTabClicked()
	{
		if (!this.isSelected)
		{
			this.SetSelected(true);
			Action<string> action = this.onButtonClicked;
			if (action == null)
			{
				return;
			}
			action(this.id);
		}
	}

	// Token: 0x06003AB5 RID: 15029 RVA: 0x001185C6 File Offset: 0x001167C6
	public void SetSelected(bool isSelected)
	{
		this.isSelected = isSelected;
		this.buttonImage.sprite = (isSelected ? this.selectedSprite : this.unselectedSprite);
		this.canvas.overrideSorting = isSelected;
	}

	// Token: 0x04002E43 RID: 11843
	[SerializeField]
	private LazyButton button;

	// Token: 0x04002E44 RID: 11844
	[SerializeField]
	private Canvas canvas;

	// Token: 0x04002E45 RID: 11845
	[SerializeField]
	private Image buttonImage;

	// Token: 0x04002E46 RID: 11846
	[SerializeField]
	private Sprite unselectedSprite;

	// Token: 0x04002E47 RID: 11847
	[SerializeField]
	private Sprite selectedSprite;

	// Token: 0x04002E48 RID: 11848
	private Action<string> onButtonClicked;

	// Token: 0x04002E49 RID: 11849
	private bool isSelected;

	// Token: 0x04002E4A RID: 11850
	private string id;
}

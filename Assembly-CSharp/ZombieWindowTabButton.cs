using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A84 RID: 2692
public class ZombieWindowTabButton : MonoBehaviour
{
	// Token: 0x06004959 RID: 18777 RVA: 0x0015AA14 File Offset: 0x00158C14
	public void Init(Action onPressAction)
	{
		this.button.onClick.AddListener(delegate
		{
			LazyAudio.PlayAndForget("tab_click");
			Action onPressAction2 = onPressAction;
			if (onPressAction2 == null)
			{
				return;
			}
			onPressAction2();
		});
		this.button.onExit.RemoveAllListeners();
		this.button.onExit.AddListener(new UnityAction(this.OnDeselect));
		this.button.onEnter.RemoveAllListeners();
		this.button.onEnter.AddListener(new UnityAction(this.OnSelect));
	}

	// Token: 0x0600495A RID: 18778 RVA: 0x0015AAA2 File Offset: 0x00158CA2
	public void UpdateActionIndicatorStatus(bool value)
	{
		this.actionIndicator.SetActive(value);
	}

	// Token: 0x0600495B RID: 18779 RVA: 0x0015AAB0 File Offset: 0x00158CB0
	public void UpdateText(string text)
	{
		this.label.text = text;
	}

	// Token: 0x0600495C RID: 18780 RVA: 0x0015AAC0 File Offset: 0x00158CC0
	public void UpdateState(bool isActive, Canvas windowCanvas)
	{
		if (isActive)
		{
			this.activeStyle.ApplyStyle(this.label, false, null, null, null);
			this.backgroundImage.sprite = this.activeBackSprite;
		}
		else
		{
			this.inactiveStyle.ApplyStyle(this.label, false, null, null, null);
			this.backgroundImage.sprite = this.inactiveBackSprite;
		}
		this.button.interactable = !isActive;
		this.canvas.sortingOrder = windowCanvas.sortingOrder + (isActive ? 2 : 1);
		this.labelCanvas.sortingOrder = this.canvas.sortingOrder + 5;
	}

	// Token: 0x0600495D RID: 18781 RVA: 0x0015AB8E File Offset: 0x00158D8E
	private void OnDisable()
	{
		if (this.button.interactable)
		{
			this.OnDeselect();
		}
	}

	// Token: 0x0600495E RID: 18782 RVA: 0x0015ABA4 File Offset: 0x00158DA4
	private void OnSelect()
	{
		this.backgroundImage.sprite = this.inactiveBackSpriteSelected;
		this.activeStyle.ApplyStyle(this.label, false, null, null, null);
	}

	// Token: 0x0600495F RID: 18783 RVA: 0x0015ABF0 File Offset: 0x00158DF0
	private void OnDeselect()
	{
		this.backgroundImage.sprite = this.inactiveBackSprite;
		this.inactiveStyle.ApplyStyle(this.label, false, null, null, null);
	}

	// Token: 0x0400392B RID: 14635
	[SerializeField]
	private LazyButton button;

	// Token: 0x0400392C RID: 14636
	[SerializeField]
	private Canvas canvas;

	// Token: 0x0400392D RID: 14637
	[SerializeField]
	private Canvas labelCanvas;

	// Token: 0x0400392E RID: 14638
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x0400392F RID: 14639
	[SerializeField]
	private GameObject actionIndicator;

	// Token: 0x04003930 RID: 14640
	[SerializeField]
	private Image backgroundImage;

	// Token: 0x04003931 RID: 14641
	[SerializeField]
	private Sprite activeBackSprite;

	// Token: 0x04003932 RID: 14642
	[SerializeField]
	private Sprite inactiveBackSprite;

	// Token: 0x04003933 RID: 14643
	[SerializeField]
	private Sprite inactiveBackSpriteSelected;

	// Token: 0x04003934 RID: 14644
	[SerializeField]
	private TextStyle activeStyle;

	// Token: 0x04003935 RID: 14645
	[SerializeField]
	private TextStyle inactiveStyle;
}

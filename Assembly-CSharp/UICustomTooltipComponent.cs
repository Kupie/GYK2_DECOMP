using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000842 RID: 2114
public class UICustomTooltipComponent : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060035F2 RID: 13810 RVA: 0x00103121 File Offset: 0x00101321
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.Show();
		LazyAudio.PlayAndForget("gui_hover_light");
	}

	// Token: 0x060035F3 RID: 13811 RVA: 0x00103133 File Offset: 0x00101333
	public void OnPointerExit(PointerEventData eventData)
	{
		if (!eventData.fullyExited)
		{
			return;
		}
		this.Hide(false);
	}

	// Token: 0x060035F4 RID: 13812 RVA: 0x00103145 File Offset: 0x00101345
	public void SetCallbacks(Action<UICustomTooltipComponent> onImageTooltipOver = null, Action<UICustomTooltipComponent> onImageTooltipOut = null)
	{
		this.onImageTooltipOver = onImageTooltipOver;
		this.onImageTooltipOut = onImageTooltipOut;
	}

	// Token: 0x060035F5 RID: 13813 RVA: 0x00103158 File Offset: 0x00101358
	public void Show()
	{
		if (!string.IsNullOrEmpty(this.langToken))
		{
			UITooltip.ShowSimpleInfo(this.rectTransform, LLBase.L(this.langToken), default(Vector2), null);
			if (this.selectedImage != null)
			{
				this.selectedImage.gameObject.SetActive(true);
			}
			if (this.selectedSprite != null)
			{
				this.selfImage.sprite = this.selectedSprite;
			}
			Action<UICustomTooltipComponent> action = this.onImageTooltipOver;
			if (action == null)
			{
				return;
			}
			action(this);
		}
	}

	// Token: 0x060035F6 RID: 13814 RVA: 0x001031E4 File Offset: 0x001013E4
	public void Hide(bool immediately)
	{
		if (!string.IsNullOrEmpty(this.langToken))
		{
			if (UITooltip.IsTooltipShowingAtTarget(base.transform as RectTransform))
			{
				if (immediately)
				{
					UITooltip.HideImmediately();
				}
				else
				{
					UITooltip.Hide();
				}
			}
			if (this.selectedImage != null)
			{
				this.selectedImage.gameObject.SetActive(false);
			}
			if (this.notSelectedSprite != null)
			{
				this.selfImage.sprite = this.notSelectedSprite;
			}
			Action<UICustomTooltipComponent> action = this.onImageTooltipOut;
			if (action == null)
			{
				return;
			}
			action(this);
		}
	}

	// Token: 0x060035F7 RID: 13815 RVA: 0x0010326E File Offset: 0x0010146E
	private void OnDisable()
	{
		this.Hide(true);
	}

	// Token: 0x060035F8 RID: 13816 RVA: 0x00103277 File Offset: 0x00101477
	public void SetLocale(string locale)
	{
		this.langToken = locale;
	}

	// Token: 0x04002B37 RID: 11063
	private Action<UICustomTooltipComponent> onImageTooltipOver;

	// Token: 0x04002B38 RID: 11064
	private Action<UICustomTooltipComponent> onImageTooltipOut;

	// Token: 0x04002B39 RID: 11065
	[SerializeField]
	private string langToken;

	// Token: 0x04002B3A RID: 11066
	[SerializeField]
	private RectTransform rectTransform;

	// Token: 0x04002B3B RID: 11067
	[SerializeField]
	private Image selectedImage;

	// Token: 0x04002B3C RID: 11068
	[SerializeField]
	private Image selfImage;

	// Token: 0x04002B3D RID: 11069
	[SerializeField]
	private Sprite selectedSprite;

	// Token: 0x04002B3E RID: 11070
	[SerializeField]
	private Sprite notSelectedSprite;
}

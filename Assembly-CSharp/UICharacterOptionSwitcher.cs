using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200099F RID: 2463
public class UICharacterOptionSwitcher : MonoBehaviour
{
	// Token: 0x17000A05 RID: 2565
	// (get) Token: 0x060041D0 RID: 16848 RVA: 0x001397FE File Offset: 0x001379FE
	// (set) Token: 0x060041D1 RID: 16849 RVA: 0x0013980C File Offset: 0x00137A0C
	public bool IsInteractable
	{
		get
		{
			return this.uiSwitchButton.IsInteractable;
		}
		set
		{
			this.uiSwitchButton.IsInteractable = value;
			this.currentOptionLabel.gameObject.SetActive(value);
			this.notInteractableObj.SetActive(!value);
			if (!value)
			{
				this.pickedColorImage.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060041D2 RID: 16850 RVA: 0x00139859 File Offset: 0x00137A59
	public void Initialize(Action<int> onChangedCallback, string[] fields, int currentFieldIndex = 0, bool loopNavigation = true)
	{
		this.uiSwitchButton.Initialize(onChangedCallback, fields, currentFieldIndex, "", UICharacterOptionSwitcher.decreaseKeys, UICharacterOptionSwitcher.increaseKeys, loopNavigation);
	}

	// Token: 0x060041D3 RID: 16851 RVA: 0x0013987A File Offset: 0x00137A7A
	public void OnGamepadFocus()
	{
		this.switchLabelTextStyleComponent.SetTextStyle(this.switchLabelStyleSelected);
	}

	// Token: 0x060041D4 RID: 16852 RVA: 0x0013988D File Offset: 0x00137A8D
	public void OnGamepadUnFocus()
	{
		this.switchLabelTextStyleComponent.SetTextStyle(this.switchLabelStyle);
	}

	// Token: 0x060041D5 RID: 16853 RVA: 0x001398A0 File Offset: 0x00137AA0
	public void UpdateField(int index)
	{
		this.uiSwitchButton.UpdateField(index, true);
	}

	// Token: 0x060041D6 RID: 16854 RVA: 0x001398AF File Offset: 0x00137AAF
	public int GetCurrentIndex()
	{
		return this.uiSwitchButton.CurrentFieldIndex;
	}

	// Token: 0x060041D7 RID: 16855 RVA: 0x001398BC File Offset: 0x00137ABC
	public void SetSprite(int index)
	{
		this.pickedColorImage.gameObject.SetActive(this.IsInteractable);
		this.pickedColorImage.color = Color.white;
		this.pickedColorImage.sprite = this.spriteFields[index];
	}

	// Token: 0x060041D8 RID: 16856 RVA: 0x001398FB File Offset: 0x00137AFB
	public void SetSprite(Sprite sprite)
	{
		this.pickedColorImage.sprite = sprite;
		this.pickedColorImage.gameObject.SetActive(this.IsInteractable);
		this.pickedColorImage.color = Color.white;
	}

	// Token: 0x060041D9 RID: 16857 RVA: 0x00139930 File Offset: 0x00137B30
	public void SetColorImage(Texture2D texture2D)
	{
		this.pickedColorImage.sprite = null;
		bool flag = texture2D != null;
		if (flag)
		{
			Color32[] pixels = texture2D.GetPixels32();
			int num = pixels.Length;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			for (int i = 0; i < num; i++)
			{
				num2 += (float)pixels[i].r;
				num3 += (float)pixels[i].g;
				num4 += (float)pixels[i].b;
			}
			this.pickedColorImage.color = new Color32((byte)(num2 / (float)num), (byte)(num3 / (float)num), (byte)(num4 / (float)num), byte.MaxValue);
		}
		this.pickedColorImage.gameObject.SetActive(flag && this.IsInteractable);
	}

	// Token: 0x04003368 RID: 13160
	private static readonly GameKey[] decreaseKeys = new GameKey[] { GameKey.PrevSubTab };

	// Token: 0x04003369 RID: 13161
	private static readonly GameKey[] increaseKeys = new GameKey[] { GameKey.NextSubTab };

	// Token: 0x0400336A RID: 13162
	[SerializeField]
	private TextStyle switchLabelStyle;

	// Token: 0x0400336B RID: 13163
	[SerializeField]
	private TextStyle switchLabelStyleSelected;

	// Token: 0x0400336C RID: 13164
	[SerializeField]
	private TextStyleComponent switchLabelTextStyleComponent;

	// Token: 0x0400336D RID: 13165
	[SerializeField]
	private TextMeshProUGUI currentOptionLabel;

	// Token: 0x0400336E RID: 13166
	[SerializeField]
	private Image pickedColorImage;

	// Token: 0x0400336F RID: 13167
	[SerializeField]
	private List<Sprite> spriteFields;

	// Token: 0x04003370 RID: 13168
	[SerializeField]
	private GameObject notInteractableObj;

	// Token: 0x04003371 RID: 13169
	[SerializeField]
	[Space]
	private UISwitchButton uiSwitchButton;
}

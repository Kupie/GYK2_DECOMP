using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009AF RID: 2479
public class UIAnimSwitchButton : MonoBehaviour
{
	// Token: 0x0600421B RID: 16923 RVA: 0x0013A6FC File Offset: 0x001388FC
	private void Start()
	{
		if (this.toggle != null)
		{
			this.toggle.isOn = false;
			this.prevButton.interactable = false;
			this.nextButton.interactable = false;
			this.labelBg.color = this.prevButton.colors.disabledColor;
		}
	}

	// Token: 0x0600421C RID: 16924 RVA: 0x0013A75C File Offset: 0x0013895C
	private void UpdateBtnSelection(bool isNext)
	{
		if (isNext)
		{
			this.currentIdIndex++;
			if (this.currentIdIndex >= this.idList.Count)
			{
				this.currentIdIndex = 0;
			}
		}
		else
		{
			this.currentIdIndex--;
			if (this.currentIdIndex < 0)
			{
				this.currentIdIndex = this.idList.Count - 1;
			}
		}
		this.label.text = this.idList[this.currentIdIndex];
		this.FireCallback();
	}

	// Token: 0x0600421D RID: 16925 RVA: 0x0013A7E2 File Offset: 0x001389E2
	private void InitButtons()
	{
		this.prevButton.onClick.AddListener(delegate
		{
			this.UpdateBtnSelection(false);
		});
		this.nextButton.onClick.AddListener(delegate
		{
			this.UpdateBtnSelection(true);
		});
	}

	// Token: 0x0600421E RID: 16926 RVA: 0x0013A81C File Offset: 0x00138A1C
	private void FireCallback()
	{
		bool flag = this.toggle == null || this.toggle.isOn;
		this.currentIdIndex = Mathf.Clamp(this.currentIdIndex, 0, this.idList.Count - 1);
		Action<string, bool> action = this.clickCallback;
		if (action == null)
		{
			return;
		}
		action(this.idList[this.currentIdIndex], flag);
	}

	// Token: 0x0600421F RID: 16927 RVA: 0x0013A888 File Offset: 0x00138A88
	public void Init(List<string> idList, Action<string, bool> clickCallback)
	{
		this.idList = idList;
		this.clickCallback = clickCallback;
		this.label.text = idList[0];
		this.InitButtons();
		this.FireCallback();
		if (this.toggle != null)
		{
			this.toggle.onValueChanged.AddListener(delegate(bool toggle)
			{
				this.FireCallback();
				this.labelBg.color = (toggle ? Color.white : this.nextButton.colors.disabledColor);
				this.nextButton.interactable = toggle;
				this.prevButton.interactable = toggle;
			});
		}
	}

	// Token: 0x06004220 RID: 16928 RVA: 0x0013A8EC File Offset: 0x00138AEC
	public void OverrideValue(string id)
	{
		this.currentIdIndex = this.idList.IndexOf(id);
		if (this.currentIdIndex == -1)
		{
			this.currentIdIndex = 0;
		}
		this.label.text = this.idList[this.currentIdIndex];
		this.FireCallback();
	}

	// Token: 0x06004221 RID: 16929 RVA: 0x0013A93D File Offset: 0x00138B3D
	public void Clear()
	{
		this.prevButton.onClick.RemoveAllListeners();
		this.nextButton.onClick.RemoveAllListeners();
	}

	// Token: 0x0400339D RID: 13213
	[SerializeField]
	private Button prevButton;

	// Token: 0x0400339E RID: 13214
	[SerializeField]
	private Button nextButton;

	// Token: 0x0400339F RID: 13215
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x040033A0 RID: 13216
	[SerializeField]
	private Toggle toggle;

	// Token: 0x040033A1 RID: 13217
	[SerializeField]
	private Image labelBg;

	// Token: 0x040033A2 RID: 13218
	private List<string> idList = new List<string>();

	// Token: 0x040033A3 RID: 13219
	private int currentIdIndex;

	// Token: 0x040033A4 RID: 13220
	private Action<string, bool> clickCallback;
}

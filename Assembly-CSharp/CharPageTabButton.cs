using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000968 RID: 2408
public class CharPageTabButton : MonoBehaviour
{
	// Token: 0x06003F6C RID: 16236 RVA: 0x0012FF70 File Offset: 0x0012E170
	public void Init(string name, Action onPressAction, bool isLastTab)
	{
		this.tabName = name;
		this.button.onClick.AddListener(delegate
		{
			Action onPressAction2 = onPressAction;
			if (onPressAction2 == null)
			{
				return;
			}
			onPressAction2();
		});
		this.SetLabel();
		this.rightSeparator.SetActive(!isLastTab);
		this.button.onEnter.AddListener(new UnityAction(this.OnEnter));
		this.button.onExit.AddListener(new UnityAction(this.OnExit));
	}

	// Token: 0x06003F6D RID: 16237 RVA: 0x0012FFFA File Offset: 0x0012E1FA
	public void UpdateActionIndicatorStatus(bool value)
	{
		this.actionIndicator.SetActive(value);
	}

	// Token: 0x06003F6E RID: 16238 RVA: 0x00130008 File Offset: 0x0012E208
	public void UpdateState(bool isActive)
	{
		this.activeGameObject.SetActive(isActive);
		if (isActive)
		{
			this.selection.gameObject.SetActive(false);
			this.selectedLabelStyle.ApplyStyle(this.label, false, null, null, null);
		}
		else
		{
			this.normalLabelStyle.ApplyStyle(this.label, false, null, null, null);
		}
		this.button.interactable = !isActive;
	}

	// Token: 0x06003F6F RID: 16239 RVA: 0x001300A0 File Offset: 0x0012E2A0
	private void OnEnable()
	{
		this.SetLabel();
	}

	// Token: 0x06003F70 RID: 16240 RVA: 0x001300A8 File Offset: 0x0012E2A8
	private void OnDisable()
	{
		if (this.button.interactable)
		{
			this.OnExit();
		}
	}

	// Token: 0x06003F71 RID: 16241 RVA: 0x001300C0 File Offset: 0x0012E2C0
	private void OnEnter()
	{
		this.selection.gameObject.SetActive(true);
		this.selectedLabelStyle.ApplyStyle(this.label, false, null, null, null);
	}

	// Token: 0x06003F72 RID: 16242 RVA: 0x0013010C File Offset: 0x0012E30C
	private void OnExit()
	{
		this.selection.gameObject.SetActive(false);
		this.normalLabelStyle.ApplyStyle(this.label, false, null, null, null);
	}

	// Token: 0x06003F73 RID: 16243 RVA: 0x00130157 File Offset: 0x0012E357
	public void SetLabel()
	{
		if (string.IsNullOrEmpty(this.tabName))
		{
			return;
		}
		this.label.text = LLBase.L(this.tabName);
	}

	// Token: 0x040031FB RID: 12795
	[SerializeField]
	private LazyButton button;

	// Token: 0x040031FC RID: 12796
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x040031FD RID: 12797
	[SerializeField]
	private TextStyle normalLabelStyle;

	// Token: 0x040031FE RID: 12798
	[SerializeField]
	private TextStyle selectedLabelStyle;

	// Token: 0x040031FF RID: 12799
	[SerializeField]
	private GameObject actionIndicator;

	// Token: 0x04003200 RID: 12800
	[SerializeField]
	private GameObject activeGameObject;

	// Token: 0x04003201 RID: 12801
	[SerializeField]
	private GameObject rightSeparator;

	// Token: 0x04003202 RID: 12802
	[SerializeField]
	private GameObject selection;

	// Token: 0x04003203 RID: 12803
	private string tabName;
}

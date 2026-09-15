using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020008EA RID: 2282
public class UIFolioWindowTab : MonoBehaviour
{
	// Token: 0x170008FE RID: 2302
	// (get) Token: 0x06003BB5 RID: 15285 RVA: 0x0011D3D2 File Offset: 0x0011B5D2
	public AlchemyFormulaTab Tab
	{
		get
		{
			return this.tab;
		}
	}

	// Token: 0x06003BB6 RID: 15286 RVA: 0x0011D3DC File Offset: 0x0011B5DC
	public void Init(Action<UIFolioWindowTab> onPressAction)
	{
		this.button.onExit.RemoveAllListeners();
		this.button.onEnter.RemoveAllListeners();
		this.button.onClick.RemoveAllListeners();
		this.button.onClick.AddListener(delegate
		{
			Action<UIFolioWindowTab> onPressAction2 = onPressAction;
			if (onPressAction2 == null)
			{
				return;
			}
			onPressAction2(this);
		});
		if (this.highlightedObj != null)
		{
			this.button.onEnter.AddListener(new UnityAction(this.OnEnter));
			this.button.onExit.AddListener(new UnityAction(this.OnExit));
			this.highlightedObj.SetActive(false);
		}
		this.isHighlighted = false;
	}

	// Token: 0x06003BB7 RID: 15287 RVA: 0x0011D4A2 File Offset: 0x0011B6A2
	public void SetState(bool active)
	{
		this.isActive = active;
		this.isHighlighted = false;
		this.RefreshVisuals();
		this.button.interactable = !active;
	}

	// Token: 0x06003BB8 RID: 15288 RVA: 0x0011D4C7 File Offset: 0x0011B6C7
	private void OnDisable()
	{
		if (this.highlightedObj == null)
		{
			return;
		}
		this.isHighlighted = false;
		this.RefreshVisuals();
	}

	// Token: 0x06003BB9 RID: 15289 RVA: 0x0011D4E5 File Offset: 0x0011B6E5
	private void OnEnter()
	{
		if (this.isActive || this.highlightedObj == null)
		{
			return;
		}
		this.isHighlighted = true;
		this.RefreshVisuals();
	}

	// Token: 0x06003BBA RID: 15290 RVA: 0x0011D4C7 File Offset: 0x0011B6C7
	private void OnExit()
	{
		if (this.highlightedObj == null)
		{
			return;
		}
		this.isHighlighted = false;
		this.RefreshVisuals();
	}

	// Token: 0x06003BBB RID: 15291 RVA: 0x0011D50C File Offset: 0x0011B70C
	private void RefreshVisuals()
	{
		bool flag = !this.isActive && this.isHighlighted && this.highlightedObj != null;
		if (this.highlightedObj != null)
		{
			this.highlightedObj.SetActive(flag);
		}
		this.activeObj.SetActive(this.isActive);
		this.inactiveObj.SetActive(!this.isActive && !flag);
	}

	// Token: 0x04002F0B RID: 12043
	[SerializeField]
	private LazyButton button;

	// Token: 0x04002F0C RID: 12044
	[SerializeField]
	private GameObject activeObj;

	// Token: 0x04002F0D RID: 12045
	[SerializeField]
	private GameObject inactiveObj;

	// Token: 0x04002F0E RID: 12046
	[SerializeField]
	private GameObject highlightedObj;

	// Token: 0x04002F0F RID: 12047
	[SerializeField]
	private AlchemyFormulaTab tab;

	// Token: 0x04002F10 RID: 12048
	private bool isActive;

	// Token: 0x04002F11 RID: 12049
	private bool isHighlighted;
}

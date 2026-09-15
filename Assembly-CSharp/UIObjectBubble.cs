using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200082B RID: 2091
public class UIObjectBubble : UIWidgetContainer
{
	// Token: 0x170007FE RID: 2046
	// (get) Token: 0x06003577 RID: 13687 RVA: 0x00101138 File Offset: 0x000FF338
	public IBubbleDrawable Target
	{
		get
		{
			return this.target;
		}
	}

	// Token: 0x170007FF RID: 2047
	// (get) Token: 0x06003578 RID: 13688 RVA: 0x00101140 File Offset: 0x000FF340
	public bool IsOutOfScreen
	{
		get
		{
			return this.isOutOfScreen;
		}
	}

	// Token: 0x17000800 RID: 2048
	// (get) Token: 0x06003579 RID: 13689 RVA: 0x00101148 File Offset: 0x000FF348
	// (set) Token: 0x0600357A RID: 13690 RVA: 0x00101150 File Offset: 0x000FF350
	public long DepthKey { get; private set; }

	// Token: 0x0600357B RID: 13691 RVA: 0x00101159 File Offset: 0x000FF359
	internal void SetDepthKey(long depthKey)
	{
		this.DepthKey = depthKey;
	}

	// Token: 0x0600357C RID: 13692 RVA: 0x00101164 File Offset: 0x000FF364
	public void TryCompareDataAndRedrawExistingOrDisplay(IBubbleDrawable target, List<LazyWidgetDataBase> dataList)
	{
		if (dataList.Count != this.dataList.Count)
		{
			this.Display(target, dataList);
			return;
		}
		for (int i = 0; i < dataList.Count; i++)
		{
			if (dataList[i].GetType() != this.dataList[i].GetType())
			{
				this.Display(target, dataList);
				return;
			}
		}
		this.RedrawExistingWidgetsOnly(target, dataList);
	}

	// Token: 0x0600357D RID: 13693 RVA: 0x001011D4 File Offset: 0x000FF3D4
	public void Display(IBubbleDrawable target, List<LazyWidgetDataBase> dataList)
	{
		this.target = target;
		if (this.HasDelayedHide())
		{
			this.ConstructWidgetsWithoutTouchingDelayed(dataList);
		}
		else
		{
			base.ConstructWidgets(dataList, null);
		}
		if (this.rectTransform.GetComponentInChildren<IBubbleLayoutAlwaysActive>() != null)
		{
			this.rectTransform.EnableLayoutGroupsAndRefreshContentFitter();
		}
		else
		{
			this.rectTransform.RefreshContentFitterAndDisable();
		}
		this.UpdatePos();
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600357E RID: 13694 RVA: 0x00101238 File Offset: 0x000FF438
	public void RedrawExistingWidgetsOnly(IBubbleDrawable target, List<LazyWidgetDataBase> dataList)
	{
		bool flag = false;
		List<LazyWidgetBase> list = new List<LazyWidgetBase>();
		for (int i = 0; i < this.displayedWidgets.Count; i++)
		{
			LazyWidgetBase lazyWidgetBase = this.displayedWidgets[i];
			IDelayedUIHide delayedUIHide = lazyWidgetBase as IDelayedUIHide;
			if (delayedUIHide == null || !delayedUIHide.ShouldDelayHide)
			{
				lazyWidgetBase.Draw(dataList[i]);
				if (!(lazyWidgetBase is IUIObjectBubbleWidgetWithoutRebuildingLayout))
				{
					flag = true;
				}
				if (lazyWidgetBase is IBubbleLayoutAlwaysActive)
				{
					list.Add(lazyWidgetBase);
				}
			}
		}
		if (flag)
		{
			this.rectTransform.RefreshContentFitterAndDisable();
			this.UpdatePos();
		}
		foreach (LazyWidgetBase lazyWidgetBase2 in list)
		{
			((RectTransform)lazyWidgetBase2.transform).EnableLayoutGroupsAndRefreshContentFitter();
		}
	}

	// Token: 0x0600357F RID: 13695 RVA: 0x00101308 File Offset: 0x000FF508
	private void ConstructWidgetsWithoutTouchingDelayed(List<LazyWidgetDataBase> dataList)
	{
		foreach (LazyWidgetBase lazyWidgetBase in this.usedWidgetsList)
		{
			IDelayedUIHide delayedUIHide = lazyWidgetBase as IDelayedUIHide;
			if (delayedUIHide == null || !delayedUIHide.ShouldDelayHide)
			{
				lazyWidgetBase.Hide();
				lazyWidgetBase.gameObject.SetActive(false);
			}
		}
		this.displayedWidgets.RemoveAll(delegate(LazyWidgetBase widget)
		{
			IDelayedUIHide delayedUIHide3 = widget as IDelayedUIHide;
			return delayedUIHide3 == null || !delayedUIHide3.ShouldDelayHide;
		});
		int num = 0;
		foreach (LazyWidgetDataBase lazyWidgetDataBase in dataList)
		{
			bool flag = false;
			foreach (LazyWidgetBase lazyWidgetBase2 in this.usedWidgetsList)
			{
				IDelayedUIHide delayedUIHide2 = lazyWidgetBase2 as IDelayedUIHide;
				if (delayedUIHide2 != null && delayedUIHide2.ShouldDelayHide)
				{
					if (lazyWidgetBase2.GetDataType() == lazyWidgetDataBase.GetType())
					{
						lazyWidgetBase2.transform.SetSiblingIndex(num);
						num++;
						flag = true;
						break;
					}
				}
				else if (!lazyWidgetBase2.gameObject.activeSelf && lazyWidgetBase2.GetDataType() == lazyWidgetDataBase.GetType())
				{
					lazyWidgetBase2.Draw(lazyWidgetDataBase);
					lazyWidgetBase2.gameObject.SetActive(true);
					lazyWidgetBase2.transform.SetSiblingIndex(num);
					num++;
					flag = true;
					this.displayedWidgets.Add(lazyWidgetBase2);
					break;
				}
			}
			if (!flag)
			{
				LazyWidgetBase lazyWidgetBase3 = LazyWidgetPrefabContainer.GetPrefabFromDataObject(lazyWidgetDataBase).Copy((this.widgetContainerGO != null) ? this.widgetContainerGO.transform : base.transform, true, "");
				lazyWidgetBase3.transform.SetSiblingIndex(num);
				num++;
				lazyWidgetBase3.Draw(lazyWidgetDataBase);
				this.displayedWidgets.Add(lazyWidgetBase3);
				this.usedWidgetsList.Add(lazyWidgetBase3);
			}
		}
		this.dataList = dataList;
	}

	// Token: 0x06003580 RID: 13696 RVA: 0x00101560 File Offset: 0x000FF760
	public void HideParticularWidget<T>() where T : LazyWidgetDataBase
	{
		for (int i = 0; i < this.displayedWidgets.Count; i++)
		{
			LazyWidgetBase lazyWidgetBase = this.displayedWidgets[i];
			if (lazyWidgetBase.GetDataType() == typeof(T))
			{
				lazyWidgetBase.Hide();
				lazyWidgetBase.gameObject.SetActive(false);
				this.displayedWidgets.RemoveAt(i);
				this.rectTransform.RefreshContentFitterAndDisable();
			}
		}
	}

	// Token: 0x06003581 RID: 13697 RVA: 0x001015D0 File Offset: 0x000FF7D0
	public bool TryDelayHide(Action onReadyToHide)
	{
		foreach (LazyWidgetBase lazyWidgetBase in this.displayedWidgets)
		{
			IDelayedUIHide delayedUIHide = lazyWidgetBase as IDelayedUIHide;
			if (delayedUIHide != null && delayedUIHide.ShouldDelayHide)
			{
				delayedUIHide.AddHideAfterDelayCallback(onReadyToHide);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003582 RID: 13698 RVA: 0x0010163C File Offset: 0x000FF83C
	public void ForceCancelDelayedHides()
	{
		foreach (LazyWidgetBase lazyWidgetBase in this.displayedWidgets)
		{
			IDelayedUIHide delayedUIHide = lazyWidgetBase as IDelayedUIHide;
			if (delayedUIHide != null)
			{
				delayedUIHide.ForceCancelDelayedHide();
			}
		}
	}

	// Token: 0x06003583 RID: 13699 RVA: 0x00101698 File Offset: 0x000FF898
	public bool HasDelayedHide()
	{
		foreach (LazyWidgetBase lazyWidgetBase in this.displayedWidgets)
		{
			IDelayedUIHide delayedUIHide = lazyWidgetBase as IDelayedUIHide;
			if (delayedUIHide != null && delayedUIHide.ShouldDelayHide)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003584 RID: 13700 RVA: 0x001016FC File Offset: 0x000FF8FC
	public bool TryDelayHideWidget<T>(Action onReadyToHide) where T : LazyWidgetDataBase
	{
		foreach (LazyWidgetBase lazyWidgetBase in this.displayedWidgets)
		{
			if (lazyWidgetBase.GetDataType() == typeof(T))
			{
				IDelayedUIHide delayedUIHide = lazyWidgetBase as IDelayedUIHide;
				if (delayedUIHide != null && delayedUIHide.ShouldDelayHide)
				{
					delayedUIHide.AddHideAfterDelayCallback(onReadyToHide);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003585 RID: 13701 RVA: 0x00101780 File Offset: 0x000FF980
	public bool TryDelayHideWidget(Type widgetDataType, Action onReadyToHide)
	{
		foreach (LazyWidgetBase lazyWidgetBase in this.displayedWidgets)
		{
			if (lazyWidgetBase.GetDataType() == widgetDataType)
			{
				IDelayedUIHide delayedUIHide = lazyWidgetBase as IDelayedUIHide;
				if (delayedUIHide != null && delayedUIHide.ShouldDelayHide)
				{
					delayedUIHide.AddHideAfterDelayCallback(onReadyToHide);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003586 RID: 13702 RVA: 0x001017FC File Offset: 0x000FF9FC
	public void HideParticularWidget(LazyWidgetDataBase widgetData)
	{
		for (int i = 0; i < this.displayedWidgets.Count; i++)
		{
			LazyWidgetBase lazyWidgetBase = this.displayedWidgets[i];
			if (lazyWidgetBase.GetDataType() == widgetData.GetType())
			{
				lazyWidgetBase.Hide();
				lazyWidgetBase.gameObject.SetActive(false);
				this.displayedWidgets.RemoveAt(i);
				this.rectTransform.RefreshContentFitterAndDisable();
			}
		}
	}

	// Token: 0x06003587 RID: 13703 RVA: 0x00101868 File Offset: 0x000FFA68
	public void Hide()
	{
		foreach (LazyWidgetBase lazyWidgetBase in this.usedWidgetsList)
		{
			lazyWidgetBase.Hide();
			lazyWidgetBase.gameObject.SetActive(false);
		}
		this.displayedWidgets.Clear();
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(false);
		}
		this.target = null;
	}

	// Token: 0x06003588 RID: 13704 RVA: 0x001018F0 File Offset: 0x000FFAF0
	public override void CustomUpdate()
	{
		if (this.isOutOfScreen)
		{
			return;
		}
		base.CustomUpdate();
	}

	// Token: 0x06003589 RID: 13705 RVA: 0x00101904 File Offset: 0x000FFB04
	public void UpdatePos()
	{
		if (this.target == null)
		{
			return;
		}
		base.transform.position = CameraSystem.WorldToScreenPoint(this.target.BubbleDrawablePosition);
		if (this.HasDelayedHide())
		{
			this.isOutOfScreen = false;
			return;
		}
		this.isOutOfScreen = this.rectTransform.IsRectOutOfScreen(LazyUI.GetScreenBounds());
	}

	// Token: 0x0600358A RID: 13706 RVA: 0x0010195C File Offset: 0x000FFB5C
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0.1f, 0.8f, 0.4f, 0.4f);
		Rect worldRect = this.rectTransform.GetWorldRect();
		Gizmos.DrawCube(worldRect.center, new Vector3(worldRect.width, worldRect.height, 0.4f));
	}

	// Token: 0x04002AE5 RID: 10981
	[SerializeField]
	private RectTransform rectTransform;

	// Token: 0x04002AE6 RID: 10982
	private IBubbleDrawable target;

	// Token: 0x04002AE7 RID: 10983
	private bool isOutOfScreen;
}

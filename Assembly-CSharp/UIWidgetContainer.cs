using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000859 RID: 2137
public class UIWidgetContainer : MonoBehaviour
{
	// Token: 0x17000825 RID: 2085
	// (get) Token: 0x060036C3 RID: 14019 RVA: 0x001093E9 File Offset: 0x001075E9
	public int DisplayedWidgetsCount
	{
		get
		{
			return this.displayedWidgets.Count;
		}
	}

	// Token: 0x060036C4 RID: 14020 RVA: 0x001093F8 File Offset: 0x001075F8
	protected void ConstructWidgets(List<LazyWidgetDataBase> dataList, Transform parent = null)
	{
		foreach (LazyWidgetBase lazyWidgetBase in this.usedWidgetsList)
		{
			lazyWidgetBase.Hide();
			lazyWidgetBase.gameObject.SetActive(false);
		}
		this.displayedWidgets.Clear();
		int num = 0;
		foreach (LazyWidgetDataBase lazyWidgetDataBase in dataList)
		{
			bool flag = false;
			foreach (LazyWidgetBase lazyWidgetBase2 in this.usedWidgetsList)
			{
				if (!lazyWidgetBase2.gameObject.activeSelf && lazyWidgetBase2.GetDataType() == lazyWidgetDataBase.GetType())
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
				LazyWidgetBase lazyWidgetBase3 = LazyWidgetPrefabContainer.GetPrefabFromDataObject(lazyWidgetDataBase).Copy(parent ? parent : ((this.widgetContainerGO != null) ? this.widgetContainerGO.transform : base.transform), true, "");
				lazyWidgetBase3.transform.SetSiblingIndex(num);
				num++;
				lazyWidgetBase3.Draw(lazyWidgetDataBase);
				this.displayedWidgets.Add(lazyWidgetBase3);
				this.usedWidgetsList.Add(lazyWidgetBase3);
			}
		}
		this.dataList = dataList;
	}

	// Token: 0x060036C5 RID: 14021 RVA: 0x001095D8 File Offset: 0x001077D8
	protected void ConstructWidgetsFromScratch(List<LazyWidgetDataBase> dataList)
	{
		if (this.usedWidgetsList.Count != 0)
		{
			foreach (LazyWidgetBase lazyWidgetBase in this.usedWidgetsList)
			{
				global::UnityEngine.Object.Destroy(lazyWidgetBase.gameObject);
			}
			this.usedWidgetsList.Clear();
		}
		foreach (LazyWidgetDataBase lazyWidgetDataBase in dataList)
		{
			LazyWidgetBase lazyWidgetBase2 = LazyWidgetPrefabContainer.GetPrefabFromDataObject(lazyWidgetDataBase).Copy(base.transform, true, "");
			lazyWidgetBase2.Draw(lazyWidgetDataBase);
			this.usedWidgetsList.Add(lazyWidgetBase2);
		}
		this.dataList = dataList;
	}

	// Token: 0x060036C6 RID: 14022 RVA: 0x001096B0 File Offset: 0x001078B0
	public virtual void CustomUpdate()
	{
		foreach (LazyWidgetBase lazyWidgetBase in this.usedWidgetsList)
		{
			if (lazyWidgetBase.gameObject.activeInHierarchy)
			{
				lazyWidgetBase.CustomUpdate();
			}
		}
	}

	// Token: 0x04002BB1 RID: 11185
	[SerializeField]
	protected GameObject widgetContainerGO;

	// Token: 0x04002BB2 RID: 11186
	[SerializeField]
	protected List<LazyWidgetBase> usedWidgetsList = new List<LazyWidgetBase>();

	// Token: 0x04002BB3 RID: 11187
	protected List<LazyWidgetBase> displayedWidgets = new List<LazyWidgetBase>();

	// Token: 0x04002BB4 RID: 11188
	protected List<LazyWidgetDataBase> dataList;
}

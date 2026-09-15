using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000167 RID: 359
	public class LazyWidgetContainer : MonoBehaviour
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x000271F0 File Offset: 0x000253F0
		public int DisplayedWidgetsCount
		{
			get
			{
				return this.displayedWidgets.Count;
			}
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00027200 File Offset: 0x00025400
		public virtual void ConstructWidgets(List<LazyWidgetDataBase> dataList, Transform parent = null)
		{
			foreach (LazyWidgetBase lazyWidgetBase in this.usedWidgetsList)
			{
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
						lazyWidgetBase2.gameObject.SetActive(true);
						lazyWidgetBase2.Draw(lazyWidgetDataBase);
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

		// Token: 0x060007BF RID: 1983 RVA: 0x000273DC File Offset: 0x000255DC
		public virtual void ConstructWidgetsFromScratch(List<LazyWidgetDataBase> dataList)
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

		// Token: 0x060007C0 RID: 1984 RVA: 0x000274B4 File Offset: 0x000256B4
		public virtual void CustomUpdate()
		{
			for (int i = 0; i < this.usedWidgetsList.Count; i++)
			{
				if (this.usedWidgetsList[i].gameObject.activeSelf)
				{
					this.usedWidgetsList[i].CustomUpdate();
				}
			}
		}

		// Token: 0x040004BE RID: 1214
		[SerializeField]
		protected GameObject widgetContainerGO;

		// Token: 0x040004BF RID: 1215
		[SerializeField]
		protected List<LazyWidgetBase> usedWidgetsList = new List<LazyWidgetBase>();

		// Token: 0x040004C0 RID: 1216
		protected List<LazyWidgetBase> displayedWidgets = new List<LazyWidgetBase>();

		// Token: 0x040004C1 RID: 1217
		protected List<LazyWidgetDataBase> dataList;
	}
}

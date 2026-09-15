using System;
using System.Collections.Generic;
using LinqTools;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000169 RID: 361
	public sealed class LazyWidgetPrefabContainer : LazySingleton<LazyWidgetPrefabContainer>
	{
		// Token: 0x060007C3 RID: 1987 RVA: 0x00027528 File Offset: 0x00025728
		public void Init()
		{
			List<LazyWidgetBase> list = base.GetComponentsInChildren<LazyWidgetBase>(true).ToList<LazyWidgetBase>();
			this.widgets = new Dictionary<Type, LazyWidgetBase>();
			foreach (LazyWidgetBase lazyWidgetBase in list)
			{
				if (!(lazyWidgetBase.transform.parent != this.container.transform))
				{
					this.widgets.Add(lazyWidgetBase.GetDataType(), lazyWidgetBase);
					Debug.Log(string.Format("#lazy_ui# Added new widget prefab with data type:[{0}] widget type:[{1}]", lazyWidgetBase.GetDataType(), lazyWidgetBase.GetType()));
				}
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x000275DC File Offset: 0x000257DC
		public static LazyWidgetBase GetPrefabFromDataType<T>() where T : LazyWidgetDataBase
		{
			LazyWidgetBase lazyWidgetBase;
			if (!LazySingleton<LazyWidgetPrefabContainer>.Instance.widgets.TryGetValue(typeof(T), out lazyWidgetBase))
			{
				throw new Exception(string.Format("Cannot find prefab for type {0}", typeof(T)));
			}
			return lazyWidgetBase;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00027624 File Offset: 0x00025824
		public static LazyWidgetBase GetPrefabFromDataType(Type type)
		{
			LazyWidgetBase lazyWidgetBase;
			if (!LazySingleton<LazyWidgetPrefabContainer>.Instance.widgets.TryGetValue(type, out lazyWidgetBase))
			{
				throw new Exception(string.Format("Cannot find prefab for type {0}", type));
			}
			return lazyWidgetBase;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00027658 File Offset: 0x00025858
		public static LazyWidgetBase GetPrefabFromDataObject(LazyWidgetDataBase data)
		{
			LazyWidgetBase lazyWidgetBase;
			if (!LazySingleton<LazyWidgetPrefabContainer>.Instance.widgets.TryGetValue(data.GetType(), out lazyWidgetBase))
			{
				throw new Exception(string.Format("Cannot find prefab for type {0}", data.GetType()));
			}
			return lazyWidgetBase;
		}

		// Token: 0x040004C2 RID: 1218
		private Dictionary<Type, LazyWidgetBase> widgets;

		// Token: 0x040004C3 RID: 1219
		[SerializeField]
		private GameObject container;
	}
}

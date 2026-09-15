using System;
using System.Collections.Generic;
using LinqTools;
using UnityEngine;
using UnityEngine.UI;

namespace LazyBearTechnology
{
	// Token: 0x02000152 RID: 338
	public static class LazyUI
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00024C70 File Offset: 0x00022E70
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x00024C77 File Offset: 0x00022E77
		public static float ScaleFactor { get; private set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x00024C7F File Offset: 0x00022E7F
		public static bool IsInitialized
		{
			get
			{
				return LazyUI.canvasScaler != null;
			}
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00024C8C File Offset: 0x00022E8C
		public static void Init(CanvasScaler canvasScaler = null)
		{
			if (LazyUI.IsInitialized)
			{
				Debug.LogWarning("LazyUI.Init() called for the second time.");
			}
			if (!canvasScaler)
			{
				canvasScaler = global::UnityEngine.Object.FindObjectOfType<CanvasScaler>(true);
				if (!canvasScaler)
				{
					Debug.LogError("Error in LazyUI.Init(): CanvasScaler is not found on the scene.");
				}
			}
			LazyUI.canvasScaler = canvasScaler;
			LazyWindowsStackController.OnWindowClosed -= LazyUI.OnWindowClosed;
			LazyWindowsStackController.OnWindowClosed += LazyUI.OnWindowClosed;
			List<ILazyGUIElement> list = new List<ILazyGUIElement>(global::UnityEngine.Object.FindObjectsOfType<MonoBehaviour>(true).OfType<ILazyGUIElement>());
			LazyUI.guiElementsDictionary = new Dictionary<Type, ILazyGUIElement>();
			list.ForEach(delegate(ILazyGUIElement uiElement)
			{
				uiElement.Init();
				LazyUI.TryAddGUIElement(uiElement);
			});
			foreach (LazyWidgetBase lazyWidgetBase in LazyUI.FindWidgetsOfGenericType(typeof(LazyWidget<>)))
			{
				Type type = lazyWidgetBase.GetType();
				if (!(type != typeof(LazyWindow<>)))
				{
					LazyUI.windowsCache.Add(type, lazyWidgetBase);
				}
			}
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00024DA0 File Offset: 0x00022FA0
		public static List<LazyWidgetBase> FindWidgetsOfGenericType(Type genericType)
		{
			List<LazyWidgetBase> list = new List<LazyWidgetBase>();
			foreach (LazyWidgetBase lazyWidgetBase in global::UnityEngine.Object.FindObjectsOfType<LazyWidgetBase>(true))
			{
				Type type = lazyWidgetBase.GetType();
				while (type != null && type != typeof(object))
				{
					if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
					{
						list.Add(lazyWidgetBase);
						break;
					}
					type = type.BaseType;
				}
			}
			return list;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00024E1E File Offset: 0x0002301E
		private static bool AssertInitialized()
		{
			if (!LazyUI.IsInitialized)
			{
				Debug.LogError("You should call LazyUI.Init() before calling this method.");
				return true;
			}
			return false;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00024E34 File Offset: 0x00023034
		public static void SetCanvasScaleFactor(float scaleFactor)
		{
			if (LazyUI.AssertInitialized())
			{
				return;
			}
			LazyUI.canvasScaler.scaleFactor = scaleFactor;
			LazyUI.ScaleFactor = scaleFactor;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00024E5C File Offset: 0x0002305C
		public static Bounds GetScreenBounds()
		{
			return new Bounds(LazyUI.safeZones.center, LazyUI.safeZones.size);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00024E81 File Offset: 0x00023081
		public static void SetSafeZones(Rect rect)
		{
			LazyUI.safeZones = rect;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00024E89 File Offset: 0x00023089
		public static void SetWindowLoadAction(Func<string, LazyWidgetBase> action)
		{
			LazyUI.loadWindowActionIfNotFound = action;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00024E94 File Offset: 0x00023094
		public static void ClearWindowsFromCache(List<string> windows)
		{
			foreach (Type type in LazyUI.windowsCache.Keys.Where((Type t) => windows.Contains(t.Name)).ToList<Type>())
			{
				LazyWidgetBase lazyWidgetBase = LazyUI.windowsCache[type];
				if (LazyWindowsStackController.IsWindowOpened(lazyWidgetBase))
				{
					LazyUI.windowsPendingRemoval.Add(lazyWidgetBase);
				}
				else
				{
					global::UnityEngine.Object.Destroy(lazyWidgetBase.gameObject);
					LazyUI.windowsCache.Remove(type);
				}
			}
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00024F40 File Offset: 0x00023140
		private static void OnWindowClosed(LazyWidgetBase window)
		{
			if (!LazyUI.windowsPendingRemoval.Contains(window))
			{
				return;
			}
			Type type = window.GetType();
			LazyWidgetBase lazyWidgetBase;
			if (LazyUI.windowsCache.TryGetValue(type, out lazyWidgetBase) && lazyWidgetBase == window)
			{
				LazyUI.windowsCache.Remove(type);
			}
			LazyUI.DestroyPendingWindow(window);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00024F8C File Offset: 0x0002318C
		private static void DestroyPendingWindow(LazyWidgetBase window)
		{
			if (window == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				global::UnityEngine.Object.Destroy(window.gameObject);
				return;
			}
			global::UnityEngine.Object.DestroyImmediate(window.gameObject);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00024FB6 File Offset: 0x000231B6
		public static bool TryAddGUIElement(ILazyGUIElement newElement)
		{
			if (!LazyUI.guiElementsDictionary.TryAdd(newElement.GetType(), newElement))
			{
				Debug.Log(string.Format("GUI with type [{0}] alreadyExist", newElement.GetType()));
				return false;
			}
			return true;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00024FE4 File Offset: 0x000231E4
		public static T GetElement<T>() where T : ILazyGUIElement
		{
			ILazyGUIElement lazyGUIElement;
			if (!LazyUI.guiElementsDictionary.TryGetValue(typeof(T), out lazyGUIElement))
			{
				throw new Exception(string.Format("no such type gui element: {0}", typeof(T)));
			}
			return (T)((object)lazyGUIElement);
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00025029 File Offset: 0x00023229
		public static T Get<T>() where T : ILazyGUIElement
		{
			return LazyUI.GetElement<T>();
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00025030 File Offset: 0x00023230
		public static T GetWindow<T>() where T : LazyWidgetBase
		{
			Type typeFromHandle = typeof(T);
			LazyWidgetBase lazyWidgetBase;
			if (LazyUI.windowsCache.TryGetValue(typeFromHandle, out lazyWidgetBase))
			{
				return (T)((object)lazyWidgetBase);
			}
			T t = global::UnityEngine.Object.FindObjectsOfType<T>(true).FirstOrDefault((T window) => !LazyUI.windowsPendingRemoval.Contains(window));
			if (t)
			{
				LazyUI.windowsCache.Add(typeFromHandle, t);
				return t;
			}
			if (LazyUI.loadWindowActionIfNotFound != null)
			{
				Func<string, LazyWidgetBase> func = LazyUI.loadWindowActionIfNotFound;
				t = ((func != null) ? func(typeFromHandle.Name) : null) as T;
				LazyUI.windowsCache.Add(typeFromHandle, t);
				return t;
			}
			Debug.LogError(string.Format("Error in LazyUI.Get<{0}>: Window of this class is not found on the scene.", typeFromHandle));
			return default(T);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00025100 File Offset: 0x00023300
		public static TInterface GetWindowByInterface<TInterface>(string windowTypeName) where TInterface : class
		{
			Type typeFromHandle = typeof(TInterface);
			if (!typeFromHandle.IsInterface)
			{
				Debug.LogError("Type " + typeFromHandle.Name + " is not an interface");
				return default(TInterface);
			}
			Type type = Type.GetType(windowTypeName);
			LazyWidgetBase lazyWidgetBase;
			if (type != null && LazyUI.windowsCache.TryGetValue(type, out lazyWidgetBase) && typeFromHandle.IsAssignableFrom(type))
			{
				return lazyWidgetBase as TInterface;
			}
			foreach (LazyWidgetBase lazyWidgetBase2 in global::UnityEngine.Object.FindObjectsOfType<LazyWidgetBase>(true))
			{
				if (!LazyUI.windowsPendingRemoval.Contains(lazyWidgetBase2))
				{
					type = lazyWidgetBase2.GetType();
					if (type.Name == windowTypeName && typeFromHandle.IsAssignableFrom(type))
					{
						LazyUI.windowsCache[type] = lazyWidgetBase2;
						return lazyWidgetBase2 as TInterface;
					}
				}
			}
			if (LazyUI.loadWindowActionIfNotFound != null)
			{
				Func<string, LazyWidgetBase> func = LazyUI.loadWindowActionIfNotFound;
				TInterface tinterface = ((func != null) ? func(windowTypeName) : null) as TInterface;
				LazyUI.windowsCache.Add(type, tinterface as LazyWidgetBase);
				return tinterface;
			}
			Debug.LogWarning(string.Concat(new string[] { "No window of type '", windowTypeName, "' implementing interface '", typeFromHandle.Name, "' was found." }));
			return default(TInterface);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00025260 File Offset: 0x00023460
		public static List<LazyWidgetBase> GetAllWindows()
		{
			List<LazyWidgetBase> list = LazyUI.FindWidgetsOfGenericType(typeof(LazyWindow<>));
			foreach (LazyWidgetBase lazyWidgetBase in LazyUI.windowsCache.Values)
			{
				if (lazyWidgetBase != null && !list.Contains(lazyWidgetBase))
				{
					list.Add(lazyWidgetBase);
				}
			}
			return list;
		}

		// Token: 0x04000463 RID: 1123
		private static CanvasScaler canvasScaler;

		// Token: 0x04000465 RID: 1125
		private static Dictionary<Type, LazyWidgetBase> windowsCache = new Dictionary<Type, LazyWidgetBase>();

		// Token: 0x04000466 RID: 1126
		private static HashSet<LazyWidgetBase> windowsPendingRemoval = new HashSet<LazyWidgetBase>();

		// Token: 0x04000467 RID: 1127
		private static Rect safeZones = Screen.safeArea;

		// Token: 0x04000468 RID: 1128
		private static Dictionary<Type, ILazyGUIElement> guiElementsDictionary;

		// Token: 0x04000469 RID: 1129
		private static Func<string, LazyWidgetBase> loadWindowActionIfNotFound;
	}
}

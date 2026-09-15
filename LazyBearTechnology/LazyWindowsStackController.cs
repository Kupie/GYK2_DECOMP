using System;
using System.Collections.Generic;

namespace LazyBearTechnology
{
	// Token: 0x0200016B RID: 363
	public static class LazyWindowsStackController
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060007E9 RID: 2025 RVA: 0x00027B7C File Offset: 0x00025D7C
		// (remove) Token: 0x060007EA RID: 2026 RVA: 0x00027BB0 File Offset: 0x00025DB0
		public static event Action<LazyWidgetBase> OnWindowOpened;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060007EB RID: 2027 RVA: 0x00027BE4 File Offset: 0x00025DE4
		// (remove) Token: 0x060007EC RID: 2028 RVA: 0x00027C18 File Offset: 0x00025E18
		public static event Action<LazyWidgetBase> OnWindowClosed;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060007ED RID: 2029 RVA: 0x00027C4C File Offset: 0x00025E4C
		// (remove) Token: 0x060007EE RID: 2030 RVA: 0x00027C80 File Offset: 0x00025E80
		public static event Action OnAllWindowsClosed;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060007EF RID: 2031 RVA: 0x00027CB4 File Offset: 0x00025EB4
		// (remove) Token: 0x060007F0 RID: 2032 RVA: 0x00027CE8 File Offset: 0x00025EE8
		public static event Action<LazyWidgetBase> OnWindowBecameVisibleInStack;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060007F1 RID: 2033 RVA: 0x00027D1C File Offset: 0x00025F1C
		// (remove) Token: 0x060007F2 RID: 2034 RVA: 0x00027D50 File Offset: 0x00025F50
		public static event Action<LazyWidgetBase> OnWindowBecameHiddenInStack;

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x00027D83 File Offset: 0x00025F83
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x00027D8A File Offset: 0x00025F8A
		public static bool HasAnyModalWindowOpened { get; private set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x00027D92 File Offset: 0x00025F92
		public static LazyWidgetBase ActiveWindow
		{
			get
			{
				return LazyWindowsStackController.activeWindow;
			}
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00027D9C File Offset: 0x00025F9C
		public static void AddToStack<T>(LazyWindow<T> window) where T : LazyWidgetDataBase
		{
			if (LazyWindowsStackController.openedWindowsStack.Count > 0)
			{
				Action<LazyWidgetBase> onWindowBecameHiddenInStack = LazyWindowsStackController.OnWindowBecameHiddenInStack;
				if (onWindowBecameHiddenInStack != null)
				{
					List<LazyWidgetBase> list = LazyWindowsStackController.openedWindowsStack;
					onWindowBecameHiddenInStack(list[list.Count - 1]);
				}
			}
			LazyWindowsStackController.openedWindowsStack.Add(window);
			LazyWindowsStackController.windowsWithModalStatus.TryAdd(window, window.IsModalWindow);
			LazyWindowsStackController.UpdateModality();
			LazyWindowsStackController.activeWindow = window;
			Action<LazyWidgetBase> onWindowOpened = LazyWindowsStackController.OnWindowOpened;
			if (onWindowOpened != null)
			{
				onWindowOpened(window);
			}
			Action<LazyWidgetBase> onWindowBecameVisibleInStack = LazyWindowsStackController.OnWindowBecameVisibleInStack;
			if (onWindowBecameVisibleInStack != null)
			{
				onWindowBecameVisibleInStack(window);
			}
			if (LazyWindowsStackController.openedWindowsStack.Count > 1)
			{
				LazyWindowsStackController.activeWindowSorting += 15;
			}
			else
			{
				LazyWindowsStackController.activeWindowSorting = LazyWindowsStackController.WINDOWS_INITIAL_SORTING_ORDER_VALUE;
			}
			window.Canvas.sortingOrder = LazyWindowsStackController.activeWindowSorting;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00027E58 File Offset: 0x00026058
		public static void RemoveFromStack<T>(LazyWindow<T> window) where T : LazyWidgetDataBase
		{
			if (!LazyWindowsStackController.openedWindowsStack.Contains(window))
			{
				return;
			}
			LazyWindowsStackController.openedWindowsStack.Remove(window);
			LazyWindowsStackController.windowsWithModalStatus.Remove(window);
			LazyWindowsStackController.UpdateModality();
			Action<LazyWidgetBase> onWindowClosed = LazyWindowsStackController.OnWindowClosed;
			if (onWindowClosed != null)
			{
				onWindowClosed(window);
			}
			if (LazyWindowsStackController.openedWindowsStack.Count > 0)
			{
				List<LazyWidgetBase> list = LazyWindowsStackController.openedWindowsStack;
				LazyWindowsStackController.activeWindow = list[list.Count - 1];
				Action<LazyWidgetBase> onWindowBecameVisibleInStack = LazyWindowsStackController.OnWindowBecameVisibleInStack;
				if (onWindowBecameVisibleInStack == null)
				{
					return;
				}
				List<LazyWidgetBase> list2 = LazyWindowsStackController.openedWindowsStack;
				onWindowBecameVisibleInStack(list2[list2.Count - 1]);
				return;
			}
			else
			{
				LazyWindowsStackController.activeWindow = null;
				Action onAllWindowsClosed = LazyWindowsStackController.OnAllWindowsClosed;
				if (onAllWindowsClosed == null)
				{
					return;
				}
				onAllWindowsClosed();
				return;
			}
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00027EFC File Offset: 0x000260FC
		public static bool IsWindowOnTop<T>(LazyWindow<T> window) where T : LazyWidgetDataBase
		{
			return LazyWindowsStackController.activeWindow == window;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00027F09 File Offset: 0x00026109
		public static bool IsWindowOpened(LazyWidgetBase window)
		{
			return LazyWindowsStackController.openedWindowsStack.Contains(window);
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00027F18 File Offset: 0x00026118
		private static void UpdateModality()
		{
			using (Dictionary<LazyWidgetBase, bool>.ValueCollection.Enumerator enumerator = LazyWindowsStackController.windowsWithModalStatus.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current)
					{
						LazyWindowsStackController.HasAnyModalWindowOpened = true;
						return;
					}
				}
			}
			LazyWindowsStackController.HasAnyModalWindowOpened = false;
		}

		// Token: 0x040004CE RID: 1230
		public static int WINDOWS_INITIAL_SORTING_ORDER_VALUE = 400;

		// Token: 0x040004D4 RID: 1236
		private static List<LazyWidgetBase> openedWindowsStack = new List<LazyWidgetBase>();

		// Token: 0x040004D5 RID: 1237
		private static Dictionary<LazyWidgetBase, bool> windowsWithModalStatus = new Dictionary<LazyWidgetBase, bool>();

		// Token: 0x040004D6 RID: 1238
		private static LazyWidgetBase activeWindow;

		// Token: 0x040004D7 RID: 1239
		private static int activeWindowSorting;
	}
}

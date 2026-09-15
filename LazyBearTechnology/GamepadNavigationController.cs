using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000149 RID: 329
	public class GamepadNavigationController : MonoBehaviour
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060006BF RID: 1727 RVA: 0x00022D38 File Offset: 0x00020F38
		// (remove) Token: 0x060006C0 RID: 1728 RVA: 0x00022D70 File Offset: 0x00020F70
		public event Action<GamepadNavigationItem> OnFocusedItemChanged;

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x00022DA8 File Offset: 0x00020FA8
		public GamepadNavigationItem FocusedItem
		{
			get
			{
				foreach (GamepadNavigationItem gamepadNavigationItem in this.selectableItems)
				{
					if (gamepadNavigationItem.IsFocused)
					{
						return gamepadNavigationItem;
					}
				}
				return null;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x00022E04 File Offset: 0x00021004
		public int FocusedItemIndex
		{
			get
			{
				GamepadNavigationItem focusedItem = this.FocusedItem;
				if (!(focusedItem != null))
				{
					return -1;
				}
				return focusedItem.Index;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x00022E29 File Offset: 0x00021029
		public bool IsEnabled
		{
			get
			{
				return this.isEnabled;
			}
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00022E31 File Offset: 0x00021031
		public void Enable()
		{
			if (this.isEnabled)
			{
				return;
			}
			this.isEnabled = true;
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00022E43 File Offset: 0x00021043
		public void Enable(bool focusOnFirstActive)
		{
			this.Enable();
			GamepadNavigationItem focusedItem = this.FocusedItem;
			if (focusedItem != null)
			{
				focusedItem.Unfocus();
			}
			this.ReinitItems(focusOnFirstActive, null, null);
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00022E68 File Offset: 0x00021068
		public void ReinitItems(bool focusOnFirstActive, List<GamepadNavigationItem> customItems = null, GamepadNavigationItem skipUnfocusItem = null)
		{
			this.guiScale = base.transform.lossyScale.x;
			foreach (GamepadNavigationItem gamepadNavigationItem in this.selectableItems)
			{
				if (gamepadNavigationItem != null && gamepadNavigationItem != skipUnfocusItem)
				{
					gamepadNavigationItem.Unfocus();
				}
			}
			this.selectableItems.Clear();
			if (customItems == null)
			{
				this.selectableItems.AddRange(base.GetComponentsInChildren<GamepadNavigationItem>());
			}
			else
			{
				this.selectableItems.AddRange(customItems);
			}
			this.RemoveNullsAndSetIndexes(this.selectableItems);
			int num = 0;
			foreach (GamepadNavigationItem gamepadNavigationItem2 in this.selectableItems)
			{
				gamepadNavigationItem2.Init(num++, this, this.guiScale);
			}
			if (focusOnFirstActive)
			{
				this.FocusOnFirstActive(-1);
			}
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00022F74 File Offset: 0x00021174
		protected void RemoveNullsAndSetIndexes(List<GamepadNavigationItem> items)
		{
			items.RemoveUnityNulls<GamepadNavigationItem>();
			for (int i = 0; i < items.Count; i++)
			{
				items[i].Index = i;
			}
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00022FA8 File Offset: 0x000211A8
		public void Disable()
		{
			if (!this.isEnabled)
			{
				return;
			}
			this.isEnabled = false;
			foreach (GamepadNavigationItem gamepadNavigationItem in this.selectableItems)
			{
				gamepadNavigationItem.Unfocus();
			}
			this.selectableItems.Clear();
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00023014 File Offset: 0x00021214
		public void ResetCustomDirections()
		{
			foreach (GamepadNavigationItem gamepadNavigationItem in this.selectableItems)
			{
				gamepadNavigationItem.ResetCustomDirections();
			}
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00023064 File Offset: 0x00021264
		public void LinkRoundNavigation()
		{
			if (this.selectableItems.Count > 1)
			{
				GamepadNavigationItem gamepadNavigationItem = this.selectableItems[0];
				GUIDirection guidirection = GUIDirection.Up;
				List<GamepadNavigationItem> list = this.selectableItems;
				gamepadNavigationItem.SetCustomDirectionItem(guidirection, list[list.Count - 1], true);
			}
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0002309A File Offset: 0x0002129A
		public void FindAndRememberFocusedItem()
		{
			this.RememberFocused(this.FocusedItem);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x000230A8 File Offset: 0x000212A8
		public void RememberFocused(GamepadNavigationItem item)
		{
			if (item == null)
			{
				return;
			}
			if (this.lastFocusedItems.ContainsKey(item.group))
			{
				this.lastFocusedItems[item.group] = item;
				return;
			}
			this.lastFocusedItems.Add(item.group, item);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x000230F8 File Offset: 0x000212F8
		public bool FocusOnFirstActive(int group = -1)
		{
			int num = -1;
			bool flag = group >= 0;
			foreach (GamepadNavigationItem gamepadNavigationItem in this.selectableItems)
			{
				num++;
				if (gamepadNavigationItem == null)
				{
					Debug.LogError("Found a null item while selecting group = " + group.ToString());
				}
				else if (gamepadNavigationItem.isActiveAndEnabled && gamepadNavigationItem.Active && (!flag || gamepadNavigationItem.group == group))
				{
					this.SetFocusedItem(num);
					return true;
				}
			}
			Debug.LogWarning("no selectable items", this);
			return false;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x000231AC File Offset: 0x000213AC
		public bool FocusOnLastActive(int group = -1)
		{
			bool flag = group >= 0;
			for (int i = this.selectableItems.Count - 1; i >= 0; i--)
			{
				GamepadNavigationItem gamepadNavigationItem = this.selectableItems[i];
				if (gamepadNavigationItem == null)
				{
					Debug.LogError("Found a null item while selecting group = " + group.ToString());
				}
				else if (gamepadNavigationItem.isActiveAndEnabled && gamepadNavigationItem.Active && (!flag || gamepadNavigationItem.group == group))
				{
					this.SetFocusedItem(i);
					return true;
				}
			}
			Debug.LogWarning("no selectable items", this);
			return false;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00023238 File Offset: 0x00021438
		public bool HaveSavedFocusForGroup(int group)
		{
			if (!this.lastFocusedItems.ContainsKey(group))
			{
				return false;
			}
			if (this.lastFocusedItems[group] == null)
			{
				this.lastFocusedItems.Remove(group);
				return false;
			}
			if (this.lastFocusedItems[group].isActiveAndEnabled && this.lastFocusedItems[group].Active)
			{
				return true;
			}
			this.lastFocusedItems.Remove(group);
			return false;
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x000232B0 File Offset: 0x000214B0
		public void RestoreFocus(int group = 0)
		{
			if (this.HaveSavedFocusForGroup(group))
			{
				this.SetFocusedItem(this.lastFocusedItems[group]);
				return;
			}
			Debug.LogWarning("no saved last focus for group " + group.ToString() + ", trying to select any in this group");
			this.FocusOnFirstActive(group);
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x000232FC File Offset: 0x000214FC
		public void RestoreSelection(int group)
		{
			this.RestoreFocus(group);
			this.SelectFocusedItem();
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0002330B File Offset: 0x0002150B
		public void SetFocusedItem(GamepadNavigationItem item)
		{
			this.SetFocusedItem(this.selectableItems.IndexOf(item));
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00023320 File Offset: 0x00021520
		public void SetFocusedItem(int focusIndex)
		{
			if (focusIndex < 0 || focusIndex >= this.selectableItems.Count)
			{
				focusIndex = 0;
			}
			GamepadNavigationItem gamepadNavigationItem = null;
			for (int i = 0; i < this.selectableItems.Count; i++)
			{
				if (i == focusIndex)
				{
					if (this.selectableItems[i].Active)
					{
						gamepadNavigationItem = this.selectableItems[i];
					}
				}
				else
				{
					this.selectableItems[i].Unfocus();
				}
			}
			if (gamepadNavigationItem != null)
			{
				gamepadNavigationItem.Focus();
				this.RememberFocused(gamepadNavigationItem);
				Action<GamepadNavigationItem> onFocusedItemChanged = this.OnFocusedItemChanged;
				if (onFocusedItemChanged == null)
				{
					return;
				}
				onFocusedItemChanged(gamepadNavigationItem);
			}
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x000233B8 File Offset: 0x000215B8
		public void SelectFocusedItem()
		{
			GamepadNavigationItem focusedItem = this.FocusedItem;
			if (focusedItem == null)
			{
				return;
			}
			focusedItem.Select();
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x000233CC File Offset: 0x000215CC
		public virtual void Navigate(GUIDirection direction)
		{
			this.RemoveNullsAndSetIndexes(this.selectableItems);
			if (this.selectableItems.Count == 0)
			{
				return;
			}
			GamepadNavigationItem focusedItem = this.FocusedItem;
			if (focusedItem == null)
			{
				this.SetFocusedItem(0);
				return;
			}
			GamepadNavigationItem customDirectionItem = focusedItem.GetCustomDirectionItem(direction);
			if (customDirectionItem != null && this.selectableItems.Contains(customDirectionItem))
			{
				if (customDirectionItem != focusedItem)
				{
					this.SetFocusedItem(customDirectionItem);
				}
				return;
			}
			int group = focusedItem.group;
			Vector2 pos = focusedItem.Pos;
			List<GamepadNavigationItem> list = new List<GamepadNavigationItem>();
			foreach (GamepadNavigationItem gamepadNavigationItem in this.selectableItems)
			{
				if (!(gamepadNavigationItem == focusedItem) && !this.NeedSkipItemBecauseOfState(gamepadNavigationItem) && !this.NeedSkipItemBecauseOfGroup(gamepadNavigationItem, group) && !this.NeedSkipItemBecauseOfDirection(gamepadNavigationItem, pos, direction))
				{
					list.Add(gamepadNavigationItem);
				}
			}
			if (list.Count == 0)
			{
				GamepadNavigationItem gamepadNavigationItem2 = this.TryGetItemFromOtherGroup(this.FocusedItem.group, pos, direction);
				if (gamepadNavigationItem2 != null)
				{
					this.SetFocusedItem(gamepadNavigationItem2);
					return;
				}
				if (this.TryNavigateVerticalLoop(focusedItem, group, direction))
				{
					return;
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			GamepadNavigationItem gamepadNavigationItem3;
			if (this.checkMaxCrossAxisDistance)
			{
				gamepadNavigationItem3 = this.GetNearestItemWithCrossAxisCorridor(list, pos, direction);
			}
			else
			{
				gamepadNavigationItem3 = this.GetNearestItemWithLegacyGridAndMaxDistance(list, pos, group, direction);
				if (gamepadNavigationItem3 == null)
				{
					return;
				}
			}
			if (this.restoreLastInGroup)
			{
				int group2 = gamepadNavigationItem3.group;
				if (group2 != group && this.HaveSavedFocusForGroup(group2))
				{
					this.RestoreFocus(group2);
					return;
				}
			}
			this.SetFocusedItem(gamepadNavigationItem3);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0002356C File Offset: 0x0002176C
		protected virtual GamepadNavigationItem GetNearestItemWithCrossAxisCorridor(List<GamepadNavigationItem> items, Vector2 currentPos, GUIDirection direction)
		{
			float scaleFactor = LazyUI.ScaleFactor;
			List<Vector2> list = new List<Vector2>(items.Count);
			for (int i = 0; i < items.Count; i++)
			{
				list.Add(items[i].Pos);
			}
			int num = GamepadNavigationController.FindNearestIndexInCrossAxisCorridor(currentPos, list, direction, this.maxCrossAxisDistance * scaleFactor);
			if (num >= 0)
			{
				return items[num];
			}
			GamepadNavigationItem gamepadNavigationItem;
			return this.GetNearestItemInList(items, currentPos, direction, out gamepadNavigationItem);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x000235D8 File Offset: 0x000217D8
		public static int FindNearestIndexInCrossAxisCorridor(Vector2 currentPos, List<Vector2> positions, GUIDirection direction, float maxCrossAxisDistance)
		{
			int num = -1;
			float num2 = float.MaxValue;
			float num3 = float.MaxValue;
			for (int i = 0; i < positions.Count; i++)
			{
				float num4;
				float num5;
				GamepadNavigationController.GetAlongAndCrossDistance(currentPos, positions[i], direction, out num4, out num5);
				if (num5 <= maxCrossAxisDistance && (num4 <= num2 || Mathf.Approximately(num4, num2)) && (!Mathf.Approximately(num4, num2) || num5 < num3))
				{
					num2 = num4;
					num3 = num5;
					num = i;
				}
			}
			return num;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00023644 File Offset: 0x00021844
		public static void GetAlongAndCrossDistance(Vector2 from, Vector2 to, GUIDirection direction, out float along, out float cross)
		{
			Vector2 vector = to - from;
			if (direction <= GUIDirection.Right)
			{
				along = Mathf.Abs(vector.x);
				cross = Mathf.Abs(vector.y);
				return;
			}
			if (direction - GUIDirection.Up > 1)
			{
				along = vector.magnitude;
				cross = 0f;
				return;
			}
			along = Mathf.Abs(vector.y);
			cross = Mathf.Abs(vector.x);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x000236B0 File Offset: 0x000218B0
		protected GamepadNavigationItem GetNearestItemWithLegacyGridAndMaxDistance(List<GamepadNavigationItem> possibleItems, Vector2 currentPos, int currentGroup, GUIDirection direction)
		{
			List<GamepadNavigationItem> list = new List<GamepadNavigationItem>(possibleItems);
			List<GamepadNavigationItem> list2 = new List<GamepadNavigationItem>();
			for (int i = 0; i < list.Count; i++)
			{
				bool flag = false;
				if (this.NeedSkipItemBecauseOfGroup(list[i], currentGroup))
				{
					flag = true;
				}
				if (!flag && this.SkipItemBecauseOfGrid(list[i], currentPos, direction))
				{
					list2.Add(list[i]);
					flag = true;
				}
				if (flag)
				{
					list.RemoveAt(i);
					i--;
				}
			}
			bool flag2 = false;
			if (this.useGridSkippedIfListEmpty && list.Count == 0 && list2.Count > 0)
			{
				list.AddRange(list2);
				flag2 = true;
			}
			if (list.Count == 0)
			{
				return null;
			}
			GamepadNavigationItem gamepadNavigationItem2;
			GamepadNavigationItem gamepadNavigationItem = this.GetNearestItemInList(list, currentPos, direction, out gamepadNavigationItem2);
			if (this.checkMaxDistanceBetweenElements)
			{
				if (gamepadNavigationItem2 == null)
				{
					if (this.useGridSkippedIfListEmpty && !flag2 && list2.Count > 0)
					{
						this.GetNearestItemInList(list2, currentPos, direction, out gamepadNavigationItem2);
						if (gamepadNavigationItem2 != null)
						{
							gamepadNavigationItem = gamepadNavigationItem2;
						}
					}
				}
				else
				{
					gamepadNavigationItem = gamepadNavigationItem2;
				}
			}
			return gamepadNavigationItem;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x000237B4 File Offset: 0x000219B4
		protected virtual GamepadNavigationItem GetNearestItemInList(List<GamepadNavigationItem> items, Vector2 currentPos, GUIDirection direction, out GamepadNavigationItem nearestIfCheckMaxDistance)
		{
			GamepadNavigationItem gamepadNavigationItem = items[0];
			float num = gamepadNavigationItem.CalcDistToCurrentPos(currentPos, direction);
			nearestIfCheckMaxDistance = null;
			float num2 = this.maxDistanceBetweenElements * LazyUI.ScaleFactor;
			if (this.checkMaxDistanceBetweenElements && num <= num2)
			{
				nearestIfCheckMaxDistance = gamepadNavigationItem;
			}
			for (int i = 1; i < items.Count; i++)
			{
				float num3 = items[i].CalcDistToCurrentPos(currentPos, direction);
				if (num3 < num)
				{
					num = num3;
					gamepadNavigationItem = items[i];
					if (this.checkMaxDistanceBetweenElements && num <= num2)
					{
						nearestIfCheckMaxDistance = gamepadNavigationItem;
					}
				}
			}
			return gamepadNavigationItem;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00023838 File Offset: 0x00021A38
		protected GamepadNavigationItem TryGetItemFromOtherGroup(int groupIndex, Vector2 currentPos, GUIDirection direction)
		{
			for (int i = 0; i < this.navigationGroupSources.Count; i++)
			{
				if (this.navigationGroupSources[i].group == groupIndex)
				{
					List<GamepadNavigationItem> list = new List<GamepadNavigationItem>();
					bool flag = false;
					int num = -1;
					for (int j = 0; j < this.navigationGroupSources[i].groupTargets.Count; j++)
					{
						if (this.navigationGroupSources[i].groupTargets[j].direction == direction)
						{
							num = this.navigationGroupSources[i].groupTargets[j].group;
							flag = true;
							for (int k = 0; k < this.selectableItems.Count; k++)
							{
								if (this.selectableItems[k].group == num && !this.NeedSkipItemBecauseOfState(this.selectableItems[k]))
								{
									list.Add(this.selectableItems[k]);
								}
							}
						}
					}
					if (list.Count > 0)
					{
						GamepadNavigationItem gamepadNavigationItem;
						return this.GetNearestItemInList(list, currentPos, direction, out gamepadNavigationItem);
					}
					if (flag)
					{
						return this.TryGetItemFromOtherGroup(num, currentPos, direction);
					}
				}
			}
			return null;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00023968 File Offset: 0x00021B68
		private bool TryNavigateVerticalLoop(GamepadNavigationItem currentItem, int currentGroup, GUIDirection direction)
		{
			if (!this.loopVerticalNavigation)
			{
				return false;
			}
			if (currentGroup != 0)
			{
				return false;
			}
			if (direction != GUIDirection.Up && direction != GUIDirection.Down)
			{
				return false;
			}
			GamepadNavigationItem verticalWrapItem = this.GetVerticalWrapItem(currentItem, currentGroup, direction);
			if (verticalWrapItem == null)
			{
				return false;
			}
			this.SetFocusedItem(verticalWrapItem);
			return true;
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x000239AC File Offset: 0x00021BAC
		private GamepadNavigationItem GetVerticalWrapItem(GamepadNavigationItem currentItem, int currentGroup, GUIDirection direction)
		{
			GamepadNavigationItem gamepadNavigationItem = null;
			float num = ((direction == GUIDirection.Up) ? float.MaxValue : float.MinValue);
			foreach (GamepadNavigationItem gamepadNavigationItem2 in this.selectableItems)
			{
				if (!(gamepadNavigationItem2 == currentItem) && !this.NeedSkipItemBecauseOfState(gamepadNavigationItem2) && !this.NeedSkipItemBecauseOfGroup(gamepadNavigationItem2, currentGroup))
				{
					float y = gamepadNavigationItem2.Pos.y;
					if (direction == GUIDirection.Up)
					{
						if (y < num)
						{
							num = y;
							gamepadNavigationItem = gamepadNavigationItem2;
						}
					}
					else if (y > num)
					{
						num = y;
						gamepadNavigationItem = gamepadNavigationItem2;
					}
				}
			}
			return gamepadNavigationItem;
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00023A50 File Offset: 0x00021C50
		protected bool NeedSkipItemBecauseOfState(GamepadNavigationItem item)
		{
			return item.IsFocused || !item.isActiveAndEnabled || !item.Active;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00023A6D File Offset: 0x00021C6D
		protected bool NeedSkipItemBecauseOfGroup(GamepadNavigationItem item, int neededGroup)
		{
			return item.group != neededGroup;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00023A7B File Offset: 0x00021C7B
		protected bool NeedSkipItemBecauseOfDirection(GamepadNavigationItem item, Vector2 currentPos, GUIDirection direction)
		{
			return !item.CorrectDirection(currentPos, direction);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00023A88 File Offset: 0x00021C88
		protected bool SkipItemBecauseOfGrid(GamepadNavigationItem item, Vector2 currentPos, GUIDirection direction)
		{
			return !item.CorrectGrid(currentPos, direction);
		}

		// Token: 0x0400040F RID: 1039
		public List<GamepadNavigationController.NavigationGroupSource> navigationGroupSources;

		// Token: 0x04000411 RID: 1041
		[SerializeField]
		protected List<GamepadNavigationItem> selectableItems = new List<GamepadNavigationItem>();

		// Token: 0x04000412 RID: 1042
		private Dictionary<int, GamepadNavigationItem> lastFocusedItems = new Dictionary<int, GamepadNavigationItem>();

		// Token: 0x04000413 RID: 1043
		public bool restoreLastInGroup;

		// Token: 0x04000414 RID: 1044
		public bool useGridSkippedIfListEmpty;

		// Token: 0x04000415 RID: 1045
		public bool ignoreHoldedKeys;

		// Token: 0x04000416 RID: 1046
		public bool loopVerticalNavigation;

		// Token: 0x04000417 RID: 1047
		public bool checkMaxDistanceBetweenElements;

		// Token: 0x04000418 RID: 1048
		public float maxDistanceBetweenElements;

		// Token: 0x04000419 RID: 1049
		public bool checkMaxCrossAxisDistance;

		// Token: 0x0400041A RID: 1050
		public float maxCrossAxisDistance;

		// Token: 0x0400041B RID: 1051
		private bool isEnabled;

		// Token: 0x0400041C RID: 1052
		private float guiScale;

		// Token: 0x020001F3 RID: 499
		[Serializable]
		public class NavigationGroupTarget
		{
			// Token: 0x06000A5D RID: 2653 RVA: 0x0002ECBF File Offset: 0x0002CEBF
			public NavigationGroupTarget(int group, GUIDirection direction)
			{
				this.group = group;
				this.direction = direction;
			}

			// Token: 0x040006A5 RID: 1701
			public int group;

			// Token: 0x040006A6 RID: 1702
			public GUIDirection direction;
		}

		// Token: 0x020001F4 RID: 500
		[Serializable]
		public class NavigationGroupSource
		{
			// Token: 0x06000A5E RID: 2654 RVA: 0x0002ECD5 File Offset: 0x0002CED5
			public NavigationGroupSource(int group, List<GamepadNavigationController.NavigationGroupTarget> groupTargets)
			{
				this.group = group;
				this.groupTargets = groupTargets;
			}

			// Token: 0x040006A7 RID: 1703
			public int group;

			// Token: 0x040006A8 RID: 1704
			public List<GamepadNavigationController.NavigationGroupTarget> groupTargets;
		}
	}
}

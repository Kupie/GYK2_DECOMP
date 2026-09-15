using System;
using System.Collections.Generic;

namespace Rewired.Glyphs
{
	// Token: 0x0200006A RID: 106
	public static class GlyphTools
	{
		// Token: 0x060005CE RID: 1486 RVA: 0x0000DD98 File Offset: 0x0000BF98
		public static bool TryGetActionElementMaps(int playerId, int actionId, AxisRange actionRange, ControllerElementGlyphSelectorOptions options, List<ActionElementMap> workingActionElementMaps, out ActionElementMap aemResult1, out ActionElementMap aemResult2)
		{
			aemResult1 = null;
			aemResult2 = null;
			if (!ReInput.isReady)
			{
				return false;
			}
			if (options == null)
			{
				return false;
			}
			if (workingActionElementMaps == null)
			{
				return false;
			}
			InputAction action = ReInput.mapping.GetAction(actionId);
			if (action == null)
			{
				return false;
			}
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return false;
			}
			Controller controller = player.controllers.GetLastActiveController();
			workingActionElementMaps.Clear();
			if (options.useLastActiveController && controller != null)
			{
				Controller controller2 = null;
				if (controller.type == ControllerType.Keyboard || controller.type == ControllerType.Mouse)
				{
					if (GlyphTools.IsMousePrioritizedOverKeyboard(options))
					{
						if (ReInput.controllers.Mouse.enabled && player.controllers.hasMouse)
						{
							controller = ReInput.controllers.Mouse;
							controller2 = ReInput.controllers.Keyboard;
						}
					}
					else if (ReInput.controllers.Keyboard.enabled && player.controllers.hasKeyboard)
					{
						controller = ReInput.controllers.Keyboard;
						controller2 = ReInput.controllers.Mouse;
					}
				}
				if (GlyphTools.GetElementMapsWithAction(player, controller.type, controller.id, actionId, true, workingActionElementMaps) > 0 && GlyphTools.TryGetActionElementMaps(action, actionRange, workingActionElementMaps, out aemResult1, out aemResult2))
				{
					return true;
				}
				if (controller2 != null && GlyphTools.GetElementMapsWithAction(player, controller2.type, controller2.id, actionId, true, workingActionElementMaps) > 0 && GlyphTools.TryGetActionElementMaps(action, actionRange, workingActionElementMaps, out aemResult1, out aemResult2))
				{
					return true;
				}
				if (GlyphTools.GetElementMapsWithAction(player, controller.type, actionId, true, workingActionElementMaps) > 0 && GlyphTools.TryGetActionElementMaps(action, actionRange, workingActionElementMaps, out aemResult1, out aemResult2))
				{
					return true;
				}
			}
			int num = 0;
			ControllerType controllerType;
			while (options.TryGetControllerTypeOrder(num, out controllerType))
			{
				if (GlyphTools.GetElementMapsWithAction(player, controllerType, actionId, true, workingActionElementMaps) > 0 && GlyphTools.TryGetActionElementMaps(action, actionRange, workingActionElementMaps, out aemResult1, out aemResult2))
				{
					return true;
				}
				num++;
			}
			return GlyphTools.GetElementMapsWithAction(player, actionId, true, workingActionElementMaps) > 0 && GlyphTools.TryGetActionElementMaps(action, actionRange, workingActionElementMaps, out aemResult1, out aemResult2);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0000DF60 File Offset: 0x0000C160
		public static bool TryGetActionElementMaps(InputAction action, AxisRange actionRange, List<ActionElementMap> tempAems, out ActionElementMap aemResult1, out ActionElementMap aemResult2)
		{
			aemResult1 = null;
			aemResult2 = null;
			bool flag = action.type == InputActionType.Axis;
			int count = tempAems.Count;
			for (int i = 0; i < count; i++)
			{
				if (flag)
				{
					if (actionRange == AxisRange.Full)
					{
						ActionElementMap actionElementMap = GlyphTools.FindFirstFullAxisBinding(tempAems);
						if (actionElementMap != null)
						{
							aemResult1 = actionElementMap;
							return true;
						}
						ActionElementMap actionElementMap2;
						if (GlyphTools.FindFirstSplitAxisBindingPair(tempAems, out actionElementMap, out actionElementMap2))
						{
							aemResult1 = actionElementMap;
							aemResult2 = actionElementMap2;
							return true;
						}
					}
					else
					{
						ActionElementMap actionElementMap = GlyphTools.FindFirstBinding(tempAems, actionRange);
						if (actionElementMap != null)
						{
							aemResult1 = actionElementMap;
							return true;
						}
					}
				}
				else
				{
					ActionElementMap actionElementMap = GlyphTools.FindFirstBinding(tempAems, actionRange);
					if (actionElementMap != null)
					{
						aemResult1 = actionElementMap;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0000DFE0 File Offset: 0x0000C1E0
		public static ActionElementMap FindFirstFullAxisBinding(List<ActionElementMap> actionElementMaps)
		{
			int count = actionElementMaps.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = actionElementMaps[i];
				if (actionElementMap.elementType == ControllerElementType.Axis && actionElementMap.axisType == AxisType.Normal)
				{
					return actionElementMap;
				}
			}
			return null;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0000E01C File Offset: 0x0000C21C
		public static ActionElementMap FindFirstBinding(List<ActionElementMap> actionElementMaps, AxisRange actionRange)
		{
			if (actionElementMaps.Count == 0)
			{
				return null;
			}
			int count = actionElementMaps.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = actionElementMaps[i];
				switch (actionRange)
				{
				case AxisRange.Full:
					if (actionElementMap.axisRange == AxisRange.Full)
					{
						return actionElementMap;
					}
					break;
				case AxisRange.Positive:
					if ((actionElementMap.axisType == AxisType.Split || actionElementMap.axisType == AxisType.None) && actionElementMap.axisContribution == Pole.Positive)
					{
						return actionElementMap;
					}
					break;
				case AxisRange.Negative:
					if ((actionElementMap.axisType == AxisType.Split || actionElementMap.axisType == AxisType.None) && actionElementMap.axisContribution == Pole.Negative)
					{
						return actionElementMap;
					}
					break;
				}
			}
			if (actionRange == AxisRange.Full)
			{
				for (int j = 0; j < count; j++)
				{
					ActionElementMap actionElementMap = actionElementMaps[j];
					if ((actionElementMap.axisType == AxisType.Split || actionElementMap.axisType == AxisType.None) && actionElementMap.axisContribution == Pole.Positive)
					{
						return actionElementMap;
					}
				}
			}
			return null;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0000E0D8 File Offset: 0x0000C2D8
		public static bool FindFirstSplitAxisBindingPair(List<ActionElementMap> actionElementMaps, out ActionElementMap negativeAem, out ActionElementMap positiveAem)
		{
			negativeAem = null;
			positiveAem = null;
			int count = actionElementMaps.Count;
			int i = 0;
			while (i < count)
			{
				ActionElementMap actionElementMap = actionElementMaps[i];
				if (actionElementMap.elementType == ControllerElementType.Axis)
				{
					if (actionElementMap.axisType != AxisType.Normal)
					{
						if (actionElementMap.axisType != AxisType.None)
						{
							goto IL_003D;
						}
					}
				}
				else if (actionElementMap.elementType == ControllerElementType.Button)
				{
					goto IL_003D;
				}
				IL_0055:
				i++;
				continue;
				IL_003D:
				if (actionElementMap.axisContribution == Pole.Positive)
				{
					if (positiveAem == null)
					{
						positiveAem = actionElementMap;
						goto IL_0055;
					}
					goto IL_0055;
				}
				else
				{
					if (negativeAem == null)
					{
						negativeAem = actionElementMap;
						goto IL_0055;
					}
					goto IL_0055;
				}
			}
			return negativeAem != null || positiveAem != null;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0000E150 File Offset: 0x0000C350
		public static bool IsMousePrioritizedOverKeyboard(ControllerElementGlyphSelectorOptions options)
		{
			if (options == null)
			{
				return false;
			}
			int num = 0;
			ControllerType controllerType;
			while (options.TryGetControllerTypeOrder(num, out controllerType))
			{
				if (controllerType == ControllerType.Mouse)
				{
					return true;
				}
				if (controllerType == ControllerType.Keyboard)
				{
					return false;
				}
				num++;
			}
			return false;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0000E184 File Offset: 0x0000C384
		private static int GetElementMapsWithAction(Player player, ControllerType controllerType, int controllerId, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			int count = results.Count;
			player.controllers.maps.GetElementMapsWithAction(controllerType, controllerId, actionId, skipDisabledMaps, results);
			GlyphTools.RemoveInvalidElementMaps(player, results, count);
			return results.Count - count;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0000E1C4 File Offset: 0x0000C3C4
		private static int GetElementMapsWithAction(Player player, ControllerType controllerType, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			int count = results.Count;
			player.controllers.maps.GetElementMapsWithAction(controllerType, actionId, skipDisabledMaps, results);
			GlyphTools.RemoveInvalidElementMaps(player, results, count);
			return results.Count - count;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0000E204 File Offset: 0x0000C404
		private static int GetElementMapsWithAction(Player player, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			int count = results.Count;
			player.controllers.maps.GetElementMapsWithAction(actionId, skipDisabledMaps, results);
			GlyphTools.RemoveInvalidElementMaps(player, results, count);
			return results.Count - count;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0000E240 File Offset: 0x0000C440
		private static int RemoveInvalidElementMaps(Player player, List<ActionElementMap> results, int startIndex)
		{
			int count = results.Count;
			for (int i = count - 1; i >= startIndex; i--)
			{
				if (!player.controllers.ContainsController(results[i].controllerMap.controller) || !results[i].controllerMap.controller.enabled)
				{
					results.RemoveAt(i);
				}
			}
			return count - results.Count;
		}
	}
}

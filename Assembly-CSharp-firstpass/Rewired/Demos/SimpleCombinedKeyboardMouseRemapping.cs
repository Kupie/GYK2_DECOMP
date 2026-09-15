using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x02000108 RID: 264
	[AddComponentMenu("")]
	public class SimpleCombinedKeyboardMouseRemapping : MonoBehaviour
	{
		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x0002522D File Offset: 0x0002342D
		private Player player
		{
			get
			{
				return ReInput.players.GetPlayer(0);
			}
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x0002523C File Offset: 0x0002343C
		private void OnEnable()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.inputMapper_keyboard.options.timeout = 5f;
			this.inputMapper_mouse.options.timeout = 5f;
			this.inputMapper_mouse.options.ignoreMouseXAxis = true;
			this.inputMapper_mouse.options.ignoreMouseYAxis = true;
			this.inputMapper_keyboard.options.allowButtonsOnFullAxisAssignment = false;
			this.inputMapper_mouse.options.allowButtonsOnFullAxisAssignment = false;
			this.inputMapper_keyboard.InputMappedEvent += this.OnInputMapped;
			this.inputMapper_keyboard.StoppedEvent += this.OnStopped;
			this.inputMapper_mouse.InputMappedEvent += this.OnInputMapped;
			this.inputMapper_mouse.StoppedEvent += this.OnStopped;
			this.InitializeUI();
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00025321 File Offset: 0x00023521
		private void OnDisable()
		{
			this.inputMapper_keyboard.Stop();
			this.inputMapper_mouse.Stop();
			this.inputMapper_keyboard.RemoveAllEventListeners();
			this.inputMapper_mouse.RemoveAllEventListeners();
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00025350 File Offset: 0x00023550
		private void RedrawUI()
		{
			this.controllerNameUIText.text = "Keyboard/Mouse";
			for (int i = 0; i < this.rows.Count; i++)
			{
				SimpleCombinedKeyboardMouseRemapping.Row row = this.rows[i];
				InputAction action = this.rows[i].action;
				string text = string.Empty;
				int actionElementMapId = -1;
				for (int j = 0; j < 2; j++)
				{
					ControllerType controllerType = ((j == 0) ? ControllerType.Keyboard : ControllerType.Mouse);
					foreach (ActionElementMap actionElementMap in this.player.controllers.maps.GetMap(controllerType, 0, "Default", "Default").ElementMapsWithAction(action.id))
					{
						if (actionElementMap.ShowInField(row.actionRange))
						{
							text = actionElementMap.elementIdentifierName;
							actionElementMapId = actionElementMap.id;
							break;
						}
					}
					if (actionElementMapId >= 0)
					{
						break;
					}
				}
				row.text.text = text;
				row.button.onClick.RemoveAllListeners();
				int index = i;
				row.button.onClick.AddListener(delegate
				{
					this.OnInputFieldClicked(index, actionElementMapId);
				});
			}
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x000254BC File Offset: 0x000236BC
		private void ClearUI()
		{
			this.controllerNameUIText.text = string.Empty;
			for (int i = 0; i < this.rows.Count; i++)
			{
				this.rows[i].text.text = string.Empty;
			}
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0002550C File Offset: 0x0002370C
		private void InitializeUI()
		{
			foreach (object obj in this.actionGroupTransform)
			{
				global::UnityEngine.Object.Destroy(((Transform)obj).gameObject);
			}
			foreach (object obj2 in this.fieldGroupTransform)
			{
				global::UnityEngine.Object.Destroy(((Transform)obj2).gameObject);
			}
			foreach (InputAction inputAction in ReInput.mapping.ActionsInCategory("Default"))
			{
				if (inputAction.type == InputActionType.Axis)
				{
					this.CreateUIRow(inputAction, AxisRange.Full, inputAction.descriptiveName);
					this.CreateUIRow(inputAction, AxisRange.Positive, (!string.IsNullOrEmpty(inputAction.positiveDescriptiveName)) ? inputAction.positiveDescriptiveName : (inputAction.descriptiveName + " +"));
					this.CreateUIRow(inputAction, AxisRange.Negative, (!string.IsNullOrEmpty(inputAction.negativeDescriptiveName)) ? inputAction.negativeDescriptiveName : (inputAction.descriptiveName + " -"));
				}
				else if (inputAction.type == InputActionType.Button)
				{
					this.CreateUIRow(inputAction, AxisRange.Positive, inputAction.descriptiveName);
				}
			}
			this.RedrawUI();
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x00025684 File Offset: 0x00023884
		private void CreateUIRow(InputAction action, AxisRange actionRange, string label)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.textPrefab);
			gameObject.transform.SetParent(this.actionGroupTransform);
			gameObject.transform.SetAsLastSibling();
			gameObject.GetComponent<Text>().text = label;
			GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
			gameObject2.transform.SetParent(this.fieldGroupTransform);
			gameObject2.transform.SetAsLastSibling();
			this.rows.Add(new SimpleCombinedKeyboardMouseRemapping.Row
			{
				action = action,
				actionRange = actionRange,
				button = gameObject2.GetComponent<Button>(),
				text = gameObject2.GetComponentInChildren<Text>()
			});
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00025724 File Offset: 0x00023924
		private void OnInputFieldClicked(int index, int actionElementMapToReplaceId)
		{
			if (index < 0 || index >= this.rows.Count)
			{
				return;
			}
			ControllerMap map = this.player.controllers.maps.GetMap(ControllerType.Keyboard, 0, "Default", "Default");
			ControllerMap map2 = this.player.controllers.maps.GetMap(ControllerType.Mouse, 0, "Default", "Default");
			ControllerMap controllerMap;
			if (map.ContainsElementMap(actionElementMapToReplaceId))
			{
				controllerMap = map;
			}
			else if (map2.ContainsElementMap(actionElementMapToReplaceId))
			{
				controllerMap = map2;
			}
			else
			{
				controllerMap = null;
			}
			this._replaceTargetMapping = new SimpleCombinedKeyboardMouseRemapping.TargetMapping
			{
				actionElementMapId = actionElementMapToReplaceId,
				controllerMap = controllerMap
			};
			base.StartCoroutine(this.StartListeningDelayed(index, map, map2, actionElementMapToReplaceId));
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x000257D4 File Offset: 0x000239D4
		private IEnumerator StartListeningDelayed(int index, ControllerMap keyboardMap, ControllerMap mouseMap, int actionElementMapToReplaceId)
		{
			yield return new WaitForSeconds(0.1f);
			this.inputMapper_keyboard.Start(new InputMapper.Context
			{
				actionId = this.rows[index].action.id,
				controllerMap = keyboardMap,
				actionRange = this.rows[index].actionRange,
				actionElementMapToReplace = keyboardMap.GetElementMap(actionElementMapToReplaceId)
			});
			this.inputMapper_mouse.Start(new InputMapper.Context
			{
				actionId = this.rows[index].action.id,
				controllerMap = mouseMap,
				actionRange = this.rows[index].actionRange,
				actionElementMapToReplace = mouseMap.GetElementMap(actionElementMapToReplaceId)
			});
			this.player.controllers.maps.SetMapsEnabled(false, "UI");
			this.statusUIText.text = "Listening...";
			yield break;
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x00025800 File Offset: 0x00023A00
		private void OnInputMapped(InputMapper.InputMappedEventData data)
		{
			this.inputMapper_keyboard.Stop();
			this.inputMapper_mouse.Stop();
			if (this._replaceTargetMapping.controllerMap != null && data.actionElementMap.controllerMap != this._replaceTargetMapping.controllerMap)
			{
				this._replaceTargetMapping.controllerMap.DeleteElementMap(this._replaceTargetMapping.actionElementMapId);
			}
			this.RedrawUI();
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0002586A File Offset: 0x00023A6A
		private void OnStopped(InputMapper.StoppedEventData data)
		{
			this.statusUIText.text = string.Empty;
			this.player.controllers.maps.SetMapsEnabled(true, "UI");
		}

		// Token: 0x04000697 RID: 1687
		private const string category = "Default";

		// Token: 0x04000698 RID: 1688
		private const string layout = "Default";

		// Token: 0x04000699 RID: 1689
		private const string uiCategory = "UI";

		// Token: 0x0400069A RID: 1690
		private InputMapper inputMapper_keyboard = new InputMapper();

		// Token: 0x0400069B RID: 1691
		private InputMapper inputMapper_mouse = new InputMapper();

		// Token: 0x0400069C RID: 1692
		public GameObject buttonPrefab;

		// Token: 0x0400069D RID: 1693
		public GameObject textPrefab;

		// Token: 0x0400069E RID: 1694
		public RectTransform fieldGroupTransform;

		// Token: 0x0400069F RID: 1695
		public RectTransform actionGroupTransform;

		// Token: 0x040006A0 RID: 1696
		public Text controllerNameUIText;

		// Token: 0x040006A1 RID: 1697
		public Text statusUIText;

		// Token: 0x040006A2 RID: 1698
		private List<SimpleCombinedKeyboardMouseRemapping.Row> rows = new List<SimpleCombinedKeyboardMouseRemapping.Row>();

		// Token: 0x040006A3 RID: 1699
		private SimpleCombinedKeyboardMouseRemapping.TargetMapping _replaceTargetMapping;

		// Token: 0x02000109 RID: 265
		private class Row
		{
			// Token: 0x040006A4 RID: 1700
			public InputAction action;

			// Token: 0x040006A5 RID: 1701
			public AxisRange actionRange;

			// Token: 0x040006A6 RID: 1702
			public Button button;

			// Token: 0x040006A7 RID: 1703
			public Text text;
		}

		// Token: 0x0200010A RID: 266
		private struct TargetMapping
		{
			// Token: 0x040006A8 RID: 1704
			public ControllerMap controllerMap;

			// Token: 0x040006A9 RID: 1705
			public int actionElementMapId;
		}
	}
}

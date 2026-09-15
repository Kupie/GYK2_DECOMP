using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x0200010D RID: 269
	[AddComponentMenu("")]
	public class SimpleControlRemapping : MonoBehaviour
	{
		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x0002522D File Offset: 0x0002342D
		private Player player
		{
			get
			{
				return ReInput.players.GetPlayer(0);
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x00025A50 File Offset: 0x00023C50
		private ControllerMap controllerMap
		{
			get
			{
				if (this.controller == null)
				{
					return null;
				}
				return this.player.controllers.maps.GetMap(this.controller.type, this.controller.id, "Default", "Default");
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x00025A9C File Offset: 0x00023C9C
		private Controller controller
		{
			get
			{
				return this.player.controllers.GetController(this.selectedControllerType, this.selectedControllerId);
			}
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x00025ABC File Offset: 0x00023CBC
		private void OnEnable()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.inputMapper.options.timeout = 5f;
			this.inputMapper.options.ignoreMouseXAxis = true;
			this.inputMapper.options.ignoreMouseYAxis = true;
			ReInput.ControllerConnectedEvent += this.OnControllerChanged;
			ReInput.ControllerDisconnectedEvent += this.OnControllerChanged;
			this.inputMapper.InputMappedEvent += this.OnInputMapped;
			this.inputMapper.StoppedEvent += this.OnStopped;
			this.InitializeUI();
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x00025B5E File Offset: 0x00023D5E
		private void OnDisable()
		{
			this.inputMapper.Stop();
			this.inputMapper.RemoveAllEventListeners();
			ReInput.ControllerConnectedEvent -= this.OnControllerChanged;
			ReInput.ControllerDisconnectedEvent -= this.OnControllerChanged;
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x00025B98 File Offset: 0x00023D98
		private void RedrawUI()
		{
			if (this.controller == null)
			{
				this.ClearUI();
				return;
			}
			this.controllerNameUIText.text = this.controller.name;
			for (int i = 0; i < this.rows.Count; i++)
			{
				SimpleControlRemapping.Row row = this.rows[i];
				InputAction action = this.rows[i].action;
				string text = string.Empty;
				int actionElementMapId = -1;
				foreach (ActionElementMap actionElementMap in this.controllerMap.ElementMapsWithAction(action.id))
				{
					if (actionElementMap.ShowInField(row.actionRange))
					{
						text = actionElementMap.elementIdentifierName;
						actionElementMapId = actionElementMap.id;
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

		// Token: 0x06000CEA RID: 3306 RVA: 0x00025CD4 File Offset: 0x00023ED4
		private void ClearUI()
		{
			if (this.selectedControllerType == ControllerType.Joystick)
			{
				this.controllerNameUIText.text = "No joysticks attached";
			}
			else
			{
				this.controllerNameUIText.text = string.Empty;
			}
			for (int i = 0; i < this.rows.Count; i++)
			{
				this.rows[i].text.text = string.Empty;
			}
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x00025D40 File Offset: 0x00023F40
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

		// Token: 0x06000CEC RID: 3308 RVA: 0x00025EB8 File Offset: 0x000240B8
		private void CreateUIRow(InputAction action, AxisRange actionRange, string label)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.textPrefab);
			gameObject.transform.SetParent(this.actionGroupTransform);
			gameObject.transform.SetAsLastSibling();
			gameObject.GetComponent<Text>().text = label;
			GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
			gameObject2.transform.SetParent(this.fieldGroupTransform);
			gameObject2.transform.SetAsLastSibling();
			this.rows.Add(new SimpleControlRemapping.Row
			{
				action = action,
				actionRange = actionRange,
				button = gameObject2.GetComponent<Button>(),
				text = gameObject2.GetComponentInChildren<Text>()
			});
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x00025F58 File Offset: 0x00024158
		private void SetSelectedController(ControllerType controllerType)
		{
			bool flag = false;
			if (controllerType != this.selectedControllerType)
			{
				this.selectedControllerType = controllerType;
				flag = true;
			}
			int num = this.selectedControllerId;
			if (this.selectedControllerType == ControllerType.Joystick)
			{
				if (this.player.controllers.joystickCount > 0)
				{
					this.selectedControllerId = this.player.controllers.Joysticks[0].id;
				}
				else
				{
					this.selectedControllerId = -1;
				}
			}
			else
			{
				this.selectedControllerId = 0;
			}
			if (this.selectedControllerId != num)
			{
				flag = true;
			}
			if (flag)
			{
				this.inputMapper.Stop();
				this.RedrawUI();
			}
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00025FEE File Offset: 0x000241EE
		public void OnControllerSelected(int controllerType)
		{
			this.SetSelectedController((ControllerType)controllerType);
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x00025FF7 File Offset: 0x000241F7
		private void OnInputFieldClicked(int index, int actionElementMapToReplaceId)
		{
			if (index < 0 || index >= this.rows.Count)
			{
				return;
			}
			if (this.controller == null)
			{
				return;
			}
			base.StartCoroutine(this.StartListeningDelayed(index, actionElementMapToReplaceId));
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00026024 File Offset: 0x00024224
		private IEnumerator StartListeningDelayed(int index, int actionElementMapToReplaceId)
		{
			yield return new WaitForSeconds(0.1f);
			this.inputMapper.Start(new InputMapper.Context
			{
				actionId = this.rows[index].action.id,
				controllerMap = this.controllerMap,
				actionRange = this.rows[index].actionRange,
				actionElementMapToReplace = this.controllerMap.GetElementMap(actionElementMapToReplaceId)
			});
			this.player.controllers.maps.SetMapsEnabled(false, "UI");
			this.statusUIText.text = "Listening...";
			yield break;
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00026041 File Offset: 0x00024241
		private void OnControllerChanged(ControllerStatusChangedEventArgs args)
		{
			this.SetSelectedController(this.selectedControllerType);
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0002604F File Offset: 0x0002424F
		private void OnInputMapped(InputMapper.InputMappedEventData data)
		{
			this.RedrawUI();
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x00026057 File Offset: 0x00024257
		private void OnStopped(InputMapper.StoppedEventData data)
		{
			this.statusUIText.text = string.Empty;
			this.player.controllers.maps.SetMapsEnabled(true, "UI");
		}

		// Token: 0x040006B4 RID: 1716
		private const string category = "Default";

		// Token: 0x040006B5 RID: 1717
		private const string layout = "Default";

		// Token: 0x040006B6 RID: 1718
		private const string uiCategory = "UI";

		// Token: 0x040006B7 RID: 1719
		private InputMapper inputMapper = new InputMapper();

		// Token: 0x040006B8 RID: 1720
		public GameObject buttonPrefab;

		// Token: 0x040006B9 RID: 1721
		public GameObject textPrefab;

		// Token: 0x040006BA RID: 1722
		public RectTransform fieldGroupTransform;

		// Token: 0x040006BB RID: 1723
		public RectTransform actionGroupTransform;

		// Token: 0x040006BC RID: 1724
		public Text controllerNameUIText;

		// Token: 0x040006BD RID: 1725
		public Text statusUIText;

		// Token: 0x040006BE RID: 1726
		private ControllerType selectedControllerType;

		// Token: 0x040006BF RID: 1727
		private int selectedControllerId;

		// Token: 0x040006C0 RID: 1728
		private List<SimpleControlRemapping.Row> rows = new List<SimpleControlRemapping.Row>();

		// Token: 0x0200010E RID: 270
		private class Row
		{
			// Token: 0x040006C1 RID: 1729
			public InputAction action;

			// Token: 0x040006C2 RID: 1730
			public AxisRange actionRange;

			// Token: 0x040006C3 RID: 1731
			public Button button;

			// Token: 0x040006C4 RID: 1732
			public Text text;
		}
	}
}

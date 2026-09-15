using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000BE RID: 190
	[AddComponentMenu("")]
	public class InputBehaviorWindow : Window
	{
		// Token: 0x06000A39 RID: 2617 RVA: 0x0001CDC4 File Offset: 0x0001AFC4
		public override void Initialize(int id, Func<int, bool> isFocusedCallback)
		{
			if (this.spawnTransform == null || this.doneButton == null || this.cancelButton == null || this.defaultButton == null || this.uiControlSetPrefab == null || this.uiSliderControlPrefab == null || this.doneButtonLabel == null || this.cancelButtonLabel == null || this.defaultButtonLabel == null)
			{
				Debug.LogError("Rewired Control Mapper: All inspector values must be assigned!");
				return;
			}
			this.inputBehaviorInfo = new List<InputBehaviorWindow.InputBehaviorInfo>();
			this.buttonCallbacks = new Dictionary<int, Action<int>>();
			this.doneButtonLabel.text = ControlMapper.GetLanguage().done;
			this.cancelButtonLabel.text = ControlMapper.GetLanguage().cancel;
			this.defaultButtonLabel.text = ControlMapper.GetLanguage().default_;
			base.Initialize(id, isFocusedCallback);
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0001CEB8 File Offset: 0x0001B0B8
		public void SetData(int playerId, ControlMapper.InputBehaviorSettings[] data)
		{
			if (!base.initialized)
			{
				return;
			}
			this.playerId = playerId;
			foreach (ControlMapper.InputBehaviorSettings inputBehaviorSettings in data)
			{
				if (inputBehaviorSettings != null && inputBehaviorSettings.isValid)
				{
					InputBehavior inputBehavior = this.GetInputBehavior(inputBehaviorSettings.inputBehaviorId);
					if (inputBehavior != null)
					{
						UIControlSet uicontrolSet = this.CreateControlSet();
						Dictionary<int, InputBehaviorWindow.PropertyType> dictionary = new Dictionary<int, InputBehaviorWindow.PropertyType>();
						string customEntry = ControlMapper.GetLanguage().GetCustomEntry(inputBehaviorSettings.labelLanguageKey);
						if (!string.IsNullOrEmpty(customEntry))
						{
							uicontrolSet.SetTitle(customEntry);
						}
						else
						{
							uicontrolSet.SetTitle(inputBehavior.name);
						}
						if (inputBehaviorSettings.showJoystickAxisSensitivity)
						{
							UISliderControl uisliderControl = this.CreateSlider(uicontrolSet, inputBehavior.id, null, ControlMapper.GetLanguage().GetCustomEntry(inputBehaviorSettings.joystickAxisSensitivityLabelLanguageKey), inputBehaviorSettings.joystickAxisSensitivityIcon, inputBehaviorSettings.joystickAxisSensitivityMin, inputBehaviorSettings.joystickAxisSensitivityMax, new Action<int, int, float>(this.JoystickAxisSensitivityValueChanged), new Action<int, int>(this.JoystickAxisSensitivityCanceled));
							uisliderControl.slider.value = Mathf.Clamp(inputBehavior.joystickAxisSensitivity, inputBehaviorSettings.joystickAxisSensitivityMin, inputBehaviorSettings.joystickAxisSensitivityMax);
							dictionary.Add(uisliderControl.id, InputBehaviorWindow.PropertyType.JoystickAxisSensitivity);
						}
						if (inputBehaviorSettings.showMouseXYAxisSensitivity)
						{
							UISliderControl uisliderControl2 = this.CreateSlider(uicontrolSet, inputBehavior.id, null, ControlMapper.GetLanguage().GetCustomEntry(inputBehaviorSettings.mouseXYAxisSensitivityLabelLanguageKey), inputBehaviorSettings.mouseXYAxisSensitivityIcon, inputBehaviorSettings.mouseXYAxisSensitivityMin, inputBehaviorSettings.mouseXYAxisSensitivityMax, new Action<int, int, float>(this.MouseXYAxisSensitivityValueChanged), new Action<int, int>(this.MouseXYAxisSensitivityCanceled));
							uisliderControl2.slider.value = Mathf.Clamp(inputBehavior.mouseXYAxisSensitivity, inputBehaviorSettings.mouseXYAxisSensitivityMin, inputBehaviorSettings.mouseXYAxisSensitivityMax);
							dictionary.Add(uisliderControl2.id, InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity);
						}
						this.inputBehaviorInfo.Add(new InputBehaviorWindow.InputBehaviorInfo(inputBehavior, uicontrolSet, dictionary));
					}
				}
			}
			base.defaultUIElement = this.doneButton.gameObject;
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0001D07D File Offset: 0x0001B27D
		public void SetButtonCallback(InputBehaviorWindow.ButtonIdentifier buttonIdentifier, Action<int> callback)
		{
			if (!base.initialized)
			{
				return;
			}
			if (callback == null)
			{
				return;
			}
			if (this.buttonCallbacks.ContainsKey((int)buttonIdentifier))
			{
				this.buttonCallbacks[(int)buttonIdentifier] = callback;
				return;
			}
			this.buttonCallbacks.Add((int)buttonIdentifier, callback);
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0001D0B8 File Offset: 0x0001B2B8
		public override void Cancel()
		{
			if (!base.initialized)
			{
				return;
			}
			foreach (InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo in this.inputBehaviorInfo)
			{
				inputBehaviorInfo.RestorePreviousData();
			}
			Action<int> action;
			if (!this.buttonCallbacks.TryGetValue(1, out action))
			{
				if (this.cancelCallback != null)
				{
					this.cancelCallback();
				}
				return;
			}
			action(base.id);
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0001D144 File Offset: 0x0001B344
		public void OnDone()
		{
			if (!base.initialized)
			{
				return;
			}
			Action<int> action;
			if (!this.buttonCallbacks.TryGetValue(0, out action))
			{
				return;
			}
			action(base.id);
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x00012167 File Offset: 0x00010367
		public void OnCancel()
		{
			this.Cancel();
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0001D178 File Offset: 0x0001B378
		public void OnRestoreDefault()
		{
			if (!base.initialized)
			{
				return;
			}
			foreach (InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo in this.inputBehaviorInfo)
			{
				inputBehaviorInfo.RestoreDefaultData();
			}
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0001D1D4 File Offset: 0x0001B3D4
		private void JoystickAxisSensitivityValueChanged(int inputBehaviorId, int controlId, float value)
		{
			this.GetInputBehavior(inputBehaviorId).joystickAxisSensitivity = value;
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0001D1E3 File Offset: 0x0001B3E3
		private void MouseXYAxisSensitivityValueChanged(int inputBehaviorId, int controlId, float value)
		{
			this.GetInputBehavior(inputBehaviorId).mouseXYAxisSensitivity = value;
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0001D1F4 File Offset: 0x0001B3F4
		private void JoystickAxisSensitivityCanceled(int inputBehaviorId, int controlId)
		{
			InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo = this.GetInputBehaviorInfo(inputBehaviorId);
			if (inputBehaviorInfo == null)
			{
				return;
			}
			inputBehaviorInfo.RestoreData(InputBehaviorWindow.PropertyType.JoystickAxisSensitivity, controlId);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0001D218 File Offset: 0x0001B418
		private void MouseXYAxisSensitivityCanceled(int inputBehaviorId, int controlId)
		{
			InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo = this.GetInputBehaviorInfo(inputBehaviorId);
			if (inputBehaviorInfo == null)
			{
				return;
			}
			inputBehaviorInfo.RestoreData(InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity, controlId);
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0001D239 File Offset: 0x0001B439
		public override void TakeInputFocus()
		{
			base.TakeInputFocus();
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0001D241 File Offset: 0x0001B441
		private UIControlSet CreateControlSet()
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.uiControlSetPrefab);
			gameObject.transform.SetParent(this.spawnTransform, false);
			return gameObject.GetComponent<UIControlSet>();
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0001D268 File Offset: 0x0001B468
		private UISliderControl CreateSlider(UIControlSet set, int inputBehaviorId, string defaultTitle, string overrideTitle, Sprite icon, float minValue, float maxValue, Action<int, int, float> valueChangedCallback, Action<int, int> cancelCallback)
		{
			UISliderControl uisliderControl = set.CreateSlider(this.uiSliderControlPrefab, icon, minValue, maxValue, delegate(int cId, float value)
			{
				valueChangedCallback(inputBehaviorId, cId, value);
			}, delegate(int cId)
			{
				cancelCallback(inputBehaviorId, cId);
			});
			string text = (string.IsNullOrEmpty(overrideTitle) ? defaultTitle : overrideTitle);
			if (!string.IsNullOrEmpty(text))
			{
				uisliderControl.showTitle = true;
				uisliderControl.title.text = text;
			}
			else
			{
				uisliderControl.showTitle = false;
			}
			uisliderControl.showIcon = icon != null;
			return uisliderControl;
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0001D2FF File Offset: 0x0001B4FF
		private InputBehavior GetInputBehavior(int id)
		{
			return ReInput.mapping.GetInputBehavior(this.playerId, id);
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0001D314 File Offset: 0x0001B514
		private InputBehaviorWindow.InputBehaviorInfo GetInputBehaviorInfo(int inputBehaviorId)
		{
			int count = this.inputBehaviorInfo.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.inputBehaviorInfo[i].inputBehavior.id == inputBehaviorId)
				{
					return this.inputBehaviorInfo[i];
				}
			}
			return null;
		}

		// Token: 0x040004F4 RID: 1268
		private const float minSensitivity = 0.1f;

		// Token: 0x040004F5 RID: 1269
		[SerializeField]
		private RectTransform spawnTransform;

		// Token: 0x040004F6 RID: 1270
		[SerializeField]
		private Button doneButton;

		// Token: 0x040004F7 RID: 1271
		[SerializeField]
		private Button cancelButton;

		// Token: 0x040004F8 RID: 1272
		[SerializeField]
		private Button defaultButton;

		// Token: 0x040004F9 RID: 1273
		[SerializeField]
		private TMP_Text doneButtonLabel;

		// Token: 0x040004FA RID: 1274
		[SerializeField]
		private TMP_Text cancelButtonLabel;

		// Token: 0x040004FB RID: 1275
		[SerializeField]
		private TMP_Text defaultButtonLabel;

		// Token: 0x040004FC RID: 1276
		[SerializeField]
		private GameObject uiControlSetPrefab;

		// Token: 0x040004FD RID: 1277
		[SerializeField]
		private GameObject uiSliderControlPrefab;

		// Token: 0x040004FE RID: 1278
		private List<InputBehaviorWindow.InputBehaviorInfo> inputBehaviorInfo;

		// Token: 0x040004FF RID: 1279
		private Dictionary<int, Action<int>> buttonCallbacks;

		// Token: 0x04000500 RID: 1280
		private int playerId;

		// Token: 0x020000BF RID: 191
		private class InputBehaviorInfo
		{
			// Token: 0x170003EC RID: 1004
			// (get) Token: 0x06000A4A RID: 2634 RVA: 0x0001D368 File Offset: 0x0001B568
			public InputBehavior inputBehavior
			{
				get
				{
					return this._inputBehavior;
				}
			}

			// Token: 0x170003ED RID: 1005
			// (get) Token: 0x06000A4B RID: 2635 RVA: 0x0001D370 File Offset: 0x0001B570
			public UIControlSet controlSet
			{
				get
				{
					return this._controlSet;
				}
			}

			// Token: 0x06000A4C RID: 2636 RVA: 0x0001D378 File Offset: 0x0001B578
			public InputBehaviorInfo(InputBehavior inputBehavior, UIControlSet controlSet, Dictionary<int, InputBehaviorWindow.PropertyType> idToProperty)
			{
				this._inputBehavior = inputBehavior;
				this._controlSet = controlSet;
				this.idToProperty = idToProperty;
				this.copyOfOriginal = new InputBehavior(inputBehavior);
			}

			// Token: 0x06000A4D RID: 2637 RVA: 0x0001D3A1 File Offset: 0x0001B5A1
			public void RestorePreviousData()
			{
				this._inputBehavior.ImportData(this.copyOfOriginal);
			}

			// Token: 0x06000A4E RID: 2638 RVA: 0x0001D3B5 File Offset: 0x0001B5B5
			public void RestoreDefaultData()
			{
				this._inputBehavior.Reset();
				this.RefreshControls();
			}

			// Token: 0x06000A4F RID: 2639 RVA: 0x0001D3C8 File Offset: 0x0001B5C8
			public void RestoreData(InputBehaviorWindow.PropertyType propertyType, int controlId)
			{
				if (propertyType != InputBehaviorWindow.PropertyType.JoystickAxisSensitivity)
				{
					if (propertyType != InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity)
					{
						return;
					}
					float mouseXYAxisSensitivity = this.copyOfOriginal.mouseXYAxisSensitivity;
					this._inputBehavior.mouseXYAxisSensitivity = mouseXYAxisSensitivity;
					UISliderControl control = this._controlSet.GetControl<UISliderControl>(controlId);
					if (control != null)
					{
						control.slider.value = mouseXYAxisSensitivity;
					}
				}
				else
				{
					float joystickAxisSensitivity = this.copyOfOriginal.joystickAxisSensitivity;
					this._inputBehavior.joystickAxisSensitivity = joystickAxisSensitivity;
					UISliderControl control2 = this._controlSet.GetControl<UISliderControl>(controlId);
					if (control2 != null)
					{
						control2.slider.value = joystickAxisSensitivity;
						return;
					}
				}
			}

			// Token: 0x06000A50 RID: 2640 RVA: 0x0001D454 File Offset: 0x0001B654
			public void RefreshControls()
			{
				if (this._controlSet == null)
				{
					return;
				}
				if (this.idToProperty == null)
				{
					return;
				}
				foreach (KeyValuePair<int, InputBehaviorWindow.PropertyType> keyValuePair in this.idToProperty)
				{
					UISliderControl control = this._controlSet.GetControl<UISliderControl>(keyValuePair.Key);
					if (!(control == null))
					{
						InputBehaviorWindow.PropertyType value = keyValuePair.Value;
						if (value != InputBehaviorWindow.PropertyType.JoystickAxisSensitivity)
						{
							if (value == InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity)
							{
								control.slider.value = this._inputBehavior.mouseXYAxisSensitivity;
							}
						}
						else
						{
							control.slider.value = this._inputBehavior.joystickAxisSensitivity;
						}
					}
				}
			}

			// Token: 0x04000501 RID: 1281
			private InputBehavior _inputBehavior;

			// Token: 0x04000502 RID: 1282
			private UIControlSet _controlSet;

			// Token: 0x04000503 RID: 1283
			private Dictionary<int, InputBehaviorWindow.PropertyType> idToProperty;

			// Token: 0x04000504 RID: 1284
			private InputBehavior copyOfOriginal;
		}

		// Token: 0x020000C0 RID: 192
		public enum ButtonIdentifier
		{
			// Token: 0x04000506 RID: 1286
			Done,
			// Token: 0x04000507 RID: 1287
			Cancel,
			// Token: 0x04000508 RID: 1288
			Default
		}

		// Token: 0x020000C1 RID: 193
		private enum PropertyType
		{
			// Token: 0x0400050A RID: 1290
			JoystickAxisSensitivity,
			// Token: 0x0400050B RID: 1291
			MouseXYAxisSensitivity
		}
	}
}

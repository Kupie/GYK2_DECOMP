using System;
using System.Collections.Generic;
using Rewired.Glyphs;
using Rewired.Glyphs.UnityUI;
using Rewired.Integration.UnityUI;
using Rewired.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x0200008F RID: 143
	[AddComponentMenu("")]
	public class CalibrationWindow : Window
	{
		// Token: 0x1700031F RID: 799
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x00011BA4 File Offset: 0x0000FDA4
		private bool axisSelected
		{
			get
			{
				return this.joystick != null && this.selectedAxis >= 0 && this.selectedAxis < this.joystick.calibrationMap.axisCount;
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x00011BD4 File Offset: 0x0000FDD4
		private AxisCalibration axisCalibration
		{
			get
			{
				if (!this.axisSelected)
				{
					return null;
				}
				return this.joystick.calibrationMap.GetAxis(this.selectedAxis);
			}
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00011BF8 File Offset: 0x0000FDF8
		public override void Initialize(int id, Func<int, bool> isFocusedCallback)
		{
			if (this.rightContentContainer == null || this.valueDisplayGroup == null || this.calibratedValueMarker == null || this.rawValueMarker == null || this.calibratedZeroMarker == null || this.deadzoneArea == null || this.deadzoneSlider == null || this.sensitivitySlider == null || this.zeroSlider == null || this.invertToggle == null || this.axisScrollAreaContent == null || this.doneButton == null || this.calibrateButton == null || this.axisButtonPrefab == null || this.doneButtonLabel == null || this.cancelButtonLabel == null || this.defaultButtonLabel == null || this.deadzoneSliderLabel == null || this.zeroSliderLabel == null || this.sensitivitySliderLabel == null || this.invertToggleLabel == null || this.calibrateButtonLabel == null)
			{
				Debug.LogError("Rewired Control Mapper: All inspector values must be assigned!");
				return;
			}
			this.axisButtons = new List<Button>();
			this.buttonCallbacks = new Dictionary<int, Action<int>>();
			this.doneButtonLabel.text = ControlMapper.GetLanguage().done;
			this.cancelButtonLabel.text = ControlMapper.GetLanguage().cancel;
			this.defaultButtonLabel.text = ControlMapper.GetLanguage().default_;
			this.deadzoneSliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_deadZoneSliderLabel;
			this.zeroSliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_zeroSliderLabel;
			this.sensitivitySliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_sensitivitySliderLabel;
			this.invertToggleLabel.text = ControlMapper.GetLanguage().calibrateWindow_invertToggleLabel;
			this.calibrateButtonLabel.text = ControlMapper.GetLanguage().calibrateWindow_calibrateButtonLabel;
			base.Initialize(id, isFocusedCallback);
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x00011E30 File Offset: 0x00010030
		public void SetJoystick(int playerId, Joystick joystick)
		{
			if (!base.initialized)
			{
				return;
			}
			this.playerId = playerId;
			this.joystick = joystick;
			if (joystick == null)
			{
				Debug.LogError("Rewired Control Mapper: Joystick cannot be null!");
				return;
			}
			float num = 0f;
			for (int i = 0; i < joystick.axisCount; i++)
			{
				int index = i;
				GameObject gameObject = UITools.InstantiateGUIObject<Button>(this.axisButtonPrefab, this.axisScrollAreaContent, "Axis" + i.ToString());
				Button button = gameObject.GetComponent<Button>();
				button.onClick.AddListener(delegate
				{
					this.OnAxisSelected(index, button);
				});
				UnityUIControllerElementGlyph componentInSelfOrChildren = UnityTools.GetComponentInSelfOrChildren<UnityUIControllerElementGlyph>(gameObject);
				if (componentInSelfOrChildren != null)
				{
					componentInSelfOrChildren.allowedTypes = (ControlMapper.current.showGlyphs ? ControllerElementGlyphBase.AllowedTypes.All : ControllerElementGlyphBase.AllowedTypes.Text);
					componentInSelfOrChildren.controllerElementIdentifier = joystick.AxisElementIdentifiers[i];
				}
				else
				{
					TMP_Text componentInSelfOrChildren2 = UnityTools.GetComponentInSelfOrChildren<TMP_Text>(gameObject);
					if (componentInSelfOrChildren2 != null)
					{
						componentInSelfOrChildren2.text = ControlMapper.GetLanguage().GetElementIdentifierName(joystick, joystick.AxisElementIdentifiers[i].id, AxisRange.Full);
					}
				}
				if (num == 0f)
				{
					num = UnityTools.GetComponentInSelfOrChildren<LayoutElement>(gameObject).minHeight;
				}
				this.axisButtons.Add(button);
			}
			float spacing = this.axisScrollAreaContent.GetComponent<VerticalLayoutGroup>().spacing;
			this.axisScrollAreaContent.sizeDelta = new Vector2(this.axisScrollAreaContent.sizeDelta.x, Mathf.Max((float)joystick.axisCount * (num + spacing) - spacing, this.axisScrollAreaContent.sizeDelta.y));
			this.origCalibrationData = joystick.calibrationMap.ToXmlString();
			this.displayAreaWidth = this.rightContentContainer.sizeDelta.x;
			this.rewiredStandaloneInputModule = base.gameObject.transform.root.GetComponentInChildren<RewiredStandaloneInputModule>();
			if (this.rewiredStandaloneInputModule != null)
			{
				this.menuHorizActionId = ReInput.mapping.GetActionId(this.rewiredStandaloneInputModule.horizontalAxis);
				this.menuVertActionId = ReInput.mapping.GetActionId(this.rewiredStandaloneInputModule.verticalAxis);
			}
			if (joystick.axisCount > 0)
			{
				this.SelectAxis(0);
			}
			base.defaultUIElement = this.doneButton.gameObject;
			this.RefreshControls();
			this.Redraw();
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00012082 File Offset: 0x00010282
		public void SetButtonCallback(CalibrationWindow.ButtonIdentifier buttonIdentifier, Action<int> callback)
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

		// Token: 0x06000721 RID: 1825 RVA: 0x000120BC File Offset: 0x000102BC
		public override void Cancel()
		{
			if (!base.initialized)
			{
				return;
			}
			if (this.joystick != null)
			{
				this.joystick.ImportCalibrationMapFromXmlString(this.origCalibrationData);
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

		// Token: 0x06000722 RID: 1826 RVA: 0x0001211C File Offset: 0x0001031C
		protected override void Update()
		{
			if (!base.initialized)
			{
				return;
			}
			base.Update();
			this.UpdateDisplay();
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00012134 File Offset: 0x00010334
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

		// Token: 0x06000724 RID: 1828 RVA: 0x00012167 File Offset: 0x00010367
		public void OnCancel()
		{
			this.Cancel();
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001216F File Offset: 0x0001036F
		public void OnRestoreDefault()
		{
			if (!base.initialized)
			{
				return;
			}
			if (this.joystick == null)
			{
				return;
			}
			this.joystick.calibrationMap.Reset();
			this.RefreshControls();
			this.Redraw();
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x000121A0 File Offset: 0x000103A0
		public void OnCalibrate()
		{
			if (!base.initialized)
			{
				return;
			}
			Action<int> action;
			if (!this.buttonCallbacks.TryGetValue(3, out action))
			{
				return;
			}
			action(this.selectedAxis);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x000121D3 File Offset: 0x000103D3
		public void OnInvert(bool state)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.invert = state;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x000121F3 File Offset: 0x000103F3
		public void OnZeroValueChange(float value)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.calibratedZero = value;
			this.RedrawCalibratedZero();
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00012219 File Offset: 0x00010419
		public void OnZeroCancel()
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.calibratedZero = this.origSelectedAxisCalibrationData.zero;
			this.RedrawCalibratedZero();
			this.RefreshControls();
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00012250 File Offset: 0x00010450
		public void OnDeadzoneValueChange(float value)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.deadZone = Mathf.Clamp(value, 0f, 0.8f);
			if (value > 0.8f)
			{
				this.deadzoneSlider.value = 0.8f;
			}
			this.RedrawDeadzone();
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x000122A8 File Offset: 0x000104A8
		public void OnDeadzoneCancel()
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.deadZone = this.origSelectedAxisCalibrationData.deadZone;
			this.RedrawDeadzone();
			this.RefreshControls();
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x000122DE File Offset: 0x000104DE
		public void OnSensitivityValueChange(float value)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.SetSensitivity(this.axisCalibration, value);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x000122FF File Offset: 0x000104FF
		public void OnSensitivityCancel(float value)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.sensitivity = this.origSelectedAxisCalibrationData.sensitivity;
			this.RefreshControls();
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0001232F File Offset: 0x0001052F
		public void OnAxisScrollRectScroll(Vector2 pos)
		{
			bool initialized = base.initialized;
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00012338 File Offset: 0x00010538
		private void OnAxisSelected(int axisIndex, Button button)
		{
			if (!base.initialized)
			{
				return;
			}
			if (this.joystick == null)
			{
				return;
			}
			this.SelectAxis(axisIndex);
			this.RefreshControls();
			this.Redraw();
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0001235F File Offset: 0x0001055F
		private void UpdateDisplay()
		{
			this.RedrawValueMarkers();
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00012367 File Offset: 0x00010567
		private void Redraw()
		{
			this.RedrawCalibratedZero();
			this.RedrawValueMarkers();
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00012378 File Offset: 0x00010578
		private void RefreshControls()
		{
			if (!this.axisSelected)
			{
				this.deadzoneSlider.value = 0f;
				this.zeroSlider.value = 0f;
				this.sensitivitySlider.value = 0f;
				this.invertToggle.isOn = false;
				return;
			}
			this.deadzoneSlider.value = this.axisCalibration.deadZone;
			this.zeroSlider.value = this.axisCalibration.calibratedZero;
			this.sensitivitySlider.value = this.GetSliderSensitivity(this.axisCalibration);
			this.invertToggle.isOn = this.axisCalibration.invert;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00012424 File Offset: 0x00010624
		private void RedrawDeadzone()
		{
			if (!this.axisSelected)
			{
				return;
			}
			float num = this.displayAreaWidth * this.axisCalibration.deadZone;
			this.deadzoneArea.sizeDelta = new Vector2(num, this.deadzoneArea.sizeDelta.y);
			this.deadzoneArea.anchoredPosition = new Vector2(this.axisCalibration.calibratedZero * -this.deadzoneArea.parent.localPosition.x, this.deadzoneArea.anchoredPosition.y);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x000124B0 File Offset: 0x000106B0
		private void RedrawCalibratedZero()
		{
			if (!this.axisSelected)
			{
				return;
			}
			this.calibratedZeroMarker.anchoredPosition = new Vector2(this.axisCalibration.calibratedZero * -this.deadzoneArea.parent.localPosition.x, this.calibratedZeroMarker.anchoredPosition.y);
			this.RedrawDeadzone();
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00012510 File Offset: 0x00010710
		private void RedrawValueMarkers()
		{
			if (!this.axisSelected)
			{
				this.calibratedValueMarker.anchoredPosition = new Vector2(0f, this.calibratedValueMarker.anchoredPosition.y);
				this.rawValueMarker.anchoredPosition = new Vector2(0f, this.rawValueMarker.anchoredPosition.y);
				return;
			}
			float axis = this.joystick.GetAxis(this.selectedAxis);
			float num = Mathf.Clamp(this.joystick.GetAxisRaw(this.selectedAxis), -1f, 1f);
			this.calibratedValueMarker.anchoredPosition = new Vector2(this.displayAreaWidth * 0.5f * axis, this.calibratedValueMarker.anchoredPosition.y);
			this.rawValueMarker.anchoredPosition = new Vector2(this.displayAreaWidth * 0.5f * num, this.rawValueMarker.anchoredPosition.y);
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00012600 File Offset: 0x00010800
		private void SelectAxis(int index)
		{
			if (index < 0 || index >= this.axisButtons.Count)
			{
				return;
			}
			if (this.axisButtons[index] == null)
			{
				return;
			}
			this.axisButtons[index].interactable = false;
			this.axisButtons[index].Select();
			for (int i = 0; i < this.axisButtons.Count; i++)
			{
				if (i != index)
				{
					this.axisButtons[i].interactable = true;
				}
			}
			this.selectedAxis = index;
			this.origSelectedAxisCalibrationData = this.axisCalibration.GetData();
			this.SetMinSensitivity();
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x000126A2 File Offset: 0x000108A2
		public override void TakeInputFocus()
		{
			base.TakeInputFocus();
			if (this.selectedAxis >= 0)
			{
				this.SelectAxis(this.selectedAxis);
			}
			this.RefreshControls();
			this.Redraw();
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x000126CC File Offset: 0x000108CC
		private void SetMinSensitivity()
		{
			if (!this.axisSelected)
			{
				return;
			}
			this.minSensitivity = 0.1f;
			if (this.rewiredStandaloneInputModule != null)
			{
				if (this.IsMenuAxis(this.menuHorizActionId, this.selectedAxis))
				{
					this.GetAxisButtonDeadZone(this.playerId, this.menuHorizActionId, ref this.minSensitivity);
					return;
				}
				if (this.IsMenuAxis(this.menuVertActionId, this.selectedAxis))
				{
					this.GetAxisButtonDeadZone(this.playerId, this.menuVertActionId, ref this.minSensitivity);
				}
			}
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00012754 File Offset: 0x00010954
		private bool IsMenuAxis(int actionId, int axisIndex)
		{
			if (this.rewiredStandaloneInputModule == null)
			{
				return false;
			}
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			int count = allPlayers.Count;
			for (int i = 0; i < count; i++)
			{
				IList<JoystickMap> maps = allPlayers[i].controllers.maps.GetMaps<JoystickMap>(this.joystick.id);
				if (maps != null)
				{
					int count2 = maps.Count;
					for (int j = 0; j < count2; j++)
					{
						IList<ActionElementMap> axisMaps = maps[j].AxisMaps;
						if (axisMaps != null)
						{
							int count3 = axisMaps.Count;
							for (int k = 0; k < count3; k++)
							{
								ActionElementMap actionElementMap = axisMaps[k];
								if (actionElementMap.actionId == actionId && actionElementMap.elementIndex == axisIndex)
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00012824 File Offset: 0x00010A24
		private void GetAxisButtonDeadZone(int playerId, int actionId, ref float value)
		{
			InputAction action = ReInput.mapping.GetAction(actionId);
			if (action == null)
			{
				return;
			}
			int behaviorId = action.behaviorId;
			InputBehavior inputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
			if (inputBehavior == null)
			{
				return;
			}
			value = inputBehavior.buttonDeadZone + 0.1f;
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00012867 File Offset: 0x00010A67
		private float GetSliderSensitivity(AxisCalibration axisCalibration)
		{
			if (axisCalibration.sensitivityType == AxisSensitivityType.Multiplier)
			{
				return axisCalibration.sensitivity;
			}
			if (axisCalibration.sensitivityType == AxisSensitivityType.Power)
			{
				return CalibrationWindow.ProcessPowerValue(axisCalibration.sensitivity, 0f, this.sensitivitySlider.maxValue);
			}
			return axisCalibration.sensitivity;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x000128A4 File Offset: 0x00010AA4
		public void SetSensitivity(AxisCalibration axisCalibration, float sliderValue)
		{
			if (axisCalibration.sensitivityType == AxisSensitivityType.Multiplier)
			{
				axisCalibration.sensitivity = Mathf.Clamp(sliderValue, this.minSensitivity, float.PositiveInfinity);
				if (sliderValue < this.minSensitivity)
				{
					this.sensitivitySlider.value = this.minSensitivity;
					return;
				}
			}
			else
			{
				if (axisCalibration.sensitivityType == AxisSensitivityType.Power)
				{
					axisCalibration.sensitivity = CalibrationWindow.ProcessPowerValue(sliderValue, 0f, this.sensitivitySlider.maxValue);
					return;
				}
				axisCalibration.sensitivity = sliderValue;
			}
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00012918 File Offset: 0x00010B18
		private static float ProcessPowerValue(float value, float minValue, float maxValue)
		{
			value = Mathf.Clamp(value, minValue, maxValue);
			if (value > 1f)
			{
				value = MathTools.ValueInNewRange(value, 1f, maxValue, 1f, 0f);
			}
			else if (value < 1f)
			{
				value = MathTools.ValueInNewRange(value, 0f, 1f, maxValue, 1f);
			}
			return value;
		}

		// Token: 0x0400037F RID: 895
		private const float minSensitivityOtherAxes = 0.1f;

		// Token: 0x04000380 RID: 896
		private const float maxDeadzone = 0.8f;

		// Token: 0x04000381 RID: 897
		[SerializeField]
		private RectTransform rightContentContainer;

		// Token: 0x04000382 RID: 898
		[SerializeField]
		private RectTransform valueDisplayGroup;

		// Token: 0x04000383 RID: 899
		[SerializeField]
		private RectTransform calibratedValueMarker;

		// Token: 0x04000384 RID: 900
		[SerializeField]
		private RectTransform rawValueMarker;

		// Token: 0x04000385 RID: 901
		[SerializeField]
		private RectTransform calibratedZeroMarker;

		// Token: 0x04000386 RID: 902
		[SerializeField]
		private RectTransform deadzoneArea;

		// Token: 0x04000387 RID: 903
		[SerializeField]
		private Slider deadzoneSlider;

		// Token: 0x04000388 RID: 904
		[SerializeField]
		private Slider zeroSlider;

		// Token: 0x04000389 RID: 905
		[SerializeField]
		private Slider sensitivitySlider;

		// Token: 0x0400038A RID: 906
		[SerializeField]
		private Toggle invertToggle;

		// Token: 0x0400038B RID: 907
		[SerializeField]
		private RectTransform axisScrollAreaContent;

		// Token: 0x0400038C RID: 908
		[SerializeField]
		private Button doneButton;

		// Token: 0x0400038D RID: 909
		[SerializeField]
		private Button calibrateButton;

		// Token: 0x0400038E RID: 910
		[SerializeField]
		private TMP_Text doneButtonLabel;

		// Token: 0x0400038F RID: 911
		[SerializeField]
		private TMP_Text cancelButtonLabel;

		// Token: 0x04000390 RID: 912
		[SerializeField]
		private TMP_Text defaultButtonLabel;

		// Token: 0x04000391 RID: 913
		[SerializeField]
		private TMP_Text deadzoneSliderLabel;

		// Token: 0x04000392 RID: 914
		[SerializeField]
		private TMP_Text zeroSliderLabel;

		// Token: 0x04000393 RID: 915
		[SerializeField]
		private TMP_Text sensitivitySliderLabel;

		// Token: 0x04000394 RID: 916
		[SerializeField]
		private TMP_Text invertToggleLabel;

		// Token: 0x04000395 RID: 917
		[SerializeField]
		private TMP_Text calibrateButtonLabel;

		// Token: 0x04000396 RID: 918
		[SerializeField]
		private GameObject axisButtonPrefab;

		// Token: 0x04000397 RID: 919
		private Joystick joystick;

		// Token: 0x04000398 RID: 920
		private string origCalibrationData;

		// Token: 0x04000399 RID: 921
		private int selectedAxis = -1;

		// Token: 0x0400039A RID: 922
		private AxisCalibrationData origSelectedAxisCalibrationData;

		// Token: 0x0400039B RID: 923
		private float displayAreaWidth;

		// Token: 0x0400039C RID: 924
		private List<Button> axisButtons;

		// Token: 0x0400039D RID: 925
		private Dictionary<int, Action<int>> buttonCallbacks;

		// Token: 0x0400039E RID: 926
		private int playerId;

		// Token: 0x0400039F RID: 927
		private RewiredStandaloneInputModule rewiredStandaloneInputModule;

		// Token: 0x040003A0 RID: 928
		private int menuHorizActionId = -1;

		// Token: 0x040003A1 RID: 929
		private int menuVertActionId = -1;

		// Token: 0x040003A2 RID: 930
		private float minSensitivity;

		// Token: 0x02000090 RID: 144
		public enum ButtonIdentifier
		{
			// Token: 0x040003A4 RID: 932
			Done,
			// Token: 0x040003A5 RID: 933
			Cancel,
			// Token: 0x040003A6 RID: 934
			Default,
			// Token: 0x040003A7 RID: 935
			Calibrate
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos.GamepadTemplateUI
{
	// Token: 0x02000116 RID: 278
	public class GamepadTemplateUI : MonoBehaviour
	{
		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06000D25 RID: 3365 RVA: 0x00026AC6 File Offset: 0x00024CC6
		private Player player
		{
			get
			{
				return ReInput.players.GetPlayer(this.playerId);
			}
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x00026AD8 File Offset: 0x00024CD8
		private void Awake()
		{
			this._uiElementsArray = new GamepadTemplateUI.UIElement[]
			{
				new GamepadTemplateUI.UIElement(0, this.leftStickX),
				new GamepadTemplateUI.UIElement(1, this.leftStickY),
				new GamepadTemplateUI.UIElement(17, this.leftStickButton),
				new GamepadTemplateUI.UIElement(2, this.rightStickX),
				new GamepadTemplateUI.UIElement(3, this.rightStickY),
				new GamepadTemplateUI.UIElement(18, this.rightStickButton),
				new GamepadTemplateUI.UIElement(4, this.actionBottomRow1),
				new GamepadTemplateUI.UIElement(5, this.actionBottomRow2),
				new GamepadTemplateUI.UIElement(6, this.actionBottomRow3),
				new GamepadTemplateUI.UIElement(7, this.actionTopRow1),
				new GamepadTemplateUI.UIElement(8, this.actionTopRow2),
				new GamepadTemplateUI.UIElement(9, this.actionTopRow3),
				new GamepadTemplateUI.UIElement(14, this.center1),
				new GamepadTemplateUI.UIElement(15, this.center2),
				new GamepadTemplateUI.UIElement(16, this.center3),
				new GamepadTemplateUI.UIElement(19, this.dPadUp),
				new GamepadTemplateUI.UIElement(20, this.dPadRight),
				new GamepadTemplateUI.UIElement(21, this.dPadDown),
				new GamepadTemplateUI.UIElement(22, this.dPadLeft),
				new GamepadTemplateUI.UIElement(10, this.leftShoulder),
				new GamepadTemplateUI.UIElement(11, this.leftTrigger),
				new GamepadTemplateUI.UIElement(12, this.rightShoulder),
				new GamepadTemplateUI.UIElement(13, this.rightTrigger)
			};
			for (int i = 0; i < this._uiElementsArray.Length; i++)
			{
				this._uiElements.Add(this._uiElementsArray[i].id, this._uiElementsArray[i].element);
			}
			this._sticks = new GamepadTemplateUI.Stick[]
			{
				new GamepadTemplateUI.Stick(this.leftStick, 0, 1),
				new GamepadTemplateUI.Stick(this.rightStick, 2, 3)
			};
			ReInput.ControllerConnectedEvent += this.OnControllerConnected;
			ReInput.ControllerDisconnectedEvent += this.OnControllerDisconnected;
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x00026CED File Offset: 0x00024EED
		private void Start()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.DrawLabels();
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00026CFD File Offset: 0x00024EFD
		private void OnDestroy()
		{
			ReInput.ControllerConnectedEvent -= this.OnControllerConnected;
			ReInput.ControllerDisconnectedEvent -= this.OnControllerDisconnected;
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x00026D21 File Offset: 0x00024F21
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.DrawActiveElements();
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x00026D34 File Offset: 0x00024F34
		private void DrawActiveElements()
		{
			for (int i = 0; i < this._uiElementsArray.Length; i++)
			{
				this._uiElementsArray[i].element.Deactivate();
			}
			for (int j = 0; j < this._sticks.Length; j++)
			{
				this._sticks[j].Reset();
			}
			IList<InputAction> actions = ReInput.mapping.Actions;
			for (int k = 0; k < actions.Count; k++)
			{
				this.ActivateElements(this.player, actions[k].id);
			}
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x00026DBC File Offset: 0x00024FBC
		private void ActivateElements(Player player, int actionId)
		{
			float axis = player.GetAxis(actionId);
			if (axis == 0f)
			{
				return;
			}
			IList<InputActionSourceData> currentInputSources = player.GetCurrentInputSources(actionId);
			for (int i = 0; i < currentInputSources.Count; i++)
			{
				InputActionSourceData inputActionSourceData = currentInputSources[i];
				IGamepadTemplate template = inputActionSourceData.controller.GetTemplate<IGamepadTemplate>();
				if (template != null)
				{
					template.GetElementTargets(inputActionSourceData.actionElementMap, this._tempTargetList);
					for (int j = 0; j < this._tempTargetList.Count; j++)
					{
						ControllerTemplateElementTarget controllerTemplateElementTarget = this._tempTargetList[j];
						int id = controllerTemplateElementTarget.element.id;
						ControllerUIElement controllerUIElement = this._uiElements[id];
						if (controllerTemplateElementTarget.elementType == ControllerTemplateElementType.Axis)
						{
							controllerUIElement.Activate(axis);
						}
						else if (controllerTemplateElementTarget.elementType == ControllerTemplateElementType.Button && (player.GetButton(actionId) || player.GetNegativeButton(actionId)))
						{
							controllerUIElement.Activate(1f);
						}
						GamepadTemplateUI.Stick stick = this.GetStick(id);
						if (stick != null)
						{
							stick.SetAxisPosition(id, axis * 20f);
						}
					}
				}
			}
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x00026ED8 File Offset: 0x000250D8
		private void DrawLabels()
		{
			for (int i = 0; i < this._uiElementsArray.Length; i++)
			{
				this._uiElementsArray[i].element.ClearLabels();
			}
			IList<InputAction> actions = ReInput.mapping.Actions;
			for (int j = 0; j < actions.Count; j++)
			{
				this.DrawLabels(this.player, actions[j]);
			}
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x00026F3C File Offset: 0x0002513C
		private void DrawLabels(Player player, InputAction action)
		{
			Controller firstControllerWithTemplate = player.controllers.GetFirstControllerWithTemplate<IGamepadTemplate>();
			if (firstControllerWithTemplate == null)
			{
				return;
			}
			IGamepadTemplate template = firstControllerWithTemplate.GetTemplate<IGamepadTemplate>();
			ControllerMap map = player.controllers.maps.GetMap(firstControllerWithTemplate, "Default", "Default");
			if (map == null)
			{
				return;
			}
			for (int i = 0; i < this._uiElementsArray.Length; i++)
			{
				ControllerUIElement element = this._uiElementsArray[i].element;
				int id = this._uiElementsArray[i].id;
				IControllerTemplateElement element2 = template.GetElement(id);
				this.DrawLabel(element, action, map, template, element2);
			}
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x00026FC8 File Offset: 0x000251C8
		private void DrawLabel(ControllerUIElement uiElement, InputAction action, ControllerMap controllerMap, IControllerTemplate template, IControllerTemplateElement element)
		{
			if (element.source == null)
			{
				return;
			}
			if (element.source.type == ControllerTemplateElementSourceType.Axis)
			{
				IControllerTemplateAxisSource controllerTemplateAxisSource = element.source as IControllerTemplateAxisSource;
				if (controllerTemplateAxisSource.splitAxis)
				{
					ActionElementMap actionElementMap = controllerMap.GetFirstElementMapWithElementTarget(controllerTemplateAxisSource.positiveTarget, action.id, true);
					if (actionElementMap != null)
					{
						uiElement.SetLabel(actionElementMap.actionDescriptiveName, AxisRange.Positive);
					}
					actionElementMap = controllerMap.GetFirstElementMapWithElementTarget(controllerTemplateAxisSource.negativeTarget, action.id, true);
					if (actionElementMap != null)
					{
						uiElement.SetLabel(actionElementMap.actionDescriptiveName, AxisRange.Negative);
						return;
					}
				}
				else
				{
					ActionElementMap actionElementMap = controllerMap.GetFirstElementMapWithElementTarget(controllerTemplateAxisSource.fullTarget, action.id, true);
					if (actionElementMap != null)
					{
						uiElement.SetLabel(actionElementMap.actionDescriptiveName, AxisRange.Full);
						return;
					}
					ControllerElementTarget controllerElementTarget = new ControllerElementTarget(controllerTemplateAxisSource.fullTarget)
					{
						axisRange = AxisRange.Positive
					};
					actionElementMap = controllerMap.GetFirstElementMapWithElementTarget(controllerElementTarget, action.id, true);
					if (actionElementMap != null)
					{
						uiElement.SetLabel(actionElementMap.actionDescriptiveName, AxisRange.Positive);
					}
					controllerElementTarget = new ControllerElementTarget(controllerTemplateAxisSource.fullTarget)
					{
						axisRange = AxisRange.Negative
					};
					actionElementMap = controllerMap.GetFirstElementMapWithElementTarget(controllerElementTarget, action.id, true);
					if (actionElementMap != null)
					{
						uiElement.SetLabel(actionElementMap.actionDescriptiveName, AxisRange.Negative);
						return;
					}
				}
			}
			else if (element.source.type == ControllerTemplateElementSourceType.Button)
			{
				IControllerTemplateButtonSource controllerTemplateButtonSource = element.source as IControllerTemplateButtonSource;
				ActionElementMap actionElementMap = controllerMap.GetFirstElementMapWithElementTarget(controllerTemplateButtonSource.target, action.id, true);
				if (actionElementMap != null)
				{
					uiElement.SetLabel(actionElementMap.actionDescriptiveName, AxisRange.Full);
				}
			}
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x00027120 File Offset: 0x00025320
		private GamepadTemplateUI.Stick GetStick(int elementId)
		{
			for (int i = 0; i < this._sticks.Length; i++)
			{
				if (this._sticks[i].ContainsElement(elementId))
				{
					return this._sticks[i];
				}
			}
			return null;
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0002715A File Offset: 0x0002535A
		private void OnControllerConnected(ControllerStatusChangedEventArgs args)
		{
			this.DrawLabels();
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0002715A File Offset: 0x0002535A
		private void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
		{
			this.DrawLabels();
		}

		// Token: 0x040006E8 RID: 1768
		private const float stickRadius = 20f;

		// Token: 0x040006E9 RID: 1769
		public int playerId;

		// Token: 0x040006EA RID: 1770
		[SerializeField]
		private RectTransform leftStick;

		// Token: 0x040006EB RID: 1771
		[SerializeField]
		private RectTransform rightStick;

		// Token: 0x040006EC RID: 1772
		[SerializeField]
		private ControllerUIElement leftStickX;

		// Token: 0x040006ED RID: 1773
		[SerializeField]
		private ControllerUIElement leftStickY;

		// Token: 0x040006EE RID: 1774
		[SerializeField]
		private ControllerUIElement leftStickButton;

		// Token: 0x040006EF RID: 1775
		[SerializeField]
		private ControllerUIElement rightStickX;

		// Token: 0x040006F0 RID: 1776
		[SerializeField]
		private ControllerUIElement rightStickY;

		// Token: 0x040006F1 RID: 1777
		[SerializeField]
		private ControllerUIElement rightStickButton;

		// Token: 0x040006F2 RID: 1778
		[SerializeField]
		private ControllerUIElement actionBottomRow1;

		// Token: 0x040006F3 RID: 1779
		[SerializeField]
		private ControllerUIElement actionBottomRow2;

		// Token: 0x040006F4 RID: 1780
		[SerializeField]
		private ControllerUIElement actionBottomRow3;

		// Token: 0x040006F5 RID: 1781
		[SerializeField]
		private ControllerUIElement actionTopRow1;

		// Token: 0x040006F6 RID: 1782
		[SerializeField]
		private ControllerUIElement actionTopRow2;

		// Token: 0x040006F7 RID: 1783
		[SerializeField]
		private ControllerUIElement actionTopRow3;

		// Token: 0x040006F8 RID: 1784
		[SerializeField]
		private ControllerUIElement leftShoulder;

		// Token: 0x040006F9 RID: 1785
		[SerializeField]
		private ControllerUIElement leftTrigger;

		// Token: 0x040006FA RID: 1786
		[SerializeField]
		private ControllerUIElement rightShoulder;

		// Token: 0x040006FB RID: 1787
		[SerializeField]
		private ControllerUIElement rightTrigger;

		// Token: 0x040006FC RID: 1788
		[SerializeField]
		private ControllerUIElement center1;

		// Token: 0x040006FD RID: 1789
		[SerializeField]
		private ControllerUIElement center2;

		// Token: 0x040006FE RID: 1790
		[SerializeField]
		private ControllerUIElement center3;

		// Token: 0x040006FF RID: 1791
		[SerializeField]
		private ControllerUIElement dPadUp;

		// Token: 0x04000700 RID: 1792
		[SerializeField]
		private ControllerUIElement dPadRight;

		// Token: 0x04000701 RID: 1793
		[SerializeField]
		private ControllerUIElement dPadDown;

		// Token: 0x04000702 RID: 1794
		[SerializeField]
		private ControllerUIElement dPadLeft;

		// Token: 0x04000703 RID: 1795
		private GamepadTemplateUI.UIElement[] _uiElementsArray;

		// Token: 0x04000704 RID: 1796
		private Dictionary<int, ControllerUIElement> _uiElements = new Dictionary<int, ControllerUIElement>();

		// Token: 0x04000705 RID: 1797
		private IList<ControllerTemplateElementTarget> _tempTargetList = new List<ControllerTemplateElementTarget>(2);

		// Token: 0x04000706 RID: 1798
		private GamepadTemplateUI.Stick[] _sticks;

		// Token: 0x02000117 RID: 279
		private class Stick
		{
			// Token: 0x170004C2 RID: 1218
			// (get) Token: 0x06000D33 RID: 3379 RVA: 0x00027181 File Offset: 0x00025381
			// (set) Token: 0x06000D34 RID: 3380 RVA: 0x000271AD File Offset: 0x000253AD
			public Vector2 position
			{
				get
				{
					if (!(this._transform != null))
					{
						return Vector2.zero;
					}
					return this._transform.anchoredPosition - this._origPosition;
				}
				set
				{
					if (this._transform == null)
					{
						return;
					}
					this._transform.anchoredPosition = this._origPosition + value;
				}
			}

			// Token: 0x06000D35 RID: 3381 RVA: 0x000271D8 File Offset: 0x000253D8
			public Stick(RectTransform transform, int xAxisElementId, int yAxisElementId)
			{
				if (transform == null)
				{
					return;
				}
				this._transform = transform;
				this._origPosition = this._transform.anchoredPosition;
				this._xAxisElementId = xAxisElementId;
				this._yAxisElementId = yAxisElementId;
			}

			// Token: 0x06000D36 RID: 3382 RVA: 0x00027229 File Offset: 0x00025429
			public void Reset()
			{
				if (this._transform == null)
				{
					return;
				}
				this._transform.anchoredPosition = this._origPosition;
			}

			// Token: 0x06000D37 RID: 3383 RVA: 0x0002724B File Offset: 0x0002544B
			public bool ContainsElement(int elementId)
			{
				return !(this._transform == null) && (elementId == this._xAxisElementId || elementId == this._yAxisElementId);
			}

			// Token: 0x06000D38 RID: 3384 RVA: 0x00027274 File Offset: 0x00025474
			public void SetAxisPosition(int elementId, float value)
			{
				if (this._transform == null)
				{
					return;
				}
				Vector2 position = this.position;
				if (elementId == this._xAxisElementId)
				{
					position.x = value;
				}
				else if (elementId == this._yAxisElementId)
				{
					position.y = value;
				}
				this.position = position;
			}

			// Token: 0x04000707 RID: 1799
			private RectTransform _transform;

			// Token: 0x04000708 RID: 1800
			private Vector2 _origPosition;

			// Token: 0x04000709 RID: 1801
			private int _xAxisElementId = -1;

			// Token: 0x0400070A RID: 1802
			private int _yAxisElementId = -1;
		}

		// Token: 0x02000118 RID: 280
		private class UIElement
		{
			// Token: 0x06000D39 RID: 3385 RVA: 0x000272C2 File Offset: 0x000254C2
			public UIElement(int id, ControllerUIElement element)
			{
				this.id = id;
				this.element = element;
			}

			// Token: 0x0400070B RID: 1803
			public int id;

			// Token: 0x0400070C RID: 1804
			public ControllerUIElement element;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using Rewired.UI;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rewired.Integration.UnityUI
{
	// Token: 0x0200004F RID: 79
	public abstract class RewiredPointerInputModule : BaseInputModule
	{
		// Token: 0x1700026A RID: 618
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x0000A214 File Offset: 0x00008414
		private RewiredPointerInputModule.UnityInputSource defaultInputSource
		{
			get
			{
				if (this.__m_DefaultInputSource == null)
				{
					return this.__m_DefaultInputSource = new RewiredPointerInputModule.UnityInputSource();
				}
				return this.__m_DefaultInputSource;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x0000A23E File Offset: 0x0000843E
		private IMouseInputSource defaultMouseInputSource
		{
			get
			{
				return this.defaultInputSource;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x0000A23E File Offset: 0x0000843E
		protected ITouchInputSource defaultTouchInputSource
		{
			get
			{
				return this.defaultInputSource;
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0000A246 File Offset: 0x00008446
		protected bool IsDefaultMouse(IMouseInputSource mouse)
		{
			return this.defaultMouseInputSource == mouse;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0000A254 File Offset: 0x00008454
		public IMouseInputSource GetMouseInputSource(int playerId, int mouseIndex)
		{
			if (mouseIndex < 0)
			{
				throw new ArgumentOutOfRangeException("mouseIndex");
			}
			if (this.m_MouseInputSourcesList.Count == 0 && this.IsDefaultPlayer(playerId))
			{
				return this.defaultMouseInputSource;
			}
			int count = this.m_MouseInputSourcesList.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				IMouseInputSource mouseInputSource = this.m_MouseInputSourcesList[i];
				if (!UnityTools.IsNullOrDestroyed<IMouseInputSource>(mouseInputSource) && mouseInputSource.playerId == playerId)
				{
					if (mouseIndex == num)
					{
						return mouseInputSource;
					}
					num++;
				}
			}
			return null;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0000A2D0 File Offset: 0x000084D0
		public void RemoveMouseInputSource(IMouseInputSource source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			this.m_MouseInputSourcesList.Remove(source);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0000A2ED File Offset: 0x000084ED
		public void AddMouseInputSource(IMouseInputSource source)
		{
			if (UnityTools.IsNullOrDestroyed<IMouseInputSource>(source))
			{
				throw new ArgumentNullException("source");
			}
			this.m_MouseInputSourcesList.Add(source);
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0000A310 File Offset: 0x00008510
		public int GetMouseInputSourceCount(int playerId)
		{
			if (this.m_MouseInputSourcesList.Count == 0 && this.IsDefaultPlayer(playerId))
			{
				return 1;
			}
			int count = this.m_MouseInputSourcesList.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				IMouseInputSource mouseInputSource = this.m_MouseInputSourcesList[i];
				if (!UnityTools.IsNullOrDestroyed<IMouseInputSource>(mouseInputSource) && mouseInputSource.playerId == playerId)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0000A372 File Offset: 0x00008572
		public ITouchInputSource GetTouchInputSource(int playerId, int sourceIndex)
		{
			if (!UnityTools.IsNullOrDestroyed<ITouchInputSource>(this.m_UserDefaultTouchInputSource))
			{
				return this.m_UserDefaultTouchInputSource;
			}
			return this.defaultTouchInputSource;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0000A38E File Offset: 0x0000858E
		public void RemoveTouchInputSource(ITouchInputSource source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (this.m_UserDefaultTouchInputSource == source)
			{
				this.m_UserDefaultTouchInputSource = null;
			}
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0000A3AE File Offset: 0x000085AE
		public void AddTouchInputSource(ITouchInputSource source)
		{
			if (UnityTools.IsNullOrDestroyed<ITouchInputSource>(source))
			{
				throw new ArgumentNullException("source");
			}
			this.m_UserDefaultTouchInputSource = source;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000A3CA File Offset: 0x000085CA
		public int GetTouchInputSourceCount(int playerId)
		{
			if (!this.IsDefaultPlayer(playerId))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0000A3D8 File Offset: 0x000085D8
		protected void ClearMouseInputSources()
		{
			this.m_MouseInputSourcesList.Clear();
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x0000A3E8 File Offset: 0x000085E8
		protected virtual bool isMouseSupported
		{
			get
			{
				int count = this.m_MouseInputSourcesList.Count;
				if (count == 0)
				{
					return this.defaultMouseInputSource.enabled;
				}
				for (int i = 0; i < count; i++)
				{
					if (this.m_MouseInputSourcesList[i].enabled)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060004AB RID: 1195
		protected abstract bool IsDefaultPlayer(int playerId);

		// Token: 0x060004AC RID: 1196 RVA: 0x0000A434 File Offset: 0x00008634
		protected bool GetPointerData(int playerId, int pointerIndex, int pointerTypeId, out PlayerPointerEventData data, bool create, PointerEventType pointerEventType)
		{
			Dictionary<int, PlayerPointerEventData>[] array;
			if (!this.m_PlayerPointerData.TryGetValue(playerId, out array))
			{
				array = new Dictionary<int, PlayerPointerEventData>[pointerIndex + 1];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new Dictionary<int, PlayerPointerEventData>();
				}
				this.m_PlayerPointerData.Add(playerId, array);
			}
			if (pointerIndex >= array.Length)
			{
				Dictionary<int, PlayerPointerEventData>[] array2 = new Dictionary<int, PlayerPointerEventData>[pointerIndex + 1];
				for (int j = 0; j < array.Length; j++)
				{
					array2[j] = array[j];
				}
				array2[pointerIndex] = new Dictionary<int, PlayerPointerEventData>();
				array = array2;
				this.m_PlayerPointerData[playerId] = array;
			}
			Dictionary<int, PlayerPointerEventData> dictionary = array[pointerIndex];
			if (dictionary.TryGetValue(pointerTypeId, out data))
			{
				data.mouseSource = ((pointerEventType == PointerEventType.Mouse) ? this.GetMouseInputSource(playerId, pointerIndex) : null);
				data.touchSource = ((pointerEventType == PointerEventType.Touch) ? this.GetTouchInputSource(playerId, pointerIndex) : null);
				return false;
			}
			if (!create)
			{
				return false;
			}
			data = this.CreatePointerEventData(playerId, pointerIndex, pointerTypeId, pointerEventType);
			dictionary.Add(pointerTypeId, data);
			return true;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0000A51C File Offset: 0x0000871C
		private PlayerPointerEventData CreatePointerEventData(int playerId, int pointerIndex, int pointerTypeId, PointerEventType pointerEventType)
		{
			PlayerPointerEventData playerPointerEventData = new PlayerPointerEventData(base.eventSystem)
			{
				playerId = playerId,
				inputSourceIndex = pointerIndex,
				pointerId = pointerTypeId,
				sourceType = pointerEventType
			};
			if (pointerEventType == PointerEventType.Mouse)
			{
				playerPointerEventData.mouseSource = this.GetMouseInputSource(playerId, pointerIndex);
			}
			else if (pointerEventType == PointerEventType.Touch)
			{
				playerPointerEventData.touchSource = this.GetTouchInputSource(playerId, pointerIndex);
			}
			if (pointerTypeId == -1)
			{
				playerPointerEventData.buttonIndex = 0;
			}
			else if (pointerTypeId == -2)
			{
				playerPointerEventData.buttonIndex = 1;
			}
			else if (pointerTypeId == -3)
			{
				playerPointerEventData.buttonIndex = 2;
			}
			else if (pointerTypeId >= -2147483520 && pointerTypeId <= -2147483392)
			{
				playerPointerEventData.buttonIndex = pointerTypeId - -2147483520;
			}
			return playerPointerEventData;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0000A5C0 File Offset: 0x000087C0
		protected void RemovePointerData(PlayerPointerEventData data)
		{
			Dictionary<int, PlayerPointerEventData>[] array;
			if (this.m_PlayerPointerData.TryGetValue(data.playerId, out array) && data.inputSourceIndex < array.Length)
			{
				array[data.inputSourceIndex].Remove(data.pointerId);
			}
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0000A604 File Offset: 0x00008804
		protected PlayerPointerEventData GetTouchPointerEventData(int playerId, int touchDeviceIndex, Touch input, out bool pressed, out bool released)
		{
			PlayerPointerEventData playerPointerEventData;
			bool pointerData = this.GetPointerData(playerId, touchDeviceIndex, input.fingerId, out playerPointerEventData, true, PointerEventType.Touch);
			playerPointerEventData.Reset();
			pressed = pointerData || input.phase == TouchPhase.Began;
			released = input.phase == TouchPhase.Canceled || input.phase == TouchPhase.Ended;
			if (pointerData)
			{
				playerPointerEventData.position = input.position;
			}
			if (pressed)
			{
				playerPointerEventData.delta = Vector2.zero;
			}
			else
			{
				playerPointerEventData.delta = input.position - playerPointerEventData.position;
			}
			playerPointerEventData.position = input.position;
			playerPointerEventData.button = PointerEventData.InputButton.Left;
			base.eventSystem.RaycastAll(playerPointerEventData, this.m_RaycastResultCache);
			RaycastResult raycastResult = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			playerPointerEventData.pointerCurrentRaycast = raycastResult;
			this.m_RaycastResultCache.Clear();
			return playerPointerEventData;
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0000A6D8 File Offset: 0x000088D8
		protected virtual RewiredPointerInputModule.MouseState GetMousePointerEventData(int playerId, int mouseIndex)
		{
			IMouseInputSource mouseInputSource = this.GetMouseInputSource(playerId, mouseIndex);
			if (mouseInputSource == null)
			{
				return null;
			}
			PlayerPointerEventData playerPointerEventData;
			bool pointerData = this.GetPointerData(playerId, mouseIndex, -1, out playerPointerEventData, true, PointerEventType.Mouse);
			playerPointerEventData.Reset();
			if (pointerData)
			{
				playerPointerEventData.position = mouseInputSource.screenPosition;
			}
			Vector2 screenPosition = mouseInputSource.screenPosition;
			if (mouseInputSource.locked || !mouseInputSource.enabled)
			{
				playerPointerEventData.position = new Vector2(-1f, -1f);
				playerPointerEventData.delta = Vector2.zero;
			}
			else
			{
				playerPointerEventData.delta = screenPosition - playerPointerEventData.position;
				playerPointerEventData.position = screenPosition;
			}
			playerPointerEventData.scrollDelta = mouseInputSource.wheelDelta;
			playerPointerEventData.button = PointerEventData.InputButton.Left;
			base.eventSystem.RaycastAll(playerPointerEventData, this.m_RaycastResultCache);
			RaycastResult raycastResult = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			playerPointerEventData.pointerCurrentRaycast = raycastResult;
			this.m_RaycastResultCache.Clear();
			PlayerPointerEventData playerPointerEventData2;
			this.GetPointerData(playerId, mouseIndex, -2, out playerPointerEventData2, true, PointerEventType.Mouse);
			this.CopyFromTo(playerPointerEventData, playerPointerEventData2);
			playerPointerEventData2.button = PointerEventData.InputButton.Right;
			PlayerPointerEventData playerPointerEventData3;
			this.GetPointerData(playerId, mouseIndex, -3, out playerPointerEventData3, true, PointerEventType.Mouse);
			this.CopyFromTo(playerPointerEventData, playerPointerEventData3);
			playerPointerEventData3.button = PointerEventData.InputButton.Middle;
			for (int i = 3; i < mouseInputSource.buttonCount; i++)
			{
				PlayerPointerEventData playerPointerEventData4;
				this.GetPointerData(playerId, mouseIndex, -2147483520 + i, out playerPointerEventData4, true, PointerEventType.Mouse);
				this.CopyFromTo(playerPointerEventData, playerPointerEventData4);
				playerPointerEventData4.button = (PointerEventData.InputButton)(-1);
			}
			this.m_MouseState.SetButtonState(0, this.StateForMouseButton(playerId, mouseIndex, 0), playerPointerEventData);
			this.m_MouseState.SetButtonState(1, this.StateForMouseButton(playerId, mouseIndex, 1), playerPointerEventData2);
			this.m_MouseState.SetButtonState(2, this.StateForMouseButton(playerId, mouseIndex, 2), playerPointerEventData3);
			for (int j = 3; j < mouseInputSource.buttonCount; j++)
			{
				PlayerPointerEventData playerPointerEventData5;
				this.GetPointerData(playerId, mouseIndex, -2147483520 + j, out playerPointerEventData5, false, PointerEventType.Mouse);
				this.m_MouseState.SetButtonState(j, this.StateForMouseButton(playerId, mouseIndex, j), playerPointerEventData5);
			}
			return this.m_MouseState;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0000A8B4 File Offset: 0x00008AB4
		protected PlayerPointerEventData GetLastPointerEventData(int playerId, int pointerIndex, int pointerTypeId, bool ignorePointerTypeId, PointerEventType pointerEventType)
		{
			if (!ignorePointerTypeId)
			{
				PlayerPointerEventData playerPointerEventData;
				this.GetPointerData(playerId, pointerIndex, pointerTypeId, out playerPointerEventData, false, pointerEventType);
				return playerPointerEventData;
			}
			Dictionary<int, PlayerPointerEventData>[] array;
			if (!this.m_PlayerPointerData.TryGetValue(playerId, out array))
			{
				return null;
			}
			if (pointerIndex >= array.Length)
			{
				return null;
			}
			using (Dictionary<int, PlayerPointerEventData>.Enumerator enumerator = array[pointerIndex].GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					KeyValuePair<int, PlayerPointerEventData> keyValuePair = enumerator.Current;
					return keyValuePair.Value;
				}
			}
			return null;
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0000A93C File Offset: 0x00008B3C
		private static bool ShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
		{
			return !useDragThreshold || (pressPos - currentPos).sqrMagnitude >= threshold * threshold;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0000A968 File Offset: 0x00008B68
		protected virtual void ProcessMove(PlayerPointerEventData pointerEvent)
		{
			GameObject gameObject;
			if (pointerEvent.sourceType == PointerEventType.Mouse)
			{
				IMouseInputSource mouseInputSource = this.GetMouseInputSource(pointerEvent.playerId, pointerEvent.inputSourceIndex);
				if (mouseInputSource != null)
				{
					gameObject = ((!mouseInputSource.enabled || mouseInputSource.locked) ? null : pointerEvent.pointerCurrentRaycast.gameObject);
				}
				else
				{
					gameObject = null;
				}
			}
			else
			{
				if (pointerEvent.sourceType != PointerEventType.Touch)
				{
					throw new NotImplementedException();
				}
				gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
			}
			base.HandlePointerExitAndEnter(pointerEvent, gameObject);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0000A9E4 File Offset: 0x00008BE4
		protected virtual void ProcessDrag(PlayerPointerEventData pointerEvent)
		{
			if (!pointerEvent.IsPointerMoving() || pointerEvent.pointerDrag == null)
			{
				return;
			}
			if (pointerEvent.sourceType == PointerEventType.Mouse)
			{
				IMouseInputSource mouseInputSource = this.GetMouseInputSource(pointerEvent.playerId, pointerEvent.inputSourceIndex);
				if (mouseInputSource == null || mouseInputSource.locked || !mouseInputSource.enabled)
				{
					return;
				}
			}
			if (!pointerEvent.dragging && RewiredPointerInputModule.ShouldStartDrag(pointerEvent.pressPosition, pointerEvent.position, (float)base.eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold))
			{
				ExecuteEvents.Execute<IBeginDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);
				pointerEvent.dragging = true;
			}
			if (pointerEvent.dragging)
			{
				if (pointerEvent.pointerPress != pointerEvent.pointerDrag)
				{
					ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
					pointerEvent.eligibleForClick = false;
					pointerEvent.pointerPress = null;
					pointerEvent.rawPointerPress = null;
				}
				ExecuteEvents.Execute<IDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.dragHandler);
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0000AAD4 File Offset: 0x00008CD4
		public override bool IsPointerOverGameObject(int pointerTypeId)
		{
			foreach (KeyValuePair<int, Dictionary<int, PlayerPointerEventData>[]> keyValuePair in this.m_PlayerPointerData)
			{
				Dictionary<int, PlayerPointerEventData>[] value = keyValuePair.Value;
				for (int i = 0; i < value.Length; i++)
				{
					PlayerPointerEventData playerPointerEventData;
					if (value[i].TryGetValue(pointerTypeId, out playerPointerEventData) && playerPointerEventData.pointerEnter != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0000AB5C File Offset: 0x00008D5C
		protected void ClearSelection()
		{
			BaseEventData baseEventData = this.GetBaseEventData();
			foreach (KeyValuePair<int, Dictionary<int, PlayerPointerEventData>[]> keyValuePair in this.m_PlayerPointerData)
			{
				Dictionary<int, PlayerPointerEventData>[] value = keyValuePair.Value;
				for (int i = 0; i < value.Length; i++)
				{
					foreach (KeyValuePair<int, PlayerPointerEventData> keyValuePair2 in value[i])
					{
						base.HandlePointerExitAndEnter(keyValuePair2.Value, null);
					}
					value[i].Clear();
				}
			}
			base.eventSystem.SetSelectedGameObject(null, baseEventData);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0000AC28 File Offset: 0x00008E28
		public override string ToString()
		{
			string text = "<b>Pointer Input Module of type: </b>";
			Type type = base.GetType();
			StringBuilder stringBuilder = new StringBuilder(text + ((type != null) ? type.ToString() : null));
			stringBuilder.AppendLine();
			foreach (KeyValuePair<int, Dictionary<int, PlayerPointerEventData>[]> keyValuePair in this.m_PlayerPointerData)
			{
				stringBuilder.AppendLine("<B>Player Id:</b> " + keyValuePair.Key.ToString());
				Dictionary<int, PlayerPointerEventData>[] value = keyValuePair.Value;
				for (int i = 0; i < value.Length; i++)
				{
					stringBuilder.AppendLine("<B>Pointer Index:</b> " + i.ToString());
					foreach (KeyValuePair<int, PlayerPointerEventData> keyValuePair2 in value[i])
					{
						stringBuilder.AppendLine("<B>Button Id:</b> " + keyValuePair2.Key.ToString());
						stringBuilder.AppendLine(keyValuePair2.Value.ToString());
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0000AD74 File Offset: 0x00008F74
		protected void DeselectIfSelectionChanged(GameObject currentOverGo, BaseEventData pointerEvent)
		{
			if (ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo) != base.eventSystem.currentSelectedGameObject)
			{
				base.eventSystem.SetSelectedGameObject(null, pointerEvent);
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0000AD9B File Offset: 0x00008F9B
		protected void CopyFromTo(PointerEventData from, PointerEventData to)
		{
			to.position = from.position;
			to.delta = from.delta;
			to.scrollDelta = from.scrollDelta;
			to.pointerCurrentRaycast = from.pointerCurrentRaycast;
			to.pointerEnter = from.pointerEnter;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0000ADDC File Offset: 0x00008FDC
		protected PointerEventData.FramePressState StateForMouseButton(int playerId, int mouseIndex, int buttonId)
		{
			IMouseInputSource mouseInputSource = this.GetMouseInputSource(playerId, mouseIndex);
			if (mouseInputSource == null)
			{
				return PointerEventData.FramePressState.NotChanged;
			}
			bool buttonDown = mouseInputSource.GetButtonDown(buttonId);
			bool buttonUp = mouseInputSource.GetButtonUp(buttonId);
			if (buttonDown && buttonUp)
			{
				return PointerEventData.FramePressState.PressedAndReleased;
			}
			if (buttonDown)
			{
				return PointerEventData.FramePressState.Pressed;
			}
			if (buttonUp)
			{
				return PointerEventData.FramePressState.Released;
			}
			return PointerEventData.FramePressState.NotChanged;
		}

		// Token: 0x040002AA RID: 682
		public const int kMouseLeftId = -1;

		// Token: 0x040002AB RID: 683
		public const int kMouseRightId = -2;

		// Token: 0x040002AC RID: 684
		public const int kMouseMiddleId = -3;

		// Token: 0x040002AD RID: 685
		public const int kFakeTouchesId = -4;

		// Token: 0x040002AE RID: 686
		private const int customButtonsStartingId = -2147483520;

		// Token: 0x040002AF RID: 687
		private const int customButtonsMaxCount = 128;

		// Token: 0x040002B0 RID: 688
		private const int customButtonsLastId = -2147483392;

		// Token: 0x040002B1 RID: 689
		private readonly List<IMouseInputSource> m_MouseInputSourcesList = new List<IMouseInputSource>();

		// Token: 0x040002B2 RID: 690
		private Dictionary<int, Dictionary<int, PlayerPointerEventData>[]> m_PlayerPointerData = new Dictionary<int, Dictionary<int, PlayerPointerEventData>[]>();

		// Token: 0x040002B3 RID: 691
		private ITouchInputSource m_UserDefaultTouchInputSource;

		// Token: 0x040002B4 RID: 692
		private RewiredPointerInputModule.UnityInputSource __m_DefaultInputSource;

		// Token: 0x040002B5 RID: 693
		private readonly RewiredPointerInputModule.MouseState m_MouseState = new RewiredPointerInputModule.MouseState();

		// Token: 0x02000050 RID: 80
		protected class MouseState
		{
			// Token: 0x060004BC RID: 1212 RVA: 0x0000AE44 File Offset: 0x00009044
			public bool AnyPressesThisFrame()
			{
				for (int i = 0; i < this.m_TrackedButtons.Count; i++)
				{
					if (this.m_TrackedButtons[i].eventData.PressedThisFrame())
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060004BD RID: 1213 RVA: 0x0000AE84 File Offset: 0x00009084
			public bool AnyReleasesThisFrame()
			{
				for (int i = 0; i < this.m_TrackedButtons.Count; i++)
				{
					if (this.m_TrackedButtons[i].eventData.ReleasedThisFrame())
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060004BE RID: 1214 RVA: 0x0000AEC4 File Offset: 0x000090C4
			public RewiredPointerInputModule.ButtonState GetButtonState(int button)
			{
				RewiredPointerInputModule.ButtonState buttonState = null;
				for (int i = 0; i < this.m_TrackedButtons.Count; i++)
				{
					if (this.m_TrackedButtons[i].button == button)
					{
						buttonState = this.m_TrackedButtons[i];
						break;
					}
				}
				if (buttonState == null)
				{
					buttonState = new RewiredPointerInputModule.ButtonState
					{
						button = button,
						eventData = new RewiredPointerInputModule.MouseButtonEventData()
					};
					this.m_TrackedButtons.Add(buttonState);
				}
				return buttonState;
			}

			// Token: 0x060004BF RID: 1215 RVA: 0x0000AF34 File Offset: 0x00009134
			public void SetButtonState(int button, PointerEventData.FramePressState stateForMouseButton, PlayerPointerEventData data)
			{
				RewiredPointerInputModule.ButtonState buttonState = this.GetButtonState(button);
				buttonState.eventData.buttonState = stateForMouseButton;
				buttonState.eventData.buttonData = data;
			}

			// Token: 0x040002B6 RID: 694
			private List<RewiredPointerInputModule.ButtonState> m_TrackedButtons = new List<RewiredPointerInputModule.ButtonState>();
		}

		// Token: 0x02000051 RID: 81
		public class MouseButtonEventData
		{
			// Token: 0x060004C1 RID: 1217 RVA: 0x0000AF67 File Offset: 0x00009167
			public bool PressedThisFrame()
			{
				return this.buttonState == PointerEventData.FramePressState.Pressed || this.buttonState == PointerEventData.FramePressState.PressedAndReleased;
			}

			// Token: 0x060004C2 RID: 1218 RVA: 0x0000AF7C File Offset: 0x0000917C
			public bool ReleasedThisFrame()
			{
				return this.buttonState == PointerEventData.FramePressState.Released || this.buttonState == PointerEventData.FramePressState.PressedAndReleased;
			}

			// Token: 0x040002B7 RID: 695
			public PointerEventData.FramePressState buttonState;

			// Token: 0x040002B8 RID: 696
			public PlayerPointerEventData buttonData;
		}

		// Token: 0x02000052 RID: 82
		protected class ButtonState
		{
			// Token: 0x1700026E RID: 622
			// (get) Token: 0x060004C4 RID: 1220 RVA: 0x0000AF92 File Offset: 0x00009192
			// (set) Token: 0x060004C5 RID: 1221 RVA: 0x0000AF9A File Offset: 0x0000919A
			public RewiredPointerInputModule.MouseButtonEventData eventData
			{
				get
				{
					return this.m_EventData;
				}
				set
				{
					this.m_EventData = value;
				}
			}

			// Token: 0x1700026F RID: 623
			// (get) Token: 0x060004C6 RID: 1222 RVA: 0x0000AFA3 File Offset: 0x000091A3
			// (set) Token: 0x060004C7 RID: 1223 RVA: 0x0000AFAB File Offset: 0x000091AB
			public int button
			{
				get
				{
					return this.m_Button;
				}
				set
				{
					this.m_Button = value;
				}
			}

			// Token: 0x040002B9 RID: 697
			private int m_Button;

			// Token: 0x040002BA RID: 698
			private RewiredPointerInputModule.MouseButtonEventData m_EventData;
		}

		// Token: 0x02000053 RID: 83
		private sealed class UnityInputSource : IMouseInputSource, ITouchInputSource
		{
			// Token: 0x17000270 RID: 624
			// (get) Token: 0x060004C9 RID: 1225 RVA: 0x0000AFB4 File Offset: 0x000091B4
			int IMouseInputSource.playerId
			{
				get
				{
					this.TryUpdate();
					return 0;
				}
			}

			// Token: 0x17000271 RID: 625
			// (get) Token: 0x060004CA RID: 1226 RVA: 0x0000AFB4 File Offset: 0x000091B4
			int ITouchInputSource.playerId
			{
				get
				{
					this.TryUpdate();
					return 0;
				}
			}

			// Token: 0x17000272 RID: 626
			// (get) Token: 0x060004CB RID: 1227 RVA: 0x0000AFBD File Offset: 0x000091BD
			bool IMouseInputSource.enabled
			{
				get
				{
					this.TryUpdate();
					return Input.mousePresent;
				}
			}

			// Token: 0x17000273 RID: 627
			// (get) Token: 0x060004CC RID: 1228 RVA: 0x0000AFCA File Offset: 0x000091CA
			bool IMouseInputSource.locked
			{
				get
				{
					this.TryUpdate();
					return Cursor.lockState == CursorLockMode.Locked;
				}
			}

			// Token: 0x17000274 RID: 628
			// (get) Token: 0x060004CD RID: 1229 RVA: 0x0000AFDA File Offset: 0x000091DA
			int IMouseInputSource.buttonCount
			{
				get
				{
					this.TryUpdate();
					return 3;
				}
			}

			// Token: 0x060004CE RID: 1230 RVA: 0x0000AFE3 File Offset: 0x000091E3
			bool IMouseInputSource.GetButtonDown(int button)
			{
				this.TryUpdate();
				return Input.GetMouseButtonDown(button);
			}

			// Token: 0x060004CF RID: 1231 RVA: 0x0000AFF1 File Offset: 0x000091F1
			bool IMouseInputSource.GetButtonUp(int button)
			{
				this.TryUpdate();
				return Input.GetMouseButtonUp(button);
			}

			// Token: 0x060004D0 RID: 1232 RVA: 0x0000AFFF File Offset: 0x000091FF
			bool IMouseInputSource.GetButton(int button)
			{
				this.TryUpdate();
				return Input.GetMouseButton(button);
			}

			// Token: 0x17000275 RID: 629
			// (get) Token: 0x060004D1 RID: 1233 RVA: 0x0000B00D File Offset: 0x0000920D
			Vector2 IMouseInputSource.screenPosition
			{
				get
				{
					this.TryUpdate();
					return Input.mousePosition;
				}
			}

			// Token: 0x17000276 RID: 630
			// (get) Token: 0x060004D2 RID: 1234 RVA: 0x0000B01F File Offset: 0x0000921F
			Vector2 IMouseInputSource.screenPositionDelta
			{
				get
				{
					this.TryUpdate();
					return this.m_MousePosition - this.m_MousePositionPrev;
				}
			}

			// Token: 0x17000277 RID: 631
			// (get) Token: 0x060004D3 RID: 1235 RVA: 0x0000B038 File Offset: 0x00009238
			Vector2 IMouseInputSource.wheelDelta
			{
				get
				{
					this.TryUpdate();
					return Input.mouseScrollDelta;
				}
			}

			// Token: 0x17000278 RID: 632
			// (get) Token: 0x060004D4 RID: 1236 RVA: 0x0000B045 File Offset: 0x00009245
			bool ITouchInputSource.touchSupported
			{
				get
				{
					this.TryUpdate();
					return Input.touchSupported;
				}
			}

			// Token: 0x17000279 RID: 633
			// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0000B052 File Offset: 0x00009252
			int ITouchInputSource.touchCount
			{
				get
				{
					this.TryUpdate();
					return Input.touchCount;
				}
			}

			// Token: 0x060004D6 RID: 1238 RVA: 0x0000B05F File Offset: 0x0000925F
			Touch ITouchInputSource.GetTouch(int index)
			{
				this.TryUpdate();
				return Input.GetTouch(index);
			}

			// Token: 0x060004D7 RID: 1239 RVA: 0x0000B06D File Offset: 0x0000926D
			private void TryUpdate()
			{
				if (Time.frameCount == this.m_LastUpdatedFrame)
				{
					return;
				}
				this.m_LastUpdatedFrame = Time.frameCount;
				this.m_MousePositionPrev = this.m_MousePosition;
				this.m_MousePosition = Input.mousePosition;
			}

			// Token: 0x040002BB RID: 699
			private Vector2 m_MousePosition;

			// Token: 0x040002BC RID: 700
			private Vector2 m_MousePositionPrev;

			// Token: 0x040002BD RID: 701
			private int m_LastUpdatedFrame = -1;
		}
	}
}

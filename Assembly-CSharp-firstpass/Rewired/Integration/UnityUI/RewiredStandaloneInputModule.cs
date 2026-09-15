using System;
using System.Collections.Generic;
using Rewired.Components;
using Rewired.UI;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Rewired.Integration.UnityUI
{
	// Token: 0x02000055 RID: 85
	[AddComponentMenu("Rewired/Rewired Standalone Input Module")]
	public sealed class RewiredStandaloneInputModule : RewiredPointerInputModule
	{
		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0000B0B3 File Offset: 0x000092B3
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x0000B0BB File Offset: 0x000092BB
		public InputManager_Base RewiredInputManager
		{
			get
			{
				return this.rewiredInputManager;
			}
			set
			{
				this.rewiredInputManager = value;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x0000B0C4 File Offset: 0x000092C4
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x0000B0CC File Offset: 0x000092CC
		public bool UseAllRewiredGamePlayers
		{
			get
			{
				return this.useAllRewiredGamePlayers;
			}
			set
			{
				bool flag = value != this.useAllRewiredGamePlayers;
				this.useAllRewiredGamePlayers = value;
				if (flag)
				{
					this.SetupRewiredVars();
				}
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x0000B0E9 File Offset: 0x000092E9
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x0000B0F1 File Offset: 0x000092F1
		public bool UseRewiredSystemPlayer
		{
			get
			{
				return this.useRewiredSystemPlayer;
			}
			set
			{
				bool flag = value != this.useRewiredSystemPlayer;
				this.useRewiredSystemPlayer = value;
				if (flag)
				{
					this.SetupRewiredVars();
				}
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x0000B10E File Offset: 0x0000930E
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x0000B120 File Offset: 0x00009320
		public int[] RewiredPlayerIds
		{
			get
			{
				return (int[])this.rewiredPlayerIds.Clone();
			}
			set
			{
				this.rewiredPlayerIds = ((value != null) ? ((int[])value.Clone()) : new int[0]);
				this.SetupRewiredVars();
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x0000B144 File Offset: 0x00009344
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x0000B14C File Offset: 0x0000934C
		public bool UsePlayingPlayersOnly
		{
			get
			{
				return this.usePlayingPlayersOnly;
			}
			set
			{
				this.usePlayingPlayersOnly = value;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x0000B155 File Offset: 0x00009355
		// (set) Token: 0x060004E4 RID: 1252 RVA: 0x0000B162 File Offset: 0x00009362
		public List<PlayerMouse> PlayerMice
		{
			get
			{
				return new List<PlayerMouse>(this.playerMice);
			}
			set
			{
				if (value == null)
				{
					this.playerMice = new List<PlayerMouse>();
					this.SetupRewiredVars();
					return;
				}
				this.playerMice = new List<PlayerMouse>(value);
				this.SetupRewiredVars();
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x0000B18B File Offset: 0x0000938B
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x0000B193 File Offset: 0x00009393
		public bool MoveOneElementPerAxisPress
		{
			get
			{
				return this.moveOneElementPerAxisPress;
			}
			set
			{
				this.moveOneElementPerAxisPress = value;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x0000B19C File Offset: 0x0000939C
		// (set) Token: 0x060004E8 RID: 1256 RVA: 0x0000B1A4 File Offset: 0x000093A4
		public bool allowMouseInput
		{
			get
			{
				return this.m_allowMouseInput;
			}
			set
			{
				this.m_allowMouseInput = value;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x0000B1AD File Offset: 0x000093AD
		// (set) Token: 0x060004EA RID: 1258 RVA: 0x0000B1B5 File Offset: 0x000093B5
		public bool allowMouseInputIfTouchSupported
		{
			get
			{
				return this.m_allowMouseInputIfTouchSupported;
			}
			set
			{
				this.m_allowMouseInputIfTouchSupported = value;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0000B1BE File Offset: 0x000093BE
		// (set) Token: 0x060004EC RID: 1260 RVA: 0x0000B1C6 File Offset: 0x000093C6
		public bool allowTouchInput
		{
			get
			{
				return this.m_allowTouchInput;
			}
			set
			{
				this.m_allowTouchInput = value;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0000B1CF File Offset: 0x000093CF
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x0000B1D7 File Offset: 0x000093D7
		public bool deselectIfBackgroundClicked
		{
			get
			{
				return this.m_deselectIfBackgroundClicked;
			}
			set
			{
				this.m_deselectIfBackgroundClicked = value;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x0000B1E0 File Offset: 0x000093E0
		// (set) Token: 0x060004F0 RID: 1264 RVA: 0x0000B1E8 File Offset: 0x000093E8
		private bool deselectBeforeSelecting
		{
			get
			{
				return this.m_deselectBeforeSelecting;
			}
			set
			{
				this.m_deselectBeforeSelecting = value;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x0000B1F1 File Offset: 0x000093F1
		// (set) Token: 0x060004F2 RID: 1266 RVA: 0x0000B1F9 File Offset: 0x000093F9
		public bool SetActionsById
		{
			get
			{
				return this.setActionsById;
			}
			set
			{
				if (this.setActionsById == value)
				{
					return;
				}
				this.setActionsById = value;
				this.SetupRewiredVars();
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x0000B212 File Offset: 0x00009412
		// (set) Token: 0x060004F4 RID: 1268 RVA: 0x0000B21C File Offset: 0x0000941C
		public int HorizontalActionId
		{
			get
			{
				return this.horizontalActionId;
			}
			set
			{
				if (value == this.horizontalActionId)
				{
					return;
				}
				this.horizontalActionId = value;
				if (ReInput.isReady)
				{
					this.m_HorizontalAxis = ((ReInput.mapping.GetAction(value) != null) ? ReInput.mapping.GetAction(value).name : string.Empty);
				}
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x0000B26B File Offset: 0x0000946B
		// (set) Token: 0x060004F6 RID: 1270 RVA: 0x0000B274 File Offset: 0x00009474
		public int VerticalActionId
		{
			get
			{
				return this.verticalActionId;
			}
			set
			{
				if (value == this.verticalActionId)
				{
					return;
				}
				this.verticalActionId = value;
				if (ReInput.isReady)
				{
					this.m_VerticalAxis = ((ReInput.mapping.GetAction(value) != null) ? ReInput.mapping.GetAction(value).name : string.Empty);
				}
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x0000B2C3 File Offset: 0x000094C3
		// (set) Token: 0x060004F8 RID: 1272 RVA: 0x0000B2CC File Offset: 0x000094CC
		public int SubmitActionId
		{
			get
			{
				return this.submitActionId;
			}
			set
			{
				if (value == this.submitActionId)
				{
					return;
				}
				this.submitActionId = value;
				if (ReInput.isReady)
				{
					this.m_SubmitButton = ((ReInput.mapping.GetAction(value) != null) ? ReInput.mapping.GetAction(value).name : string.Empty);
				}
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x0000B31B File Offset: 0x0000951B
		// (set) Token: 0x060004FA RID: 1274 RVA: 0x0000B324 File Offset: 0x00009524
		public int CancelActionId
		{
			get
			{
				return this.cancelActionId;
			}
			set
			{
				if (value == this.cancelActionId)
				{
					return;
				}
				this.cancelActionId = value;
				if (ReInput.isReady)
				{
					this.m_CancelButton = ((ReInput.mapping.GetAction(value) != null) ? ReInput.mapping.GetAction(value).name : string.Empty);
				}
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x0000B373 File Offset: 0x00009573
		protected override bool isMouseSupported
		{
			get
			{
				return base.isMouseSupported && this.m_allowMouseInput && (!this.isTouchSupported || this.m_allowMouseInputIfTouchSupported);
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x0000B1BE File Offset: 0x000093BE
		private bool isTouchAllowed
		{
			get
			{
				return this.m_allowTouchInput;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x0000B399 File Offset: 0x00009599
		// (set) Token: 0x060004FE RID: 1278 RVA: 0x0000B3A1 File Offset: 0x000095A1
		[Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead")]
		public bool allowActivationOnMobileDevice
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x0000B399 File Offset: 0x00009599
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x0000B3A1 File Offset: 0x000095A1
		public bool forceModuleActive
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x0000B3AA File Offset: 0x000095AA
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x0000B3B2 File Offset: 0x000095B2
		public float inputActionsPerSecond
		{
			get
			{
				return this.m_InputActionsPerSecond;
			}
			set
			{
				this.m_InputActionsPerSecond = value;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x0000B3BB File Offset: 0x000095BB
		// (set) Token: 0x06000504 RID: 1284 RVA: 0x0000B3C3 File Offset: 0x000095C3
		public float repeatDelay
		{
			get
			{
				return this.m_RepeatDelay;
			}
			set
			{
				this.m_RepeatDelay = value;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x0000B3CC File Offset: 0x000095CC
		// (set) Token: 0x06000506 RID: 1286 RVA: 0x0000B3D4 File Offset: 0x000095D4
		public string horizontalAxis
		{
			get
			{
				return this.m_HorizontalAxis;
			}
			set
			{
				if (this.m_HorizontalAxis == value)
				{
					return;
				}
				this.m_HorizontalAxis = value;
				if (ReInput.isReady)
				{
					this.horizontalActionId = ReInput.mapping.GetActionId(value);
				}
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x0000B404 File Offset: 0x00009604
		// (set) Token: 0x06000508 RID: 1288 RVA: 0x0000B40C File Offset: 0x0000960C
		public string verticalAxis
		{
			get
			{
				return this.m_VerticalAxis;
			}
			set
			{
				if (this.m_VerticalAxis == value)
				{
					return;
				}
				this.m_VerticalAxis = value;
				if (ReInput.isReady)
				{
					this.verticalActionId = ReInput.mapping.GetActionId(value);
				}
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x0000B43C File Offset: 0x0000963C
		// (set) Token: 0x0600050A RID: 1290 RVA: 0x0000B444 File Offset: 0x00009644
		public string submitButton
		{
			get
			{
				return this.m_SubmitButton;
			}
			set
			{
				if (this.m_SubmitButton == value)
				{
					return;
				}
				this.m_SubmitButton = value;
				if (ReInput.isReady)
				{
					this.submitActionId = ReInput.mapping.GetActionId(value);
				}
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x0000B474 File Offset: 0x00009674
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x0000B47C File Offset: 0x0000967C
		public string cancelButton
		{
			get
			{
				return this.m_CancelButton;
			}
			set
			{
				if (this.m_CancelButton == value)
				{
					return;
				}
				this.m_CancelButton = value;
				if (ReInput.isReady)
				{
					this.cancelActionId = ReInput.mapping.GetActionId(value);
				}
			}
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0000B4AC File Offset: 0x000096AC
		private RewiredStandaloneInputModule()
		{
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0000B553 File Offset: 0x00009753
		protected override void Awake()
		{
			base.Awake();
			this.isTouchSupported = base.defaultTouchInputSource.touchSupported;
			ReInput.InitializedEvent += this.OnRewiredInitialized;
			this.InitializeRewired();
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0000B583 File Offset: 0x00009783
		public override void UpdateModule()
		{
			this.CheckEditorRecompile();
			if (this.recompiling)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.m_HasFocus)
			{
				this.ShouldIgnoreEventsOnNoFocus();
				return;
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0000B5AC File Offset: 0x000097AC
		public override bool IsModuleSupported()
		{
			return true;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0000B5B0 File Offset: 0x000097B0
		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}
			if (this.recompiling)
			{
				return false;
			}
			if (!ReInput.isReady)
			{
				return false;
			}
			bool flag = this.m_ForceModuleActive;
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					flag |= this.GetButtonDown(player, this.submitActionId);
					flag |= this.GetButtonDown(player, this.cancelActionId);
					if (this.moveOneElementPerAxisPress)
					{
						flag |= this.GetButtonDown(player, this.horizontalActionId) || this.GetNegativeButtonDown(player, this.horizontalActionId);
						flag |= this.GetButtonDown(player, this.verticalActionId) || this.GetNegativeButtonDown(player, this.verticalActionId);
					}
					else
					{
						flag |= !Mathf.Approximately(this.GetAxis(player, this.horizontalActionId), 0f);
						flag |= !Mathf.Approximately(this.GetAxis(player, this.verticalActionId), 0f);
					}
				}
			}
			if (this.isMouseSupported)
			{
				flag |= this.DidAnyMouseMove();
				flag |= this.GetMouseButtonDownOnAnyMouse(0);
			}
			if (this.isTouchAllowed)
			{
				for (int j = 0; j < base.defaultTouchInputSource.touchCount; j++)
				{
					Touch touch = base.defaultTouchInputSource.GetTouch(j);
					flag |= touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary;
				}
			}
			return flag;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0000B73C File Offset: 0x0000993C
		public override void ActivateModule()
		{
			if (!this.m_HasFocus && this.ShouldIgnoreEventsOnNoFocus())
			{
				return;
			}
			base.ActivateModule();
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0000B793 File Offset: 0x00009993
		public void ClearMouseSelection()
		{
			base.ClearSelection();
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0000B79B File Offset: 0x0000999B
		public void ForceInitEventSystem()
		{
			if (base.eventSystem == null && base.gameObject.activeSelf)
			{
				this.OnEnable();
			}
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0000B7BE File Offset: 0x000099BE
		public override void DeactivateModule()
		{
			base.DeactivateModule();
			base.ClearSelection();
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0000B7CC File Offset: 0x000099CC
		public override void Process()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.m_HasFocus && this.ShouldIgnoreEventsOnNoFocus())
			{
				return;
			}
			if (!base.enabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			bool flag = this.SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag |= this.SendMoveEventToSelectedObject();
				}
				if (!flag)
				{
					this.SendSubmitEventToSelectedObject();
				}
			}
			if (!this.ProcessTouchEvents() && this.isMouseSupported && !this.isGamepadActive)
			{
				this.ProcessMouseEvents();
			}
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0000B850 File Offset: 0x00009A50
		private bool ProcessTouchEvents()
		{
			if (!this.isTouchAllowed)
			{
				return false;
			}
			for (int i = 0; i < base.defaultTouchInputSource.touchCount; i++)
			{
				Touch touch = base.defaultTouchInputSource.GetTouch(i);
				if (touch.type != TouchType.Indirect)
				{
					bool flag;
					bool flag2;
					PlayerPointerEventData touchPointerEventData = base.GetTouchPointerEventData(0, 0, touch, out flag, out flag2);
					this.ProcessTouchPress(touchPointerEventData, flag, flag2);
					if (!flag2)
					{
						this.ProcessMove(touchPointerEventData);
						this.ProcessDrag(touchPointerEventData);
					}
					else
					{
						base.RemovePointerData(touchPointerEventData);
					}
				}
			}
			return base.defaultTouchInputSource.touchCount > 0;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0000B8D8 File Offset: 0x00009AD8
		private void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
		{
			GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
			if (pressed)
			{
				pointerEvent.eligibleForClick = true;
				pointerEvent.delta = Vector2.zero;
				pointerEvent.dragging = false;
				pointerEvent.useDragThreshold = true;
				pointerEvent.pressPosition = pointerEvent.position;
				pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;
				this.HandleMouseTouchDeselectionOnSelectionChanged(gameObject, pointerEvent);
				if (pointerEvent.pointerEnter != gameObject)
				{
					base.HandlePointerExitAndEnter(pointerEvent, gameObject);
					pointerEvent.pointerEnter = gameObject;
				}
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, pointerEvent, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				double unscaledTime = ReInput.time.unscaledTime;
				if (gameObject2 == pointerEvent.lastPress)
				{
					if (unscaledTime - (double)pointerEvent.clickTime < 0.30000001192092896)
					{
						int num = pointerEvent.clickCount + 1;
						pointerEvent.clickCount = num;
					}
					else
					{
						pointerEvent.clickCount = 1;
					}
					pointerEvent.clickTime = (float)unscaledTime;
				}
				else
				{
					pointerEvent.clickCount = 1;
				}
				pointerEvent.pointerPress = gameObject2;
				pointerEvent.rawPointerPress = gameObject;
				pointerEvent.clickTime = (float)unscaledTime;
				pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (released)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (pointerEvent.pointerPress == eventHandler && pointerEvent.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
				}
				else if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, pointerEvent, ExecuteEvents.dropHandler);
				}
				pointerEvent.eligibleForClick = false;
				pointerEvent.pointerPress = null;
				pointerEvent.rawPointerPress = null;
				if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.dragging = false;
				pointerEvent.pointerDrag = null;
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.pointerDrag = null;
				ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(pointerEvent.pointerEnter, pointerEvent, ExecuteEvents.pointerExitHandler);
				pointerEvent.pointerEnter = null;
			}
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0000BB08 File Offset: 0x00009D08
		private bool SendSubmitEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			if (this.recompiling)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					if (this.GetButtonDown(player, this.submitActionId))
					{
						ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
						break;
					}
					if (this.GetButtonDown(player, this.cancelActionId))
					{
						ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
						break;
					}
				}
			}
			return baseEventData.used;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0000BBD0 File Offset: 0x00009DD0
		private Vector2 GetRawMoveVector()
		{
			if (this.recompiling)
			{
				return Vector2.zero;
			}
			Vector2 zero = Vector2.zero;
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					float num = this.GetAxis(player, this.horizontalActionId);
					float num2 = this.GetAxis(player, this.verticalActionId);
					if (Mathf.Approximately(num, 0f))
					{
						num = 0f;
					}
					if (Mathf.Approximately(num2, 0f))
					{
						num2 = 0f;
					}
					if (this.moveOneElementPerAxisPress)
					{
						if (this.GetButtonDown(player, this.horizontalActionId) && num > 0f)
						{
							zero.x += 1f;
						}
						if (this.GetNegativeButtonDown(player, this.horizontalActionId) && num < 0f)
						{
							zero.x -= 1f;
						}
						if (this.GetButtonDown(player, this.verticalActionId) && num2 > 0f)
						{
							zero.y += 1f;
						}
						if (this.GetNegativeButtonDown(player, this.verticalActionId) && num2 < 0f)
						{
							zero.y -= 1f;
						}
					}
					else
					{
						if (this.GetButton(player, this.horizontalActionId) && num > 0f)
						{
							zero.x += 1f;
						}
						if (this.GetNegativeButton(player, this.horizontalActionId) && num < 0f)
						{
							zero.x -= 1f;
						}
						if (this.GetButton(player, this.verticalActionId) && num2 > 0f)
						{
							zero.y += 1f;
						}
						if (this.GetNegativeButton(player, this.verticalActionId) && num2 < 0f)
						{
							zero.y -= 1f;
						}
					}
				}
			}
			return zero;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0000BDD0 File Offset: 0x00009FD0
		private bool SendMoveEventToSelectedObject()
		{
			if (this.recompiling)
			{
				return false;
			}
			double unscaledTime = ReInput.time.unscaledTime;
			Vector2 rawMoveVector = this.GetRawMoveVector();
			if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
			{
				this.m_ConsecutiveMoveCount = 0;
				return false;
			}
			bool flag = Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0f;
			bool flag2;
			bool flag3;
			this.CheckButtonOrKeyMovement(out flag2, out flag3);
			AxisEventData axisEventData = null;
			bool flag4 = flag2 || flag3;
			if (flag4)
			{
				axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0f);
				MoveDirection moveDir = axisEventData.moveDir;
				flag4 = ((moveDir == MoveDirection.Up || moveDir == MoveDirection.Down) && flag3) || ((moveDir == MoveDirection.Left || moveDir == MoveDirection.Right) && flag2);
			}
			if (!flag4)
			{
				if (this.m_RepeatDelay > 0f)
				{
					if (flag && this.m_ConsecutiveMoveCount == 1)
					{
						flag4 = unscaledTime > this.m_PrevActionTime + (double)this.m_RepeatDelay;
					}
					else
					{
						flag4 = unscaledTime > this.m_PrevActionTime + (double)(1f / this.m_InputActionsPerSecond);
					}
				}
				else
				{
					flag4 = unscaledTime > this.m_PrevActionTime + (double)(1f / this.m_InputActionsPerSecond);
				}
			}
			if (!flag4)
			{
				return false;
			}
			if (axisEventData == null)
			{
				axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0f);
			}
			if (axisEventData.moveDir != MoveDirection.None)
			{
				ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
				if (!flag)
				{
					this.m_ConsecutiveMoveCount = 0;
				}
				if (this.m_ConsecutiveMoveCount == 0 || (!flag2 && !flag3))
				{
					this.m_ConsecutiveMoveCount++;
				}
				this.m_PrevActionTime = unscaledTime;
				this.m_LastMoveVector = rawMoveVector;
			}
			else
			{
				this.m_ConsecutiveMoveCount = 0;
			}
			return axisEventData.used;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0000BF88 File Offset: 0x0000A188
		private void CheckButtonOrKeyMovement(out bool downHorizontal, out bool downVertical)
		{
			downHorizontal = false;
			downVertical = false;
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					downHorizontal |= this.GetButtonDown(player, this.horizontalActionId) || this.GetNegativeButtonDown(player, this.horizontalActionId);
					downVertical |= this.GetButtonDown(player, this.verticalActionId) || this.GetNegativeButtonDown(player, this.verticalActionId);
				}
			}
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0000C01C File Offset: 0x0000A21C
		private void ProcessMouseEvents()
		{
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					int mouseInputSourceCount = base.GetMouseInputSourceCount(this.playerIds[i]);
					for (int j = 0; j < mouseInputSourceCount; j++)
					{
						this.ProcessMouseEvent(this.playerIds[i], j);
					}
				}
			}
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0000C08C File Offset: 0x0000A28C
		private void ProcessMouseEvent(int playerId, int pointerIndex)
		{
			RewiredPointerInputModule.MouseState mousePointerEventData = this.GetMousePointerEventData(playerId, pointerIndex);
			if (mousePointerEventData == null)
			{
				return;
			}
			RewiredPointerInputModule.MouseButtonEventData eventData = mousePointerEventData.GetButtonState(0).eventData;
			this.ProcessMousePress(eventData);
			this.ProcessMove(eventData.buttonData);
			this.ProcessDrag(eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(1).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(1).eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(2).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(2).eventData.buttonData);
			IMouseInputSource mouseInputSource = base.GetMouseInputSource(playerId, pointerIndex);
			if (mouseInputSource == null)
			{
				return;
			}
			for (int i = 3; i < mouseInputSource.buttonCount; i++)
			{
				this.ProcessMousePress(mousePointerEventData.GetButtonState(i).eventData);
				this.ProcessDrag(mousePointerEventData.GetButtonState(i).eventData.buttonData);
			}
			if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), eventData.buttonData, ExecuteEvents.scrollHandler);
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0000C1B8 File Offset: 0x0000A3B8
		private bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0000C200 File Offset: 0x0000A400
		private void ProcessMousePress(RewiredPointerInputModule.MouseButtonEventData data)
		{
			PlayerPointerEventData buttonData = data.buttonData;
			if (base.GetMouseInputSource(buttonData.playerId, buttonData.inputSourceIndex) == null)
			{
				return;
			}
			GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
			if (data.PressedThisFrame())
			{
				buttonData.eligibleForClick = true;
				buttonData.delta = Vector2.zero;
				buttonData.dragging = false;
				buttonData.useDragThreshold = true;
				buttonData.pressPosition = buttonData.position;
				buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
				this.HandleMouseTouchDeselectionOnSelectionChanged(gameObject, buttonData);
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				double unscaledTime = ReInput.time.unscaledTime;
				if (gameObject2 == buttonData.lastPress)
				{
					if (unscaledTime - (double)buttonData.clickTime < 0.30000001192092896)
					{
						PlayerPointerEventData playerPointerEventData = buttonData;
						int num = playerPointerEventData.clickCount + 1;
						playerPointerEventData.clickCount = num;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.clickTime = (float)unscaledTime;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.pointerPress = gameObject2;
				buttonData.rawPointerPress = gameObject;
				buttonData.clickTime = (float)unscaledTime;
				buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (buttonData.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (data.ReleasedThisFrame())
			{
				ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
				}
				else if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, buttonData, ExecuteEvents.dropHandler);
				}
				buttonData.eligibleForClick = false;
				buttonData.pointerPress = null;
				buttonData.rawPointerPress = null;
				if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
				}
				buttonData.dragging = false;
				buttonData.pointerDrag = null;
				if (gameObject != buttonData.pointerEnter)
				{
					base.HandlePointerExitAndEnter(buttonData, null);
					base.HandlePointerExitAndEnter(buttonData, gameObject);
				}
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0000C41C File Offset: 0x0000A61C
		private void HandleMouseTouchDeselectionOnSelectionChanged(GameObject currentOverGo, BaseEventData pointerEvent)
		{
			if (this.m_deselectIfBackgroundClicked && this.m_deselectBeforeSelecting)
			{
				base.DeselectIfSelectionChanged(currentOverGo, pointerEvent);
				return;
			}
			GameObject eventHandler = ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo);
			if (this.m_deselectIfBackgroundClicked)
			{
				if (eventHandler != base.eventSystem.currentSelectedGameObject && eventHandler != null)
				{
					base.eventSystem.SetSelectedGameObject(null, pointerEvent);
					return;
				}
			}
			else if (this.m_deselectBeforeSelecting && eventHandler != null && eventHandler != base.eventSystem.currentSelectedGameObject)
			{
				base.eventSystem.SetSelectedGameObject(null, pointerEvent);
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0000C4AC File Offset: 0x0000A6AC
		private void OnApplicationFocus(bool hasFocus)
		{
			this.m_HasFocus = hasFocus;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0000C4B5 File Offset: 0x0000A6B5
		private bool ShouldIgnoreEventsOnNoFocus()
		{
			return !ReInput.isReady || ReInput.configuration.ignoreInputWhenAppNotInFocus;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0000C4CA File Offset: 0x0000A6CA
		protected override void OnDestroy()
		{
			base.OnDestroy();
			ReInput.InitializedEvent -= this.OnRewiredInitialized;
			ReInput.ShutDownEvent -= this.OnRewiredShutDown;
			ReInput.EditorRecompileEvent -= this.OnEditorRecompile;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0000C508 File Offset: 0x0000A708
		protected override bool IsDefaultPlayer(int playerId)
		{
			if (this.playerIds == null)
			{
				return false;
			}
			if (!ReInput.isReady)
			{
				return false;
			}
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < this.playerIds.Length; j++)
				{
					Player player = ReInput.players.GetPlayer(this.playerIds[j]);
					if (player != null && (i >= 1 || !this.usePlayingPlayersOnly || player.isPlaying) && (i >= 2 || player.controllers.hasMouse))
					{
						return this.playerIds[j] == playerId;
					}
				}
			}
			return false;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0000C590 File Offset: 0x0000A790
		private void InitializeRewired()
		{
			if (!ReInput.isReady)
			{
				Debug.LogError("Rewired is not initialized! Are you missing a Rewired Input Manager in your scene?");
				return;
			}
			ReInput.ShutDownEvent -= this.OnRewiredShutDown;
			ReInput.ShutDownEvent += this.OnRewiredShutDown;
			ReInput.EditorRecompileEvent -= this.OnEditorRecompile;
			ReInput.EditorRecompileEvent += this.OnEditorRecompile;
			this.SetupRewiredVars();
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0000C5FC File Offset: 0x0000A7FC
		private void SetupRewiredVars()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.SetUpRewiredActions();
			if (this.useAllRewiredGamePlayers)
			{
				IList<Player> list = (this.useRewiredSystemPlayer ? ReInput.players.AllPlayers : ReInput.players.Players);
				this.playerIds = new int[list.Count];
				for (int i = 0; i < list.Count; i++)
				{
					this.playerIds[i] = list[i].id;
				}
			}
			else
			{
				bool flag = false;
				List<int> list2 = new List<int>(this.rewiredPlayerIds.Length + 1);
				for (int j = 0; j < this.rewiredPlayerIds.Length; j++)
				{
					Player player = ReInput.players.GetPlayer(this.rewiredPlayerIds[j]);
					if (player != null && !list2.Contains(player.id))
					{
						list2.Add(player.id);
						if (player.id == 9999999)
						{
							flag = true;
						}
					}
				}
				if (this.useRewiredSystemPlayer && !flag)
				{
					list2.Insert(0, ReInput.players.GetSystemPlayer().id);
				}
				this.playerIds = list2.ToArray();
			}
			this.SetUpRewiredPlayerMice();
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0000C71C File Offset: 0x0000A91C
		private void SetUpRewiredPlayerMice()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			base.ClearMouseInputSources();
			for (int i = 0; i < this.playerMice.Count; i++)
			{
				PlayerMouse playerMouse = this.playerMice[i];
				if (!UnityTools.IsNullOrDestroyed<PlayerMouse>(playerMouse))
				{
					base.AddMouseInputSource(playerMouse);
				}
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0000C76C File Offset: 0x0000A96C
		private void SetUpRewiredActions()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.setActionsById)
			{
				this.horizontalActionId = ReInput.mapping.GetActionId(this.m_HorizontalAxis);
				this.verticalActionId = ReInput.mapping.GetActionId(this.m_VerticalAxis);
				this.submitActionId = ReInput.mapping.GetActionId(this.m_SubmitButton);
				this.cancelActionId = ReInput.mapping.GetActionId(this.m_CancelButton);
				return;
			}
			InputAction inputAction = ReInput.mapping.GetAction(this.horizontalActionId);
			this.m_HorizontalAxis = ((inputAction != null) ? inputAction.name : string.Empty);
			if (inputAction == null)
			{
				this.horizontalActionId = -1;
			}
			inputAction = ReInput.mapping.GetAction(this.verticalActionId);
			this.m_VerticalAxis = ((inputAction != null) ? inputAction.name : string.Empty);
			if (inputAction == null)
			{
				this.verticalActionId = -1;
			}
			inputAction = ReInput.mapping.GetAction(this.submitActionId);
			this.m_SubmitButton = ((inputAction != null) ? inputAction.name : string.Empty);
			if (inputAction == null)
			{
				this.submitActionId = -1;
			}
			inputAction = ReInput.mapping.GetAction(this.cancelActionId);
			this.m_CancelButton = ((inputAction != null) ? inputAction.name : string.Empty);
			if (inputAction == null)
			{
				this.cancelActionId = -1;
			}
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0000C8A6 File Offset: 0x0000AAA6
		private bool GetButton(Player player, int actionId)
		{
			return actionId >= 0 && player.GetButton(actionId);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0000C8B5 File Offset: 0x0000AAB5
		private bool GetButtonDown(Player player, int actionId)
		{
			return actionId >= 0 && player.GetButtonDown(actionId);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0000C8C4 File Offset: 0x0000AAC4
		private bool GetNegativeButton(Player player, int actionId)
		{
			return actionId >= 0 && player.GetNegativeButton(actionId);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0000C8D3 File Offset: 0x0000AAD3
		private bool GetNegativeButtonDown(Player player, int actionId)
		{
			return actionId >= 0 && player.GetNegativeButtonDown(actionId);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0000C8E2 File Offset: 0x0000AAE2
		private float GetAxis(Player player, int actionId)
		{
			if (actionId < 0)
			{
				return 0f;
			}
			return player.GetAxis(actionId);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0000C8F5 File Offset: 0x0000AAF5
		private void CheckEditorRecompile()
		{
			if (!this.recompiling)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			this.recompiling = false;
			this.InitializeRewired();
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0000C915 File Offset: 0x0000AB15
		private void OnEditorRecompile()
		{
			this.recompiling = true;
			this.ClearRewiredVars();
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0000C924 File Offset: 0x0000AB24
		private void ClearRewiredVars()
		{
			Array.Clear(this.playerIds, 0, this.playerIds.Length);
			base.ClearMouseInputSources();
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0000C940 File Offset: 0x0000AB40
		private bool DidAnyMouseMove()
		{
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				int num = this.playerIds[i];
				Player player = ReInput.players.GetPlayer(num);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					int mouseInputSourceCount = base.GetMouseInputSourceCount(num);
					for (int j = 0; j < mouseInputSourceCount; j++)
					{
						IMouseInputSource mouseInputSource = base.GetMouseInputSource(num, j);
						if (mouseInputSource != null && mouseInputSource.screenPositionDelta.sqrMagnitude > 0f)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0000C9CC File Offset: 0x0000ABCC
		private bool GetMouseButtonDownOnAnyMouse(int buttonIndex)
		{
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				int num = this.playerIds[i];
				Player player = ReInput.players.GetPlayer(num);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					int mouseInputSourceCount = base.GetMouseInputSourceCount(num);
					for (int j = 0; j < mouseInputSourceCount; j++)
					{
						IMouseInputSource mouseInputSource = base.GetMouseInputSource(num, j);
						if (mouseInputSource != null && mouseInputSource.GetButtonDown(buttonIndex))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0000CA48 File Offset: 0x0000AC48
		private void OnRewiredInitialized()
		{
			this.InitializeRewired();
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0000CA50 File Offset: 0x0000AC50
		private void OnRewiredShutDown()
		{
			this.ClearRewiredVars();
		}

		// Token: 0x040002C1 RID: 705
		private const string DEFAULT_ACTION_MOVE_HORIZONTAL = "UIHorizontal";

		// Token: 0x040002C2 RID: 706
		private const string DEFAULT_ACTION_MOVE_VERTICAL = "UIVertical";

		// Token: 0x040002C3 RID: 707
		private const string DEFAULT_ACTION_SUBMIT = "UISubmit";

		// Token: 0x040002C4 RID: 708
		private const string DEFAULT_ACTION_CANCEL = "UICancel";

		// Token: 0x040002C5 RID: 709
		[Tooltip("(Optional) Link the Rewired Input Manager here for easier access to Player ids, etc.")]
		[SerializeField]
		private InputManager_Base rewiredInputManager;

		// Token: 0x040002C6 RID: 710
		[SerializeField]
		[Tooltip("Use all Rewired game Players to control the UI. This does not include the System Player. If enabled, this setting overrides individual Player Ids set in Rewired Player Ids.")]
		private bool useAllRewiredGamePlayers;

		// Token: 0x040002C7 RID: 711
		[SerializeField]
		[Tooltip("Allow the Rewired System Player to control the UI.")]
		private bool useRewiredSystemPlayer;

		// Token: 0x040002C8 RID: 712
		[SerializeField]
		[Tooltip("A list of Player Ids that are allowed to control the UI. If Use All Rewired Game Players = True, this list will be ignored.")]
		private int[] rewiredPlayerIds = new int[1];

		// Token: 0x040002C9 RID: 713
		[SerializeField]
		[Tooltip("Allow only Players with Player.isPlaying = true to control the UI.")]
		private bool usePlayingPlayersOnly;

		// Token: 0x040002CA RID: 714
		[SerializeField]
		[Tooltip("Player Mice allowed to interact with the UI. Each Player that owns a Player Mouse must also be allowed to control the UI or the Player Mouse will not function.")]
		private List<PlayerMouse> playerMice = new List<PlayerMouse>();

		// Token: 0x040002CB RID: 715
		[SerializeField]
		[Tooltip("Makes an axis press always move only one UI selection. Enable if you do not want to allow scrolling through UI elements by holding an axis direction.")]
		private bool moveOneElementPerAxisPress;

		// Token: 0x040002CC RID: 716
		[SerializeField]
		[Tooltip("If enabled, Action Ids will be used to set the Actions. If disabled, string names will be used to set the Actions.")]
		private bool setActionsById;

		// Token: 0x040002CD RID: 717
		[SerializeField]
		[Tooltip("Id of the horizontal Action for movement (if axis events are used).")]
		private int horizontalActionId = -1;

		// Token: 0x040002CE RID: 718
		[SerializeField]
		[Tooltip("Id of the vertical Action for movement (if axis events are used).")]
		private int verticalActionId = -1;

		// Token: 0x040002CF RID: 719
		[SerializeField]
		[Tooltip("Id of the Action used to submit.")]
		private int submitActionId = -1;

		// Token: 0x040002D0 RID: 720
		[SerializeField]
		[Tooltip("Id of the Action used to cancel.")]
		private int cancelActionId = -1;

		// Token: 0x040002D1 RID: 721
		[SerializeField]
		[Tooltip("Name of the horizontal axis for movement (if axis events are used).")]
		private string m_HorizontalAxis = "UIHorizontal";

		// Token: 0x040002D2 RID: 722
		[SerializeField]
		[Tooltip("Name of the vertical axis for movement (if axis events are used).")]
		private string m_VerticalAxis = "UIVertical";

		// Token: 0x040002D3 RID: 723
		[SerializeField]
		[Tooltip("Name of the action used to submit.")]
		private string m_SubmitButton = "UISubmit";

		// Token: 0x040002D4 RID: 724
		[SerializeField]
		[Tooltip("Name of the action used to cancel.")]
		private string m_CancelButton = "UICancel";

		// Token: 0x040002D5 RID: 725
		[SerializeField]
		[Tooltip("Number of selection changes allowed per second when a movement button/axis is held in a direction.")]
		private float m_InputActionsPerSecond = 10f;

		// Token: 0x040002D6 RID: 726
		[SerializeField]
		[Tooltip("Delay in seconds before vertical/horizontal movement starts repeating continouously when a movement direction is held.")]
		private float m_RepeatDelay;

		// Token: 0x040002D7 RID: 727
		[SerializeField]
		[Tooltip("Allows the mouse to be used to select elements.")]
		private bool m_allowMouseInput = true;

		// Token: 0x040002D8 RID: 728
		[SerializeField]
		[Tooltip("Allows the mouse to be used to select elements if the device also supports touch control.")]
		private bool m_allowMouseInputIfTouchSupported = true;

		// Token: 0x040002D9 RID: 729
		[SerializeField]
		[Tooltip("Allows touch input to be used to select elements.")]
		private bool m_allowTouchInput = true;

		// Token: 0x040002DA RID: 730
		[SerializeField]
		[Tooltip("Deselects the current selection on mouse/touch click when the pointer is not over a selectable object.")]
		private bool m_deselectIfBackgroundClicked = true;

		// Token: 0x040002DB RID: 731
		[SerializeField]
		[Tooltip("Deselects the current selection on mouse/touch click before selecting the next object.")]
		private bool m_deselectBeforeSelecting = true;

		// Token: 0x040002DC RID: 732
		[SerializeField]
		[FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
		[Tooltip("Forces the module to always be active.")]
		private bool m_ForceModuleActive;

		// Token: 0x040002DD RID: 733
		[NonSerialized]
		private int[] playerIds;

		// Token: 0x040002DE RID: 734
		private bool recompiling;

		// Token: 0x040002DF RID: 735
		[NonSerialized]
		private bool isTouchSupported;

		// Token: 0x040002E0 RID: 736
		[NonSerialized]
		private double m_PrevActionTime;

		// Token: 0x040002E1 RID: 737
		[NonSerialized]
		private Vector2 m_LastMoveVector;

		// Token: 0x040002E2 RID: 738
		[NonSerialized]
		private int m_ConsecutiveMoveCount;

		// Token: 0x040002E3 RID: 739
		[NonSerialized]
		private bool m_HasFocus = true;

		// Token: 0x040002E4 RID: 740
		public bool isGamepadActive;

		// Token: 0x02000056 RID: 86
		[Serializable]
		public class PlayerSetting
		{
			// Token: 0x06000536 RID: 1334 RVA: 0x0000CA58 File Offset: 0x0000AC58
			public PlayerSetting()
			{
			}

			// Token: 0x06000537 RID: 1335 RVA: 0x0000CA6C File Offset: 0x0000AC6C
			private PlayerSetting(RewiredStandaloneInputModule.PlayerSetting other)
			{
				if (other == null)
				{
					throw new ArgumentNullException("other");
				}
				this.playerId = other.playerId;
				this.playerMice = new List<PlayerMouse>();
				if (other.playerMice != null)
				{
					foreach (PlayerMouse playerMouse in other.playerMice)
					{
						this.playerMice.Add(playerMouse);
					}
				}
			}

			// Token: 0x06000538 RID: 1336 RVA: 0x0000CB04 File Offset: 0x0000AD04
			public RewiredStandaloneInputModule.PlayerSetting Clone()
			{
				return new RewiredStandaloneInputModule.PlayerSetting(this);
			}

			// Token: 0x040002E5 RID: 741
			public int playerId;

			// Token: 0x040002E6 RID: 742
			public List<PlayerMouse> playerMice = new List<PlayerMouse>();
		}
	}
}

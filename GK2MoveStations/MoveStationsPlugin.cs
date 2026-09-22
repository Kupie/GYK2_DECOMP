using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GK2MoveStations
{
	// Token: 0x02000002 RID: 2
	[BepInPlugin("com.example.gk2.movestations", "GK2 Move Stations", "2.0.3")]
	public class MoveStationsPlugin : BaseUnityPlugin
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private void Awake()
		{
			this.debugLogs = base.Config.Bind<bool>("General", "DebugLogs", false, "When true, extra diagnostic messages are written to the log and the on-screen panel shows technical state.");
			this.debug = this.debugLogs.Value;
			this.InitReflection();
			this.SubscribeBuildMenuOpenEvent();
			this.CreateUI();
			this.CreateMarker();
			Canvas.willRenderCanvases += this.OnCanvasWillRenderCanvases;
			base.Logger.LogInfo("GK2 Move Stations v2.0.3 loaded.");
			base.Logger.LogInfo("Open a Build menu, choose Move, then click a station. LMB = place, R = rotate, Esc or RMB = cancel.");
			if (this.debug)
			{
				base.Logger.LogInfo("Diagnostics enabled. Open the Build menu, pick a station and press F7 to re-read the game's build grid.");
				this.Dbg("Diagnostics enabled.");
				base.Invoke("DumpBuildSystemProbe", 3f);
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000210C File Offset: 0x0000030C
		private void OnDestroy()
		{
			try
			{
				Canvas.willRenderCanvases -= this.OnCanvasWillRenderCanvases;
			}
			catch
			{
			}
			try
			{
				if (this.eLazyWindowOpened != null && this.dLazyWindowOpened != null)
				{
					this.eLazyWindowOpened.RemoveEventHandler(null, this.dLazyWindowOpened);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000217C File Offset: 0x0000037C
		private void Update()
		{
			this.UpdateVerify();
			if (this.moving)
			{
				this.UpdateMoveMode();
				return;
			}
			if (this.moveMenuArmed && (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)))
			{
				this.moveMenuArmed = false;
				this.targetedWgo = null;
				if (this.nativeGridOnlyMode)
				{
					this.StopNativeBuildPreview();
				}
				this.SetHint("");
				this.ReopenBuildMenu();
			}
			if (this.debug && Input.GetKeyDown(KeyCode.F7))
			{
				this.nextSnapTry = 0f;
				this.nextSelTry = 0f;
				this.nextGridFind = 0f;
				this.nextBcFind = 0f;
				this.cachedGridObj = null;
				this.gridStatusMsg = "";
				base.Logger.LogInfo("Build grid re-scan requested.");
				this.TrySnapshotGrid(true);
				this.DumpGridPresence();
				this.DumpBuildUiProbe();
				if (this.snapValid)
				{
					base.Logger.LogInfo(string.Concat(new string[]
					{
						"Build grid: ",
						this.snapCells.Count.ToString(),
						" cells cached (src=",
						this.snapSource,
						")."
					}));
				}
				else if (this.snapRejected)
				{
					base.Logger.LogInfo("Build grid: rejected - " + this.snapRejectReason + ". Collider rules are used instead.");
				}
				else
				{
					base.Logger.LogInfo("Build grid: still not found. Open the Build menu (build desk), keep it open, then press F7 again.");
				}
				if (this.selOverrideReady)
				{
					base.Logger.LogInfo("Game cell map: ready " + this.selCalib + ".");
				}
				else
				{
					base.Logger.LogInfo("Game cell map: not usable yet " + this.selCalib + ".");
				}
			}
			if (this.moveMenuArmed && this.nativeGridOnlyMode)
			{
				this.LockNativeBuildInput(this.buildControllerInstance);
			}
			if (this.moveMenuArmed && Time.realtimeSinceStartup >= this.nextScan)
			{
				this.nextScan = Time.realtimeSinceStartup + 0.15f;
				this.ScanTarget();
			}
			if (this.moveMenuArmed && Time.realtimeSinceStartup >= this.moveMenuArmReadyAt && this.targetedWgo != null && Input.GetMouseButtonDown(0))
			{
				this.moveMenuArmed = false;
				this.StartMoveMode();
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000023BC File Offset: 0x000005BC
		private void ScanTarget()
		{
			this.targetedWgo = null;
			try
			{
				if (this.reflectionOk)
				{
					if (this.moveMenuArmed)
					{
						this.ScanTargetUnderCursor();
					}
					else
					{
						float realtimeSinceStartup = Time.realtimeSinceStartup;
						if (this.wgoCache == null || realtimeSinceStartup >= this.nextWgoRefresh)
						{
							this.nextWgoRefresh = realtimeSinceStartup + 1f;
							this.wgoCache = global::UnityEngine.Object.FindObjectsOfType(this.wgoType);
						}
						global::UnityEngine.Object[] array = this.wgoCache;
						if (array != null)
						{
							foreach (global::UnityEngine.Object @object in array)
							{
								if (!(@object == null))
								{
									object obj = MoveStationsPlugin.TryGet(this.fWgoHandler, @object);
									if (obj != null && MoveStationsPlugin.TryGet(this.fHandlerInteractor, obj) != null)
									{
										if (MoveStationsPlugin.IsMovableHandler(obj.GetType().Name))
										{
											this.targetedWgo = @object;
										}
										break;
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg("ScanTarget error: " + ex.Message);
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000024BC File Offset: 0x000006BC
		private void ScanTargetUnderCursor()
		{
			try
			{
				Camera worldCamera = this.GetWorldCamera();
				if (!(worldCamera == null) && !(this.wgoType == null) && !(this.fWgoHandler == null))
				{
					RaycastHit[] array = Physics.RaycastAll(worldCamera.ScreenPointToRay(Input.mousePosition), 200f, -1, QueryTriggerInteraction.Collide);
					if (array != null && array.Length != 0)
					{
						Array.Sort<RaycastHit>(array, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
						foreach (RaycastHit raycastHit in array)
						{
							if (!(raycastHit.collider == null))
							{
								Component component = null;
								try
								{
									component = raycastHit.collider.GetComponentInParent(this.wgoType);
								}
								catch
								{
								}
								if (!(component == null) && this.IsSupportedWgoForMove(component))
								{
									this.targetedWgo = component;
									if (this.lastCursorTargetLogged != this.targetedWgo)
									{
										this.lastCursorTargetLogged = this.targetedWgo;
										this.Dbg(string.Concat(new string[]
										{
											"cursor target: '",
											MoveStationsPlugin.PathOf(((Component)this.targetedWgo).gameObject),
											"' hit='",
											MoveStationsPlugin.PathOf(raycastHit.collider.gameObject),
											"' distance=",
											raycastHit.distance.ToString("F2")
										}));
									}
									return;
								}
							}
						}
						if (this.lastCursorTargetLogged != null)
						{
							this.lastCursorTargetLogged = null;
							this.Dbg("cursor target: none");
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg("ScanTargetUnderCursor error: " + ex.Message);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000026B4 File Offset: 0x000008B4
		private bool IsSupportedWgoForMove(Component wgo)
		{
			bool flag;
			try
			{
				if (wgo == null || this.fWgoHandler == null)
				{
					flag = false;
				}
				else
				{
					object obj = MoveStationsPlugin.TryGet(this.fWgoHandler, wgo);
					if (obj == null)
					{
						flag = false;
					}
					else
					{
						flag = MoveStationsPlugin.IsMovableHandler(obj.GetType().Name);
					}
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000271C File Offset: 0x0000091C
		private static bool IsMovableHandler(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return false;
			}
			foreach (string text in new string[]
			{
				"Station", "Craft", "Porter", "Chest", "Embalm", "Autopsy", "Crematorium", "Alchemy", "Reservoir", "Resurrection",
				"WellUpgrade"
			})
			{
				if (name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000027B8 File Offset: 0x000009B8
		private void StartMoveMode()
		{
			try
			{
				if (!(this.targetedWgo == null) && this.reflectionOk)
				{
					if (this.nativeGridOnlyMode)
					{
						this.StopNativeBuildPreview();
					}
					object obj = MoveStationsPlugin.TryGet(this.fWgoData, this.targetedWgo);
					if (obj != null)
					{
						object value = this.pDataPosition.GetValue(obj, null);
						if (value is Vector3)
						{
							this.movingWgo = this.targetedWgo;
							this.movingGo = ((Component)this.targetedWgo).gameObject;
							this.movingData = obj;
							this.originalPos = (Vector3)value;
							this.previewPos = this.originalPos;
							this.moving = true;
							this.verifyActive = false;
							this.selMismatchLogged = false;
							this.requiredAreaId = this.ResolveRequiredArea(obj);
							this.ReadFootprint();
							this.lastGridResult = "";
							this.lastGridOobKey = "";
							this.LearnOwnCells();
							this.hasValidated = false;
							this.nativeBuildPreview = this.TryStartNativeBuildPreview();
							this.EnsureValidation();
							this.UpdateMarker();
							this.UpdateMoveHint();
							if (this.debug)
							{
								base.Logger.LogInfo("Moving station started. Required build area: '" + (string.IsNullOrEmpty(this.requiredAreaId) ? "(any)" : this.requiredAreaId) + "'");
							}
							this.Dbg(string.Concat(new string[]
							{
								"grab: go='",
								MoveStationsPlugin.PathOf(this.movingGo),
								"' goLayer=",
								this.movingGo.layer.ToString(),
								" id=",
								this.ShortId(obj),
								" dataPos=",
								MoveStationsPlugin.Fmt(this.originalPos),
								" trPos=",
								MoveStationsPlugin.Fmt(((Component)this.movingWgo).transform.position)
							}));
							this.DbgGrabDetails(obj);
							this.DumpGrabColliders();
							this.Dbg("grab: foot half-extents=" + this.footHalfX.ToString("F2") + " x " + this.footHalfZ.ToString("F2"));
							this.Dbg(string.Concat(new string[]
							{
								"rules: gameSnap=",
								(this.mSnapToBounds != null).ToString(),
								" defResolved=",
								(this.movingBuildingDef != null).ToString(),
								" snapshot=",
								this.snapValid ? this.snapCells.Count.ToString() : "none",
								" buildMode=",
								this.lastBuildModeActive.ToString(),
								" gameMap=",
								this.selOverrideReady ? this.selCalib : "off",
								" wgoId='",
								this.movingWgoId,
								"' nativePreview=",
								this.nativeBuildPreview.ToString()
							}));
						}
					}
				}
			}
			catch (Exception ex)
			{
				base.Logger.LogWarning("StartMoveMode error: " + ex.Message);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002AFC File Offset: 0x00000CFC
		private void DbgGrabDetails(object data)
		{
			if (!this.debug)
			{
				return;
			}
			try
			{
				this.Dbg("grab: wgo type=" + this.movingWgo.GetType().FullName + " data type=" + data.GetType().FullName);
				if (this.defType != null)
				{
					object obj = MoveStationsPlugin.FindReferenceOfType(data, this.defType);
					if (obj != null)
					{
						this.Dbg("grab: def type=" + obj.GetType().FullName);
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg("DbgGrabDetails error: " + ex.Message);
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002BA8 File Offset: 0x00000DA8
		private void DumpGrabColliders()
		{
			if (!this.debug || this.movingGo == null)
			{
				return;
			}
			try
			{
				Collider[] componentsInChildren = this.movingGo.GetComponentsInChildren<Collider>(true);
				int num = 0;
				string text = "";
				foreach (Collider collider in componentsInChildren)
				{
					if (!(collider == null))
					{
						num++;
						if (num <= 8)
						{
							text = string.Concat(new string[]
							{
								text,
								(num > 1) ? ", " : "",
								collider.gameObject.name,
								"(L",
								collider.gameObject.layer.ToString(),
								collider.isTrigger ? ",trig" : "",
								")"
							});
						}
					}
				}
				this.Dbg("grab colliders: " + num.ToString() + " -> " + text);
			}
			catch (Exception ex)
			{
				this.Dbg("grab collider dump error: " + ex.Message);
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002CCC File Offset: 0x00000ECC
		private void UpdateMoveMode()
		{
			if (this.movingWgo == null || this.movingGo == null)
			{
				base.Logger.LogWarning("Station vanished while moving; cancelling.");
				this.EndMoveMode();
				return;
			}
			if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
			{
				this.CancelMove();
				return;
			}
			if (Input.GetKeyDown(KeyCode.R))
			{
				this.RotateStation();
			}
			if (this.nativeBuildPreview)
			{
				this.UpdateNativeBuildPreview();
			}
			else
			{
				Vector3 cursorWorldPos = this.GetCursorWorldPos(this.originalPos.y);
				Vector3 vector = this.ClampFromPlayer(cursorWorldPos);
				vector.y = this.originalPos.y;
				vector = this.SnapToGrid(vector);
				if ((vector - this.previewPos).sqrMagnitude > 0.0001f)
				{
					this.previewPos = vector;
					this.ApplyPreviewPosition();
					this.Dbg("cursor raw=" + MoveStationsPlugin.Fmt(cursorWorldPos) + " -> snapped=" + MoveStationsPlugin.Fmt(vector));
				}
			}
			this.EnsureValidation();
			this.UpdateMarker();
			this.UpdateMoveHint();
			if (Input.GetMouseButtonDown(0))
			{
				bool flag = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
				if (this.spotValid)
				{
					this.PlaceStation();
					return;
				}
				if (flag)
				{
					base.Logger.LogInfo("Force place at " + MoveStationsPlugin.Fmt(this.previewPos) + " - the mod's rules said: " + this.spotReason);
					this.PlaceStation();
					return;
				}
				this.Dbg("click ignored: " + this.spotReason);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002E50 File Offset: 0x00001050
		private bool TryStartNativeBuildPreview()
		{
			bool flag;
			try
			{
				if (this.nativeGridOnlyMode)
				{
					this.StopNativeBuildPreview();
				}
				if (this.buildControllerType == null || this.buildDataType == null || this.worldZoneType == null || this.movingBuildingDef == null)
				{
					this.Dbg("native preview unavailable: missing BuildController/BuildData/WorldZone/BuildingDef");
					flag = false;
				}
				else if (this.mBuildDataForBuild == null || this.mBcEnableBuildMode == null || this.fBcBuildPointer == null)
				{
					this.Dbg("native preview unavailable: required build methods/field missing");
					flag = false;
				}
				else
				{
					global::UnityEngine.Object @object = this.buildControllerInstance;
					if (@object == null || @object == null)
					{
						@object = MoveStationsPlugin.FindAnyUnityObject(this.buildControllerType);
						this.buildControllerInstance = @object;
					}
					if (@object == null)
					{
						flag = false;
					}
					else
					{
						this.nativeWorldZone = this.FindWorldZoneForMovingWgo();
						if (this.nativeWorldZone == null)
						{
							this.Dbg("native preview unavailable: world zone for station not found");
							flag = false;
						}
						else
						{
							object obj = this.mBuildDataForBuild.Invoke(null, new object[] { this.movingBuildingDef });
							if (obj == null)
							{
								flag = false;
							}
							else
							{
								this.movingGoWasActive = this.movingGo != null && this.movingGo.activeSelf;
								this.movingDataWasTemp = MoveStationsPlugin.ReadBoolField(this.fWgoDataTemp, this.movingData);
								if (this.fWgoDataTemp != null && this.movingData != null)
								{
									this.fWgoDataTemp.SetValue(this.movingData, true);
								}
								if (this.movingGo != null)
								{
									this.movingGo.SetActive(false);
								}
								MethodBase methodBase = this.mBcEnableBuildMode;
								object obj2 = @object;
								object[] array = new object[4];
								array[0] = obj;
								array[1] = this.nativeWorldZone;
								methodBase.Invoke(obj2, array);
								this.nativeBuildModeEnabled = true;
								this.SetPlayerBuildControlTaken(true);
								this.nativePointer = this.fBcBuildPointer.GetValue(@object);
								if (this.nativePointer == null)
								{
									throw new InvalidOperationException("BuildController.buildPointer is null after EnableBuildMode");
								}
								object prop = MoveStationsPlugin.GetProp(this.nativePointer, "PointerObject");
								this.nativePointerObject = prop;
								if (prop == null)
								{
									throw new InvalidOperationException("BuildPointer.PointerObject is null after EnableBuildMode");
								}
								this.LockNativeBuildInput(@object);
								this.UpdateNativeBuildPreview();
								string text = "native preview started: worldZone=";
								object prop2 = MoveStationsPlugin.GetProp(this.nativeWorldZone, "Id");
								this.Dbg(text + ((prop2 != null) ? prop2.ToString() : null) + " pointer=" + prop.GetType().Name);
								flag = true;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg("native preview start failed: " + MoveStationsPlugin.UnwrapInvocationError(ex));
				if (this.nativeBuildModeEnabled)
				{
					this.StopNativeBuildPreview();
				}
				else
				{
					this.RestoreOriginalAfterNativePreview();
				}
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000310C File Offset: 0x0000130C
		private void UpdateNativeBuildPreview()
		{
			try
			{
				global::UnityEngine.Object @object = this.buildControllerInstance;
				if (!(@object == null) && !(@object == null) && this.nativePointer != null)
				{
					this.LockNativeBuildInput(@object);
					if (this.mBcUpdatePointerAtPos != null)
					{
						object[] array;
						if (this.mBcUpdatePointerAtPos.GetParameters().Length < 2)
						{
							(array = new object[1])[0] = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
						}
						else
						{
							object[] array2 = new object[2];
							array2[0] = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
							array = array2;
							array2[1] = true;
						}
						object[] array3 = array;
						this.mBcUpdatePointerAtPos.Invoke(@object, array3);
					}
					object prop = MoveStationsPlugin.GetProp(this.nativePointer, "PointerObject");
					this.nativePointerObject = prop;
					if (prop != null)
					{
						if (this.mBpUpdateAvailability != null)
						{
							this.mBpUpdateAvailability.Invoke(this.nativePointer, null);
						}
						object prop2 = MoveStationsPlugin.GetProp(prop, "Target");
						object obj = MoveStationsPlugin.TryGet(this.fWgoData, prop2);
						object obj2 = ((this.pDataPosition != null) ? this.pDataPosition.GetValue(obj, null) : null);
						if (obj2 is Vector3)
						{
							Vector3 vector = (Vector3)obj2;
							if ((vector - this.previewPos).sqrMagnitude > 0.0001f)
							{
								this.previewPos = vector;
								this.Dbg("native cursor -> " + MoveStationsPlugin.Fmt(this.previewPos));
							}
						}
						bool flag = MoveStationsPlugin.ReadBoolField(this.fPointerShownAsActive, prop);
						this.spotValid = flag;
						this.spotReason = (flag ? "" : "rejected by the game's build rules");
						this.hasValidated = true;
						this.lastValidatedPos = this.previewPos;
					}
				}
			}
			catch (Exception ex)
			{
				this.spotValid = false;
				this.spotReason = "native build preview error";
				this.Dbg("native preview update failed: " + MoveStationsPlugin.UnwrapInvocationError(ex));
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00003334 File Offset: 0x00001534
		private void PlaceNativePreviewStation()
		{
			try
			{
				bool flag = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
				if (!this.spotValid && !flag)
				{
					this.Dbg("native click ignored: " + this.spotReason);
				}
				else
				{
					if (!this.spotValid && flag)
					{
						base.Logger.LogInfo("Force place at " + MoveStationsPlugin.Fmt(this.previewPos) + " - the game's native rules said: " + this.spotReason);
					}
					if (this.movingData != null && this.pDataPosition != null)
					{
						this.pDataPosition.SetValue(this.movingData, this.previewPos, null);
					}
					if (this.movingGo != null)
					{
						this.movingGo.transform.position = this.previewPos;
						this.movingGo.SetActive(this.movingGoWasActive);
					}
					this.RestoreOriginalTempFlag();
					this.RefreshAttachedGdPoints();
					this.ReRegisterInChunker();
					base.Logger.LogInfo("Station placed with native build rules at " + MoveStationsPlugin.Fmt(this.previewPos));
					this.ScheduleVerify(this.movingData, this.movingGo, this.previewPos, "native-place");
					this.EndMoveMode();
				}
			}
			catch (Exception ex)
			{
				base.Logger.LogWarning("Native place error: " + MoveStationsPlugin.UnwrapInvocationError(ex));
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000034B4 File Offset: 0x000016B4
		private void CancelNativeBuildPreview()
		{
			this.previewPos = this.originalPos;
			this.StopNativeBuildPreview();
			base.Logger.LogInfo("Move cancelled, station returned to " + MoveStationsPlugin.Fmt(this.originalPos));
			this.ScheduleVerify(this.movingData, this.movingGo, this.originalPos, "native-cancel");
			this.EndMoveMode();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00003518 File Offset: 0x00001718
		private void StopNativeBuildPreview()
		{
			if (!this.nativeBuildModeEnabled)
			{
				return;
			}
			try
			{
				if (this.mDisableBuildMode != null && this.buildControllerInstance != null)
				{
					this.mDisableBuildMode.Invoke(this.buildControllerInstance, null);
				}
			}
			catch (Exception ex)
			{
				this.Dbg("native preview disable failed: " + MoveStationsPlugin.UnwrapInvocationError(ex));
			}
			this.nativeBuildModeEnabled = false;
			this.nativePointer = null;
			this.nativePointerObject = null;
			this.nativeWorldZone = null;
			this.SetPlayerBuildControlTaken(false);
			this.RestoreOriginalTempFlag();
			if (this.movingGo != null && this.movingGoWasActive)
			{
				this.movingGo.SetActive(true);
			}
			this.nativeBuildPreview = false;
			this.nativeGridOnlyMode = false;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000035E4 File Offset: 0x000017E4
		private void RestoreOriginalAfterNativePreview()
		{
			this.RestoreOriginalTempFlag();
			if (this.movingGo != null)
			{
				this.movingGo.SetActive(this.movingGoWasActive);
			}
			this.nativeBuildModeEnabled = false;
			this.nativePointer = null;
			this.nativePointerObject = null;
			this.nativeWorldZone = null;
			this.nativeBuildPreview = false;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000363C File Offset: 0x0000183C
		private void RestoreOriginalTempFlag()
		{
			try
			{
				if (this.fWgoDataTemp != null && this.movingData != null)
				{
					this.fWgoDataTemp.SetValue(this.movingData, this.movingDataWasTemp);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00003690 File Offset: 0x00001890
		private bool TryStartNativeGridOnlyMode()
		{
			bool flag;
			try
			{
				if (this.nativeBuildModeEnabled)
				{
					this.StopNativeBuildPreview();
				}
				if (this.buildControllerType == null || this.buildDataType == null || this.worldZoneType == null || this.mBuildDataForRemove == null || this.mBcEnableBuildMode == null || this.fBcBuildPointer == null)
				{
					this.Dbg("native grid unavailable: remove BuildData/BuildController/WorldZone API missing");
					flag = false;
				}
				else
				{
					global::UnityEngine.Object @object = this.buildControllerInstance;
					if (@object == null || @object == null)
					{
						@object = MoveStationsPlugin.FindAnyUnityObject(this.buildControllerType);
						this.buildControllerInstance = @object;
					}
					if (@object == null)
					{
						flag = false;
					}
					else
					{
						object obj = this.FindBuildMenuWorldZone();
						if (obj == null)
						{
							this.Dbg("native grid unavailable: BuildManager.WorldZone not found");
							flag = false;
						}
						else
						{
							object obj2 = this.mBuildDataForRemove.Invoke(null, null);
							if (obj2 == null)
							{
								flag = false;
							}
							else
							{
								MethodBase methodBase = this.mBcEnableBuildMode;
								object obj3 = @object;
								object[] array = new object[4];
								array[0] = obj2;
								array[1] = obj;
								methodBase.Invoke(obj3, array);
								this.nativeBuildModeEnabled = true;
								this.SetPlayerBuildControlTaken(true);
								this.nativeGridOnlyMode = true;
								this.nativeWorldZone = obj;
								this.nativePointer = this.fBcBuildPointer.GetValue(@object);
								this.LockNativeBuildInput(@object);
								string text = "native grid opened immediately: worldZone=";
								object prop = MoveStationsPlugin.GetProp(obj, "Id");
								this.Dbg(text + ((prop != null) ? prop.ToString() : null));
								flag = true;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg("native grid start failed: " + MoveStationsPlugin.UnwrapInvocationError(ex));
				if (this.nativeBuildModeEnabled)
				{
					this.StopNativeBuildPreview();
				}
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00003844 File Offset: 0x00001A44
		private object FindBuildMenuWorldZone()
		{
			if (this.buildManagerType == null || this.pBmWorldZone == null)
			{
				return null;
			}
			object obj;
			try
			{
				global::UnityEngine.Object @object = global::UnityEngine.Object.FindObjectOfType(this.buildManagerType);
				if (@object == null)
				{
					obj = null;
				}
				else
				{
					obj = this.pBmWorldZone.GetValue(@object, null);
				}
			}
			catch (Exception ex)
			{
				this.Dbg("BuildManager.WorldZone lookup failed: " + ex.Message);
				obj = null;
			}
			return obj;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000038C8 File Offset: 0x00001AC8
		private void SetPlayerBuildControlTaken(bool taken)
		{
			try
			{
				if (taken != this.nativePlayerControlsTaken)
				{
					if (!(this.playerControllerType == null) && !(this.takenControlType == null) && !(this.mPcSetControlTakenType == null))
					{
						global::UnityEngine.Object @object = global::UnityEngine.Object.FindObjectOfType(this.playerControllerType);
						if (!(@object == null))
						{
							object obj = Enum.Parse(this.takenControlType, "ByBuilding");
							this.mPcSetControlTakenType.Invoke(@object, new object[]
							{
								obj,
								!taken
							});
							this.nativePlayerControlsTaken = taken;
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg("player build-control lock failed: " + MoveStationsPlugin.UnwrapInvocationError(ex));
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000398C File Offset: 0x00001B8C
		private void LockNativeBuildInput(global::UnityEngine.Object controller)
		{
			try
			{
				if (this.fBcInputLocked != null && controller != null)
				{
					this.fBcInputLocked.SetValue(controller, true);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000039D8 File Offset: 0x00001BD8
		private object FindWorldZoneForMovingWgo()
		{
			if (this.worldZoneType == null || this.movingWgo == null)
			{
				return null;
			}
			object obj2;
			try
			{
				global::UnityEngine.Object[] array = global::UnityEngine.Object.FindObjectsOfType(this.worldZoneType);
				object obj = null;
				foreach (global::UnityEngine.Object @object in array)
				{
					if (!(@object == null))
					{
						object prop = MoveStationsPlugin.GetProp(@object, "Wgos");
						if (prop is IEnumerable)
						{
							using (IEnumerator enumerator = ((IEnumerable)prop).GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									if ((global::UnityEngine.Object)enumerator.Current == this.movingWgo)
									{
										return @object;
									}
								}
							}
						}
						object prop2 = MoveStationsPlugin.GetProp(@object, "ZoneCollider");
						if (prop2 is Collider && ((Collider)prop2).bounds.Contains(this.originalPos))
						{
							obj = @object;
						}
					}
				}
				obj2 = obj;
			}
			catch (Exception ex)
			{
				this.Dbg("world zone lookup failed: " + ex.Message);
				obj2 = null;
			}
			return obj2;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003B14 File Offset: 0x00001D14
		private static bool ReadBoolField(FieldInfo field, object instance)
		{
			bool flag;
			try
			{
				flag = field != null && instance != null && field.GetValue(instance) is bool && (bool)field.GetValue(instance);
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00003B64 File Offset: 0x00001D64
		private Vector3 SnapToGrid(Vector3 p)
		{
			if (this.mSnapToBounds != null && this.buildControllerType != null)
			{
				try
				{
					if (this.buildControllerInstance == null || this.buildControllerInstance == null)
					{
						float realtimeSinceStartup = Time.realtimeSinceStartup;
						if (realtimeSinceStartup >= this.nextBcFind)
						{
							this.nextBcFind = realtimeSinceStartup + 2f;
							this.buildControllerInstance = MoveStationsPlugin.FindAnyUnityObject(this.buildControllerType);
						}
					}
					if (this.buildControllerInstance != null)
					{
						object obj = this.mSnapToBounds.Invoke(this.buildControllerInstance, new object[] { p });
						if (obj is Vector3)
						{
							Vector3 vector = (Vector3)obj;
							vector.y = p.y;
							float num = Mathf.Abs(vector.x - p.x);
							float num2 = Mathf.Abs(vector.z - p.z);
							if (num <= 1f && num2 <= 1f)
							{
								if (!this.snapLogged)
								{
									this.snapLogged = true;
									this.Dbg("snap: game snap accepted: " + MoveStationsPlugin.Fmt(p) + " -> " + MoveStationsPlugin.Fmt(vector));
								}
								return vector;
							}
							if (!this.snapRejectLogged)
							{
								this.snapRejectLogged = true;
								this.Dbg(string.Concat(new string[]
								{
									"snap: game snap REJECTED (moved too far): ",
									MoveStationsPlugin.Fmt(p),
									" -> ",
									MoveStationsPlugin.Fmt(vector),
									" (dx=",
									num.ToString("F2"),
									" dz=",
									num2.ToString("F2"),
									") -> fallback grid"
								}));
							}
						}
						else if (!this.snapRejectLogged)
						{
							this.snapRejectLogged = true;
							this.Dbg("snap: game snap returned no Vector3 -> fallback grid");
						}
					}
					else if (!this.snapRejectLogged)
					{
						this.snapRejectLogged = true;
						this.Dbg("snap: no BuildController instance found -> fallback grid");
					}
				}
				catch (Exception ex)
				{
					this.snapErrors++;
					if (this.snapErrors >= 3)
					{
						this.Dbg("snap: game snapping threw (" + ex.Message + ") -> fallback grid");
						this.mSnapToBounds = null;
					}
				}
			}
			if (!this.snapLogged)
			{
				this.snapLogged = true;
				this.Dbg(string.Concat(new string[]
				{
					"snap: fallback grid in use (",
					0.32f.ToString("F2"),
					" x ",
					0.3f.ToString("F2"),
					")"
				}));
			}
			p.x = Mathf.Round(p.x / 0.32f) * 0.32f;
			p.z = Mathf.Round(p.z / 0.3f) * 0.3f;
			return p;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00003E54 File Offset: 0x00002054
		private static global::UnityEngine.Object FindAnyUnityObject(Type t)
		{
			if (t == null)
			{
				return null;
			}
			try
			{
				if (MoveStationsPlugin.cachedAnyObj != null && MoveStationsPlugin.cachedAnyObj != null)
				{
					return MoveStationsPlugin.cachedAnyObj;
				}
			}
			catch
			{
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (realtimeSinceStartup < MoveStationsPlugin.nextAnyFind)
			{
				return null;
			}
			MoveStationsPlugin.nextAnyFind = realtimeSinceStartup + 10f;
			try
			{
				global::UnityEngine.Object @object = global::UnityEngine.Object.FindObjectOfType(t);
				if (@object != null)
				{
					MoveStationsPlugin.cachedAnyObj = @object;
					return @object;
				}
			}
			catch
			{
			}
			try
			{
				global::UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(t);
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != null)
						{
							MoveStationsPlugin.cachedAnyObj = array[i];
							return array[i];
						}
					}
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00003F3C File Offset: 0x0000213C
		private void ApplyPreviewPosition()
		{
			if (this.nativeBuildPreview)
			{
				return;
			}
			if (this.movingData != null && this.pDataPosition != null)
			{
				try
				{
					this.pDataPosition.SetValue(this.movingData, this.previewPos, null);
				}
				catch
				{
				}
			}
			if (this.movingGo != null)
			{
				try
				{
					this.movingGo.transform.position = this.previewPos;
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003FD0 File Offset: 0x000021D0
		private void PlaceStation()
		{
			if (this.nativeBuildPreview)
			{
				this.PlaceNativePreviewStation();
				return;
			}
			this.ApplyPreviewPosition();
			this.RefreshAttachedGdPoints();
			this.ReRegisterInChunker();
			this.UpdateSnapshotAfterPlace();
			base.Logger.LogInfo("Station placed at " + MoveStationsPlugin.Fmt(this.previewPos));
			this.ScheduleVerify(this.movingData, this.movingGo, this.previewPos, "place");
			this.EndMoveMode();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00004048 File Offset: 0x00002248
		private void UpdateSnapshotAfterPlace()
		{
			if (!this.snapValid || this.ownOffsets == null)
			{
				return;
			}
			try
			{
				if (this.ownCells != null)
				{
					foreach (Vector2Int vector2Int in this.ownCells)
					{
						this.snapCells.Remove(vector2Int);
					}
				}
				int num = Mathf.RoundToInt(this.previewPos.x / 0.32f);
				int num2 = Mathf.RoundToInt(this.previewPos.z / 0.3f);
				string text = ((!string.IsNullOrEmpty(this.movingWgoId)) ? this.movingWgoId : this.DefLabel(this.movingBuildingDef));
				foreach (Vector2Int vector2Int2 in this.ownOffsets)
				{
					this.snapCells[new Vector2Int(num + vector2Int2.x, num2 + vector2Int2.y)] = new MoveStationsPlugin.SnapCell
					{
						label = text,
						def = this.movingBuildingDef
					};
				}
				this.Dbg("snapshot updated: " + this.ownOffsets.Count.ToString() + " cells moved to " + MoveStationsPlugin.Fmt(this.previewPos));
			}
			catch (Exception ex)
			{
				this.Dbg("snapshot update error: " + ex.Message);
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00004208 File Offset: 0x00002408
		private void CancelMove()
		{
			if (this.nativeBuildPreview)
			{
				this.CancelNativeBuildPreview();
				return;
			}
			this.previewPos = this.originalPos;
			this.ApplyPreviewPosition();
			this.ReRegisterInChunker();
			base.Logger.LogInfo("Move cancelled, station returned to " + MoveStationsPlugin.Fmt(this.originalPos));
			this.ScheduleVerify(this.movingData, this.movingGo, this.originalPos, "cancel");
			this.EndMoveMode();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00004280 File Offset: 0x00002480
		private void EndMoveMode()
		{
			this.StopNativeBuildPreview();
			this.ReopenBuildMenu();
			this.moving = false;
			this.movingWgo = null;
			this.movingGo = null;
			this.movingData = null;
			this.movingBuildingDef = null;
			this.targetedWgo = null;
			this.requiredAreaId = "";
			this.hasValidated = false;
			this.ownCells = null;
			this.ownOffsets = null;
			this.lastGridResult = "";
			this.lastGridOobKey = "";
			this.selMismatchLogged = false;
			this.wgoCache = null;
			this.nextWgoRefresh = 0f;
			if (this.markerGO != null)
			{
				this.markerGO.SetActive(false);
			}
			this.SetHint("");
			this.nextScan = Time.realtimeSinceStartup + 0.3f;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00004348 File Offset: 0x00002548
		private void RotateStation()
		{
			if (this.mTryRotate == null || this.movingWgo == null)
			{
				return;
			}
			try
			{
				object obj = this.mTryRotate.Invoke(this.movingWgo, new object[] { false });
				this.Dbg("Rotate result: " + ((obj != null) ? obj.ToString() : null));
				if (this.nativeBuildPreview && this.nativePointer != null && this.mBpRotate != null)
				{
					this.mBpRotate.Invoke(this.nativePointer, null);
					this.Dbg("native preview rotated");
				}
			}
			catch (Exception ex)
			{
				this.Dbg("Rotate error: " + ex.Message);
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00004418 File Offset: 0x00002618
		private void ReRegisterInChunker()
		{
			if (this.pWgoRegistered == null || this.movingWgo == null)
			{
				return;
			}
			try
			{
				this.pWgoRegistered.SetValue(this.movingWgo, false, null);
				this.pWgoRegistered.SetValue(this.movingWgo, true, null);
			}
			catch (Exception ex)
			{
				this.Dbg("Chunker re-register error: " + ex.Message);
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000044A0 File Offset: 0x000026A0
		private void RefreshAttachedGdPoints()
		{
			try
			{
				if (!(this.movingWgo == null) && this.movingData != null && !(this.fWgoGdPointsData == null) && !(this.mWgoRegisterGdPoints == null))
				{
					ICollection collection = this.fWgoGdPointsData.GetValue(this.movingData) as ICollection;
					if (collection != null && collection.Count != 0)
					{
						this.mWgoRegisterGdPoints.Invoke(this.movingWgo, null);
						if (this.mWgoBindGdPointViews != null)
						{
							this.mWgoBindGdPointViews.Invoke(this.movingWgo, null);
						}
						this.Dbg("GD points refreshed after station move: " + collection.Count.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg("GD point refresh failed: " + MoveStationsPlugin.UnwrapInvocationError(ex));
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00004588 File Offset: 0x00002788
		private void ScheduleVerify(object data, GameObject go, Vector3 expected, string label)
		{
			this.verifyActive = true;
			this.verifyData = data;
			this.verifyGo = go;
			this.verifyExpected = expected;
			this.verifyLabel = label;
			this.verifyTime = Time.realtimeSinceStartup + 2f;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000045C0 File Offset: 0x000027C0
		private void UpdateVerify()
		{
			if (!this.verifyActive || Time.realtimeSinceStartup < this.verifyTime)
			{
				return;
			}
			this.verifyActive = false;
			try
			{
				Vector3 vector = this.verifyExpected;
				bool flag = false;
				if (this.verifyData != null && this.pDataPosition != null)
				{
					object value = this.pDataPosition.GetValue(this.verifyData, null);
					if (value is Vector3)
					{
						vector = (Vector3)value;
						flag = true;
					}
				}
				Vector3 position = this.verifyExpected;
				bool flag2 = false;
				if (this.verifyGo != null)
				{
					position = this.verifyGo.transform.position;
					flag2 = true;
				}
				float magnitude = (vector - this.verifyExpected).magnitude;
				float magnitude2 = (position - this.verifyExpected).magnitude;
				this.Dbg(string.Concat(new string[]
				{
					"persist[",
					this.verifyLabel,
					"]: expected=",
					MoveStationsPlugin.Fmt(this.verifyExpected),
					" data=",
					flag ? MoveStationsPlugin.Fmt(vector) : "-",
					"(",
					magnitude.ToString("F2"),
					"m) transform=",
					flag2 ? MoveStationsPlugin.Fmt(position) : "-",
					"(",
					magnitude2.ToString("F2"),
					"m) -> ",
					(magnitude < 0.06f && magnitude2 < 0.06f) ? "PERSISTED" : "REVERTED?"
				}));
			}
			catch (Exception ex)
			{
				this.Dbg("persist check error: " + ex.Message);
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000478C File Offset: 0x0000298C
		private bool IsBuildModeActive()
		{
			try
			{
				if (this.fBcBuildModeActive == null || this.buildControllerType == null)
				{
					return false;
				}
				if (this.buildControllerInstance == null || this.buildControllerInstance == null)
				{
					float realtimeSinceStartup = Time.realtimeSinceStartup;
					if (realtimeSinceStartup < this.nextBcFind)
					{
						return false;
					}
					this.nextBcFind = realtimeSinceStartup + 2f;
					this.buildControllerInstance = MoveStationsPlugin.FindAnyUnityObject(this.buildControllerType);
				}
				if (this.buildControllerInstance == null)
				{
					return false;
				}
				object obj = MoveStationsPlugin.TryGet(this.fBcBuildModeActive, this.buildControllerInstance);
				if (obj is bool)
				{
					return (bool)obj;
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00004858 File Offset: 0x00002A58
		private void GridStatusOnce(string msg)
		{
			if (this.gridStatusMsg == msg)
			{
				return;
			}
			this.gridStatusMsg = msg;
			this.Dbg(msg);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00004878 File Offset: 0x00002A78
		private void TrySnapshotGrid(bool force)
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (!force && realtimeSinceStartup < this.nextSnapTry)
			{
				return;
			}
			if (force)
			{
				this.gridRejectLogged = false;
				this.snapDumpDone = false;
				this.selCaptured = false;
			}
			bool flag = this.IsBuildModeActive();
			if (!this.buildModeSeen || flag != this.lastBuildModeActive)
			{
				this.buildModeSeen = true;
				this.lastBuildModeActive = flag;
				this.Dbg(string.Concat(new string[]
				{
					"buildmode: active=",
					flag.ToString(),
					" (snapshot ",
					this.snapValid ? ("cached " + this.snapCells.Count.ToString() + " cells") : "not captured yet",
					")"
				}));
			}
			if (!flag && !force)
			{
				this.nextSnapTry = realtimeSinceStartup + 0.5f;
				return;
			}
			if (this.snapRejected && !force)
			{
				this.nextSnapTry = realtimeSinceStartup + 5f;
				return;
			}
			if (this.snapValid && !force && realtimeSinceStartup < this.lastSnapRefresh + 2f)
			{
				this.nextSnapTry = realtimeSinceStartup + 2f;
				return;
			}
			this.nextSnapTry = realtimeSinceStartup + (this.snapRejected ? 5f : 0.5f);
			object obj;
			bool flag2;
			string text;
			this.FindBuildGrid(out obj, out flag2, out text);
			if (obj == null)
			{
				this.GridStatusOnce("grid: no build-grid object found yet (open the Build menu once; polling continues)");
				return;
			}
			this.BuildSnapshot(obj, flag2, text, flag, force);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000049D8 File Offset: 0x00002BD8
		private void FindBuildGrid(out object grid, out bool active, out string src)
		{
			grid = null;
			active = false;
			src = "";
			try
			{
				if (this.cachedGridObj != null && this.cachedGridObj != null)
				{
					grid = this.cachedGridObj;
					active = MoveStationsPlugin.ComponentActive(grid);
					src = "cached";
					return;
				}
			}
			catch
			{
			}
			this.cachedGridObj = null;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (realtimeSinceStartup < this.nextGridFind)
			{
				return;
			}
			this.nextGridFind = realtimeSinceStartup + 2f;
			this.FindBuildGridRaw(out grid, out active, out src);
			try
			{
				this.cachedGridObj = grid as global::UnityEngine.Object;
			}
			catch
			{
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00004A88 File Offset: 0x00002C88
		private void FindBuildGridRaw(out object grid, out bool active, out string src)
		{
			grid = null;
			active = false;
			src = "";
			try
			{
				if (this.buildGridDataType != null)
				{
					global::UnityEngine.Object @object = global::UnityEngine.Object.FindObjectOfType(this.buildGridDataType);
					if (@object != null)
					{
						grid = @object;
						active = true;
						src = "BuildGridData";
						return;
					}
				}
			}
			catch
			{
			}
			try
			{
				if (this.buildControllerType != null && this.fBcLayout != null)
				{
					global::UnityEngine.Object object2 = global::UnityEngine.Object.FindObjectOfType(this.buildControllerType);
					if (object2 != null)
					{
						this.buildControllerInstance = object2;
						object obj = MoveStationsPlugin.TryGet(this.fBcLayout, object2);
						object obj2 = ((obj != null && this.fLayoutGridData != null) ? MoveStationsPlugin.TryGet(this.fLayoutGridData, obj) : null);
						if (obj2 != null)
						{
							grid = obj2;
							active = MoveStationsPlugin.ComponentActive(obj2);
							src = "BuildController.buildLayout";
							return;
						}
					}
				}
			}
			catch
			{
			}
			try
			{
				if (this.buildLayoutType != null && this.fLayoutGridData != null)
				{
					global::UnityEngine.Object[] array = global::UnityEngine.Object.FindObjectsOfType(this.buildLayoutType);
					if (array != null)
					{
						foreach (global::UnityEngine.Object object3 in array)
						{
							object obj3 = MoveStationsPlugin.TryGet(this.fLayoutGridData, object3);
							if (obj3 != null)
							{
								grid = obj3;
								active = MoveStationsPlugin.ComponentActive(obj3);
								src = "BuildLayout";
								return;
							}
						}
					}
				}
			}
			catch
			{
			}
			try
			{
				if (this.buildGridDataType != null)
				{
					global::UnityEngine.Object[] array3 = Resources.FindObjectsOfTypeAll(this.buildGridDataType);
					if (array3 != null)
					{
						foreach (global::UnityEngine.Object object4 in array3)
						{
							Component component = object4 as Component;
							if (!(component == null) && component.hideFlags == HideFlags.None)
							{
								try
								{
									if (!component.gameObject.scene.IsValid())
									{
										goto IL_01D6;
									}
								}
								catch
								{
									goto IL_01D6;
								}
								grid = object4;
								active = component.gameObject.activeInHierarchy;
								src = "BuildGridData(inactive-scan)";
								return;
							}
							IL_01D6:;
						}
					}
				}
			}
			catch
			{
			}
			try
			{
				if (this.buildLayoutType != null && this.fLayoutGridData != null)
				{
					global::UnityEngine.Object[] array4 = Resources.FindObjectsOfTypeAll(this.buildLayoutType);
					if (array4 != null)
					{
						foreach (global::UnityEngine.Object object5 in array4)
						{
							Component component2 = object5 as Component;
							if (!(component2 == null) && component2.hideFlags == HideFlags.None)
							{
								try
								{
									if (!component2.gameObject.scene.IsValid())
									{
										goto IL_0299;
									}
								}
								catch
								{
									goto IL_0299;
								}
								object obj4 = MoveStationsPlugin.TryGet(this.fLayoutGridData, object5);
								if (obj4 != null)
								{
									grid = obj4;
									active = component2.gameObject.activeInHierarchy;
									src = "BuildLayout(inactive-scan)";
									break;
								}
							}
							IL_0299:;
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00004D9C File Offset: 0x00002F9C
		private static bool ComponentActive(object o)
		{
			bool flag;
			try
			{
				Component component = o as Component;
				if (component == null)
				{
					flag = true;
				}
				else
				{
					flag = component.gameObject.activeInHierarchy;
				}
			}
			catch
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00004DE4 File Offset: 0x00002FE4
		private void BuildSnapshot(object gridObj, bool active, string src, bool bm, bool forceLog)
		{
			try
			{
				Array array = MoveStationsPlugin.TryGet(this.fGridGridData, gridObj) as Array;
				if (array == null)
				{
					string text = this.ReadEnumField(this.fGridBuildMode, gridObj);
					string text2 = MoveStationsPlugin.ReadFieldState(this.fGridSelection, gridObj);
					this.GridStatusOnce(string.Concat(new string[]
					{
						"grid: object found (",
						src,
						") but gridData array is null [buildModeField=",
						text,
						" selectionGridData=",
						text2,
						" objActive=",
						active.ToString(),
						" buildMode=",
						bm.ToString(),
						"]"
					}));
				}
				else
				{
					int length = array.GetLength(0);
					int length2 = array.GetLength(1);
					if (length <= 0 || length2 <= 0)
					{
						this.GridStatusOnce(string.Concat(new string[]
						{
							"grid: found grid (",
							src,
							") but empty dims=",
							length.ToString(),
							"x",
							length2.ToString()
						}));
					}
					else
					{
						this.BuildSelectionSnapshot(gridObj, forceLog);
						bool flag = (long)length * (long)length2 > 120000L;
						Dictionary<Vector2Int, MoveStationsPlugin.SnapCell> dictionary = new Dictionary<Vector2Int, MoveStationsPlugin.SnapCell>();
						int num = int.MaxValue;
						int num2 = int.MinValue;
						int num3 = int.MaxValue;
						int num4 = int.MinValue;
						int num5 = 0;
						for (int i = 0; i < length; i++)
						{
							bool flag2 = false;
							for (int j = 0; j < length2; j++)
							{
								if (flag && num5 >= 120000)
								{
									flag2 = true;
									break;
								}
								num5++;
								object value = array.GetValue(i, j);
								if (value != null)
								{
									object value2 = this.fCellCoords.GetValue(value);
									if (value2 is Vector3)
									{
										Vector3 vector = (Vector3)value2;
										if (vector.x != 0f || vector.z != 0f)
										{
											int num6 = MoveStationsPlugin.CellX(vector.x);
											int num7 = MoveStationsPlugin.CellZ(vector.z);
											if (num6 < num)
											{
												num = num6;
											}
											if (num6 > num2)
											{
												num2 = num6;
											}
											if (num7 < num3)
											{
												num3 = num7;
											}
											if (num7 > num4)
											{
												num4 = num7;
											}
											object value3 = this.fCellDef.GetValue(value);
											if (value3 != null)
											{
												dictionary[new Vector2Int(num6, num7)] = new MoveStationsPlugin.SnapCell
												{
													label = this.DefLabel(value3),
													def = value3
												};
											}
										}
									}
								}
							}
							if (flag2)
							{
								break;
							}
						}
						if (dictionary.Count == 0)
						{
							this.GridStatusOnce(string.Concat(new string[]
							{
								"grid: found grid (",
								src,
								") dims=",
								length.ToString(),
								"x",
								length2.ToString(),
								" but 0 occupied cells (snapValid=",
								this.snapValid.ToString(),
								")"
							}));
						}
						else
						{
							if (this.debug && !this.snapDumpDone)
							{
								this.snapDumpDone = true;
								this.DumpGridDiagnostics(array, gridObj);
							}
							string text3;
							int num8;
							int num9;
							this.ValidateSnapshotWorld(dictionary, out text3, out num8, out num9);
							if (text3.Length > 0)
							{
								this.Dbg("grid world-probe:" + text3);
							}
							if (num8 >= 3 && num9 == 0)
							{
								this.snapValid = false;
								this.snapRejected = true;
								this.snapRejectReason = string.Concat(new string[]
								{
									"the captured grid is not world occupancy (",
									num9.ToString(),
									"/",
									num8.ToString(),
									" placed objects matched)"
								});
								if (!this.gridRejectLogged)
								{
									this.gridRejectLogged = true;
									this.Dbg("grid snapshot REJECTED: " + this.snapRejectReason + " -> collider rules in use");
									if (this.debug)
									{
										base.Logger.LogInfo("Build grid: rejected the captured grid - " + this.snapRejectReason + ". Collider rules are used instead.");
									}
								}
							}
							else
							{
								this.snapRejected = false;
								this.snapRejectReason = "";
								this.gridRejectLogged = false;
								bool flag3 = !this.snapValid || dictionary.Count != this.snapCountLogged;
								this.snapCells = dictionary;
								this.snapValid = true;
								this.snapSource = src;
								this.snapMinX = num;
								this.snapMaxX = num2;
								this.snapMinZ = num3;
								this.snapMaxZ = num4;
								this.lastSnapRefresh = Time.realtimeSinceStartup;
								this.gridStatusMsg = "";
								if (flag3 || forceLog)
								{
									this.snapCountLogged = dictionary.Count;
									this.Dbg(string.Concat(new string[]
									{
										"grid snapshot: occupied=",
										dictionary.Count.ToString(),
										" dims=",
										length.ToString(),
										"x",
										length2.ToString(),
										flag ? (" (scan capped at " + 120000.ToString() + ")") : "",
										" extentX=[",
										num.ToString(),
										"..",
										num2.ToString(),
										"] extentZ=[",
										num3.ToString(),
										"..",
										num4.ToString(),
										"] active=",
										active.ToString(),
										" buildmode=",
										bm.ToString(),
										" src=",
										src
									}));
									if (this.debug)
									{
										base.Logger.LogInfo(string.Concat(new string[]
										{
											"Build grid cached: ",
											dictionary.Count.ToString(),
											" occupied cells (dims ",
											length.ToString(),
											"x",
											length2.ToString(),
											", extentX=[",
											num.ToString(),
											"..",
											num2.ToString(),
											"], extentZ=[",
											num3.ToString(),
											"..",
											num4.ToString(),
											"], src=",
											src,
											")."
										}));
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.GridStatusOnce("grid snapshot error: " + ex.Message);
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00005450 File Offset: 0x00003650
		private string ReadEnumField(FieldInfo f, object o)
		{
			string text;
			try
			{
				if (f == null || o == null)
				{
					text = "?";
				}
				else
				{
					object value = f.GetValue(o);
					text = ((value == null) ? "null" : value.ToString());
				}
			}
			catch
			{
				text = "?";
			}
			return text;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000054A8 File Offset: 0x000036A8
		private static string ReadFieldState(FieldInfo f, object o)
		{
			string text;
			try
			{
				if (f == null || o == null)
				{
					text = "?";
				}
				else
				{
					text = ((f.GetValue(o) == null) ? "null" : "set");
				}
			}
			catch
			{
				text = "?";
			}
			return text;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000054FC File Offset: 0x000036FC
		private object DefFor(object data)
		{
			try
			{
				if (data == null || this.defType == null)
				{
					return null;
				}
				object obj = MoveStationsPlugin.FindReferenceOfType(data, this.defType);
				if (obj == null)
				{
					return null;
				}
				if (this.mTryGetBDef != null && this.bDefType != null)
				{
					object[] array = new object[1];
					object obj2 = null;
					try
					{
						obj2 = this.mTryGetBDef.Invoke(obj, array);
					}
					catch
					{
					}
					if (obj2 is bool && (bool)obj2 && array[0] != null)
					{
						return array[0];
					}
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000055AC File Offset: 0x000037AC
		private void ValidateSnapshotWorld(Dictionary<Vector2Int, MoveStationsPlugin.SnapCell> map, out string report, out int probes, out int matched)
		{
			report = "";
			probes = 0;
			matched = 0;
			try
			{
				if (map != null && !(this.wgoType == null) && !(this.fWgoData == null) && !(this.pDataPosition == null))
				{
					foreach (global::UnityEngine.Object @object in global::UnityEngine.Object.FindObjectsOfType(this.wgoType))
					{
						if (!(@object == null))
						{
							if (probes >= 6)
							{
								break;
							}
							object obj = MoveStationsPlugin.TryGet(this.fWgoData, @object);
							if (obj != null)
							{
								object obj2 = this.DefFor(obj);
								if (obj2 != null)
								{
									string text = this.WgoIdRaw(obj2);
									if (!string.IsNullOrEmpty(text))
									{
										object value = this.pDataPosition.GetValue(obj, null);
										if (value is Vector3)
										{
											Vector3 vector = (Vector3)value;
											int num = MoveStationsPlugin.CellX(vector.x);
											int num2 = MoveStationsPlugin.CellZ(vector.z);
											string text2 = "(none)";
											MoveStationsPlugin.SnapCell snapCell;
											bool flag = map.TryGetValue(new Vector2Int(num, num2), out snapCell);
											if (flag && snapCell != null && !string.IsNullOrEmpty(snapCell.label))
											{
												text2 = snapCell.label;
											}
											bool flag2 = flag && snapCell != null && string.Equals(snapCell.label, text, StringComparison.OrdinalIgnoreCase);
											probes++;
											if (flag2)
											{
												matched++;
											}
											report = string.Concat(new string[]
											{
												report,
												"\n  probe: '",
												MoveStationsPlugin.FriendlyFromId(text),
												"' id=",
												text,
												" at ",
												MoveStationsPlugin.Fmt(vector),
												" cell(",
												num.ToString(),
												",",
												num2.ToString(),
												") gridDef='",
												text2,
												"' ",
												flag2 ? "MATCH" : "MISMATCH"
											});
										}
									}
								}
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000057C4 File Offset: 0x000039C4
		private void DumpGridDiagnostics(Array arr, object gridObj)
		{
			try
			{
				this.Dbg(string.Concat(new string[]
				{
					"grid selection: currentBuildingDef='",
					this.DefLabel(MoveStationsPlugin.TryGet(this.fGridCurrentDef, gridObj)),
					"' buildMode=",
					this.ReadEnumField(this.fGridBuildMode, gridObj),
					" busySlots=",
					this.CountOf(this.fGridBusySlots, gridObj),
					"(",
					MoveStationsPlugin.ElemTypeName(this.fGridBusySlots),
					") freeSlots=",
					this.CountOf(this.fGridFreeSlots, gridObj),
					"(",
					MoveStationsPlugin.ElemTypeName(this.fGridFreeSlots),
					")"
				}));
				if (arr == null)
				{
					this.Dbg("grid array: null");
				}
				else
				{
					int length = arr.GetLength(0);
					int length2 = arr.GetLength(1);
					this.Dbg(string.Concat(new string[]
					{
						"grid array: ",
						length.ToString(),
						"x",
						length2.ToString(),
						" stateField=",
						(this.fCellState != null).ToString()
					}));
					if (!(this.fCellState == null))
					{
						Dictionary<int, int> dictionary = new Dictionary<int, int>();
						int num = 0;
						int num2 = 0;
						int num3 = 0;
						int num4 = 0;
						while (num4 < length && num3 < 40000)
						{
							int num5 = 0;
							while (num5 < length2 && num3 < 40000)
							{
								num3++;
								object value = arr.GetValue(num4, num5);
								if (value != null)
								{
									int num6 = MoveStationsPlugin.ReadInt(this.fCellState, value);
									int num7;
									dictionary.TryGetValue(num6, out num7);
									dictionary[num6] = num7 + 1;
									if (MoveStationsPlugin.TryGet(this.fCellDef, value) != null)
									{
										num++;
									}
									object obj = ((this.fCellCoords != null) ? this.fCellCoords.GetValue(value) : null);
									if (obj is Vector3)
									{
										Vector3 vector = (Vector3)obj;
										if (vector.x != 0f || vector.z != 0f)
										{
											num2++;
										}
									}
								}
								num5++;
							}
							num4++;
						}
						List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>(dictionary);
						list.Sort((KeyValuePair<int, int> a, KeyValuePair<int, int> b) => b.Value.CompareTo(a.Value));
						string text = "";
						int num8 = 0;
						foreach (KeyValuePair<int, int> keyValuePair in list)
						{
							if (num8 >= 8)
							{
								break;
							}
							num8++;
							text = string.Concat(new string[]
							{
								text,
								(num8 > 1) ? ", " : "",
								"state=",
								keyValuePair.Key.ToString(),
								":",
								keyValuePair.Value.ToString()
							});
						}
						this.Dbg(string.Concat(new string[]
						{
							"grid cell states (scanned=",
							num3.ToString(),
							" filled=",
							num.ToString(),
							" withCoords=",
							num2.ToString(),
							"): ",
							text
						}));
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg("grid diagnostics error: " + ex.Message);
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00005B6C File Offset: 0x00003D6C
		private void BuildSelectionSnapshot(object gridObj, bool force)
		{
			try
			{
				if (!this.selCaptured || force)
				{
					float realtimeSinceStartup = Time.realtimeSinceStartup;
					if (force || realtimeSinceStartup >= this.nextSelTry)
					{
						this.nextSelTry = realtimeSinceStartup + 3f;
						if (!(this.fSelState == null))
						{
							Array array = MoveStationsPlugin.TryGet(this.fGridSelection, gridObj) as Array;
							if (array == null)
							{
								this.selCalib = "(no selection array)";
							}
							else
							{
								int length = array.GetLength(0);
								int length2 = array.GetLength(1);
								if (length <= 0 || length2 <= 0)
								{
									this.selCalib = "(empty selection array)";
								}
								else
								{
									this.selDefName = this.DefLabel(MoveStationsPlugin.TryGet(this.fGridCurrentDef, gridObj));
									Dictionary<Vector2Int, MoveStationsPlugin.SelCell> dictionary = new Dictionary<Vector2Int, MoveStationsPlugin.SelCell>();
									int num = 0;
									int num2 = 0;
									for (int i = 0; i < length; i++)
									{
										for (int j = 0; j < length2; j++)
										{
											object value = array.GetValue(i, j);
											if (value != null)
											{
												object obj = ((this.fSelCoords != null) ? this.fSelCoords.GetValue(value) : null);
												if (obj is Vector3)
												{
													Vector3 vector = (Vector3)obj;
													if (vector.x != 0f || vector.z != 0f)
													{
														num++;
														Vector2Int vector2Int = new Vector2Int(MoveStationsPlugin.CellX(vector.x), MoveStationsPlugin.CellZ(vector.z));
														if (dictionary.ContainsKey(vector2Int))
														{
															num2++;
														}
														MoveStationsPlugin.SelCell selCell = new MoveStationsPlugin.SelCell();
														selCell.state = MoveStationsPlugin.ReadInt(this.fSelState, value);
														selCell.coords = vector;
														dictionary[vector2Int] = selCell;
													}
												}
											}
										}
									}
									this.Dbg(string.Concat(new string[]
									{
										"sel map: cellsWithCoords=",
										num.ToString(),
										" keys=",
										dictionary.Count.ToString(),
										" duplicates=",
										num2.ToString(),
										" array=",
										length.ToString(),
										"x",
										length2.ToString(),
										" selectedDef='",
										this.selDefName,
										"'"
									}));
									if (dictionary.Count < 200)
									{
										this.selCalib = "(selection coords empty: cellsWithCoords=" + num.ToString() + ")";
									}
									else
									{
										this.selStates = dictionary;
										Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
										foreach (MoveStationsPlugin.SelCell selCell2 in dictionary.Values)
										{
											int num3;
											dictionary2.TryGetValue(selCell2.state, out num3);
											dictionary2[selCell2.state] = num3 + 1;
										}
										List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>(dictionary2);
										list.Sort((KeyValuePair<int, int> a, KeyValuePair<int, int> b) => b.Value.CompareTo(a.Value));
										string text = "";
										int num4 = 0;
										foreach (KeyValuePair<int, int> keyValuePair in list)
										{
											if (num4 >= 8)
											{
												break;
											}
											num4++;
											text = string.Concat(new string[]
											{
												text,
												(num4 > 1) ? ", " : "",
												"state=",
												keyValuePair.Key.ToString(),
												":",
												keyValuePair.Value.ToString()
											});
										}
										this.Dbg("sel map states: " + text);
										object obj2 = ((this.buildControllerInstance != null) ? MoveStationsPlugin.TryGet(this.fBcCurPos, this.buildControllerInstance) : null);
										if (obj2 is Vector3)
										{
											Vector3 vector2 = (Vector3)obj2;
											this.Dbg(string.Concat(new string[]
											{
												"sel map: build cursor at ",
												MoveStationsPlugin.Fmt(vector2),
												" cell(",
												MoveStationsPlugin.CellX(vector2.x).ToString(),
												",",
												MoveStationsPlugin.CellZ(vector2.z).ToString(),
												") state=",
												this.SelStateAtCell(vector2).ToString()
											}));
										}
										this.CalibrateSelection();
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.selCalib = "(selection read error: " + ex.Message + ")";
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00006054 File Offset: 0x00004254
		private void CalibrateSelection()
		{
			try
			{
				if (this.selStates == null || this.selStates.Count == 0)
				{
					this.selCalib = "(no map)";
				}
				else
				{
					List<Vector2Int> list = new List<Vector2Int>();
					if (this.wgoType != null && this.fWgoData != null && this.pDataPosition != null)
					{
						foreach (global::UnityEngine.Object @object in global::UnityEngine.Object.FindObjectsOfType(this.wgoType))
						{
							if (!(@object == null))
							{
								if (list.Count >= 80)
								{
									break;
								}
								object obj = MoveStationsPlugin.TryGet(this.fWgoData, @object);
								if (obj != null && this.DefFor(obj) != null)
								{
									object value = this.pDataPosition.GetValue(obj, null);
									if (value is Vector3)
									{
										Vector3 vector = (Vector3)value;
										list.Add(new Vector2Int(MoveStationsPlugin.CellX(vector.x), MoveStationsPlugin.CellZ(vector.z)));
									}
								}
							}
						}
					}
					List<Vector2Int> list2 = new List<Vector2Int>();
					if (this.buildAreaType != null)
					{
						foreach (global::UnityEngine.Object object2 in global::UnityEngine.Object.FindObjectsOfType(this.buildAreaType))
						{
							if (!(object2 == null))
							{
								if (list2.Count >= 250)
								{
									break;
								}
								Component component = object2 as Component;
								if (!(component == null))
								{
									Collider component2 = component.GetComponent<Collider>();
									if (!(component2 == null))
									{
										Bounds bounds = component2.bounds;
										if (bounds.size.x > 0.01f && bounds.size.z > 0.01f)
										{
											int num = MoveStationsPlugin.CellX(bounds.min.x);
											int num2 = MoveStationsPlugin.CellX(bounds.max.x);
											int num3 = MoveStationsPlugin.CellZ(bounds.min.z);
											int num4 = MoveStationsPlugin.CellZ(bounds.max.z);
											int num5 = num;
											while (num5 <= num2 && list2.Count < 250)
											{
												int num6 = num3;
												while (num6 <= num4 && list2.Count < 250)
												{
													Vector2Int vector2Int = new Vector2Int(num5, num6);
													if (this.selStates.ContainsKey(vector2Int) && !list.Contains(vector2Int))
													{
														bool flag = false;
														try
														{
															flag = Physics.CheckBox(new Vector3((float)num5 * 0.32f, 6.01f, (float)num6 * 0.3f) + new Vector3(0f, 0.5f, 0f), new Vector3(0.384f, 0.6f, 0.36f), Quaternion.identity, 524544, QueryTriggerInteraction.Collide);
														}
														catch
														{
														}
														if (!flag)
														{
															list2.Add(vector2Int);
														}
													}
													num6++;
												}
												num5++;
											}
										}
									}
								}
							}
						}
					}
					if (list.Count < 10 || list2.Count < 15)
					{
						string[] array2 = new string[5];
						array2[0] = "(too few samples: occupied=";
						int num7 = 1;
						int i = list.Count;
						array2[num7] = i.ToString();
						array2[2] = " free=";
						int num8 = 3;
						i = list2.Count;
						array2[num8] = i.ToString();
						array2[4] = ")";
						this.selCalib = string.Concat(array2);
						this.Dbg("sel calibration: " + this.selCalib);
					}
					else
					{
						HashSet<int> hashSet = new HashSet<int>();
						foreach (MoveStationsPlugin.SelCell selCell in this.selStates.Values)
						{
							hashSet.Add(selCell.state);
						}
						int num9 = int.MinValue;
						double num10 = -1.0;
						int num11 = int.MinValue;
						double num12 = -1.0;
						string text = "";
						foreach (int num13 in hashSet)
						{
							int num14 = 0;
							int num15 = 0;
							foreach (Vector2Int vector2Int2 in list)
							{
								MoveStationsPlugin.SelCell selCell2;
								if (this.selStates.TryGetValue(vector2Int2, out selCell2) && selCell2.state == num13)
								{
									num14++;
								}
							}
							foreach (Vector2Int vector2Int3 in list2)
							{
								MoveStationsPlugin.SelCell selCell3;
								if (this.selStates.TryGetValue(vector2Int3, out selCell3) && selCell3.state == num13)
								{
									num15++;
								}
							}
							double num16 = (double)num14 / (double)list.Count;
							double num17 = (double)num15 / (double)list2.Count;
							if (text.Length < 1500)
							{
								string[] array3 = new string[16];
								array3[0] = text;
								array3[1] = "\n  sel value ";
								array3[2] = num13.ToString();
								array3[3] = ": occupied ";
								array3[4] = num16.ToString("F2");
								array3[5] = " (";
								array3[6] = num14.ToString();
								array3[7] = "/";
								int num18 = 8;
								int i = list.Count;
								array3[num18] = i.ToString();
								array3[9] = "), free ";
								array3[10] = num17.ToString("F2");
								array3[11] = " (";
								array3[12] = num15.ToString();
								array3[13] = "/";
								int num19 = 14;
								i = list2.Count;
								array3[num19] = i.ToString();
								array3[15] = ")";
								text = string.Concat(array3);
							}
							if (num17 > num10)
							{
								num10 = num17;
								num9 = num13;
							}
							if (num16 > num12)
							{
								num12 = num16;
								num11 = num13;
							}
						}
						this.Dbg("sel calibration table:" + text);
						this.selBlockedValue = num11;
						if (num9 == num11 || num10 < 0.6 || num12 > 0.3)
						{
							this.selCalib = string.Concat(new string[]
							{
								"(no clean free state: bestFree=",
								num9.ToString(),
								" rate=",
								num10.ToString("F2"),
								" bestBlocked=",
								num11.ToString(),
								" rate=",
								num12.ToString("F2"),
								")"
							});
							this.Dbg("sel calibration: " + this.selCalib + " -> game override disabled");
						}
						else
						{
							this.selFreeValue = num9;
							this.selOverrideReady = true;
							this.selCaptured = true;
							string[] array4 = new string[13];
							array4[0] = "(free=";
							array4[1] = this.selFreeValue.ToString();
							array4[2] = " rate=";
							array4[3] = num10.ToString("F2");
							array4[4] = " blocked=";
							array4[5] = this.selBlockedValue.ToString();
							array4[6] = " occRate=";
							array4[7] = num12.ToString("F2");
							array4[8] = " samples free=";
							int num20 = 9;
							int i = list2.Count;
							array4[num20] = i.ToString();
							array4[10] = " occupied=";
							int num21 = 11;
							i = list.Count;
							array4[num21] = i.ToString();
							array4[12] = ")";
							this.selCalib = string.Concat(array4);
							if (this.debug)
							{
								base.Logger.LogInfo(string.Concat(new string[] { "Game cell map read: ", this.selCalib, " selectedDef='", this.selDefName, "'" }));
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.selCalib = "(calibration error: " + ex.Message + ")";
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00006894 File Offset: 0x00004A94
		private int SelStateAtCell(Vector3 pos)
		{
			if (this.selStates == null)
			{
				return int.MinValue;
			}
			int num = MoveStationsPlugin.CellX(pos.x);
			int num2 = MoveStationsPlugin.CellZ(pos.z);
			MoveStationsPlugin.SelCell selCell = null;
			float num3 = float.MaxValue;
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					MoveStationsPlugin.SelCell selCell2;
					if (this.selStates.TryGetValue(new Vector2Int(num + i, num2 + j), out selCell2))
					{
						float num4 = Mathf.Abs(selCell2.coords.x - pos.x) + Mathf.Abs(selCell2.coords.z - pos.z);
						if (num4 < num3)
						{
							num3 = num4;
							selCell = selCell2;
						}
					}
				}
			}
			if (selCell == null)
			{
				return int.MinValue;
			}
			if (num3 > 0.62f)
			{
				return int.MinValue;
			}
			return selCell.state;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00006968 File Offset: 0x00004B68
		private bool SelOverrideApplies()
		{
			if (!this.selOverrideReady)
			{
				return false;
			}
			if (string.IsNullOrEmpty(this.movingWgoId) || string.IsNullOrEmpty(this.selDefName))
			{
				return false;
			}
			string text = this.selDefName;
			int num = text.IndexOf("_place", StringComparison.OrdinalIgnoreCase);
			if (num > 0)
			{
				text = text.Substring(0, num);
			}
			return text.Length != 0 && string.Equals(text, this.movingWgoId, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000069D3 File Offset: 0x00004BD3
		private static int CellX(float x)
		{
			return Mathf.RoundToInt(x / 0.32f);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000069E1 File Offset: 0x00004BE1
		private static int CellZ(float z)
		{
			return Mathf.RoundToInt(z / 0.3f);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000069EF File Offset: 0x00004BEF
		private static float MarginFor(float size, float grid)
		{
			return Mathf.Min(grid * 0.95f, size * 0.3f);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00006A04 File Offset: 0x00004C04
		private bool OverlapsAfterMargin(Collider c, Vector3 pos, out string info)
		{
			info = "";
			bool flag;
			try
			{
				if (c == null)
				{
					flag = false;
				}
				else
				{
					Bounds bounds = c.bounds;
					float num = MoveStationsPlugin.MarginFor(bounds.size.x, 0.32f);
					float num2 = MoveStationsPlugin.MarginFor(bounds.size.z, 0.3f);
					float num3 = bounds.min.x + num;
					float num4 = bounds.max.x - num;
					float num5 = bounds.min.z + num2;
					float num6 = bounds.max.z - num2;
					if (num4 < num3)
					{
						num4 = num3;
					}
					if (num6 < num5)
					{
						num6 = num5;
					}
					float num7 = pos.x - this.footHalfX;
					float num8 = pos.x + this.footHalfX;
					float num9 = pos.z - this.footHalfZ;
					float num10 = pos.z + this.footHalfZ;
					float num11 = Mathf.Min(num8, num4) - Mathf.Max(num7, num3);
					float num12 = Mathf.Min(num10, num6) - Mathf.Max(num9, num5);
					bool flag2 = num11 > 0f && num12 > 0f;
					string[] array = new string[14];
					array[0] = " size=";
					int num13 = 1;
					Vector3 vector = bounds.size;
					array[num13] = vector.x.ToString("F2");
					array[2] = "x";
					int num14 = 3;
					vector = bounds.size;
					array[num14] = vector.z.ToString("F2");
					array[4] = " shrunk=";
					array[5] = (num4 - num3).ToString("F2");
					array[6] = "x";
					array[7] = (num6 - num5).ToString("F2");
					array[8] = " overlap=";
					array[9] = Mathf.Max(0f, num11).ToString("F2");
					array[10] = "x";
					array[11] = Mathf.Max(0f, num12).ToString("F2");
					array[12] = " -> ";
					array[13] = (flag2 ? "blocks" : "ignored (outer ring only)");
					info = string.Concat(array);
					flag = flag2;
				}
			}
			catch
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00006C50 File Offset: 0x00004E50
		private static int ReadInt(FieldInfo f, object o)
		{
			int num;
			try
			{
				if (f == null || o == null)
				{
					num = int.MinValue;
				}
				else
				{
					object value = f.GetValue(o);
					if (value == null)
					{
						num = int.MinValue;
					}
					else
					{
						num = Convert.ToInt32(value);
					}
				}
			}
			catch
			{
				num = int.MinValue;
			}
			return num;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00006CA8 File Offset: 0x00004EA8
		private string CountOf(FieldInfo f, object o)
		{
			string text;
			try
			{
				if (f == null || o == null)
				{
					text = "?";
				}
				else
				{
					object value = f.GetValue(o);
					if (value == null)
					{
						text = "null";
					}
					else
					{
						PropertyInfo property = value.GetType().GetProperty("Count");
						if (property == null)
						{
							text = "?";
						}
						else
						{
							object value2 = property.GetValue(value, null);
							text = ((value2 != null) ? value2.ToString() : "?");
						}
					}
				}
			}
			catch
			{
				text = "?";
			}
			return text;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00006D34 File Offset: 0x00004F34
		private static string ElemTypeName(FieldInfo f)
		{
			string text;
			try
			{
				if (f == null)
				{
					text = "?";
				}
				else
				{
					Type fieldType = f.FieldType;
					if (fieldType.IsGenericType)
					{
						Type[] genericArguments = fieldType.GetGenericArguments();
						if (genericArguments.Length != 0)
						{
							return genericArguments[0].Name;
						}
					}
					text = fieldType.Name;
				}
			}
			catch
			{
				text = "?";
			}
			return text;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00006D9C File Offset: 0x00004F9C
		private void LearnOwnCells()
		{
			this.ownCells = null;
			this.ownOffsets = null;
			if (!this.snapValid)
			{
				return;
			}
			try
			{
				int num = Mathf.RoundToInt(this.originalPos.x / 0.32f);
				int num2 = Mathf.RoundToInt(this.originalPos.z / 0.3f);
				List<Vector2Int> list = new List<Vector2Int>();
				for (int i = -3; i <= 3; i++)
				{
					for (int j = -3; j <= 3; j++)
					{
						MoveStationsPlugin.SnapCell snapCell;
						if (this.snapCells.TryGetValue(new Vector2Int(num + i, num2 + j), out snapCell) && this.IsOwnCellRecord(snapCell))
						{
							list.Add(new Vector2Int(i, j));
						}
					}
				}
				if (list.Count == 0)
				{
					this.Dbg(string.Concat(new string[]
					{
						"own cells: none found in snapshot near ",
						MoveStationsPlugin.Fmt(this.originalPos),
						" (label='",
						this.movingWgoId,
						"')"
					}));
				}
				else
				{
					List<Vector2Int> list2 = MoveStationsPlugin.Cluster(list);
					if (list2.Count == 0 || list2.Count > 64)
					{
						this.Dbg("own cells: template rejected n=" + list2.Count.ToString());
					}
					else
					{
						this.ownOffsets = list2;
						this.ownCells = new HashSet<Vector2Int>();
						foreach (Vector2Int vector2Int in list2)
						{
							this.ownCells.Add(new Vector2Int(num + vector2Int.x, num2 + vector2Int.y));
						}
						this.Dbg("own cells: n=" + list2.Count.ToString() + " offsets=" + MoveStationsPlugin.OffsetsString(list2));
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg("own cells error: " + ex.Message);
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00006FB4 File Offset: 0x000051B4
		private bool IsOwnCellRecord(MoveStationsPlugin.SnapCell rec)
		{
			if (rec == null)
			{
				return false;
			}
			try
			{
				if (rec.def != null && this.movingBuildingDef != null && rec.def == this.movingBuildingDef)
				{
					return true;
				}
			}
			catch
			{
			}
			return !string.IsNullOrEmpty(this.movingWgoId) && rec.label == this.movingWgoId;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00007024 File Offset: 0x00005224
		private bool SnapshotBlocks(Vector3 pos, int cx, int cz, out string reason)
		{
			reason = "";
			int num = 0;
			string text = "";
			string text2 = "";
			string text3 = "";
			if (this.ownOffsets != null)
			{
				for (int i = 0; i < this.ownOffsets.Count; i++)
				{
					int num2 = cx + this.ownOffsets[i].x;
					int num3 = cz + this.ownOffsets[i].y;
					MoveStationsPlugin.SnapCell snapCell;
					if ((this.ownCells == null || !this.ownCells.Contains(new Vector2Int(num2, num3))) && this.snapCells.TryGetValue(new Vector2Int(num2, num3), out snapCell))
					{
						num++;
						if (num == 1)
						{
							text = snapCell.label;
							text2 = MoveStationsPlugin.FriendlyFromId(snapCell.label);
						}
						if (num <= 3)
						{
							text3 = string.Concat(new string[]
							{
								text3,
								" [",
								this.ownOffsets[i].x.ToString(),
								",",
								this.ownOffsets[i].y.ToString(),
								"]=",
								snapCell.label
							});
						}
					}
				}
			}
			else
			{
				int num4 = Mathf.Max(1, Mathf.RoundToInt(this.footHalfX / 0.32f));
				int num5 = Mathf.Max(1, Mathf.RoundToInt(this.footHalfZ / 0.3f));
				for (int j = -num4; j <= num4; j++)
				{
					for (int k = -num5; k <= num5; k++)
					{
						MoveStationsPlugin.SnapCell snapCell2;
						if (this.snapCells.TryGetValue(new Vector2Int(cx + j, cz + k), out snapCell2))
						{
							num++;
							if (num == 1)
							{
								text = snapCell2.label;
								text2 = MoveStationsPlugin.FriendlyFromId(snapCell2.label);
							}
							if (num <= 3)
							{
								text3 = string.Concat(new string[]
								{
									text3,
									" [",
									j.ToString(),
									",",
									k.ToString(),
									"]=",
									snapCell2.label
								});
							}
						}
					}
				}
			}
			string text4 = ((num > 0) ? ("B:" + text + ":" + num.ToString()) : "free");
			if (text4 != this.lastGridResult)
			{
				this.lastGridResult = text4;
				if (num > 0)
				{
					this.Dbg(string.Concat(new string[]
					{
						"grid: BLOCKED occupied by '",
						text,
						"' cells=",
						num.ToString(),
						text3,
						" at ",
						MoveStationsPlugin.Fmt(pos),
						(this.ownOffsets == null) ? " (no own template)" : ""
					}));
				}
				else
				{
					this.Dbg("grid: free at " + MoveStationsPlugin.Fmt(pos));
				}
			}
			if (num > 0)
			{
				reason = "occupied by " + text2 + ((num > 1) ? (" (+" + (num - 1).ToString() + " more)") : "");
				return true;
			}
			return false;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00007358 File Offset: 0x00005558
		private bool WorldObstacleBlocks(Vector3 pos, out string reason, out string diag)
		{
			reason = "";
			diag = "";
			Collider[] array = Physics.OverlapBox(pos + new Vector3(0f, 0.5f, 0f), new Vector3(this.footHalfX, 0.6f, this.footHalfZ), Quaternion.identity, 524544, QueryTriggerInteraction.Collide);
			int num = 0;
			string text = "";
			int num2 = 0;
			string text2 = "";
			foreach (Collider collider in array)
			{
				if (!(collider == null) && !this.IsSelfCollider(collider) && (!(this.buildAreaType != null) || !(collider.GetComponentInParent(this.buildAreaType) != null)) && MoveStationsPlugin.IsHardBlocker(collider))
				{
					bool flag = false;
					try
					{
						flag = this.wgoType != null && collider.GetComponentInParent(this.wgoType) != null;
					}
					catch
					{
					}
					string text3;
					if (flag)
					{
						num2++;
						if (text2.Length == 0)
						{
							text2 = MoveStationsPlugin.FriendlyName(collider.gameObject);
						}
					}
					else if (this.OverlapsAfterMargin(collider, pos, out text3))
					{
						num++;
						if (text.Length == 0)
						{
							text = MoveStationsPlugin.FriendlyName(collider.gameObject);
						}
					}
				}
			}
			if (num2 > 0)
			{
				diag = " [ignored building colliders: " + num2.ToString() + ((text2.Length > 0) ? (", e.g. '" + text2 + "'") : "") + "]";
			}
			if (num > 0)
			{
				reason = "blocked by " + text + ((num > 1) ? (" (+" + (num - 1).ToString() + " more)") : "");
				return true;
			}
			return false;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00007528 File Offset: 0x00005728
		private void EnsureValidation()
		{
			if (this.hasValidated && (this.previewPos - this.lastValidatedPos).sqrMagnitude < 0.0004f)
			{
				return;
			}
			if (this.nativeBuildPreview)
			{
				this.UpdateNativeBuildPreview();
				return;
			}
			this.lastValidatedPos = this.previewPos;
			this.hasValidated = true;
			string text;
			bool flag = this.ValidateSpot(this.previewPos, out text);
			this.spotValid = flag;
			this.spotReason = (flag ? "" : text);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000075A8 File Offset: 0x000057A8
		private bool ValidateSpot(Vector3 pos, out string reason)
		{
			reason = "";
			if (this.buildAreaType != null)
			{
				bool flag;
				if (!string.IsNullOrEmpty(this.requiredAreaId))
				{
					flag = this.HasBuildAreaWithId(pos, this.requiredAreaId);
				}
				else
				{
					flag = this.HasAnyBuildArea(pos);
				}
				if (!flag)
				{
					reason = "outside the allowed build area";
					this.Dbg("validate " + MoveStationsPlugin.Fmt(pos) + " -> BLOCKED (outside the allowed build area)");
					this.Dbg("  area: required='" + this.requiredAreaId + "' not found under " + MoveStationsPlugin.Fmt(pos));
					return false;
				}
			}
			if (this.snapValid)
			{
				int num = Mathf.RoundToInt(pos.x / 0.32f);
				int num2 = Mathf.RoundToInt(pos.z / 0.3f);
				if (num >= this.snapMinX - 1 && num <= this.snapMaxX + 1 && num2 >= this.snapMinZ - 1 && num2 <= this.snapMaxZ + 1)
				{
					string text;
					if (this.SnapshotBlocks(pos, num, num2, out text))
					{
						reason = text;
						this.Dbg(string.Concat(new string[]
						{
							"validate ",
							MoveStationsPlugin.Fmt(pos),
							" -> BLOCKED (",
							reason,
							") [snapshot]"
						}));
						return false;
					}
					string text2;
					string text3;
					if (this.WorldObstacleBlocks(pos, out text2, out text3))
					{
						reason = text2;
						this.Dbg(string.Concat(new string[]
						{
							"validate ",
							MoveStationsPlugin.Fmt(pos),
							" -> BLOCKED (",
							reason,
							") [snapshot free, world obstacle]"
						}));
						return false;
					}
					this.Dbg("validate " + MoveStationsPlugin.Fmt(pos) + " -> OK [snapshot]" + text3);
					return true;
				}
				else
				{
					string text4 = "oob:" + num.ToString() + "," + num2.ToString();
					if (this.lastGridOobKey != text4)
					{
						this.lastGridOobKey = text4;
						this.Dbg("grid: target outside snapshot extent -> collider checks at " + MoveStationsPlugin.Fmt(pos));
					}
				}
			}
			Collider[] array = Physics.OverlapBox(pos + new Vector3(0f, 0.5f, 0f), new Vector3(this.footHalfX, 0.6f, this.footHalfZ), Quaternion.identity, 524544, QueryTriggerInteraction.Collide);
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			string text5 = "";
			int num7 = 0;
			foreach (Collider collider in array)
			{
				if (!(collider == null))
				{
					bool flag2 = this.IsSelfCollider(collider);
					bool flag3 = this.buildAreaType != null && collider.GetComponentInParent(this.buildAreaType) != null;
					if (flag2 || flag3)
					{
						if (num7 < 4)
						{
							num7++;
							this.Dbg(string.Concat(new string[]
							{
								"  skip: '",
								MoveStationsPlugin.PathOf(collider.gameObject),
								"' self=",
								flag2.ToString(),
								" area=",
								flag3.ToString()
							}));
						}
					}
					else if (!MoveStationsPlugin.IsHardBlocker(collider))
					{
						num4++;
						this.Dbg(string.Concat(new string[]
						{
							"  ignore-aux: '",
							MoveStationsPlugin.PathOf(collider.gameObject),
							"' layer=",
							collider.gameObject.layer.ToString(),
							" trigger=",
							collider.isTrigger.ToString()
						}));
					}
					else
					{
						bool flag4 = false;
						try
						{
							flag4 = this.wgoType != null && collider.GetComponentInParent(this.wgoType) != null;
						}
						catch
						{
						}
						string text6;
						if (flag4 && collider.gameObject.layer == 8)
						{
							num5++;
							if (num5 <= 4)
							{
								this.Dbg("  ignore-decor: '" + MoveStationsPlugin.PathOf(collider.gameObject) + "' layer=8 (building decoration, not a build cell)");
							}
						}
						else if (!this.OverlapsAfterMargin(collider, pos, out text6))
						{
							num6++;
							if (num6 <= 6)
							{
								this.Dbg(string.Concat(new string[]
								{
									"  ignore-margin: '",
									MoveStationsPlugin.PathOf(collider.gameObject),
									"' layer=",
									collider.gameObject.layer.ToString(),
									" trigger=",
									collider.isTrigger.ToString(),
									text6
								}));
							}
						}
						else
						{
							num3++;
							if (num3 == 1)
							{
								text5 = MoveStationsPlugin.FriendlyName(collider.gameObject);
							}
							if (num3 <= 6)
							{
								this.Dbg(string.Concat(new string[]
								{
									"  blocker[",
									num3.ToString(),
									"]: '",
									MoveStationsPlugin.PathOf(collider.gameObject),
									"' layer=",
									collider.gameObject.layer.ToString(),
									"(",
									LayerMask.LayerToName(collider.gameObject.layer),
									") trigger=",
									collider.isTrigger.ToString(),
									flag4 ? " wgo" : " static",
									text6
								}));
							}
						}
					}
				}
			}
			string text7 = "";
			if (num4 > 0)
			{
				text7 = text7 + " [ignored aux=" + num4.ToString() + "]";
			}
			if (num5 > 0)
			{
				text7 = text7 + " [ignored decor=" + num5.ToString() + "]";
			}
			if (num6 > 0)
			{
				text7 = text7 + " [ignored outer collider ring=" + num6.ToString() + "]";
			}
			if (num3 > 0)
			{
				if (num3 == 1)
				{
					reason = "blocked by " + text5;
				}
				else
				{
					reason = string.Concat(new string[]
					{
						"blocked by ",
						text5,
						" (+",
						(num3 - 1).ToString(),
						" more)"
					});
				}
				if (this.selOverrideReady && this.SelOverrideApplies())
				{
					int num8 = this.SelStateAtCell(pos);
					if (num8 == this.selFreeValue)
					{
						this.Dbg(string.Concat(new string[]
						{
							"validate ",
							MoveStationsPlugin.Fmt(pos),
							" -> OK [game map override: state=",
							num8.ToString(),
							" is free, colliders said '",
							reason,
							"']",
							text7
						}));
						return true;
					}
				}
				else if (this.selOverrideReady && !this.selMismatchLogged)
				{
					this.selMismatchLogged = true;
					this.Dbg(string.Concat(new string[] { "game map override off: map was captured for '", this.selDefName, "', moving '", this.movingWgoId, "'" }));
				}
				this.Dbg(string.Concat(new string[]
				{
					"validate ",
					MoveStationsPlugin.Fmt(pos),
					" -> BLOCKED (",
					reason,
					")",
					text7
				}));
				return false;
			}
			this.Dbg("validate " + MoveStationsPlugin.Fmt(pos) + " -> OK" + text7);
			return true;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00007CE0 File Offset: 0x00005EE0
		private static bool IsHardBlocker(Collider c)
		{
			int layer = c.gameObject.layer;
			return layer == 8 || (layer == 19 && c.isTrigger);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00007D0C File Offset: 0x00005F0C
		private bool IsSelfCollider(Collider c)
		{
			if (c == null || this.movingGo == null)
			{
				return false;
			}
			Transform transform = c.transform;
			if (transform == this.movingGo.transform || transform.IsChildOf(this.movingGo.transform))
			{
				return true;
			}
			if (this.wgoType != null && this.movingWgo != null)
			{
				Component componentInParent = c.GetComponentInParent(this.wgoType);
				if (componentInParent != null && componentInParent == this.movingWgo)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00007DA4 File Offset: 0x00005FA4
		private static string FriendlyName(GameObject go)
		{
			if (go == null)
			{
				return "something";
			}
			string text = go.name;
			try
			{
				Transform transform = go.transform;
				int num = 0;
				while (transform != null && num < 6)
				{
					if (!MoveStationsPlugin.IsInternalName(transform.name))
					{
						text = transform.name;
						break;
					}
					transform = transform.parent;
					num++;
				}
			}
			catch
			{
			}
			string text2 = text;
			int num2 = text2.IndexOf("(Clone)", StringComparison.OrdinalIgnoreCase);
			if (num2 > 0)
			{
				text2 = text2.Substring(0, num2);
			}
			int num3 = text2.IndexOf('(');
			if (num3 > 0)
			{
				text2 = text2.Substring(0, num3);
			}
			text2 = MoveStationsPlugin.TitleCase(text2.Replace('_', ' ').Trim());
			if (text2.Length == 0)
			{
				text2 = text;
			}
			string text3 = text2.ToLowerInvariant();
			if (text3.Contains("fence"))
			{
				return "Fence";
			}
			if (text3.Contains("rock") || text3.Contains("stone"))
			{
				return "Rock";
			}
			if (text3.Contains("tree"))
			{
				return "Tree";
			}
			if (text3.Contains("bush"))
			{
				return "Bush";
			}
			if (text3.Contains("ruin"))
			{
				return "Ruins";
			}
			if (text3.Contains("lamp") || text3.Contains("lantern") || text3.Contains("light"))
			{
				return "Lamp Post";
			}
			if (text3.Contains("hammer"))
			{
				return "Hammer Station";
			}
			if (text2.Length > 26)
			{
				text2 = text2.Substring(0, 26) + "...";
			}
			return text2;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00007F50 File Offset: 0x00006150
		private static string FriendlyFromId(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return "building";
			}
			string text = id;
			int num = text.IndexOf("(Clone)", StringComparison.OrdinalIgnoreCase);
			if (num > 0)
			{
				text = text.Substring(0, num);
			}
			int num2 = text.IndexOf('(');
			if (num2 > 0)
			{
				text = text.Substring(0, num2);
			}
			text = MoveStationsPlugin.TitleCase(text.Replace('_', ' ').Trim());
			if (text.Length == 0)
			{
				text = id;
			}
			string text2 = text.ToLowerInvariant();
			if (text2.Contains("fence"))
			{
				return "Fence";
			}
			if (text2.Contains("rock") || text2.Contains("stone"))
			{
				return "Rock";
			}
			if (text2.Contains("tree"))
			{
				return "Tree";
			}
			if (text2.Contains("bush"))
			{
				return "Bush";
			}
			if (text2.Contains("ruin"))
			{
				return "Ruins";
			}
			if (text2.Contains("lamp") || text2.Contains("lantern") || text2.Contains("light"))
			{
				return "Lamp Post";
			}
			if (text2.Contains("hammer"))
			{
				return "Hammer Station";
			}
			if (text.Length > 26)
			{
				text = text.Substring(0, 26) + "...";
			}
			return text;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00008090 File Offset: 0x00006290
		private static bool IsInternalName(string n)
		{
			if (string.IsNullOrEmpty(n))
			{
				return true;
			}
			string text = n.ToLowerInvariant();
			return text == "world" || text == "new game object" || (text.Contains("collider") || text.Contains("obstacle")) || (text == "base" || text == "root" || text == "down" || text == "left" || text == "right") || (text == "horizontal" || text == "vertical" || text.Contains("variation")) || (text == "pillow" || text == "physics") || text.Contains("rotation");
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00008180 File Offset: 0x00006380
		private static string TitleCase(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			string[] array = s.Split(new char[] { ' ' });
			string text = "";
			foreach (string text2 in array)
			{
				if (text2.Length != 0)
				{
					if (text.Length > 0)
					{
						text += " ";
					}
					text += char.ToUpperInvariant(text2[0]).ToString();
					if (text2.Length > 1)
					{
						text += text2.Substring(1);
					}
				}
			}
			return text;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00008214 File Offset: 0x00006414
		private static object GetProp(object o, string name)
		{
			object obj;
			try
			{
				if (o == null)
				{
					obj = null;
				}
				else
				{
					PropertyInfo property = o.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					obj = ((property != null) ? property.GetValue(o, null) : null);
				}
			}
			catch
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00008264 File Offset: 0x00006464
		private static void SetProp(object o, string name, object v)
		{
			try
			{
				if (o != null && v != null)
				{
					PropertyInfo property = o.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (!(property == null) && property.CanWrite)
					{
						if (property.PropertyType == typeof(Color32) && v is Color)
						{
							property.SetValue(o, (Color)v, null);
						}
						else if (property.PropertyType == typeof(Color) && v is Color32)
						{
							property.SetValue(o, (Color32)v, null);
						}
						else
						{
							property.SetValue(o, v, null);
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00008334 File Offset: 0x00006534
		private bool HasBuildAreaWithId(Vector3 pos, string id)
		{
			foreach (Collider collider in Physics.OverlapSphere(pos + new Vector3(0f, 0.04f, 0f), 0.3f, -1, QueryTriggerInteraction.Collide))
			{
				if (!(collider == null) && !this.IsSelfCollider(collider))
				{
					Component componentInParent = collider.GetComponentInParent(this.buildAreaType);
					if (!(componentInParent == null))
					{
						object obj = MoveStationsPlugin.TryGet(this.fAreaId, componentInParent);
						string text = ((obj != null) ? obj.ToString() : null);
						if (!string.IsNullOrEmpty(text) && text.Equals(id, StringComparison.OrdinalIgnoreCase))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000083D8 File Offset: 0x000065D8
		private bool HasAnyBuildArea(Vector3 pos)
		{
			foreach (Collider collider in Physics.OverlapSphere(pos + new Vector3(0f, 0.04f, 0f), 0.3f, -1, QueryTriggerInteraction.Collide))
			{
				if (!(collider == null) && !this.IsSelfCollider(collider) && collider.GetComponentInParent(this.buildAreaType) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00008448 File Offset: 0x00006648
		private string ResolveRequiredArea(object data)
		{
			this.movingBuildingDef = null;
			this.movingWgoId = "";
			try
			{
				if (this.defType == null)
				{
					return "";
				}
				object obj = MoveStationsPlugin.FindReferenceOfType(data, this.defType);
				if (obj == null)
				{
					return "";
				}
				if (this.mTryGetBDef != null && this.bDefType != null)
				{
					object[] array = new object[1];
					object obj2 = null;
					try
					{
						obj2 = this.mTryGetBDef.Invoke(obj, array);
					}
					catch
					{
					}
					if (obj2 is bool && (bool)obj2 && array[0] != null)
					{
						this.movingBuildingDef = array[0];
						this.movingWgoId = this.WgoIdRaw(this.movingBuildingDef);
						object obj3 = MoveStationsPlugin.TryGet(this.fBDefAreaId, array[0]);
						if (obj3 != null)
						{
							return obj3.ToString();
						}
					}
				}
				if (this.fDefAreaId != null)
				{
					object obj4 = MoveStationsPlugin.TryGet(this.fDefAreaId, obj);
					if (obj4 != null)
					{
						return obj4.ToString();
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg("ResolveRequiredArea error: " + ex.Message);
			}
			return "";
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000858C File Offset: 0x0000678C
		private void ReadFootprint()
		{
			this.footHalfX = 0.32f;
			this.footHalfZ = 0.3f;
			try
			{
				if (!(this.movingGo == null))
				{
					Collider[] componentsInChildren = this.movingGo.GetComponentsInChildren<Collider>(true);
					bool flag = false;
					float num = float.MaxValue;
					float num2 = float.MinValue;
					float num3 = float.MaxValue;
					float num4 = float.MinValue;
					foreach (Collider collider in componentsInChildren)
					{
						if (!(collider == null) && collider.enabled)
						{
							string text = collider.gameObject.name.Replace(" ", "");
							if (text.IndexOf("BuildCollider", StringComparison.OrdinalIgnoreCase) >= 0 && text.IndexOf("Capsule", StringComparison.OrdinalIgnoreCase) < 0)
							{
								Bounds bounds = collider.bounds;
								if (bounds.size.sqrMagnitude >= 0.0001f)
								{
									flag = true;
									if (bounds.min.x < num)
									{
										num = bounds.min.x;
									}
									if (bounds.max.x > num2)
									{
										num2 = bounds.max.x;
									}
									if (bounds.min.z < num3)
									{
										num3 = bounds.min.z;
									}
									if (bounds.max.z > num4)
									{
										num4 = bounds.max.z;
									}
								}
							}
						}
					}
					if (flag)
					{
						this.footHalfX = Mathf.Clamp((num2 - num) * 0.5f, 0.15f, 1.2f);
						this.footHalfZ = Mathf.Clamp((num4 - num3) * 0.5f, 0.15f, 1.2f);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000875C File Offset: 0x0000695C
		private static List<Vector2Int> Cluster(List<Vector2Int> found)
		{
			List<Vector2Int> list = new List<Vector2Int>();
			if (found == null || found.Count == 0)
			{
				return list;
			}
			Vector2Int vector2Int = found[0];
			int num = Mathf.Abs(vector2Int.x) + Mathf.Abs(vector2Int.y);
			for (int i = 1; i < found.Count; i++)
			{
				int num2 = Mathf.Abs(found[i].x) + Mathf.Abs(found[i].y);
				if (num2 < num)
				{
					num = num2;
					vector2Int = found[i];
				}
			}
			HashSet<Vector2Int> hashSet = new HashSet<Vector2Int>(found);
			HashSet<Vector2Int> hashSet2 = new HashSet<Vector2Int>();
			Queue<Vector2Int> queue = new Queue<Vector2Int>();
			queue.Enqueue(vector2Int);
			hashSet2.Add(vector2Int);
			while (queue.Count > 0)
			{
				Vector2Int vector2Int2 = queue.Dequeue();
				list.Add(vector2Int2);
				foreach (Vector2Int vector2Int3 in new Vector2Int[]
				{
					new Vector2Int(vector2Int2.x + 1, vector2Int2.y),
					new Vector2Int(vector2Int2.x - 1, vector2Int2.y),
					new Vector2Int(vector2Int2.x, vector2Int2.y + 1),
					new Vector2Int(vector2Int2.x, vector2Int2.y - 1)
				})
				{
					if (hashSet.Contains(vector2Int3) && !hashSet2.Contains(vector2Int3))
					{
						hashSet2.Add(vector2Int3);
						queue.Enqueue(vector2Int3);
					}
				}
			}
			return list;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000088FC File Offset: 0x00006AFC
		private static string OffsetsString(List<Vector2Int> tpl)
		{
			string text = "";
			int num = Mathf.Min(tpl.Count, 12);
			for (int i = 0; i < num; i++)
			{
				if (i > 0)
				{
					text += ",";
				}
				text = string.Concat(new string[]
				{
					text,
					"(",
					tpl[i].x.ToString(),
					",",
					tpl[i].y.ToString(),
					")"
				});
			}
			if (tpl.Count > num)
			{
				text += "...";
			}
			return text;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000089B0 File Offset: 0x00006BB0
		private string DefLabel(object def)
		{
			string text;
			try
			{
				if (def == null)
				{
					text = "?";
				}
				else
				{
					string text2 = this.WgoIdRaw(def);
					if (!string.IsNullOrEmpty(text2))
					{
						text = text2;
					}
					else
					{
						text = def.GetType().Name;
					}
				}
			}
			catch
			{
				text = "?";
			}
			return text;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00008A04 File Offset: 0x00006C04
		private string WgoIdRaw(object def)
		{
			string text;
			try
			{
				if (def == null || this.fBDefWgoId == null)
				{
					text = null;
				}
				else
				{
					object value = this.fBDefWgoId.GetValue(def);
					text = ((value != null) ? value.ToString() : null);
				}
			}
			catch
			{
				text = null;
			}
			return text;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00008A58 File Offset: 0x00006C58
		private Vector3 GetCursorWorldPos(float groundY)
		{
			Camera worldCamera = this.GetWorldCamera();
			if (worldCamera == null)
			{
				return this.previewPos;
			}
			Ray ray = worldCamera.ScreenPointToRay(Input.mousePosition);
			Plane plane = new Plane(Vector3.up, new Vector3(0f, groundY, 0f));
			float num;
			if (plane.Raycast(ray, out num) && num > 0f)
			{
				return ray.GetPoint(num);
			}
			return this.previewPos;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00008AC8 File Offset: 0x00006CC8
		private Camera GetWorldCamera()
		{
			if (this.worldCam != null && this.worldCam.isActiveAndEnabled)
			{
				return this.worldCam;
			}
			GameObject gameObject = GameObject.Find("PlayerPhysicalBody");
			Vector3 vector = ((gameObject != null) ? gameObject.transform.position : Vector3.zero);
			List<Camera> list = new List<Camera>();
			try
			{
				foreach (Camera camera in Camera.allCameras)
				{
					if (camera != null && !list.Contains(camera))
					{
						list.Add(camera);
					}
				}
			}
			catch
			{
			}
			try
			{
				foreach (Camera camera2 in global::UnityEngine.Object.FindObjectsOfType<Camera>())
				{
					if (camera2 != null && !list.Contains(camera2))
					{
						list.Add(camera2);
					}
				}
			}
			catch
			{
			}
			Camera camera3 = null;
			float num = float.MinValue;
			foreach (Camera camera4 in list)
			{
				if (!(camera4 == null))
				{
					string text = MoveStationsPlugin.CamPath(camera4).ToLowerInvariant();
					bool flag = false;
					float num2 = 0f;
					try
					{
						if (gameObject != null)
						{
							Vector3 vector2 = camera4.WorldToScreenPoint(vector);
							flag = vector2.z > 0f && vector2.x >= 0f && vector2.y >= 0f && vector2.x <= (float)camera4.pixelWidth && vector2.y <= (float)camera4.pixelHeight;
							num2 += (flag ? 1E+09f : (-100000000f));
						}
						if (camera4.targetTexture != null)
						{
							num2 -= 5E+09f;
						}
						if (text.Contains("main"))
						{
							num2 += 2E+09f;
						}
						if (text.Contains("deform"))
						{
							num2 -= 5E+09f;
						}
						if (text.Contains("minimap") || text.Contains("shadow") || text.Contains("preview") || text.Contains("reflection"))
						{
							num2 -= 2E+09f;
						}
						if (camera4.isActiveAndEnabled)
						{
							num2 += 100000000f;
						}
						else
						{
							num2 -= 100000000f;
						}
						num2 += (float)camera4.pixelWidth * (float)camera4.pixelHeight * 0.001f;
					}
					catch
					{
					}
					if (!this.camDumpDone)
					{
						this.Dbg(string.Concat(new string[]
						{
							"cam: '",
							MoveStationsPlugin.CamPath(camera4),
							"' ortho=",
							camera4.orthographic.ToString(),
							" depth=",
							camera4.depth.ToString(),
							" rtt=",
							(camera4.targetTexture != null).ToString(),
							" onScreen=",
							flag.ToString(),
							" score=",
							num2.ToString("F0")
						}));
					}
					if (num2 > num)
					{
						num = num2;
						camera3 = camera4;
					}
				}
			}
			this.camDumpDone = true;
			this.worldCam = camera3;
			if (!this.camLogged)
			{
				this.camLogged = true;
				if (this.debug)
				{
					if (this.worldCam != null)
					{
						base.Logger.LogInfo(string.Concat(new string[]
						{
							"World camera: '",
							MoveStationsPlugin.CamPath(this.worldCam),
							"' (orthographic=",
							this.worldCam.orthographic.ToString(),
							")"
						}));
					}
					else
					{
						base.Logger.LogWarning("World camera not found!");
					}
				}
			}
			return this.worldCam;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00008F14 File Offset: 0x00007114
		private static string CamPath(Camera c)
		{
			string text = c.gameObject.name;
			Transform transform = c.transform.parent;
			int num = 0;
			while (transform != null && num < 2)
			{
				text = transform.name + "/" + text;
				transform = transform.parent;
				num++;
			}
			return text;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00008F68 File Offset: 0x00007168
		private Vector3 ClampFromPlayer(Vector3 pos)
		{
			GameObject gameObject = GameObject.Find("PlayerPhysicalBody");
			if (gameObject == null)
			{
				return pos;
			}
			Vector3 position = gameObject.transform.position;
			Vector3 vector = new Vector3(pos.x - position.x, 0f, pos.z - position.z);
			float magnitude = vector.magnitude;
			if (magnitude > 22f)
			{
				vector *= 22f / magnitude;
				pos = new Vector3(position.x + vector.x, pos.y, position.z + vector.z);
			}
			return pos;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00009004 File Offset: 0x00007204
		private void OnCanvasWillRenderCanvases()
		{
			try
			{
				if (!(this.moveMenuRow != null))
				{
					float realtimeSinceStartup = Time.realtimeSinceStartup;
					if (this.buildMenuOpenEventsReady)
					{
						if (!this.buildMenuInjectionRequested || this.cachedBuildMenuWindow == null || !this.cachedBuildMenuWindow.activeInHierarchy)
						{
							return;
						}
					}
					else if (!this.IsBuildMenuWindowShown(realtimeSinceStartup))
					{
						return;
					}
					if (realtimeSinceStartup >= this.nextMoveMenuSearch)
					{
						this.nextMoveMenuSearch = realtimeSinceStartup + 0.5f;
						this.TryInjectMoveMenuRow();
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg("build menu injection error: " + ex.Message);
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000090A8 File Offset: 0x000072A8
		private bool IsBuildMenuWindowShown(float now)
		{
			bool flag;
			try
			{
				if (this.cachedBuildMenuWindow != null)
				{
					flag = this.cachedBuildMenuWindow.activeInHierarchy;
				}
				else if (now < this.nextBuildMenuWindowLookup)
				{
					flag = false;
				}
				else
				{
					this.nextBuildMenuWindowLookup = now + 0.5f;
					GameObject gameObject = GameObject.Find("UI/UIRoot/UIBuildingWindow");
					if (gameObject == null)
					{
						flag = false;
					}
					else if (this.uiBuildingWindowType != null && gameObject.GetComponent(this.uiBuildingWindowType) == null)
					{
						flag = false;
					}
					else
					{
						this.cachedBuildMenuWindow = gameObject;
						flag = gameObject.activeInHierarchy;
					}
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00009150 File Offset: 0x00007350
		private void SubscribeBuildMenuOpenEvent()
		{
			try
			{
				Type type = MoveStationsPlugin.FindTypeInAnyAssembly("LazyWindowsStackController");
				if (!(type == null))
				{
					EventInfo @event = type.GetEvent("OnWindowOpened", BindingFlags.Static | BindingFlags.Public);
					if (!(@event == null) && !(@event.EventHandlerType == null))
					{
						MethodInfo method = @event.EventHandlerType.GetMethod("Invoke");
						ParameterInfo[] array = ((method != null) ? method.GetParameters() : null);
						if (array != null && array.Length == 1)
						{
							MethodInfo methodInfo = base.GetType().GetMethod("OnLazyWindowOpened", BindingFlags.Instance | BindingFlags.NonPublic);
							if (!(methodInfo == null) && methodInfo.IsGenericMethodDefinition)
							{
								methodInfo = methodInfo.MakeGenericMethod(new Type[] { array[0].ParameterType });
								Delegate @delegate = Delegate.CreateDelegate(@event.EventHandlerType, this, methodInfo, false);
								if (@delegate != null)
								{
									@event.AddEventHandler(null, @delegate);
									this.eLazyWindowOpened = @event;
									this.dLazyWindowOpened = @delegate;
									this.buildMenuOpenEventsReady = true;
									this.Dbg("build menu: subscribed to LazyWindowsStackController.OnWindowOpened");
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg("build menu: window-open event unavailable: " + ex.Message);
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00009284 File Offset: 0x00007484
		private void OnLazyWindowOpened<TWindow>(TWindow window)
		{
			try
			{
				Component component = window as Component;
				if (!(component == null) && !(this.uiBuildingWindowType == null) && this.uiBuildingWindowType.IsInstanceOfType(component))
				{
					this.cachedBuildMenuWindow = component.gameObject;
					this.buildMenuInjectionRequested = true;
					this.nextMoveMenuSearch = 0f;
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000092F8 File Offset: 0x000074F8
		private void TryInjectMoveMenuRow()
		{
			Button button = this.FindRemoveRowButtonByBuildMode();
			if (button == null)
			{
				button = this.FindRemoveRowButtonByEnglishLabel();
			}
			if (button == null)
			{
				return;
			}
			try
			{
				GameObject gameObject = button.gameObject;
				GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(gameObject, gameObject.transform.parent, false);
				if (!(gameObject2 == null))
				{
					gameObject2.name = "GK2MoveStationsBuildMenuMoveRow";
					gameObject2.transform.SetSiblingIndex(gameObject.transform.GetSiblingIndex());
					foreach (Component component in gameObject2.GetComponentsInChildren<Component>(true))
					{
						if (!(component == null))
						{
							Text text = component as Text;
							if (text != null)
							{
								text.text = "Move";
								text.color = new Color(0.68f, 1f, 0.9f, 1f);
							}
							else if (component.GetType().Name == "TextMeshProUGUI" || component.GetType().Name == "TMP_Text")
							{
								MoveStationsPlugin.SetProp(component, "text", "Move");
								MoveStationsPlugin.SetProp(component, "color", new Color(0.68f, 1f, 0.9f, 1f));
							}
							Image image = component as Image;
							if (image != null && image.gameObject.name.IndexOf("Icon", StringComparison.OrdinalIgnoreCase) >= 0)
							{
								image.sprite = this.GetMoveMenuIconSprite();
								image.type = Image.Type.Simple;
								image.preserveAspect = true;
								image.color = Color.white;
								image.rectTransform.localScale = new Vector3(0.68f, 0.68f, 1f);
							}
						}
					}
					Button component2 = gameObject2.GetComponent<Button>();
					if (component2 == null)
					{
						global::UnityEngine.Object.Destroy(gameObject2);
					}
					else
					{
						component2.onClick.RemoveAllListeners();
						component2.onClick.AddListener(new UnityAction(this.OnMoveMenuClicked));
						this.moveMenuRow = gameObject2;
						this.moveMenuButton = component2;
						this.Dbg("build menu: injected Move row from '" + MoveStationsPlugin.PathOf(gameObject) + "'");
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg("build menu: row clone failed: " + ex.Message);
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00009564 File Offset: 0x00007764
		private Button FindRemoveRowButtonByBuildMode()
		{
			try
			{
				if (this.uiBuildingWidgetType == null || this.fUiBuildingWidgetData == null)
				{
					return null;
				}
				global::UnityEngine.Object[] array = global::UnityEngine.Object.FindObjectsOfType(this.uiBuildingWidgetType);
				if (array == null)
				{
					return null;
				}
				global::UnityEngine.Object[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					Component component = array2[i] as Component;
					if (!(component == null) && component.gameObject.activeInHierarchy && !MoveStationsPlugin.IsMoveMenuRow(component.transform))
					{
						object prop = MoveStationsPlugin.GetProp(MoveStationsPlugin.GetProp(this.fUiBuildingWidgetData.GetValue(component), "BuildData"), "BuildingMode");
						if (prop != null && string.Equals(prop.ToString(), "Remove", StringComparison.OrdinalIgnoreCase))
						{
							Button button = MoveStationsPlugin.FindParentButton(component.transform);
							if (button == null)
							{
								Button[] componentsInChildren = component.GetComponentsInChildren<Button>(true);
								if (componentsInChildren != null && componentsInChildren.Length != 0)
								{
									button = componentsInChildren[0];
								}
							}
							if (button != null)
							{
								this.Dbg("build menu: localized Remove row found by BuildData.BuildingMode");
								return button;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg("build menu: semantic Remove lookup failed: " + ex.Message);
			}
			return null;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000096BC File Offset: 0x000078BC
		private static bool IsMoveMenuRow(Transform start)
		{
			Transform transform = start;
			int num = 0;
			while (transform != null && num < 8)
			{
				if (string.Equals(transform.name, "GK2MoveStationsBuildMenuMoveRow", StringComparison.Ordinal))
				{
					return true;
				}
				num++;
				transform = transform.parent;
			}
			return false;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00009700 File Offset: 0x00007900
		private Button FindRemoveRowButtonByEnglishLabel()
		{
			try
			{
				Text[] array = global::UnityEngine.Object.FindObjectsOfType<Text>();
				if (array != null)
				{
					foreach (Text text in array)
					{
						if (!(text == null) && string.Equals((text.text ?? "").Trim(), "Remove", StringComparison.OrdinalIgnoreCase))
						{
							Button button = MoveStationsPlugin.FindParentButton(text.transform);
							if (button != null)
							{
								return button;
							}
						}
					}
				}
				Type type = MoveStationsPlugin.FindTypeInAnyAssembly("TextMeshProUGUI");
				if (type == null)
				{
					return null;
				}
				global::UnityEngine.Object[] array3 = global::UnityEngine.Object.FindObjectsOfType(type);
				if (array3 == null)
				{
					return null;
				}
				global::UnityEngine.Object[] array4 = array3;
				for (int i = 0; i < array4.Length; i++)
				{
					Component component = array4[i] as Component;
					if (!(component == null) && string.Equals(((MoveStationsPlugin.GetProp(component, "text") as string) ?? "").Trim(), "Remove", StringComparison.OrdinalIgnoreCase))
					{
						Button button2 = MoveStationsPlugin.FindParentButton(component.transform);
						if (button2 != null)
						{
							return button2;
						}
					}
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00009844 File Offset: 0x00007A44
		private Sprite GetMoveMenuIconSprite()
		{
			if (this.moveMenuIconSprite != null)
			{
				return this.moveMenuIconSprite;
			}
			Texture2D texture2D = new Texture2D(64, 64, TextureFormat.RGBA32, false);
			texture2D.filterMode = FilterMode.Point;
			texture2D.wrapMode = TextureWrapMode.Clamp;
			Color color = new Color(0.035f, 0.16f, 0.2f, 1f);
			Color color2 = new Color(0.06f, 0.44f, 0.47f, 1f);
			Color color3 = new Color(0.27f, 0.92f, 0.82f, 1f);
			Color color4 = new Color(0.88f, 1f, 0.94f, 1f);
			for (int i = 0; i < 64; i++)
			{
				for (int j = 0; j < 64; j++)
				{
					bool flag = j < 4 || j >= 60 || i < 4 || i >= 60;
					bool flag2 = j == 5 || j == 58 || i == 5 || i == 58;
					texture2D.SetPixel(j, i, flag ? color : (flag2 ? color3 : color2));
				}
			}
			MoveStationsPlugin.FillRect(texture2D, 28, 28, 8, 8, color4);
			MoveStationsPlugin.FillRect(texture2D, 29, 38, 6, 13, color4);
			MoveStationsPlugin.FillRect(texture2D, 26, 47, 12, 4, color4);
			MoveStationsPlugin.FillRect(texture2D, 29, 13, 6, 13, color4);
			MoveStationsPlugin.FillRect(texture2D, 26, 13, 12, 4, color4);
			MoveStationsPlugin.FillRect(texture2D, 38, 29, 13, 6, color4);
			MoveStationsPlugin.FillRect(texture2D, 47, 26, 4, 12, color4);
			MoveStationsPlugin.FillRect(texture2D, 13, 29, 13, 6, color4);
			MoveStationsPlugin.FillRect(texture2D, 13, 26, 4, 12, color4);
			texture2D.Apply(false, false);
			this.moveMenuIconSprite = Sprite.Create(texture2D, new Rect(0f, 0f, 64f, 64f), new Vector2(0.5f, 0.5f), 64f);
			this.moveMenuIconSprite.name = "GK2MoveStationsMoveIcon";
			return this.moveMenuIconSprite;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00009A40 File Offset: 0x00007C40
		private static void FillRect(Texture2D tex, int x0, int y0, int width, int height, Color color)
		{
			if (tex == null)
			{
				return;
			}
			for (int i = y0; i < y0 + height; i++)
			{
				for (int j = x0; j < x0 + width; j++)
				{
					if (j >= 0 && i >= 0 && j < tex.width && i < tex.height)
					{
						tex.SetPixel(j, i, color);
					}
				}
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00009A98 File Offset: 0x00007C98
		private void CaptureBuildMenuBuilder()
		{
			try
			{
				if (!(this.moveMenuRow == null) && !(this.uiBuildingWindowType == null) && !(this.uiBuildingWindowDataType == null))
				{
					Component componentInParent = this.moveMenuRow.GetComponentInParent(this.uiBuildingWindowType);
					if (!(componentInParent == null))
					{
						object prop = MoveStationsPlugin.GetProp(MoveStationsPlugin.FindReferenceOfType(componentInParent, this.uiBuildingWindowDataType), "AssignedWgo");
						if (prop is global::UnityEngine.Object && (global::UnityEngine.Object)prop != null)
						{
							this.moveMenuBuilderWgo = (global::UnityEngine.Object)prop;
							this.Dbg("build menu: captured builder '" + MoveStationsPlugin.PathOf(((Component)this.moveMenuBuilderWgo).gameObject) + "'");
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg("build menu: builder capture failed: " + ex.Message);
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00009B80 File Offset: 0x00007D80
		private void ReopenBuildMenu()
		{
			try
			{
				if (!(this.moveMenuBuilderWgo == null) && !(this.buildManagerType == null) && !(this.mBmTryEnable == null))
				{
					if (this.moveMenuRow != null)
					{
						global::UnityEngine.Object.Destroy(this.moveMenuRow);
					}
					this.moveMenuRow = null;
					this.moveMenuButton = null;
					global::UnityEngine.Object @object = global::UnityEngine.Object.FindObjectOfType(this.buildManagerType);
					if (!(@object == null))
					{
						MethodBase methodBase = this.mBmTryEnable;
						object obj = @object;
						object[] array = new object[2];
						array[0] = this.moveMenuBuilderWgo;
						object obj2 = methodBase.Invoke(obj, array);
						this.Dbg("build menu: reopen result=" + ((obj2 != null) ? obj2.ToString() : "(void)"));
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg("build menu: reopen failed: " + MoveStationsPlugin.UnwrapInvocationError(ex));
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00009C64 File Offset: 0x00007E64
		private static Button FindParentButton(Transform start)
		{
			Transform transform = start;
			int num = 0;
			while (transform != null && num < 8)
			{
				try
				{
					Button component = transform.GetComponent<Button>();
					if (component != null)
					{
						return component;
					}
				}
				catch
				{
				}
				num++;
				transform = transform.parent;
			}
			return null;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00009CBC File Offset: 0x00007EBC
		private void OnMoveMenuClicked()
		{
			this.CaptureBuildMenuBuilder();
			this.moveMenuArmed = true;
			this.moveMenuArmReadyAt = Time.realtimeSinceStartup + 0.25f;
			this.targetedWgo = null;
			this.lastCursorTargetLogged = null;
			this.SetHint("Click a station to move it.");
			try
			{
				if (this.mDisableBuildMode != null && this.buildControllerType != null)
				{
					global::UnityEngine.Object @object = global::UnityEngine.Object.FindObjectOfType(this.buildControllerType);
					if (@object != null && this.fBcBuildModeActive != null && MoveStationsPlugin.TryGet(this.fBcBuildModeActive, @object) is bool && (bool)MoveStationsPlugin.TryGet(this.fBcBuildModeActive, @object))
					{
						this.mDisableBuildMode.Invoke(@object, null);
					}
					else
					{
						this.Dbg("build menu: DisableBuildMode skipped (controller is not in build mode)");
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg("build menu: DisableBuildMode failed: " + MoveStationsPlugin.UnwrapInvocationError(ex));
			}
			try
			{
				if (!this.TryCloseBuildMenuUi(this.moveMenuRow))
				{
					this.Dbg("build menu: no vanilla close button found after Move selection");
				}
			}
			catch (Exception ex2)
			{
				this.Dbg("build menu: close UI failed: " + MoveStationsPlugin.UnwrapInvocationError(ex2));
			}
			this.TryStartNativeGridOnlyMode();
			this.Dbg("build menu: Move selected; waiting for station click");
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00009E04 File Offset: 0x00008004
		private bool TryCloseBuildMenuUi(GameObject moveRow)
		{
			if (moveRow == null)
			{
				return false;
			}
			try
			{
				Transform transform = moveRow.transform;
				int num = 0;
				while (transform != null && num < 12)
				{
					Button[] componentsInChildren = transform.GetComponentsInChildren<Button>(true);
					if (componentsInChildren != null)
					{
						foreach (Button button in componentsInChildren)
						{
							if (!(button == null) && !(button == this.moveMenuButton) && MoveStationsPlugin.LooksLikeCloseButton(button))
							{
								this.Dbg("build menu: invoking close button '" + MoveStationsPlugin.FullPathOf(button.gameObject) + "'");
								button.onClick.Invoke();
								return true;
							}
						}
					}
					num++;
					transform = transform.parent;
				}
			}
			catch (Exception ex)
			{
				this.Dbg("build menu: close button invoke failed: " + MoveStationsPlugin.UnwrapInvocationError(ex));
			}
			return false;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00009EF4 File Offset: 0x000080F4
		private static bool LooksLikeCloseButton(Button b)
		{
			if (b == null)
			{
				return false;
			}
			string text = (b.gameObject.name ?? "").ToLowerInvariant();
			if (text.IndexOf("remove", StringComparison.Ordinal) >= 0 || text.IndexOf("delete", StringComparison.Ordinal) >= 0)
			{
				return false;
			}
			if (text.IndexOf("close", StringComparison.Ordinal) >= 0 || text.IndexOf("exit", StringComparison.Ordinal) >= 0 || text.IndexOf("back", StringComparison.Ordinal) >= 0 || text.IndexOf("cancel", StringComparison.Ordinal) >= 0 || text.IndexOf("cross", StringComparison.Ordinal) >= 0 || text == "x" || text.IndexOf("xbutton", StringComparison.Ordinal) >= 0)
			{
				return true;
			}
			Image[] componentsInChildren = b.GetComponentsInChildren<Image>(true);
			if (componentsInChildren == null)
			{
				return false;
			}
			foreach (Image image in componentsInChildren)
			{
				if (!(image == null) && !(image.sprite == null))
				{
					string text2 = (image.sprite.name ?? "").ToLowerInvariant();
					if (text2.IndexOf("close", StringComparison.Ordinal) >= 0 || text2.IndexOf("exit", StringComparison.Ordinal) >= 0 || text2.IndexOf("cancel", StringComparison.Ordinal) >= 0 || text2.IndexOf("cross", StringComparison.Ordinal) >= 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000A050 File Offset: 0x00008250
		private static string UnwrapInvocationError(Exception e)
		{
			try
			{
				TargetInvocationException ex = e as TargetInvocationException;
				if (ex != null && ex.InnerException != null)
				{
					return ex.InnerException.GetType().Name + ": " + ex.InnerException.Message;
				}
			}
			catch
			{
			}
			return e.Message;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000A0B4 File Offset: 0x000082B4
		private void CreateMarker()
		{
			try
			{
				int num = 64;
				int num2 = 60;
				Texture2D texture2D = new Texture2D(num, num2, TextureFormat.RGBA32, false);
				Color color = new Color(1f, 1f, 1f, 0.1f);
				Color color2 = new Color(1f, 1f, 1f, 0.85f);
				for (int i = 0; i < num2; i++)
				{
					for (int j = 0; j < num; j++)
					{
						bool flag = j < 2 || j >= num - 2 || i < 2 || i >= num2 - 2;
						texture2D.SetPixel(j, i, flag ? color2 : color);
					}
				}
				texture2D.Apply();
				Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)num, (float)num2), new Vector2(0.5f, 0.5f), 100f);
				this.markerGO = new GameObject("GK2MoveStationsMarker");
				global::UnityEngine.Object.DontDestroyOnLoad(this.markerGO);
				this.markerSr = this.markerGO.AddComponent<SpriteRenderer>();
				this.markerSr.sprite = sprite;
				this.markerSr.sortingOrder = 32000;
				this.markerGO.SetActive(false);
			}
			catch (Exception ex)
			{
				base.Logger.LogWarning("Marker creation failed: " + ex.Message);
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000A224 File Offset: 0x00008424
		private void UpdateMarker()
		{
			if (this.markerGO == null)
			{
				return;
			}
			if (!this.markerGO.activeSelf)
			{
				this.markerGO.SetActive(true);
			}
			this.markerGO.transform.position = new Vector3(this.previewPos.x, this.previewPos.y + 0.05f, this.previewPos.z);
			this.markerGO.transform.localScale = new Vector3(this.footHalfX * 2f / 0.64f, this.footHalfZ * 2f / 0.6f, 1f);
			if (this.markerSr != null)
			{
				this.markerSr.color = (this.spotValid ? MoveStationsPlugin.ValidColor : MoveStationsPlugin.InvalidColor);
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000A300 File Offset: 0x00008500
		private void CreateUI()
		{
			try
			{
				GameObject gameObject = new GameObject("GK2MoveStationsCanvas");
				global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
				Canvas canvas = gameObject.AddComponent<Canvas>();
				canvas.renderMode = RenderMode.ScreenSpaceOverlay;
				canvas.sortingOrder = 30000;
				CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
				canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
				canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
				this.hintPanel = new GameObject("HintPanel");
				this.hintPanel.transform.SetParent(gameObject.transform, false);
				Image image = this.hintPanel.AddComponent<Image>();
				image.color = new Color(0f, 0f, 0f, 0.55f);
				image.raycastTarget = false;
				this.hintPanelRt = image.rectTransform;
				this.hintPanelRt.anchorMin = new Vector2(0.5f, 0f);
				this.hintPanelRt.anchorMax = new Vector2(0.5f, 0f);
				this.hintPanelRt.pivot = new Vector2(0.5f, 0f);
				this.hintPanelRt.anchoredPosition = new Vector2(0f, 150f);
				this.hintPanelRt.sizeDelta = new Vector2(760f, 56f);
				GameObject gameObject2 = new GameObject("HintText");
				gameObject2.transform.SetParent(this.hintPanel.transform, false);
				this.hintText = gameObject2.AddComponent<Text>();
				this.hintText.font = this.GetFont();
				this.hintText.fontSize = 16;
				this.hintText.lineSpacing = 1.05f;
				this.hintText.color = new Color(1f, 0.95f, 0.75f, 1f);
				this.hintText.alignment = TextAnchor.MiddleCenter;
				this.hintText.horizontalOverflow = HorizontalWrapMode.Overflow;
				this.hintText.verticalOverflow = VerticalWrapMode.Overflow;
				this.hintText.supportRichText = true;
				this.hintText.raycastTarget = false;
				RectTransform rectTransform = this.hintText.rectTransform;
				rectTransform.anchorMin = Vector2.zero;
				rectTransform.anchorMax = Vector2.one;
				rectTransform.pivot = new Vector2(0.5f, 0.5f);
				rectTransform.anchoredPosition = Vector2.zero;
				rectTransform.sizeDelta = new Vector2(-20f, -8f);
				this.keyCanvas = canvas;
				this.hintPanel.SetActive(false);
			}
			catch (Exception ex)
			{
				base.Logger.LogWarning("UI creation error: " + ex.Message);
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000A5A4 File Offset: 0x000087A4
		private void CreateMoveKeyLabel()
		{
			try
			{
				if (this.moveKeyGO != null)
				{
					try
					{
						global::UnityEngine.Object.Destroy(this.moveKeyGO);
					}
					catch
					{
					}
				}
				this.moveKeyGO = null;
				this.moveKeyRt = null;
				this.moveKeyText = null;
				this.moveKeyComp = null;
				this.moveKeyIsTmp = false;
				GameObject gameObject = new GameObject("GK2MoveStationsKey");
				gameObject.transform.SetParent(this.keyCanvas.transform, false);
				if (this.styleSrcIsTmp && this.styleSrc != null)
				{
					Type type = MoveStationsPlugin.FindTypeInAnyAssembly("TextMeshProUGUI");
					if (type != null)
					{
						try
						{
							Component component = gameObject.AddComponent(type);
							if (component != null)
							{
								MoveStationsPlugin.SetProp(component, "text", "Move");
								MoveStationsPlugin.SetProp(component, "raycastTarget", false);
								MoveStationsPlugin.SetProp(component, "enableWordWrapping", false);
								try
								{
									PropertyInfo property = type.GetProperty("alignment");
									if (property != null)
									{
										MoveStationsPlugin.SetProp(component, "alignment", Enum.Parse(property.PropertyType, "Bottom"));
									}
								}
								catch
								{
								}
								try
								{
									PropertyInfo property2 = type.GetProperty("overflowMode");
									if (property2 != null)
									{
										MoveStationsPlugin.SetProp(component, "overflowMode", Enum.Parse(property2.PropertyType, "Overflow"));
									}
								}
								catch
								{
								}
								MoveStationsPlugin.SetProp(component, "font", MoveStationsPlugin.GetProp(this.styleSrc, "font"));
								MoveStationsPlugin.SetProp(component, "fontSize", 28f);
								MoveStationsPlugin.SetProp(component, "color", MoveStationsPlugin.GetProp(this.styleSrc, "color"));
								MoveStationsPlugin.SetProp(component, "outlineColor", new Color(0f, 0f, 0f, 0.9f));
								MoveStationsPlugin.SetProp(component, "outlineWidth", 0.15f);
								this.moveKeyComp = component;
								this.moveKeyIsTmp = true;
							}
						}
						catch
						{
						}
					}
				}
				if (this.moveKeyComp == null)
				{
					Text text = gameObject.AddComponent<Text>();
					Font font = ((this.styleSrc != null) ? (MoveStationsPlugin.GetProp(this.styleSrc, "font") as Font) : null);
					text.font = ((font != null) ? font : this.GetFont());
					text.fontSize = Mathf.RoundToInt(28f);
					text.fontStyle = FontStyle.Normal;
					text.alignment = TextAnchor.LowerCenter;
					text.color = new Color(0.96f, 0.95f, 0.9f, 1f);
					text.supportRichText = true;
					text.horizontalOverflow = HorizontalWrapMode.Overflow;
					text.verticalOverflow = VerticalWrapMode.Overflow;
					text.raycastTarget = false;
					text.text = "Move";
					Outline outline = gameObject.AddComponent<Outline>();
					outline.effectColor = new Color(0f, 0f, 0f, 0.95f);
					outline.effectDistance = new Vector2(1.4f, -1.4f);
					this.moveKeyText = text;
					this.moveKeyComp = text;
				}
				this.moveKeyRt = gameObject.transform as RectTransform;
				this.moveKeyRt.anchorMin = new Vector2(0.5f, 0f);
				this.moveKeyRt.anchorMax = new Vector2(0.5f, 0f);
				this.moveKeyRt.pivot = new Vector2(0.5f, 0f);
				this.moveKeyRt.anchoredPosition = new Vector2(0f, 105f);
				this.moveKeyRt.sizeDelta = (this.moveKeyIsTmp ? new Vector2(360f, 48f) : new Vector2(300f, 40f));
				this.moveKeyGO = gameObject;
				gameObject.SetActive(false);
			}
			catch (Exception ex)
			{
				base.Logger.LogWarning("Key label creation error: " + ex.Message);
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000AA04 File Offset: 0x00008C04
		private Font GetFont()
		{
			try
			{
				return Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			}
			catch
			{
			}
			try
			{
				return Resources.GetBuiltinResource<Font>("Arial.ttf");
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000AA54 File Offset: 0x00008C54
		private void SetHint(string s)
		{
			if (this.hintPanel == null)
			{
				return;
			}
			if (s == null)
			{
				s = "";
			}
			bool flag = s.Length > 0;
			if (this.hintPanel.activeSelf != flag)
			{
				this.hintPanel.SetActive(flag);
			}
			if (!flag)
			{
				this.currentHint = "";
				return;
			}
			if (this.currentHint == s)
			{
				return;
			}
			this.currentHint = s;
			this.hintText.text = s;
			if (this.hintPanelRt != null)
			{
				int num = 1;
				for (int i = 0; i < s.Length; i++)
				{
					if (s[i] == '\n')
					{
						num++;
					}
				}
				this.hintPanelRt.sizeDelta = new Vector2(760f, (float)num * 21f + 12f);
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000AB24 File Offset: 0x00008D24
		private void UpdateMoveHint()
		{
			string text = "Moving station";
			if (this.debug)
			{
				if (this.selOverrideReady)
				{
					text += " <color=#9ED8FF>(game map)</color>";
				}
				else if (this.snapRejected)
				{
					text += " <color=#FFD24A>(collider rules)</color>";
				}
				else if (!this.snapValid)
				{
					text += " <color=#FFD24A>(grid not loaded)</color>";
				}
			}
			string text2 = (this.debug ? "<size=14>LMB: Place    R: Rotate    Esc / RMB: Cancel    Shift+LMB: Force place</size>" : "<size=14>LMB: Place    R: Rotate    Esc / RMB: Cancel</size>");
			string text3;
			if (this.spotValid)
			{
				text3 = text + "\n" + text2;
			}
			else
			{
				text3 = string.Concat(new string[] { text, "\n<color=#FF9E9E>Can't place here - ", this.spotReason, "</color>\n", text2 });
			}
			this.SetHint(text3);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000ABE0 File Offset: 0x00008DE0
		private void HideMoveKey()
		{
			try
			{
				if (this.moveKeyGO != null && this.moveKeyGO.activeSelf)
				{
					this.moveKeyGO.SetActive(false);
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000AC2C File Offset: 0x00008E2C
		private void UpdateMoveKeyLabel()
		{
			if (this.moveKeyGO == null || this.moveKeyRt == null)
			{
				return;
			}
			if (this.moving)
			{
				this.HideMoveKey();
				return;
			}
			if (this.moveMenuArmed)
			{
				if (this.targetedWgo == null)
				{
					this.HideMoveKey();
					return;
				}
				this.SetMoveKeyText("<color=#FFD24A>[LMB]</color> Move");
				if (!this.moveKeyGO.activeSelf)
				{
					this.moveKeyGO.SetActive(true);
				}
				return;
			}
			else
			{
				if (this.targetedWgo == null)
				{
					this.HideMoveKey();
					return;
				}
				if (!this.styleApplied && !this.styleGaveUp)
				{
					this.UpdateStyleProbe();
				}
				this.SetMoveKeyText("Move");
				if (!this.moveKeyGO.activeSelf)
				{
					this.moveKeyGO.SetActive(true);
				}
				return;
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000ACF8 File Offset: 0x00008EF8
		private void SetMoveKeyText(string value)
		{
			try
			{
				if (this.moveKeyIsTmp && this.moveKeyComp != null)
				{
					MoveStationsPlugin.SetProp(this.moveKeyComp, "text", value);
				}
				else if (this.moveKeyText != null)
				{
					this.moveKeyText.text = value;
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000AD60 File Offset: 0x00008F60
		private void UpdateStyleProbe()
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (realtimeSinceStartup < this.nextStyleTry)
			{
				return;
			}
			this.nextStyleTry = realtimeSinceStartup + 2f;
			this.styleTries++;
			if (this.styleTries > 15)
			{
				this.styleGaveUp = true;
				this.Dbg("key label: game prompt not found, keeping the default font");
				return;
			}
			Component component = null;
			bool flag = false;
			int num = 1;
			try
			{
				foreach (Text text in global::UnityEngine.Object.FindObjectsOfType<Text>())
				{
					if (!(text == null) && !(text.gameObject.name == "GK2MoveStationsKey"))
					{
						int num2 = MoveStationsPlugin.KeyPromptScore(text.text);
						if (num2 > num)
						{
							num = num2;
							component = text;
							flag = false;
						}
					}
				}
			}
			catch
			{
			}
			try
			{
				Type type = MoveStationsPlugin.FindTypeInAnyAssembly("TextMeshProUGUI");
				if (type != null)
				{
					global::UnityEngine.Object[] array2 = global::UnityEngine.Object.FindObjectsOfType(type);
					for (int j = 0; j < array2.Length; j++)
					{
						Component component2 = array2[j] as Component;
						if (!(component2 == null) && !(component2.gameObject.name == "GK2MoveStationsKey"))
						{
							int num3 = MoveStationsPlugin.KeyPromptScore(MoveStationsPlugin.GetProp(component2, "text") as string);
							if (num3 > num)
							{
								num = num3;
								component = component2;
								flag = true;
							}
						}
					}
				}
			}
			catch
			{
			}
			if (component == null)
			{
				return;
			}
			this.styleApplied = true;
			this.styleSrc = component;
			this.styleSrcIsTmp = flag;
			this.Dbg(string.Concat(new string[]
			{
				"key label: font copied from '",
				component.gameObject.name,
				"' (",
				component.GetType().Name,
				")"
			}));
			if (flag)
			{
				this.CreateMoveKeyLabel();
				return;
			}
			if (this.moveKeyText != null)
			{
				try
				{
					Font font = MoveStationsPlugin.GetProp(component, "font") as Font;
					if (font != null)
					{
						this.moveKeyText.font = font;
					}
					this.moveKeyText.fontSize = Mathf.RoundToInt(28f);
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000AF90 File Offset: 0x00009190
		private static int KeyPromptScore(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return -1;
			}
			if (s.Length > 64)
			{
				return -1;
			}
			if (s.IndexOf('[') < 0 || s.IndexOf(']') < 0)
			{
				return -1;
			}
			int num = 0;
			if (s.IndexOf("e]", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				num += 2;
			}
			if (s.IndexOf("craft", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				num++;
			}
			if (s.Length <= 24)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000B004 File Offset: 0x00009204
		private void DumpBuildSystemProbe()
		{
			if (!this.debug)
			{
				return;
			}
			try
			{
				this.Dbg("=== BUILD GRID PROBE ===");
				this.Dbg(string.Concat(new string[]
				{
					"reflection: BuildController=",
					(this.buildControllerType != null).ToString(),
					" buildLayoutField=",
					(this.fBcLayout != null).ToString(),
					" snapMethod=",
					(this.mSnapToBounds != null).ToString(),
					" buildModeField=",
					(this.fBcBuildModeActive != null).ToString(),
					" layoutGridField=",
					(this.fLayoutGridData != null).ToString(),
					" gridArrayField=",
					(this.fGridGridData != null).ToString(),
					" gridModeField=",
					(this.fGridBuildMode != null).ToString(),
					" selectionStateField=",
					(this.fSelState != null).ToString(),
					" selectionCoordsField=",
					(this.fSelCoords != null).ToString(),
					" buildCursor=",
					(this.fBcCurPos != null).ToString()
				}));
				this.DumpGridPresence();
				this.Dbg("=== BUILD GRID PROBE END ===");
			}
			catch (Exception ex)
			{
				this.Dbg("probe error: " + ex.Message);
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000B1C4 File Offset: 0x000093C4
		private void DumpGridPresence()
		{
			try
			{
				if (this.buildGridDataType != null)
				{
					global::UnityEngine.Object[] array = global::UnityEngine.Object.FindObjectsOfType(this.buildGridDataType);
					global::UnityEngine.Object[] array2 = Resources.FindObjectsOfTypeAll(this.buildGridDataType);
					this.Dbg("PROBE presence: BuildGridData active=" + ((array != null) ? array.Length : 0).ToString() + " all=" + ((array2 != null) ? array2.Length : 0).ToString());
					if (array2 != null)
					{
						int num = 0;
						foreach (global::UnityEngine.Object @object in array2)
						{
							if (!(@object == null))
							{
								num++;
								if (num > 3)
								{
									break;
								}
								Component component = @object as Component;
								string text = ((component != null) ? MoveStationsPlugin.PathOf(component.gameObject) : @object.name);
								bool flag = component != null && component.gameObject.activeInHierarchy;
								string text2 = this.ReadEnumField(this.fGridBuildMode, @object);
								string text3 = MoveStationsPlugin.ReadFieldState(this.fGridGridData, @object);
								string text4 = MoveStationsPlugin.ReadFieldState(this.fGridSelection, @object);
								this.Dbg(string.Concat(new string[]
								{
									"PROBE grid: '",
									text,
									"' active=",
									flag.ToString(),
									" buildModeField=",
									text2,
									" gridData=",
									text3,
									" selectionGridData=",
									text4
								}));
							}
						}
					}
				}
				if (this.buildLayoutType != null)
				{
					global::UnityEngine.Object[] array4 = global::UnityEngine.Object.FindObjectsOfType(this.buildLayoutType);
					global::UnityEngine.Object[] array5 = Resources.FindObjectsOfTypeAll(this.buildLayoutType);
					this.Dbg("PROBE presence: BuildLayout active=" + ((array4 != null) ? array4.Length : 0).ToString() + " all=" + ((array5 != null) ? array5.Length : 0).ToString());
				}
				this.Dbg("PROBE buildMode: isBuildModeActive=" + this.IsBuildModeActive().ToString());
				this.Dbg(string.Concat(new string[]
				{
					"PROBE snapshot: valid=",
					this.snapValid.ToString(),
					" cells=",
					((this.snapCells != null) ? this.snapCells.Count : 0).ToString(),
					" src='",
					this.snapSource,
					"'"
				}));
				this.Dbg("PROBE gameMap: ready=" + this.selOverrideReady.ToString() + " " + this.selCalib);
			}
			catch (Exception ex)
			{
				this.Dbg("PROBE presence error: " + ex.Message);
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000B488 File Offset: 0x00009688
		private void DumpBuildUiProbe()
		{
			if (!this.debug)
			{
				return;
			}
			try
			{
				this.Dbg("=== BUILD UI / VALIDATION PROBE ===");
				int num = 0;
				Text[] array = global::UnityEngine.Object.FindObjectsOfType<Text>();
				if (array != null)
				{
					foreach (Text text in array)
					{
						if (!(text == null) && MoveStationsPlugin.IsBuildMenuProbeText(text.text))
						{
							this.LogBuildUiCandidate(text.gameObject, text.text, ref num);
							if (num >= 40)
							{
								break;
							}
						}
					}
				}
				Type type = MoveStationsPlugin.FindTypeInAnyAssembly("TextMeshProUGUI");
				if (type != null && num < 40)
				{
					global::UnityEngine.Object[] array3 = global::UnityEngine.Object.FindObjectsOfType(type);
					if (array3 != null)
					{
						global::UnityEngine.Object[] array4 = array3;
						for (int i = 0; i < array4.Length; i++)
						{
							Component component = array4[i] as Component;
							if (!(component == null))
							{
								string text2 = MoveStationsPlugin.GetProp(component, "text") as string;
								if (MoveStationsPlugin.IsBuildMenuProbeText(text2))
								{
									this.LogBuildUiCandidate(component.gameObject, text2, ref num);
									if (num >= 40)
									{
										break;
									}
								}
							}
						}
					}
				}
				if (num == 0)
				{
					this.Dbg("build UI: no active Remove/Build/Yard text candidate found");
				}
				if (this.buildControllerType != null)
				{
					object obj = this.buildControllerInstance;
					if (obj == null || (global::UnityEngine.Object)obj == null)
					{
						obj = MoveStationsPlugin.FindAnyUnityObject(this.buildControllerType);
					}
					if (obj != null)
					{
						FieldInfo fieldHierarchy = MoveStationsPlugin.GetFieldHierarchy(this.buildControllerType, "currentBuildData");
						this.Dbg(string.Concat(new string[]
						{
							"BuildController state: IsBuildModeActive=",
							MoveStationsPlugin.ReadPropertyState(this.buildControllerType, "IsBuildModeActive", obj),
							" IsRemoveMode=",
							MoveStationsPlugin.ReadPropertyState(this.buildControllerType, "IsRemoveMode", obj),
							" currentBuildData=",
							MoveStationsPlugin.ReadFieldType(fieldHierarchy, obj)
						}));
					}
					int num2 = 0;
					foreach (MethodInfo methodInfo in this.buildControllerType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
					{
						if (!(methodInfo == null) && MoveStationsPlugin.IsBuildApiProbeName(methodInfo.Name))
						{
							this.Dbg("BuildController method: " + MoveStationsPlugin.FormatMethod(methodInfo));
							num2++;
							if (num2 >= 80)
							{
								break;
							}
						}
					}
					if (num2 == 0)
					{
						this.Dbg("BuildController methods: no candidate names found");
					}
					int num3 = 0;
					Type baseType = this.buildControllerType;
					while (baseType != null && baseType != typeof(object) && num3 < 80)
					{
						foreach (FieldInfo fieldInfo in baseType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
						{
							if (!(fieldInfo == null) && MoveStationsPlugin.IsBuildApiProbeName(fieldInfo.Name))
							{
								this.Dbg("BuildController field: " + fieldInfo.Name + " : " + fieldInfo.FieldType.Name);
								num3++;
								if (num3 >= 80)
								{
									break;
								}
							}
						}
						baseType = baseType.BaseType;
					}
					if (num3 == 0)
					{
						this.Dbg("BuildController fields: no candidate names found");
					}
				}
				else
				{
					this.Dbg("BuildController: type not resolved");
				}
				this.DumpBuildTypeApi("BuildPointer", false);
				this.DumpBuildTypeApi("BuildLayout", false);
				this.DumpBuildTypeApi("BuildGridData", false);
				this.DumpBuildTypeApi("BuildData", true);
				this.Dbg("=== BUILD UI / VALIDATION PROBE END ===");
			}
			catch (Exception ex)
			{
				this.Dbg("build UI probe error: " + ex.Message);
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000B7F8 File Offset: 0x000099F8
		private void DumpBuildTypeApi(string typeName, bool printAllMethods)
		{
			try
			{
				Type type = MoveStationsPlugin.FindType(typeName);
				if (type == null)
				{
					this.Dbg(typeName + ": type not resolved");
				}
				else
				{
					this.Dbg(typeName + ": resolved=" + type.FullName);
					MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					int num = 0;
					foreach (MethodInfo methodInfo in methods)
					{
						if (!(methodInfo == null) && (printAllMethods || MoveStationsPlugin.IsBuildApiProbeName(methodInfo.Name)))
						{
							this.Dbg(typeName + " method: " + MoveStationsPlugin.FormatMethod(methodInfo));
							num++;
							if (num >= 120)
							{
								break;
							}
						}
					}
					if (num == 0)
					{
						this.Dbg(typeName + " methods: no candidate names found");
					}
					int num2 = 0;
					Type type2 = type;
					while (type2 != null && type2 != typeof(object) && num2 < 120)
					{
						foreach (FieldInfo fieldInfo in type2.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
						{
							if (!(fieldInfo == null) && (printAllMethods || MoveStationsPlugin.IsBuildApiProbeName(fieldInfo.Name)))
							{
								this.Dbg(string.Concat(new string[]
								{
									typeName,
									" field: ",
									fieldInfo.Name,
									" : ",
									fieldInfo.FieldType.Name
								}));
								num2++;
								if (num2 >= 120)
								{
									break;
								}
							}
						}
						type2 = type2.BaseType;
					}
					if (num2 == 0)
					{
						this.Dbg(typeName + " fields: no candidate names found");
					}
				}
			}
			catch (Exception ex)
			{
				this.Dbg(typeName + " API probe error: " + ex.Message);
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000B9C0 File Offset: 0x00009BC0
		private static string ReadPropertyState(Type owner, string name, object instance)
		{
			string text;
			try
			{
				PropertyInfo propertyHierarchy = MoveStationsPlugin.GetPropertyHierarchy(owner, name);
				if (propertyHierarchy == null || instance == null)
				{
					text = "(missing)";
				}
				else
				{
					object value = propertyHierarchy.GetValue(instance, null);
					text = ((value == null) ? "null" : value.ToString());
				}
			}
			catch
			{
				text = "(error)";
			}
			return text;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000BA20 File Offset: 0x00009C20
		private static string ReadFieldType(FieldInfo f, object instance)
		{
			string text;
			try
			{
				if (f == null || instance == null)
				{
					text = "(missing)";
				}
				else
				{
					object value = f.GetValue(instance);
					text = ((value == null) ? "null" : value.GetType().Name);
				}
			}
			catch
			{
				text = "(error)";
			}
			return text;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000BA7C File Offset: 0x00009C7C
		private static bool IsBuildMenuProbeText(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			string text2 = text.Trim();
			return text2.Equals("Remove", StringComparison.OrdinalIgnoreCase) || text2.Equals("Move", StringComparison.OrdinalIgnoreCase) || text2.Equals("Stone stockpile", StringComparison.OrdinalIgnoreCase) || text2.Equals("Stone cutter", StringComparison.OrdinalIgnoreCase) || text2.Equals("Potter's wheel", StringComparison.OrdinalIgnoreCase) || text2.Equals("Stone cutter II", StringComparison.OrdinalIgnoreCase) || text2.Equals("Build", StringComparison.OrdinalIgnoreCase) || text2.Equals("Yard", StringComparison.OrdinalIgnoreCase) || text2.IndexOf("Remove", StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000BB20 File Offset: 0x00009D20
		private void LogBuildUiCandidate(GameObject go, string text, ref int count)
		{
			if (go == null)
			{
				return;
			}
			count++;
			Button button = null;
			Transform transform = go.transform;
			int num = 0;
			while (transform != null && num < 8)
			{
				try
				{
					button = transform.GetComponent<Button>();
					if (button != null)
					{
						break;
					}
				}
				catch
				{
				}
				num++;
				transform = transform.parent;
			}
			string text2 = ((button != null) ? MoveStationsPlugin.PathOf(button.gameObject) : "(no Button parent)");
			this.Dbg(string.Concat(new string[]
			{
				"build UI candidate: text='",
				text,
				"' object='",
				MoveStationsPlugin.PathOf(go),
				"' button='",
				text2,
				"'"
			}));
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000BBE8 File Offset: 0x00009DE8
		private static bool IsBuildApiProbeName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return false;
			}
			foreach (string text in new string[]
			{
				"Valid", "Place", "Build", "Check", "Can", "Select", "Remove", "Move", "Grid", "Cell",
				"Cursor", "Confirm", "Cancel"
			})
			{
				if (name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000BC94 File Offset: 0x00009E94
		private static string FormatMethod(MethodInfo m)
		{
			string text2;
			try
			{
				ParameterInfo[] parameters = m.GetParameters();
				string text = "";
				for (int i = 0; i < parameters.Length; i++)
				{
					if (i > 0)
					{
						text += ", ";
					}
					text += parameters[i].ParameterType.Name;
				}
				text2 = string.Concat(new string[]
				{
					m.Name,
					"(",
					text,
					") -> ",
					m.ReturnType.Name
				});
			}
			catch
			{
				text2 = m.Name;
			}
			return text2;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000BD34 File Offset: 0x00009F34
		private void InitReflection()
		{
			this.wgoType = MoveStationsPlugin.FindType("Wgo");
			this.wgoDataType = MoveStationsPlugin.FindType("WgoData");
			this.defType = MoveStationsPlugin.FindType("WGODef");
			this.bDefType = MoveStationsPlugin.FindType("BuildingDef");
			this.buildAreaType = MoveStationsPlugin.FindType("BuildArea");
			this.buildDataType = MoveStationsPlugin.FindType("BuildData");
			this.buildPointerType = MoveStationsPlugin.FindType("BuildPointer");
			this.buildPointerObjectType = MoveStationsPlugin.FindType("BuildPointerObject");
			this.worldZoneType = MoveStationsPlugin.FindType("WorldZone");
			this.buildManagerType = MoveStationsPlugin.FindType("BuildManager");
			this.playerControllerType = MoveStationsPlugin.FindType("PlayerController");
			this.takenControlType = MoveStationsPlugin.FindType("TakenControlType");
			this.uiBuildingWindowType = MoveStationsPlugin.FindType("UIBuildingWindow");
			this.uiBuildingWindowDataType = MoveStationsPlugin.FindType("UIBuildingWindowData");
			this.uiBuildingWidgetType = MoveStationsPlugin.FindType("UIBuildingWidget");
			if (this.uiBuildingWidgetType != null)
			{
				this.fUiBuildingWidgetData = MoveStationsPlugin.GetFieldHierarchy(this.uiBuildingWidgetType, "data");
			}
			if (this.wgoType != null)
			{
				this.fWgoData = MoveStationsPlugin.GetFieldHierarchy(this.wgoType, "data");
				this.fWgoHandler = MoveStationsPlugin.GetFieldHierarchy(this.wgoType, "interactionHandler");
				this.mTryRotate = MoveStationsPlugin.GetMethodHierarchy(this.wgoType, "TryRotate");
				this.mWgoRegisterGdPoints = MoveStationsPlugin.GetMethodAnyHierarchy(this.wgoType, "RegisterGDPointsFromBakedData");
				this.mWgoBindGdPointViews = MoveStationsPlugin.GetMethodAnyHierarchy(this.wgoType, "BindGDPointViews");
				this.pWgoRegistered = MoveStationsPlugin.GetPropertyHierarchy(this.wgoType, "RegisteredInChunker");
			}
			if (this.wgoDataType != null)
			{
				this.pDataPosition = MoveStationsPlugin.GetPropertyHierarchy(this.wgoDataType, "Position");
				this.fWgoDataTemp = MoveStationsPlugin.GetFieldHierarchy(this.wgoDataType, "isTempObject");
				this.fWgoGdPointsData = MoveStationsPlugin.GetFieldHierarchy(this.wgoDataType, "gdPointsData");
			}
			if (this.defType != null)
			{
				this.fDefAreaId = MoveStationsPlugin.GetFieldHierarchy(this.defType, "customBuildAreaId");
				this.mTryGetBDef = MoveStationsPlugin.GetMethodHierarchy(this.defType, "TryGetBuildingDefForWgo");
			}
			if (this.bDefType != null)
			{
				this.fBDefAreaId = MoveStationsPlugin.GetFieldHierarchy(this.bDefType, "customBuildAreaId");
				this.fBDefWgoId = MoveStationsPlugin.GetFieldHierarchy(this.bDefType, "wgoId");
			}
			if (this.buildAreaType != null)
			{
				this.fAreaId = MoveStationsPlugin.GetFieldHierarchy(this.buildAreaType, "id");
			}
			Type type = MoveStationsPlugin.FindType("WGOInteractionHandlerBase");
			if (type != null)
			{
				this.fHandlerInteractor = MoveStationsPlugin.GetFieldHierarchy(type, "interactor");
			}
			Type type2 = MoveStationsPlugin.FindType("BuildController");
			if (type2 != null)
			{
				this.buildControllerType = type2;
				this.mSnapToBounds = MoveStationsPlugin.GetMethodAnyHierarchy(type2, "SnapToBounds");
				this.mDisableBuildMode = MoveStationsPlugin.GetMethodAnyHierarchy(type2, "DisableBuildMode");
				this.mBcEnableBuildMode = MoveStationsPlugin.GetMethodAnyHierarchy(type2, "EnableBuildMode");
				this.mBcUpdatePointerObjectPosition = MoveStationsPlugin.GetMethodAnyHierarchy(type2, "UpdatePointerObjectPosition");
				this.mBcUpdatePointerAtPos = MoveStationsPlugin.GetMethodAnyHierarchy(type2, "UpdatePointerAtPos");
				this.fBcLayout = MoveStationsPlugin.GetFieldHierarchy(type2, "buildLayout");
				this.fBcBuildModeActive = MoveStationsPlugin.GetFieldHierarchy(type2, "isBuildModeActive");
				this.fBcBuildPointer = MoveStationsPlugin.GetFieldHierarchy(type2, "buildPointer");
				this.fBcInputLocked = MoveStationsPlugin.GetFieldHierarchy(type2, "isBuildModeInputLocked");
				this.fBcCurPos = MoveStationsPlugin.GetFieldHierarchy(type2, "curPosActual");
				this.fBcLastSnapped = MoveStationsPlugin.GetFieldHierarchy(type2, "lastSnappedCursorPos");
			}
			if (this.buildDataType != null)
			{
				this.mBuildDataForBuild = MoveStationsPlugin.GetMethodAnyHierarchy(this.buildDataType, "GetDataForBuild");
				this.mBuildDataForRemove = MoveStationsPlugin.GetMethodAnyHierarchy(this.buildDataType, "GetDataForRemove");
			}
			if (this.buildPointerType != null)
			{
				this.mBpUpdateAvailability = MoveStationsPlugin.GetMethodAnyHierarchy(this.buildPointerType, "UpdateAvailability");
				this.mBpRotate = MoveStationsPlugin.GetMethodAnyHierarchy(this.buildPointerType, "Rotate");
			}
			if (this.buildPointerObjectType != null)
			{
				this.fPointerShownAsActive = MoveStationsPlugin.GetFieldHierarchy(this.buildPointerObjectType, "shownAsActive");
			}
			if (this.buildManagerType != null)
			{
				this.pBmWorldZone = MoveStationsPlugin.GetPropertyHierarchy(this.buildManagerType, "WorldZone");
				this.mBmTryEnable = MoveStationsPlugin.GetMethodAnyHierarchy(this.buildManagerType, "TryEnable");
			}
			if (this.playerControllerType != null)
			{
				this.mPcSetControlTakenType = MoveStationsPlugin.GetMethodAnyHierarchy(this.playerControllerType, "SetControlTakenType");
			}
			this.buildLayoutType = MoveStationsPlugin.FindType("BuildLayout");
			this.buildGridDataType = MoveStationsPlugin.FindType("BuildGridData");
			this.buildCellDataType = MoveStationsPlugin.FindType("BuildCellData");
			this.buildCellSelectionType = MoveStationsPlugin.FindType("BuildCellSelectionData");
			if (this.buildCellSelectionType != null)
			{
				this.fSelState = MoveStationsPlugin.GetFieldHierarchy(this.buildCellSelectionType, "state");
				this.fSelCoords = MoveStationsPlugin.GetFieldHierarchy(this.buildCellSelectionType, "coords");
			}
			if (this.buildLayoutType != null)
			{
				this.fLayoutGridData = MoveStationsPlugin.GetFieldHierarchy(this.buildLayoutType, "buildGridData");
			}
			if (this.buildGridDataType != null)
			{
				this.fGridGridData = MoveStationsPlugin.GetFieldHierarchy(this.buildGridDataType, "gridData");
				this.fGridBuildMode = MoveStationsPlugin.GetFieldHierarchy(this.buildGridDataType, "buildMode");
				this.fGridSelection = MoveStationsPlugin.GetFieldHierarchy(this.buildGridDataType, "selectionGridData");
				this.fGridCurrentDef = MoveStationsPlugin.GetFieldHierarchy(this.buildGridDataType, "currentBuildingDef");
				this.fGridBusySlots = MoveStationsPlugin.GetFieldHierarchy(this.buildGridDataType, "busySlotAreas");
				this.fGridFreeSlots = MoveStationsPlugin.GetFieldHierarchy(this.buildGridDataType, "freeSlotAreas");
			}
			if (this.buildCellDataType != null)
			{
				this.fCellCoords = MoveStationsPlugin.GetFieldHierarchy(this.buildCellDataType, "coords");
				this.fCellDef = MoveStationsPlugin.GetFieldHierarchy(this.buildCellDataType, "buildingDef");
				this.fCellState = MoveStationsPlugin.GetFieldHierarchy(this.buildCellDataType, "state");
			}
			this.reflectionOk = this.wgoType != null && this.wgoDataType != null && this.fWgoData != null && this.fWgoHandler != null && this.fHandlerInteractor != null && this.pDataPosition != null;
			if (this.debug)
			{
				base.Logger.LogInfo(string.Concat(new string[]
				{
					"Reflection ready: Position=",
					(this.pDataPosition != null).ToString(),
					", TryRotate=",
					(this.mTryRotate != null).ToString(),
					", BuildingDef lookup=",
					(this.fBDefAreaId != null).ToString(),
					", BuildArea id=",
					(this.fAreaId != null).ToString(),
					", GameSnap=",
					(this.mSnapToBounds != null).ToString(),
					", BuildGrid=",
					(this.fGridGridData != null && this.fCellDef != null && this.fCellCoords != null).ToString(),
					", BuildModeField=",
					(this.fBcBuildModeActive != null).ToString(),
					", SelectionCell=",
					(this.fSelState != null && this.fSelCoords != null).ToString(),
					", BuildCursor=",
					(this.fBcCurPos != null).ToString(),
					", NativeBuildPreview=",
					(this.mBuildDataForBuild != null && this.mBcEnableBuildMode != null && this.mBcUpdatePointerAtPos != null && this.fBcBuildPointer != null && this.fPointerShownAsActive != null).ToString(),
					", NativeGrid=",
					(this.mBuildDataForRemove != null && this.mBcEnableBuildMode != null && this.fBcBuildPointer != null).ToString(),
					", BuildMenuZone=",
					(this.pBmWorldZone != null).ToString(),
					", BuildControlLock=",
					(this.mPcSetControlTakenType != null && this.takenControlType != null).ToString()
				}));
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000C608 File Offset: 0x0000A808
		private static Type FindType(string name)
		{
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (assembly.FullName.Contains("Assembly-CSharp"))
				{
					Type type = MoveStationsPlugin.FindTypeInAssembly(assembly, name);
					if (type != null)
					{
						return type;
					}
				}
			}
			return null;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000C658 File Offset: 0x0000A858
		private static Type FindTypeInAnyAssembly(string name)
		{
			try
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int i = 0; i < assemblies.Length; i++)
				{
					Type type = MoveStationsPlugin.FindTypeInAssembly(assemblies[i], name);
					if (type != null)
					{
						return type;
					}
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000C6B0 File Offset: 0x0000A8B0
		private static Type FindTypeInAssembly(Assembly asm, string name)
		{
			Type[] array = null;
			try
			{
				array = asm.GetTypes();
			}
			catch
			{
				return null;
			}
			foreach (Type type in array)
			{
				if (type.Name == name)
				{
					return type;
				}
			}
			return null;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000C708 File Offset: 0x0000A908
		private static FieldInfo GetFieldHierarchy(Type start, string name)
		{
			Type type = start;
			while (type != null && type != typeof(object))
			{
				FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null)
				{
					return field;
				}
				type = type.BaseType;
			}
			return null;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000C754 File Offset: 0x0000A954
		private static PropertyInfo GetPropertyHierarchy(Type start, string name)
		{
			Type type = start;
			while (type != null && type != typeof(object))
			{
				PropertyInfo property = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property != null)
				{
					return property;
				}
				type = type.BaseType;
			}
			return null;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000C7A0 File Offset: 0x0000A9A0
		private static MethodInfo GetMethodHierarchy(Type start, string name)
		{
			Type type = start;
			while (type != null && type != typeof(object))
			{
				MethodInfo method = type.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (method != null)
				{
					return method;
				}
				type = type.BaseType;
			}
			return null;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000C7EC File Offset: 0x0000A9EC
		private static MethodInfo GetMethodAnyHierarchy(Type start, string name)
		{
			Type type = start;
			while (type != null && type != typeof(object))
			{
				try
				{
					MethodInfo method = type.GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					if (method != null)
					{
						return method;
					}
				}
				catch
				{
				}
				type = type.BaseType;
			}
			return null;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000C850 File Offset: 0x0000AA50
		private static object TryGet(FieldInfo f, object o)
		{
			if (f == null || o == null)
			{
				return null;
			}
			object obj;
			try
			{
				obj = f.GetValue(o);
			}
			catch
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000C88C File Offset: 0x0000AA8C
		private static object FindReferenceOfType(object obj, Type targetType)
		{
			if (obj == null || targetType == null)
			{
				return null;
			}
			Type type = obj.GetType();
			while (type != null && type != typeof(object))
			{
				foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (targetType.IsAssignableFrom(fieldInfo.FieldType))
					{
						try
						{
							object value = fieldInfo.GetValue(obj);
							if (value != null)
							{
								return value;
							}
						}
						catch
						{
						}
					}
				}
				foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (targetType.IsAssignableFrom(propertyInfo.PropertyType))
					{
						try
						{
							MethodInfo getMethod = propertyInfo.GetGetMethod(true);
							if (!(getMethod == null))
							{
								object obj2 = getMethod.Invoke(obj, null);
								if (obj2 != null)
								{
									return obj2;
								}
							}
						}
						catch
						{
						}
					}
				}
				type = type.BaseType;
			}
			return null;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000C99C File Offset: 0x0000AB9C
		private string ShortId(object data)
		{
			string text;
			try
			{
				if (data == null || this.wgoDataType == null)
				{
					text = "?";
				}
				else
				{
					object obj = MoveStationsPlugin.TryGet(MoveStationsPlugin.GetFieldHierarchy(this.wgoDataType, "uniqueId"), data);
					if (obj == null)
					{
						text = "?";
					}
					else
					{
						PropertyInfo property = obj.GetType().GetProperty("Id");
						if (property == null)
						{
							text = "?";
						}
						else
						{
							object value = property.GetValue(obj, null);
							string text2 = ((value != null) ? value.ToString() : "?");
							text = ((text2.Length > 8) ? text2.Substring(0, 8) : text2);
						}
					}
				}
			}
			catch
			{
				text = "?";
			}
			return text;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000CA5C File Offset: 0x0000AC5C
		private static string PathOf(GameObject go)
		{
			if (go == null)
			{
				return "null";
			}
			string text = go.name;
			Transform transform = go.transform.parent;
			int num = 0;
			while (transform != null && num < 3)
			{
				text = transform.name + "/" + text;
				transform = transform.parent;
				num++;
			}
			return text;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000CABC File Offset: 0x0000ACBC
		private static string FullPathOf(GameObject go)
		{
			if (go == null)
			{
				return "null";
			}
			string text = go.name;
			Transform transform = go.transform.parent;
			int num = 0;
			while (transform != null && num++ < 32)
			{
				text = transform.name + "/" + text;
				transform = transform.parent;
			}
			return text;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000CB1B File Offset: 0x0000AD1B
		private static string Fmt(Vector3 v)
		{
			return v.ToString("F2");
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000CB29 File Offset: 0x0000AD29
		private void Dbg(string msg)
		{
			if (this.debug)
			{
				base.Logger.LogInfo("[MoveStations] " + msg);
			}
		}

		// Token: 0x04000001 RID: 1
		private const float GridX = 0.32f;

		// Token: 0x04000002 RID: 2
		private const float GridZ = 0.3f;

		// Token: 0x04000003 RID: 3
		private const int OurBlockMask = 524544;

		// Token: 0x04000004 RID: 4
		private const int GameOverlapMask = 537592064;

		// Token: 0x04000005 RID: 5
		private const float MaxDistanceFromPlayer = 22f;

		// Token: 0x04000006 RID: 6
		private const float ScanInterval = 0.15f;

		// Token: 0x04000007 RID: 7
		private const string Version = "2.0.3";

		// Token: 0x04000008 RID: 8
		private Type wgoType;

		// Token: 0x04000009 RID: 9
		private Type wgoDataType;

		// Token: 0x0400000A RID: 10
		private Type defType;

		// Token: 0x0400000B RID: 11
		private Type bDefType;

		// Token: 0x0400000C RID: 12
		private Type buildAreaType;

		// Token: 0x0400000D RID: 13
		private Type buildControllerType;

		// Token: 0x0400000E RID: 14
		private Type buildLayoutType;

		// Token: 0x0400000F RID: 15
		private Type buildGridDataType;

		// Token: 0x04000010 RID: 16
		private Type buildCellDataType;

		// Token: 0x04000011 RID: 17
		private Type buildCellSelectionType;

		// Token: 0x04000012 RID: 18
		private Type buildDataType;

		// Token: 0x04000013 RID: 19
		private Type buildPointerType;

		// Token: 0x04000014 RID: 20
		private Type buildPointerObjectType;

		// Token: 0x04000015 RID: 21
		private Type worldZoneType;

		// Token: 0x04000016 RID: 22
		private Type buildManagerType;

		// Token: 0x04000017 RID: 23
		private Type playerControllerType;

		// Token: 0x04000018 RID: 24
		private Type takenControlType;

		// Token: 0x04000019 RID: 25
		private Type uiBuildingWindowType;

		// Token: 0x0400001A RID: 26
		private Type uiBuildingWindowDataType;

		// Token: 0x0400001B RID: 27
		private FieldInfo fWgoData;

		// Token: 0x0400001C RID: 28
		private FieldInfo fWgoHandler;

		// Token: 0x0400001D RID: 29
		private FieldInfo fHandlerInteractor;

		// Token: 0x0400001E RID: 30
		private FieldInfo fDefAreaId;

		// Token: 0x0400001F RID: 31
		private FieldInfo fBDefAreaId;

		// Token: 0x04000020 RID: 32
		private FieldInfo fAreaId;

		// Token: 0x04000021 RID: 33
		private FieldInfo fBDefWgoId;

		// Token: 0x04000022 RID: 34
		private PropertyInfo pDataPosition;

		// Token: 0x04000023 RID: 35
		private PropertyInfo pWgoRegistered;

		// Token: 0x04000024 RID: 36
		private MethodInfo mTryRotate;

		// Token: 0x04000025 RID: 37
		private MethodInfo mTryGetBDef;

		// Token: 0x04000026 RID: 38
		private MethodInfo mWgoRegisterGdPoints;

		// Token: 0x04000027 RID: 39
		private MethodInfo mWgoBindGdPointViews;

		// Token: 0x04000028 RID: 40
		private MethodInfo mSnapToBounds;

		// Token: 0x04000029 RID: 41
		private MethodInfo mBuildDataForBuild;

		// Token: 0x0400002A RID: 42
		private MethodInfo mBuildDataForRemove;

		// Token: 0x0400002B RID: 43
		private MethodInfo mBcEnableBuildMode;

		// Token: 0x0400002C RID: 44
		private MethodInfo mBcUpdatePointerObjectPosition;

		// Token: 0x0400002D RID: 45
		private MethodInfo mBcUpdatePointerAtPos;

		// Token: 0x0400002E RID: 46
		private MethodInfo mBpUpdateAvailability;

		// Token: 0x0400002F RID: 47
		private MethodInfo mBpRotate;

		// Token: 0x04000030 RID: 48
		private MethodInfo mPcSetControlTakenType;

		// Token: 0x04000031 RID: 49
		private MethodInfo mBmTryEnable;

		// Token: 0x04000032 RID: 50
		private FieldInfo fBcLayout;

		// Token: 0x04000033 RID: 51
		private FieldInfo fBcBuildModeActive;

		// Token: 0x04000034 RID: 52
		private FieldInfo fBcBuildPointer;

		// Token: 0x04000035 RID: 53
		private FieldInfo fBcInputLocked;

		// Token: 0x04000036 RID: 54
		private FieldInfo fLayoutGridData;

		// Token: 0x04000037 RID: 55
		private FieldInfo fGridGridData;

		// Token: 0x04000038 RID: 56
		private FieldInfo fGridBuildMode;

		// Token: 0x04000039 RID: 57
		private FieldInfo fGridSelection;

		// Token: 0x0400003A RID: 58
		private FieldInfo fCellDef;

		// Token: 0x0400003B RID: 59
		private FieldInfo fCellCoords;

		// Token: 0x0400003C RID: 60
		private FieldInfo fCellState;

		// Token: 0x0400003D RID: 61
		private FieldInfo fGridCurrentDef;

		// Token: 0x0400003E RID: 62
		private FieldInfo fGridBusySlots;

		// Token: 0x0400003F RID: 63
		private FieldInfo fGridFreeSlots;

		// Token: 0x04000040 RID: 64
		private FieldInfo fSelState;

		// Token: 0x04000041 RID: 65
		private FieldInfo fSelCoords;

		// Token: 0x04000042 RID: 66
		private FieldInfo fBcCurPos;

		// Token: 0x04000043 RID: 67
		private FieldInfo fBcLastSnapped;

		// Token: 0x04000044 RID: 68
		private FieldInfo fPointerShownAsActive;

		// Token: 0x04000045 RID: 69
		private FieldInfo fWgoDataTemp;

		// Token: 0x04000046 RID: 70
		private FieldInfo fWgoGdPointsData;

		// Token: 0x04000047 RID: 71
		private PropertyInfo pBmWorldZone;

		// Token: 0x04000048 RID: 72
		private bool selMismatchLogged;

		// Token: 0x04000049 RID: 73
		private bool selCaptured;

		// Token: 0x0400004A RID: 74
		private float nextSelTry;

		// Token: 0x0400004B RID: 75
		private Dictionary<Vector2Int, MoveStationsPlugin.SelCell> selStates;

		// Token: 0x0400004C RID: 76
		private bool selOverrideReady;

		// Token: 0x0400004D RID: 77
		private int selBlockedValue = int.MinValue;

		// Token: 0x0400004E RID: 78
		private int selFreeValue = int.MinValue;

		// Token: 0x0400004F RID: 79
		private string selDefName = "";

		// Token: 0x04000050 RID: 80
		private string selCalib = "(not calibrated)";

		// Token: 0x04000051 RID: 81
		private global::UnityEngine.Object buildControllerInstance;

		// Token: 0x04000052 RID: 82
		private int snapErrors;

		// Token: 0x04000053 RID: 83
		private bool reflectionOk;

		// Token: 0x04000054 RID: 84
		private Dictionary<Vector2Int, MoveStationsPlugin.SnapCell> snapCells;

		// Token: 0x04000055 RID: 85
		private bool snapValid;

		// Token: 0x04000056 RID: 86
		private string snapSource = "";

		// Token: 0x04000057 RID: 87
		private int snapMinX;

		// Token: 0x04000058 RID: 88
		private int snapMaxX;

		// Token: 0x04000059 RID: 89
		private int snapMinZ;

		// Token: 0x0400005A RID: 90
		private int snapMaxZ;

		// Token: 0x0400005B RID: 91
		private float nextSnapTry;

		// Token: 0x0400005C RID: 92
		private float lastSnapRefresh;

		// Token: 0x0400005D RID: 93
		private int snapCountLogged = -1;

		// Token: 0x0400005E RID: 94
		private string gridStatusMsg = "";

		// Token: 0x0400005F RID: 95
		private bool snapRejected;

		// Token: 0x04000060 RID: 96
		private string snapRejectReason = "";

		// Token: 0x04000061 RID: 97
		private bool gridRejectLogged;

		// Token: 0x04000062 RID: 98
		private bool snapDumpDone;

		// Token: 0x04000063 RID: 99
		private bool buildModeSeen;

		// Token: 0x04000064 RID: 100
		private bool lastBuildModeActive;

		// Token: 0x04000065 RID: 101
		private HashSet<Vector2Int> ownCells;

		// Token: 0x04000066 RID: 102
		private List<Vector2Int> ownOffsets;

		// Token: 0x04000067 RID: 103
		private string lastGridResult = "";

		// Token: 0x04000068 RID: 104
		private string lastGridOobKey = "";

		// Token: 0x04000069 RID: 105
		private ConfigEntry<bool> debugLogs;

		// Token: 0x0400006A RID: 106
		private bool debug;

		// Token: 0x0400006B RID: 107
		private Camera worldCam;

		// Token: 0x0400006C RID: 108
		private bool camLogged;

		// Token: 0x0400006D RID: 109
		private bool camDumpDone;

		// Token: 0x0400006E RID: 110
		private bool snapLogged;

		// Token: 0x0400006F RID: 111
		private bool snapRejectLogged;

		// Token: 0x04000070 RID: 112
		private global::UnityEngine.Object cachedGridObj;

		// Token: 0x04000071 RID: 113
		private float nextGridFind;

		// Token: 0x04000072 RID: 114
		private float nextBcFind;

		// Token: 0x04000073 RID: 115
		private global::UnityEngine.Object[] wgoCache;

		// Token: 0x04000074 RID: 116
		private float nextWgoRefresh;

		// Token: 0x04000075 RID: 117
		private static global::UnityEngine.Object cachedAnyObj;

		// Token: 0x04000076 RID: 118
		private static float nextAnyFind;

		// Token: 0x04000077 RID: 119
		private GameObject moveKeyGO;

		// Token: 0x04000078 RID: 120
		private RectTransform moveKeyRt;

		// Token: 0x04000079 RID: 121
		private Text moveKeyText;

		// Token: 0x0400007A RID: 122
		private Component moveKeyComp;

		// Token: 0x0400007B RID: 123
		private bool moveKeyIsTmp;

		// Token: 0x0400007C RID: 124
		private Canvas keyCanvas;

		// Token: 0x0400007D RID: 125
		private const float KeyLabelBottomOffset = 105f;

		// Token: 0x0400007E RID: 126
		private const float KeyLabelFontSize = 28f;

		// Token: 0x0400007F RID: 127
		private Component styleSrc;

		// Token: 0x04000080 RID: 128
		private bool styleSrcIsTmp;

		// Token: 0x04000081 RID: 129
		private bool styleApplied;

		// Token: 0x04000082 RID: 130
		private bool styleGaveUp;

		// Token: 0x04000083 RID: 131
		private int styleTries;

		// Token: 0x04000084 RID: 132
		private float nextStyleTry;

		// Token: 0x04000085 RID: 133
		private float nextScan;

		// Token: 0x04000086 RID: 134
		private global::UnityEngine.Object targetedWgo;

		// Token: 0x04000087 RID: 135
		private global::UnityEngine.Object lastCursorTargetLogged;

		// Token: 0x04000088 RID: 136
		private bool moving;

		// Token: 0x04000089 RID: 137
		private global::UnityEngine.Object movingWgo;

		// Token: 0x0400008A RID: 138
		private GameObject movingGo;

		// Token: 0x0400008B RID: 139
		private object movingData;

		// Token: 0x0400008C RID: 140
		private object movingBuildingDef;

		// Token: 0x0400008D RID: 141
		private Vector3 originalPos;

		// Token: 0x0400008E RID: 142
		private Vector3 previewPos;

		// Token: 0x0400008F RID: 143
		private string requiredAreaId = "";

		// Token: 0x04000090 RID: 144
		private string movingWgoId = "";

		// Token: 0x04000091 RID: 145
		private bool nativeBuildPreview;

		// Token: 0x04000092 RID: 146
		private bool nativeBuildModeEnabled;

		// Token: 0x04000093 RID: 147
		private bool nativeGridOnlyMode;

		// Token: 0x04000094 RID: 148
		private bool nativePlayerControlsTaken;

		// Token: 0x04000095 RID: 149
		private bool movingGoWasActive;

		// Token: 0x04000096 RID: 150
		private bool movingDataWasTemp;

		// Token: 0x04000097 RID: 151
		private object nativePointer;

		// Token: 0x04000098 RID: 152
		private object nativePointerObject;

		// Token: 0x04000099 RID: 153
		private object nativeWorldZone;

		// Token: 0x0400009A RID: 154
		private float footHalfX = 0.32f;

		// Token: 0x0400009B RID: 155
		private float footHalfZ = 0.3f;

		// Token: 0x0400009C RID: 156
		private bool hasValidated;

		// Token: 0x0400009D RID: 157
		private Vector3 lastValidatedPos;

		// Token: 0x0400009E RID: 158
		private bool spotValid;

		// Token: 0x0400009F RID: 159
		private string spotReason = "";

		// Token: 0x040000A0 RID: 160
		private bool verifyActive;

		// Token: 0x040000A1 RID: 161
		private float verifyTime;

		// Token: 0x040000A2 RID: 162
		private object verifyData;

		// Token: 0x040000A3 RID: 163
		private GameObject verifyGo;

		// Token: 0x040000A4 RID: 164
		private Vector3 verifyExpected;

		// Token: 0x040000A5 RID: 165
		private string verifyLabel = "";

		// Token: 0x040000A6 RID: 166
		private GameObject markerGO;

		// Token: 0x040000A7 RID: 167
		private SpriteRenderer markerSr;

		// Token: 0x040000A8 RID: 168
		private static readonly Color ValidColor = new Color(0.45f, 1f, 0.5f, 0.95f);

		// Token: 0x040000A9 RID: 169
		private static readonly Color InvalidColor = new Color(1f, 0.45f, 0.4f, 0.95f);

		// Token: 0x040000AA RID: 170
		private GameObject hintPanel;

		// Token: 0x040000AB RID: 171
		private RectTransform hintPanelRt;

		// Token: 0x040000AC RID: 172
		private Text hintText;

		// Token: 0x040000AD RID: 173
		private string currentHint = "";

		// Token: 0x040000AE RID: 174
		private GameObject moveMenuRow;

		// Token: 0x040000AF RID: 175
		private Button moveMenuButton;

		// Token: 0x040000B0 RID: 176
		private Sprite moveMenuIconSprite;

		// Token: 0x040000B1 RID: 177
		private global::UnityEngine.Object moveMenuBuilderWgo;

		// Token: 0x040000B2 RID: 178
		private float nextMoveMenuSearch;

		// Token: 0x040000B3 RID: 179
		private GameObject cachedBuildMenuWindow;

		// Token: 0x040000B4 RID: 180
		private float nextBuildMenuWindowLookup;

		// Token: 0x040000B5 RID: 181
		private bool buildMenuOpenEventsReady;

		// Token: 0x040000B6 RID: 182
		private bool buildMenuInjectionRequested;

		// Token: 0x040000B7 RID: 183
		private EventInfo eLazyWindowOpened;

		// Token: 0x040000B8 RID: 184
		private Delegate dLazyWindowOpened;

		// Token: 0x040000B9 RID: 185
		private bool moveMenuArmed;

		// Token: 0x040000BA RID: 186
		private float moveMenuArmReadyAt;

		// Token: 0x040000BB RID: 187
		private MethodInfo mDisableBuildMode;

		// Token: 0x040000BC RID: 188
		private Type uiBuildingWidgetType;

		// Token: 0x040000BD RID: 189
		private FieldInfo fUiBuildingWidgetData;

		// Token: 0x02000003 RID: 3
		private class SelCell
		{
			// Token: 0x040000BE RID: 190
			public int state;

			// Token: 0x040000BF RID: 191
			public Vector3 coords;
		}

		// Token: 0x02000004 RID: 4
		private class SnapCell
		{
			// Token: 0x040000C0 RID: 192
			public string label;

			// Token: 0x040000C1 RID: 193
			public object def;
		}
	}
}

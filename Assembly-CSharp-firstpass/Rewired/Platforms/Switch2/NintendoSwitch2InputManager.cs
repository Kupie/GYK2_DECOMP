using System;
using System.Collections.Generic;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired.Platforms.Switch2
{
	// Token: 0x02000020 RID: 32
	[AddComponentMenu("Rewired/Nintendo Switch 2 Input Manager")]
	[RequireComponent(typeof(InputManager))]
	public sealed class NintendoSwitch2InputManager : MonoBehaviour, IExternalInputManager
	{
		// Token: 0x0600025F RID: 607 RVA: 0x000034C1 File Offset: 0x000016C1
		object IExternalInputManager.Initialize(Platform platform, object configVars)
		{
			return null;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00003466 File Offset: 0x00001666
		void IExternalInputManager.Deinitialize()
		{
		}

		// Token: 0x040001DC RID: 476
		[SerializeField]
		private NintendoSwitch2InputManager.UserData _userData = new NintendoSwitch2InputManager.UserData();

		// Token: 0x02000021 RID: 33
		[Serializable]
		private class UserData : IKeyedData<int>
		{
			// Token: 0x170001FD RID: 509
			// (get) Token: 0x06000262 RID: 610 RVA: 0x000034D7 File Offset: 0x000016D7
			// (set) Token: 0x06000263 RID: 611 RVA: 0x000034DF File Offset: 0x000016DF
			public int allowedNpadStyles
			{
				get
				{
					return this._allowedNpadStyles;
				}
				set
				{
					this._allowedNpadStyles = value;
				}
			}

			// Token: 0x170001FE RID: 510
			// (get) Token: 0x06000264 RID: 612 RVA: 0x000034E8 File Offset: 0x000016E8
			// (set) Token: 0x06000265 RID: 613 RVA: 0x000034F0 File Offset: 0x000016F0
			public int joyConGripStyle
			{
				get
				{
					return this._joyConGripStyle;
				}
				set
				{
					this._joyConGripStyle = value;
				}
			}

			// Token: 0x170001FF RID: 511
			// (get) Token: 0x06000266 RID: 614 RVA: 0x000034F9 File Offset: 0x000016F9
			// (set) Token: 0x06000267 RID: 615 RVA: 0x00003501 File Offset: 0x00001701
			public bool adjustIMUsForGripStyle
			{
				get
				{
					return this._adjustIMUsForGripStyle;
				}
				set
				{
					this._adjustIMUsForGripStyle = value;
				}
			}

			// Token: 0x17000200 RID: 512
			// (get) Token: 0x06000268 RID: 616 RVA: 0x0000350A File Offset: 0x0000170A
			// (set) Token: 0x06000269 RID: 617 RVA: 0x00003512 File Offset: 0x00001712
			public int handheldActivationMode
			{
				get
				{
					return this._handheldActivationMode;
				}
				set
				{
					this._handheldActivationMode = value;
				}
			}

			// Token: 0x17000201 RID: 513
			// (get) Token: 0x0600026A RID: 618 RVA: 0x0000351B File Offset: 0x0000171B
			// (set) Token: 0x0600026B RID: 619 RVA: 0x00003523 File Offset: 0x00001723
			public bool assignJoysticksByNpadId
			{
				get
				{
					return this._assignJoysticksByNpadId;
				}
				set
				{
					this._assignJoysticksByNpadId = value;
				}
			}

			// Token: 0x17000202 RID: 514
			// (get) Token: 0x0600026C RID: 620 RVA: 0x0000352C File Offset: 0x0000172C
			// (set) Token: 0x0600026D RID: 621 RVA: 0x00003534 File Offset: 0x00001734
			public bool useVibrationThread
			{
				get
				{
					return this._useVibrationThread;
				}
				set
				{
					this._useVibrationThread = value;
				}
			}

			// Token: 0x17000203 RID: 515
			// (get) Token: 0x0600026E RID: 622 RVA: 0x0000353D File Offset: 0x0000173D
			// (set) Token: 0x0600026F RID: 623 RVA: 0x00003545 File Offset: 0x00001745
			public bool autoStartIMUs
			{
				get
				{
					return this._autoStartIMUs;
				}
				set
				{
					this._autoStartIMUs = value;
				}
			}

			// Token: 0x17000204 RID: 516
			// (get) Token: 0x06000270 RID: 624 RVA: 0x0000354E File Offset: 0x0000174E
			// (set) Token: 0x06000271 RID: 625 RVA: 0x00003556 File Offset: 0x00001756
			public bool autoStartJoyConMouseSensors
			{
				get
				{
					return this._autoStartJoyConMouseSensors;
				}
				set
				{
					this._autoStartJoyConMouseSensors = value;
				}
			}

			// Token: 0x17000205 RID: 517
			// (get) Token: 0x06000272 RID: 626 RVA: 0x0000355F File Offset: 0x0000175F
			// (set) Token: 0x06000273 RID: 627 RVA: 0x00003567 File Offset: 0x00001767
			public bool allowJoyConMouseRebindPolling
			{
				get
				{
					return this._allowJoyConMouseRebindPolling;
				}
				set
				{
					this._allowJoyConMouseRebindPolling = value;
				}
			}

			// Token: 0x17000206 RID: 518
			// (get) Token: 0x06000274 RID: 628 RVA: 0x00003570 File Offset: 0x00001770
			// (set) Token: 0x06000275 RID: 629 RVA: 0x00003578 File Offset: 0x00001778
			public bool supportJoyConMouseSensors
			{
				get
				{
					return this._supportJoyConMouseSensors;
				}
				set
				{
					this._supportJoyConMouseSensors = value;
				}
			}

			// Token: 0x17000207 RID: 519
			// (get) Token: 0x06000276 RID: 630 RVA: 0x00003581 File Offset: 0x00001781
			// (set) Token: 0x06000277 RID: 631 RVA: 0x00003589 File Offset: 0x00001789
			public int streamPlayAllowedGuestNpadStyles
			{
				get
				{
					return this._streamPlayAllowedGuestNpadStyles;
				}
				set
				{
					this._streamPlayAllowedGuestNpadStyles = value;
				}
			}

			// Token: 0x17000208 RID: 520
			// (get) Token: 0x06000278 RID: 632 RVA: 0x00003592 File Offset: 0x00001792
			// (set) Token: 0x06000279 RID: 633 RVA: 0x0000359A File Offset: 0x0000179A
			public int streamPlayGuestJoyConGripStyle
			{
				get
				{
					return this._streamPlayGuestJoyConGripStyle;
				}
				set
				{
					this._streamPlayGuestJoyConGripStyle = value;
				}
			}

			// Token: 0x17000209 RID: 521
			// (get) Token: 0x0600027A RID: 634 RVA: 0x000035A3 File Offset: 0x000017A3
			// (set) Token: 0x0600027B RID: 635 RVA: 0x000035AB File Offset: 0x000017AB
			public int streamPlayGuestSupportedHidFeatures
			{
				get
				{
					return this._streamPlayGuestSupportedHidFeatures;
				}
				set
				{
					this._streamPlayGuestSupportedHidFeatures = value;
				}
			}

			// Token: 0x1700020A RID: 522
			// (get) Token: 0x0600027C RID: 636 RVA: 0x000035B4 File Offset: 0x000017B4
			private NintendoSwitch2InputManager.NpadSettings_Internal npadNo1
			{
				get
				{
					return this._npadNo1;
				}
			}

			// Token: 0x1700020B RID: 523
			// (get) Token: 0x0600027D RID: 637 RVA: 0x000035BC File Offset: 0x000017BC
			private NintendoSwitch2InputManager.NpadSettings_Internal npadNo2
			{
				get
				{
					return this._npadNo2;
				}
			}

			// Token: 0x1700020C RID: 524
			// (get) Token: 0x0600027E RID: 638 RVA: 0x000035C4 File Offset: 0x000017C4
			private NintendoSwitch2InputManager.NpadSettings_Internal npadNo3
			{
				get
				{
					return this._npadNo3;
				}
			}

			// Token: 0x1700020D RID: 525
			// (get) Token: 0x0600027F RID: 639 RVA: 0x000035CC File Offset: 0x000017CC
			private NintendoSwitch2InputManager.NpadSettings_Internal npadNo4
			{
				get
				{
					return this._npadNo4;
				}
			}

			// Token: 0x1700020E RID: 526
			// (get) Token: 0x06000280 RID: 640 RVA: 0x000035D4 File Offset: 0x000017D4
			private NintendoSwitch2InputManager.NpadSettings_Internal npadNo5
			{
				get
				{
					return this._npadNo5;
				}
			}

			// Token: 0x1700020F RID: 527
			// (get) Token: 0x06000281 RID: 641 RVA: 0x000035DC File Offset: 0x000017DC
			private NintendoSwitch2InputManager.NpadSettings_Internal npadNo6
			{
				get
				{
					return this._npadNo6;
				}
			}

			// Token: 0x17000210 RID: 528
			// (get) Token: 0x06000282 RID: 642 RVA: 0x000035E4 File Offset: 0x000017E4
			private NintendoSwitch2InputManager.NpadSettings_Internal npadNo7
			{
				get
				{
					return this._npadNo7;
				}
			}

			// Token: 0x17000211 RID: 529
			// (get) Token: 0x06000283 RID: 643 RVA: 0x000035EC File Offset: 0x000017EC
			private NintendoSwitch2InputManager.NpadSettings_Internal npadNo8
			{
				get
				{
					return this._npadNo8;
				}
			}

			// Token: 0x17000212 RID: 530
			// (get) Token: 0x06000284 RID: 644 RVA: 0x000035F4 File Offset: 0x000017F4
			private NintendoSwitch2InputManager.NpadSettings_Internal npadHandheld
			{
				get
				{
					return this._npadHandheld;
				}
			}

			// Token: 0x17000213 RID: 531
			// (get) Token: 0x06000285 RID: 645 RVA: 0x000035FC File Offset: 0x000017FC
			public NintendoSwitch2InputManager.DebugPadSettings_Internal debugPad
			{
				get
				{
					return this._debugPad;
				}
			}

			// Token: 0x17000214 RID: 532
			// (get) Token: 0x06000286 RID: 646 RVA: 0x00003604 File Offset: 0x00001804
			private NintendoSwitch2InputManager.StreamPlayGuestSettings_Internal streamPlayGuest1
			{
				get
				{
					return this._streamPlayGuest1;
				}
			}

			// Token: 0x17000215 RID: 533
			// (get) Token: 0x06000287 RID: 647 RVA: 0x0000360C File Offset: 0x0000180C
			private NintendoSwitch2InputManager.StreamPlayGuestSettings_Internal streamPlayGuest2
			{
				get
				{
					return this._streamPlayGuest2;
				}
			}

			// Token: 0x17000216 RID: 534
			// (get) Token: 0x06000288 RID: 648 RVA: 0x00003614 File Offset: 0x00001814
			private NintendoSwitch2InputManager.StreamPlayGuestSettings_Internal streamPlayGuest3
			{
				get
				{
					return this._streamPlayGuest3;
				}
			}

			// Token: 0x17000217 RID: 535
			// (get) Token: 0x06000289 RID: 649 RVA: 0x0000361C File Offset: 0x0000181C
			private Dictionary<int, object[]> delegates
			{
				get
				{
					if (this.__delegates != null)
					{
						return this.__delegates;
					}
					Dictionary<int, object[]> dictionary = new Dictionary<int, object[]>();
					dictionary.Add(0, new object[]
					{
						new Func<int>(() => this.allowedNpadStyles),
						new Action<int>(delegate(int x)
						{
							this.allowedNpadStyles = x;
						})
					});
					dictionary.Add(1, new object[]
					{
						new Func<int>(() => this.joyConGripStyle),
						new Action<int>(delegate(int x)
						{
							this.joyConGripStyle = x;
						})
					});
					dictionary.Add(2, new object[]
					{
						new Func<bool>(() => this.adjustIMUsForGripStyle),
						new Action<bool>(delegate(bool x)
						{
							this.adjustIMUsForGripStyle = x;
						})
					});
					dictionary.Add(3, new object[]
					{
						new Func<int>(() => this.handheldActivationMode),
						new Action<int>(delegate(int x)
						{
							this.handheldActivationMode = x;
						})
					});
					dictionary.Add(4, new object[]
					{
						new Func<bool>(() => this.assignJoysticksByNpadId),
						new Action<bool>(delegate(bool x)
						{
							this.assignJoysticksByNpadId = x;
						})
					});
					Dictionary<int, object[]> dictionary2 = dictionary;
					int num = 5;
					object[] array = new object[2];
					array[0] = new Func<object>(() => this.npadNo1);
					dictionary2.Add(num, array);
					Dictionary<int, object[]> dictionary3 = dictionary;
					int num2 = 6;
					object[] array2 = new object[2];
					array2[0] = new Func<object>(() => this.npadNo2);
					dictionary3.Add(num2, array2);
					Dictionary<int, object[]> dictionary4 = dictionary;
					int num3 = 7;
					object[] array3 = new object[2];
					array3[0] = new Func<object>(() => this.npadNo3);
					dictionary4.Add(num3, array3);
					Dictionary<int, object[]> dictionary5 = dictionary;
					int num4 = 8;
					object[] array4 = new object[2];
					array4[0] = new Func<object>(() => this.npadNo4);
					dictionary5.Add(num4, array4);
					Dictionary<int, object[]> dictionary6 = dictionary;
					int num5 = 9;
					object[] array5 = new object[2];
					array5[0] = new Func<object>(() => this.npadNo5);
					dictionary6.Add(num5, array5);
					Dictionary<int, object[]> dictionary7 = dictionary;
					int num6 = 10;
					object[] array6 = new object[2];
					array6[0] = new Func<object>(() => this.npadNo6);
					dictionary7.Add(num6, array6);
					Dictionary<int, object[]> dictionary8 = dictionary;
					int num7 = 11;
					object[] array7 = new object[2];
					array7[0] = new Func<object>(() => this.npadNo7);
					dictionary8.Add(num7, array7);
					Dictionary<int, object[]> dictionary9 = dictionary;
					int num8 = 12;
					object[] array8 = new object[2];
					array8[0] = new Func<object>(() => this.npadNo8);
					dictionary9.Add(num8, array8);
					Dictionary<int, object[]> dictionary10 = dictionary;
					int num9 = 13;
					object[] array9 = new object[2];
					array9[0] = new Func<object>(() => this.npadHandheld);
					dictionary10.Add(num9, array9);
					Dictionary<int, object[]> dictionary11 = dictionary;
					int num10 = 14;
					object[] array10 = new object[2];
					array10[0] = new Func<object>(() => this.debugPad);
					dictionary11.Add(num10, array10);
					dictionary.Add(15, new object[]
					{
						new Func<bool>(() => this.useVibrationThread),
						new Action<bool>(delegate(bool x)
						{
							this.useVibrationThread = x;
						})
					});
					dictionary.Add(16, new object[]
					{
						new Func<bool>(() => this.autoStartIMUs),
						new Action<bool>(delegate(bool x)
						{
							this.autoStartIMUs = x;
						})
					});
					dictionary.Add(17, new object[]
					{
						new Func<bool>(() => this.autoStartJoyConMouseSensors),
						new Action<bool>(delegate(bool x)
						{
							this.autoStartJoyConMouseSensors = x;
						})
					});
					dictionary.Add(18, new object[]
					{
						new Func<bool>(() => this.allowJoyConMouseRebindPolling),
						new Action<bool>(delegate(bool x)
						{
							this.allowJoyConMouseRebindPolling = x;
						})
					});
					dictionary.Add(19, new object[]
					{
						new Func<bool>(() => this._initializeJcms),
						new Action<bool>(delegate(bool x)
						{
							this._initializeJcms = x;
						})
					});
					dictionary.Add(20, new object[]
					{
						new Func<bool>(() => this._supportJoyConMouseSensors),
						new Action<bool>(delegate(bool x)
						{
							this._supportJoyConMouseSensors = x;
						})
					});
					dictionary.Add(21, new object[]
					{
						new Func<int>(() => this._streamPlayAllowedGuestNpadStyles),
						new Action<int>(delegate(int x)
						{
							this._streamPlayAllowedGuestNpadStyles = x;
						})
					});
					dictionary.Add(22, new object[]
					{
						new Func<int>(() => this._streamPlayGuestJoyConGripStyle),
						new Action<int>(delegate(int x)
						{
							this._streamPlayGuestJoyConGripStyle = x;
						})
					});
					dictionary.Add(23, new object[]
					{
						new Func<int>(() => this._streamPlayGuestSupportedHidFeatures),
						new Action<int>(delegate(int x)
						{
							this._streamPlayGuestSupportedHidFeatures = x;
						})
					});
					Dictionary<int, object[]> dictionary12 = dictionary;
					int num11 = 100;
					object[] array11 = new object[2];
					array11[0] = new Func<object>(() => this.streamPlayGuest1);
					dictionary12.Add(num11, array11);
					Dictionary<int, object[]> dictionary13 = dictionary;
					int num12 = 101;
					object[] array12 = new object[2];
					array12[0] = new Func<object>(() => this.streamPlayGuest2);
					dictionary13.Add(num12, array12);
					Dictionary<int, object[]> dictionary14 = dictionary;
					int num13 = 102;
					object[] array13 = new object[2];
					array13[0] = new Func<object>(() => this.streamPlayGuest3);
					dictionary14.Add(num13, array13);
					return this.__delegates = dictionary;
				}
			}

			// Token: 0x0600028A RID: 650 RVA: 0x00003A20 File Offset: 0x00001C20
			bool IKeyedData<int>.TryGetValue<T>(int key, out T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					value = default(T);
					return false;
				}
				Func<T> func = array[0] as Func<T>;
				if (func == null)
				{
					value = default(T);
					return false;
				}
				value = func();
				return true;
			}

			// Token: 0x0600028B RID: 651 RVA: 0x00003A68 File Offset: 0x00001C68
			bool IKeyedData<int>.TrySetValue<T>(int key, T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					return false;
				}
				Action<T> action = array[1] as Action<T>;
				if (action == null)
				{
					return false;
				}
				action(value);
				return true;
			}

			// Token: 0x040001DD RID: 477
			private const int defaultAllowedNpadStyles = 31;

			// Token: 0x040001DE RID: 478
			[SerializeField]
			private int _allowedNpadStyles = 31;

			// Token: 0x040001DF RID: 479
			[SerializeField]
			private int _joyConGripStyle = 1;

			// Token: 0x040001E0 RID: 480
			[SerializeField]
			private bool _adjustIMUsForGripStyle = true;

			// Token: 0x040001E1 RID: 481
			[SerializeField]
			private int _handheldActivationMode;

			// Token: 0x040001E2 RID: 482
			[SerializeField]
			private bool _assignJoysticksByNpadId = true;

			// Token: 0x040001E3 RID: 483
			[SerializeField]
			private bool _useVibrationThread = true;

			// Token: 0x040001E4 RID: 484
			[SerializeField]
			private bool _autoStartIMUs = true;

			// Token: 0x040001E5 RID: 485
			[SerializeField]
			private bool _autoStartJoyConMouseSensors;

			// Token: 0x040001E6 RID: 486
			[SerializeField]
			private bool _allowJoyConMouseRebindPolling;

			// Token: 0x040001E7 RID: 487
			[SerializeField]
			private bool _initializeJcms;

			// Token: 0x040001E8 RID: 488
			[SerializeField]
			private bool _supportJoyConMouseSensors;

			// Token: 0x040001E9 RID: 489
			[SerializeField]
			private int _streamPlayAllowedGuestNpadStyles = 31;

			// Token: 0x040001EA RID: 490
			[SerializeField]
			private int _streamPlayGuestJoyConGripStyle = 1;

			// Token: 0x040001EB RID: 491
			[SerializeField]
			private int _streamPlayGuestSupportedHidFeatures = 4;

			// Token: 0x040001EC RID: 492
			[SerializeField]
			private NintendoSwitch2InputManager.NpadSettings_Internal _npadNo1 = new NintendoSwitch2InputManager.NpadSettings_Internal(0);

			// Token: 0x040001ED RID: 493
			[SerializeField]
			private NintendoSwitch2InputManager.NpadSettings_Internal _npadNo2 = new NintendoSwitch2InputManager.NpadSettings_Internal(1);

			// Token: 0x040001EE RID: 494
			[SerializeField]
			private NintendoSwitch2InputManager.NpadSettings_Internal _npadNo3 = new NintendoSwitch2InputManager.NpadSettings_Internal(2);

			// Token: 0x040001EF RID: 495
			[SerializeField]
			private NintendoSwitch2InputManager.NpadSettings_Internal _npadNo4 = new NintendoSwitch2InputManager.NpadSettings_Internal(3);

			// Token: 0x040001F0 RID: 496
			[SerializeField]
			private NintendoSwitch2InputManager.NpadSettings_Internal _npadNo5 = new NintendoSwitch2InputManager.NpadSettings_Internal(4);

			// Token: 0x040001F1 RID: 497
			[SerializeField]
			private NintendoSwitch2InputManager.NpadSettings_Internal _npadNo6 = new NintendoSwitch2InputManager.NpadSettings_Internal(5);

			// Token: 0x040001F2 RID: 498
			[SerializeField]
			private NintendoSwitch2InputManager.NpadSettings_Internal _npadNo7 = new NintendoSwitch2InputManager.NpadSettings_Internal(6);

			// Token: 0x040001F3 RID: 499
			[SerializeField]
			private NintendoSwitch2InputManager.NpadSettings_Internal _npadNo8 = new NintendoSwitch2InputManager.NpadSettings_Internal(7);

			// Token: 0x040001F4 RID: 500
			[SerializeField]
			private NintendoSwitch2InputManager.NpadSettings_Internal _npadHandheld = new NintendoSwitch2InputManager.NpadSettings_Internal(0);

			// Token: 0x040001F5 RID: 501
			[SerializeField]
			private NintendoSwitch2InputManager.DebugPadSettings_Internal _debugPad = new NintendoSwitch2InputManager.DebugPadSettings_Internal(0);

			// Token: 0x040001F6 RID: 502
			[SerializeField]
			private NintendoSwitch2InputManager.StreamPlayGuestSettings_Internal _streamPlayGuest1 = new NintendoSwitch2InputManager.StreamPlayGuestSettings_Internal(0, 1);

			// Token: 0x040001F7 RID: 503
			[SerializeField]
			private NintendoSwitch2InputManager.StreamPlayGuestSettings_Internal _streamPlayGuest2 = new NintendoSwitch2InputManager.StreamPlayGuestSettings_Internal(1, 2);

			// Token: 0x040001F8 RID: 504
			[SerializeField]
			private NintendoSwitch2InputManager.StreamPlayGuestSettings_Internal _streamPlayGuest3 = new NintendoSwitch2InputManager.StreamPlayGuestSettings_Internal(2, 3);

			// Token: 0x040001F9 RID: 505
			private Dictionary<int, object[]> __delegates;
		}

		// Token: 0x02000022 RID: 34
		[Serializable]
		private sealed class NpadSettings_Internal : IKeyedData<int>
		{
			// Token: 0x17000218 RID: 536
			// (get) Token: 0x060002B6 RID: 694 RVA: 0x00003CA5 File Offset: 0x00001EA5
			// (set) Token: 0x060002B7 RID: 695 RVA: 0x00003CAD File Offset: 0x00001EAD
			private bool isAllowed
			{
				get
				{
					return this._isAllowed;
				}
				set
				{
					this._isAllowed = value;
				}
			}

			// Token: 0x17000219 RID: 537
			// (get) Token: 0x060002B8 RID: 696 RVA: 0x00003CB6 File Offset: 0x00001EB6
			// (set) Token: 0x060002B9 RID: 697 RVA: 0x00003CBE File Offset: 0x00001EBE
			private int rewiredPlayerId
			{
				get
				{
					return this._rewiredPlayerId;
				}
				set
				{
					this._rewiredPlayerId = value;
				}
			}

			// Token: 0x1700021A RID: 538
			// (get) Token: 0x060002BA RID: 698 RVA: 0x00003CC7 File Offset: 0x00001EC7
			// (set) Token: 0x060002BB RID: 699 RVA: 0x00003CCF File Offset: 0x00001ECF
			private int joyConAssignmentMode
			{
				get
				{
					return this._joyConAssignmentMode;
				}
				set
				{
					this._joyConAssignmentMode = value;
				}
			}

			// Token: 0x060002BC RID: 700 RVA: 0x00003CD8 File Offset: 0x00001ED8
			internal NpadSettings_Internal(int playerId)
			{
				this._rewiredPlayerId = playerId;
			}

			// Token: 0x1700021B RID: 539
			// (get) Token: 0x060002BD RID: 701 RVA: 0x00003CF8 File Offset: 0x00001EF8
			private Dictionary<int, object[]> delegates
			{
				get
				{
					if (this.__delegates != null)
					{
						return this.__delegates;
					}
					return this.__delegates = new Dictionary<int, object[]>
					{
						{
							0,
							new object[]
							{
								new Func<bool>(() => this.isAllowed),
								new Action<bool>(delegate(bool x)
								{
									this.isAllowed = x;
								})
							}
						},
						{
							1,
							new object[]
							{
								new Func<int>(() => this.rewiredPlayerId),
								new Action<int>(delegate(int x)
								{
									this.rewiredPlayerId = x;
								})
							}
						},
						{
							2,
							new object[]
							{
								new Func<int>(() => this.joyConAssignmentMode),
								new Action<int>(delegate(int x)
								{
									this.joyConAssignmentMode = x;
								})
							}
						}
					};
				}
			}

			// Token: 0x060002BE RID: 702 RVA: 0x00003DA8 File Offset: 0x00001FA8
			bool IKeyedData<int>.TryGetValue<T>(int key, out T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					value = default(T);
					return false;
				}
				Func<T> func = array[0] as Func<T>;
				if (func == null)
				{
					value = default(T);
					return false;
				}
				value = func();
				return true;
			}

			// Token: 0x060002BF RID: 703 RVA: 0x00003DF0 File Offset: 0x00001FF0
			bool IKeyedData<int>.TrySetValue<T>(int key, T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					return false;
				}
				Action<T> action = array[1] as Action<T>;
				if (action == null)
				{
					return false;
				}
				action(value);
				return true;
			}

			// Token: 0x040001FA RID: 506
			[Tooltip("Determines whether this Npad id is allowed to be used by the system.")]
			[SerializeField]
			private bool _isAllowed = true;

			// Token: 0x040001FB RID: 507
			[Tooltip("The Rewired Player Id assigned to this Npad id.")]
			[SerializeField]
			private int _rewiredPlayerId;

			// Token: 0x040001FC RID: 508
			[Tooltip("Determines how Joy-Cons should be handled.\n\nUnmodified: Joy-Con assignment mode will be left at the system default.\nDual: Joy-Cons pairs are handled as a single controller.\nSingle: Joy-Cons are handled as individual controllers.")]
			[SerializeField]
			private int _joyConAssignmentMode = -1;

			// Token: 0x040001FD RID: 509
			private Dictionary<int, object[]> __delegates;
		}

		// Token: 0x02000023 RID: 35
		[Serializable]
		private sealed class StreamPlayGuestSettings_Internal : IKeyedData<int>
		{
			// Token: 0x1700021C RID: 540
			// (get) Token: 0x060002C6 RID: 710 RVA: 0x00003E58 File Offset: 0x00002058
			// (set) Token: 0x060002C7 RID: 711 RVA: 0x00003E60 File Offset: 0x00002060
			private int rewiredPlayerId
			{
				get
				{
					return this._rewiredPlayerId;
				}
				set
				{
					this._rewiredPlayerId = value;
				}
			}

			// Token: 0x060002C8 RID: 712 RVA: 0x00003E6C File Offset: 0x0000206C
			internal StreamPlayGuestSettings_Internal(int guestId, int playerId)
			{
				this._guestId = guestId;
				this._rewiredPlayerId = playerId;
			}

			// Token: 0x1700021D RID: 541
			// (get) Token: 0x060002C9 RID: 713 RVA: 0x00003F04 File Offset: 0x00002104
			private Dictionary<int, object[]> delegates
			{
				get
				{
					if (this.__delegates != null)
					{
						return this.__delegates;
					}
					Dictionary<int, object[]> dictionary = new Dictionary<int, object[]>();
					Dictionary<int, object[]> dictionary2 = dictionary;
					int num = 0;
					object[] array = new object[2];
					array[0] = new Func<int>(() => this._guestId);
					dictionary2.Add(num, array);
					dictionary.Add(1, new object[]
					{
						new Func<int>(() => this.rewiredPlayerId),
						new Action<int>(delegate(int x)
						{
							this.rewiredPlayerId = x;
						})
					});
					Dictionary<int, object[]> dictionary3 = dictionary;
					int num2 = 2;
					object[] array2 = new object[2];
					array2[0] = new Func<IKeyedData<int>>(() => this._npadNo1);
					dictionary3.Add(num2, array2);
					Dictionary<int, object[]> dictionary4 = dictionary;
					int num3 = 3;
					object[] array3 = new object[2];
					array3[0] = new Func<IKeyedData<int>>(() => this._npadNo2);
					dictionary4.Add(num3, array3);
					Dictionary<int, object[]> dictionary5 = dictionary;
					int num4 = 4;
					object[] array4 = new object[2];
					array4[0] = new Func<IKeyedData<int>>(() => this._npadNo3);
					dictionary5.Add(num4, array4);
					Dictionary<int, object[]> dictionary6 = dictionary;
					int num5 = 5;
					object[] array5 = new object[2];
					array5[0] = new Func<IKeyedData<int>>(() => this._npadNo4);
					dictionary6.Add(num5, array5);
					Dictionary<int, object[]> dictionary7 = dictionary;
					int num6 = 6;
					object[] array6 = new object[2];
					array6[0] = new Func<IKeyedData<int>>(() => this._npadNo5);
					dictionary7.Add(num6, array6);
					Dictionary<int, object[]> dictionary8 = dictionary;
					int num7 = 7;
					object[] array7 = new object[2];
					array7[0] = new Func<IKeyedData<int>>(() => this._npadNo6);
					dictionary8.Add(num7, array7);
					Dictionary<int, object[]> dictionary9 = dictionary;
					int num8 = 8;
					object[] array8 = new object[2];
					array8[0] = new Func<IKeyedData<int>>(() => this._npadNo7);
					dictionary9.Add(num8, array8);
					Dictionary<int, object[]> dictionary10 = dictionary;
					int num9 = 9;
					object[] array9 = new object[2];
					array9[0] = new Func<IKeyedData<int>>(() => this._npadNo8);
					dictionary10.Add(num9, array9);
					Dictionary<int, object[]> dictionary11 = dictionary;
					int num10 = 10;
					object[] array10 = new object[2];
					array10[0] = new Func<IKeyedData<int>>(() => this._npadHandheld);
					dictionary11.Add(num10, array10);
					return this.__delegates = dictionary;
				}
			}

			// Token: 0x060002CA RID: 714 RVA: 0x00004078 File Offset: 0x00002278
			bool IKeyedData<int>.TryGetValue<T>(int key, out T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					value = default(T);
					return false;
				}
				Func<T> func = array[0] as Func<T>;
				if (func == null)
				{
					value = default(T);
					return false;
				}
				value = func();
				return true;
			}

			// Token: 0x060002CB RID: 715 RVA: 0x000040C0 File Offset: 0x000022C0
			bool IKeyedData<int>.TrySetValue<T>(int key, T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					return false;
				}
				Action<T> action = array[1] as Action<T>;
				if (action == null)
				{
					return false;
				}
				action(value);
				return true;
			}

			// Token: 0x040001FE RID: 510
			[NonSerialized]
			private readonly int _guestId;

			// Token: 0x040001FF RID: 511
			[Tooltip("The Player Id assigned to this Guest. If set to a valid Player Id, all Npads on the Guest will be assigned to this Player. Any NpadId not overridden will be assigned the Guest Rewired Player Id.Set to -1 for no Player.")]
			[SerializeField]
			private int _rewiredPlayerId = -1;

			// Token: 0x04000200 RID: 512
			[Tooltip("Npad 1 settings.")]
			[SerializeField]
			private NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal _npadNo1 = new NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal(0);

			// Token: 0x04000201 RID: 513
			[Tooltip("Npad 2 settings.")]
			[SerializeField]
			private NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal _npadNo2 = new NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal(1);

			// Token: 0x04000202 RID: 514
			[Tooltip("Npad 3 settings.")]
			[SerializeField]
			private NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal _npadNo3 = new NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal(2);

			// Token: 0x04000203 RID: 515
			[Tooltip("Npad 4 settings.")]
			[SerializeField]
			private NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal _npadNo4 = new NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal(3);

			// Token: 0x04000204 RID: 516
			[Tooltip("Npad 5 settings.")]
			[SerializeField]
			private NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal _npadNo5 = new NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal(4);

			// Token: 0x04000205 RID: 517
			[Tooltip("Npad 6 settings.")]
			[SerializeField]
			private NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal _npadNo6 = new NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal(5);

			// Token: 0x04000206 RID: 518
			[Tooltip("Npad 7 settings.")]
			[SerializeField]
			private NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal _npadNo7 = new NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal(6);

			// Token: 0x04000207 RID: 519
			[Tooltip("Npad 8 settings.")]
			[SerializeField]
			private NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal _npadNo8 = new NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal(7);

			// Token: 0x04000208 RID: 520
			[Tooltip("Handheld Npad settings.")]
			[SerializeField]
			private NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal _npadHandheld = new NintendoSwitch2InputManager.StreamPlayGuestPadSettings_Internal(32);

			// Token: 0x04000209 RID: 521
			private Dictionary<int, object[]> __delegates;
		}

		// Token: 0x02000024 RID: 36
		[Serializable]
		private sealed class StreamPlayGuestPadSettings_Internal : IKeyedData<int>
		{
			// Token: 0x1700021E RID: 542
			// (get) Token: 0x060002D8 RID: 728 RVA: 0x00004156 File Offset: 0x00002356
			// (set) Token: 0x060002D9 RID: 729 RVA: 0x0000415E File Offset: 0x0000235E
			private bool overrideParent
			{
				get
				{
					return this._overrideParent;
				}
				set
				{
					this._overrideParent = value;
				}
			}

			// Token: 0x1700021F RID: 543
			// (get) Token: 0x060002DA RID: 730 RVA: 0x00004167 File Offset: 0x00002367
			// (set) Token: 0x060002DB RID: 731 RVA: 0x0000416F File Offset: 0x0000236F
			private int rewiredPlayerId
			{
				get
				{
					return this._rewiredPlayerId;
				}
				set
				{
					this._rewiredPlayerId = value;
				}
			}

			// Token: 0x060002DC RID: 732 RVA: 0x00004178 File Offset: 0x00002378
			internal StreamPlayGuestPadSettings_Internal(int npadId)
			{
				this._npadId = npadId;
			}

			// Token: 0x17000220 RID: 544
			// (get) Token: 0x060002DD RID: 733 RVA: 0x00004190 File Offset: 0x00002390
			private Dictionary<int, object[]> delegates
			{
				get
				{
					if (this.__delegates != null)
					{
						return this.__delegates;
					}
					Dictionary<int, object[]> dictionary = new Dictionary<int, object[]>();
					Dictionary<int, object[]> dictionary2 = dictionary;
					int num = 0;
					object[] array = new object[2];
					array[0] = new Func<int>(() => this._npadId);
					dictionary2.Add(num, array);
					dictionary.Add(1, new object[]
					{
						new Func<int>(() => this.rewiredPlayerId),
						new Action<int>(delegate(int x)
						{
							this.rewiredPlayerId = x;
						})
					});
					dictionary.Add(2, new object[]
					{
						new Func<bool>(() => this.overrideParent),
						new Action<bool>(delegate(bool x)
						{
							this.overrideParent = x;
						})
					});
					return this.__delegates = dictionary;
				}
			}

			// Token: 0x060002DE RID: 734 RVA: 0x00004230 File Offset: 0x00002430
			bool IKeyedData<int>.TryGetValue<T>(int key, out T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					value = default(T);
					return false;
				}
				Func<T> func = array[0] as Func<T>;
				if (func == null)
				{
					value = default(T);
					return false;
				}
				value = func();
				return true;
			}

			// Token: 0x060002DF RID: 735 RVA: 0x00004278 File Offset: 0x00002478
			bool IKeyedData<int>.TrySetValue<T>(int key, T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					return false;
				}
				Action<T> action = array[1] as Action<T>;
				if (action == null)
				{
					return false;
				}
				action(value);
				return true;
			}

			// Token: 0x0400020A RID: 522
			[NonSerialized]
			private readonly int _npadId;

			// Token: 0x0400020B RID: 523
			[Tooltip("If enabled, default values in the parent Guest will be overridden with local values.")]
			[SerializeField]
			private bool _overrideParent;

			// Token: 0x0400020C RID: 524
			[Tooltip("The Rewired Player Id assigned to this NpadId. Set to -1 for no Player.")]
			[SerializeField]
			private int _rewiredPlayerId = -1;

			// Token: 0x0400020D RID: 525
			private Dictionary<int, object[]> __delegates;
		}

		// Token: 0x02000025 RID: 37
		[Serializable]
		private sealed class DebugPadSettings_Internal : IKeyedData<int>
		{
			// Token: 0x17000221 RID: 545
			// (get) Token: 0x060002E5 RID: 741 RVA: 0x000042D7 File Offset: 0x000024D7
			// (set) Token: 0x060002E6 RID: 742 RVA: 0x000042DF File Offset: 0x000024DF
			private int rewiredPlayerId
			{
				get
				{
					return this._rewiredPlayerId;
				}
				set
				{
					this._rewiredPlayerId = value;
				}
			}

			// Token: 0x17000222 RID: 546
			// (get) Token: 0x060002E7 RID: 743 RVA: 0x000042E8 File Offset: 0x000024E8
			// (set) Token: 0x060002E8 RID: 744 RVA: 0x000042F0 File Offset: 0x000024F0
			private bool enabled
			{
				get
				{
					return this._enabled;
				}
				set
				{
					this._enabled = value;
				}
			}

			// Token: 0x060002E9 RID: 745 RVA: 0x000042F9 File Offset: 0x000024F9
			internal DebugPadSettings_Internal(int playerId)
			{
				this._rewiredPlayerId = playerId;
			}

			// Token: 0x17000223 RID: 547
			// (get) Token: 0x060002EA RID: 746 RVA: 0x00004308 File Offset: 0x00002508
			private Dictionary<int, object[]> delegates
			{
				get
				{
					if (this.__delegates != null)
					{
						return this.__delegates;
					}
					return this.__delegates = new Dictionary<int, object[]>
					{
						{
							0,
							new object[]
							{
								new Func<bool>(() => this.enabled),
								new Action<bool>(delegate(bool x)
								{
									this.enabled = x;
								})
							}
						},
						{
							1,
							new object[]
							{
								new Func<int>(() => this.rewiredPlayerId),
								new Action<int>(delegate(int x)
								{
									this.rewiredPlayerId = x;
								})
							}
						}
					};
				}
			}

			// Token: 0x060002EB RID: 747 RVA: 0x0000438C File Offset: 0x0000258C
			bool IKeyedData<int>.TryGetValue<T>(int key, out T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					value = default(T);
					return false;
				}
				Func<T> func = array[0] as Func<T>;
				if (func == null)
				{
					value = default(T);
					return false;
				}
				value = func();
				return true;
			}

			// Token: 0x060002EC RID: 748 RVA: 0x000043D4 File Offset: 0x000025D4
			bool IKeyedData<int>.TrySetValue<T>(int key, T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					return false;
				}
				Action<T> action = array[1] as Action<T>;
				if (action == null)
				{
					return false;
				}
				action(value);
				return true;
			}

			// Token: 0x0400020E RID: 526
			[Tooltip("Determines whether the Debug Pad will be enabled.")]
			[SerializeField]
			private bool _enabled;

			// Token: 0x0400020F RID: 527
			[Tooltip("The Rewired Player Id to which the Debug Pad will be assigned.")]
			[SerializeField]
			private int _rewiredPlayerId;

			// Token: 0x04000210 RID: 528
			private Dictionary<int, object[]> __delegates;
		}
	}
}

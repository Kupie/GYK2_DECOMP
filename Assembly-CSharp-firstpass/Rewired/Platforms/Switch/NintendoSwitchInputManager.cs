using System;
using System.Collections.Generic;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired.Platforms.Switch
{
	// Token: 0x02000026 RID: 38
	[AddComponentMenu("Rewired/Nintendo Switch Input Manager")]
	[RequireComponent(typeof(InputManager))]
	public sealed class NintendoSwitchInputManager : MonoBehaviour, IExternalInputManager
	{
		// Token: 0x060002F1 RID: 753 RVA: 0x000034C1 File Offset: 0x000016C1
		object IExternalInputManager.Initialize(Platform platform, object configVars)
		{
			return null;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00003466 File Offset: 0x00001666
		void IExternalInputManager.Deinitialize()
		{
		}

		// Token: 0x04000211 RID: 529
		[SerializeField]
		private NintendoSwitchInputManager.UserData _userData = new NintendoSwitchInputManager.UserData();

		// Token: 0x02000027 RID: 39
		[Serializable]
		private class UserData : IKeyedData<int>
		{
			// Token: 0x17000224 RID: 548
			// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000443E File Offset: 0x0000263E
			// (set) Token: 0x060002F5 RID: 757 RVA: 0x00004446 File Offset: 0x00002646
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

			// Token: 0x17000225 RID: 549
			// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000444F File Offset: 0x0000264F
			// (set) Token: 0x060002F7 RID: 759 RVA: 0x00004457 File Offset: 0x00002657
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

			// Token: 0x17000226 RID: 550
			// (get) Token: 0x060002F8 RID: 760 RVA: 0x00004460 File Offset: 0x00002660
			// (set) Token: 0x060002F9 RID: 761 RVA: 0x00004468 File Offset: 0x00002668
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

			// Token: 0x17000227 RID: 551
			// (get) Token: 0x060002FA RID: 762 RVA: 0x00004471 File Offset: 0x00002671
			// (set) Token: 0x060002FB RID: 763 RVA: 0x00004479 File Offset: 0x00002679
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

			// Token: 0x17000228 RID: 552
			// (get) Token: 0x060002FC RID: 764 RVA: 0x00004482 File Offset: 0x00002682
			// (set) Token: 0x060002FD RID: 765 RVA: 0x0000448A File Offset: 0x0000268A
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

			// Token: 0x17000229 RID: 553
			// (get) Token: 0x060002FE RID: 766 RVA: 0x00004493 File Offset: 0x00002693
			// (set) Token: 0x060002FF RID: 767 RVA: 0x0000449B File Offset: 0x0000269B
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

			// Token: 0x1700022A RID: 554
			// (get) Token: 0x06000300 RID: 768 RVA: 0x000044A4 File Offset: 0x000026A4
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo1
			{
				get
				{
					return this._npadNo1;
				}
			}

			// Token: 0x1700022B RID: 555
			// (get) Token: 0x06000301 RID: 769 RVA: 0x000044AC File Offset: 0x000026AC
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo2
			{
				get
				{
					return this._npadNo2;
				}
			}

			// Token: 0x1700022C RID: 556
			// (get) Token: 0x06000302 RID: 770 RVA: 0x000044B4 File Offset: 0x000026B4
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo3
			{
				get
				{
					return this._npadNo3;
				}
			}

			// Token: 0x1700022D RID: 557
			// (get) Token: 0x06000303 RID: 771 RVA: 0x000044BC File Offset: 0x000026BC
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo4
			{
				get
				{
					return this._npadNo4;
				}
			}

			// Token: 0x1700022E RID: 558
			// (get) Token: 0x06000304 RID: 772 RVA: 0x000044C4 File Offset: 0x000026C4
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo5
			{
				get
				{
					return this._npadNo5;
				}
			}

			// Token: 0x1700022F RID: 559
			// (get) Token: 0x06000305 RID: 773 RVA: 0x000044CC File Offset: 0x000026CC
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo6
			{
				get
				{
					return this._npadNo6;
				}
			}

			// Token: 0x17000230 RID: 560
			// (get) Token: 0x06000306 RID: 774 RVA: 0x000044D4 File Offset: 0x000026D4
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo7
			{
				get
				{
					return this._npadNo7;
				}
			}

			// Token: 0x17000231 RID: 561
			// (get) Token: 0x06000307 RID: 775 RVA: 0x000044DC File Offset: 0x000026DC
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo8
			{
				get
				{
					return this._npadNo8;
				}
			}

			// Token: 0x17000232 RID: 562
			// (get) Token: 0x06000308 RID: 776 RVA: 0x000044E4 File Offset: 0x000026E4
			private NintendoSwitchInputManager.NpadSettings_Internal npadHandheld
			{
				get
				{
					return this._npadHandheld;
				}
			}

			// Token: 0x17000233 RID: 563
			// (get) Token: 0x06000309 RID: 777 RVA: 0x000044EC File Offset: 0x000026EC
			public NintendoSwitchInputManager.DebugPadSettings_Internal debugPad
			{
				get
				{
					return this._debugPad;
				}
			}

			// Token: 0x17000234 RID: 564
			// (get) Token: 0x0600030A RID: 778 RVA: 0x000044F4 File Offset: 0x000026F4
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
					return this.__delegates = dictionary;
				}
			}

			// Token: 0x0600030B RID: 779 RVA: 0x00004744 File Offset: 0x00002944
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

			// Token: 0x0600030C RID: 780 RVA: 0x0000478C File Offset: 0x0000298C
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

			// Token: 0x04000212 RID: 530
			[SerializeField]
			private int _allowedNpadStyles = -1;

			// Token: 0x04000213 RID: 531
			[SerializeField]
			private int _joyConGripStyle = 1;

			// Token: 0x04000214 RID: 532
			[SerializeField]
			private bool _adjustIMUsForGripStyle = true;

			// Token: 0x04000215 RID: 533
			[SerializeField]
			private int _handheldActivationMode;

			// Token: 0x04000216 RID: 534
			[SerializeField]
			private bool _assignJoysticksByNpadId = true;

			// Token: 0x04000217 RID: 535
			[SerializeField]
			private bool _useVibrationThread = true;

			// Token: 0x04000218 RID: 536
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo1 = new NintendoSwitchInputManager.NpadSettings_Internal(0);

			// Token: 0x04000219 RID: 537
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo2 = new NintendoSwitchInputManager.NpadSettings_Internal(1);

			// Token: 0x0400021A RID: 538
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo3 = new NintendoSwitchInputManager.NpadSettings_Internal(2);

			// Token: 0x0400021B RID: 539
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo4 = new NintendoSwitchInputManager.NpadSettings_Internal(3);

			// Token: 0x0400021C RID: 540
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo5 = new NintendoSwitchInputManager.NpadSettings_Internal(4);

			// Token: 0x0400021D RID: 541
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo6 = new NintendoSwitchInputManager.NpadSettings_Internal(5);

			// Token: 0x0400021E RID: 542
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo7 = new NintendoSwitchInputManager.NpadSettings_Internal(6);

			// Token: 0x0400021F RID: 543
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo8 = new NintendoSwitchInputManager.NpadSettings_Internal(7);

			// Token: 0x04000220 RID: 544
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadHandheld = new NintendoSwitchInputManager.NpadSettings_Internal(0);

			// Token: 0x04000221 RID: 545
			[SerializeField]
			private NintendoSwitchInputManager.DebugPadSettings_Internal _debugPad = new NintendoSwitchInputManager.DebugPadSettings_Internal(0);

			// Token: 0x04000222 RID: 546
			private Dictionary<int, object[]> __delegates;
		}

		// Token: 0x02000028 RID: 40
		[Serializable]
		private sealed class NpadSettings_Internal : IKeyedData<int>
		{
			// Token: 0x17000235 RID: 565
			// (get) Token: 0x06000324 RID: 804 RVA: 0x00004928 File Offset: 0x00002B28
			// (set) Token: 0x06000325 RID: 805 RVA: 0x00004930 File Offset: 0x00002B30
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

			// Token: 0x17000236 RID: 566
			// (get) Token: 0x06000326 RID: 806 RVA: 0x00004939 File Offset: 0x00002B39
			// (set) Token: 0x06000327 RID: 807 RVA: 0x00004941 File Offset: 0x00002B41
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

			// Token: 0x17000237 RID: 567
			// (get) Token: 0x06000328 RID: 808 RVA: 0x0000494A File Offset: 0x00002B4A
			// (set) Token: 0x06000329 RID: 809 RVA: 0x00004952 File Offset: 0x00002B52
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

			// Token: 0x0600032A RID: 810 RVA: 0x0000495B File Offset: 0x00002B5B
			internal NpadSettings_Internal(int playerId)
			{
				this._rewiredPlayerId = playerId;
			}

			// Token: 0x17000238 RID: 568
			// (get) Token: 0x0600032B RID: 811 RVA: 0x00004978 File Offset: 0x00002B78
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

			// Token: 0x0600032C RID: 812 RVA: 0x00004A28 File Offset: 0x00002C28
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

			// Token: 0x0600032D RID: 813 RVA: 0x00004A70 File Offset: 0x00002C70
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

			// Token: 0x04000223 RID: 547
			[Tooltip("Determines whether this Npad id is allowed to be used by the system.")]
			[SerializeField]
			private bool _isAllowed = true;

			// Token: 0x04000224 RID: 548
			[Tooltip("The Rewired Player Id assigned to this Npad id.")]
			[SerializeField]
			private int _rewiredPlayerId;

			// Token: 0x04000225 RID: 549
			[Tooltip("Determines how Joy-Cons should be handled.\n\nUnmodified: Joy-Con assignment mode will be left at the system default.\nDual: Joy-Cons pairs are handled as a single controller.\nSingle: Joy-Cons are handled as individual controllers.")]
			[SerializeField]
			private int _joyConAssignmentMode = -1;

			// Token: 0x04000226 RID: 550
			private Dictionary<int, object[]> __delegates;
		}

		// Token: 0x02000029 RID: 41
		[Serializable]
		private sealed class DebugPadSettings_Internal : IKeyedData<int>
		{
			// Token: 0x17000239 RID: 569
			// (get) Token: 0x06000334 RID: 820 RVA: 0x00004AD8 File Offset: 0x00002CD8
			// (set) Token: 0x06000335 RID: 821 RVA: 0x00004AE0 File Offset: 0x00002CE0
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

			// Token: 0x1700023A RID: 570
			// (get) Token: 0x06000336 RID: 822 RVA: 0x00004AE9 File Offset: 0x00002CE9
			// (set) Token: 0x06000337 RID: 823 RVA: 0x00004AF1 File Offset: 0x00002CF1
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

			// Token: 0x06000338 RID: 824 RVA: 0x00004AFA File Offset: 0x00002CFA
			internal DebugPadSettings_Internal(int playerId)
			{
				this._rewiredPlayerId = playerId;
			}

			// Token: 0x1700023B RID: 571
			// (get) Token: 0x06000339 RID: 825 RVA: 0x00004B0C File Offset: 0x00002D0C
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

			// Token: 0x0600033A RID: 826 RVA: 0x00004B90 File Offset: 0x00002D90
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

			// Token: 0x0600033B RID: 827 RVA: 0x00004BD8 File Offset: 0x00002DD8
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

			// Token: 0x04000227 RID: 551
			[Tooltip("Determines whether the Debug Pad will be enabled.")]
			[SerializeField]
			private bool _enabled;

			// Token: 0x04000228 RID: 552
			[Tooltip("The Rewired Player Id to which the Debug Pad will be assigned.")]
			[SerializeField]
			private int _rewiredPlayerId;

			// Token: 0x04000229 RID: 553
			private Dictionary<int, object[]> __delegates;
		}
	}
}

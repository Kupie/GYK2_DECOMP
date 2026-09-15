using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired.Glyphs;
using Rewired.Glyphs.UnityUI;
using Rewired.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000095 RID: 149
	[AddComponentMenu("")]
	public class ControlMapper : MonoBehaviour
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000748 RID: 1864 RVA: 0x00012AC0 File Offset: 0x00010CC0
		// (remove) Token: 0x06000749 RID: 1865 RVA: 0x00012AD9 File Offset: 0x00010CD9
		public event Action ScreenClosedEvent
		{
			add
			{
				this._ScreenClosedEvent = (Action)Delegate.Combine(this._ScreenClosedEvent, value);
			}
			remove
			{
				this._ScreenClosedEvent = (Action)Delegate.Remove(this._ScreenClosedEvent, value);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600074A RID: 1866 RVA: 0x00012AF2 File Offset: 0x00010CF2
		// (remove) Token: 0x0600074B RID: 1867 RVA: 0x00012B0B File Offset: 0x00010D0B
		public event Action ScreenOpenedEvent
		{
			add
			{
				this._ScreenOpenedEvent = (Action)Delegate.Combine(this._ScreenOpenedEvent, value);
			}
			remove
			{
				this._ScreenOpenedEvent = (Action)Delegate.Remove(this._ScreenOpenedEvent, value);
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600074C RID: 1868 RVA: 0x00012B24 File Offset: 0x00010D24
		// (remove) Token: 0x0600074D RID: 1869 RVA: 0x00012B3D File Offset: 0x00010D3D
		public event Action PopupWindowClosedEvent
		{
			add
			{
				this._PopupWindowClosedEvent = (Action)Delegate.Combine(this._PopupWindowClosedEvent, value);
			}
			remove
			{
				this._PopupWindowClosedEvent = (Action)Delegate.Remove(this._PopupWindowClosedEvent, value);
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600074E RID: 1870 RVA: 0x00012B56 File Offset: 0x00010D56
		// (remove) Token: 0x0600074F RID: 1871 RVA: 0x00012B6F File Offset: 0x00010D6F
		public event Action PopupWindowOpenedEvent
		{
			add
			{
				this._PopupWindowOpenedEvent = (Action)Delegate.Combine(this._PopupWindowOpenedEvent, value);
			}
			remove
			{
				this._PopupWindowOpenedEvent = (Action)Delegate.Remove(this._PopupWindowOpenedEvent, value);
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000750 RID: 1872 RVA: 0x00012B88 File Offset: 0x00010D88
		// (remove) Token: 0x06000751 RID: 1873 RVA: 0x00012BA1 File Offset: 0x00010DA1
		public event Action InputPollingStartedEvent
		{
			add
			{
				this._InputPollingStartedEvent = (Action)Delegate.Combine(this._InputPollingStartedEvent, value);
			}
			remove
			{
				this._InputPollingStartedEvent = (Action)Delegate.Remove(this._InputPollingStartedEvent, value);
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000752 RID: 1874 RVA: 0x00012BBA File Offset: 0x00010DBA
		// (remove) Token: 0x06000753 RID: 1875 RVA: 0x00012BD3 File Offset: 0x00010DD3
		public event Action InputPollingEndedEvent
		{
			add
			{
				this._InputPollingEndedEvent = (Action)Delegate.Combine(this._InputPollingEndedEvent, value);
			}
			remove
			{
				this._InputPollingEndedEvent = (Action)Delegate.Remove(this._InputPollingEndedEvent, value);
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000754 RID: 1876 RVA: 0x00012BEC File Offset: 0x00010DEC
		// (remove) Token: 0x06000755 RID: 1877 RVA: 0x00012BFA File Offset: 0x00010DFA
		public event UnityAction onScreenClosed
		{
			add
			{
				this._onScreenClosed.AddListener(value);
			}
			remove
			{
				this._onScreenClosed.RemoveListener(value);
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000756 RID: 1878 RVA: 0x00012C08 File Offset: 0x00010E08
		// (remove) Token: 0x06000757 RID: 1879 RVA: 0x00012C16 File Offset: 0x00010E16
		public event UnityAction onScreenOpened
		{
			add
			{
				this._onScreenOpened.AddListener(value);
			}
			remove
			{
				this._onScreenOpened.RemoveListener(value);
			}
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000758 RID: 1880 RVA: 0x00012C24 File Offset: 0x00010E24
		// (remove) Token: 0x06000759 RID: 1881 RVA: 0x00012C32 File Offset: 0x00010E32
		public event UnityAction onPopupWindowClosed
		{
			add
			{
				this._onPopupWindowClosed.AddListener(value);
			}
			remove
			{
				this._onPopupWindowClosed.RemoveListener(value);
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600075A RID: 1882 RVA: 0x00012C40 File Offset: 0x00010E40
		// (remove) Token: 0x0600075B RID: 1883 RVA: 0x00012C4E File Offset: 0x00010E4E
		public event UnityAction onPopupWindowOpened
		{
			add
			{
				this._onPopupWindowOpened.AddListener(value);
			}
			remove
			{
				this._onPopupWindowOpened.RemoveListener(value);
			}
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600075C RID: 1884 RVA: 0x00012C5C File Offset: 0x00010E5C
		// (remove) Token: 0x0600075D RID: 1885 RVA: 0x00012C6A File Offset: 0x00010E6A
		public event UnityAction onInputPollingStarted
		{
			add
			{
				this._onInputPollingStarted.AddListener(value);
			}
			remove
			{
				this._onInputPollingStarted.RemoveListener(value);
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600075E RID: 1886 RVA: 0x00012C78 File Offset: 0x00010E78
		// (remove) Token: 0x0600075F RID: 1887 RVA: 0x00012C86 File Offset: 0x00010E86
		public event UnityAction onInputPollingEnded
		{
			add
			{
				this._onInputPollingEnded.AddListener(value);
			}
			remove
			{
				this._onInputPollingEnded.RemoveListener(value);
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x00012C94 File Offset: 0x00010E94
		// (set) Token: 0x06000761 RID: 1889 RVA: 0x00012C9C File Offset: 0x00010E9C
		public InputManager rewiredInputManager
		{
			get
			{
				return this._rewiredInputManager;
			}
			set
			{
				this._rewiredInputManager = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x00012CAC File Offset: 0x00010EAC
		// (set) Token: 0x06000763 RID: 1891 RVA: 0x00012CB4 File Offset: 0x00010EB4
		public bool dontDestroyOnLoad
		{
			get
			{
				return this._dontDestroyOnLoad;
			}
			set
			{
				if (value != this._dontDestroyOnLoad && value)
				{
					global::UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
				}
				this._dontDestroyOnLoad = value;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x00012CD9 File Offset: 0x00010ED9
		// (set) Token: 0x06000765 RID: 1893 RVA: 0x00012CE1 File Offset: 0x00010EE1
		public int keyboardMapDefaultLayout
		{
			get
			{
				return this._keyboardMapDefaultLayout;
			}
			set
			{
				this._keyboardMapDefaultLayout = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x00012CF1 File Offset: 0x00010EF1
		// (set) Token: 0x06000767 RID: 1895 RVA: 0x00012CF9 File Offset: 0x00010EF9
		public int mouseMapDefaultLayout
		{
			get
			{
				return this._mouseMapDefaultLayout;
			}
			set
			{
				this._mouseMapDefaultLayout = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x00012D09 File Offset: 0x00010F09
		// (set) Token: 0x06000769 RID: 1897 RVA: 0x00012D11 File Offset: 0x00010F11
		public int joystickMapDefaultLayout
		{
			get
			{
				return this._joystickMapDefaultLayout;
			}
			set
			{
				this._joystickMapDefaultLayout = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x00012D21 File Offset: 0x00010F21
		// (set) Token: 0x0600076B RID: 1899 RVA: 0x00012D3A File Offset: 0x00010F3A
		public bool showPlayers
		{
			get
			{
				return this._showPlayers && ReInput.players.playerCount > 1;
			}
			set
			{
				this._showPlayers = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x00012D4A File Offset: 0x00010F4A
		// (set) Token: 0x0600076D RID: 1901 RVA: 0x00012D52 File Offset: 0x00010F52
		public bool showControllers
		{
			get
			{
				return this._showControllers;
			}
			set
			{
				this._showControllers = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x00012D62 File Offset: 0x00010F62
		// (set) Token: 0x0600076F RID: 1903 RVA: 0x00012D6A File Offset: 0x00010F6A
		public bool showKeyboard
		{
			get
			{
				return this._showKeyboard;
			}
			set
			{
				this._showKeyboard = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x00012D7A File Offset: 0x00010F7A
		// (set) Token: 0x06000771 RID: 1905 RVA: 0x00012D82 File Offset: 0x00010F82
		public bool showMouse
		{
			get
			{
				return this._showMouse;
			}
			set
			{
				this._showMouse = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000772 RID: 1906 RVA: 0x00012D92 File Offset: 0x00010F92
		// (set) Token: 0x06000773 RID: 1907 RVA: 0x00012D9A File Offset: 0x00010F9A
		public int maxControllersPerPlayer
		{
			get
			{
				return this._maxControllersPerPlayer;
			}
			set
			{
				this._maxControllersPerPlayer = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x00012DAA File Offset: 0x00010FAA
		// (set) Token: 0x06000775 RID: 1909 RVA: 0x00012DB2 File Offset: 0x00010FB2
		public bool showActionCategoryLabels
		{
			get
			{
				return this._showActionCategoryLabels;
			}
			set
			{
				this._showActionCategoryLabels = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x00012DC2 File Offset: 0x00010FC2
		// (set) Token: 0x06000777 RID: 1911 RVA: 0x00012DCA File Offset: 0x00010FCA
		public int keyboardInputFieldCount
		{
			get
			{
				return this._keyboardInputFieldCount;
			}
			set
			{
				this._keyboardInputFieldCount = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x00012DDA File Offset: 0x00010FDA
		// (set) Token: 0x06000779 RID: 1913 RVA: 0x00012DE2 File Offset: 0x00010FE2
		public int mouseInputFieldCount
		{
			get
			{
				return this._mouseInputFieldCount;
			}
			set
			{
				this._mouseInputFieldCount = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x00012DF2 File Offset: 0x00010FF2
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x00012DFA File Offset: 0x00010FFA
		public int controllerInputFieldCount
		{
			get
			{
				return this._controllerInputFieldCount;
			}
			set
			{
				this._controllerInputFieldCount = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x00012E0A File Offset: 0x0001100A
		// (set) Token: 0x0600077D RID: 1917 RVA: 0x00012E12 File Offset: 0x00011012
		public bool showFullAxisInputFields
		{
			get
			{
				return this._showFullAxisInputFields;
			}
			set
			{
				this._showFullAxisInputFields = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x0600077E RID: 1918 RVA: 0x00012E22 File Offset: 0x00011022
		// (set) Token: 0x0600077F RID: 1919 RVA: 0x00012E2A File Offset: 0x0001102A
		public bool showSplitAxisInputFields
		{
			get
			{
				return this._showSplitAxisInputFields;
			}
			set
			{
				this._showSplitAxisInputFields = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x00012E3A File Offset: 0x0001103A
		// (set) Token: 0x06000781 RID: 1921 RVA: 0x00012E42 File Offset: 0x00011042
		public bool showGlyphs
		{
			get
			{
				return this._showGlyphs;
			}
			set
			{
				this._showGlyphs = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x00012E52 File Offset: 0x00011052
		// (set) Token: 0x06000783 RID: 1923 RVA: 0x00012E5A File Offset: 0x0001105A
		public bool allowElementAssignmentConflicts
		{
			get
			{
				return this._allowElementAssignmentConflicts;
			}
			set
			{
				this._allowElementAssignmentConflicts = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x00012E6A File Offset: 0x0001106A
		// (set) Token: 0x06000785 RID: 1925 RVA: 0x00012E72 File Offset: 0x00011072
		public bool allowElementAssignmentSwap
		{
			get
			{
				return this._allowElementAssignmentSwap;
			}
			set
			{
				this._allowElementAssignmentSwap = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x00012E82 File Offset: 0x00011082
		// (set) Token: 0x06000787 RID: 1927 RVA: 0x00012E8A File Offset: 0x0001108A
		public int actionLabelWidth
		{
			get
			{
				return this._actionLabelWidth;
			}
			set
			{
				this._actionLabelWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x00012E9A File Offset: 0x0001109A
		// (set) Token: 0x06000789 RID: 1929 RVA: 0x00012EA2 File Offset: 0x000110A2
		public int keyboardColMaxWidth
		{
			get
			{
				return this._keyboardColMaxWidth;
			}
			set
			{
				this._keyboardColMaxWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x00012EB2 File Offset: 0x000110B2
		// (set) Token: 0x0600078B RID: 1931 RVA: 0x00012EBA File Offset: 0x000110BA
		public int mouseColMaxWidth
		{
			get
			{
				return this._mouseColMaxWidth;
			}
			set
			{
				this._mouseColMaxWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x00012ECA File Offset: 0x000110CA
		// (set) Token: 0x0600078D RID: 1933 RVA: 0x00012ED2 File Offset: 0x000110D2
		public int controllerColMaxWidth
		{
			get
			{
				return this._controllerColMaxWidth;
			}
			set
			{
				this._controllerColMaxWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x00012EE2 File Offset: 0x000110E2
		// (set) Token: 0x0600078F RID: 1935 RVA: 0x00012EEA File Offset: 0x000110EA
		public int inputRowHeight
		{
			get
			{
				return this._inputRowHeight;
			}
			set
			{
				this._inputRowHeight = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x00012EFA File Offset: 0x000110FA
		// (set) Token: 0x06000791 RID: 1937 RVA: 0x00012F02 File Offset: 0x00011102
		public int inputColumnSpacing
		{
			get
			{
				return this._inputColumnSpacing;
			}
			set
			{
				this._inputColumnSpacing = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x00012F12 File Offset: 0x00011112
		// (set) Token: 0x06000793 RID: 1939 RVA: 0x00012F1A File Offset: 0x0001111A
		public int inputRowCategorySpacing
		{
			get
			{
				return this._inputRowCategorySpacing;
			}
			set
			{
				this._inputRowCategorySpacing = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x00012F2A File Offset: 0x0001112A
		// (set) Token: 0x06000795 RID: 1941 RVA: 0x00012F32 File Offset: 0x00011132
		public int invertToggleWidth
		{
			get
			{
				return this._invertToggleWidth;
			}
			set
			{
				this._invertToggleWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x00012F42 File Offset: 0x00011142
		// (set) Token: 0x06000797 RID: 1943 RVA: 0x00012F4A File Offset: 0x0001114A
		public int defaultWindowWidth
		{
			get
			{
				return this._defaultWindowWidth;
			}
			set
			{
				this._defaultWindowWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x00012F5A File Offset: 0x0001115A
		// (set) Token: 0x06000799 RID: 1945 RVA: 0x00012F62 File Offset: 0x00011162
		public int defaultWindowHeight
		{
			get
			{
				return this._defaultWindowHeight;
			}
			set
			{
				this._defaultWindowHeight = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x00012F72 File Offset: 0x00011172
		// (set) Token: 0x0600079B RID: 1947 RVA: 0x00012F7A File Offset: 0x0001117A
		public float controllerAssignmentTimeout
		{
			get
			{
				return this._controllerAssignmentTimeout;
			}
			set
			{
				this._controllerAssignmentTimeout = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x00012F8A File Offset: 0x0001118A
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x00012F92 File Offset: 0x00011192
		public float preInputAssignmentTimeout
		{
			get
			{
				return this._preInputAssignmentTimeout;
			}
			set
			{
				this._preInputAssignmentTimeout = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x00012FA2 File Offset: 0x000111A2
		// (set) Token: 0x0600079F RID: 1951 RVA: 0x00012FAA File Offset: 0x000111AA
		public float inputAssignmentTimeout
		{
			get
			{
				return this._inputAssignmentTimeout;
			}
			set
			{
				this._inputAssignmentTimeout = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x00012FBA File Offset: 0x000111BA
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x00012FC2 File Offset: 0x000111C2
		public float axisCalibrationTimeout
		{
			get
			{
				return this._axisCalibrationTimeout;
			}
			set
			{
				this._axisCalibrationTimeout = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x00012FD2 File Offset: 0x000111D2
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x00012FDA File Offset: 0x000111DA
		public bool ignoreMouseXAxisAssignment
		{
			get
			{
				return this._ignoreMouseXAxisAssignment;
			}
			set
			{
				this._ignoreMouseXAxisAssignment = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x00012FEA File Offset: 0x000111EA
		// (set) Token: 0x060007A5 RID: 1957 RVA: 0x00012FF2 File Offset: 0x000111F2
		public bool ignoreMouseYAxisAssignment
		{
			get
			{
				return this._ignoreMouseYAxisAssignment;
			}
			set
			{
				this._ignoreMouseYAxisAssignment = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x00013002 File Offset: 0x00011202
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x0001300A File Offset: 0x0001120A
		public bool universalCancelClosesScreen
		{
			get
			{
				return this._universalCancelClosesScreen;
			}
			set
			{
				this._universalCancelClosesScreen = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x0001301A File Offset: 0x0001121A
		// (set) Token: 0x060007A9 RID: 1961 RVA: 0x00013022 File Offset: 0x00011222
		public bool showInputBehaviorSettings
		{
			get
			{
				return this._showInputBehaviorSettings;
			}
			set
			{
				this._showInputBehaviorSettings = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x00013032 File Offset: 0x00011232
		// (set) Token: 0x060007AB RID: 1963 RVA: 0x0001303A File Offset: 0x0001123A
		public bool useThemeSettings
		{
			get
			{
				return this._useThemeSettings;
			}
			set
			{
				this._useThemeSettings = value;
				this.InspectorPropertyChanged(true);
				if (value)
				{
					this.ApplyTheme();
				}
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x00013053 File Offset: 0x00011253
		// (set) Token: 0x060007AD RID: 1965 RVA: 0x0001305B File Offset: 0x0001125B
		public ThemeSettings themeSettings
		{
			get
			{
				return this._themeSettings;
			}
			set
			{
				this._themeSettings = value;
				this.InspectorPropertyChanged(true);
				this.ApplyTheme();
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x00013071 File Offset: 0x00011271
		// (set) Token: 0x060007AF RID: 1967 RVA: 0x00013079 File Offset: 0x00011279
		public LanguageDataBase language
		{
			get
			{
				return this._language;
			}
			set
			{
				this._language = value;
				if (this._language != null)
				{
					this._language.Initialize();
				}
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x000130A2 File Offset: 0x000112A2
		// (set) Token: 0x060007B1 RID: 1969 RVA: 0x000130AA File Offset: 0x000112AA
		public bool showPlayersGroupLabel
		{
			get
			{
				return this._showPlayersGroupLabel;
			}
			set
			{
				this._showPlayersGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x000130BA File Offset: 0x000112BA
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x000130C2 File Offset: 0x000112C2
		public bool showControllerGroupLabel
		{
			get
			{
				return this._showControllerGroupLabel;
			}
			set
			{
				this._showControllerGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x000130D2 File Offset: 0x000112D2
		// (set) Token: 0x060007B5 RID: 1973 RVA: 0x000130DA File Offset: 0x000112DA
		public bool showAssignedControllersGroupLabel
		{
			get
			{
				return this._showAssignedControllersGroupLabel;
			}
			set
			{
				this._showAssignedControllersGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x000130EA File Offset: 0x000112EA
		// (set) Token: 0x060007B7 RID: 1975 RVA: 0x000130F2 File Offset: 0x000112F2
		public bool showSettingsGroupLabel
		{
			get
			{
				return this._showSettingsGroupLabel;
			}
			set
			{
				this._showSettingsGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x00013102 File Offset: 0x00011302
		// (set) Token: 0x060007B9 RID: 1977 RVA: 0x0001310A File Offset: 0x0001130A
		public bool showMapCategoriesGroupLabel
		{
			get
			{
				return this._showMapCategoriesGroupLabel;
			}
			set
			{
				this._showMapCategoriesGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x0001311A File Offset: 0x0001131A
		// (set) Token: 0x060007BB RID: 1979 RVA: 0x00013122 File Offset: 0x00011322
		public bool showControllerNameLabel
		{
			get
			{
				return this._showControllerNameLabel;
			}
			set
			{
				this._showControllerNameLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x00013132 File Offset: 0x00011332
		// (set) Token: 0x060007BD RID: 1981 RVA: 0x0001313A File Offset: 0x0001133A
		public bool showAssignedControllers
		{
			get
			{
				return this._showAssignedControllers;
			}
			set
			{
				this._showAssignedControllers = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x0001314A File Offset: 0x0001134A
		// (set) Token: 0x060007BF RID: 1983 RVA: 0x00013152 File Offset: 0x00011352
		public Action restoreDefaultsDelegate
		{
			get
			{
				return this._restoreDefaultsDelegate;
			}
			set
			{
				this._restoreDefaultsDelegate = value;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x0001315B File Offset: 0x0001135B
		public bool isOpen
		{
			get
			{
				if (!this.initialized)
				{
					return this.references.canvas != null && this.references.canvas.gameObject.activeInHierarchy;
				}
				return this.canvas.activeInHierarchy;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x0001319B File Offset: 0x0001139B
		private bool isFocused
		{
			get
			{
				return this.initialized && !this.windowManager.isWindowOpen;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x000131B5 File Offset: 0x000113B5
		private bool inputAllowed
		{
			get
			{
				return this.blockInputOnFocusEndTime <= Time.unscaledTime;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x000131C8 File Offset: 0x000113C8
		private int inputGridColumnCount
		{
			get
			{
				int num = 1;
				if (this._showKeyboard)
				{
					num++;
				}
				if (this._showMouse)
				{
					num++;
				}
				if (this._showControllers)
				{
					num++;
				}
				return num;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x000131FC File Offset: 0x000113FC
		private int inputGridWidth
		{
			get
			{
				return this._actionLabelWidth + (this._showKeyboard ? this._keyboardColMaxWidth : 0) + (this._showMouse ? this._mouseColMaxWidth : 0) + (this._showControllers ? this._controllerColMaxWidth : 0) + (this.inputGridColumnCount - 1) * this._inputColumnSpacing;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x00013255 File Offset: 0x00011455
		private Player currentPlayer
		{
			get
			{
				return ReInput.players.GetPlayer(this.currentPlayerId);
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x00013267 File Offset: 0x00011467
		private InputCategory currentMapCategory
		{
			get
			{
				return ReInput.mapping.GetMapCategory(this.currentMapCategoryId);
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x0001327C File Offset: 0x0001147C
		private ControlMapper.MappingSet currentMappingSet
		{
			get
			{
				if (this.currentMapCategoryId < 0)
				{
					return null;
				}
				for (int i = 0; i < this._mappingSets.Length; i++)
				{
					if (this._mappingSets[i].mapCategoryId == this.currentMapCategoryId)
					{
						return this._mappingSets[i];
					}
				}
				return null;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x000132C6 File Offset: 0x000114C6
		private Joystick currentJoystick
		{
			get
			{
				return ReInput.controllers.GetJoystick(this.currentJoystickId);
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x000132D8 File Offset: 0x000114D8
		private bool isJoystickSelected
		{
			get
			{
				return this.currentJoystickId >= 0;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x000132E6 File Offset: 0x000114E6
		private GameObject currentUISelection
		{
			get
			{
				if (!(EventSystem.current != null))
				{
					return null;
				}
				return EventSystem.current.currentSelectedGameObject;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x00013301 File Offset: 0x00011501
		private bool showSettings
		{
			get
			{
				return this._showInputBehaviorSettings && this._inputBehaviorSettings.Length != 0;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x00013317 File Offset: 0x00011517
		private bool showMapCategories
		{
			get
			{
				return this._mappingSets != null && this._mappingSets.Length > 1;
			}
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00013331 File Offset: 0x00011531
		private void Awake()
		{
			if (this._dontDestroyOnLoad)
			{
				global::UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
			}
			this.PreInitialize();
			if (this.isOpen)
			{
				this.Initialize();
				this.Open(true);
			}
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00013366 File Offset: 0x00011566
		private void Start()
		{
			if (this._openOnStart)
			{
				this.Open(false);
			}
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00013377 File Offset: 0x00011577
		private void Update()
		{
			if (!this.isOpen)
			{
				return;
			}
			if (!this.initialized)
			{
				return;
			}
			this.CheckUISelection();
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00013391 File Offset: 0x00011591
		private void OnDestroy()
		{
			ReInput.ControllerConnectedEvent -= this.OnJoystickConnected;
			ReInput.ControllerDisconnectedEvent -= this.OnJoystickDisconnected;
			ReInput.ControllerPreDisconnectEvent -= this.OnJoystickPreDisconnect;
			this.UnsubscribeMenuControlInputEvents();
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x000133CC File Offset: 0x000115CC
		private void PreInitialize()
		{
			if (!ReInput.isReady)
			{
				Debug.LogError("Rewired Control Mapper: Rewired has not been initialized! Are you missing a Rewired Input Manager in your scene?");
				return;
			}
			this.SubscribeMenuControlInputEvents();
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x000133E8 File Offset: 0x000115E8
		private void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			if (this._rewiredInputManager == null)
			{
				this._rewiredInputManager = global::UnityEngine.Object.FindObjectOfType<InputManager>();
				if (this._rewiredInputManager == null)
				{
					Debug.LogError("Rewired Control Mapper: A Rewired Input Manager was not assigned in the inspector or found in the current scene! Control Mapper will not function.");
					return;
				}
			}
			if (ControlMapper.Instance != null)
			{
				Debug.LogError("Rewired Control Mapper: Only one ControlMapper can exist at one time!");
				return;
			}
			ControlMapper.Instance = this;
			if (this.prefabs == null || !this.prefabs.Check())
			{
				Debug.LogError("Rewired Control Mapper: All prefabs must be assigned in the inspector!");
				return;
			}
			if (this.references == null || !this.references.Check())
			{
				Debug.LogError("Rewired Control Mapper: All references must be assigned in the inspector!");
				return;
			}
			this.references.inputGridLayoutElement = this.references.inputGridContainer.GetComponent<LayoutElement>();
			if (this.references.inputGridLayoutElement == null)
			{
				Debug.LogError("Rewired Control Mapper: InputGridContainer is missing LayoutElement component!");
				return;
			}
			if (this._showKeyboard && this._keyboardInputFieldCount < 1)
			{
				Debug.LogWarning("Rewired Control Mapper: Keyboard Input Fields must be at least 1!");
				this._keyboardInputFieldCount = 1;
			}
			if (this._showMouse && this._mouseInputFieldCount < 1)
			{
				Debug.LogWarning("Rewired Control Mapper: Mouse Input Fields must be at least 1!");
				this._mouseInputFieldCount = 1;
			}
			if (this._showControllers && this._controllerInputFieldCount < 1)
			{
				Debug.LogWarning("Rewired Control Mapper: Controller Input Fields must be at least 1!");
				this._controllerInputFieldCount = 1;
			}
			if (this._maxControllersPerPlayer < 0)
			{
				Debug.LogWarning("Rewired Control Mapper: Max Controllers Per Player must be at least 0 (no limit)!");
				this._maxControllersPerPlayer = 0;
			}
			if (this._useThemeSettings && this._themeSettings == null)
			{
				Debug.LogWarning("Rewired Control Mapper: To use theming, Theme Settings must be set in the inspector! Theming has been disabled.");
				this._useThemeSettings = false;
			}
			if (this._language == null)
			{
				Debug.LogError("Rawired UI: Language must be set in the inspector!");
				return;
			}
			this._language.Initialize();
			this.inputFieldActivatedDelegate = new Action<InputFieldInfo>(this.OnInputFieldActivated);
			this.inputFieldInvertToggleStateChangedDelegate = new Action<ToggleInfo, bool>(this.OnInputFieldInvertToggleStateChanged);
			ReInput.ControllerConnectedEvent += this.OnJoystickConnected;
			ReInput.ControllerDisconnectedEvent += this.OnJoystickDisconnected;
			ReInput.ControllerPreDisconnectEvent += this.OnJoystickPreDisconnect;
			this.playerCount = ReInput.players.playerCount;
			this.canvas = this.references.canvas.gameObject;
			this.windowManager = new ControlMapper.WindowManager(this.prefabs.window, this.prefabs.fader, this.references.canvas.transform);
			this.playerButtons = new List<ControlMapper.GUIButton>();
			this.mapCategoryButtons = new List<ControlMapper.GUIButton>();
			this.assignedControllerButtons = new List<ControlMapper.GUIButton>();
			this.miscInstantiatedObjects = new List<GameObject>();
			List<ControlMapper.MappingSet> list = new List<ControlMapper.MappingSet>(this._mappingSets.Length);
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				ControlMapper.MappingSet mappingSet = this._mappingSets[i];
				if (mappingSet != null && mappingSet.isValid)
				{
					list.Add(mappingSet);
				}
			}
			this._mappingSets = list.ToArray();
			this.currentMapCategoryId = this._mappingSets[0].mapCategoryId;
			this.Draw();
			this.CreateInputGrid();
			this.CreateLayout();
			this.SubscribeFixedUISelectionEvents();
			this.initialized = true;
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x000136EE File Offset: 0x000118EE
		private void OnJoystickConnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this._showControllers)
			{
				return;
			}
			this.ClearVarsOnJoystickChange();
			this.ForceRefresh();
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x000136EE File Offset: 0x000118EE
		private void OnJoystickDisconnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this._showControllers)
			{
				return;
			}
			this.ClearVarsOnJoystickChange();
			this.ForceRefresh();
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0001370E File Offset: 0x0001190E
		private void OnJoystickPreDisconnect(ControllerStatusChangedEventArgs args)
		{
			if (!this.initialized)
			{
				return;
			}
			bool showControllers = this._showControllers;
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00013720 File Offset: 0x00011920
		public void OnButtonActivated(ButtonInfo buttonInfo)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.inputAllowed)
			{
				return;
			}
			string identifier = buttonInfo.identifier;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(identifier);
			if (num <= 1656078790U)
			{
				if (num <= 1293854844U)
				{
					if (num != 36291085U)
					{
						if (num != 1293854844U)
						{
							return;
						}
						if (!(identifier == "AssignController"))
						{
							return;
						}
						this.ShowAssignControllerWindow();
						return;
					}
					else
					{
						if (!(identifier == "MapCategorySelection"))
						{
							return;
						}
						this.OnMapCategorySelected(buttonInfo.intData, true);
						return;
					}
				}
				else if (num != 1619204974U)
				{
					if (num != 1656078790U)
					{
						return;
					}
					if (!(identifier == "EditInputBehaviors"))
					{
						return;
					}
					this.ShowEditInputBehaviorsWindow();
					return;
				}
				else
				{
					if (!(identifier == "PlayerSelection"))
					{
						return;
					}
					this.OnPlayerSelected(buttonInfo.intData, true);
					return;
				}
			}
			else if (num <= 2379421585U)
			{
				if (num != 2139278426U)
				{
					if (num != 2379421585U)
					{
						return;
					}
					if (!(identifier == "Done"))
					{
						return;
					}
					this.Close(true);
					return;
				}
				else
				{
					if (!(identifier == "CalibrateController"))
					{
						return;
					}
					this.ShowCalibrateControllerWindow();
					return;
				}
			}
			else if (num != 2857234147U)
			{
				if (num != 3019194153U)
				{
					if (num != 3496297297U)
					{
						return;
					}
					if (!(identifier == "AssignedControllerSelection"))
					{
						return;
					}
					this.OnControllerSelected(buttonInfo.intData);
					return;
				}
				else
				{
					if (!(identifier == "RemoveController"))
					{
						return;
					}
					this.OnRemoveCurrentController();
					return;
				}
			}
			else
			{
				if (!(identifier == "RestoreDefaults"))
				{
					return;
				}
				this.OnRestoreDefaults();
				return;
			}
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00013888 File Offset: 0x00011A88
		public void OnInputFieldActivated(InputFieldInfo fieldInfo)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.inputAllowed)
			{
				return;
			}
			if (this.currentPlayer == null)
			{
				return;
			}
			InputAction action = ReInput.mapping.GetAction(fieldInfo.actionId);
			if (action == null)
			{
				return;
			}
			AxisRange axisRange = ((action.type == InputActionType.Axis) ? fieldInfo.axisRange : AxisRange.Full);
			string actionName = this._language.GetActionName(action.id, axisRange);
			ControllerMap controllerMap = this.GetControllerMap(fieldInfo.controllerType);
			if (controllerMap == null)
			{
				return;
			}
			ActionElementMap actionElementMap = ((fieldInfo.actionElementMapId >= 0) ? controllerMap.GetElementMap(fieldInfo.actionElementMapId) : null);
			if (actionElementMap != null)
			{
				this.ShowBeginElementAssignmentReplacementWindow(fieldInfo, action, controllerMap, actionElementMap, actionName);
				return;
			}
			this.ShowCreateNewElementAssignmentWindow(fieldInfo, action, controllerMap, actionName);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00013930 File Offset: 0x00011B30
		public void OnInputFieldInvertToggleStateChanged(ToggleInfo toggleInfo, bool newState)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.inputAllowed)
			{
				return;
			}
			this.SetActionAxisInverted(newState, toggleInfo.controllerType, toggleInfo.actionElementMapId);
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00013957 File Offset: 0x00011B57
		private void OnPlayerSelected(int playerId, bool redraw)
		{
			if (!this.initialized)
			{
				return;
			}
			this.currentPlayerId = playerId;
			this.ClearVarsOnPlayerChange();
			if (redraw)
			{
				this.Redraw(true, true);
			}
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x0001397A File Offset: 0x00011B7A
		private void OnControllerSelected(int joystickId)
		{
			if (!this.initialized)
			{
				return;
			}
			this.currentJoystickId = joystickId;
			this.Redraw(true, true);
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00013994 File Offset: 0x00011B94
		private void OnRemoveCurrentController()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (this.currentJoystickId < 0)
			{
				return;
			}
			this.RemoveController(this.currentPlayer, this.currentJoystickId);
			this.ClearVarsOnJoystickChange();
			this.Redraw(false, false);
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x000139C9 File Offset: 0x00011BC9
		private void OnMapCategorySelected(int id, bool redraw)
		{
			if (!this.initialized)
			{
				return;
			}
			this.currentMapCategoryId = id;
			if (redraw)
			{
				this.Redraw(true, true);
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x000139E6 File Offset: 0x00011BE6
		private void OnRestoreDefaults()
		{
			if (!this.initialized)
			{
				return;
			}
			this.ShowRestoreDefaultsWindow();
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x000139F7 File Offset: 0x00011BF7
		private void OnScreenToggleActionPressed(InputActionEventData data)
		{
			if (!this.isOpen)
			{
				this.Open();
				return;
			}
			if (!this.initialized)
			{
				return;
			}
			if (!this.isFocused)
			{
				return;
			}
			this.Close(true);
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00013A21 File Offset: 0x00011C21
		private void OnScreenOpenActionPressed(InputActionEventData data)
		{
			this.Open();
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00013A29 File Offset: 0x00011C29
		private void OnScreenCloseActionPressed(InputActionEventData data)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (!this.isFocused)
			{
				return;
			}
			this.Close(true);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00013A4D File Offset: 0x00011C4D
		private void OnUniversalCancelActionPressed(InputActionEventData data)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (this._universalCancelClosesScreen)
			{
				if (this.isFocused)
				{
					this.Close(true);
					return;
				}
			}
			else if (this.isFocused)
			{
				return;
			}
			this.CloseAllWindows();
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00013A88 File Offset: 0x00011C88
		private void OnWindowCancel(int windowId)
		{
			if (!this.initialized)
			{
				return;
			}
			if (windowId < 0)
			{
				return;
			}
			this.CloseWindow(windowId);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00013A9F File Offset: 0x00011C9F
		private void OnRemoveElementAssignment(int windowId, ControllerMap map, ActionElementMap aem)
		{
			if (map == null || aem == null)
			{
				return;
			}
			map.DeleteElementMap(aem.id);
			this.CloseWindow(windowId);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00013ABC File Offset: 0x00011CBC
		private void OnBeginElementAssignment(InputFieldInfo fieldInfo, ControllerMap map, ActionElementMap aem, string actionName)
		{
			if (fieldInfo == null || map == null)
			{
				return;
			}
			this.pendingInputMapping = new ControlMapper.InputMapping(actionName, fieldInfo, map, aem, fieldInfo.controllerType, fieldInfo.controllerId);
			switch (fieldInfo.controllerType)
			{
			case ControllerType.Keyboard:
				this.ShowElementAssignmentPollingWindow();
				return;
			case ControllerType.Mouse:
				this.ShowElementAssignmentPollingWindow();
				return;
			case ControllerType.Joystick:
				this.ShowElementAssignmentPrePollingWindow();
				return;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00013B27 File Offset: 0x00011D27
		private void OnControllerAssignmentConfirmed(int windowId, Player player, int controllerId)
		{
			if (windowId < 0 || player == null || controllerId < 0)
			{
				return;
			}
			this.AssignController(player, controllerId);
			this.CloseWindow(windowId);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00013B44 File Offset: 0x00011D44
		private void OnMouseAssignmentConfirmed(int windowId, Player player)
		{
			if (windowId < 0 || player == null)
			{
				return;
			}
			IList<Player> players = ReInput.players.Players;
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i] != player)
				{
					players[i].controllers.hasMouse = false;
				}
			}
			player.controllers.hasMouse = true;
			this.CloseWindow(windowId);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00013BA4 File Offset: 0x00011DA4
		private void OnElementAssignmentConflictReplaceConfirmed(int windowId, ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers, bool allowSwap)
		{
			if (this.currentPlayer == null || mapping == null)
			{
				return;
			}
			ElementAssignmentConflictCheck elementAssignmentConflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out elementAssignmentConflictCheck))
			{
				Debug.LogError("Rewired Control Mapper: Error creating conflict check!");
				this.CloseWindow(windowId);
				return;
			}
			ElementAssignmentConflictInfo elementAssignmentConflictInfo = default(ElementAssignmentConflictInfo);
			ActionElementMap actionElementMap = null;
			ActionElementMap actionElementMap2 = null;
			bool flag = false;
			if (allowSwap && mapping.aem != null && this.GetFirstElementAssignmentConflict(elementAssignmentConflictCheck, out elementAssignmentConflictInfo, skipOtherPlayers))
			{
				flag = true;
				actionElementMap2 = new ActionElementMap(mapping.aem);
				actionElementMap = new ActionElementMap(elementAssignmentConflictInfo.elementMap);
			}
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				Player player = allPlayers[i];
				if (!skipOtherPlayers || player == this.currentPlayer || player == ReInput.players.SystemPlayer)
				{
					player.controllers.conflictChecking.RemoveElementAssignmentConflicts(elementAssignmentConflictCheck);
				}
			}
			mapping.map.ReplaceOrCreateElementMap(assignment);
			if (allowSwap && flag)
			{
				int actionId = actionElementMap.actionId;
				Pole axisContribution = actionElementMap.axisContribution;
				bool flag2 = actionElementMap.invert;
				AxisRange axisRange = actionElementMap2.axisRange;
				ControllerElementType elementType = actionElementMap2.elementType;
				int elementIdentifierId = actionElementMap2.elementIdentifierId;
				KeyCode keyCode = actionElementMap2.keyCode;
				ModifierKeyFlags modifierKeyFlags = actionElementMap2.modifierKeyFlags;
				if (elementType == actionElementMap.elementType && elementType == ControllerElementType.Axis)
				{
					if (axisRange != actionElementMap.axisRange)
					{
						if (axisRange == AxisRange.Full)
						{
							axisRange = AxisRange.Positive;
						}
						else if (actionElementMap.axisRange == AxisRange.Full)
						{
						}
					}
				}
				else if (elementType == ControllerElementType.Axis && (actionElementMap.elementType == ControllerElementType.Button || (actionElementMap.elementType == ControllerElementType.Axis && actionElementMap.axisRange != AxisRange.Full)) && axisRange == AxisRange.Full)
				{
					axisRange = AxisRange.Positive;
				}
				if (elementType != ControllerElementType.Axis || axisRange != AxisRange.Full)
				{
					flag2 = false;
				}
				int num = 0;
				foreach (ActionElementMap actionElementMap3 in elementAssignmentConflictInfo.controllerMap.ElementMapsWithAction(actionId))
				{
					if (this.SwapIsSameInputRange(elementType, axisRange, axisContribution, actionElementMap3.elementType, actionElementMap3.axisRange, actionElementMap3.axisContribution))
					{
						num++;
					}
				}
				if (num < this.GetControllerInputFieldCount(mapping.controllerType))
				{
					elementAssignmentConflictInfo.controllerMap.ReplaceOrCreateElementMap(ElementAssignment.CompleteAssignment(mapping.controllerType, elementType, elementIdentifierId, axisRange, keyCode, modifierKeyFlags, actionId, axisContribution, flag2));
				}
			}
			this.CloseWindow(windowId);
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00013DE0 File Offset: 0x00011FE0
		private void OnElementAssignmentAddConfirmed(int windowId, ControlMapper.InputMapping mapping, ElementAssignment assignment)
		{
			if (this.currentPlayer == null || mapping == null)
			{
				return;
			}
			mapping.map.ReplaceOrCreateElementMap(assignment);
			this.CloseWindow(windowId);
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00013E04 File Offset: 0x00012004
		private void OnRestoreDefaultsConfirmed(int windowId)
		{
			if (this._restoreDefaultsDelegate == null)
			{
				IList<Player> players = ReInput.players.Players;
				for (int i = 0; i < players.Count; i++)
				{
					Player player = players[i];
					if (this._showControllers)
					{
						player.controllers.maps.LoadDefaultMaps(ControllerType.Joystick);
					}
					if (this._showKeyboard)
					{
						player.controllers.maps.LoadDefaultMaps(ControllerType.Keyboard);
					}
					if (this._showMouse)
					{
						player.controllers.maps.LoadDefaultMaps(ControllerType.Mouse);
					}
				}
			}
			this.CloseWindow(windowId);
			if (this._restoreDefaultsDelegate != null)
			{
				this._restoreDefaultsDelegate();
			}
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00013EA4 File Offset: 0x000120A4
		private void OnAssignControllerWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			this.InputPollingStarted();
			if (window.timer.finished)
			{
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			ControllerPollingInfo controllerPollingInfo = ReInput.controllers.polling.PollAllControllersOfTypeForFirstElementDown(ControllerType.Joystick);
			if (!controllerPollingInfo.success)
			{
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
				return;
			}
			this.InputPollingStopped();
			if (ReInput.controllers.IsControllerAssigned(ControllerType.Joystick, controllerPollingInfo.controllerId) && !this.currentPlayer.controllers.ContainsController(ControllerType.Joystick, controllerPollingInfo.controllerId))
			{
				this.ShowControllerAssignmentConflictWindow(controllerPollingInfo.controllerId);
				return;
			}
			this.OnControllerAssignmentConfirmed(windowId, this.currentPlayer, controllerPollingInfo.controllerId);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00013F80 File Offset: 0x00012180
		private void OnElementAssignmentPrePollingWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingInputMapping == null)
			{
				return;
			}
			this.InputPollingStarted();
			if (!window.timer.finished)
			{
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
				ControllerType controllerType = this.pendingInputMapping.controllerType;
				ControllerPollingInfo controllerPollingInfo;
				if (controllerType > ControllerType.Mouse)
				{
					if (controllerType != ControllerType.Joystick)
					{
						throw new NotImplementedException();
					}
					if (this.currentPlayer.controllers.joystickCount == 0)
					{
						return;
					}
					controllerPollingInfo = ReInput.controllers.polling.PollControllerForFirstButtonDown(this.pendingInputMapping.controllerType, this.currentJoystick.id);
				}
				else
				{
					controllerPollingInfo = ReInput.controllers.polling.PollControllerForFirstButtonDown(this.pendingInputMapping.controllerType, 0);
				}
				if (!controllerPollingInfo.success)
				{
					return;
				}
			}
			this.ShowElementAssignmentPollingWindow();
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0001406C File Offset: 0x0001226C
		private void OnJoystickElementAssignmentPollingWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingInputMapping == null)
			{
				return;
			}
			this.InputPollingStarted();
			if (window.timer.finished)
			{
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
			if (this.currentPlayer.controllers.joystickCount == 0)
			{
				return;
			}
			ControllerPollingInfo controllerPollingInfo = ReInput.controllers.polling.PollControllerForFirstElementDown(ControllerType.Joystick, this.currentJoystick.id);
			if (!controllerPollingInfo.success)
			{
				return;
			}
			if (!this.IsAllowedAssignment(this.pendingInputMapping, controllerPollingInfo))
			{
				return;
			}
			ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(controllerPollingInfo);
			if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, false))
			{
				this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			this.InputPollingStopped();
			this.ShowElementAssignmentConflictWindow(elementAssignment, false);
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00014178 File Offset: 0x00012378
		private void OnKeyboardElementAssignmentPollingWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingInputMapping == null)
			{
				return;
			}
			this.InputPollingStarted();
			if (window.timer.finished)
			{
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			ControllerPollingInfo controllerPollingInfo;
			bool flag;
			ModifierKeyFlags modifierKeyFlags;
			string text;
			this.PollKeyboardForAssignment(out controllerPollingInfo, out flag, out modifierKeyFlags, out text);
			if (flag)
			{
				window.timer.Start(this._inputAssignmentTimeout);
			}
			window.SetContentText(flag ? string.Empty : Mathf.CeilToInt(window.timer.remaining).ToString(), 2);
			window.SetContentText(text, 1);
			if (!controllerPollingInfo.success)
			{
				return;
			}
			if (!this.IsAllowedAssignment(this.pendingInputMapping, controllerPollingInfo))
			{
				return;
			}
			ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(controllerPollingInfo, modifierKeyFlags);
			if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, false))
			{
				this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			this.InputPollingStopped();
			this.ShowElementAssignmentConflictWindow(elementAssignment, false);
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00014290 File Offset: 0x00012490
		private void OnMouseElementAssignmentPollingWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingInputMapping == null)
			{
				return;
			}
			this.InputPollingStarted();
			if (window.timer.finished)
			{
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
			ControllerPollingInfo controllerPollingInfo;
			if (this._ignoreMouseXAxisAssignment || this._ignoreMouseYAxisAssignment)
			{
				controllerPollingInfo = default(ControllerPollingInfo);
				using (IEnumerator<ControllerPollingInfo> enumerator = ReInput.controllers.polling.PollControllerForAllElementsDown(ControllerType.Mouse, 0).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ControllerPollingInfo controllerPollingInfo2 = enumerator.Current;
						if (controllerPollingInfo2.elementType != ControllerElementType.Axis || ((!this._ignoreMouseXAxisAssignment || controllerPollingInfo2.elementIndex != 0) && (!this._ignoreMouseYAxisAssignment || controllerPollingInfo2.elementIndex != 1)))
						{
							controllerPollingInfo = controllerPollingInfo2;
							break;
						}
					}
					goto IL_00F9;
				}
			}
			controllerPollingInfo = ReInput.controllers.polling.PollControllerForFirstElementDown(ControllerType.Mouse, 0);
			IL_00F9:
			if (!controllerPollingInfo.success)
			{
				return;
			}
			if (!this.IsAllowedAssignment(this.pendingInputMapping, controllerPollingInfo))
			{
				return;
			}
			ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(controllerPollingInfo);
			if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, true))
			{
				this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			this.InputPollingStopped();
			this.ShowElementAssignmentConflictWindow(elementAssignment, true);
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00014414 File Offset: 0x00012614
		private void OnCalibrateAxisStep1WindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingAxisCalibration == null || !this.pendingAxisCalibration.isValid)
			{
				return;
			}
			this.InputPollingStarted();
			if (!window.timer.finished)
			{
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
				if (this.currentPlayer.controllers.joystickCount == 0)
				{
					return;
				}
				if (!this.pendingAxisCalibration.joystick.PollForFirstButtonDown().success)
				{
					return;
				}
			}
			this.pendingAxisCalibration.RecordZero();
			this.CloseWindow(windowId);
			this.ShowCalibrateAxisStep2Window();
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x000144CC File Offset: 0x000126CC
		private void OnCalibrateAxisStep2WindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingAxisCalibration == null || !this.pendingAxisCalibration.isValid)
			{
				return;
			}
			if (!window.timer.finished)
			{
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
				this.pendingAxisCalibration.RecordMinMax();
				if (this.currentPlayer.controllers.joystickCount == 0)
				{
					return;
				}
				if (!this.pendingAxisCalibration.joystick.PollForFirstButtonDown().success)
				{
					return;
				}
			}
			this.EndAxisCalibration();
			this.InputPollingStopped();
			this.CloseWindow(windowId);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00014584 File Offset: 0x00012784
		private void ShowAssignControllerWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (ReInput.controllers.joystickCount == 0)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.SetUpdateCallback(new Action<int>(this.OnAssignControllerWindowUpdate));
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.assignControllerWindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.assignControllerWindowMessage);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
			window.timer.Start(this._controllerAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00014664 File Offset: 0x00012864
		private void ShowControllerAssignmentConflictWindow(int controllerId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (ReInput.controllers.joystickCount == 0)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			string text = string.Empty;
			IList<Player> players = ReInput.players.Players;
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i] != this.currentPlayer && players[i].controllers.ContainsController(ControllerType.Joystick, controllerId))
				{
					text = this._language.GetPlayerName(players[i].id);
					break;
				}
			}
			Joystick joystick = ReInput.controllers.GetJoystick(controllerId);
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.controllerAssignmentConflictWindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetControllerAssignmentConflictWindowMessage(this._language.GetControllerName(joystick), text, this._language.GetPlayerName(this.currentPlayer.id)));
			UnityAction unityAction = delegate
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.yes, delegate
			{
				this.OnControllerAssignmentConfirmed(window.id, this.currentPlayer, controllerId);
			}, unityAction, true);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.no, unityAction, unityAction, false);
			this.windowManager.Focus(window);
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00014858 File Offset: 0x00012A58
		private void ShowBeginElementAssignmentReplacementWindow(InputFieldInfo fieldInfo, InputAction action, ControllerMap map, ActionElementMap aem, string actionName)
		{
			ControlMapper.GUIInputField guiinputField = this.inputGrid.GetGUIInputField(this.currentMapCategoryId, action.id, fieldInfo.axisRange, fieldInfo.controllerType, fieldInfo.intData);
			if (guiinputField == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), guiinputField.GetLabel());
			UnityAction unityAction = delegate
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.replace, delegate
			{
				this.OnBeginElementAssignment(fieldInfo, map, aem, actionName);
			}, unityAction, true);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.remove, delegate
			{
				this.OnRemoveElementAssignment(window.id, map, aem);
			}, unityAction, false);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.cancel, unityAction, unityAction, false);
			this.windowManager.Focus(window);
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00014A18 File Offset: 0x00012C18
		private void ShowCreateNewElementAssignmentWindow(InputFieldInfo fieldInfo, InputAction action, ControllerMap map, string actionName)
		{
			if (this.inputGrid.GetGUIInputField(this.currentMapCategoryId, action.id, fieldInfo.axisRange, fieldInfo.controllerType, fieldInfo.intData) == null)
			{
				return;
			}
			this.OnBeginElementAssignment(fieldInfo, map, null, actionName);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00014A54 File Offset: 0x00012C54
		private void ShowElementAssignmentPrePollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.elementAssignmentPrePollingWindowMessage);
			if (this.prefabs.centerStickGraphic != null)
			{
				window.AddContentImage(this.prefabs.centerStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, new Vector2(0f, 40f));
			}
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
			window.SetUpdateCallback(new Action<int>(this.OnElementAssignmentPrePollingWindowUpdate));
			window.timer.Start(this._preInputAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00014B64 File Offset: 0x00012D64
		private void ShowElementAssignmentPollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			switch (this.pendingInputMapping.controllerType)
			{
			case ControllerType.Keyboard:
				this.ShowKeyboardElementAssignmentPollingWindow();
				return;
			case ControllerType.Mouse:
				if (this.currentPlayer.controllers.hasMouse)
				{
					this.ShowMouseElementAssignmentPollingWindow();
					return;
				}
				this.ShowMouseAssignmentConflictWindow();
				return;
			case ControllerType.Joystick:
				this.ShowJoystickElementAssignmentPollingWindow();
				return;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00014BD0 File Offset: 0x00012DD0
		private void ShowJoystickElementAssignmentPollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			string text = ((this.pendingInputMapping.axisRange == AxisRange.Full && this._showFullAxisInputFields && !this._showSplitAxisInputFields) ? this._language.GetJoystickElementAssignmentPollingWindowMessage_FullAxisFieldOnly(this.pendingInputMapping.actionName) : this._language.GetJoystickElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName));
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), text);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
			window.SetUpdateCallback(new Action<int>(this.OnJoystickElementAssignmentPollingWindowUpdate));
			window.timer.Start(this._inputAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00014CE4 File Offset: 0x00012EE4
		private void ShowKeyboardElementAssignmentPollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetKeyboardElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName));
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -(window.GetContentTextHeight(0) + 50f)), "");
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
			window.SetUpdateCallback(new Action<int>(this.OnKeyboardElementAssignmentPollingWindowUpdate));
			window.timer.Start(this._inputAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00014DFC File Offset: 0x00012FFC
		private void ShowMouseElementAssignmentPollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			string text = ((this.pendingInputMapping.axisRange == AxisRange.Full && this._showFullAxisInputFields && !this._showSplitAxisInputFields) ? this._language.GetMouseElementAssignmentPollingWindowMessage_FullAxisFieldOnly(this.pendingInputMapping.actionName) : this._language.GetMouseElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName));
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), text);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
			window.SetUpdateCallback(new Action<int>(this.OnMouseElementAssignmentPollingWindowUpdate));
			window.timer.Start(this._inputAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00014F10 File Offset: 0x00013110
		private void ShowElementAssignmentConflictWindow(ElementAssignment assignment, bool skipOtherPlayers)
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			bool flag = this.IsBlockingAssignmentConflict(this.pendingInputMapping, assignment, skipOtherPlayers);
			string text = (flag ? this._language.GetElementAlreadyInUseBlocked(this.pendingInputMapping.elementName) : this._language.GetElementAlreadyInUseCanReplace(this.pendingInputMapping.elementName, this._allowElementAssignmentConflicts));
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.elementAssignmentConflictWindowMessage);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), text);
			UnityAction unityAction = delegate
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			if (flag)
			{
				window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.okay, unityAction, unityAction, true);
			}
			else
			{
				window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.replace, delegate
				{
					this.OnElementAssignmentConflictReplaceConfirmed(window.id, this.pendingInputMapping, assignment, skipOtherPlayers, false);
				}, unityAction, true);
				if (this._allowElementAssignmentConflicts)
				{
					window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.add, delegate
					{
						this.OnElementAssignmentAddConfirmed(window.id, this.pendingInputMapping, assignment);
					}, unityAction, false);
				}
				else if (this.ShowSwapButton(window.id, this.pendingInputMapping, assignment, skipOtherPlayers))
				{
					window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.swap, delegate
					{
						this.OnElementAssignmentConflictReplaceConfirmed(window.id, this.pendingInputMapping, assignment, skipOtherPlayers, true);
					}, unityAction, false);
				}
				window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.cancel, unityAction, unityAction, false);
			}
			this.windowManager.Focus(window);
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x00015188 File Offset: 0x00013388
		private void ShowMouseAssignmentConflictWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			string text = string.Empty;
			IList<Player> players = ReInput.players.Players;
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i] != this.currentPlayer && players[i].controllers.hasMouse)
				{
					text = this._language.GetPlayerName(players[i].id);
					break;
				}
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.mouseAssignmentConflictWindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetMouseAssignmentConflictWindowMessage(text, this._language.GetPlayerName(this.currentPlayer.id)));
			UnityAction unityAction = delegate
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.yes, delegate
			{
				this.OnMouseAssignmentConfirmed(window.id, this.currentPlayer);
			}, unityAction, true);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.no, unityAction, unityAction, false);
			this.windowManager.Focus(window);
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00015340 File Offset: 0x00013540
		private void ShowCalibrateControllerWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (this.currentPlayer.controllers.joystickCount == 0)
			{
				return;
			}
			CalibrationWindow calibrationWindow = this.OpenWindow(this.prefabs.calibrationWindow, "CalibrationWindow", true) as CalibrationWindow;
			if (calibrationWindow == null)
			{
				return;
			}
			Joystick currentJoystick = this.currentJoystick;
			calibrationWindow.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.calibrateControllerWindowTitle);
			calibrationWindow.SetJoystick(this.currentPlayer.id, currentJoystick);
			calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Done, new Action<int>(this.CloseWindow));
			calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Calibrate, new Action<int>(this.StartAxisCalibration));
			calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Cancel, new Action<int>(this.CloseWindow));
			this.windowManager.Focus(calibrationWindow);
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00015410 File Offset: 0x00013610
		private void ShowCalibrateAxisStep1Window()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.OpenWindow(false);
			if (window == null)
			{
				return;
			}
			if (this.pendingAxisCalibration == null)
			{
				return;
			}
			Joystick joystick = this.pendingAxisCalibration.joystick;
			if (joystick.axisCount == 0)
			{
				return;
			}
			int axisIndex = this.pendingAxisCalibration.axisIndex;
			if (axisIndex < 0 || axisIndex >= joystick.axisCount)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.calibrateAxisStep1WindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetCalibrateAxisStep1WindowMessage(this._language.GetElementIdentifierName(joystick, joystick.AxisElementIdentifiers[axisIndex].id, AxisRange.Full)));
			if (this.prefabs.centerStickGraphic != null)
			{
				window.AddContentImage(this.prefabs.centerStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, new Vector2(0f, 40f));
			}
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
			window.SetUpdateCallback(new Action<int>(this.OnCalibrateAxisStep1WindowUpdate));
			window.timer.Start(this._axisCalibrationTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00015578 File Offset: 0x00013778
		private void ShowCalibrateAxisStep2Window()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.OpenWindow(false);
			if (window == null)
			{
				return;
			}
			if (this.pendingAxisCalibration == null)
			{
				return;
			}
			Joystick joystick = this.pendingAxisCalibration.joystick;
			if (joystick.axisCount == 0)
			{
				return;
			}
			int axisIndex = this.pendingAxisCalibration.axisIndex;
			if (axisIndex < 0 || axisIndex >= joystick.axisCount)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.calibrateAxisStep2WindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetCalibrateAxisStep2WindowMessage(this._language.GetElementIdentifierName(joystick, joystick.AxisElementIdentifiers[axisIndex].id, AxisRange.Full)));
			if (this.prefabs.moveStickGraphic != null)
			{
				window.AddContentImage(this.prefabs.moveStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, new Vector2(0f, 40f));
			}
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
			window.SetUpdateCallback(new Action<int>(this.OnCalibrateAxisStep2WindowUpdate));
			window.timer.Start(this._axisCalibrationTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x000156E0 File Offset: 0x000138E0
		private void ShowEditInputBehaviorsWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (this._inputBehaviorSettings == null)
			{
				return;
			}
			InputBehaviorWindow inputBehaviorWindow = this.OpenWindow(this.prefabs.inputBehaviorsWindow, "EditInputBehaviorsWindow", true) as InputBehaviorWindow;
			if (inputBehaviorWindow == null)
			{
				return;
			}
			inputBehaviorWindow.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.inputBehaviorSettingsWindowTitle);
			inputBehaviorWindow.SetData(this.currentPlayer.id, this._inputBehaviorSettings);
			inputBehaviorWindow.SetButtonCallback(InputBehaviorWindow.ButtonIdentifier.Done, new Action<int>(this.CloseWindow));
			inputBehaviorWindow.SetButtonCallback(InputBehaviorWindow.ButtonIdentifier.Cancel, new Action<int>(this.CloseWindow));
			this.windowManager.Focus(inputBehaviorWindow);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00015790 File Offset: 0x00013990
		private void ShowRestoreDefaultsWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			this.OpenModal(this._language.restoreDefaultsWindowTitle, this._language.restoreDefaultsWindowMessage, this._language.yes, new Action<int>(this.OnRestoreDefaultsConfirmed), this._language.no, new Action<int>(this.OnWindowCancel), true);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x000157F4 File Offset: 0x000139F4
		private void CreateInputGrid()
		{
			this.InitializeInputGrid();
			this.CreateHeaderLabels();
			this.CreateActionLabelColumn();
			this.CreateKeyboardInputFieldColumn();
			this.CreateMouseInputFieldColumn();
			this.CreateControllerInputFieldColumn();
			this.CreateInputActionLabels();
			this.CreateInputFields();
			this.inputGrid.HideAll();
			this.ResetInputGridScrollBar();
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00015844 File Offset: 0x00013A44
		private void InitializeInputGrid()
		{
			if (this.inputGrid == null)
			{
				this.inputGrid = new ControlMapper.InputGrid();
			}
			else
			{
				this.inputGrid.ClearAll();
			}
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				ControlMapper.MappingSet mappingSet = this._mappingSets[i];
				if (mappingSet != null && mappingSet.isValid)
				{
					InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(mappingSet.mapCategoryId);
					if (mapCategory != null && mapCategory.userAssignable)
					{
						this.inputGrid.AddMapCategory(mappingSet.mapCategoryId);
						if (mappingSet.actionListMode == ControlMapper.MappingSet.ActionListMode.ActionCategory)
						{
							IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
							for (int j = 0; j < actionCategoryIds.Count; j++)
							{
								int num = actionCategoryIds[j];
								InputCategory actionCategory = ReInput.mapping.GetActionCategory(num);
								if (actionCategory != null && actionCategory.userAssignable)
								{
									this.inputGrid.AddActionCategory(mappingSet.mapCategoryId, num);
									foreach (InputAction inputAction in ReInput.mapping.UserAssignableActionsInCategory(num))
									{
										if (inputAction.type == InputActionType.Axis)
										{
											if (this._showFullAxisInputFields)
											{
												this.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, AxisRange.Full);
											}
											if (this._showSplitAxisInputFields)
											{
												this.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, AxisRange.Positive);
												this.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, AxisRange.Negative);
											}
										}
										else if (inputAction.type == InputActionType.Button)
										{
											this.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, AxisRange.Positive);
										}
									}
								}
							}
						}
						else
						{
							IList<int> actionIds = mappingSet.actionIds;
							for (int k = 0; k < actionIds.Count; k++)
							{
								InputAction action = ReInput.mapping.GetAction(actionIds[k]);
								if (action != null)
								{
									if (action.type == InputActionType.Axis)
									{
										if (this._showFullAxisInputFields)
										{
											this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Full);
										}
										if (this._showSplitAxisInputFields)
										{
											this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Positive);
											this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Negative);
										}
									}
									else if (action.type == InputActionType.Button)
									{
										this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Positive);
									}
								}
							}
						}
					}
				}
			}
			this.references.inputGridInnerGroup.GetComponent<HorizontalLayoutGroup>().spacing = (float)this._inputColumnSpacing;
			this.references.inputGridLayoutElement.flexibleWidth = 0f;
			this.references.inputGridLayoutElement.preferredWidth = (float)this.inputGridWidth;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00015B00 File Offset: 0x00013D00
		private void RefreshInputGridStructure()
		{
			if (this.currentMappingSet == null)
			{
				return;
			}
			this.inputGrid.HideAll();
			this.inputGrid.Show(this.currentMappingSet.mapCategoryId);
			this.references.inputGridInnerGroup.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.inputGrid.GetColumnHeight(this.currentMappingSet.mapCategoryId));
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00015B64 File Offset: 0x00013D64
		private void CreateHeaderLabels()
		{
			this.references.inputGridHeader1 = this.CreateNewColumnGroup("ActionsHeader", this.references.inputGridHeadersGroup, this._actionLabelWidth).transform;
			this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.actionColumnLabel, this.references.inputGridHeader1, Vector2.zero);
			if (this._showKeyboard)
			{
				this.references.inputGridHeader2 = this.CreateNewColumnGroup("KeybordHeader", this.references.inputGridHeadersGroup, this._keyboardColMaxWidth).transform;
				this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.keyboardColumnLabel, this.references.inputGridHeader2, Vector2.zero).SetTextAlignment(TextAlignmentOptions.Center);
			}
			if (this._showMouse)
			{
				this.references.inputGridHeader3 = this.CreateNewColumnGroup("MouseHeader", this.references.inputGridHeadersGroup, this._mouseColMaxWidth).transform;
				this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.mouseColumnLabel, this.references.inputGridHeader3, Vector2.zero).SetTextAlignment(TextAlignmentOptions.Center);
			}
			if (this._showControllers)
			{
				this.references.inputGridHeader4 = this.CreateNewColumnGroup("ControllerHeader", this.references.inputGridHeadersGroup, this._controllerColMaxWidth).transform;
				this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.controllerColumnLabel, this.references.inputGridHeader4, Vector2.zero).SetTextAlignment(TextAlignmentOptions.Center);
			}
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00015D08 File Offset: 0x00013F08
		private void CreateActionLabelColumn()
		{
			Transform transform = this.CreateNewColumnGroup("ActionLabelColumn", this.references.inputGridInnerGroup, this._actionLabelWidth).transform;
			this.references.inputGridActionColumn = transform;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00015D43 File Offset: 0x00013F43
		private void CreateKeyboardInputFieldColumn()
		{
			if (!this._showKeyboard)
			{
				return;
			}
			this.CreateInputFieldColumn("KeyboardColumn", ControllerType.Keyboard, this._keyboardColMaxWidth, this._keyboardInputFieldCount, true);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00015D67 File Offset: 0x00013F67
		private void CreateMouseInputFieldColumn()
		{
			if (!this._showMouse)
			{
				return;
			}
			this.CreateInputFieldColumn("MouseColumn", ControllerType.Mouse, this._mouseColMaxWidth, this._mouseInputFieldCount, false);
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00015D8B File Offset: 0x00013F8B
		private void CreateControllerInputFieldColumn()
		{
			if (!this._showControllers)
			{
				return;
			}
			this.CreateInputFieldColumn("ControllerColumn", ControllerType.Joystick, this._controllerColMaxWidth, this._controllerInputFieldCount, false);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00015DB0 File Offset: 0x00013FB0
		private void CreateInputFieldColumn(string name, ControllerType controllerType, int maxWidth, int cols, bool disableFullAxis)
		{
			Transform transform = this.CreateNewColumnGroup(name, this.references.inputGridInnerGroup, maxWidth).transform;
			switch (controllerType)
			{
			case ControllerType.Keyboard:
				this.references.inputGridKeyboardColumn = transform;
				return;
			case ControllerType.Mouse:
				this.references.inputGridMouseColumn = transform;
				return;
			case ControllerType.Joystick:
				this.references.inputGridControllerColumn = transform;
				return;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00015E18 File Offset: 0x00014018
		private void CreateInputActionLabels()
		{
			Transform inputGridActionColumn = this.references.inputGridActionColumn;
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				ControlMapper.MappingSet mappingSet = this._mappingSets[i];
				if (mappingSet != null && mappingSet.isValid)
				{
					int num = 0;
					if (mappingSet.actionListMode == ControlMapper.MappingSet.ActionListMode.ActionCategory)
					{
						int num2 = 0;
						IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
						for (int j = 0; j < actionCategoryIds.Count; j++)
						{
							InputCategory actionCategory = ReInput.mapping.GetActionCategory(actionCategoryIds[j]);
							if (actionCategory != null && actionCategory.userAssignable && this.CountIEnumerable<InputAction>(ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id)) != 0)
							{
								if (this._showActionCategoryLabels)
								{
									if (num2 > 0)
									{
										num -= this._inputRowCategorySpacing;
									}
									ControlMapper.GUILabel guilabel = this.CreateLabel(this._language.GetActionCategoryName(actionCategory.id), inputGridActionColumn, new Vector2(0f, (float)num));
									guilabel.SetFontStyle(FontStyles.Bold);
									guilabel.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
									this.inputGrid.AddActionCategoryLabel(mappingSet.mapCategoryId, actionCategory.id, guilabel);
									num -= this._inputRowHeight;
								}
								foreach (InputAction inputAction in ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id, true))
								{
									if (inputAction.type == InputActionType.Axis)
									{
										if (this._showFullAxisInputFields)
										{
											ControlMapper.GUILabel guilabel2 = this.CreateLabel(this._language.GetActionName(inputAction.id, AxisRange.Full), inputGridActionColumn, new Vector2(0f, (float)num));
											guilabel2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
											this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Full, guilabel2);
											num -= this._inputRowHeight;
										}
										if (this._showSplitAxisInputFields)
										{
											string actionName = this._language.GetActionName(inputAction.id, AxisRange.Positive);
											ControlMapper.GUILabel guilabel2 = this.CreateLabel(actionName, inputGridActionColumn, new Vector2(0f, (float)num));
											guilabel2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
											this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Positive, guilabel2);
											num -= this._inputRowHeight;
											string actionName2 = this._language.GetActionName(inputAction.id, AxisRange.Negative);
											guilabel2 = this.CreateLabel(actionName2, inputGridActionColumn, new Vector2(0f, (float)num));
											guilabel2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
											this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Negative, guilabel2);
											num -= this._inputRowHeight;
										}
									}
									else if (inputAction.type == InputActionType.Button)
									{
										ControlMapper.GUILabel guilabel2 = this.CreateLabel(this._language.GetActionName(inputAction.id), inputGridActionColumn, new Vector2(0f, (float)num));
										guilabel2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
										this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Positive, guilabel2);
										num -= this._inputRowHeight;
									}
								}
								num2++;
							}
						}
					}
					else
					{
						IList<int> actionIds = mappingSet.actionIds;
						for (int k = 0; k < actionIds.Count; k++)
						{
							InputAction action = ReInput.mapping.GetAction(actionIds[k]);
							if (action != null && action.userAssignable)
							{
								InputCategory actionCategory2 = ReInput.mapping.GetActionCategory(action.categoryId);
								if (actionCategory2 != null && actionCategory2.userAssignable)
								{
									if (action.type == InputActionType.Axis)
									{
										if (this._showFullAxisInputFields)
										{
											ControlMapper.GUILabel guilabel3 = this.CreateLabel(this._language.GetActionName(action.id, AxisRange.Full), inputGridActionColumn, new Vector2(0f, (float)num));
											guilabel3.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
											this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Full, guilabel3);
											num -= this._inputRowHeight;
										}
										if (this._showSplitAxisInputFields)
										{
											ControlMapper.GUILabel guilabel3 = this.CreateLabel(this._language.GetActionName(action.id, AxisRange.Positive), inputGridActionColumn, new Vector2(0f, (float)num));
											guilabel3.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
											this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Positive, guilabel3);
											num -= this._inputRowHeight;
											guilabel3 = this.CreateLabel(this._language.GetActionName(action.id, AxisRange.Negative), inputGridActionColumn, new Vector2(0f, (float)num));
											guilabel3.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
											this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Negative, guilabel3);
											num -= this._inputRowHeight;
										}
									}
									else if (action.type == InputActionType.Button)
									{
										ControlMapper.GUILabel guilabel3 = this.CreateLabel(this._language.GetActionName(action.id), inputGridActionColumn, new Vector2(0f, (float)num));
										guilabel3.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
										this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Positive, guilabel3);
										num -= this._inputRowHeight;
									}
								}
							}
						}
					}
					this.inputGrid.SetColumnHeight(mappingSet.mapCategoryId, (float)(-(float)num));
				}
			}
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x000163A4 File Offset: 0x000145A4
		private void CreateInputFields()
		{
			if (this._showControllers)
			{
				this.CreateInputFields(this.references.inputGridControllerColumn, ControllerType.Joystick, this._controllerColMaxWidth, this._controllerInputFieldCount, false);
			}
			if (this._showKeyboard)
			{
				this.CreateInputFields(this.references.inputGridKeyboardColumn, ControllerType.Keyboard, this._keyboardColMaxWidth, this._keyboardInputFieldCount, true);
			}
			if (this._showMouse)
			{
				this.CreateInputFields(this.references.inputGridMouseColumn, ControllerType.Mouse, this._mouseColMaxWidth, this._mouseInputFieldCount, false);
			}
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00016428 File Offset: 0x00014628
		private void CreateInputFields(Transform columnXform, ControllerType controllerType, int maxWidth, int cols, bool disableFullAxis)
		{
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				ControlMapper.MappingSet mappingSet = this._mappingSets[i];
				if (mappingSet != null && mappingSet.isValid)
				{
					int num = maxWidth / cols;
					int num2 = 0;
					int num3 = 0;
					if (mappingSet.actionListMode == ControlMapper.MappingSet.ActionListMode.ActionCategory)
					{
						IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
						for (int j = 0; j < actionCategoryIds.Count; j++)
						{
							InputCategory actionCategory = ReInput.mapping.GetActionCategory(actionCategoryIds[j]);
							if (actionCategory != null && actionCategory.userAssignable && this.CountIEnumerable<InputAction>(ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id)) != 0)
							{
								if (this._showActionCategoryLabels)
								{
									num2 -= ((num3 > 0) ? (this._inputRowHeight + this._inputRowCategorySpacing) : this._inputRowHeight);
								}
								foreach (InputAction inputAction in ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id, true))
								{
									if (inputAction.type == InputActionType.Axis)
									{
										if (this._showFullAxisInputFields)
										{
											this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, AxisRange.Full, controllerType, cols, num, ref num2, disableFullAxis);
										}
										if (this._showSplitAxisInputFields)
										{
											this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, AxisRange.Positive, controllerType, cols, num, ref num2, false);
											this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, AxisRange.Negative, controllerType, cols, num, ref num2, false);
										}
									}
									else if (inputAction.type == InputActionType.Button)
									{
										this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, AxisRange.Positive, controllerType, cols, num, ref num2, false);
									}
									num3++;
								}
							}
						}
					}
					else
					{
						IList<int> actionIds = mappingSet.actionIds;
						for (int k = 0; k < actionIds.Count; k++)
						{
							InputAction action = ReInput.mapping.GetAction(actionIds[k]);
							if (action != null && action.userAssignable)
							{
								InputCategory actionCategory2 = ReInput.mapping.GetActionCategory(action.categoryId);
								if (actionCategory2 != null && actionCategory2.userAssignable)
								{
									if (action.type == InputActionType.Axis)
									{
										if (this._showFullAxisInputFields)
										{
											this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Full, controllerType, cols, num, ref num2, disableFullAxis);
										}
										if (this._showSplitAxisInputFields)
										{
											this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Positive, controllerType, cols, num, ref num2, false);
											this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Negative, controllerType, cols, num, ref num2, false);
										}
									}
									else if (action.type == InputActionType.Button)
									{
										this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Positive, controllerType, cols, num, ref num2, false);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x000166DC File Offset: 0x000148DC
		private void CreateInputFieldSet(Transform parent, int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, int cols, int fieldWidth, ref int yPos, bool disableFullAxis)
		{
			GameObject gameObject = this.CreateNewGUIObject("FieldLayoutGroup", parent, new Vector2(0f, (float)yPos));
			HorizontalLayoutGroup horizontalLayoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.padding = this._inputRowPadding;
			horizontalLayoutGroup.spacing = (float)this._inputRowFieldSpacing;
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 1f);
			component.anchorMax = new Vector2(1f, 1f);
			component.pivot = new Vector2(0f, 1f);
			component.sizeDelta = Vector2.zero;
			component.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
			this.inputGrid.AddInputFieldSet(mapCategoryId, action, axisRange, controllerType, gameObject);
			for (int i = 0; i < cols; i++)
			{
				int num = ((axisRange == AxisRange.Full) ? this._invertToggleWidth : 0);
				ControlMapper.GUIInputField guiinputField = this.CreateInputField(horizontalLayoutGroup.transform, Vector2.zero, "", action.id, axisRange, controllerType, i);
				guiinputField.SetFirstChildObjectWidth(ControlMapper.LayoutElementSizeType.PreferredSize, fieldWidth - num);
				guiinputField.SetFirstChildObjectWidth(ControlMapper.LayoutElementSizeType.MinSize, 0);
				this.inputGrid.AddInputField(mapCategoryId, action, axisRange, controllerType, i, guiinputField);
				if (axisRange == AxisRange.Full)
				{
					if (!disableFullAxis)
					{
						ControlMapper.GUIToggle guitoggle = this.CreateToggle(this.prefabs.inputGridFieldInvertToggle, horizontalLayoutGroup.transform, Vector2.zero, "", action.id, axisRange, controllerType, i);
						guitoggle.SetFirstChildObjectWidth(ControlMapper.LayoutElementSizeType.MinSize, num);
						guiinputField.AddToggle(guitoggle);
					}
					else
					{
						guiinputField.SetInteractible(false, false, true);
					}
				}
			}
			yPos -= this._inputRowHeight;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00016864 File Offset: 0x00014A64
		private void PopulateInputFields()
		{
			this.inputGrid.InitializeFields(this.currentMapCategoryId);
			if (this.currentPlayer == null)
			{
				return;
			}
			this.inputGrid.SetFieldsActive(this.currentMapCategoryId, true);
			foreach (ControlMapper.InputActionSet inputActionSet in this.inputGrid.GetActionSets(this.currentMapCategoryId))
			{
				if (this._showKeyboard)
				{
					ControllerType controllerType = ControllerType.Keyboard;
					int num = 0;
					int num2 = this._keyboardMapDefaultLayout;
					int num3 = this._keyboardInputFieldCount;
					ControllerMap controllerMapOrCreateNew = this.GetControllerMapOrCreateNew(controllerType, num, num2);
					this.PopulateInputFieldGroup(inputActionSet, controllerMapOrCreateNew, controllerType, num, num3);
				}
				if (this._showMouse)
				{
					ControllerType controllerType = ControllerType.Mouse;
					int num = 0;
					int num2 = this._mouseMapDefaultLayout;
					int num3 = this._mouseInputFieldCount;
					ControllerMap controllerMapOrCreateNew2 = this.GetControllerMapOrCreateNew(controllerType, num, num2);
					if (this.currentPlayer.controllers.hasMouse)
					{
						this.PopulateInputFieldGroup(inputActionSet, controllerMapOrCreateNew2, controllerType, num, num3);
					}
				}
				if (this.isJoystickSelected && this.currentPlayer.controllers.joystickCount > 0)
				{
					ControllerType controllerType = ControllerType.Joystick;
					int num = this.currentJoystick.id;
					int num2 = this._joystickMapDefaultLayout;
					int num3 = this._controllerInputFieldCount;
					ControllerMap controllerMapOrCreateNew3 = this.GetControllerMapOrCreateNew(controllerType, num, num2);
					this.PopulateInputFieldGroup(inputActionSet, controllerMapOrCreateNew3, controllerType, num, num3);
				}
				else
				{
					this.DisableInputFieldGroup(inputActionSet, ControllerType.Joystick, this._controllerInputFieldCount);
				}
			}
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x000169C8 File Offset: 0x00014BC8
		private void PopulateInputFieldGroup(ControlMapper.InputActionSet actionSet, ControllerMap controllerMap, ControllerType controllerType, int controllerId, int maxFields)
		{
			if (controllerMap == null)
			{
				return;
			}
			int num = 0;
			this.inputGrid.SetFixedFieldData(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId);
			foreach (ActionElementMap actionElementMap in controllerMap.ElementMapsWithAction(actionSet.actionId))
			{
				if (actionElementMap.elementType == ControllerElementType.Button)
				{
					if (actionSet.axisRange == AxisRange.Full)
					{
						continue;
					}
					if (actionSet.axisRange == AxisRange.Positive)
					{
						if (actionElementMap.axisContribution == Pole.Negative)
						{
							continue;
						}
					}
					else if (actionSet.axisRange == AxisRange.Negative && actionElementMap.axisContribution == Pole.Positive)
					{
						continue;
					}
					this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, this._language.GetElementIdentifierName(actionElementMap), false);
				}
				else if (actionElementMap.elementType == ControllerElementType.Axis)
				{
					if (actionSet.axisRange == AxisRange.Full)
					{
						if (actionElementMap.axisRange != AxisRange.Full)
						{
							continue;
						}
						this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, this._language.GetElementIdentifierName(actionElementMap), actionElementMap.invert);
					}
					else if (actionSet.axisRange == AxisRange.Positive)
					{
						if ((actionElementMap.axisRange == AxisRange.Full && ReInput.mapping.GetAction(actionSet.actionId).type != InputActionType.Button) || actionElementMap.axisContribution == Pole.Negative)
						{
							continue;
						}
						this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, this._language.GetElementIdentifierName(actionElementMap), false);
					}
					else if (actionSet.axisRange == AxisRange.Negative)
					{
						if (actionElementMap.axisRange == AxisRange.Full || actionElementMap.axisContribution == Pole.Positive)
						{
							continue;
						}
						this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, this._language.GetElementIdentifierName(actionElementMap), false);
					}
				}
				num++;
				if (num > maxFields)
				{
					break;
				}
			}
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00016BE8 File Offset: 0x00014DE8
		private void DisableInputFieldGroup(ControlMapper.InputActionSet actionSet, ControllerType controllerType, int fieldCount)
		{
			for (int i = 0; i < fieldCount; i++)
			{
				ControlMapper.GUIInputField guiinputField = this.inputGrid.GetGUIInputField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, i);
				if (guiinputField != null)
				{
					guiinputField.SetInteractible(false, false);
				}
			}
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00016C2C File Offset: 0x00014E2C
		private void ResetInputGridScrollBar()
		{
			this.references.inputGridInnerGroup.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			this.references.inputGridVScrollbar.value = 1f;
			this.references.inputGridScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00016C7C File Offset: 0x00014E7C
		private void CreateLayout()
		{
			this.references.playersGroup.gameObject.SetActive(this.showPlayers);
			this.references.controllerGroup.gameObject.SetActive(this._showControllers);
			this.references.assignedControllersGroup.gameObject.SetActive(this._showControllers && this.ShowAssignedControllers());
			this.references.settingsAndMapCategoriesGroup.gameObject.SetActive(this.showSettings || this.showMapCategories);
			this.references.settingsGroup.gameObject.SetActive(this.showSettings);
			this.references.mapCategoriesGroup.gameObject.SetActive(this.showMapCategories);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00016D41 File Offset: 0x00014F41
		private void Draw()
		{
			this.DrawPlayersGroup();
			this.DrawControllersGroup();
			this.DrawSettingsGroup();
			this.DrawMapCategoriesGroup();
			this.DrawWindowButtonsGroup();
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x00016D64 File Offset: 0x00014F64
		private void DrawPlayersGroup()
		{
			if (!this.showPlayers)
			{
				return;
			}
			this.references.playersGroup.labelText = this._language.playersGroupLabel;
			this.references.playersGroup.SetLabelActive(this._showPlayersGroupLabel);
			for (int i = 0; i < this.playerCount; i++)
			{
				Player player = ReInput.players.GetPlayer(i);
				if (player != null)
				{
					ControlMapper.GUIButton guibutton = new ControlMapper.GUIButton(UITools.InstantiateGUIObject<ButtonInfo>(this.prefabs.button, this.references.playersGroup.content, "Player" + i.ToString() + "Button"));
					guibutton.SetLabel(this._language.GetPlayerName(player.id));
					guibutton.SetButtonInfoData("PlayerSelection", player.id);
					guibutton.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
					guibutton.buttonInfo.OnSelectedEvent += this.OnUIElementSelected;
					this.playerButtons.Add(guibutton);
				}
			}
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00016E70 File Offset: 0x00015070
		private void DrawControllersGroup()
		{
			if (!this._showControllers)
			{
				return;
			}
			this.references.controllerSettingsGroup.labelText = this._language.controllerSettingsGroupLabel;
			this.references.controllerSettingsGroup.SetLabelActive(this._showControllerGroupLabel);
			this.references.controllerNameLabel.gameObject.SetActive(this._showControllerNameLabel);
			this.references.controllerGroupLabelGroup.gameObject.SetActive(this._showControllerGroupLabel || this._showControllerNameLabel);
			if (this.ShowAssignedControllers())
			{
				this.references.assignedControllersGroup.labelText = this._language.assignedControllersGroupLabel;
				this.references.assignedControllersGroup.SetLabelActive(this._showAssignedControllersGroupLabel);
			}
			this.references.removeControllerButton.GetComponent<ButtonInfo>().text.text = this._language.removeControllerButtonLabel;
			this.references.calibrateControllerButton.GetComponent<ButtonInfo>().text.text = this._language.calibrateControllerButtonLabel;
			this.references.assignControllerButton.GetComponent<ButtonInfo>().text.text = this._language.assignControllerButtonLabel;
			ControlMapper.GUIButton guibutton = this.CreateButton(this._language.none, this.references.assignedControllersGroup.content, Vector2.zero);
			guibutton.SetInteractible(false, false, true);
			this.assignedControllerButtonsPlaceholder = guibutton;
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00016FD8 File Offset: 0x000151D8
		private void DrawSettingsGroup()
		{
			if (!this.showSettings)
			{
				return;
			}
			this.references.settingsGroup.labelText = this._language.settingsGroupLabel;
			this.references.settingsGroup.SetLabelActive(this._showSettingsGroupLabel);
			ControlMapper.GUIButton guibutton = this.CreateButton(this._language.inputBehaviorSettingsButtonLabel, this.references.settingsGroup.content, Vector2.zero);
			this.miscInstantiatedObjects.Add(guibutton.gameObject);
			guibutton.buttonInfo.OnSelectedEvent += this.OnUIElementSelected;
			guibutton.SetButtonInfoData("EditInputBehaviors", 0);
			guibutton.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0001708C File Offset: 0x0001528C
		private void DrawMapCategoriesGroup()
		{
			if (!this.showMapCategories)
			{
				return;
			}
			if (this._mappingSets == null)
			{
				return;
			}
			this.references.mapCategoriesGroup.labelText = this._language.mapCategoriesGroupLabel;
			this.references.mapCategoriesGroup.SetLabelActive(this._showMapCategoriesGroupLabel);
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				ControlMapper.MappingSet mappingSet = this._mappingSets[i];
				if (mappingSet != null && mappingSet.isValid)
				{
					InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(mappingSet.mapCategoryId);
					if (mapCategory != null)
					{
						ControlMapper.GUIButton guibutton = new ControlMapper.GUIButton(UITools.InstantiateGUIObject<ButtonInfo>(this.prefabs.button, this.references.mapCategoriesGroup.content, mapCategory.name + "Button"));
						guibutton.SetLabel(this._language.GetMapCategoryName(mapCategory.id));
						guibutton.SetButtonInfoData("MapCategorySelection", mapCategory.id);
						guibutton.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
						guibutton.buttonInfo.OnSelectedEvent += this.OnUIElementSelected;
						this.mapCategoryButtons.Add(guibutton);
					}
				}
			}
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x000171BC File Offset: 0x000153BC
		private void DrawWindowButtonsGroup()
		{
			this.references.doneButton.GetComponent<ButtonInfo>().text.text = this._language.doneButtonLabel;
			this.references.restoreDefaultsButton.GetComponent<ButtonInfo>().text.text = this._language.restoreDefaultsButtonLabel;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x00017213 File Offset: 0x00015413
		private void Redraw(bool listsChanged, bool playTransitions)
		{
			this.RedrawPlayerGroup(playTransitions);
			this.RedrawControllerGroup();
			this.RedrawMapCategoriesGroup(playTransitions);
			this.RedrawInputGrid(listsChanged);
			if (this.currentUISelection == null || !this.currentUISelection.activeInHierarchy)
			{
				this.RestoreLastUISelection();
			}
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00017254 File Offset: 0x00015454
		private void RedrawPlayerGroup(bool playTransitions)
		{
			if (!this.showPlayers)
			{
				return;
			}
			for (int i = 0; i < this.playerButtons.Count; i++)
			{
				bool flag = this.currentPlayerId != this.playerButtons[i].buttonInfo.intData;
				this.playerButtons[i].SetInteractible(flag, playTransitions);
			}
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x000172B8 File Offset: 0x000154B8
		private void RedrawControllerGroup()
		{
			int num = -1;
			this.references.controllerNameLabel.text = this._language.none;
			UITools.SetInteractable(this.references.removeControllerButton, false, false);
			UITools.SetInteractable(this.references.assignControllerButton, false, false);
			UITools.SetInteractable(this.references.calibrateControllerButton, false, false);
			if (this.ShowAssignedControllers())
			{
				foreach (ControlMapper.GUIButton guibutton in this.assignedControllerButtons)
				{
					if (!(guibutton.gameObject == null))
					{
						if (this.currentUISelection == guibutton.gameObject)
						{
							num = guibutton.buttonInfo.intData;
						}
						global::UnityEngine.Object.Destroy(guibutton.gameObject);
					}
				}
				this.assignedControllerButtons.Clear();
				this.assignedControllerButtonsPlaceholder.SetActive(true);
			}
			Player player = ReInput.players.GetPlayer(this.currentPlayerId);
			if (player == null)
			{
				return;
			}
			if (this.ShowAssignedControllers())
			{
				if (player.controllers.joystickCount > 0)
				{
					this.assignedControllerButtonsPlaceholder.SetActive(false);
				}
				foreach (Joystick joystick in player.controllers.Joysticks)
				{
					ControlMapper.GUIButton guibutton2 = this.CreateButton(this._language.GetControllerName(joystick), this.references.assignedControllersGroup.content, Vector2.zero);
					guibutton2.SetButtonInfoData("AssignedControllerSelection", joystick.id);
					guibutton2.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
					guibutton2.buttonInfo.OnSelectedEvent += this.OnUIElementSelected;
					this.assignedControllerButtons.Add(guibutton2);
					if (joystick.id == this.currentJoystickId)
					{
						guibutton2.SetInteractible(false, true);
					}
				}
				if (player.controllers.joystickCount > 0 && !this.isJoystickSelected)
				{
					this.currentJoystickId = player.controllers.Joysticks[0].id;
					this.assignedControllerButtons[0].SetInteractible(false, false);
				}
				if (num < 0)
				{
					goto IL_02B0;
				}
				using (List<ControlMapper.GUIButton>.Enumerator enumerator = this.assignedControllerButtons.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ControlMapper.GUIButton guibutton3 = enumerator.Current;
						if (guibutton3.buttonInfo.intData == num)
						{
							this.SetUISelection(guibutton3.gameObject);
							break;
						}
					}
					goto IL_02B0;
				}
			}
			if (player.controllers.joystickCount > 0 && !this.isJoystickSelected)
			{
				this.currentJoystickId = player.controllers.Joysticks[0].id;
			}
			IL_02B0:
			if (this.isJoystickSelected && player.controllers.joystickCount > 0)
			{
				this.references.removeControllerButton.interactable = true;
				this.references.controllerNameLabel.text = this._language.GetControllerName(this.currentJoystick);
				if (this.currentJoystick.axisCount > 0)
				{
					this.references.calibrateControllerButton.interactable = true;
				}
			}
			int joystickCount = player.controllers.joystickCount;
			int joystickCount2 = ReInput.controllers.joystickCount;
			int maxControllersPerPlayer = this.GetMaxControllersPerPlayer();
			bool flag = maxControllersPerPlayer == 0;
			if (joystickCount2 > 0 && joystickCount < joystickCount2 && (maxControllersPerPlayer == 1 || flag || joystickCount < maxControllersPerPlayer))
			{
				UITools.SetInteractable(this.references.assignControllerButton, true, false);
			}
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00017654 File Offset: 0x00015854
		private void RedrawMapCategoriesGroup(bool playTransitions)
		{
			if (!this.showMapCategories)
			{
				return;
			}
			for (int i = 0; i < this.mapCategoryButtons.Count; i++)
			{
				bool flag = this.currentMapCategoryId != this.mapCategoryButtons[i].buttonInfo.intData;
				this.mapCategoryButtons[i].SetInteractible(flag, playTransitions);
			}
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x000176B5 File Offset: 0x000158B5
		private void RedrawInputGrid(bool listsChanged)
		{
			if (listsChanged)
			{
				this.RefreshInputGridStructure();
			}
			this.PopulateInputFields();
			if (listsChanged)
			{
				this.ResetInputGridScrollBar();
			}
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x000176CF File Offset: 0x000158CF
		private void ForceRefresh()
		{
			if (this.windowManager.isWindowOpen)
			{
				this.CloseAllWindows();
				return;
			}
			this.Redraw(false, false);
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x000176F0 File Offset: 0x000158F0
		private void CreateInputCategoryRow(ref int rowCount, InputCategory category)
		{
			this.CreateLabel(this._language.GetMapCategoryName(category.id), this.references.inputGridActionColumn, new Vector2(0f, (float)(rowCount * this._inputRowHeight) * -1f));
			rowCount++;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00017740 File Offset: 0x00015940
		private ControlMapper.GUILabel CreateLabel(string labelText, Transform parent, Vector2 offset)
		{
			return this.CreateLabel(this.prefabs.inputGridLabel, labelText, parent, offset);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00017758 File Offset: 0x00015958
		private ControlMapper.GUILabel CreateLabel(GameObject prefab, string labelText, Transform parent, Vector2 offset)
		{
			GameObject gameObject = this.InstantiateGUIObject(prefab, parent, offset);
			TMP_Text componentInSelfOrChildren = UnityTools.GetComponentInSelfOrChildren<TMP_Text>(gameObject);
			if (componentInSelfOrChildren == null)
			{
				Debug.LogError("Rewired Control Mapper: Label prefab is missing Text component!");
				return null;
			}
			componentInSelfOrChildren.text = labelText;
			return new ControlMapper.GUILabel(gameObject);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00017799 File Offset: 0x00015999
		private ControlMapper.GUIButton CreateButton(string labelText, Transform parent, Vector2 offset)
		{
			ControlMapper.GUIButton guibutton = new ControlMapper.GUIButton(this.InstantiateGUIObject(this.prefabs.button, parent, offset));
			guibutton.SetLabel(labelText);
			return guibutton;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x000177BA File Offset: 0x000159BA
		private ControlMapper.GUIButton CreateFitButton(string labelText, Transform parent, Vector2 offset)
		{
			ControlMapper.GUIButton guibutton = new ControlMapper.GUIButton(this.InstantiateGUIObject(this.prefabs.fitButton, parent, offset));
			guibutton.SetLabel(labelText);
			return guibutton;
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x000177DC File Offset: 0x000159DC
		private ControlMapper.GUIInputField CreateInputField(Transform parent, Vector2 offset, string label, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
		{
			ControlMapper.GUIInputField guiinputField = this.CreateInputField(parent, offset);
			guiinputField.SetLabel("");
			guiinputField.SetFieldInfoData(actionId, axisRange, controllerType, fieldIndex);
			guiinputField.SetOnClickCallback(this.inputFieldActivatedDelegate);
			guiinputField.fieldInfo.OnSelectedEvent += this.OnUIElementSelected;
			if (guiinputField.fieldInfo.glyphOrText != null)
			{
				guiinputField.fieldInfo.glyphOrText.allowedTypes = (this._showGlyphs ? ControllerElementGlyphBase.AllowedTypes.All : ControllerElementGlyphBase.AllowedTypes.Text);
			}
			return guiinputField;
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0001785E File Offset: 0x00015A5E
		private ControlMapper.GUIInputField CreateInputField(Transform parent, Vector2 offset)
		{
			return new ControlMapper.GUIInputField(this.InstantiateGUIObject(this.prefabs.inputGridFieldButton, parent, offset));
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00017878 File Offset: 0x00015A78
		private ControlMapper.GUIToggle CreateToggle(GameObject prefab, Transform parent, Vector2 offset, string label, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
		{
			ControlMapper.GUIToggle guitoggle = this.CreateToggle(prefab, parent, offset);
			guitoggle.SetToggleInfoData(actionId, axisRange, controllerType, fieldIndex);
			guitoggle.SetOnSubmitCallback(this.inputFieldInvertToggleStateChangedDelegate);
			guitoggle.toggleInfo.OnSelectedEvent += this.OnUIElementSelected;
			return guitoggle;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x000178B4 File Offset: 0x00015AB4
		private ControlMapper.GUIToggle CreateToggle(GameObject prefab, Transform parent, Vector2 offset)
		{
			return new ControlMapper.GUIToggle(this.InstantiateGUIObject(prefab, parent, offset));
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x000178C4 File Offset: 0x00015AC4
		private GameObject InstantiateGUIObject(GameObject prefab, Transform parent, Vector2 offset)
		{
			if (prefab == null)
			{
				Debug.LogError("Rewired Control Mapper: Prefab is null!");
				return null;
			}
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(prefab);
			return this.InitializeNewGUIGameObject(gameObject, parent, offset);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x000178F8 File Offset: 0x00015AF8
		private GameObject CreateNewGUIObject(string name, Transform parent, Vector2 offset)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = name;
			gameObject.AddComponent<RectTransform>();
			return this.InitializeNewGUIGameObject(gameObject, parent, offset);
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00017924 File Offset: 0x00015B24
		private GameObject InitializeNewGUIGameObject(GameObject gameObject, Transform parent, Vector2 offset)
		{
			if (gameObject == null)
			{
				Debug.LogError("Rewired Control Mapper: GameObject is null!");
				return null;
			}
			RectTransform component = gameObject.GetComponent<RectTransform>();
			if (component == null)
			{
				Debug.LogError("Rewired Control Mapper: GameObject does not have a RectTransform component!");
				return gameObject;
			}
			if (parent != null)
			{
				component.SetParent(parent, false);
			}
			component.anchoredPosition = offset;
			return gameObject;
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0001797C File Offset: 0x00015B7C
		private GameObject CreateNewColumnGroup(string name, Transform parent, int maxWidth)
		{
			GameObject gameObject = this.CreateNewGUIObject(name, parent, Vector2.zero);
			this.inputGrid.AddGroup(gameObject);
			LayoutElement layoutElement = gameObject.AddComponent<LayoutElement>();
			if (maxWidth >= 0)
			{
				layoutElement.preferredWidth = (float)maxWidth;
			}
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 0f);
			component.anchorMax = new Vector2(1f, 0f);
			return gameObject;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x000179E6 File Offset: 0x00015BE6
		private Window OpenWindow(bool closeOthers)
		{
			return this.OpenWindow(string.Empty, closeOthers);
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x000179F4 File Offset: 0x00015BF4
		private Window OpenWindow(string name, bool closeOthers)
		{
			if (closeOthers)
			{
				this.windowManager.CancelAll();
			}
			Window window = this.windowManager.OpenWindow(name, this._defaultWindowWidth, this._defaultWindowHeight);
			if (window == null)
			{
				return null;
			}
			this.ChildWindowOpened();
			return window;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x00017A3A File Offset: 0x00015C3A
		private Window OpenWindow(GameObject windowPrefab, bool closeOthers)
		{
			return this.OpenWindow(windowPrefab, string.Empty, closeOthers);
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00017A4C File Offset: 0x00015C4C
		private Window OpenWindow(GameObject windowPrefab, string name, bool closeOthers)
		{
			if (closeOthers)
			{
				this.windowManager.CancelAll();
			}
			Window window = this.windowManager.OpenWindow(windowPrefab, name);
			if (window == null)
			{
				return null;
			}
			this.ChildWindowOpened();
			return window;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00017A88 File Offset: 0x00015C88
		private void OpenModal(string title, string message, string confirmText, Action<int> confirmAction, string cancelText, Action<int> cancelAction, bool closeOthers)
		{
			Window window = this.OpenWindow(closeOthers);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, title);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), message);
			UnityAction unityAction = delegate
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, confirmText, delegate
			{
				this.OnRestoreDefaultsConfirmed(window.id);
			}, unityAction, false);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, cancelText, unityAction, unityAction, true);
			this.windowManager.Focus(window);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00017B93 File Offset: 0x00015D93
		private void CloseWindow(int windowId)
		{
			if (!this.windowManager.isWindowOpen)
			{
				return;
			}
			this.windowManager.CloseWindow(windowId);
			this.ChildWindowClosed();
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00017BB5 File Offset: 0x00015DB5
		private void CloseTopWindow()
		{
			if (!this.windowManager.isWindowOpen)
			{
				return;
			}
			this.windowManager.CloseTop();
			this.ChildWindowClosed();
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00017BD6 File Offset: 0x00015DD6
		private void CloseAllWindows()
		{
			if (!this.windowManager.isWindowOpen)
			{
				return;
			}
			this.windowManager.CancelAll();
			this.ChildWindowClosed();
			this.InputPollingStopped();
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00017BFD File Offset: 0x00015DFD
		private void ChildWindowOpened()
		{
			if (!this.windowManager.isWindowOpen)
			{
				return;
			}
			this.SetIsFocused(false);
			if (this._PopupWindowOpenedEvent != null)
			{
				this._PopupWindowOpenedEvent();
			}
			if (this._onPopupWindowOpened != null)
			{
				this._onPopupWindowOpened.Invoke();
			}
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00017C3A File Offset: 0x00015E3A
		private void ChildWindowClosed()
		{
			if (this.windowManager.isWindowOpen)
			{
				return;
			}
			this.SetIsFocused(true);
			if (this._PopupWindowClosedEvent != null)
			{
				this._PopupWindowClosedEvent();
			}
			if (this._onPopupWindowClosed != null)
			{
				this._onPopupWindowClosed.Invoke();
			}
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00017C78 File Offset: 0x00015E78
		private bool HasElementAssignmentConflicts(Player player, ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers)
		{
			if (player == null || mapping == null)
			{
				return false;
			}
			ElementAssignmentConflictCheck elementAssignmentConflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out elementAssignmentConflictCheck))
			{
				return false;
			}
			if (skipOtherPlayers)
			{
				return ReInput.players.SystemPlayer.controllers.conflictChecking.DoesElementAssignmentConflict(elementAssignmentConflictCheck) || player.controllers.conflictChecking.DoesElementAssignmentConflict(elementAssignmentConflictCheck);
			}
			return ReInput.controllers.conflictChecking.DoesElementAssignmentConflict(elementAssignmentConflictCheck);
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00017CE4 File Offset: 0x00015EE4
		private bool IsBlockingAssignmentConflict(ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers)
		{
			ElementAssignmentConflictCheck elementAssignmentConflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out elementAssignmentConflictCheck))
			{
				return false;
			}
			if (skipOtherPlayers)
			{
				foreach (ElementAssignmentConflictInfo elementAssignmentConflictInfo in ReInput.players.SystemPlayer.controllers.conflictChecking.ElementAssignmentConflicts(elementAssignmentConflictCheck))
				{
					if (!elementAssignmentConflictInfo.isUserAssignable)
					{
						return true;
					}
				}
				using (IEnumerator<ElementAssignmentConflictInfo> enumerator = this.currentPlayer.controllers.conflictChecking.ElementAssignmentConflicts(elementAssignmentConflictCheck).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ElementAssignmentConflictInfo elementAssignmentConflictInfo2 = enumerator.Current;
						if (!elementAssignmentConflictInfo2.isUserAssignable)
						{
							return true;
						}
					}
					return false;
				}
			}
			foreach (ElementAssignmentConflictInfo elementAssignmentConflictInfo3 in ReInput.controllers.conflictChecking.ElementAssignmentConflicts(elementAssignmentConflictCheck))
			{
				if (!elementAssignmentConflictInfo3.isUserAssignable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00017E08 File Offset: 0x00016008
		private IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(Player player, ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers)
		{
			if (player == null || mapping == null)
			{
				yield break;
			}
			ElementAssignmentConflictCheck conflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out conflictCheck))
			{
				yield break;
			}
			if (skipOtherPlayers)
			{
				foreach (ElementAssignmentConflictInfo elementAssignmentConflictInfo in ReInput.players.SystemPlayer.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
				{
					if (!elementAssignmentConflictInfo.isUserAssignable)
					{
						yield return elementAssignmentConflictInfo;
					}
				}
				IEnumerator<ElementAssignmentConflictInfo> enumerator = null;
				foreach (ElementAssignmentConflictInfo elementAssignmentConflictInfo2 in player.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
				{
					if (!elementAssignmentConflictInfo2.isUserAssignable)
					{
						yield return elementAssignmentConflictInfo2;
					}
				}
				enumerator = null;
			}
			else
			{
				foreach (ElementAssignmentConflictInfo elementAssignmentConflictInfo3 in ReInput.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
				{
					if (!elementAssignmentConflictInfo3.isUserAssignable)
					{
						yield return elementAssignmentConflictInfo3;
					}
				}
				IEnumerator<ElementAssignmentConflictInfo> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00017E38 File Offset: 0x00016038
		private bool CreateConflictCheck(ControlMapper.InputMapping mapping, ElementAssignment assignment, out ElementAssignmentConflictCheck conflictCheck)
		{
			if (mapping == null || this.currentPlayer == null)
			{
				conflictCheck = default(ElementAssignmentConflictCheck);
				return false;
			}
			conflictCheck = assignment.ToElementAssignmentConflictCheck();
			conflictCheck.playerId = this.currentPlayer.id;
			conflictCheck.controllerType = mapping.controllerType;
			conflictCheck.controllerId = mapping.controllerId;
			conflictCheck.controllerMapId = mapping.map.id;
			conflictCheck.controllerMapCategoryId = mapping.map.categoryId;
			if (mapping.aem != null)
			{
				conflictCheck.elementMapId = mapping.aem.id;
			}
			return true;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00017ECC File Offset: 0x000160CC
		private void PollKeyboardForAssignment(out ControllerPollingInfo pollingInfo, out bool modifierKeyPressed, out ModifierKeyFlags modifierFlags, out string label)
		{
			pollingInfo = default(ControllerPollingInfo);
			label = string.Empty;
			modifierKeyPressed = false;
			modifierFlags = ModifierKeyFlags.None;
			int num = 0;
			ControllerPollingInfo controllerPollingInfo = default(ControllerPollingInfo);
			ControllerPollingInfo controllerPollingInfo2 = default(ControllerPollingInfo);
			ModifierKeyFlags modifierKeyFlags = ModifierKeyFlags.None;
			foreach (ControllerPollingInfo controllerPollingInfo3 in ReInput.controllers.Keyboard.PollForAllKeys())
			{
				KeyCode keyboardKey = controllerPollingInfo3.keyboardKey;
				if (keyboardKey != KeyCode.AltGr)
				{
					if (Keyboard.IsModifierKey(controllerPollingInfo3.keyboardKey))
					{
						if (num == 0)
						{
							controllerPollingInfo2 = controllerPollingInfo3;
						}
						modifierKeyFlags |= Keyboard.KeyCodeToModifierKeyFlags(keyboardKey);
						num++;
					}
					else if (controllerPollingInfo.keyboardKey == KeyCode.None)
					{
						controllerPollingInfo = controllerPollingInfo3;
					}
				}
			}
			if (controllerPollingInfo.keyboardKey == KeyCode.None)
			{
				if (num > 0)
				{
					modifierKeyPressed = true;
					if (num == 1)
					{
						if (ReInput.controllers.Keyboard.GetKeyTimePressed(controllerPollingInfo2.keyboardKey) > 1.0)
						{
							pollingInfo = controllerPollingInfo2;
							return;
						}
						label = Keyboard.GetKeyName(controllerPollingInfo2.keyboardKey);
						return;
					}
					else
					{
						label = this._language.ModifierKeyFlagsToString(modifierKeyFlags);
					}
				}
				return;
			}
			if (!ReInput.controllers.Keyboard.GetKeyDown(controllerPollingInfo.keyboardKey))
			{
				return;
			}
			if (num == 0)
			{
				pollingInfo = controllerPollingInfo;
				return;
			}
			pollingInfo = controllerPollingInfo;
			modifierFlags = modifierKeyFlags;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00018018 File Offset: 0x00016218
		private bool GetFirstElementAssignmentConflict(ElementAssignmentConflictCheck conflictCheck, out ElementAssignmentConflictInfo conflict, bool skipOtherPlayers)
		{
			if (this.GetFirstElementAssignmentConflict(this.currentPlayer, conflictCheck, out conflict))
			{
				return true;
			}
			if (this.GetFirstElementAssignmentConflict(ReInput.players.SystemPlayer, conflictCheck, out conflict))
			{
				return true;
			}
			if (!skipOtherPlayers)
			{
				IList<Player> players = ReInput.players.Players;
				for (int i = 0; i < players.Count; i++)
				{
					Player player = players[i];
					if (player != this.currentPlayer && this.GetFirstElementAssignmentConflict(player, conflictCheck, out conflict))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0001808C File Offset: 0x0001628C
		private bool GetFirstElementAssignmentConflict(Player player, ElementAssignmentConflictCheck conflictCheck, out ElementAssignmentConflictInfo conflict)
		{
			using (IEnumerator<ElementAssignmentConflictInfo> enumerator = player.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					ElementAssignmentConflictInfo elementAssignmentConflictInfo = enumerator.Current;
					conflict = elementAssignmentConflictInfo;
					return true;
				}
			}
			conflict = default(ElementAssignmentConflictInfo);
			return false;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x000180F4 File Offset: 0x000162F4
		private void StartAxisCalibration(int axisIndex)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (this.currentPlayer.controllers.joystickCount == 0)
			{
				return;
			}
			Joystick currentJoystick = this.currentJoystick;
			if (axisIndex < 0 || axisIndex >= currentJoystick.axisCount)
			{
				return;
			}
			this.pendingAxisCalibration = new ControlMapper.AxisCalibrator(currentJoystick, axisIndex);
			this.ShowCalibrateAxisStep1Window();
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00018145 File Offset: 0x00016345
		private void EndAxisCalibration()
		{
			if (this.pendingAxisCalibration == null)
			{
				return;
			}
			this.pendingAxisCalibration.Commit();
			this.pendingAxisCalibration = null;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x00018162 File Offset: 0x00016362
		private void SetUISelection(GameObject selection)
		{
			if (EventSystem.current == null)
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(selection);
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0001817D File Offset: 0x0001637D
		private void RestoreLastUISelection()
		{
			if (this.lastUISelection == null || !this.lastUISelection.activeInHierarchy)
			{
				this.SetDefaultUISelection();
				return;
			}
			this.SetUISelection(this.lastUISelection);
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x000181AD File Offset: 0x000163AD
		private void SetDefaultUISelection()
		{
			if (!this.isOpen)
			{
				return;
			}
			if (this.references.defaultSelection == null)
			{
				this.SetUISelection(null);
				return;
			}
			this.SetUISelection(this.references.defaultSelection.gameObject);
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x000181EC File Offset: 0x000163EC
		private void SelectDefaultMapCategory(bool redraw)
		{
			this.currentMapCategoryId = this.GetDefaultMapCategoryId();
			this.OnMapCategorySelected(this.currentMapCategoryId, redraw);
			if (!this.showMapCategories)
			{
				return;
			}
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				if (ReInput.mapping.GetMapCategory(this._mappingSets[i].mapCategoryId) != null)
				{
					this.currentMapCategoryId = this._mappingSets[i].mapCategoryId;
					break;
				}
			}
			if (this.currentMapCategoryId < 0)
			{
				return;
			}
			for (int j = 0; j < this._mappingSets.Length; j++)
			{
				bool flag = this._mappingSets[j].mapCategoryId != this.currentMapCategoryId;
				this.mapCategoryButtons[j].SetInteractible(flag, false);
			}
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x000182A6 File Offset: 0x000164A6
		private void CheckUISelection()
		{
			if (!this.isFocused)
			{
				return;
			}
			if (this.currentUISelection == null)
			{
				this.RestoreLastUISelection();
			}
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x000182C5 File Offset: 0x000164C5
		private void OnUIElementSelected(GameObject selectedObject)
		{
			this.lastUISelection = selectedObject;
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x000182CE File Offset: 0x000164CE
		private void SetIsFocused(bool state)
		{
			this.references.mainCanvasGroup.interactable = state;
			if (state)
			{
				this.Redraw(false, false);
				this.RestoreLastUISelection();
				this.blockInputOnFocusEndTime = Time.unscaledTime + 0.1f;
			}
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00018303 File Offset: 0x00016503
		public void Toggle()
		{
			if (this.isOpen)
			{
				this.Close(true);
				return;
			}
			this.Open();
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0001831B File Offset: 0x0001651B
		public void Open()
		{
			this.Open(false);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00018324 File Offset: 0x00016524
		private void Open(bool force)
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			if (!this.initialized)
			{
				return;
			}
			if (!force && this.isOpen)
			{
				return;
			}
			this.Clear();
			if (ControlMapper.Instance == null)
			{
				ControlMapper.Instance = this;
			}
			this.canvas.SetActive(true);
			this.OnPlayerSelected(0, false);
			this.SelectDefaultMapCategory(false);
			this.SetDefaultUISelection();
			this.Redraw(true, false);
			if (this._ScreenOpenedEvent != null)
			{
				this._ScreenOpenedEvent();
			}
			if (this._onScreenOpened != null)
			{
				this._onScreenOpened.Invoke();
			}
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x000183BC File Offset: 0x000165BC
		public void Close(bool save)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (save && ReInput.userDataStore != null)
			{
				ReInput.userDataStore.Save();
			}
			this.Clear();
			this.canvas.SetActive(false);
			this.SetUISelection(null);
			if (ControlMapper.Instance == this)
			{
				ControlMapper.Instance = null;
			}
			if (this._ScreenClosedEvent != null)
			{
				this._ScreenClosedEvent();
			}
			if (this._onScreenClosed != null)
			{
				this._onScreenClosed.Invoke();
			}
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x00018441 File Offset: 0x00016641
		private void Clear()
		{
			this.CloseAllWindows();
			this.lastUISelection = null;
			this.pendingInputMapping = null;
			this.pendingAxisCalibration = null;
			this.InputPollingStopped();
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00018464 File Offset: 0x00016664
		private void ClearCompletely()
		{
			this.Clear();
			this.ClearSpawnedObjects();
			this.ClearAllVars();
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00018478 File Offset: 0x00016678
		private void ClearSpawnedObjects()
		{
			this.windowManager.ClearCompletely();
			this.inputGrid.ClearAll();
			foreach (ControlMapper.GUIButton guibutton in this.playerButtons)
			{
				global::UnityEngine.Object.Destroy(guibutton.gameObject);
			}
			this.playerButtons.Clear();
			foreach (ControlMapper.GUIButton guibutton2 in this.mapCategoryButtons)
			{
				global::UnityEngine.Object.Destroy(guibutton2.gameObject);
			}
			this.mapCategoryButtons.Clear();
			foreach (ControlMapper.GUIButton guibutton3 in this.assignedControllerButtons)
			{
				global::UnityEngine.Object.Destroy(guibutton3.gameObject);
			}
			this.assignedControllerButtons.Clear();
			if (this.assignedControllerButtonsPlaceholder != null)
			{
				global::UnityEngine.Object.Destroy(this.assignedControllerButtonsPlaceholder.gameObject);
				this.assignedControllerButtonsPlaceholder = null;
			}
			foreach (GameObject gameObject in this.miscInstantiatedObjects)
			{
				global::UnityEngine.Object.Destroy(gameObject);
			}
			this.miscInstantiatedObjects.Clear();
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x000185F8 File Offset: 0x000167F8
		private void ClearVarsOnPlayerChange()
		{
			this.currentJoystickId = -1;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x000185F8 File Offset: 0x000167F8
		private void ClearVarsOnJoystickChange()
		{
			this.currentJoystickId = -1;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x00018604 File Offset: 0x00016804
		private void ClearAllVars()
		{
			this.initialized = false;
			if (ControlMapper.Instance == this)
			{
				ControlMapper.Instance = null;
			}
			this.playerCount = 0;
			this.inputGrid = null;
			this.windowManager = null;
			this.currentPlayerId = -1;
			this.currentMapCategoryId = -1;
			this.playerButtons = null;
			this.mapCategoryButtons = null;
			this.miscInstantiatedObjects = null;
			this.canvas = null;
			this.lastUISelection = null;
			this.currentJoystickId = -1;
			this.pendingInputMapping = null;
			this.pendingAxisCalibration = null;
			this.inputFieldActivatedDelegate = null;
			this.inputFieldInvertToggleStateChangedDelegate = null;
			this.isPollingForInput = false;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0001869B File Offset: 0x0001689B
		public void Reset()
		{
			if (!this.initialized)
			{
				return;
			}
			bool isOpen = this.isOpen;
			this.Close(false);
			this.ClearCompletely();
			this.Initialize();
			if (isOpen)
			{
				this.Open(true);
			}
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x000186C8 File Offset: 0x000168C8
		private void SetActionAxisInverted(bool state, ControllerType controllerType, int actionElementMapId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			ControllerMapWithAxes controllerMapWithAxes = this.GetControllerMap(controllerType) as ControllerMapWithAxes;
			if (controllerMapWithAxes == null)
			{
				return;
			}
			ActionElementMap elementMap = controllerMapWithAxes.GetElementMap(actionElementMapId);
			if (elementMap == null)
			{
				return;
			}
			elementMap.invert = state;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x00018704 File Offset: 0x00016904
		private ControllerMap GetControllerMap(ControllerType type)
		{
			if (this.currentPlayer == null)
			{
				return null;
			}
			int num = 0;
			switch (type)
			{
			case ControllerType.Keyboard:
			case ControllerType.Mouse:
				break;
			case ControllerType.Joystick:
				if (this.currentPlayer.controllers.joystickCount <= 0)
				{
					return null;
				}
				num = this.currentJoystick.id;
				break;
			default:
				throw new NotImplementedException();
			}
			return this.currentPlayer.controllers.maps.GetFirstMapInCategory(type, num, this.currentMapCategoryId);
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00018778 File Offset: 0x00016978
		private ControllerMap GetControllerMapOrCreateNew(ControllerType controllerType, int controllerId, int layoutId)
		{
			ControllerMap controllerMap = this.GetControllerMap(controllerType);
			if (controllerMap == null)
			{
				this.currentPlayer.controllers.maps.AddEmptyMap(controllerType, controllerId, this.currentMapCategoryId, layoutId);
				controllerMap = this.currentPlayer.controllers.maps.GetMap(controllerType, controllerId, this.currentMapCategoryId, layoutId);
			}
			return controllerMap;
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x000187D0 File Offset: 0x000169D0
		private int CountIEnumerable<T>(IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				return 0;
			}
			IEnumerator<T> enumerator = enumerable.GetEnumerator();
			if (enumerator == null)
			{
				return 0;
			}
			int num = 0;
			while (enumerator.MoveNext())
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x00018800 File Offset: 0x00016A00
		private int GetDefaultMapCategoryId()
		{
			if (this._mappingSets.Length == 0)
			{
				return 0;
			}
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				if (ReInput.mapping.GetMapCategory(this._mappingSets[i].mapCategoryId) != null)
				{
					return this._mappingSets[i].mapCategoryId;
				}
			}
			return 0;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00018854 File Offset: 0x00016A54
		private void SubscribeFixedUISelectionEvents()
		{
			if (this.references.fixedSelectableUIElements == null)
			{
				return;
			}
			GameObject[] fixedSelectableUIElements = this.references.fixedSelectableUIElements;
			for (int i = 0; i < fixedSelectableUIElements.Length; i++)
			{
				UIElementInfo component = UnityTools.GetComponent<UIElementInfo>(fixedSelectableUIElements[i]);
				if (!(component == null))
				{
					component.OnSelectedEvent += this.OnUIElementSelected;
				}
			}
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x000188B0 File Offset: 0x00016AB0
		private void SubscribeMenuControlInputEvents()
		{
			this.SubscribeRewiredInputEventAllPlayers(this._screenToggleAction, new Action<InputActionEventData>(this.OnScreenToggleActionPressed));
			this.SubscribeRewiredInputEventAllPlayers(this._screenOpenAction, new Action<InputActionEventData>(this.OnScreenOpenActionPressed));
			this.SubscribeRewiredInputEventAllPlayers(this._screenCloseAction, new Action<InputActionEventData>(this.OnScreenCloseActionPressed));
			this.SubscribeRewiredInputEventAllPlayers(this._universalCancelAction, new Action<InputActionEventData>(this.OnUniversalCancelActionPressed));
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00018920 File Offset: 0x00016B20
		private void UnsubscribeMenuControlInputEvents()
		{
			this.UnsubscribeRewiredInputEventAllPlayers(this._screenToggleAction, new Action<InputActionEventData>(this.OnScreenToggleActionPressed));
			this.UnsubscribeRewiredInputEventAllPlayers(this._screenOpenAction, new Action<InputActionEventData>(this.OnScreenOpenActionPressed));
			this.UnsubscribeRewiredInputEventAllPlayers(this._screenCloseAction, new Action<InputActionEventData>(this.OnScreenCloseActionPressed));
			this.UnsubscribeRewiredInputEventAllPlayers(this._universalCancelAction, new Action<InputActionEventData>(this.OnUniversalCancelActionPressed));
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x00018990 File Offset: 0x00016B90
		private void SubscribeRewiredInputEventAllPlayers(int actionId, Action<InputActionEventData> callback)
		{
			if (actionId < 0 || callback == null)
			{
				return;
			}
			if (ReInput.mapping.GetAction(actionId) == null)
			{
				Debug.LogWarning("Rewired Control Mapper: " + actionId.ToString() + " is not a valid Action id!");
				return;
			}
			foreach (Player player in ReInput.players.AllPlayers)
			{
				player.AddInputEventDelegate(callback, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, actionId);
			}
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x00018A14 File Offset: 0x00016C14
		private void UnsubscribeRewiredInputEventAllPlayers(int actionId, Action<InputActionEventData> callback)
		{
			if (actionId < 0 || callback == null)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			if (ReInput.mapping.GetAction(actionId) == null)
			{
				Debug.LogWarning("Rewired Control Mapper: " + actionId.ToString() + " is not a valid Action id!");
				return;
			}
			foreach (Player player in ReInput.players.AllPlayers)
			{
				player.RemoveInputEventDelegate(callback, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, actionId);
			}
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x00018AA0 File Offset: 0x00016CA0
		private int GetMaxControllersPerPlayer()
		{
			if (this._rewiredInputManager.userData.ConfigVars.autoAssignJoysticks)
			{
				return this._rewiredInputManager.userData.ConfigVars.maxJoysticksPerPlayer;
			}
			return this._maxControllersPerPlayer;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x00018AD5 File Offset: 0x00016CD5
		private bool ShowAssignedControllers()
		{
			return this._showControllers && (this._showAssignedControllers || this.GetMaxControllersPerPlayer() != 1);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x00018AF7 File Offset: 0x00016CF7
		private void InspectorPropertyChanged(bool reset = false)
		{
			if (reset)
			{
				this.Reset();
			}
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x00018B04 File Offset: 0x00016D04
		private void AssignController(Player player, int controllerId)
		{
			if (player == null)
			{
				return;
			}
			if (player.controllers.ContainsController(ControllerType.Joystick, controllerId))
			{
				return;
			}
			if (this.GetMaxControllersPerPlayer() == 1)
			{
				this.RemoveAllControllers(player);
				this.ClearVarsOnJoystickChange();
			}
			foreach (Player player2 in ReInput.players.Players)
			{
				if (player2 != player)
				{
					this.RemoveController(player2, controllerId);
				}
			}
			player.controllers.AddController(ControllerType.Joystick, controllerId, false);
			if (ReInput.userDataStore != null)
			{
				ReInput.userDataStore.LoadControllerData(player.id, ControllerType.Joystick, controllerId);
			}
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x00018BAC File Offset: 0x00016DAC
		private void RemoveAllControllers(Player player)
		{
			if (player == null)
			{
				return;
			}
			IList<Joystick> joysticks = player.controllers.Joysticks;
			for (int i = joysticks.Count - 1; i >= 0; i--)
			{
				this.RemoveController(player, joysticks[i].id);
			}
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x00018BEF File Offset: 0x00016DEF
		private void RemoveController(Player player, int controllerId)
		{
			if (player == null)
			{
				return;
			}
			if (!player.controllers.ContainsController(ControllerType.Joystick, controllerId))
			{
				return;
			}
			if (ReInput.userDataStore != null)
			{
				ReInput.userDataStore.SaveControllerData(player.id, ControllerType.Joystick, controllerId);
			}
			player.controllers.RemoveController(ControllerType.Joystick, controllerId);
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x00018C2B File Offset: 0x00016E2B
		private bool IsAllowedAssignment(ControlMapper.InputMapping pendingInputMapping, ControllerPollingInfo pollingInfo)
		{
			return pendingInputMapping != null && (pendingInputMapping.axisRange != AxisRange.Full || this._showSplitAxisInputFields || pollingInfo.elementType != ControllerElementType.Button);
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00018C4F File Offset: 0x00016E4F
		private void InputPollingStarted()
		{
			bool flag = this.isPollingForInput;
			this.isPollingForInput = true;
			if (!flag)
			{
				if (this._InputPollingStartedEvent != null)
				{
					this._InputPollingStartedEvent();
				}
				if (this._onInputPollingStarted != null)
				{
					this._onInputPollingStarted.Invoke();
				}
			}
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x00018C86 File Offset: 0x00016E86
		private void InputPollingStopped()
		{
			bool flag = this.isPollingForInput;
			this.isPollingForInput = false;
			if (flag)
			{
				if (this._InputPollingEndedEvent != null)
				{
					this._InputPollingEndedEvent();
				}
				if (this._onInputPollingEnded != null)
				{
					this._onInputPollingEnded.Invoke();
				}
			}
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00018CBD File Offset: 0x00016EBD
		private int GetControllerInputFieldCount(ControllerType controllerType)
		{
			switch (controllerType)
			{
			case ControllerType.Keyboard:
				return this._keyboardInputFieldCount;
			case ControllerType.Mouse:
				return this._mouseInputFieldCount;
			case ControllerType.Joystick:
				return this._controllerInputFieldCount;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x00018CF0 File Offset: 0x00016EF0
		private bool ShowSwapButton(int windowId, ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers)
		{
			if (this.currentPlayer == null)
			{
				return false;
			}
			if (!this._allowElementAssignmentSwap)
			{
				return false;
			}
			if (mapping == null || mapping.aem == null)
			{
				return false;
			}
			ElementAssignmentConflictCheck elementAssignmentConflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out elementAssignmentConflictCheck))
			{
				Debug.LogError("Rewired Control Mapper: Error creating conflict check!");
				return false;
			}
			List<ElementAssignmentConflictInfo> list = new List<ElementAssignmentConflictInfo>();
			list.AddRange(this.currentPlayer.controllers.conflictChecking.ElementAssignmentConflicts(elementAssignmentConflictCheck));
			list.AddRange(ReInput.players.SystemPlayer.controllers.conflictChecking.ElementAssignmentConflicts(elementAssignmentConflictCheck));
			if (list.Count == 0)
			{
				return false;
			}
			ActionElementMap aem2 = mapping.aem;
			ElementAssignmentConflictInfo elementAssignmentConflictInfo = list[0];
			int actionId = elementAssignmentConflictInfo.elementMap.actionId;
			Pole axisContribution = elementAssignmentConflictInfo.elementMap.axisContribution;
			AxisRange axisRange = aem2.axisRange;
			ControllerElementType elementType = aem2.elementType;
			if (elementType == elementAssignmentConflictInfo.elementMap.elementType && elementType == ControllerElementType.Axis)
			{
				if (axisRange != elementAssignmentConflictInfo.elementMap.axisRange)
				{
					if (axisRange == AxisRange.Full)
					{
						axisRange = AxisRange.Positive;
					}
					else if (elementAssignmentConflictInfo.elementMap.axisRange == AxisRange.Full)
					{
					}
				}
			}
			else if (elementType == ControllerElementType.Axis && (elementAssignmentConflictInfo.elementMap.elementType == ControllerElementType.Button || (elementAssignmentConflictInfo.elementMap.elementType == ControllerElementType.Axis && elementAssignmentConflictInfo.elementMap.axisRange != AxisRange.Full)) && axisRange == AxisRange.Full)
			{
				axisRange = AxisRange.Positive;
			}
			int num = 0;
			if (assignment.actionId == elementAssignmentConflictInfo.actionId && mapping.map == elementAssignmentConflictInfo.controllerMap)
			{
				Controller controller = ReInput.controllers.GetController(mapping.controllerType, mapping.controllerId);
				if (this.SwapIsSameInputRange(elementType, axisRange, axisContribution, controller.GetElementById(assignment.elementIdentifierId).type, assignment.axisRange, assignment.axisContribution))
				{
					num++;
				}
			}
			using (IEnumerator<ActionElementMap> enumerator = elementAssignmentConflictInfo.controllerMap.ElementMapsWithAction(actionId).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActionElementMap aem = enumerator.Current;
					if (aem.id != aem2.id && list.FindIndex((ElementAssignmentConflictInfo x) => x.elementMapId == aem.id) < 0 && this.SwapIsSameInputRange(elementType, axisRange, axisContribution, aem.elementType, aem.axisRange, aem.axisContribution))
					{
						num++;
					}
				}
			}
			return num < this.GetControllerInputFieldCount(mapping.controllerType);
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00018F60 File Offset: 0x00017160
		private bool SwapIsSameInputRange(ControllerElementType origElementType, AxisRange origAxisRange, Pole origAxisContribution, ControllerElementType conflictElementType, AxisRange conflictAxisRange, Pole conflictAxisContribution)
		{
			return ((origElementType == ControllerElementType.Button || (origElementType == ControllerElementType.Axis && origAxisRange != AxisRange.Full)) && (conflictElementType == ControllerElementType.Button || (conflictElementType == ControllerElementType.Axis && conflictAxisRange != AxisRange.Full)) && conflictAxisContribution == origAxisContribution) || (origElementType == ControllerElementType.Axis && origAxisRange == AxisRange.Full && conflictElementType == ControllerElementType.Axis && conflictAxisRange == AxisRange.Full);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00018F94 File Offset: 0x00017194
		private void ApplyTheme()
		{
			for (int i = 0; i < this.themedElements.Count; i++)
			{
				if (this.themedElements != null)
				{
					this.themedElements[i].ApplyTheme();
				}
			}
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00018FD0 File Offset: 0x000171D0
		public static void Register(ThemedElement themedElement)
		{
			if (themedElement == null)
			{
				return;
			}
			if (ControlMapper.Instance == null)
			{
				return;
			}
			if (ControlMapper.Instance.themedElements.Contains(themedElement))
			{
				return;
			}
			ControlMapper.Instance.themedElements.Add(themedElement);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0001900D File Offset: 0x0001720D
		public static void Unregister(ThemedElement themedElement)
		{
			if (themedElement == null)
			{
				return;
			}
			if (ControlMapper.Instance == null)
			{
				return;
			}
			ControlMapper.Instance.themedElements.Remove(themedElement);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00019038 File Offset: 0x00017238
		public static void ApplyTheme(ThemedElement.ElementInfo[] elementInfo)
		{
			if (ControlMapper.Instance == null)
			{
				return;
			}
			if (ControlMapper.Instance._themeSettings == null)
			{
				return;
			}
			if (!ControlMapper.Instance._useThemeSettings)
			{
				return;
			}
			ControlMapper.Instance._themeSettings.Apply(elementInfo);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00019078 File Offset: 0x00017278
		public static LanguageDataBase GetLanguage()
		{
			if (ControlMapper.Instance == null)
			{
				return null;
			}
			return ControlMapper.Instance._language;
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x00019093 File Offset: 0x00017293
		public static ControlMapper current
		{
			get
			{
				return ControlMapper.Instance;
			}
		}

		// Token: 0x040003B3 RID: 947
		public const int versionMajor = 1;

		// Token: 0x040003B4 RID: 948
		public const int versionMinor = 1;

		// Token: 0x040003B5 RID: 949
		public const bool usesTMPro = true;

		// Token: 0x040003B6 RID: 950
		private const float blockInputOnFocusTimeout = 0.1f;

		// Token: 0x040003B7 RID: 951
		private const string buttonIdentifier_playerSelection = "PlayerSelection";

		// Token: 0x040003B8 RID: 952
		private const string buttonIdentifier_removeController = "RemoveController";

		// Token: 0x040003B9 RID: 953
		private const string buttonIdentifier_assignController = "AssignController";

		// Token: 0x040003BA RID: 954
		private const string buttonIdentifier_calibrateController = "CalibrateController";

		// Token: 0x040003BB RID: 955
		private const string buttonIdentifier_editInputBehaviors = "EditInputBehaviors";

		// Token: 0x040003BC RID: 956
		private const string buttonIdentifier_mapCategorySelection = "MapCategorySelection";

		// Token: 0x040003BD RID: 957
		private const string buttonIdentifier_assignedControllerSelection = "AssignedControllerSelection";

		// Token: 0x040003BE RID: 958
		private const string buttonIdentifier_done = "Done";

		// Token: 0x040003BF RID: 959
		private const string buttonIdentifier_restoreDefaults = "RestoreDefaults";

		// Token: 0x040003C0 RID: 960
		[SerializeField]
		[Tooltip("Must be assigned a Rewired Input Manager scene object or prefab.")]
		private InputManager _rewiredInputManager;

		// Token: 0x040003C1 RID: 961
		[SerializeField]
		[Tooltip("Set to True to prevent the Game Object from being destroyed when a new scene is loaded.\n\nNOTE: Changing this value from True to False at runtime will have no effect because Object.DontDestroyOnLoad cannot be undone once set.")]
		private bool _dontDestroyOnLoad;

		// Token: 0x040003C2 RID: 962
		[SerializeField]
		[Tooltip("Open the control mapping screen immediately on start. Mainly used for testing.")]
		private bool _openOnStart;

		// Token: 0x040003C3 RID: 963
		[SerializeField]
		[Tooltip("The Layout of the Keyboard Maps to be displayed.")]
		private int _keyboardMapDefaultLayout;

		// Token: 0x040003C4 RID: 964
		[SerializeField]
		[Tooltip("The Layout of the Mouse Maps to be displayed.")]
		private int _mouseMapDefaultLayout;

		// Token: 0x040003C5 RID: 965
		[SerializeField]
		[Tooltip("The Layout of the Mouse Maps to be displayed.")]
		private int _joystickMapDefaultLayout;

		// Token: 0x040003C6 RID: 966
		[SerializeField]
		private ControlMapper.MappingSet[] _mappingSets = new ControlMapper.MappingSet[] { ControlMapper.MappingSet.Default };

		// Token: 0x040003C7 RID: 967
		[SerializeField]
		[Tooltip("Display a selectable list of Players. If your game only supports 1 player, you can disable this.")]
		private bool _showPlayers = true;

		// Token: 0x040003C8 RID: 968
		[SerializeField]
		[Tooltip("Display the Controller column for input mapping.")]
		private bool _showControllers = true;

		// Token: 0x040003C9 RID: 969
		[SerializeField]
		[Tooltip("Display the Keyboard column for input mapping.")]
		private bool _showKeyboard = true;

		// Token: 0x040003CA RID: 970
		[SerializeField]
		[Tooltip("Display the Mouse column for input mapping.")]
		private bool _showMouse = true;

		// Token: 0x040003CB RID: 971
		[SerializeField]
		[Tooltip("The maximum number of controllers allowed to be assigned to a Player. If set to any value other than 1, a selectable list of currently-assigned controller will be displayed to the user. [0 = infinite]")]
		private int _maxControllersPerPlayer = 1;

		// Token: 0x040003CC RID: 972
		[SerializeField]
		[Tooltip("Display section labels for each Action Category in the input field grid. Only applies if Action Categories are used to display the Action list.")]
		private bool _showActionCategoryLabels;

		// Token: 0x040003CD RID: 973
		[SerializeField]
		[Tooltip("The number of input fields to display for the keyboard. If you want to support alternate mappings on the same device, set this to 2 or more.")]
		private int _keyboardInputFieldCount = 2;

		// Token: 0x040003CE RID: 974
		[SerializeField]
		[Tooltip("The number of input fields to display for the mouse. If you want to support alternate mappings on the same device, set this to 2 or more.")]
		private int _mouseInputFieldCount = 1;

		// Token: 0x040003CF RID: 975
		[SerializeField]
		[Tooltip("The number of input fields to display for joysticks. If you want to support alternate mappings on the same device, set this to 2 or more.")]
		private int _controllerInputFieldCount = 1;

		// Token: 0x040003D0 RID: 976
		[SerializeField]
		[Tooltip("Display a full-axis input assignment field for every axis-type Action in the input field grid. Also displays an invert toggle for the user  to invert the full-axis assignment direction.\n\n*IMPORTANT*: This field is required if you have made any full-axis assignments in the Rewired Input Manager or in saved XML user data. Disabling this field when you have full-axis assignments will result in the inability for the user to view, remove, or modify these full-axis assignments. In addition, these assignments may cause conflicts when trying to remap the same axes to Actions.")]
		private bool _showFullAxisInputFields = true;

		// Token: 0x040003D1 RID: 977
		[SerializeField]
		[Tooltip("Display a positive and negative input assignment field for every axis-type Action in the input field grid.\n\n*IMPORTANT*: These fields are required to assign buttons, keyboard keys, and hat or D-Pad directions to axis-type Actions. If you have made any split-axis assignments or button/key/D-pad assignments to axis-type Actions in the Rewired Input Manager or in saved XML user data, disabling these fields will result in the inability for the user to view, remove, or modify these assignments. In addition, these assignments may cause conflicts when trying to remap the same elements to Actions.")]
		private bool _showSplitAxisInputFields = true;

		// Token: 0x040003D2 RID: 978
		[SerializeField]
		[Tooltip("Show glyphs if available. Glyph Provider must be configured for glyphs to be displayed. See Glyphs documentation for more information.")]
		private bool _showGlyphs = true;

		// Token: 0x040003D3 RID: 979
		[SerializeField]
		[Tooltip("If enabled, when an element assignment conflict is found, an option will be displayed that allows the user to make the conflicting assignment anyway.")]
		private bool _allowElementAssignmentConflicts;

		// Token: 0x040003D4 RID: 980
		[SerializeField]
		[Tooltip("If enabled, when an element assignment conflict is found, an option will be displayed that allows the user to swap conflicting assignments. This only applies to the first conflicting assignment found. This option will not be displayed if allowElementAssignmentConflicts is true.")]
		private bool _allowElementAssignmentSwap;

		// Token: 0x040003D5 RID: 981
		[SerializeField]
		[Tooltip("The width in relative pixels of the Action label column.")]
		private int _actionLabelWidth = 200;

		// Token: 0x040003D6 RID: 982
		[SerializeField]
		[Tooltip("The width in relative pixels of the Keyboard column.")]
		private int _keyboardColMaxWidth = 360;

		// Token: 0x040003D7 RID: 983
		[SerializeField]
		[Tooltip("The width in relative pixels of the Mouse column.")]
		private int _mouseColMaxWidth = 200;

		// Token: 0x040003D8 RID: 984
		[SerializeField]
		[Tooltip("The width in relative pixels of the Controller column.")]
		private int _controllerColMaxWidth = 200;

		// Token: 0x040003D9 RID: 985
		[SerializeField]
		[Tooltip("The height in relative pixels of the input grid button rows.")]
		private int _inputRowHeight = 40;

		// Token: 0x040003DA RID: 986
		[SerializeField]
		[Tooltip("The padding of the input grid button rows.")]
		private RectOffset _inputRowPadding = new RectOffset();

		// Token: 0x040003DB RID: 987
		[SerializeField]
		[Tooltip("The width in relative pixels of spacing between input fields in a single column.")]
		private int _inputRowFieldSpacing;

		// Token: 0x040003DC RID: 988
		[SerializeField]
		[Tooltip("The width in relative pixels of spacing between columns.")]
		private int _inputColumnSpacing = 40;

		// Token: 0x040003DD RID: 989
		[SerializeField]
		[Tooltip("The height in relative pixels of the space between Action Category sections. Only applicable if Show Action Category Labels is checked.")]
		private int _inputRowCategorySpacing = 20;

		// Token: 0x040003DE RID: 990
		[SerializeField]
		[Tooltip("The width in relative pixels of the invert toggle buttons.")]
		private int _invertToggleWidth = 40;

		// Token: 0x040003DF RID: 991
		[SerializeField]
		[Tooltip("The width in relative pixels of generated popup windows.")]
		private int _defaultWindowWidth = 500;

		// Token: 0x040003E0 RID: 992
		[SerializeField]
		[Tooltip("The height in relative pixels of generated popup windows.")]
		private int _defaultWindowHeight = 400;

		// Token: 0x040003E1 RID: 993
		[SerializeField]
		[Tooltip("The time in seconds the user has to press an element on a controller when assigning a controller to a Player. If this time elapses with no user input a controller, the assignment will be canceled.")]
		private float _controllerAssignmentTimeout = 5f;

		// Token: 0x040003E2 RID: 994
		[SerializeField]
		[Tooltip("The time in seconds the user has to press an element on a controller while waiting for axes to be centered before assigning input.")]
		private float _preInputAssignmentTimeout = 5f;

		// Token: 0x040003E3 RID: 995
		[SerializeField]
		[Tooltip("The time in seconds the user has to press an element on a controller when assigning input. If this time elapses with no user input on the target controller, the assignment will be canceled.")]
		private float _inputAssignmentTimeout = 5f;

		// Token: 0x040003E4 RID: 996
		[SerializeField]
		[Tooltip("The time in seconds the user has to press an element on a controller during calibration.")]
		private float _axisCalibrationTimeout = 5f;

		// Token: 0x040003E5 RID: 997
		[SerializeField]
		[Tooltip("If checked, mouse X-axis movement will always be ignored during input assignment. Check this if you don't want the horizontal mouse axis to be user-assignable to any Actions.")]
		private bool _ignoreMouseXAxisAssignment = true;

		// Token: 0x040003E6 RID: 998
		[SerializeField]
		[Tooltip("If checked, mouse Y-axis movement will always be ignored during input assignment. Check this if you don't want the vertical mouse axis to be user-assignable to any Actions.")]
		private bool _ignoreMouseYAxisAssignment = true;

		// Token: 0x040003E7 RID: 999
		[SerializeField]
		[Tooltip("An Action that when activated will alternately close or open the main screen as long as no popup windows are open.")]
		private int _screenToggleAction = -1;

		// Token: 0x040003E8 RID: 1000
		[SerializeField]
		[Tooltip("An Action that when activated will open the main screen if it is closed.")]
		private int _screenOpenAction = -1;

		// Token: 0x040003E9 RID: 1001
		[SerializeField]
		[Tooltip("An Action that when activated will close the main screen as long as no popup windows are open.")]
		private int _screenCloseAction = -1;

		// Token: 0x040003EA RID: 1002
		[SerializeField]
		[Tooltip("An Action that when activated will cancel and close any open popup window. Use with care because the element assigned to this Action can never be mapped by the user (because it would just cancel his assignment).")]
		private int _universalCancelAction = -1;

		// Token: 0x040003EB RID: 1003
		[SerializeField]
		[Tooltip("If enabled, Universal Cancel will also close the main screen if pressed when no windows are open.")]
		private bool _universalCancelClosesScreen = true;

		// Token: 0x040003EC RID: 1004
		[SerializeField]
		[Tooltip("If checked, controls will be displayed which will allow the user to customize certain Input Behavior settings.")]
		private bool _showInputBehaviorSettings;

		// Token: 0x040003ED RID: 1005
		[SerializeField]
		[Tooltip("Customizable settings for user-modifiable Input Behaviors. This can be used for settings like Mouse Look Sensitivity.")]
		private ControlMapper.InputBehaviorSettings[] _inputBehaviorSettings;

		// Token: 0x040003EE RID: 1006
		[SerializeField]
		[Tooltip("If enabled, UI elements will be themed based on the settings in Theme Settings.")]
		private bool _useThemeSettings = true;

		// Token: 0x040003EF RID: 1007
		[SerializeField]
		[Tooltip("Must be assigned a ThemeSettings object. Used to theme UI elements.")]
		private ThemeSettings _themeSettings;

		// Token: 0x040003F0 RID: 1008
		[SerializeField]
		[Tooltip("Must be assigned a LanguageData object. Used to retrieve language entries for UI elements.")]
		private LanguageDataBase _language;

		// Token: 0x040003F1 RID: 1009
		[SerializeField]
		[Tooltip("A list of prefabs. You should not have to modify this.")]
		private ControlMapper.Prefabs prefabs;

		// Token: 0x040003F2 RID: 1010
		[SerializeField]
		[Tooltip("A list of references to elements in the hierarchy. You should not have to modify this.")]
		private ControlMapper.References references;

		// Token: 0x040003F3 RID: 1011
		[SerializeField]
		[Tooltip("Show the label for the Players button group?")]
		private bool _showPlayersGroupLabel = true;

		// Token: 0x040003F4 RID: 1012
		[SerializeField]
		[Tooltip("Show the label for the Controller button group?")]
		private bool _showControllerGroupLabel = true;

		// Token: 0x040003F5 RID: 1013
		[SerializeField]
		[Tooltip("Show the label for the Assigned Controllers button group?")]
		private bool _showAssignedControllersGroupLabel = true;

		// Token: 0x040003F6 RID: 1014
		[SerializeField]
		[Tooltip("Show the label for the Settings button group?")]
		private bool _showSettingsGroupLabel = true;

		// Token: 0x040003F7 RID: 1015
		[SerializeField]
		[Tooltip("Show the label for the Map Categories button group?")]
		private bool _showMapCategoriesGroupLabel = true;

		// Token: 0x040003F8 RID: 1016
		[SerializeField]
		[Tooltip("Show the label for the current controller name?")]
		private bool _showControllerNameLabel = true;

		// Token: 0x040003F9 RID: 1017
		[SerializeField]
		[Tooltip("Show the Assigned Controllers group? If joystick auto-assignment is enabled in the Rewired Input Manager and the max joysticks per player is set to any value other than 1, the Assigned Controllers group will always be displayed.")]
		private bool _showAssignedControllers = true;

		// Token: 0x040003FA RID: 1018
		private Action _ScreenClosedEvent;

		// Token: 0x040003FB RID: 1019
		private Action _ScreenOpenedEvent;

		// Token: 0x040003FC RID: 1020
		private Action _PopupWindowOpenedEvent;

		// Token: 0x040003FD RID: 1021
		private Action _PopupWindowClosedEvent;

		// Token: 0x040003FE RID: 1022
		private Action _InputPollingStartedEvent;

		// Token: 0x040003FF RID: 1023
		private Action _InputPollingEndedEvent;

		// Token: 0x04000400 RID: 1024
		private Action _ThemeAppliedEvent;

		// Token: 0x04000401 RID: 1025
		[SerializeField]
		[Tooltip("Event sent when the UI is closed.")]
		private UnityEvent _onScreenClosed;

		// Token: 0x04000402 RID: 1026
		[SerializeField]
		[Tooltip("Event sent when the UI is opened.")]
		private UnityEvent _onScreenOpened;

		// Token: 0x04000403 RID: 1027
		[SerializeField]
		[Tooltip("Event sent when a popup window is closed.")]
		private UnityEvent _onPopupWindowClosed;

		// Token: 0x04000404 RID: 1028
		[SerializeField]
		[Tooltip("Event sent when a popup window is opened.")]
		private UnityEvent _onPopupWindowOpened;

		// Token: 0x04000405 RID: 1029
		[SerializeField]
		[Tooltip("Event sent when polling for input has started.")]
		private UnityEvent _onInputPollingStarted;

		// Token: 0x04000406 RID: 1030
		[SerializeField]
		[Tooltip("Event sent when polling for input has ended.")]
		private UnityEvent _onInputPollingEnded;

		// Token: 0x04000407 RID: 1031
		private static ControlMapper Instance;

		// Token: 0x04000408 RID: 1032
		[NonSerialized]
		private bool initialized;

		// Token: 0x04000409 RID: 1033
		[NonSerialized]
		private int playerCount;

		// Token: 0x0400040A RID: 1034
		private ControlMapper.InputGrid inputGrid;

		// Token: 0x0400040B RID: 1035
		private ControlMapper.WindowManager windowManager;

		// Token: 0x0400040C RID: 1036
		[NonSerialized]
		private int currentPlayerId;

		// Token: 0x0400040D RID: 1037
		[NonSerialized]
		private int currentMapCategoryId;

		// Token: 0x0400040E RID: 1038
		[NonSerialized]
		private List<ControlMapper.GUIButton> playerButtons;

		// Token: 0x0400040F RID: 1039
		[NonSerialized]
		private List<ControlMapper.GUIButton> mapCategoryButtons;

		// Token: 0x04000410 RID: 1040
		[NonSerialized]
		private List<ControlMapper.GUIButton> assignedControllerButtons;

		// Token: 0x04000411 RID: 1041
		private ControlMapper.GUIButton assignedControllerButtonsPlaceholder;

		// Token: 0x04000412 RID: 1042
		[NonSerialized]
		private List<GameObject> miscInstantiatedObjects;

		// Token: 0x04000413 RID: 1043
		[NonSerialized]
		private GameObject canvas;

		// Token: 0x04000414 RID: 1044
		[NonSerialized]
		private GameObject lastUISelection;

		// Token: 0x04000415 RID: 1045
		[NonSerialized]
		private int currentJoystickId = -1;

		// Token: 0x04000416 RID: 1046
		[NonSerialized]
		private float blockInputOnFocusEndTime;

		// Token: 0x04000417 RID: 1047
		[NonSerialized]
		private bool isPollingForInput;

		// Token: 0x04000418 RID: 1048
		[NonSerialized]
		private List<ThemedElement> themedElements = new List<ThemedElement>();

		// Token: 0x04000419 RID: 1049
		[NonSerialized]
		private ControlMapper.InputMapping pendingInputMapping;

		// Token: 0x0400041A RID: 1050
		[NonSerialized]
		private ControlMapper.AxisCalibrator pendingAxisCalibration;

		// Token: 0x0400041B RID: 1051
		private Action<InputFieldInfo> inputFieldActivatedDelegate;

		// Token: 0x0400041C RID: 1052
		private Action<ToggleInfo, bool> inputFieldInvertToggleStateChangedDelegate;

		// Token: 0x0400041D RID: 1053
		private Action _restoreDefaultsDelegate;

		// Token: 0x02000096 RID: 150
		private abstract class GUIElement
		{
			// Token: 0x1700035F RID: 863
			// (get) Token: 0x0600086E RID: 2158 RVA: 0x00019224 File Offset: 0x00017424
			// (set) Token: 0x0600086F RID: 2159 RVA: 0x0001922C File Offset: 0x0001742C
			public RectTransform rectTransform { get; private set; }

			// Token: 0x06000870 RID: 2160 RVA: 0x00019238 File Offset: 0x00017438
			public GUIElement(GameObject gameObject)
			{
				if (gameObject == null)
				{
					Debug.LogError("Rewired Control Mapper: gameObject is null!");
					return;
				}
				this.selectable = gameObject.GetComponent<Selectable>();
				if (this.selectable == null)
				{
					Debug.LogError("Rewired Control Mapper: Selectable is null!");
					return;
				}
				this.gameObject = gameObject;
				this.rectTransform = gameObject.GetComponent<RectTransform>();
				this.text = UnityTools.GetComponentInSelfOrChildren<TMP_Text>(gameObject);
				this.uiElementInfo = gameObject.GetComponent<UIElementInfo>();
				this.children = new List<ControlMapper.GUIElement>();
			}

			// Token: 0x06000871 RID: 2161 RVA: 0x000192BC File Offset: 0x000174BC
			public GUIElement(Selectable selectable, TMP_Text label)
			{
				if (selectable == null)
				{
					Debug.LogError("Rewired Control Mapper: Selectable is null!");
					return;
				}
				this.selectable = selectable;
				this.gameObject = selectable.gameObject;
				this.rectTransform = this.gameObject.GetComponent<RectTransform>();
				this.text = label;
				this.uiElementInfo = this.gameObject.GetComponent<UIElementInfo>();
				this.children = new List<ControlMapper.GUIElement>();
			}

			// Token: 0x06000872 RID: 2162 RVA: 0x0001932A File Offset: 0x0001752A
			public virtual void SetInteractible(bool state, bool playTransition)
			{
				this.SetInteractible(state, playTransition, false);
			}

			// Token: 0x06000873 RID: 2163 RVA: 0x00019338 File Offset: 0x00017538
			public virtual void SetInteractible(bool state, bool playTransition, bool permanent)
			{
				for (int i = 0; i < this.children.Count; i++)
				{
					if (this.children[i] != null)
					{
						this.children[i].SetInteractible(state, playTransition, permanent);
					}
				}
				if (this.permanentStateSet)
				{
					return;
				}
				if (this.selectable == null)
				{
					return;
				}
				if (permanent)
				{
					this.permanentStateSet = true;
				}
				if (this.selectable.interactable == state)
				{
					return;
				}
				UITools.SetInteractable(this.selectable, state, playTransition);
			}

			// Token: 0x06000874 RID: 2164 RVA: 0x000193BC File Offset: 0x000175BC
			public virtual void SetTextWidth(int value)
			{
				if (this.text == null)
				{
					return;
				}
				LayoutElement layoutElement = this.text.GetComponent<LayoutElement>();
				if (layoutElement == null)
				{
					layoutElement = this.text.gameObject.AddComponent<LayoutElement>();
				}
				layoutElement.preferredWidth = (float)value;
			}

			// Token: 0x06000875 RID: 2165 RVA: 0x00019408 File Offset: 0x00017608
			public virtual void SetFirstChildObjectWidth(ControlMapper.LayoutElementSizeType type, int value)
			{
				if (this.rectTransform.childCount == 0)
				{
					return;
				}
				Transform child = this.rectTransform.GetChild(0);
				LayoutElement layoutElement = child.GetComponent<LayoutElement>();
				if (layoutElement == null)
				{
					layoutElement = child.gameObject.AddComponent<LayoutElement>();
				}
				if (type == ControlMapper.LayoutElementSizeType.MinSize)
				{
					layoutElement.minWidth = (float)value;
					return;
				}
				if (type == ControlMapper.LayoutElementSizeType.PreferredSize)
				{
					layoutElement.preferredWidth = (float)value;
					return;
				}
				throw new NotImplementedException();
			}

			// Token: 0x06000876 RID: 2166 RVA: 0x0001946A File Offset: 0x0001766A
			public virtual void SetLabel(string label)
			{
				if (this.text == null)
				{
					return;
				}
				this.text.text = label;
			}

			// Token: 0x06000877 RID: 2167 RVA: 0x00019487 File Offset: 0x00017687
			public virtual string GetLabel()
			{
				if (this.text == null)
				{
					return string.Empty;
				}
				return this.text.text;
			}

			// Token: 0x06000878 RID: 2168 RVA: 0x000194A8 File Offset: 0x000176A8
			public virtual void AddChild(ControlMapper.GUIElement child)
			{
				this.children.Add(child);
			}

			// Token: 0x06000879 RID: 2169 RVA: 0x000194B6 File Offset: 0x000176B6
			public void SetElementInfoData(string identifier, int intData)
			{
				if (this.uiElementInfo == null)
				{
					return;
				}
				this.uiElementInfo.identifier = identifier;
				this.uiElementInfo.intData = intData;
			}

			// Token: 0x0600087A RID: 2170 RVA: 0x000194DF File Offset: 0x000176DF
			public virtual void SetActive(bool state)
			{
				if (this.gameObject == null)
				{
					return;
				}
				this.gameObject.SetActive(state);
			}

			// Token: 0x0600087B RID: 2171 RVA: 0x000194FC File Offset: 0x000176FC
			protected virtual bool Init()
			{
				bool flag = true;
				for (int i = 0; i < this.children.Count; i++)
				{
					if (this.children[i] != null && !this.children[i].Init())
					{
						flag = false;
					}
				}
				if (this.selectable == null)
				{
					Debug.LogError("Rewired Control Mapper: UI Element is missing Selectable component!");
					flag = false;
				}
				if (this.rectTransform == null)
				{
					Debug.LogError("Rewired Control Mapper: UI Element is missing RectTransform component!");
					flag = false;
				}
				if (this.uiElementInfo == null)
				{
					Debug.LogError("Rewired Control Mapper: UI Element is missing UIElementInfo component!");
					flag = false;
				}
				return flag;
			}

			// Token: 0x0400041E RID: 1054
			public readonly GameObject gameObject;

			// Token: 0x0400041F RID: 1055
			protected readonly TMP_Text text;

			// Token: 0x04000420 RID: 1056
			public readonly Selectable selectable;

			// Token: 0x04000421 RID: 1057
			protected readonly UIElementInfo uiElementInfo;

			// Token: 0x04000422 RID: 1058
			protected bool permanentStateSet;

			// Token: 0x04000423 RID: 1059
			protected readonly List<ControlMapper.GUIElement> children;
		}

		// Token: 0x02000097 RID: 151
		private class GUIButton : ControlMapper.GUIElement
		{
			// Token: 0x17000360 RID: 864
			// (get) Token: 0x0600087C RID: 2172 RVA: 0x00019593 File Offset: 0x00017793
			protected Button button
			{
				get
				{
					return this.selectable as Button;
				}
			}

			// Token: 0x17000361 RID: 865
			// (get) Token: 0x0600087D RID: 2173 RVA: 0x000195A0 File Offset: 0x000177A0
			public ButtonInfo buttonInfo
			{
				get
				{
					return this.uiElementInfo as ButtonInfo;
				}
			}

			// Token: 0x0600087E RID: 2174 RVA: 0x000195AD File Offset: 0x000177AD
			public GUIButton(GameObject gameObject)
				: base(gameObject)
			{
				this.Init();
			}

			// Token: 0x0600087F RID: 2175 RVA: 0x000195BD File Offset: 0x000177BD
			public GUIButton(Button button, TMP_Text label)
				: base(button, label)
			{
				this.Init();
			}

			// Token: 0x06000880 RID: 2176 RVA: 0x000195CE File Offset: 0x000177CE
			public void SetButtonInfoData(string identifier, int intData)
			{
				base.SetElementInfoData(identifier, intData);
			}

			// Token: 0x06000881 RID: 2177 RVA: 0x000195D8 File Offset: 0x000177D8
			public void SetOnClickCallback(Action<ButtonInfo> callback)
			{
				if (this.button == null)
				{
					return;
				}
				this.button.onClick.AddListener(delegate
				{
					callback(this.buttonInfo);
				});
			}
		}

		// Token: 0x02000099 RID: 153
		private class GUIInputField : ControlMapper.GUIElement
		{
			// Token: 0x17000362 RID: 866
			// (get) Token: 0x06000884 RID: 2180 RVA: 0x00019593 File Offset: 0x00017793
			protected Button button
			{
				get
				{
					return this.selectable as Button;
				}
			}

			// Token: 0x17000363 RID: 867
			// (get) Token: 0x06000885 RID: 2181 RVA: 0x0001963C File Offset: 0x0001783C
			public InputFieldInfo fieldInfo
			{
				get
				{
					return this.uiElementInfo as InputFieldInfo;
				}
			}

			// Token: 0x17000364 RID: 868
			// (get) Token: 0x06000886 RID: 2182 RVA: 0x00019649 File Offset: 0x00017849
			public bool hasToggle
			{
				get
				{
					return this.toggle != null;
				}
			}

			// Token: 0x17000365 RID: 869
			// (get) Token: 0x06000887 RID: 2183 RVA: 0x00019654 File Offset: 0x00017854
			// (set) Token: 0x06000888 RID: 2184 RVA: 0x0001965C File Offset: 0x0001785C
			public ControlMapper.GUIToggle toggle { get; private set; }

			// Token: 0x17000366 RID: 870
			// (get) Token: 0x06000889 RID: 2185 RVA: 0x00019665 File Offset: 0x00017865
			// (set) Token: 0x0600088A RID: 2186 RVA: 0x00019682 File Offset: 0x00017882
			public int actionElementMapId
			{
				get
				{
					if (this.fieldInfo == null)
					{
						return -1;
					}
					return this.fieldInfo.actionElementMapId;
				}
				set
				{
					if (this.fieldInfo == null)
					{
						return;
					}
					this.fieldInfo.actionElementMapId = value;
				}
			}

			// Token: 0x17000367 RID: 871
			// (get) Token: 0x0600088B RID: 2187 RVA: 0x0001969F File Offset: 0x0001789F
			// (set) Token: 0x0600088C RID: 2188 RVA: 0x000196BC File Offset: 0x000178BC
			public int controllerId
			{
				get
				{
					if (this.fieldInfo == null)
					{
						return -1;
					}
					return this.fieldInfo.controllerId;
				}
				set
				{
					if (this.fieldInfo == null)
					{
						return;
					}
					this.fieldInfo.controllerId = value;
				}
			}

			// Token: 0x0600088D RID: 2189 RVA: 0x000196D9 File Offset: 0x000178D9
			public GUIInputField(GameObject gameObject)
				: base(gameObject)
			{
				if (!this.Init())
				{
					return;
				}
				this.fieldInfo.glyphOrText = UnityTools.GetComponentInSelfOrChildren<UnityUIControllerElementGlyph>(gameObject);
			}

			// Token: 0x0600088E RID: 2190 RVA: 0x000196FC File Offset: 0x000178FC
			public GUIInputField(Button button, TMP_Text label)
				: base(button, label)
			{
				if (!this.Init())
				{
					return;
				}
				this.fieldInfo.glyphOrText = UnityTools.GetComponentInSelfOrChildren<UnityUIControllerElementGlyph>(button.gameObject);
			}

			// Token: 0x0600088F RID: 2191 RVA: 0x00019728 File Offset: 0x00017928
			public void SetFieldInfoData(int actionId, AxisRange axisRange, ControllerType controllerType, int intData)
			{
				base.SetElementInfoData(string.Empty, intData);
				if (this.fieldInfo == null)
				{
					return;
				}
				this.fieldInfo.actionId = actionId;
				this.fieldInfo.axisRange = axisRange;
				this.fieldInfo.controllerType = controllerType;
			}

			// Token: 0x06000890 RID: 2192 RVA: 0x00019778 File Offset: 0x00017978
			public void SetOnClickCallback(Action<InputFieldInfo> callback)
			{
				if (this.button == null)
				{
					return;
				}
				this.button.onClick.AddListener(delegate
				{
					callback(this.fieldInfo);
				});
			}

			// Token: 0x06000891 RID: 2193 RVA: 0x000197C4 File Offset: 0x000179C4
			public virtual void SetInteractable(bool state, bool playTransition, bool permanent)
			{
				if (this.permanentStateSet)
				{
					return;
				}
				if (this.hasToggle && !state)
				{
					this.toggle.SetInteractible(state, playTransition, permanent);
				}
				base.SetInteractible(state, playTransition, permanent);
			}

			// Token: 0x06000892 RID: 2194 RVA: 0x000197F1 File Offset: 0x000179F1
			public void AddToggle(ControlMapper.GUIToggle toggle)
			{
				if (toggle == null)
				{
					return;
				}
				this.toggle = toggle;
			}
		}

		// Token: 0x0200009B RID: 155
		private class GUIToggle : ControlMapper.GUIElement
		{
			// Token: 0x17000368 RID: 872
			// (get) Token: 0x06000895 RID: 2197 RVA: 0x00019816 File Offset: 0x00017A16
			protected Toggle toggle
			{
				get
				{
					return this.selectable as Toggle;
				}
			}

			// Token: 0x17000369 RID: 873
			// (get) Token: 0x06000896 RID: 2198 RVA: 0x00019823 File Offset: 0x00017A23
			public ToggleInfo toggleInfo
			{
				get
				{
					return this.uiElementInfo as ToggleInfo;
				}
			}

			// Token: 0x1700036A RID: 874
			// (get) Token: 0x06000897 RID: 2199 RVA: 0x00019830 File Offset: 0x00017A30
			// (set) Token: 0x06000898 RID: 2200 RVA: 0x0001984D File Offset: 0x00017A4D
			public int actionElementMapId
			{
				get
				{
					if (this.toggleInfo == null)
					{
						return -1;
					}
					return this.toggleInfo.actionElementMapId;
				}
				set
				{
					if (this.toggleInfo == null)
					{
						return;
					}
					this.toggleInfo.actionElementMapId = value;
				}
			}

			// Token: 0x06000899 RID: 2201 RVA: 0x000195AD File Offset: 0x000177AD
			public GUIToggle(GameObject gameObject)
				: base(gameObject)
			{
				this.Init();
			}

			// Token: 0x0600089A RID: 2202 RVA: 0x000195BD File Offset: 0x000177BD
			public GUIToggle(Toggle toggle, TMP_Text label)
				: base(toggle, label)
			{
				this.Init();
			}

			// Token: 0x0600089B RID: 2203 RVA: 0x0001986C File Offset: 0x00017A6C
			public void SetToggleInfoData(int actionId, AxisRange axisRange, ControllerType controllerType, int intData)
			{
				base.SetElementInfoData(string.Empty, intData);
				if (this.toggleInfo == null)
				{
					return;
				}
				this.toggleInfo.actionId = actionId;
				this.toggleInfo.axisRange = axisRange;
				this.toggleInfo.controllerType = controllerType;
			}

			// Token: 0x0600089C RID: 2204 RVA: 0x000198BC File Offset: 0x00017ABC
			public void SetOnSubmitCallback(Action<ToggleInfo, bool> callback)
			{
				if (this.toggle == null)
				{
					return;
				}
				EventTrigger eventTrigger = this.toggle.GetComponent<EventTrigger>();
				if (eventTrigger == null)
				{
					eventTrigger = this.toggle.gameObject.AddComponent<EventTrigger>();
				}
				EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
				triggerEvent.AddListener(delegate(BaseEventData data)
				{
					PointerEventData pointerEventData = data as PointerEventData;
					if (pointerEventData != null && pointerEventData.button != PointerEventData.InputButton.Left)
					{
						return;
					}
					callback(this.toggleInfo, this.toggle.isOn);
				});
				EventTrigger.Entry entry = new EventTrigger.Entry
				{
					callback = triggerEvent,
					eventID = EventTriggerType.Submit
				};
				EventTrigger.Entry entry2 = new EventTrigger.Entry
				{
					callback = triggerEvent,
					eventID = EventTriggerType.PointerClick
				};
				if (eventTrigger.triggers != null)
				{
					eventTrigger.triggers.Clear();
				}
				else
				{
					eventTrigger.triggers = new List<EventTrigger.Entry>();
				}
				eventTrigger.triggers.Add(entry);
				eventTrigger.triggers.Add(entry2);
			}

			// Token: 0x0600089D RID: 2205 RVA: 0x0001998D File Offset: 0x00017B8D
			public void SetToggleState(bool state)
			{
				if (this.toggle == null)
				{
					return;
				}
				this.toggle.isOn = state;
			}
		}

		// Token: 0x0200009D RID: 157
		private class GUILabel
		{
			// Token: 0x1700036B RID: 875
			// (get) Token: 0x060008A0 RID: 2208 RVA: 0x000199F2 File Offset: 0x00017BF2
			// (set) Token: 0x060008A1 RID: 2209 RVA: 0x000199FA File Offset: 0x00017BFA
			public GameObject gameObject { get; private set; }

			// Token: 0x1700036C RID: 876
			// (get) Token: 0x060008A2 RID: 2210 RVA: 0x00019A03 File Offset: 0x00017C03
			// (set) Token: 0x060008A3 RID: 2211 RVA: 0x00019A0B File Offset: 0x00017C0B
			private TMP_Text text { get; set; }

			// Token: 0x1700036D RID: 877
			// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00019A14 File Offset: 0x00017C14
			// (set) Token: 0x060008A5 RID: 2213 RVA: 0x00019A1C File Offset: 0x00017C1C
			public RectTransform rectTransform { get; private set; }

			// Token: 0x060008A6 RID: 2214 RVA: 0x00019A25 File Offset: 0x00017C25
			public GUILabel(GameObject gameObject)
			{
				if (gameObject == null)
				{
					Debug.LogError("Rewired Control Mapper: gameObject is null!");
					return;
				}
				this.text = UnityTools.GetComponentInSelfOrChildren<TMP_Text>(gameObject);
				this.Check();
			}

			// Token: 0x060008A7 RID: 2215 RVA: 0x00019A54 File Offset: 0x00017C54
			public GUILabel(TMP_Text label)
			{
				this.text = label;
				this.Check();
			}

			// Token: 0x060008A8 RID: 2216 RVA: 0x00019A6A File Offset: 0x00017C6A
			public void SetSize(int width, int height)
			{
				if (this.text == null)
				{
					return;
				}
				this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)width);
				this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)height);
			}

			// Token: 0x060008A9 RID: 2217 RVA: 0x00019A97 File Offset: 0x00017C97
			public void SetWidth(int width)
			{
				if (this.text == null)
				{
					return;
				}
				this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)width);
			}

			// Token: 0x060008AA RID: 2218 RVA: 0x00019AB6 File Offset: 0x00017CB6
			public void SetHeight(int height)
			{
				if (this.text == null)
				{
					return;
				}
				this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)height);
			}

			// Token: 0x060008AB RID: 2219 RVA: 0x00019AD5 File Offset: 0x00017CD5
			public void SetLabel(string label)
			{
				if (this.text == null)
				{
					return;
				}
				this.text.text = label;
			}

			// Token: 0x060008AC RID: 2220 RVA: 0x00019AF2 File Offset: 0x00017CF2
			public void SetFontStyle(FontStyles style)
			{
				if (this.text == null)
				{
					return;
				}
				this.text.fontStyle = style;
			}

			// Token: 0x060008AD RID: 2221 RVA: 0x00019B0F File Offset: 0x00017D0F
			public void SetTextAlignment(TextAlignmentOptions alignment)
			{
				if (this.text == null)
				{
					return;
				}
				this.text.alignment = alignment;
			}

			// Token: 0x060008AE RID: 2222 RVA: 0x00019B2C File Offset: 0x00017D2C
			public void SetActive(bool state)
			{
				if (this.gameObject == null)
				{
					return;
				}
				this.gameObject.SetActive(state);
			}

			// Token: 0x060008AF RID: 2223 RVA: 0x00019B4C File Offset: 0x00017D4C
			private bool Check()
			{
				bool flag = true;
				if (this.text == null)
				{
					Debug.LogError("Rewired Control Mapper: Button is missing Text child component!");
					flag = false;
				}
				this.gameObject = this.text.gameObject;
				this.rectTransform = this.text.GetComponent<RectTransform>();
				return flag;
			}
		}

		// Token: 0x0200009E RID: 158
		[Serializable]
		public class MappingSet
		{
			// Token: 0x1700036E RID: 878
			// (get) Token: 0x060008B0 RID: 2224 RVA: 0x00019B98 File Offset: 0x00017D98
			public int mapCategoryId
			{
				get
				{
					return this._mapCategoryId;
				}
			}

			// Token: 0x1700036F RID: 879
			// (get) Token: 0x060008B1 RID: 2225 RVA: 0x00019BA0 File Offset: 0x00017DA0
			public ControlMapper.MappingSet.ActionListMode actionListMode
			{
				get
				{
					return this._actionListMode;
				}
			}

			// Token: 0x17000370 RID: 880
			// (get) Token: 0x060008B2 RID: 2226 RVA: 0x00019BA8 File Offset: 0x00017DA8
			public IList<int> actionCategoryIds
			{
				get
				{
					if (this._actionCategoryIds == null)
					{
						return null;
					}
					if (this._actionCategoryIdsReadOnly == null)
					{
						this._actionCategoryIdsReadOnly = new ReadOnlyCollection<int>(this._actionCategoryIds);
					}
					return this._actionCategoryIdsReadOnly;
				}
			}

			// Token: 0x17000371 RID: 881
			// (get) Token: 0x060008B3 RID: 2227 RVA: 0x00019BD3 File Offset: 0x00017DD3
			public IList<int> actionIds
			{
				get
				{
					if (this._actionIds == null)
					{
						return null;
					}
					if (this._actionIdsReadOnly == null)
					{
						this._actionIdsReadOnly = new ReadOnlyCollection<int>(this._actionIds);
					}
					return this._actionIds;
				}
			}

			// Token: 0x17000372 RID: 882
			// (get) Token: 0x060008B4 RID: 2228 RVA: 0x00019C00 File Offset: 0x00017E00
			public bool isValid
			{
				get
				{
					if (this._mapCategoryId < 0)
					{
						return false;
					}
					InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(this._mapCategoryId);
					return mapCategory != null && mapCategory.userAssignable;
				}
			}

			// Token: 0x060008B5 RID: 2229 RVA: 0x00019C39 File Offset: 0x00017E39
			public MappingSet()
			{
				this._mapCategoryId = -1;
				this._actionCategoryIds = new int[0];
				this._actionIds = new int[0];
				this._actionListMode = ControlMapper.MappingSet.ActionListMode.ActionCategory;
			}

			// Token: 0x060008B6 RID: 2230 RVA: 0x00019C67 File Offset: 0x00017E67
			private MappingSet(int mapCategoryId, ControlMapper.MappingSet.ActionListMode actionListMode, int[] actionCategoryIds, int[] actionIds)
			{
				this._mapCategoryId = mapCategoryId;
				this._actionListMode = actionListMode;
				this._actionCategoryIds = actionCategoryIds;
				this._actionIds = actionIds;
			}

			// Token: 0x17000373 RID: 883
			// (get) Token: 0x060008B7 RID: 2231 RVA: 0x00019C8C File Offset: 0x00017E8C
			public static ControlMapper.MappingSet Default
			{
				get
				{
					return new ControlMapper.MappingSet(0, ControlMapper.MappingSet.ActionListMode.ActionCategory, new int[1], new int[0]);
				}
			}

			// Token: 0x0400042F RID: 1071
			[SerializeField]
			[Tooltip("The Map Category that will be displayed to the user for remapping.")]
			private int _mapCategoryId;

			// Token: 0x04000430 RID: 1072
			[SerializeField]
			[Tooltip("Choose whether you want to list Actions to display for this Map Category by individual Action or by all the Actions in an Action Category.")]
			private ControlMapper.MappingSet.ActionListMode _actionListMode;

			// Token: 0x04000431 RID: 1073
			[SerializeField]
			private int[] _actionCategoryIds;

			// Token: 0x04000432 RID: 1074
			[SerializeField]
			private int[] _actionIds;

			// Token: 0x04000433 RID: 1075
			private IList<int> _actionCategoryIdsReadOnly;

			// Token: 0x04000434 RID: 1076
			private IList<int> _actionIdsReadOnly;

			// Token: 0x0200009F RID: 159
			public enum ActionListMode
			{
				// Token: 0x04000436 RID: 1078
				ActionCategory,
				// Token: 0x04000437 RID: 1079
				Action
			}
		}

		// Token: 0x020000A0 RID: 160
		[Serializable]
		public class InputBehaviorSettings
		{
			// Token: 0x17000374 RID: 884
			// (get) Token: 0x060008B8 RID: 2232 RVA: 0x00019CA1 File Offset: 0x00017EA1
			public int inputBehaviorId
			{
				get
				{
					return this._inputBehaviorId;
				}
			}

			// Token: 0x17000375 RID: 885
			// (get) Token: 0x060008B9 RID: 2233 RVA: 0x00019CA9 File Offset: 0x00017EA9
			public bool showJoystickAxisSensitivity
			{
				get
				{
					return this._showJoystickAxisSensitivity;
				}
			}

			// Token: 0x17000376 RID: 886
			// (get) Token: 0x060008BA RID: 2234 RVA: 0x00019CB1 File Offset: 0x00017EB1
			public bool showMouseXYAxisSensitivity
			{
				get
				{
					return this._showMouseXYAxisSensitivity;
				}
			}

			// Token: 0x17000377 RID: 887
			// (get) Token: 0x060008BB RID: 2235 RVA: 0x00019CB9 File Offset: 0x00017EB9
			public string labelLanguageKey
			{
				get
				{
					return this._labelLanguageKey;
				}
			}

			// Token: 0x17000378 RID: 888
			// (get) Token: 0x060008BC RID: 2236 RVA: 0x00019CC1 File Offset: 0x00017EC1
			public string joystickAxisSensitivityLabelLanguageKey
			{
				get
				{
					return this._joystickAxisSensitivityLabelLanguageKey;
				}
			}

			// Token: 0x17000379 RID: 889
			// (get) Token: 0x060008BD RID: 2237 RVA: 0x00019CC9 File Offset: 0x00017EC9
			public string mouseXYAxisSensitivityLabelLanguageKey
			{
				get
				{
					return this._mouseXYAxisSensitivityLabelLanguageKey;
				}
			}

			// Token: 0x1700037A RID: 890
			// (get) Token: 0x060008BE RID: 2238 RVA: 0x00019CD1 File Offset: 0x00017ED1
			public Sprite joystickAxisSensitivityIcon
			{
				get
				{
					return this._joystickAxisSensitivityIcon;
				}
			}

			// Token: 0x1700037B RID: 891
			// (get) Token: 0x060008BF RID: 2239 RVA: 0x00019CD9 File Offset: 0x00017ED9
			public Sprite mouseXYAxisSensitivityIcon
			{
				get
				{
					return this._mouseXYAxisSensitivityIcon;
				}
			}

			// Token: 0x1700037C RID: 892
			// (get) Token: 0x060008C0 RID: 2240 RVA: 0x00019CE1 File Offset: 0x00017EE1
			public float joystickAxisSensitivityMin
			{
				get
				{
					return this._joystickAxisSensitivityMin;
				}
			}

			// Token: 0x1700037D RID: 893
			// (get) Token: 0x060008C1 RID: 2241 RVA: 0x00019CE9 File Offset: 0x00017EE9
			public float joystickAxisSensitivityMax
			{
				get
				{
					return this._joystickAxisSensitivityMax;
				}
			}

			// Token: 0x1700037E RID: 894
			// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00019CF1 File Offset: 0x00017EF1
			public float mouseXYAxisSensitivityMin
			{
				get
				{
					return this._mouseXYAxisSensitivityMin;
				}
			}

			// Token: 0x1700037F RID: 895
			// (get) Token: 0x060008C3 RID: 2243 RVA: 0x00019CF9 File Offset: 0x00017EF9
			public float mouseXYAxisSensitivityMax
			{
				get
				{
					return this._mouseXYAxisSensitivityMax;
				}
			}

			// Token: 0x17000380 RID: 896
			// (get) Token: 0x060008C4 RID: 2244 RVA: 0x00019D01 File Offset: 0x00017F01
			public bool isValid
			{
				get
				{
					return this._inputBehaviorId >= 0 && (this._showJoystickAxisSensitivity || this._showMouseXYAxisSensitivity);
				}
			}

			// Token: 0x04000438 RID: 1080
			[SerializeField]
			[Tooltip("The Input Behavior that will be displayed to the user for modification.")]
			private int _inputBehaviorId = -1;

			// Token: 0x04000439 RID: 1081
			[SerializeField]
			[Tooltip("If checked, a slider will be displayed so the user can change this value.")]
			private bool _showJoystickAxisSensitivity = true;

			// Token: 0x0400043A RID: 1082
			[SerializeField]
			[Tooltip("If checked, a slider will be displayed so the user can change this value.")]
			private bool _showMouseXYAxisSensitivity = true;

			// Token: 0x0400043B RID: 1083
			[SerializeField]
			[Tooltip("If set to a non-blank value, this key will be used to look up the name in Language to be displayed as the title for the Input Behavior control set. Otherwise, the name field of the InputBehavior will be used.")]
			private string _labelLanguageKey = string.Empty;

			// Token: 0x0400043C RID: 1084
			[SerializeField]
			[Tooltip("If set to a non-blank value, this name will be displayed above the individual slider control. Otherwise, no name will be displayed.")]
			private string _joystickAxisSensitivityLabelLanguageKey = string.Empty;

			// Token: 0x0400043D RID: 1085
			[SerializeField]
			[Tooltip("If set to a non-blank value, this key will be used to look up the name in Language to be displayed above the individual slider control. Otherwise, no name will be displayed.")]
			private string _mouseXYAxisSensitivityLabelLanguageKey = string.Empty;

			// Token: 0x0400043E RID: 1086
			[SerializeField]
			[Tooltip("The icon to display next to the slider. Set to none for no icon.")]
			private Sprite _joystickAxisSensitivityIcon;

			// Token: 0x0400043F RID: 1087
			[SerializeField]
			[Tooltip("The icon to display next to the slider. Set to none for no icon.")]
			private Sprite _mouseXYAxisSensitivityIcon;

			// Token: 0x04000440 RID: 1088
			[SerializeField]
			[Tooltip("Minimum value the user is allowed to set for this property.")]
			private float _joystickAxisSensitivityMin;

			// Token: 0x04000441 RID: 1089
			[SerializeField]
			[Tooltip("Maximum value the user is allowed to set for this property.")]
			private float _joystickAxisSensitivityMax = 2f;

			// Token: 0x04000442 RID: 1090
			[SerializeField]
			[Tooltip("Minimum value the user is allowed to set for this property.")]
			private float _mouseXYAxisSensitivityMin;

			// Token: 0x04000443 RID: 1091
			[SerializeField]
			[Tooltip("Maximum value the user is allowed to set for this property.")]
			private float _mouseXYAxisSensitivityMax = 2f;
		}

		// Token: 0x020000A1 RID: 161
		[Serializable]
		private class Prefabs
		{
			// Token: 0x17000381 RID: 897
			// (get) Token: 0x060008C6 RID: 2246 RVA: 0x00019D7F File Offset: 0x00017F7F
			public GameObject button
			{
				get
				{
					return this._button;
				}
			}

			// Token: 0x17000382 RID: 898
			// (get) Token: 0x060008C7 RID: 2247 RVA: 0x00019D87 File Offset: 0x00017F87
			public GameObject fitButton
			{
				get
				{
					return this._fitButton;
				}
			}

			// Token: 0x17000383 RID: 899
			// (get) Token: 0x060008C8 RID: 2248 RVA: 0x00019D8F File Offset: 0x00017F8F
			public GameObject inputGridLabel
			{
				get
				{
					return this._inputGridLabel;
				}
			}

			// Token: 0x17000384 RID: 900
			// (get) Token: 0x060008C9 RID: 2249 RVA: 0x00019D97 File Offset: 0x00017F97
			public GameObject inputGridHeaderLabel
			{
				get
				{
					return this._inputGridHeaderLabel;
				}
			}

			// Token: 0x17000385 RID: 901
			// (get) Token: 0x060008CA RID: 2250 RVA: 0x00019D9F File Offset: 0x00017F9F
			public GameObject inputGridFieldButton
			{
				get
				{
					return this._inputGridFieldButton;
				}
			}

			// Token: 0x17000386 RID: 902
			// (get) Token: 0x060008CB RID: 2251 RVA: 0x00019DA7 File Offset: 0x00017FA7
			public GameObject inputGridFieldInvertToggle
			{
				get
				{
					return this._inputGridFieldInvertToggle;
				}
			}

			// Token: 0x17000387 RID: 903
			// (get) Token: 0x060008CC RID: 2252 RVA: 0x00019DAF File Offset: 0x00017FAF
			public GameObject window
			{
				get
				{
					return this._window;
				}
			}

			// Token: 0x17000388 RID: 904
			// (get) Token: 0x060008CD RID: 2253 RVA: 0x00019DB7 File Offset: 0x00017FB7
			public GameObject windowTitleText
			{
				get
				{
					return this._windowTitleText;
				}
			}

			// Token: 0x17000389 RID: 905
			// (get) Token: 0x060008CE RID: 2254 RVA: 0x00019DBF File Offset: 0x00017FBF
			public GameObject windowContentText
			{
				get
				{
					return this._windowContentText;
				}
			}

			// Token: 0x1700038A RID: 906
			// (get) Token: 0x060008CF RID: 2255 RVA: 0x00019DC7 File Offset: 0x00017FC7
			public GameObject fader
			{
				get
				{
					return this._fader;
				}
			}

			// Token: 0x1700038B RID: 907
			// (get) Token: 0x060008D0 RID: 2256 RVA: 0x00019DCF File Offset: 0x00017FCF
			public GameObject calibrationWindow
			{
				get
				{
					return this._calibrationWindow;
				}
			}

			// Token: 0x1700038C RID: 908
			// (get) Token: 0x060008D1 RID: 2257 RVA: 0x00019DD7 File Offset: 0x00017FD7
			public GameObject inputBehaviorsWindow
			{
				get
				{
					return this._inputBehaviorsWindow;
				}
			}

			// Token: 0x1700038D RID: 909
			// (get) Token: 0x060008D2 RID: 2258 RVA: 0x00019DDF File Offset: 0x00017FDF
			public GameObject centerStickGraphic
			{
				get
				{
					return this._centerStickGraphic;
				}
			}

			// Token: 0x1700038E RID: 910
			// (get) Token: 0x060008D3 RID: 2259 RVA: 0x00019DE7 File Offset: 0x00017FE7
			public GameObject moveStickGraphic
			{
				get
				{
					return this._moveStickGraphic;
				}
			}

			// Token: 0x060008D4 RID: 2260 RVA: 0x00019DF0 File Offset: 0x00017FF0
			public bool Check()
			{
				return !(this._button == null) && !(this._fitButton == null) && !(this._inputGridLabel == null) && !(this._inputGridHeaderLabel == null) && !(this._inputGridFieldButton == null) && !(this._inputGridFieldInvertToggle == null) && !(this._window == null) && !(this._windowTitleText == null) && !(this._windowContentText == null) && !(this._fader == null) && !(this._calibrationWindow == null) && !(this._inputBehaviorsWindow == null);
			}

			// Token: 0x04000444 RID: 1092
			[SerializeField]
			private GameObject _button;

			// Token: 0x04000445 RID: 1093
			[SerializeField]
			private GameObject _fitButton;

			// Token: 0x04000446 RID: 1094
			[SerializeField]
			private GameObject _inputGridLabel;

			// Token: 0x04000447 RID: 1095
			[SerializeField]
			private GameObject _inputGridHeaderLabel;

			// Token: 0x04000448 RID: 1096
			[SerializeField]
			private GameObject _inputGridFieldButton;

			// Token: 0x04000449 RID: 1097
			[SerializeField]
			private GameObject _inputGridFieldInvertToggle;

			// Token: 0x0400044A RID: 1098
			[SerializeField]
			private GameObject _window;

			// Token: 0x0400044B RID: 1099
			[SerializeField]
			private GameObject _windowTitleText;

			// Token: 0x0400044C RID: 1100
			[SerializeField]
			private GameObject _windowContentText;

			// Token: 0x0400044D RID: 1101
			[SerializeField]
			private GameObject _fader;

			// Token: 0x0400044E RID: 1102
			[SerializeField]
			private GameObject _calibrationWindow;

			// Token: 0x0400044F RID: 1103
			[SerializeField]
			private GameObject _inputBehaviorsWindow;

			// Token: 0x04000450 RID: 1104
			[SerializeField]
			private GameObject _centerStickGraphic;

			// Token: 0x04000451 RID: 1105
			[SerializeField]
			private GameObject _moveStickGraphic;
		}

		// Token: 0x020000A2 RID: 162
		[Serializable]
		private class References
		{
			// Token: 0x1700038F RID: 911
			// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00019EAE File Offset: 0x000180AE
			public Canvas canvas
			{
				get
				{
					return this._canvas;
				}
			}

			// Token: 0x17000390 RID: 912
			// (get) Token: 0x060008D7 RID: 2263 RVA: 0x00019EB6 File Offset: 0x000180B6
			public CanvasGroup mainCanvasGroup
			{
				get
				{
					return this._mainCanvasGroup;
				}
			}

			// Token: 0x17000391 RID: 913
			// (get) Token: 0x060008D8 RID: 2264 RVA: 0x00019EBE File Offset: 0x000180BE
			public Transform mainContent
			{
				get
				{
					return this._mainContent;
				}
			}

			// Token: 0x17000392 RID: 914
			// (get) Token: 0x060008D9 RID: 2265 RVA: 0x00019EC6 File Offset: 0x000180C6
			public Transform mainContentInner
			{
				get
				{
					return this._mainContentInner;
				}
			}

			// Token: 0x17000393 RID: 915
			// (get) Token: 0x060008DA RID: 2266 RVA: 0x00019ECE File Offset: 0x000180CE
			public UIGroup playersGroup
			{
				get
				{
					return this._playersGroup;
				}
			}

			// Token: 0x17000394 RID: 916
			// (get) Token: 0x060008DB RID: 2267 RVA: 0x00019ED6 File Offset: 0x000180D6
			public Transform controllerGroup
			{
				get
				{
					return this._controllerGroup;
				}
			}

			// Token: 0x17000395 RID: 917
			// (get) Token: 0x060008DC RID: 2268 RVA: 0x00019EDE File Offset: 0x000180DE
			public Transform controllerGroupLabelGroup
			{
				get
				{
					return this._controllerGroupLabelGroup;
				}
			}

			// Token: 0x17000396 RID: 918
			// (get) Token: 0x060008DD RID: 2269 RVA: 0x00019EE6 File Offset: 0x000180E6
			public UIGroup controllerSettingsGroup
			{
				get
				{
					return this._controllerSettingsGroup;
				}
			}

			// Token: 0x17000397 RID: 919
			// (get) Token: 0x060008DE RID: 2270 RVA: 0x00019EEE File Offset: 0x000180EE
			public UIGroup assignedControllersGroup
			{
				get
				{
					return this._assignedControllersGroup;
				}
			}

			// Token: 0x17000398 RID: 920
			// (get) Token: 0x060008DF RID: 2271 RVA: 0x00019EF6 File Offset: 0x000180F6
			public Transform settingsAndMapCategoriesGroup
			{
				get
				{
					return this._settingsAndMapCategoriesGroup;
				}
			}

			// Token: 0x17000399 RID: 921
			// (get) Token: 0x060008E0 RID: 2272 RVA: 0x00019EFE File Offset: 0x000180FE
			public UIGroup settingsGroup
			{
				get
				{
					return this._settingsGroup;
				}
			}

			// Token: 0x1700039A RID: 922
			// (get) Token: 0x060008E1 RID: 2273 RVA: 0x00019F06 File Offset: 0x00018106
			public UIGroup mapCategoriesGroup
			{
				get
				{
					return this._mapCategoriesGroup;
				}
			}

			// Token: 0x1700039B RID: 923
			// (get) Token: 0x060008E2 RID: 2274 RVA: 0x00019F0E File Offset: 0x0001810E
			public Transform inputGridGroup
			{
				get
				{
					return this._inputGridGroup;
				}
			}

			// Token: 0x1700039C RID: 924
			// (get) Token: 0x060008E3 RID: 2275 RVA: 0x00019F16 File Offset: 0x00018116
			public Transform inputGridContainer
			{
				get
				{
					return this._inputGridContainer;
				}
			}

			// Token: 0x1700039D RID: 925
			// (get) Token: 0x060008E4 RID: 2276 RVA: 0x00019F1E File Offset: 0x0001811E
			public Transform inputGridHeadersGroup
			{
				get
				{
					return this._inputGridHeadersGroup;
				}
			}

			// Token: 0x1700039E RID: 926
			// (get) Token: 0x060008E5 RID: 2277 RVA: 0x00019F26 File Offset: 0x00018126
			public Scrollbar inputGridVScrollbar
			{
				get
				{
					return this._inputGridVScrollbar;
				}
			}

			// Token: 0x1700039F RID: 927
			// (get) Token: 0x060008E6 RID: 2278 RVA: 0x00019F2E File Offset: 0x0001812E
			public ScrollRect inputGridScrollRect
			{
				get
				{
					return this._inputGridScrollRect;
				}
			}

			// Token: 0x170003A0 RID: 928
			// (get) Token: 0x060008E7 RID: 2279 RVA: 0x00019F36 File Offset: 0x00018136
			public Transform inputGridInnerGroup
			{
				get
				{
					return this._inputGridInnerGroup;
				}
			}

			// Token: 0x170003A1 RID: 929
			// (get) Token: 0x060008E8 RID: 2280 RVA: 0x00019F3E File Offset: 0x0001813E
			public TMP_Text controllerNameLabel
			{
				get
				{
					return this._controllerNameLabel;
				}
			}

			// Token: 0x170003A2 RID: 930
			// (get) Token: 0x060008E9 RID: 2281 RVA: 0x00019F46 File Offset: 0x00018146
			public Button removeControllerButton
			{
				get
				{
					return this._removeControllerButton;
				}
			}

			// Token: 0x170003A3 RID: 931
			// (get) Token: 0x060008EA RID: 2282 RVA: 0x00019F4E File Offset: 0x0001814E
			public Button assignControllerButton
			{
				get
				{
					return this._assignControllerButton;
				}
			}

			// Token: 0x170003A4 RID: 932
			// (get) Token: 0x060008EB RID: 2283 RVA: 0x00019F56 File Offset: 0x00018156
			public Button calibrateControllerButton
			{
				get
				{
					return this._calibrateControllerButton;
				}
			}

			// Token: 0x170003A5 RID: 933
			// (get) Token: 0x060008EC RID: 2284 RVA: 0x00019F5E File Offset: 0x0001815E
			public Button doneButton
			{
				get
				{
					return this._doneButton;
				}
			}

			// Token: 0x170003A6 RID: 934
			// (get) Token: 0x060008ED RID: 2285 RVA: 0x00019F66 File Offset: 0x00018166
			public Button restoreDefaultsButton
			{
				get
				{
					return this._restoreDefaultsButton;
				}
			}

			// Token: 0x170003A7 RID: 935
			// (get) Token: 0x060008EE RID: 2286 RVA: 0x00019F6E File Offset: 0x0001816E
			public Selectable defaultSelection
			{
				get
				{
					return this._defaultSelection;
				}
			}

			// Token: 0x170003A8 RID: 936
			// (get) Token: 0x060008EF RID: 2287 RVA: 0x00019F76 File Offset: 0x00018176
			public GameObject[] fixedSelectableUIElements
			{
				get
				{
					return this._fixedSelectableUIElements;
				}
			}

			// Token: 0x170003A9 RID: 937
			// (get) Token: 0x060008F0 RID: 2288 RVA: 0x00019F7E File Offset: 0x0001817E
			public Image mainBackgroundImage
			{
				get
				{
					return this._mainBackgroundImage;
				}
			}

			// Token: 0x170003AA RID: 938
			// (get) Token: 0x060008F1 RID: 2289 RVA: 0x00019F86 File Offset: 0x00018186
			// (set) Token: 0x060008F2 RID: 2290 RVA: 0x00019F8E File Offset: 0x0001818E
			public LayoutElement inputGridLayoutElement { get; set; }

			// Token: 0x170003AB RID: 939
			// (get) Token: 0x060008F3 RID: 2291 RVA: 0x00019F97 File Offset: 0x00018197
			// (set) Token: 0x060008F4 RID: 2292 RVA: 0x00019F9F File Offset: 0x0001819F
			public Transform inputGridActionColumn { get; set; }

			// Token: 0x170003AC RID: 940
			// (get) Token: 0x060008F5 RID: 2293 RVA: 0x00019FA8 File Offset: 0x000181A8
			// (set) Token: 0x060008F6 RID: 2294 RVA: 0x00019FB0 File Offset: 0x000181B0
			public Transform inputGridKeyboardColumn { get; set; }

			// Token: 0x170003AD RID: 941
			// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00019FB9 File Offset: 0x000181B9
			// (set) Token: 0x060008F8 RID: 2296 RVA: 0x00019FC1 File Offset: 0x000181C1
			public Transform inputGridMouseColumn { get; set; }

			// Token: 0x170003AE RID: 942
			// (get) Token: 0x060008F9 RID: 2297 RVA: 0x00019FCA File Offset: 0x000181CA
			// (set) Token: 0x060008FA RID: 2298 RVA: 0x00019FD2 File Offset: 0x000181D2
			public Transform inputGridControllerColumn { get; set; }

			// Token: 0x170003AF RID: 943
			// (get) Token: 0x060008FB RID: 2299 RVA: 0x00019FDB File Offset: 0x000181DB
			// (set) Token: 0x060008FC RID: 2300 RVA: 0x00019FE3 File Offset: 0x000181E3
			public Transform inputGridHeader1 { get; set; }

			// Token: 0x170003B0 RID: 944
			// (get) Token: 0x060008FD RID: 2301 RVA: 0x00019FEC File Offset: 0x000181EC
			// (set) Token: 0x060008FE RID: 2302 RVA: 0x00019FF4 File Offset: 0x000181F4
			public Transform inputGridHeader2 { get; set; }

			// Token: 0x170003B1 RID: 945
			// (get) Token: 0x060008FF RID: 2303 RVA: 0x00019FFD File Offset: 0x000181FD
			// (set) Token: 0x06000900 RID: 2304 RVA: 0x0001A005 File Offset: 0x00018205
			public Transform inputGridHeader3 { get; set; }

			// Token: 0x170003B2 RID: 946
			// (get) Token: 0x06000901 RID: 2305 RVA: 0x0001A00E File Offset: 0x0001820E
			// (set) Token: 0x06000902 RID: 2306 RVA: 0x0001A016 File Offset: 0x00018216
			public Transform inputGridHeader4 { get; set; }

			// Token: 0x06000903 RID: 2307 RVA: 0x0001A020 File Offset: 0x00018220
			public bool Check()
			{
				return !(this._canvas == null) && !(this._mainCanvasGroup == null) && !(this._mainContent == null) && !(this._mainContentInner == null) && !(this._playersGroup == null) && !(this._controllerGroup == null) && !(this._controllerGroupLabelGroup == null) && !(this._controllerSettingsGroup == null) && !(this._assignedControllersGroup == null) && !(this._settingsAndMapCategoriesGroup == null) && !(this._settingsGroup == null) && !(this._mapCategoriesGroup == null) && !(this._inputGridGroup == null) && !(this._inputGridContainer == null) && !(this._inputGridHeadersGroup == null) && !(this._inputGridVScrollbar == null) && !(this._inputGridScrollRect == null) && !(this._inputGridInnerGroup == null) && !(this._controllerNameLabel == null) && !(this._removeControllerButton == null) && !(this._assignControllerButton == null) && !(this._calibrateControllerButton == null) && !(this._doneButton == null) && !(this._restoreDefaultsButton == null) && !(this._defaultSelection == null);
			}

			// Token: 0x04000452 RID: 1106
			[SerializeField]
			private Canvas _canvas;

			// Token: 0x04000453 RID: 1107
			[SerializeField]
			private CanvasGroup _mainCanvasGroup;

			// Token: 0x04000454 RID: 1108
			[SerializeField]
			private Transform _mainContent;

			// Token: 0x04000455 RID: 1109
			[SerializeField]
			private Transform _mainContentInner;

			// Token: 0x04000456 RID: 1110
			[SerializeField]
			private UIGroup _playersGroup;

			// Token: 0x04000457 RID: 1111
			[SerializeField]
			private Transform _controllerGroup;

			// Token: 0x04000458 RID: 1112
			[SerializeField]
			private Transform _controllerGroupLabelGroup;

			// Token: 0x04000459 RID: 1113
			[SerializeField]
			private UIGroup _controllerSettingsGroup;

			// Token: 0x0400045A RID: 1114
			[SerializeField]
			private UIGroup _assignedControllersGroup;

			// Token: 0x0400045B RID: 1115
			[SerializeField]
			private Transform _settingsAndMapCategoriesGroup;

			// Token: 0x0400045C RID: 1116
			[SerializeField]
			private UIGroup _settingsGroup;

			// Token: 0x0400045D RID: 1117
			[SerializeField]
			private UIGroup _mapCategoriesGroup;

			// Token: 0x0400045E RID: 1118
			[SerializeField]
			private Transform _inputGridGroup;

			// Token: 0x0400045F RID: 1119
			[SerializeField]
			private Transform _inputGridContainer;

			// Token: 0x04000460 RID: 1120
			[SerializeField]
			private Transform _inputGridHeadersGroup;

			// Token: 0x04000461 RID: 1121
			[SerializeField]
			private Scrollbar _inputGridVScrollbar;

			// Token: 0x04000462 RID: 1122
			[SerializeField]
			private ScrollRect _inputGridScrollRect;

			// Token: 0x04000463 RID: 1123
			[SerializeField]
			private Transform _inputGridInnerGroup;

			// Token: 0x04000464 RID: 1124
			[SerializeField]
			private TMP_Text _controllerNameLabel;

			// Token: 0x04000465 RID: 1125
			[SerializeField]
			private Button _removeControllerButton;

			// Token: 0x04000466 RID: 1126
			[SerializeField]
			private Button _assignControllerButton;

			// Token: 0x04000467 RID: 1127
			[SerializeField]
			private Button _calibrateControllerButton;

			// Token: 0x04000468 RID: 1128
			[SerializeField]
			private Button _doneButton;

			// Token: 0x04000469 RID: 1129
			[SerializeField]
			private Button _restoreDefaultsButton;

			// Token: 0x0400046A RID: 1130
			[SerializeField]
			private Selectable _defaultSelection;

			// Token: 0x0400046B RID: 1131
			[SerializeField]
			private GameObject[] _fixedSelectableUIElements;

			// Token: 0x0400046C RID: 1132
			[SerializeField]
			private Image _mainBackgroundImage;
		}

		// Token: 0x020000A3 RID: 163
		private class InputActionSet
		{
			// Token: 0x170003B3 RID: 947
			// (get) Token: 0x06000905 RID: 2309 RVA: 0x0001A1BB File Offset: 0x000183BB
			public int actionId
			{
				get
				{
					return this._actionId;
				}
			}

			// Token: 0x170003B4 RID: 948
			// (get) Token: 0x06000906 RID: 2310 RVA: 0x0001A1C3 File Offset: 0x000183C3
			public AxisRange axisRange
			{
				get
				{
					return this._axisRange;
				}
			}

			// Token: 0x06000907 RID: 2311 RVA: 0x0001A1CB File Offset: 0x000183CB
			public InputActionSet(int actionId, AxisRange axisRange)
			{
				this._actionId = actionId;
				this._axisRange = axisRange;
			}

			// Token: 0x04000476 RID: 1142
			private int _actionId;

			// Token: 0x04000477 RID: 1143
			private AxisRange _axisRange;
		}

		// Token: 0x020000A4 RID: 164
		private class InputMapping
		{
			// Token: 0x170003B5 RID: 949
			// (get) Token: 0x06000908 RID: 2312 RVA: 0x0001A1E1 File Offset: 0x000183E1
			// (set) Token: 0x06000909 RID: 2313 RVA: 0x0001A1E9 File Offset: 0x000183E9
			public string actionName { get; private set; }

			// Token: 0x170003B6 RID: 950
			// (get) Token: 0x0600090A RID: 2314 RVA: 0x0001A1F2 File Offset: 0x000183F2
			// (set) Token: 0x0600090B RID: 2315 RVA: 0x0001A1FA File Offset: 0x000183FA
			public InputFieldInfo fieldInfo { get; private set; }

			// Token: 0x170003B7 RID: 951
			// (get) Token: 0x0600090C RID: 2316 RVA: 0x0001A203 File Offset: 0x00018403
			// (set) Token: 0x0600090D RID: 2317 RVA: 0x0001A20B File Offset: 0x0001840B
			public ControllerMap map { get; private set; }

			// Token: 0x170003B8 RID: 952
			// (get) Token: 0x0600090E RID: 2318 RVA: 0x0001A214 File Offset: 0x00018414
			// (set) Token: 0x0600090F RID: 2319 RVA: 0x0001A21C File Offset: 0x0001841C
			public ActionElementMap aem { get; private set; }

			// Token: 0x170003B9 RID: 953
			// (get) Token: 0x06000910 RID: 2320 RVA: 0x0001A225 File Offset: 0x00018425
			// (set) Token: 0x06000911 RID: 2321 RVA: 0x0001A22D File Offset: 0x0001842D
			public ControllerType controllerType { get; private set; }

			// Token: 0x170003BA RID: 954
			// (get) Token: 0x06000912 RID: 2322 RVA: 0x0001A236 File Offset: 0x00018436
			// (set) Token: 0x06000913 RID: 2323 RVA: 0x0001A23E File Offset: 0x0001843E
			public int controllerId { get; private set; }

			// Token: 0x170003BB RID: 955
			// (get) Token: 0x06000914 RID: 2324 RVA: 0x0001A247 File Offset: 0x00018447
			// (set) Token: 0x06000915 RID: 2325 RVA: 0x0001A24F File Offset: 0x0001844F
			public ControllerPollingInfo pollingInfo { get; set; }

			// Token: 0x170003BC RID: 956
			// (get) Token: 0x06000916 RID: 2326 RVA: 0x0001A258 File Offset: 0x00018458
			// (set) Token: 0x06000917 RID: 2327 RVA: 0x0001A260 File Offset: 0x00018460
			public ModifierKeyFlags modifierKeyFlags { get; set; }

			// Token: 0x170003BD RID: 957
			// (get) Token: 0x06000918 RID: 2328 RVA: 0x0001A26C File Offset: 0x0001846C
			public AxisRange axisRange
			{
				get
				{
					AxisRange axisRange = AxisRange.Positive;
					if (this.pollingInfo.elementType == ControllerElementType.Axis)
					{
						if (this.fieldInfo.axisRange == AxisRange.Full)
						{
							axisRange = AxisRange.Full;
						}
						else
						{
							axisRange = ((this.pollingInfo.axisPole == Pole.Positive) ? AxisRange.Positive : AxisRange.Negative);
						}
					}
					return axisRange;
				}
			}

			// Token: 0x170003BE RID: 958
			// (get) Token: 0x06000919 RID: 2329 RVA: 0x0001A2B4 File Offset: 0x000184B4
			public string elementName
			{
				get
				{
					if (this.controllerType == ControllerType.Keyboard)
					{
						return ControlMapper.GetLanguage().GetElementIdentifierName(this.pollingInfo.keyboardKey, this.modifierKeyFlags);
					}
					return ControlMapper.GetLanguage().GetElementIdentifierName(this.pollingInfo.controller, this.pollingInfo.elementIdentifierId, (this.pollingInfo.axisPole == Pole.Positive) ? AxisRange.Positive : AxisRange.Negative);
				}
			}

			// Token: 0x0600091A RID: 2330 RVA: 0x0001A322 File Offset: 0x00018522
			public InputMapping(string actionName, InputFieldInfo fieldInfo, ControllerMap map, ActionElementMap aem, ControllerType controllerType, int controllerId)
			{
				this.actionName = actionName;
				this.fieldInfo = fieldInfo;
				this.map = map;
				this.aem = aem;
				this.controllerType = controllerType;
				this.controllerId = controllerId;
			}

			// Token: 0x0600091B RID: 2331 RVA: 0x0001A357 File Offset: 0x00018557
			public ElementAssignment ToElementAssignment(ControllerPollingInfo pollingInfo)
			{
				this.pollingInfo = pollingInfo;
				return this.ToElementAssignment();
			}

			// Token: 0x0600091C RID: 2332 RVA: 0x0001A366 File Offset: 0x00018566
			public ElementAssignment ToElementAssignment(ControllerPollingInfo pollingInfo, ModifierKeyFlags modifierKeyFlags)
			{
				this.pollingInfo = pollingInfo;
				this.modifierKeyFlags = modifierKeyFlags;
				return this.ToElementAssignment();
			}

			// Token: 0x0600091D RID: 2333 RVA: 0x0001A37C File Offset: 0x0001857C
			public ElementAssignment ToElementAssignment()
			{
				return new ElementAssignment(this.controllerType, this.pollingInfo.elementType, this.pollingInfo.elementIdentifierId, this.axisRange, this.pollingInfo.keyboardKey, this.modifierKeyFlags, this.fieldInfo.actionId, (this.fieldInfo.axisRange == AxisRange.Negative) ? Pole.Negative : Pole.Positive, false, (this.aem != null) ? this.aem.id : (-1));
			}
		}

		// Token: 0x020000A5 RID: 165
		private class AxisCalibrator
		{
			// Token: 0x170003BF RID: 959
			// (get) Token: 0x0600091E RID: 2334 RVA: 0x0001A3FE File Offset: 0x000185FE
			public bool isValid
			{
				get
				{
					return this.axis != null;
				}
			}

			// Token: 0x0600091F RID: 2335 RVA: 0x0001A40C File Offset: 0x0001860C
			public AxisCalibrator(Joystick joystick, int axisIndex)
			{
				this.data = default(AxisCalibrationData);
				this.joystick = joystick;
				this.axisIndex = axisIndex;
				if (joystick != null && axisIndex >= 0 && joystick.axisCount > axisIndex)
				{
					this.axis = joystick.Axes[axisIndex];
					this.data = joystick.calibrationMap.GetAxis(axisIndex).GetData();
				}
				this.firstRun = true;
			}

			// Token: 0x06000920 RID: 2336 RVA: 0x0001A47C File Offset: 0x0001867C
			public void RecordMinMax()
			{
				if (this.axis == null)
				{
					return;
				}
				float valueRaw = this.axis.valueRaw;
				if (this.firstRun || valueRaw < this.data.min)
				{
					this.data.min = valueRaw;
				}
				if (this.firstRun || valueRaw > this.data.max)
				{
					this.data.max = valueRaw;
				}
				this.firstRun = false;
			}

			// Token: 0x06000921 RID: 2337 RVA: 0x0001A4E9 File Offset: 0x000186E9
			public void RecordZero()
			{
				if (this.axis == null)
				{
					return;
				}
				this.data.zero = this.axis.valueRaw;
			}

			// Token: 0x06000922 RID: 2338 RVA: 0x0001A50C File Offset: 0x0001870C
			public void Commit()
			{
				if (this.axis == null)
				{
					return;
				}
				AxisCalibration axisCalibration = this.joystick.calibrationMap.GetAxis(this.axisIndex);
				if (axisCalibration == null)
				{
					return;
				}
				if ((double)Mathf.Abs(this.data.max - this.data.min) < 0.1)
				{
					return;
				}
				axisCalibration.SetData(this.data);
			}

			// Token: 0x04000480 RID: 1152
			public AxisCalibrationData data;

			// Token: 0x04000481 RID: 1153
			public readonly Joystick joystick;

			// Token: 0x04000482 RID: 1154
			public readonly int axisIndex;

			// Token: 0x04000483 RID: 1155
			private Controller.Axis axis;

			// Token: 0x04000484 RID: 1156
			private bool firstRun;
		}

		// Token: 0x020000A6 RID: 166
		private class IndexedDictionary<TKey, TValue>
		{
			// Token: 0x170003C0 RID: 960
			// (get) Token: 0x06000923 RID: 2339 RVA: 0x0001A572 File Offset: 0x00018772
			public int Count
			{
				get
				{
					return this.list.Count;
				}
			}

			// Token: 0x06000924 RID: 2340 RVA: 0x0001A57F File Offset: 0x0001877F
			public IndexedDictionary()
			{
				this.list = new List<ControlMapper.IndexedDictionary<TKey, TValue>.Entry>();
			}

			// Token: 0x170003C1 RID: 961
			public TValue this[int index]
			{
				get
				{
					return this.list[index].value;
				}
			}

			// Token: 0x06000926 RID: 2342 RVA: 0x0001A5A8 File Offset: 0x000187A8
			public TValue Get(TKey key)
			{
				int num = this.IndexOfKey(key);
				if (num < 0)
				{
					throw new Exception("Key does not exist!");
				}
				return this.list[num].value;
			}

			// Token: 0x06000927 RID: 2343 RVA: 0x0001A5E0 File Offset: 0x000187E0
			public bool TryGet(TKey key, out TValue value)
			{
				value = default(TValue);
				int num = this.IndexOfKey(key);
				if (num < 0)
				{
					return false;
				}
				value = this.list[num].value;
				return true;
			}

			// Token: 0x06000928 RID: 2344 RVA: 0x0001A61A File Offset: 0x0001881A
			public void Add(TKey key, TValue value)
			{
				if (this.ContainsKey(key))
				{
					throw new Exception("Key " + key.ToString() + " is already in use!");
				}
				this.list.Add(new ControlMapper.IndexedDictionary<TKey, TValue>.Entry(key, value));
			}

			// Token: 0x06000929 RID: 2345 RVA: 0x0001A65C File Offset: 0x0001885C
			public int IndexOfKey(TKey key)
			{
				int count = this.list.Count;
				for (int i = 0; i < count; i++)
				{
					if (EqualityComparer<TKey>.Default.Equals(this.list[i].key, key))
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x0600092A RID: 2346 RVA: 0x0001A6A4 File Offset: 0x000188A4
			public bool ContainsKey(TKey key)
			{
				int count = this.list.Count;
				for (int i = 0; i < count; i++)
				{
					if (EqualityComparer<TKey>.Default.Equals(this.list[i].key, key))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600092B RID: 2347 RVA: 0x0001A6EA File Offset: 0x000188EA
			public void Clear()
			{
				this.list.Clear();
			}

			// Token: 0x04000485 RID: 1157
			private List<ControlMapper.IndexedDictionary<TKey, TValue>.Entry> list;

			// Token: 0x020000A7 RID: 167
			private class Entry
			{
				// Token: 0x0600092C RID: 2348 RVA: 0x0001A6F7 File Offset: 0x000188F7
				public Entry(TKey key, TValue value)
				{
					this.key = key;
					this.value = value;
				}

				// Token: 0x04000486 RID: 1158
				public TKey key;

				// Token: 0x04000487 RID: 1159
				public TValue value;
			}
		}

		// Token: 0x020000A8 RID: 168
		private enum LayoutElementSizeType
		{
			// Token: 0x04000489 RID: 1161
			MinSize,
			// Token: 0x0400048A RID: 1162
			PreferredSize
		}

		// Token: 0x020000A9 RID: 169
		private enum WindowType
		{
			// Token: 0x0400048C RID: 1164
			None,
			// Token: 0x0400048D RID: 1165
			ChooseJoystick,
			// Token: 0x0400048E RID: 1166
			JoystickAssignmentConflict,
			// Token: 0x0400048F RID: 1167
			ElementAssignment,
			// Token: 0x04000490 RID: 1168
			ElementAssignmentPrePolling,
			// Token: 0x04000491 RID: 1169
			ElementAssignmentPolling,
			// Token: 0x04000492 RID: 1170
			ElementAssignmentResult,
			// Token: 0x04000493 RID: 1171
			ElementAssignmentConflict,
			// Token: 0x04000494 RID: 1172
			Calibration,
			// Token: 0x04000495 RID: 1173
			CalibrateStep1,
			// Token: 0x04000496 RID: 1174
			CalibrateStep2
		}

		// Token: 0x020000AA RID: 170
		private class InputGrid
		{
			// Token: 0x0600092D RID: 2349 RVA: 0x0001A70D File Offset: 0x0001890D
			public InputGrid()
			{
				this.list = new ControlMapper.InputGridEntryList();
				this.groups = new List<GameObject>();
			}

			// Token: 0x0600092E RID: 2350 RVA: 0x0001A72B File Offset: 0x0001892B
			public void AddMapCategory(int mapCategoryId)
			{
				this.list.AddMapCategory(mapCategoryId);
			}

			// Token: 0x0600092F RID: 2351 RVA: 0x0001A739 File Offset: 0x00018939
			public void AddAction(int mapCategoryId, InputAction action, AxisRange axisRange)
			{
				this.list.AddAction(mapCategoryId, action, axisRange);
			}

			// Token: 0x06000930 RID: 2352 RVA: 0x0001A749 File Offset: 0x00018949
			public void AddActionCategory(int mapCategoryId, int actionCategoryId)
			{
				this.list.AddActionCategory(mapCategoryId, actionCategoryId);
			}

			// Token: 0x06000931 RID: 2353 RVA: 0x0001A758 File Offset: 0x00018958
			public void AddInputFieldSet(int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, GameObject fieldSetContainer)
			{
				this.list.AddInputFieldSet(mapCategoryId, action, axisRange, controllerType, fieldSetContainer);
			}

			// Token: 0x06000932 RID: 2354 RVA: 0x0001A76C File Offset: 0x0001896C
			public void AddInputField(int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, int fieldIndex, ControlMapper.GUIInputField inputField)
			{
				this.list.AddInputField(mapCategoryId, action, axisRange, controllerType, fieldIndex, inputField);
			}

			// Token: 0x06000933 RID: 2355 RVA: 0x0001A782 File Offset: 0x00018982
			public void AddGroup(GameObject group)
			{
				this.groups.Add(group);
			}

			// Token: 0x06000934 RID: 2356 RVA: 0x0001A790 File Offset: 0x00018990
			public void AddActionLabel(int mapCategoryId, int actionId, AxisRange axisRange, ControlMapper.GUILabel label)
			{
				this.list.AddActionLabel(mapCategoryId, actionId, axisRange, label);
			}

			// Token: 0x06000935 RID: 2357 RVA: 0x0001A7A2 File Offset: 0x000189A2
			public void AddActionCategoryLabel(int mapCategoryId, int actionCategoryId, ControlMapper.GUILabel label)
			{
				this.list.AddActionCategoryLabel(mapCategoryId, actionCategoryId, label);
			}

			// Token: 0x06000936 RID: 2358 RVA: 0x0001A7B2 File Offset: 0x000189B2
			public bool Contains(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
			{
				return this.list.Contains(mapCategoryId, actionId, axisRange, controllerType, fieldIndex);
			}

			// Token: 0x06000937 RID: 2359 RVA: 0x0001A7C6 File Offset: 0x000189C6
			public ControlMapper.GUIInputField GetGUIInputField(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
			{
				return this.list.GetGUIInputField(mapCategoryId, actionId, axisRange, controllerType, fieldIndex);
			}

			// Token: 0x06000938 RID: 2360 RVA: 0x0001A7DA File Offset: 0x000189DA
			public IEnumerable<ControlMapper.InputActionSet> GetActionSets(int mapCategoryId)
			{
				return this.list.GetActionSets(mapCategoryId);
			}

			// Token: 0x06000939 RID: 2361 RVA: 0x0001A7E8 File Offset: 0x000189E8
			public void SetColumnHeight(int mapCategoryId, float height)
			{
				this.list.SetColumnHeight(mapCategoryId, height);
			}

			// Token: 0x0600093A RID: 2362 RVA: 0x0001A7F7 File Offset: 0x000189F7
			public float GetColumnHeight(int mapCategoryId)
			{
				return this.list.GetColumnHeight(mapCategoryId);
			}

			// Token: 0x0600093B RID: 2363 RVA: 0x0001A805 File Offset: 0x00018A05
			public void SetFieldsActive(int mapCategoryId, bool state)
			{
				this.list.SetFieldsActive(mapCategoryId, state);
			}

			// Token: 0x0600093C RID: 2364 RVA: 0x0001A814 File Offset: 0x00018A14
			public void SetFieldLabel(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int index, string label)
			{
				this.list.SetLabel(mapCategoryId, actionId, axisRange, controllerType, index, label);
			}

			// Token: 0x0600093D RID: 2365 RVA: 0x0001A82C File Offset: 0x00018A2C
			public void PopulateField(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int controllerId, int index, int actionElementMapId, string label, bool invert)
			{
				this.list.PopulateField(mapCategoryId, actionId, axisRange, controllerType, controllerId, index, actionElementMapId, label, invert);
			}

			// Token: 0x0600093E RID: 2366 RVA: 0x0001A853 File Offset: 0x00018A53
			public void SetFixedFieldData(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int controllerId)
			{
				this.list.SetFixedFieldData(mapCategoryId, actionId, axisRange, controllerType, controllerId);
			}

			// Token: 0x0600093F RID: 2367 RVA: 0x0001A867 File Offset: 0x00018A67
			public void InitializeFields(int mapCategoryId)
			{
				this.list.InitializeFields(mapCategoryId);
			}

			// Token: 0x06000940 RID: 2368 RVA: 0x0001A875 File Offset: 0x00018A75
			public void Show(int mapCategoryId)
			{
				this.list.Show(mapCategoryId);
			}

			// Token: 0x06000941 RID: 2369 RVA: 0x0001A883 File Offset: 0x00018A83
			public void HideAll()
			{
				this.list.HideAll();
			}

			// Token: 0x06000942 RID: 2370 RVA: 0x0001A890 File Offset: 0x00018A90
			public void ClearLabels(int mapCategoryId)
			{
				this.list.ClearLabels(mapCategoryId);
			}

			// Token: 0x06000943 RID: 2371 RVA: 0x0001A8A0 File Offset: 0x00018AA0
			private void ClearGroups()
			{
				for (int i = 0; i < this.groups.Count; i++)
				{
					if (!(this.groups[i] == null))
					{
						global::UnityEngine.Object.Destroy(this.groups[i]);
					}
				}
			}

			// Token: 0x06000944 RID: 2372 RVA: 0x0001A8E8 File Offset: 0x00018AE8
			public void ClearAll()
			{
				this.ClearGroups();
				this.list.Clear();
			}

			// Token: 0x04000497 RID: 1175
			private ControlMapper.InputGridEntryList list;

			// Token: 0x04000498 RID: 1176
			private List<GameObject> groups;
		}

		// Token: 0x020000AB RID: 171
		private class InputGridEntryList
		{
			// Token: 0x06000945 RID: 2373 RVA: 0x0001A8FB File Offset: 0x00018AFB
			public InputGridEntryList()
			{
				this.entries = new ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.MapCategoryEntry>();
			}

			// Token: 0x06000946 RID: 2374 RVA: 0x0001A90E File Offset: 0x00018B0E
			public void AddMapCategory(int mapCategoryId)
			{
				if (mapCategoryId < 0)
				{
					return;
				}
				if (this.entries.ContainsKey(mapCategoryId))
				{
					return;
				}
				this.entries.Add(mapCategoryId, new ControlMapper.InputGridEntryList.MapCategoryEntry());
			}

			// Token: 0x06000947 RID: 2375 RVA: 0x0001A935 File Offset: 0x00018B35
			public void AddAction(int mapCategoryId, InputAction action, AxisRange axisRange)
			{
				this.AddActionEntry(mapCategoryId, action, axisRange);
			}

			// Token: 0x06000948 RID: 2376 RVA: 0x0001A944 File Offset: 0x00018B44
			private ControlMapper.InputGridEntryList.ActionEntry AddActionEntry(int mapCategoryId, InputAction action, AxisRange axisRange)
			{
				if (action == null)
				{
					return null;
				}
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return null;
				}
				return mapCategoryEntry.AddAction(action, axisRange);
			}

			// Token: 0x06000949 RID: 2377 RVA: 0x0001A970 File Offset: 0x00018B70
			public void AddActionLabel(int mapCategoryId, int actionId, AxisRange axisRange, ControlMapper.GUILabel label)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = mapCategoryEntry.GetActionEntry(actionId, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.SetLabel(label);
			}

			// Token: 0x0600094A RID: 2378 RVA: 0x0001A9A3 File Offset: 0x00018BA3
			public void AddActionCategory(int mapCategoryId, int actionCategoryId)
			{
				this.AddActionCategoryEntry(mapCategoryId, actionCategoryId);
			}

			// Token: 0x0600094B RID: 2379 RVA: 0x0001A9B0 File Offset: 0x00018BB0
			private ControlMapper.InputGridEntryList.ActionCategoryEntry AddActionCategoryEntry(int mapCategoryId, int actionCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return null;
				}
				return mapCategoryEntry.AddActionCategory(actionCategoryId);
			}

			// Token: 0x0600094C RID: 2380 RVA: 0x0001A9D8 File Offset: 0x00018BD8
			public void AddActionCategoryLabel(int mapCategoryId, int actionCategoryId, ControlMapper.GUILabel label)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				ControlMapper.InputGridEntryList.ActionCategoryEntry actionCategoryEntry = mapCategoryEntry.GetActionCategoryEntry(actionCategoryId);
				if (actionCategoryEntry == null)
				{
					return;
				}
				actionCategoryEntry.SetLabel(label);
			}

			// Token: 0x0600094D RID: 2381 RVA: 0x0001AA0C File Offset: 0x00018C0C
			public void AddInputFieldSet(int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, GameObject fieldSetContainer)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, action, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.AddInputFieldSet(controllerType, fieldSetContainer);
			}

			// Token: 0x0600094E RID: 2382 RVA: 0x0001AA34 File Offset: 0x00018C34
			public void AddInputField(int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, int fieldIndex, ControlMapper.GUIInputField inputField)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, action, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.AddInputField(controllerType, fieldIndex, inputField);
			}

			// Token: 0x0600094F RID: 2383 RVA: 0x0001AA5B File Offset: 0x00018C5B
			public bool Contains(int mapCategoryId, int actionId, AxisRange axisRange)
			{
				return this.GetActionEntry(mapCategoryId, actionId, axisRange) != null;
			}

			// Token: 0x06000950 RID: 2384 RVA: 0x0001AA6C File Offset: 0x00018C6C
			public bool Contains(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				return actionEntry != null && actionEntry.Contains(controllerType, fieldIndex);
			}

			// Token: 0x06000951 RID: 2385 RVA: 0x0001AA94 File Offset: 0x00018C94
			public void SetColumnHeight(int mapCategoryId, float height)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				mapCategoryEntry.columnHeight = height;
			}

			// Token: 0x06000952 RID: 2386 RVA: 0x0001AABC File Offset: 0x00018CBC
			public float GetColumnHeight(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return 0f;
				}
				return mapCategoryEntry.columnHeight;
			}

			// Token: 0x06000953 RID: 2387 RVA: 0x0001AAE8 File Offset: 0x00018CE8
			public ControlMapper.GUIInputField GetGUIInputField(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				if (actionEntry == null)
				{
					return null;
				}
				return actionEntry.GetGUIInputField(controllerType, fieldIndex);
			}

			// Token: 0x06000954 RID: 2388 RVA: 0x0001AB10 File Offset: 0x00018D10
			private ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(int mapCategoryId, int actionId, AxisRange axisRange)
			{
				if (actionId < 0)
				{
					return null;
				}
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return null;
				}
				return mapCategoryEntry.GetActionEntry(actionId, axisRange);
			}

			// Token: 0x06000955 RID: 2389 RVA: 0x0001AB3D File Offset: 0x00018D3D
			private ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(int mapCategoryId, InputAction action, AxisRange axisRange)
			{
				if (action == null)
				{
					return null;
				}
				return this.GetActionEntry(mapCategoryId, action.id, axisRange);
			}

			// Token: 0x06000956 RID: 2390 RVA: 0x0001AB52 File Offset: 0x00018D52
			public IEnumerable<ControlMapper.InputActionSet> GetActionSets(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					yield break;
				}
				List<ControlMapper.InputGridEntryList.ActionEntry> list = mapCategoryEntry.actionList;
				int count = ((list != null) ? list.Count : 0);
				int num;
				for (int i = 0; i < count; i = num + 1)
				{
					yield return list[i].actionSet;
					num = i;
				}
				yield break;
			}

			// Token: 0x06000957 RID: 2391 RVA: 0x0001AB6C File Offset: 0x00018D6C
			public void SetFieldsActive(int mapCategoryId, bool state)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				List<ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
				int num = ((actionList != null) ? actionList.Count : 0);
				for (int i = 0; i < num; i++)
				{
					actionList[i].SetFieldsActive(state);
				}
			}

			// Token: 0x06000958 RID: 2392 RVA: 0x0001ABB8 File Offset: 0x00018DB8
			public void SetLabel(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int index, string label)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.SetFieldLabel(controllerType, index, label);
			}

			// Token: 0x06000959 RID: 2393 RVA: 0x0001ABE0 File Offset: 0x00018DE0
			public void PopulateField(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int controllerId, int index, int actionElementMapId, string label, bool invert)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.PopulateField(controllerType, controllerId, index, actionElementMapId, label, invert);
			}

			// Token: 0x0600095A RID: 2394 RVA: 0x0001AC10 File Offset: 0x00018E10
			public void SetFixedFieldData(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int controllerId)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.SetFixedFieldData(controllerType, controllerId);
			}

			// Token: 0x0600095B RID: 2395 RVA: 0x0001AC38 File Offset: 0x00018E38
			public void InitializeFields(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				List<ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
				int num = ((actionList != null) ? actionList.Count : 0);
				for (int i = 0; i < num; i++)
				{
					actionList[i].Initialize();
				}
			}

			// Token: 0x0600095C RID: 2396 RVA: 0x0001AC84 File Offset: 0x00018E84
			public void Show(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				mapCategoryEntry.SetAllActive(true);
			}

			// Token: 0x0600095D RID: 2397 RVA: 0x0001ACAC File Offset: 0x00018EAC
			public void HideAll()
			{
				for (int i = 0; i < this.entries.Count; i++)
				{
					this.entries[i].SetAllActive(false);
				}
			}

			// Token: 0x0600095E RID: 2398 RVA: 0x0001ACE4 File Offset: 0x00018EE4
			public void ClearLabels(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				List<ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
				int num = ((actionList != null) ? actionList.Count : 0);
				for (int i = 0; i < num; i++)
				{
					actionList[i].ClearLabels();
				}
			}

			// Token: 0x0600095F RID: 2399 RVA: 0x0001AD2E File Offset: 0x00018F2E
			public void Clear()
			{
				this.entries.Clear();
			}

			// Token: 0x04000499 RID: 1177
			private ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.MapCategoryEntry> entries;

			// Token: 0x020000AC RID: 172
			private class MapCategoryEntry
			{
				// Token: 0x170003C2 RID: 962
				// (get) Token: 0x06000960 RID: 2400 RVA: 0x0001AD3B File Offset: 0x00018F3B
				public List<ControlMapper.InputGridEntryList.ActionEntry> actionList
				{
					get
					{
						return this._actionList;
					}
				}

				// Token: 0x170003C3 RID: 963
				// (get) Token: 0x06000961 RID: 2401 RVA: 0x0001AD43 File Offset: 0x00018F43
				public ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.ActionCategoryEntry> actionCategoryList
				{
					get
					{
						return this._actionCategoryList;
					}
				}

				// Token: 0x170003C4 RID: 964
				// (get) Token: 0x06000962 RID: 2402 RVA: 0x0001AD4B File Offset: 0x00018F4B
				// (set) Token: 0x06000963 RID: 2403 RVA: 0x0001AD53 File Offset: 0x00018F53
				public float columnHeight
				{
					get
					{
						return this._columnHeight;
					}
					set
					{
						this._columnHeight = value;
					}
				}

				// Token: 0x06000964 RID: 2404 RVA: 0x0001AD5C File Offset: 0x00018F5C
				public MapCategoryEntry()
				{
					this._actionList = new List<ControlMapper.InputGridEntryList.ActionEntry>();
					this._actionCategoryList = new ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.ActionCategoryEntry>();
				}

				// Token: 0x06000965 RID: 2405 RVA: 0x0001AD7C File Offset: 0x00018F7C
				public ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(int actionId, AxisRange axisRange)
				{
					int num = this.IndexOfActionEntry(actionId, axisRange);
					if (num < 0)
					{
						return null;
					}
					return this._actionList[num];
				}

				// Token: 0x06000966 RID: 2406 RVA: 0x0001ADA4 File Offset: 0x00018FA4
				public int IndexOfActionEntry(int actionId, AxisRange axisRange)
				{
					int count = this._actionList.Count;
					for (int i = 0; i < count; i++)
					{
						if (this._actionList[i].Matches(actionId, axisRange))
						{
							return i;
						}
					}
					return -1;
				}

				// Token: 0x06000967 RID: 2407 RVA: 0x0001ADE1 File Offset: 0x00018FE1
				public bool ContainsActionEntry(int actionId, AxisRange axisRange)
				{
					return this.IndexOfActionEntry(actionId, axisRange) >= 0;
				}

				// Token: 0x06000968 RID: 2408 RVA: 0x0001ADF4 File Offset: 0x00018FF4
				public ControlMapper.InputGridEntryList.ActionEntry AddAction(InputAction action, AxisRange axisRange)
				{
					if (action == null)
					{
						return null;
					}
					if (this.ContainsActionEntry(action.id, axisRange))
					{
						return null;
					}
					this._actionList.Add(new ControlMapper.InputGridEntryList.ActionEntry(action, axisRange));
					return this._actionList[this._actionList.Count - 1];
				}

				// Token: 0x06000969 RID: 2409 RVA: 0x0001AE41 File Offset: 0x00019041
				public ControlMapper.InputGridEntryList.ActionCategoryEntry GetActionCategoryEntry(int actionCategoryId)
				{
					if (!this._actionCategoryList.ContainsKey(actionCategoryId))
					{
						return null;
					}
					return this._actionCategoryList.Get(actionCategoryId);
				}

				// Token: 0x0600096A RID: 2410 RVA: 0x0001AE5F File Offset: 0x0001905F
				public ControlMapper.InputGridEntryList.ActionCategoryEntry AddActionCategory(int actionCategoryId)
				{
					if (actionCategoryId < 0)
					{
						return null;
					}
					if (this._actionCategoryList.ContainsKey(actionCategoryId))
					{
						return null;
					}
					this._actionCategoryList.Add(actionCategoryId, new ControlMapper.InputGridEntryList.ActionCategoryEntry(actionCategoryId));
					return this._actionCategoryList.Get(actionCategoryId);
				}

				// Token: 0x0600096B RID: 2411 RVA: 0x0001AE98 File Offset: 0x00019098
				public void SetAllActive(bool state)
				{
					for (int i = 0; i < this._actionCategoryList.Count; i++)
					{
						this._actionCategoryList[i].SetActive(state);
					}
					for (int j = 0; j < this._actionList.Count; j++)
					{
						this._actionList[j].SetActive(state);
					}
				}

				// Token: 0x0400049A RID: 1178
				private List<ControlMapper.InputGridEntryList.ActionEntry> _actionList;

				// Token: 0x0400049B RID: 1179
				private ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.ActionCategoryEntry> _actionCategoryList;

				// Token: 0x0400049C RID: 1180
				private float _columnHeight;
			}

			// Token: 0x020000AD RID: 173
			private class ActionEntry
			{
				// Token: 0x0600096C RID: 2412 RVA: 0x0001AEF5 File Offset: 0x000190F5
				public ActionEntry(InputAction action, AxisRange axisRange)
				{
					this.action = action;
					this.axisRange = axisRange;
					this.actionSet = new ControlMapper.InputActionSet(action.id, axisRange);
					this.fieldSets = new ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.FieldSet>();
				}

				// Token: 0x0600096D RID: 2413 RVA: 0x0001AF28 File Offset: 0x00019128
				public void SetLabel(ControlMapper.GUILabel label)
				{
					this.label = label;
				}

				// Token: 0x0600096E RID: 2414 RVA: 0x0001AF31 File Offset: 0x00019131
				public bool Matches(int actionId, AxisRange axisRange)
				{
					return this.action.id == actionId && this.axisRange == axisRange;
				}

				// Token: 0x0600096F RID: 2415 RVA: 0x0001AF4F File Offset: 0x0001914F
				public void AddInputFieldSet(ControllerType controllerType, GameObject fieldSetContainer)
				{
					if (this.fieldSets.ContainsKey((int)controllerType))
					{
						return;
					}
					this.fieldSets.Add((int)controllerType, new ControlMapper.InputGridEntryList.FieldSet(fieldSetContainer));
				}

				// Token: 0x06000970 RID: 2416 RVA: 0x0001AF74 File Offset: 0x00019174
				public void AddInputField(ControllerType controllerType, int fieldIndex, ControlMapper.GUIInputField inputField)
				{
					if (!this.fieldSets.ContainsKey((int)controllerType))
					{
						return;
					}
					ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets.Get((int)controllerType);
					if (fieldSet.fields.ContainsKey(fieldIndex))
					{
						return;
					}
					fieldSet.fields.Add(fieldIndex, inputField);
				}

				// Token: 0x06000971 RID: 2417 RVA: 0x0001AFBC File Offset: 0x000191BC
				public ControlMapper.GUIInputField GetGUIInputField(ControllerType controllerType, int fieldIndex)
				{
					if (!this.fieldSets.ContainsKey((int)controllerType))
					{
						return null;
					}
					if (!this.fieldSets.Get((int)controllerType).fields.ContainsKey(fieldIndex))
					{
						return null;
					}
					return this.fieldSets.Get((int)controllerType).fields.Get(fieldIndex);
				}

				// Token: 0x06000972 RID: 2418 RVA: 0x0001B00B File Offset: 0x0001920B
				public bool Contains(ControllerType controllerType, int fieldId)
				{
					return this.fieldSets.ContainsKey((int)controllerType) && this.fieldSets.Get((int)controllerType).fields.ContainsKey(fieldId);
				}

				// Token: 0x06000973 RID: 2419 RVA: 0x0001B03C File Offset: 0x0001923C
				public void SetFieldLabel(ControllerType controllerType, int index, string label)
				{
					if (!this.fieldSets.ContainsKey((int)controllerType))
					{
						return;
					}
					if (!this.fieldSets.Get((int)controllerType).fields.ContainsKey(index))
					{
						return;
					}
					this.fieldSets.Get((int)controllerType).fields.Get(index).SetLabel(label);
				}

				// Token: 0x06000974 RID: 2420 RVA: 0x0001B090 File Offset: 0x00019290
				public void PopulateField(ControllerType controllerType, int controllerId, int index, int actionElementMapId, string label, bool invert)
				{
					if (!this.fieldSets.ContainsKey((int)controllerType))
					{
						return;
					}
					if (!this.fieldSets.Get((int)controllerType).fields.ContainsKey(index))
					{
						return;
					}
					ControlMapper.GUIInputField guiinputField = this.fieldSets.Get((int)controllerType).fields.Get(index);
					guiinputField.SetLabel(label);
					guiinputField.actionElementMapId = actionElementMapId;
					guiinputField.controllerId = controllerId;
					if (guiinputField.hasToggle)
					{
						guiinputField.toggle.SetInteractible(true, false);
						guiinputField.toggle.SetToggleState(invert);
						guiinputField.toggle.actionElementMapId = actionElementMapId;
					}
				}

				// Token: 0x06000975 RID: 2421 RVA: 0x0001B124 File Offset: 0x00019324
				public void SetFixedFieldData(ControllerType controllerType, int controllerId)
				{
					if (!this.fieldSets.ContainsKey((int)controllerType))
					{
						return;
					}
					ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets.Get((int)controllerType);
					int count = fieldSet.fields.Count;
					for (int i = 0; i < count; i++)
					{
						fieldSet.fields[i].controllerId = controllerId;
					}
				}

				// Token: 0x06000976 RID: 2422 RVA: 0x0001B178 File Offset: 0x00019378
				public void Initialize()
				{
					for (int i = 0; i < this.fieldSets.Count; i++)
					{
						ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[i];
						int count = fieldSet.fields.Count;
						for (int j = 0; j < count; j++)
						{
							ControlMapper.GUIInputField guiinputField = fieldSet.fields[j];
							if (guiinputField.hasToggle)
							{
								guiinputField.toggle.SetInteractible(false, false);
								guiinputField.toggle.SetToggleState(false);
								guiinputField.toggle.actionElementMapId = -1;
							}
							guiinputField.SetLabel("");
							guiinputField.actionElementMapId = -1;
							guiinputField.controllerId = -1;
						}
					}
				}

				// Token: 0x06000977 RID: 2423 RVA: 0x0001B224 File Offset: 0x00019424
				public void SetActive(bool state)
				{
					if (this.label != null)
					{
						this.label.SetActive(state);
					}
					int count = this.fieldSets.Count;
					for (int i = 0; i < count; i++)
					{
						this.fieldSets[i].groupContainer.SetActive(state);
					}
				}

				// Token: 0x06000978 RID: 2424 RVA: 0x0001B274 File Offset: 0x00019474
				public void ClearLabels()
				{
					for (int i = 0; i < this.fieldSets.Count; i++)
					{
						ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[i];
						int count = fieldSet.fields.Count;
						for (int j = 0; j < count; j++)
						{
							fieldSet.fields[j].SetLabel("");
						}
					}
				}

				// Token: 0x06000979 RID: 2425 RVA: 0x0001B2D4 File Offset: 0x000194D4
				public void SetFieldsActive(bool state)
				{
					for (int i = 0; i < this.fieldSets.Count; i++)
					{
						ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[i];
						int count = fieldSet.fields.Count;
						for (int j = 0; j < count; j++)
						{
							ControlMapper.GUIInputField guiinputField = fieldSet.fields[j];
							guiinputField.SetInteractible(state, false);
							if (guiinputField.hasToggle && (!state || guiinputField.toggle.actionElementMapId >= 0))
							{
								guiinputField.toggle.SetInteractible(state, false);
							}
						}
					}
				}

				// Token: 0x0400049D RID: 1181
				private ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.FieldSet> fieldSets;

				// Token: 0x0400049E RID: 1182
				public ControlMapper.GUILabel label;

				// Token: 0x0400049F RID: 1183
				public readonly InputAction action;

				// Token: 0x040004A0 RID: 1184
				public readonly AxisRange axisRange;

				// Token: 0x040004A1 RID: 1185
				public readonly ControlMapper.InputActionSet actionSet;
			}

			// Token: 0x020000AE RID: 174
			private class FieldSet
			{
				// Token: 0x0600097A RID: 2426 RVA: 0x0001B35C File Offset: 0x0001955C
				public FieldSet(GameObject groupContainer)
				{
					this.groupContainer = groupContainer;
					this.fields = new ControlMapper.IndexedDictionary<int, ControlMapper.GUIInputField>();
				}

				// Token: 0x040004A2 RID: 1186
				public readonly GameObject groupContainer;

				// Token: 0x040004A3 RID: 1187
				public readonly ControlMapper.IndexedDictionary<int, ControlMapper.GUIInputField> fields;
			}

			// Token: 0x020000AF RID: 175
			private class ActionCategoryEntry
			{
				// Token: 0x0600097B RID: 2427 RVA: 0x0001B376 File Offset: 0x00019576
				public ActionCategoryEntry(int actionCategoryId)
				{
					this.actionCategoryId = actionCategoryId;
				}

				// Token: 0x0600097C RID: 2428 RVA: 0x0001B385 File Offset: 0x00019585
				public void SetLabel(ControlMapper.GUILabel label)
				{
					this.label = label;
				}

				// Token: 0x0600097D RID: 2429 RVA: 0x0001B38E File Offset: 0x0001958E
				public void SetActive(bool state)
				{
					if (this.label != null)
					{
						this.label.SetActive(state);
					}
				}

				// Token: 0x040004A4 RID: 1188
				public readonly int actionCategoryId;

				// Token: 0x040004A5 RID: 1189
				public ControlMapper.GUILabel label;
			}
		}

		// Token: 0x020000B1 RID: 177
		private class WindowManager
		{
			// Token: 0x170003C7 RID: 967
			// (get) Token: 0x06000986 RID: 2438 RVA: 0x0001B4E0 File Offset: 0x000196E0
			public bool isWindowOpen
			{
				get
				{
					for (int i = this.windows.Count - 1; i >= 0; i--)
					{
						if (!(this.windows[i] == null))
						{
							return true;
						}
					}
					return false;
				}
			}

			// Token: 0x170003C8 RID: 968
			// (get) Token: 0x06000987 RID: 2439 RVA: 0x0001B51C File Offset: 0x0001971C
			public Window topWindow
			{
				get
				{
					for (int i = this.windows.Count - 1; i >= 0; i--)
					{
						if (!(this.windows[i] == null))
						{
							return this.windows[i];
						}
					}
					return null;
				}
			}

			// Token: 0x06000988 RID: 2440 RVA: 0x0001B564 File Offset: 0x00019764
			public WindowManager(GameObject windowPrefab, GameObject faderPrefab, Transform parent)
			{
				this.windowPrefab = windowPrefab;
				this.parent = parent;
				this.windows = new List<Window>();
				this.fader = global::UnityEngine.Object.Instantiate<GameObject>(faderPrefab);
				this.fader.transform.SetParent(parent, false);
				this.fader.GetComponent<RectTransform>().localScale = Vector2.one;
				this.SetFaderActive(false);
			}

			// Token: 0x06000989 RID: 2441 RVA: 0x0001B5CF File Offset: 0x000197CF
			public Window OpenWindow(string name, int width, int height)
			{
				Window window = this.InstantiateWindow(name, width, height);
				this.UpdateFader();
				return window;
			}

			// Token: 0x0600098A RID: 2442 RVA: 0x0001B5E0 File Offset: 0x000197E0
			public Window OpenWindow(GameObject windowPrefab, string name)
			{
				if (windowPrefab == null)
				{
					Debug.LogError("Rewired Control Mapper: Window Prefab is null!");
					return null;
				}
				Window window = this.InstantiateWindow(name, windowPrefab);
				this.UpdateFader();
				return window;
			}

			// Token: 0x0600098B RID: 2443 RVA: 0x0001B608 File Offset: 0x00019808
			public void CloseTop()
			{
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (!(this.windows[i] == null))
					{
						this.DestroyWindow(this.windows[i]);
						this.windows.RemoveAt(i);
						break;
					}
					this.windows.RemoveAt(i);
				}
				this.UpdateFader();
			}

			// Token: 0x0600098C RID: 2444 RVA: 0x0001B675 File Offset: 0x00019875
			public void CloseWindow(int windowId)
			{
				this.CloseWindow(this.GetWindow(windowId));
			}

			// Token: 0x0600098D RID: 2445 RVA: 0x0001B684 File Offset: 0x00019884
			public void CloseWindow(Window window)
			{
				if (window == null)
				{
					return;
				}
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (this.windows[i] == null)
					{
						this.windows.RemoveAt(i);
					}
					else if (!(this.windows[i] != window))
					{
						this.DestroyWindow(this.windows[i]);
						this.windows.RemoveAt(i);
						break;
					}
				}
				this.UpdateFader();
				this.FocusTopWindow();
			}

			// Token: 0x0600098E RID: 2446 RVA: 0x0001B718 File Offset: 0x00019918
			public void CloseAll()
			{
				this.SetFaderActive(false);
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (this.windows[i] == null)
					{
						this.windows.RemoveAt(i);
					}
					else
					{
						this.DestroyWindow(this.windows[i]);
						this.windows.RemoveAt(i);
					}
				}
				this.UpdateFader();
			}

			// Token: 0x0600098F RID: 2447 RVA: 0x0001B78C File Offset: 0x0001998C
			public void CancelAll()
			{
				if (!this.isWindowOpen)
				{
					return;
				}
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (!(this.windows[i] == null))
					{
						this.windows[i].Cancel();
					}
				}
				this.CloseAll();
			}

			// Token: 0x06000990 RID: 2448 RVA: 0x0001B7E8 File Offset: 0x000199E8
			public Window GetWindow(int windowId)
			{
				if (windowId < 0)
				{
					return null;
				}
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (!(this.windows[i] == null) && this.windows[i].id == windowId)
					{
						return this.windows[i];
					}
				}
				return null;
			}

			// Token: 0x06000991 RID: 2449 RVA: 0x0001B849 File Offset: 0x00019A49
			public bool IsFocused(int windowId)
			{
				return windowId >= 0 && !(this.topWindow == null) && this.topWindow.id == windowId;
			}

			// Token: 0x06000992 RID: 2450 RVA: 0x0001B86F File Offset: 0x00019A6F
			public void Focus(int windowId)
			{
				this.Focus(this.GetWindow(windowId));
			}

			// Token: 0x06000993 RID: 2451 RVA: 0x0001B87E File Offset: 0x00019A7E
			public void Focus(Window window)
			{
				if (window == null)
				{
					return;
				}
				window.TakeInputFocus();
				this.DefocusOtherWindows(window.id);
			}

			// Token: 0x06000994 RID: 2452 RVA: 0x0001B89C File Offset: 0x00019A9C
			private void DefocusOtherWindows(int focusedWindowId)
			{
				if (focusedWindowId < 0)
				{
					return;
				}
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (!(this.windows[i] == null) && this.windows[i].id != focusedWindowId)
					{
						this.windows[i].Disable();
					}
				}
			}

			// Token: 0x06000995 RID: 2453 RVA: 0x0001B900 File Offset: 0x00019B00
			private void UpdateFader()
			{
				if (!this.isWindowOpen)
				{
					this.SetFaderActive(false);
					return;
				}
				if (this.topWindow.transform.parent == null)
				{
					return;
				}
				this.SetFaderActive(true);
				this.fader.transform.SetAsLastSibling();
				int siblingIndex = this.topWindow.transform.GetSiblingIndex();
				this.fader.transform.SetSiblingIndex(siblingIndex);
			}

			// Token: 0x06000996 RID: 2454 RVA: 0x0001B96F File Offset: 0x00019B6F
			private void FocusTopWindow()
			{
				if (this.topWindow == null)
				{
					return;
				}
				this.topWindow.TakeInputFocus();
			}

			// Token: 0x06000997 RID: 2455 RVA: 0x0001B98B File Offset: 0x00019B8B
			private void SetFaderActive(bool state)
			{
				this.fader.SetActive(state);
			}

			// Token: 0x06000998 RID: 2456 RVA: 0x0001B99C File Offset: 0x00019B9C
			private Window InstantiateWindow(string name, int width, int height)
			{
				if (string.IsNullOrEmpty(name))
				{
					name = "Window";
				}
				GameObject gameObject = UITools.InstantiateGUIObject<Window>(this.windowPrefab, this.parent, name);
				if (gameObject == null)
				{
					return null;
				}
				Window component = gameObject.GetComponent<Window>();
				if (component != null)
				{
					component.Initialize(this.GetNewId(), new Func<int, bool>(this.IsFocused));
					this.windows.Add(component);
					component.SetSize(width, height);
				}
				return component;
			}

			// Token: 0x06000999 RID: 2457 RVA: 0x0001BA14 File Offset: 0x00019C14
			private Window InstantiateWindow(string name, GameObject windowPrefab)
			{
				if (string.IsNullOrEmpty(name))
				{
					name = "Window";
				}
				if (windowPrefab == null)
				{
					return null;
				}
				GameObject gameObject = UITools.InstantiateGUIObject<Window>(windowPrefab, this.parent, name);
				if (gameObject == null)
				{
					return null;
				}
				Window component = gameObject.GetComponent<Window>();
				if (component != null)
				{
					component.Initialize(this.GetNewId(), new Func<int, bool>(this.IsFocused));
					this.windows.Add(component);
				}
				return component;
			}

			// Token: 0x0600099A RID: 2458 RVA: 0x0001BA89 File Offset: 0x00019C89
			private void DestroyWindow(Window window)
			{
				if (window == null)
				{
					return;
				}
				global::UnityEngine.Object.Destroy(window.gameObject);
			}

			// Token: 0x0600099B RID: 2459 RVA: 0x0001BAA0 File Offset: 0x00019CA0
			private int GetNewId()
			{
				int num = this.idCounter;
				this.idCounter++;
				return num;
			}

			// Token: 0x0600099C RID: 2460 RVA: 0x0001BAB6 File Offset: 0x00019CB6
			public void ClearCompletely()
			{
				this.CloseAll();
				if (this.fader != null)
				{
					global::UnityEngine.Object.Destroy(this.fader);
				}
			}

			// Token: 0x040004AF RID: 1199
			private List<Window> windows;

			// Token: 0x040004B0 RID: 1200
			private GameObject windowPrefab;

			// Token: 0x040004B1 RID: 1201
			private Transform parent;

			// Token: 0x040004B2 RID: 1202
			private GameObject fader;

			// Token: 0x040004B3 RID: 1203
			private int idCounter;
		}
	}
}

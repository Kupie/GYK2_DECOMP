using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LazyBearTechnology
{
	// Token: 0x0200014A RID: 330
	public class GamepadNavigationItem : MonoBehaviour
	{
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060006E3 RID: 1763 RVA: 0x00023AB4 File Offset: 0x00021CB4
		// (remove) Token: 0x060006E4 RID: 1764 RVA: 0x00023AE8 File Offset: 0x00021CE8
		public static event Action<GamepadNavigationItem> OnFocusStatic;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060006E5 RID: 1765 RVA: 0x00023B1C File Offset: 0x00021D1C
		// (remove) Token: 0x060006E6 RID: 1766 RVA: 0x00023B50 File Offset: 0x00021D50
		public static event Action<GamepadNavigationItem> OnUnfocusStatic;

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x00023B83 File Offset: 0x00021D83
		// (set) Token: 0x060006E8 RID: 1768 RVA: 0x00023BC0 File Offset: 0x00021DC0
		public RectTransform FocusRectTransform
		{
			get
			{
				if (this.focusRectTransform == null)
				{
					this.focusRectTransform = ((this.focusFrame != null) ? this.focusFrame.GetComponent<RectTransform>() : base.GetComponent<RectTransform>());
				}
				return this.focusRectTransform;
			}
			set
			{
				this.focusRectTransform = value;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x00023BC9 File Offset: 0x00021DC9
		public UnityEvent OnFocus
		{
			get
			{
				return this.onFocus;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x00023BD1 File Offset: 0x00021DD1
		public UnityEvent OnUnfocus
		{
			get
			{
				return this.onUnfocus;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x00023BD9 File Offset: 0x00021DD9
		public UnityEvent OnSelect
		{
			get
			{
				return this.onSelect;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x00023BE1 File Offset: 0x00021DE1
		public bool IsFocused
		{
			get
			{
				return this.isFocused;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x00023BE9 File Offset: 0x00021DE9
		// (set) Token: 0x060006EE RID: 1774 RVA: 0x00023BF1 File Offset: 0x00021DF1
		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				this.active = value;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x00023BFA File Offset: 0x00021DFA
		// (set) Token: 0x060006F0 RID: 1776 RVA: 0x00023C02 File Offset: 0x00021E02
		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x00023C0B File Offset: 0x00021E0B
		public Vector2 Pos
		{
			get
			{
				return base.transform.position;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x00023C1D File Offset: 0x00021E1D
		public GamepadNavigationController Controller
		{
			get
			{
				return this.controller;
			}
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00023C25 File Offset: 0x00021E25
		public void Init(int index, GamepadNavigationController controller, float guiScale)
		{
			this.index = index;
			this.controller = controller;
			this.halfSize = new Vector2(1f, 1f);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00023C4A File Offset: 0x00021E4A
		public void Focus()
		{
			if (this.isFocused)
			{
				return;
			}
			this.SetFocus(true);
			UnityEvent unityEvent = this.onFocus;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			Action<GamepadNavigationItem> onFocusStatic = GamepadNavigationItem.OnFocusStatic;
			if (onFocusStatic == null)
			{
				return;
			}
			onFocusStatic(this);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x00023C7D File Offset: 0x00021E7D
		public void Unfocus()
		{
			if (!this.isFocused)
			{
				return;
			}
			this.SetFocus(false);
			UnityEvent unityEvent = this.onUnfocus;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			Action<GamepadNavigationItem> onUnfocusStatic = GamepadNavigationItem.OnUnfocusStatic;
			if (onUnfocusStatic == null)
			{
				return;
			}
			onUnfocusStatic(this);
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00023CB0 File Offset: 0x00021EB0
		public void Select()
		{
			if (this.configuredForButton != null && !this.configuredForButton.interactable)
			{
				return;
			}
			UnityEvent unityEvent = this.onSelect;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00023CDE File Offset: 0x00021EDE
		public void SetFocus(bool focused)
		{
			this.isFocused = focused;
			if (this.focusFrame != null)
			{
				this.focusFrame.SetActive(focused);
			}
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00023D04 File Offset: 0x00021F04
		public float CalcDistToCurrentPos(Vector2 currentPos, GUIDirection direction)
		{
			Vector2 vector = this.Pos;
			switch (direction)
			{
			case GUIDirection.Left:
				vector.x += this.halfSize.x;
				break;
			case GUIDirection.Right:
				vector.x -= this.halfSize.x;
				break;
			case GUIDirection.Up:
				vector.y -= this.halfSize.y;
				break;
			case GUIDirection.Down:
				vector.y += this.halfSize.y;
				break;
			}
			vector -= currentPos;
			return vector.magnitude;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00023DA0 File Offset: 0x00021FA0
		public bool CorrectDirection(Vector2 otherPos, GUIDirection direction)
		{
			Vector2 vector = this.Pos - otherPos;
			if (vector.magnitude.EqualsTo(0f, 1E-05f))
			{
				return false;
			}
			switch (direction)
			{
			case GUIDirection.Left:
				return vector.x < -this.halfSize.x;
			case GUIDirection.Right:
				return vector.x > this.halfSize.x;
			case GUIDirection.Up:
				return vector.y > this.halfSize.y;
			case GUIDirection.Down:
				return vector.y < -this.halfSize.y;
			default:
				return false;
			}
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00023E40 File Offset: 0x00022040
		public bool CorrectGrid(Vector2 otherPos, GUIDirection direction)
		{
			Vector2 vector = this.Pos - otherPos;
			if (direction > GUIDirection.Right)
			{
				return direction - GUIDirection.Up <= 1 && Mathf.Abs(vector.y) > Mathf.Abs(vector.x) && Mathf.Abs(vector.x) <= this.halfSize.x;
			}
			return Mathf.Abs(vector.x) > Mathf.Abs(vector.y) && Mathf.Abs(vector.y) <= this.halfSize.y;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00023ED4 File Offset: 0x000220D4
		public GamepadNavigationItem GetCustomDirectionItem(GUIDirection dir)
		{
			GamepadNavigationItem gamepadNavigationItem = null;
			switch (dir)
			{
			case GUIDirection.Left:
				gamepadNavigationItem = this.leftItem;
				break;
			case GUIDirection.Right:
				gamepadNavigationItem = this.rightItem;
				break;
			case GUIDirection.Up:
				gamepadNavigationItem = this.upItem;
				break;
			case GUIDirection.Down:
				gamepadNavigationItem = this.downItem;
				break;
			}
			if (!(gamepadNavigationItem == null) && gamepadNavigationItem.isActiveAndEnabled && gamepadNavigationItem.Active)
			{
				return gamepadNavigationItem;
			}
			return null;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00023F3C File Offset: 0x0002213C
		public void SetCustomDirectionItem(GUIDirection dir, GamepadNavigationItem customItem, bool setAlsoBackwardsCustomDirection = false)
		{
			switch (dir)
			{
			case GUIDirection.Left:
				this.leftItem = customItem;
				if (setAlsoBackwardsCustomDirection && customItem != null)
				{
					customItem.SetCustomDirectionItem(GUIDirection.Right, this, false);
					return;
				}
				break;
			case GUIDirection.Right:
				this.rightItem = customItem;
				if (setAlsoBackwardsCustomDirection && customItem != null)
				{
					customItem.SetCustomDirectionItem(GUIDirection.Left, this, false);
					return;
				}
				break;
			case GUIDirection.Up:
				this.upItem = customItem;
				if (setAlsoBackwardsCustomDirection && customItem != null)
				{
					customItem.SetCustomDirectionItem(GUIDirection.Down, this, false);
					return;
				}
				break;
			case GUIDirection.Down:
				this.downItem = customItem;
				if (setAlsoBackwardsCustomDirection && customItem != null)
				{
					customItem.SetCustomDirectionItem(GUIDirection.Up, this, false);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00023FD3 File Offset: 0x000221D3
		public void ResetCustomDirections()
		{
			this.leftItem = null;
			this.rightItem = null;
			this.upItem = null;
			this.downItem = null;
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00023FF4 File Offset: 0x000221F4
		public void SyncOnSelectWithButton()
		{
			Button component = base.GetComponent<Button>();
			if (component == null)
			{
				Debug.LogError("SyncOnSelectWithButton() couldn't find a button [GameObject name: " + base.name + "]", this);
				return;
			}
			this.onSelect = component.onClick;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0002403C File Offset: 0x0002223C
		public void SetCallbacks(UnityAction onFocus, UnityAction onUnfocus, UnityAction onSelect)
		{
			this.onFocus.RemoveAllListeners();
			this.onUnfocus.RemoveAllListeners();
			this.onSelect.RemoveAllListeners();
			if (onFocus != null)
			{
				this.onFocus.AddListener(onFocus);
			}
			if (onUnfocus != null)
			{
				this.onUnfocus.AddListener(onUnfocus);
			}
			if (onSelect != null)
			{
				this.onSelect.AddListener(onSelect);
			}
		}

		// Token: 0x0400041F RID: 1055
		public int group;

		// Token: 0x04000420 RID: 1056
		public bool ignoreScroll;

		// Token: 0x04000421 RID: 1057
		public GameObject focusFrame;

		// Token: 0x04000422 RID: 1058
		[SerializeField]
		private RectTransform focusRectTransform;

		// Token: 0x04000423 RID: 1059
		[SerializeField]
		private Button configuredForButton;

		// Token: 0x04000424 RID: 1060
		private GamepadNavigationController controller;

		// Token: 0x04000425 RID: 1061
		private int index;

		// Token: 0x04000426 RID: 1062
		private bool isFocused;

		// Token: 0x04000427 RID: 1063
		private Vector2 halfSize;

		// Token: 0x04000428 RID: 1064
		private bool active = true;

		// Token: 0x04000429 RID: 1065
		[Header("Custom Navigation")]
		[SerializeField]
		private GamepadNavigationItem leftItem;

		// Token: 0x0400042A RID: 1066
		[SerializeField]
		private GamepadNavigationItem rightItem;

		// Token: 0x0400042B RID: 1067
		[SerializeField]
		private GamepadNavigationItem upItem;

		// Token: 0x0400042C RID: 1068
		[SerializeField]
		private GamepadNavigationItem downItem;

		// Token: 0x0400042D RID: 1069
		[Space]
		[SerializeField]
		private UnityEvent onFocus = new UnityEvent();

		// Token: 0x0400042E RID: 1070
		[SerializeField]
		private UnityEvent onUnfocus = new UnityEvent();

		// Token: 0x0400042F RID: 1071
		[SerializeField]
		private UnityEvent onSelect = new UnityEvent();
	}
}

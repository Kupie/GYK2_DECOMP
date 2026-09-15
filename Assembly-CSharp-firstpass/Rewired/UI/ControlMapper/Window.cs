using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000E4 RID: 228
	[AddComponentMenu("")]
	[RequireComponent(typeof(CanvasGroup))]
	public class Window : MonoBehaviour
	{
		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x0001F9EB File Offset: 0x0001DBEB
		public bool hasFocus
		{
			get
			{
				return this._isFocusedCallback != null && this._isFocusedCallback(this._id);
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x0001FA08 File Offset: 0x0001DC08
		public int id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06000B93 RID: 2963 RVA: 0x0001FA10 File Offset: 0x0001DC10
		public RectTransform rectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = base.gameObject.GetComponent<RectTransform>();
				}
				return this._rectTransform;
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x0001FA37 File Offset: 0x0001DC37
		public TMP_Text titleText
		{
			get
			{
				return this._titleText;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x0001FA3F File Offset: 0x0001DC3F
		public List<TMP_Text> contentText
		{
			get
			{
				return this._contentText;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x0001FA47 File Offset: 0x0001DC47
		// (set) Token: 0x06000B97 RID: 2967 RVA: 0x0001FA4F File Offset: 0x0001DC4F
		public GameObject defaultUIElement
		{
			get
			{
				return this._defaultUIElement;
			}
			set
			{
				this._defaultUIElement = value;
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x0001FA58 File Offset: 0x0001DC58
		// (set) Token: 0x06000B99 RID: 2969 RVA: 0x0001FA60 File Offset: 0x0001DC60
		public Action<int> updateCallback
		{
			get
			{
				return this._updateCallback;
			}
			set
			{
				this._updateCallback = value;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x0001FA69 File Offset: 0x0001DC69
		public Window.Timer timer
		{
			get
			{
				return this._timer;
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06000B9B RID: 2971 RVA: 0x0001FA71 File Offset: 0x0001DC71
		// (set) Token: 0x06000B9C RID: 2972 RVA: 0x0001FA84 File Offset: 0x0001DC84
		public int width
		{
			get
			{
				return (int)this.rectTransform.sizeDelta.x;
			}
			set
			{
				Vector2 sizeDelta = this.rectTransform.sizeDelta;
				sizeDelta.x = (float)value;
				this.rectTransform.sizeDelta = sizeDelta;
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06000B9D RID: 2973 RVA: 0x0001FAB2 File Offset: 0x0001DCB2
		// (set) Token: 0x06000B9E RID: 2974 RVA: 0x0001FAC8 File Offset: 0x0001DCC8
		public int height
		{
			get
			{
				return (int)this.rectTransform.sizeDelta.y;
			}
			set
			{
				Vector2 sizeDelta = this.rectTransform.sizeDelta;
				sizeDelta.y = (float)value;
				this.rectTransform.sizeDelta = sizeDelta;
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06000B9F RID: 2975 RVA: 0x0001FAF6 File Offset: 0x0001DCF6
		protected bool initialized
		{
			get
			{
				return this._initialized;
			}
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0001FAFE File Offset: 0x0001DCFE
		private void OnEnable()
		{
			base.StartCoroutine("OnEnableAsync");
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0001FB0C File Offset: 0x0001DD0C
		protected virtual void Update()
		{
			if (!this._initialized)
			{
				return;
			}
			if (!this.hasFocus)
			{
				return;
			}
			this.CheckUISelection();
			if (this._updateCallback != null)
			{
				this._updateCallback(this._id);
			}
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0001FB40 File Offset: 0x0001DD40
		public virtual void Initialize(int id, Func<int, bool> isFocusedCallback)
		{
			if (this._initialized)
			{
				Debug.LogError("Window is already initialized!");
				return;
			}
			this._id = id;
			this._isFocusedCallback = isFocusedCallback;
			this._timer = new Window.Timer();
			this._contentText = new List<TMP_Text>();
			this._canvasGroup = base.GetComponent<CanvasGroup>();
			this._initialized = true;
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0001FB97 File Offset: 0x0001DD97
		public void SetSize(int width, int height)
		{
			this.rectTransform.sizeDelta = new Vector2((float)width, (float)height);
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0001FBAD File Offset: 0x0001DDAD
		public void CreateTitleText(GameObject prefab, Vector2 offset)
		{
			this.CreateText(prefab, ref this._titleText, "Title Text", UIPivot.TopCenter, UIAnchor.TopHStretch, offset);
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0001FBCC File Offset: 0x0001DDCC
		public void CreateTitleText(GameObject prefab, Vector2 offset, string text)
		{
			this.CreateTitleText(prefab, offset);
			this.SetTitleText(text);
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0001FBE0 File Offset: 0x0001DDE0
		public void AddContentText(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset)
		{
			TMP_Text tmp_Text = null;
			this.CreateText(prefab, ref tmp_Text, "Content Text", pivot, anchor, offset);
			this._contentText.Add(tmp_Text);
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0001FC0D File Offset: 0x0001DE0D
		public void AddContentText(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset, string text)
		{
			this.AddContentText(prefab, pivot, anchor, offset);
			this.SetContentText(text, this._contentText.Count - 1);
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0001FC2F File Offset: 0x0001DE2F
		public void AddContentImage(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset)
		{
			this.CreateImage(prefab, "Image", pivot, anchor, offset);
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0001FC41 File Offset: 0x0001DE41
		public void AddContentImage(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset, string text)
		{
			this.AddContentImage(prefab, pivot, anchor, offset);
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x0001FC50 File Offset: 0x0001DE50
		public void CreateButton(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset, string buttonText, UnityAction confirmCallback, UnityAction cancelCallback, bool setDefault)
		{
			if (prefab == null)
			{
				return;
			}
			ButtonInfo buttonInfo;
			GameObject gameObject = this.CreateButton(prefab, "Button", anchor, pivot, offset, out buttonInfo);
			if (gameObject == null)
			{
				return;
			}
			Button component = gameObject.GetComponent<Button>();
			if (confirmCallback != null)
			{
				component.onClick.AddListener(confirmCallback);
			}
			CustomButton customButton = component as CustomButton;
			if (cancelCallback != null && customButton != null)
			{
				customButton.CancelEvent += cancelCallback;
			}
			if (buttonInfo.text != null)
			{
				buttonInfo.text.text = buttonText;
			}
			if (setDefault)
			{
				this._defaultUIElement = gameObject;
			}
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x0001FCDE File Offset: 0x0001DEDE
		public string GetTitleText(string text)
		{
			if (this._titleText == null)
			{
				return string.Empty;
			}
			return this._titleText.text;
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x0001FCFF File Offset: 0x0001DEFF
		public void SetTitleText(string text)
		{
			if (this._titleText == null)
			{
				return;
			}
			this._titleText.text = text;
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x0001FD1C File Offset: 0x0001DF1C
		public string GetContentText(int index)
		{
			if (this._contentText == null || this._contentText.Count <= index || this._contentText[index] == null)
			{
				return string.Empty;
			}
			return this._contentText[index].text;
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x0001FD6C File Offset: 0x0001DF6C
		public float GetContentTextHeight(int index)
		{
			if (this._contentText == null || this._contentText.Count <= index || this._contentText[index] == null)
			{
				return 0f;
			}
			return this._contentText[index].rectTransform.sizeDelta.y;
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x0001FDC4 File Offset: 0x0001DFC4
		public void SetContentText(string text, int index)
		{
			if (this._contentText == null || this._contentText.Count <= index || this._contentText[index] == null)
			{
				return;
			}
			this._contentText[index].text = text;
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x0001FE03 File Offset: 0x0001E003
		public void SetUpdateCallback(Action<int> callback)
		{
			this.updateCallback = callback;
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0001FE0C File Offset: 0x0001E00C
		public virtual void TakeInputFocus()
		{
			if (EventSystem.current == null)
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(this._defaultUIElement);
			this.Enable();
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0001FE32 File Offset: 0x0001E032
		public virtual void Enable()
		{
			this._canvasGroup.interactable = true;
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0001FE40 File Offset: 0x0001E040
		public virtual void Disable()
		{
			this._canvasGroup.interactable = false;
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0001FE4E File Offset: 0x0001E04E
		public virtual void Cancel()
		{
			if (!this.initialized)
			{
				return;
			}
			if (this.cancelCallback != null)
			{
				this.cancelCallback();
			}
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0001FE6C File Offset: 0x0001E06C
		private void CreateText(GameObject prefab, ref TMP_Text textComponent, string name, UIPivot pivot, UIAnchor anchor, Vector2 offset)
		{
			if (prefab == null || this.content == null)
			{
				return;
			}
			if (textComponent != null)
			{
				Debug.LogError("Window already has " + name + "!");
				return;
			}
			GameObject gameObject = UITools.InstantiateGUIObject<TMP_Text>(prefab, this.content.transform, name, pivot, anchor.min, anchor.max, offset);
			if (gameObject == null)
			{
				return;
			}
			textComponent = gameObject.GetComponent<TMP_Text>();
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0001FEF0 File Offset: 0x0001E0F0
		private void CreateImage(GameObject prefab, string name, UIPivot pivot, UIAnchor anchor, Vector2 offset)
		{
			if (prefab == null || this.content == null)
			{
				return;
			}
			UITools.InstantiateGUIObject<Image>(prefab, this.content.transform, name, pivot, anchor.min, anchor.max, offset);
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0001FF40 File Offset: 0x0001E140
		private GameObject CreateButton(GameObject prefab, string name, UIAnchor anchor, UIPivot pivot, Vector2 offset, out ButtonInfo buttonInfo)
		{
			buttonInfo = null;
			if (prefab == null)
			{
				return null;
			}
			GameObject gameObject = UITools.InstantiateGUIObject<ButtonInfo>(prefab, this.content.transform, name, pivot, anchor.min, anchor.max, offset);
			if (gameObject == null)
			{
				return null;
			}
			buttonInfo = gameObject.GetComponent<ButtonInfo>();
			if (gameObject.GetComponent<Button>() == null)
			{
				Debug.Log("Button prefab is missing Button component!");
				return null;
			}
			if (buttonInfo == null)
			{
				Debug.Log("Button prefab is missing ButtonInfo component!");
				return null;
			}
			return gameObject;
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0001FFCA File Offset: 0x0001E1CA
		private IEnumerator OnEnableAsync()
		{
			yield return 1;
			if (EventSystem.current == null)
			{
				yield break;
			}
			if (this.defaultUIElement != null)
			{
				EventSystem.current.SetSelectedGameObject(this.defaultUIElement);
			}
			else
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
			yield break;
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0001FFDC File Offset: 0x0001E1DC
		private void CheckUISelection()
		{
			if (!this.hasFocus)
			{
				return;
			}
			if (EventSystem.current == null)
			{
				return;
			}
			if (EventSystem.current.currentSelectedGameObject == null)
			{
				this.RestoreDefaultOrLastUISelection();
			}
			this.lastUISelection = EventSystem.current.currentSelectedGameObject;
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x00020028 File Offset: 0x0001E228
		private void RestoreDefaultOrLastUISelection()
		{
			if (!this.hasFocus)
			{
				return;
			}
			if (this.lastUISelection == null || !this.lastUISelection.activeInHierarchy)
			{
				this.SetUISelection(this._defaultUIElement);
				return;
			}
			this.SetUISelection(this.lastUISelection);
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00018162 File Offset: 0x00016362
		private void SetUISelection(GameObject selection)
		{
			if (EventSystem.current == null)
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(selection);
		}

		// Token: 0x040005C4 RID: 1476
		public Image backgroundImage;

		// Token: 0x040005C5 RID: 1477
		public GameObject content;

		// Token: 0x040005C6 RID: 1478
		private bool _initialized;

		// Token: 0x040005C7 RID: 1479
		private int _id = -1;

		// Token: 0x040005C8 RID: 1480
		private RectTransform _rectTransform;

		// Token: 0x040005C9 RID: 1481
		private TMP_Text _titleText;

		// Token: 0x040005CA RID: 1482
		private List<TMP_Text> _contentText;

		// Token: 0x040005CB RID: 1483
		private GameObject _defaultUIElement;

		// Token: 0x040005CC RID: 1484
		private Action<int> _updateCallback;

		// Token: 0x040005CD RID: 1485
		private Func<int, bool> _isFocusedCallback;

		// Token: 0x040005CE RID: 1486
		private Window.Timer _timer;

		// Token: 0x040005CF RID: 1487
		private CanvasGroup _canvasGroup;

		// Token: 0x040005D0 RID: 1488
		public UnityAction cancelCallback;

		// Token: 0x040005D1 RID: 1489
		private GameObject lastUISelection;

		// Token: 0x020000E5 RID: 229
		public class Timer
		{
			// Token: 0x17000491 RID: 1169
			// (get) Token: 0x06000BBD RID: 3005 RVA: 0x00020076 File Offset: 0x0001E276
			public bool started
			{
				get
				{
					return this._started;
				}
			}

			// Token: 0x17000492 RID: 1170
			// (get) Token: 0x06000BBE RID: 3006 RVA: 0x0002007E File Offset: 0x0001E27E
			public bool finished
			{
				get
				{
					if (!this.started)
					{
						return false;
					}
					if (Time.realtimeSinceStartup < this.end)
					{
						return false;
					}
					this._started = false;
					return true;
				}
			}

			// Token: 0x17000493 RID: 1171
			// (get) Token: 0x06000BBF RID: 3007 RVA: 0x000200A1 File Offset: 0x0001E2A1
			public float remaining
			{
				get
				{
					if (!this._started)
					{
						return 0f;
					}
					return this.end - Time.realtimeSinceStartup;
				}
			}

			// Token: 0x06000BC1 RID: 3009 RVA: 0x000200BD File Offset: 0x0001E2BD
			public void Start(float length)
			{
				this.end = Time.realtimeSinceStartup + length;
				this._started = true;
			}

			// Token: 0x06000BC2 RID: 3010 RVA: 0x000200D3 File Offset: 0x0001E2D3
			public void Stop()
			{
				this._started = false;
			}

			// Token: 0x040005D2 RID: 1490
			private bool _started;

			// Token: 0x040005D3 RID: 1491
			private float end;
		}
	}
}

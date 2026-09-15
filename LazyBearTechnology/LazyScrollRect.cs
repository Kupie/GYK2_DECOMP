using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000017 RID: 23
public class LazyScrollRect : ScrollRect
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000064 RID: 100 RVA: 0x00003A68 File Offset: 0x00001C68
	private LazyScrollableElement ElementPrefab
	{
		get
		{
			if (this.elementPrefab == null)
			{
				this.elementPrefab = base.GetComponentInChildren<LazyScrollableElement>();
				if (this.elementPrefab == null)
				{
					GameObject gameObject = new GameObject();
					this.elementPrefab = gameObject.AddComponent<LazyScrollableElement>();
					gameObject.AddComponent<RectTransform>();
					gameObject.name = "LazyScrollableElementPrefab";
					gameObject.transform.SetParent(base.content);
				}
				this.elementPrefab.gameObject.SetActive(false);
			}
			return this.elementPrefab;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000065 RID: 101 RVA: 0x00003AE9 File Offset: 0x00001CE9
	public List<LazyScrollableElement> DisplayingElements
	{
		get
		{
			return this.displayingElements;
		}
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00003AF4 File Offset: 0x00001CF4
	public void Init(Func<LazyScrollableElement, LazyWidgetBase> widgetGetterFunc, Action<LazyScrollableElement> releaseWidgetAction, GamepadNavigationController gamepadNavigationController, Action onGamepadReInit = null, Func<bool> customVisibilityCondition = null, Action onGamepadFocus = null, Action onGamepadUnFocus = null, Action onGamepadSelect = null)
	{
		this.widgetGetterFunc = widgetGetterFunc;
		this.releaseWidgetAction = releaseWidgetAction;
		this.onGamepadReInit = onGamepadReInit;
		if (this.contentLayoutGroup == null)
		{
			this.contentLayoutGroup = base.content.GetComponent<LayoutGroup>();
		}
		gamepadNavigationController.OnFocusedItemChanged += delegate(GamepadNavigationItem _)
		{
			this.CheckVisibility();
		};
		base.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnValueChanged));
		this.elementsPool = LazyPooler.CreatePoolById("LazyScrollableElement " + this.ElementPrefab.GetHashCode().ToString(), this.ElementPrefab, 0, Pool.PoolType.ImmediateActivation, false, true, delegate(MonoBehaviour behaviour)
		{
			LazyScrollableElement lazyScrollableElement = behaviour as LazyScrollableElement;
			if (lazyScrollableElement != null)
			{
				lazyScrollableElement.Init(this.viewport, new Action<LazyScrollableElement>(this.OnUIScrollElementBecameVisible), new Action<LazyScrollableElement>(this.OnUIScrollElementBecameInvisible), customVisibilityCondition);
				if (lazyScrollableElement.GamepadNavigationItem != null)
				{
					LazyWidgetBase widget = lazyScrollableElement.ForceVisibilityAndGetWidget();
					lazyScrollableElement.GamepadNavigationItem.OnFocus.AddListener(delegate
					{
						if (widget != null)
						{
							Action onGamepadFocus2 = onGamepadFocus;
							if (onGamepadFocus2 == null)
							{
								return;
							}
							onGamepadFocus2();
						}
					});
					lazyScrollableElement.GamepadNavigationItem.OnUnfocus.AddListener(delegate
					{
						if (widget != null)
						{
							Action onGamepadUnFocus2 = onGamepadUnFocus;
							if (onGamepadUnFocus2 == null)
							{
								return;
							}
							onGamepadUnFocus2();
						}
					});
					lazyScrollableElement.GamepadNavigationItem.OnSelect.AddListener(delegate
					{
						if (widget != null)
						{
							Action onGamepadSelect2 = onGamepadSelect;
							if (onGamepadSelect2 == null)
							{
								return;
							}
							onGamepadSelect2();
						}
					});
				}
			}
		});
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00003BCC File Offset: 0x00001DCC
	public LazyScrollableElement AddScrollableElement(LazyWidgetDataBase data)
	{
		LazyScrollableElement orCreateObject = this.elementsPool.GetOrCreateObject<LazyScrollableElement>();
		orCreateObject.RectTransform.localScale = Vector3.one;
		orCreateObject.Data = data;
		this.displayingElements.Add(orCreateObject);
		return orCreateObject;
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00003C0C File Offset: 0x00001E0C
	public void RemoveScrollableElementAt(int index)
	{
		if (index < 0 || index >= this.displayingElements.Count)
		{
			Debug.LogWarning(string.Format("Can't remove ScrollableElement at index [{0}] because index is out of range.", index));
			return;
		}
		LazyScrollableElement lazyScrollableElement = this.displayingElements[index];
		if (lazyScrollableElement != null)
		{
			if (lazyScrollableElement.Widget != null)
			{
				this.OnReleaseWidget(lazyScrollableElement);
			}
			this.elementsPool.ReleaseObject<LazyScrollableElement>(lazyScrollableElement);
			this.OnGamepadReInit();
			this.displayingElements.RemoveAt(index);
		}
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00003C8C File Offset: 0x00001E8C
	public void CheckVisibility()
	{
		this.isUpdatingVisibilityForElements = true;
		foreach (LazyScrollableElement lazyScrollableElement in this.displayingElements)
		{
			lazyScrollableElement.CheckVisibility();
		}
		if (this.anyElementUpdatedVisibility)
		{
			this.OnGamepadReInit();
			this.anyElementUpdatedVisibility = false;
		}
		this.isUpdatingVisibilityForElements = false;
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00003D00 File Offset: 0x00001F00
	public void ClearDisplayingScrollableElements()
	{
		bool flag = false;
		for (int i = this.displayingElements.Count - 1; i >= 0; i--)
		{
			LazyScrollableElement lazyScrollableElement = this.displayingElements[i];
			if (lazyScrollableElement.Widget != null)
			{
				this.OnReleaseWidget(lazyScrollableElement);
				flag = true;
			}
			this.elementsPool.ReleaseObject<LazyScrollableElement>(lazyScrollableElement);
		}
		if (flag)
		{
			this.OnGamepadReInit();
		}
		this.displayingElements.Clear();
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00003D6C File Offset: 0x00001F6C
	private void OnUIScrollElementBecameVisible(LazyScrollableElement element)
	{
		if (element.Widget == null && element.Data != null)
		{
			LazyWidgetBase lazyWidgetBase = this.widgetGetterFunc(element);
			RectTransform component = lazyWidgetBase.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0.5f, 0.5f);
			component.anchorMax = new Vector2(0.5f, 0.5f);
			element.SetElement(lazyWidgetBase);
			element.UpdateSize();
			if (this.contentLayoutGroup != null)
			{
				this.contentLayoutGroup.CalculateLayoutInputHorizontal();
				this.contentLayoutGroup.CalculateLayoutInputVertical();
				this.contentLayoutGroup.SetLayoutHorizontal();
				this.contentLayoutGroup.SetLayoutVertical();
			}
			if (this.isUpdatingVisibilityForElements)
			{
				this.anyElementUpdatedVisibility = true;
				return;
			}
			this.OnGamepadReInit();
		}
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00003E2E File Offset: 0x0000202E
	private void OnUIScrollElementBecameInvisible(LazyScrollableElement element)
	{
		if (element.Widget != null)
		{
			this.OnReleaseWidget(element);
			if (this.isUpdatingVisibilityForElements)
			{
				this.anyElementUpdatedVisibility = true;
				return;
			}
			this.OnGamepadReInit();
		}
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00003E5B File Offset: 0x0000205B
	private void OnReleaseWidget(LazyScrollableElement element)
	{
		Action<LazyScrollableElement> action = this.releaseWidgetAction;
		if (action != null)
		{
			action(element);
		}
		element.UnsetElement();
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00003E75 File Offset: 0x00002075
	private void OnValueChanged(Vector2 value)
	{
		this.CheckVisibility();
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00003E7D File Offset: 0x0000207D
	private void OnGamepadReInit()
	{
		if (LazyInput.IsGamepadActive && base.gameObject.activeSelf)
		{
			Action action = this.onGamepadReInit;
			if (action == null)
			{
				return;
			}
			action();
		}
	}

	// Token: 0x04000052 RID: 82
	private Func<LazyScrollableElement, LazyWidgetBase> widgetGetterFunc;

	// Token: 0x04000053 RID: 83
	private Action<LazyScrollableElement> releaseWidgetAction;

	// Token: 0x04000054 RID: 84
	private Action onGamepadReInit;

	// Token: 0x04000055 RID: 85
	private Pool elementsPool;

	// Token: 0x04000056 RID: 86
	private bool isUpdatingVisibilityForElements;

	// Token: 0x04000057 RID: 87
	private bool anyElementUpdatedVisibility;

	// Token: 0x04000058 RID: 88
	[SerializeField]
	private LayoutGroup contentLayoutGroup;

	// Token: 0x04000059 RID: 89
	private LazyScrollableElement elementPrefab;

	// Token: 0x0400005A RID: 90
	private List<LazyScrollableElement> displayingElements = new List<LazyScrollableElement>();
}

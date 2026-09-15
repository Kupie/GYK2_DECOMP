using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000016 RID: 22
public class LazyScrollableElement : MonoBehaviour
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000058 RID: 88 RVA: 0x000038E4 File Offset: 0x00001AE4
	public RectTransform RectTransform
	{
		get
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			return this.rectTransform;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000059 RID: 89 RVA: 0x00003906 File Offset: 0x00001B06
	public GamepadNavigationItem GamepadNavigationItem
	{
		get
		{
			if (this.gamepadNavigationItem == null)
			{
				this.gamepadNavigationItem = base.GetComponent<GamepadNavigationItem>();
			}
			return this.gamepadNavigationItem;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x0600005A RID: 90 RVA: 0x00003928 File Offset: 0x00001B28
	// (set) Token: 0x0600005B RID: 91 RVA: 0x00003930 File Offset: 0x00001B30
	public LazyWidgetDataBase Data { get; set; }

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600005C RID: 92 RVA: 0x00003939 File Offset: 0x00001B39
	public LazyWidgetBase Widget
	{
		get
		{
			return this.widget;
		}
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00003941 File Offset: 0x00001B41
	public void Init(RectTransform viewport, Action<LazyScrollableElement> onVisibleAction, Action<LazyScrollableElement> onInvisibleAction, Func<bool> customVisibilityCondition = null)
	{
		this.viewport = viewport;
		this.onVisibleAction = onVisibleAction;
		this.onInvisibleAction = onInvisibleAction;
		this.customVisibilityCondition = customVisibilityCondition;
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00003960 File Offset: 0x00001B60
	public void SetElement(LazyWidgetBase element)
	{
		element.transform.SetParent(this.RectTransform);
		RectTransform component = element.GetComponent<RectTransform>();
		component.anchoredPosition = Vector2.zero;
		component.localScale = Vector3.one;
		this.widget = element;
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00003995 File Offset: 0x00001B95
	public void UnsetElement()
	{
		this.widget = null;
	}

	// Token: 0x06000060 RID: 96 RVA: 0x0000399E File Offset: 0x00001B9E
	public void UpdateSize()
	{
		if (this.Widget != null)
		{
			this.RectTransform.sizeDelta = this.Widget.GetComponent<RectTransform>().sizeDelta;
		}
	}

	// Token: 0x06000061 RID: 97 RVA: 0x000039CC File Offset: 0x00001BCC
	public void CheckVisibility()
	{
		if (this.customVisibilityCondition != null && this.customVisibilityCondition())
		{
			Action<LazyScrollableElement> action = this.onVisibleAction;
			if (action == null)
			{
				return;
			}
			action(this);
			return;
		}
		else if (this.viewport.IsRectTransformOverlapsOtherRectTransform(this.RectTransform))
		{
			Action<LazyScrollableElement> action2 = this.onVisibleAction;
			if (action2 == null)
			{
				return;
			}
			action2(this);
			return;
		}
		else
		{
			Action<LazyScrollableElement> action3 = this.onInvisibleAction;
			if (action3 == null)
			{
				return;
			}
			action3(this);
			return;
		}
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00003A36 File Offset: 0x00001C36
	public LazyWidgetBase ForceVisibilityAndGetWidget()
	{
		if (this.widget == null)
		{
			Action<LazyScrollableElement> action = this.onVisibleAction;
			if (action != null)
			{
				action(this);
			}
		}
		return this.widget;
	}

	// Token: 0x04000049 RID: 73
	private Action<LazyScrollableElement> onVisibleAction;

	// Token: 0x0400004A RID: 74
	private Action<LazyScrollableElement> onInvisibleAction;

	// Token: 0x0400004B RID: 75
	private Func<bool> customVisibilityCondition;

	// Token: 0x0400004C RID: 76
	[SerializeField]
	private RectTransform viewport;

	// Token: 0x0400004D RID: 77
	[SerializeField]
	private RectTransform rectTransform;

	// Token: 0x0400004E RID: 78
	[SerializeField]
	private GamepadNavigationItem gamepadNavigationItem;

	// Token: 0x0400004F RID: 79
	private LazyWidgetDataBase data;

	// Token: 0x04000051 RID: 81
	[SerializeField]
	private LazyWidgetBase widget;
}

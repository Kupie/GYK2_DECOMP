using System;
using System.Collections.Generic;
using Cinemachine;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200082D RID: 2093
[DefaultExecutionOrder(-100)]
[RequireComponent(typeof(Canvas))]
public class UIObjectBubbleManager : MonoBehaviour, ILazyGUIElement
{
	// Token: 0x17000801 RID: 2049
	// (get) Token: 0x0600358F RID: 13711 RVA: 0x001019F2 File Offset: 0x000FFBF2
	// (set) Token: 0x06003590 RID: 13712 RVA: 0x001019FA File Offset: 0x000FFBFA
	public SGuid PutOnTopTargetId { get; set; } = SGuid.Empty;

	// Token: 0x17000802 RID: 2050
	// (get) Token: 0x06003591 RID: 13713 RVA: 0x00101A03 File Offset: 0x000FFC03
	public static UIObjectBubbleManager Instance
	{
		get
		{
			return UIObjectBubbleManager.instance;
		}
	}

	// Token: 0x06003592 RID: 13714 RVA: 0x00101A0C File Offset: 0x000FFC0C
	public void Init()
	{
		if (UIObjectBubbleManager.instance != null)
		{
			Debug.LogError("Found duplication of UIObjectBubbleManager");
			return;
		}
		UIObjectBubbleManager.instance = this;
		this.pool = new Pool(this.prefab, this.poolParent, 0, Pool.PoolType.ImmediateActivation, false, null);
		this.canvas = base.GetComponent<Canvas>();
		this.canvas.overrideSorting = true;
		this.canvas.sortingOrder = 40;
		CinemachineCore.CameraUpdatedEvent.AddListener(new UnityAction<CinemachineBrain>(this.UpdatePos));
	}

	// Token: 0x06003593 RID: 13715 RVA: 0x00101A90 File Offset: 0x000FFC90
	public void Clear()
	{
		foreach (UIObjectBubble uiobjectBubble in this.displayedObjectBubbles)
		{
			uiobjectBubble.ForceCancelDelayedHides();
			uiobjectBubble.Hide();
			this.pool.ReleaseObject<UIObjectBubble>(uiobjectBubble);
		}
		this.displayedObjectBubbles.Clear();
		this.displayedBubblesCache.Clear();
		this.displayedBubblesCacheByDepth.Clear();
		this.pendingDisplayTargets.Clear();
		this.pendingDisplayTargetsBuffer.Clear();
	}

	// Token: 0x06003594 RID: 13716 RVA: 0x00101B2C File Offset: 0x000FFD2C
	public void RequestDisplay(IBubbleDrawable target)
	{
		if (target == null || UIObjectBubbleManager.IsTargetDestroyed(target))
		{
			return;
		}
		this.pendingDisplayTargets[target.BubbleDrawableUniqueId] = target;
	}

	// Token: 0x06003595 RID: 13717 RVA: 0x00101B4C File Offset: 0x000FFD4C
	public void Display(IBubbleDrawable target)
	{
		if (target == null || UIObjectBubbleManager.IsTargetDestroyed(target) || SGuid.IsNullOrEmpty(target.BubbleDrawableUniqueId))
		{
			return;
		}
		List<LazyWidgetDataBase> bubbleDrawableWidgets = target.BubbleDrawableWidgets;
		if (bubbleDrawableWidgets.Count == 0)
		{
			this.Hide(target);
			return;
		}
		UIObjectBubble uiobjectBubble;
		if (this.displayedBubblesCache.TryGetValue(target.BubbleDrawableUniqueId, out uiobjectBubble))
		{
			uiobjectBubble.TryCompareDataAndRedrawExistingOrDisplay(target, bubbleDrawableWidgets);
			return;
		}
		UIObjectBubble orCreateObject = this.pool.GetOrCreateObject<UIObjectBubble>();
		orCreateObject.transform.SetParent(base.transform);
		this.displayedObjectBubbles.Add(orCreateObject);
		this.displayedBubblesCache.Add(target.BubbleDrawableUniqueId, orCreateObject);
		orCreateObject.Display(target, bubbleDrawableWidgets);
		long depthKeyFromTarget = UIObjectBubbleManager.GetDepthKeyFromTarget(target);
		orCreateObject.SetDepthKey(depthKeyFromTarget);
		List<UIObjectBubble> list;
		if (!this.displayedBubblesCacheByDepth.TryGetValue(depthKeyFromTarget, out list))
		{
			list = new List<UIObjectBubble>();
			this.displayedBubblesCacheByDepth.Add(depthKeyFromTarget, list);
		}
		list.Add(orCreateObject);
		list.Sort(new Comparison<UIObjectBubble>(UIObjectBubbleManager.CompareBubblesByTargetId));
	}

	// Token: 0x06003596 RID: 13718 RVA: 0x00101C38 File Offset: 0x000FFE38
	public void HideWidget<T>(IBubbleDrawable target) where T : LazyWidgetDataBase
	{
		UIObjectBubble uiobjectBubble;
		if (this.displayedBubblesCache.TryGetValue(target.BubbleDrawableUniqueId, out uiobjectBubble))
		{
			if (uiobjectBubble.TryDelayHideWidget<T>(delegate
			{
				this.HideWidget<T>(target);
			}))
			{
				return;
			}
			uiobjectBubble.HideParticularWidget<T>();
			if (uiobjectBubble.DisplayedWidgetsCount == 0)
			{
				this.Hide(target);
			}
		}
	}

	// Token: 0x06003597 RID: 13719 RVA: 0x00101CA4 File Offset: 0x000FFEA4
	public void HideWidget(IBubbleDrawable target, LazyWidgetDataBase widgetData)
	{
		UIObjectBubble uiobjectBubble;
		if (this.displayedBubblesCache.TryGetValue(target.BubbleDrawableUniqueId, out uiobjectBubble))
		{
			if (uiobjectBubble.TryDelayHideWidget(widgetData.GetType(), delegate
			{
				this.HideWidget(target, widgetData);
			}))
			{
				return;
			}
			uiobjectBubble.HideParticularWidget(widgetData);
			if (uiobjectBubble.DisplayedWidgetsCount == 0)
			{
				this.Hide(target);
			}
		}
	}

	// Token: 0x06003598 RID: 13720 RVA: 0x00101D26 File Offset: 0x000FFF26
	public bool TryGetDisplayedBubble(SGuid uniqueId, out UIObjectBubble bubble)
	{
		bubble = null;
		return !SGuid.IsNullOrEmpty(uniqueId) && this.displayedBubblesCache.TryGetValue(uniqueId, out bubble);
	}

	// Token: 0x06003599 RID: 13721 RVA: 0x00101D44 File Offset: 0x000FFF44
	public void Hide(IBubbleDrawable target)
	{
		if (target == null || SGuid.IsNullOrEmpty(target.BubbleDrawableUniqueId))
		{
			return;
		}
		this.pendingDisplayTargets.Remove(target.BubbleDrawableUniqueId);
		UIObjectBubble uiobjectBubble;
		if (this.displayedBubblesCache.TryGetValue(target.BubbleDrawableUniqueId, out uiobjectBubble))
		{
			if (uiobjectBubble.TryDelayHide(delegate
			{
				this.Hide(target);
			}))
			{
				return;
			}
			this.displayedObjectBubbles.Remove(uiobjectBubble);
			this.displayedBubblesCache.Remove(target.BubbleDrawableUniqueId);
			uiobjectBubble.Hide();
			long depthKey = uiobjectBubble.DepthKey;
			List<UIObjectBubble> list;
			if (this.displayedBubblesCacheByDepth.TryGetValue(depthKey, out list))
			{
				list.Remove(uiobjectBubble);
				if (list.Count == 0)
				{
					this.displayedBubblesCacheByDepth.Remove(depthKey);
				}
			}
			this.pool.ReleaseObject<UIObjectBubble>(uiobjectBubble);
		}
	}

	// Token: 0x0600359A RID: 13722 RVA: 0x00101E30 File Offset: 0x00100030
	private static long GetDepthKey(float worldZ)
	{
		return Convert.ToInt64(Math.Round((double)(worldZ * 10f)));
	}

	// Token: 0x0600359B RID: 13723 RVA: 0x00101E44 File Offset: 0x00100044
	private static long GetDepthKeyFromTarget(IBubbleDrawable target)
	{
		return UIObjectBubbleManager.GetDepthKey(target.BubbleDrawablePosition.z);
	}

	// Token: 0x0600359C RID: 13724 RVA: 0x00101E58 File Offset: 0x00100058
	private static int CompareBubblesByTargetId(UIObjectBubble a, UIObjectBubble b)
	{
		return a.Target.BubbleDrawableUniqueId.Guid.CompareTo(b.Target.BubbleDrawableUniqueId.Guid);
	}

	// Token: 0x0600359D RID: 13725 RVA: 0x00101E90 File Offset: 0x00100090
	private static bool IsTargetDestroyed(IBubbleDrawable target)
	{
		global::UnityEngine.Object @object = target as global::UnityEngine.Object;
		return @object != null && @object == null;
	}

	// Token: 0x0600359E RID: 13726 RVA: 0x00101EB0 File Offset: 0x001000B0
	private void FlushPendingDisplays()
	{
		if (this.pendingDisplayTargets.Count == 0)
		{
			return;
		}
		this.pendingDisplayTargetsBuffer.Clear();
		foreach (IBubbleDrawable bubbleDrawable in this.pendingDisplayTargets.Values)
		{
			this.pendingDisplayTargetsBuffer.Add(bubbleDrawable);
		}
		this.pendingDisplayTargets.Clear();
		foreach (IBubbleDrawable bubbleDrawable2 in this.pendingDisplayTargetsBuffer)
		{
			if (!UIObjectBubbleManager.IsTargetDestroyed(bubbleDrawable2))
			{
				this.Display(bubbleDrawable2);
			}
		}
		this.pendingDisplayTargetsBuffer.Clear();
		this.UpdatePos(null);
	}

	// Token: 0x0600359F RID: 13727 RVA: 0x00101F90 File Offset: 0x00100190
	private void ApplyDepthSorting()
	{
		this.sortedDepthKeysBuffer.Clear();
		foreach (long num in this.displayedBubblesCacheByDepth.Keys)
		{
			this.sortedDepthKeysBuffer.Add(num);
		}
		this.sortedDepthKeysBuffer.Sort((long a, long b) => b.CompareTo(a));
		foreach (long num2 in this.sortedDepthKeysBuffer)
		{
			foreach (UIObjectBubble uiobjectBubble in this.displayedBubblesCacheByDepth[num2])
			{
				if (!uiobjectBubble.IsOutOfScreen && uiobjectBubble.gameObject.activeSelf)
				{
					uiobjectBubble.transform.SetAsLastSibling();
				}
			}
		}
	}

	// Token: 0x060035A0 RID: 13728 RVA: 0x001020C4 File Offset: 0x001002C4
	private void UpdatePos(CinemachineBrain brain)
	{
		foreach (UIObjectBubble uiobjectBubble in this.displayedObjectBubbles)
		{
			uiobjectBubble.UpdatePos();
		}
		this.ApplyDepthSorting();
		UIObjectBubble uiobjectBubble2;
		if (!SGuid.IsNullOrEmpty(this.PutOnTopTargetId) && this.displayedBubblesCache.TryGetValue(this.PutOnTopTargetId, out uiobjectBubble2))
		{
			uiobjectBubble2.transform.SetAsLastSibling();
		}
	}

	// Token: 0x060035A1 RID: 13729 RVA: 0x00102148 File Offset: 0x00100348
	private void Update()
	{
		this.displayedObjectBubbles.ForEach(delegate(UIObjectBubble item)
		{
			item.CustomUpdate();
		});
	}

	// Token: 0x060035A2 RID: 13730 RVA: 0x00102174 File Offset: 0x00100374
	private void LateUpdate()
	{
		this.FlushPendingDisplays();
	}

	// Token: 0x060035A3 RID: 13731 RVA: 0x0010217C File Offset: 0x0010037C
	private void OnDestroy()
	{
		CinemachineCore.CameraUpdatedEvent.RemoveListener(new UnityAction<CinemachineBrain>(this.UpdatePos));
	}

	// Token: 0x04002AEB RID: 10987
	private static UIObjectBubbleManager instance;

	// Token: 0x04002AEC RID: 10988
	[SerializeField]
	private List<UIObjectBubble> displayedObjectBubbles = new List<UIObjectBubble>();

	// Token: 0x04002AED RID: 10989
	private Dictionary<SGuid, UIObjectBubble> displayedBubblesCache = new Dictionary<SGuid, UIObjectBubble>();

	// Token: 0x04002AEE RID: 10990
	private Dictionary<long, List<UIObjectBubble>> displayedBubblesCacheByDepth = new Dictionary<long, List<UIObjectBubble>>();

	// Token: 0x04002AEF RID: 10991
	private readonly Dictionary<SGuid, IBubbleDrawable> pendingDisplayTargets = new Dictionary<SGuid, IBubbleDrawable>();

	// Token: 0x04002AF0 RID: 10992
	private readonly List<long> sortedDepthKeysBuffer = new List<long>();

	// Token: 0x04002AF1 RID: 10993
	private readonly List<IBubbleDrawable> pendingDisplayTargetsBuffer = new List<IBubbleDrawable>();

	// Token: 0x04002AF2 RID: 10994
	[SerializeField]
	private UIObjectBubble prefab;

	// Token: 0x04002AF3 RID: 10995
	[SerializeField]
	private Transform poolParent;

	// Token: 0x04002AF4 RID: 10996
	private Pool pool;

	// Token: 0x04002AF6 RID: 10998
	private Canvas canvas;
}

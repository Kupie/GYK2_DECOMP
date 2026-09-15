using System;
using System.Collections.Generic;
using DG.Tweening;

namespace UnityEngine.UI
{
	// Token: 0x02000029 RID: 41
	public static class UIExtensions
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x00004D6E File Offset: 0x00002F6E
		public static void ResetPosition(this ScrollRect scrollRect)
		{
			scrollRect.verticalNormalizedPosition = 1f;
			scrollRect.horizontalNormalizedPosition = 1f;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004D88 File Offset: 0x00002F88
		public static void RefreshContentFitter(this RectTransform transform)
		{
			if (transform == null || !transform.gameObject.activeSelf)
			{
				return;
			}
			for (int i = 0; i < transform.childCount; i++)
			{
				RectTransform rectTransform = transform.GetChild(i) as RectTransform;
				if (rectTransform != null)
				{
					rectTransform.RefreshContentFitter();
				}
			}
			LayoutGroup component = transform.GetComponent<LayoutGroup>();
			Object component2 = transform.GetComponent<ContentSizeFitter>();
			if (component != null)
			{
				component.CalculateLayoutInputHorizontal();
				component.CalculateLayoutInputVertical();
				component.SetLayoutHorizontal();
				component.SetLayoutVertical();
			}
			if (component2 != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004E10 File Offset: 0x00003010
		public static void EnableLayoutGroupsAndRefreshContentFitter(this RectTransform transform)
		{
			if (transform == null || !transform.gameObject.activeSelf)
			{
				return;
			}
			List<LayoutGroup> list = new List<LayoutGroup>();
			List<ContentSizeFitter> list2 = new List<ContentSizeFitter>();
			List<LayoutElement> list3 = new List<LayoutElement>();
			transform.GetComponentsInChildren<LayoutGroup>(true, list);
			transform.GetComponentsInChildren<ContentSizeFitter>(true, list2);
			transform.GetComponentsInChildren<LayoutElement>(true, list3);
			foreach (LayoutGroup layoutGroup in list)
			{
				layoutGroup.enabled = true;
			}
			foreach (ContentSizeFitter contentSizeFitter in list2)
			{
				contentSizeFitter.enabled = true;
			}
			foreach (LayoutElement layoutElement in list3)
			{
				layoutElement.enabled = true;
			}
			for (int i = 0; i < transform.childCount; i++)
			{
				RectTransform rectTransform = transform.GetChild(i) as RectTransform;
				if (rectTransform != null)
				{
					rectTransform.RefreshContentFitter();
				}
			}
			LayoutGroup component = transform.GetComponent<LayoutGroup>();
			Object component2 = transform.GetComponent<ContentSizeFitter>();
			if (component != null)
			{
				component.CalculateLayoutInputHorizontal();
				component.CalculateLayoutInputVertical();
				component.SetLayoutHorizontal();
				component.SetLayoutVertical();
			}
			if (component2 != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004F84 File Offset: 0x00003184
		public static void RefreshContentFitterAndDisable(this RectTransform transform)
		{
			if (transform == null || !transform.gameObject.activeSelf)
			{
				return;
			}
			List<LayoutGroup> list = new List<LayoutGroup>();
			List<ContentSizeFitter> list2 = new List<ContentSizeFitter>();
			List<LayoutElement> list3 = new List<LayoutElement>();
			transform.GetComponentsInChildren<LayoutGroup>(true, list);
			transform.GetComponentsInChildren<ContentSizeFitter>(true, list2);
			transform.GetComponentsInChildren<LayoutElement>(true, list3);
			foreach (LayoutGroup layoutGroup in list)
			{
				layoutGroup.enabled = true;
			}
			foreach (ContentSizeFitter contentSizeFitter in list2)
			{
				contentSizeFitter.enabled = true;
			}
			foreach (LayoutElement layoutElement in list3)
			{
				layoutElement.enabled = true;
			}
			transform.RefreshContentFitter();
			foreach (LayoutGroup layoutGroup2 in list)
			{
				layoutGroup2.enabled = false;
			}
			foreach (ContentSizeFitter contentSizeFitter2 in list2)
			{
				contentSizeFitter2.enabled = false;
			}
			foreach (LayoutElement layoutElement2 in list3)
			{
				layoutElement2.enabled = false;
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005144 File Offset: 0x00003344
		public static void ScrollToTarget(this ScrollRect scrollRect, RectTransform target, RectTransform.Axis axis, float autoScrollTime, bool isIndependentUpdate = false)
		{
			RectTransform viewport = scrollRect.viewport;
			RectTransform rectTransform = (viewport ? viewport : scrollRect.GetComponent<RectTransform>());
			Rect rect = rectTransform.rect;
			Bounds bounds = target.TransformBoundsTo(rectTransform);
			if (axis == RectTransform.Axis.Vertical)
			{
				float num = rect.center.y - bounds.center.y;
				float num2 = scrollRect.verticalNormalizedPosition - scrollRect.NormalizeScrollDistance(axis, num);
				scrollRect.DOVerticalNormalizedPos(Mathf.Clamp(num2, 0f, 1f), autoScrollTime, false).SetUpdate(isIndependentUpdate);
				return;
			}
			float num3 = rect.center.x - bounds.center.x;
			float num4 = scrollRect.horizontalNormalizedPosition - scrollRect.NormalizeScrollDistance(axis, num3);
			scrollRect.DOHorizontalNormalizedPos(Mathf.Clamp(num4, 0f, 1f), autoScrollTime, false).SetUpdate(isIndependentUpdate);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000521C File Offset: 0x0000341C
		public static void ScrollToTarget(this ScrollRect scrollRect, RectTransform target, RectTransform.Axis axis, float autoScrollTime, out Tween tween, bool isIndependentUpdate = false)
		{
			RectTransform viewport = scrollRect.viewport;
			RectTransform rectTransform = (viewport ? viewport : scrollRect.GetComponent<RectTransform>());
			Rect rect = rectTransform.rect;
			Bounds bounds = target.TransformBoundsTo(rectTransform);
			if (axis == RectTransform.Axis.Vertical)
			{
				float num = rect.center.y - bounds.center.y;
				float num2 = scrollRect.verticalNormalizedPosition - scrollRect.NormalizeScrollDistance(axis, num);
				tween = scrollRect.DOVerticalNormalizedPos(Mathf.Clamp(num2, 0f, 1f), autoScrollTime, false).SetUpdate(isIndependentUpdate);
				return;
			}
			float num3 = rect.center.x - bounds.center.x;
			float num4 = scrollRect.horizontalNormalizedPosition - scrollRect.NormalizeScrollDistance(axis, num3);
			tween = scrollRect.DOHorizontalNormalizedPos(Mathf.Clamp(num4, 0f, 1f), autoScrollTime, false).SetUpdate(isIndependentUpdate);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000052F8 File Offset: 0x000034F8
		public static void ScrollToVisiblePosition(this ScrollRect scrollRect, RectTransform target, RectTransform.Axis axis, float borderOffset, float autoScrollTime, bool isIndependentUpdate = false)
		{
			RectTransform viewport = scrollRect.viewport;
			RectTransform rectTransform = (viewport ? viewport : scrollRect.GetComponent<RectTransform>());
			Rect rect = rectTransform.rect;
			Bounds bounds = target.TransformBoundsTo(rectTransform);
			if (axis == RectTransform.Axis.Vertical)
			{
				float num = rect.min.y - bounds.min.y + borderOffset;
				float num2 = rect.max.y - bounds.max.y - borderOffset;
				if (num < 0f && num2 > 0f)
				{
					return;
				}
				float num3 = ((Mathf.Abs(num2) < Mathf.Abs(num)) ? num2 : num);
				float num4 = scrollRect.verticalNormalizedPosition - scrollRect.NormalizeScrollDistance(axis, num3);
				scrollRect.DOVerticalNormalizedPos(Mathf.Clamp(num4, 0f, 1f), autoScrollTime, false).SetUpdate(isIndependentUpdate);
				return;
			}
			else
			{
				float num5 = rect.min.x - bounds.min.x + borderOffset;
				float num6 = rect.max.x - bounds.max.x - borderOffset;
				if (num5 < 0f && num6 > 0f)
				{
					return;
				}
				float num7 = ((Mathf.Abs(num6) < Mathf.Abs(num5)) ? num6 : num5);
				float num8 = scrollRect.horizontalNormalizedPosition - scrollRect.NormalizeScrollDistance(axis, num7);
				scrollRect.DOHorizontalNormalizedPos(Mathf.Clamp(num8, 0f, 1f), autoScrollTime, false).SetUpdate(isIndependentUpdate);
				return;
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00005468 File Offset: 0x00003668
		public static void ScrollToVisiblePosition(this ScrollRect scrollRect, RectTransform target, RectTransform.Axis axis, float borderOffset, float autoScrollTime, out Tween tween, bool isIndependentUpdate = false)
		{
			tween = null;
			RectTransform viewport = scrollRect.viewport;
			RectTransform rectTransform = (viewport ? viewport : scrollRect.GetComponent<RectTransform>());
			Rect rect = rectTransform.rect;
			Bounds bounds = target.TransformBoundsTo(rectTransform);
			if (axis == RectTransform.Axis.Vertical)
			{
				float num = rect.min.y - bounds.min.y + borderOffset;
				float num2 = rect.max.y - bounds.max.y - borderOffset;
				if (num < 0f && num2 > 0f)
				{
					return;
				}
				float num3 = ((Mathf.Abs(num2) < Mathf.Abs(num)) ? num2 : num);
				float num4 = scrollRect.verticalNormalizedPosition - scrollRect.NormalizeScrollDistance(axis, num3);
				tween = scrollRect.DOVerticalNormalizedPos(Mathf.Clamp(num4, 0f, 1f), autoScrollTime, false).SetUpdate(isIndependentUpdate);
				return;
			}
			else
			{
				float num5 = rect.min.x - bounds.min.x + borderOffset;
				float num6 = rect.max.x - bounds.max.x - borderOffset;
				if (num5 < 0f && num6 > 0f)
				{
					return;
				}
				float num7 = ((Mathf.Abs(num6) < Mathf.Abs(num5)) ? num6 : num5);
				float num8 = scrollRect.horizontalNormalizedPosition - scrollRect.NormalizeScrollDistance(axis, num7);
				tween = scrollRect.DOHorizontalNormalizedPos(Mathf.Clamp(num8, 0f, 1f), autoScrollTime, false).SetUpdate(isIndependentUpdate);
				return;
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000055E0 File Offset: 0x000037E0
		public static void ScrollToTargetInstant(this ScrollRect scrollRect, RectTransform target, RectTransform.Axis axis)
		{
			RectTransform viewport = scrollRect.viewport;
			RectTransform rectTransform = (viewport ? viewport : scrollRect.GetComponent<RectTransform>());
			Rect rect = rectTransform.rect;
			Bounds bounds = target.TransformBoundsTo(rectTransform);
			if (axis == RectTransform.Axis.Vertical)
			{
				float num = rect.center.y - bounds.center.y;
				float num2 = scrollRect.verticalNormalizedPosition - scrollRect.NormalizeScrollDistance(axis, num);
				scrollRect.verticalNormalizedPosition = Mathf.Clamp(num2, 0f, 1f);
				return;
			}
			float num3 = rect.center.x - bounds.center.x;
			float num4 = scrollRect.horizontalNormalizedPosition - scrollRect.NormalizeScrollDistance(axis, num3);
			scrollRect.horizontalNormalizedPosition = Mathf.Clamp(num4, 0f, 1f);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000056A4 File Offset: 0x000038A4
		public static float NormalizeScrollDistance(this ScrollRect scrollRect, RectTransform.Axis axis, float distance)
		{
			RectTransform viewport = scrollRect.viewport;
			RectTransform rectTransform = ((viewport != null) ? viewport : scrollRect.GetComponent<RectTransform>());
			Rect rect = rectTransform.rect;
			Bounds bounds = new Bounds(rect.center, rect.size);
			RectTransform content = scrollRect.content;
			float num = ((content != null) ? content.TransformBoundsTo(rectTransform) : default(Bounds)).size[(int)axis] - bounds.size[(int)axis];
			return distance / num;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00005744 File Offset: 0x00003944
		public static Bounds TransformBoundsTo(this RectTransform source, Transform target)
		{
			source.GetWorldCorners(UIExtensions.cornersCached);
			Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
			Matrix4x4 worldToLocalMatrix = target.worldToLocalMatrix;
			for (int i = 0; i < 4; i++)
			{
				Vector3 vector3 = worldToLocalMatrix.MultiplyPoint3x4(UIExtensions.cornersCached[i]);
				vector = Vector3.Min(vector3, vector);
				vector2 = Vector3.Max(vector3, vector2);
			}
			Bounds bounds = new Bounds(vector, Vector3.zero);
			bounds.Encapsulate(vector2);
			return bounds;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000057D8 File Offset: 0x000039D8
		public static Rect GetWorldRect(this RectTransform rectTransform)
		{
			Vector3[] array = new Vector3[4];
			rectTransform.GetWorldCorners(array);
			Vector2 vector = array[0];
			Vector2 vector2 = array[2] - vector;
			return new Rect(vector, vector2);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000581C File Offset: 0x00003A1C
		public static bool IsRectTransformFullyInsideOtherRectTransform(this RectTransform rectTransform1, RectTransform rectTransform2)
		{
			Rect worldRect = rectTransform1.GetWorldRect();
			Rect worldRect2 = rectTransform2.GetWorldRect();
			return worldRect.xMin <= worldRect2.xMin && worldRect.yMin <= worldRect2.yMin && worldRect.xMax >= worldRect2.xMax && worldRect.yMax >= worldRect2.yMax;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000587C File Offset: 0x00003A7C
		public static bool IsRectTransformOverlapsOtherRectTransform(this RectTransform rectTransform1, RectTransform rectTransform2)
		{
			Rect worldRect = rectTransform1.GetWorldRect();
			Rect worldRect2 = rectTransform2.GetWorldRect();
			return worldRect.Overlaps(worldRect2);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000589F File Offset: 0x00003A9F
		public static string FontIcon(this string iconName)
		{
			return "<sprite name=\"" + iconName + "\">";
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000058B1 File Offset: 0x00003AB1
		public static string NOBR(this string str)
		{
			return "<nobr>" + str + "</nobr>";
		}

		// Token: 0x04000068 RID: 104
		private static Vector3[] cornersCached = new Vector3[4];
	}
}

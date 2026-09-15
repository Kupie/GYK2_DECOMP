using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LazyBearTechnology
{
	// Token: 0x02000199 RID: 409
	[DisallowMultipleComponent]
	[RequireComponent(typeof(ScrollRect))]
	[DefaultExecutionOrder(-50)]
	public class SmoothMouseWheelScroll : MonoBehaviour, IScrollHandler, IEventSystemHandler, IBeginDragHandler
	{
		// Token: 0x0600093A RID: 2362 RVA: 0x0002C8E0 File Offset: 0x0002AAE0
		public static SmoothMouseWheelScroll Ensure(ScrollRect target, float? wheelSensitivity = null)
		{
			if (target == null)
			{
				return null;
			}
			if (wheelSensitivity != null)
			{
				target.scrollSensitivity = wheelSensitivity.Value;
			}
			SmoothMouseWheelScroll smoothMouseWheelScroll;
			if (!target.TryGetComponent<SmoothMouseWheelScroll>(out smoothMouseWheelScroll))
			{
				smoothMouseWheelScroll = target.gameObject.AddComponent<SmoothMouseWheelScroll>();
			}
			else if (wheelSensitivity != null)
			{
				smoothMouseWheelScroll.ApplyWheelSensitivity(wheelSensitivity.Value);
			}
			return smoothMouseWheelScroll;
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0002C93D File Offset: 0x0002AB3D
		public static SmoothMouseWheelScroll EnsureForItem(ScrollRect target, float itemSize)
		{
			return SmoothMouseWheelScroll.Ensure(target, new float?(Mathf.Max(0f, itemSize)));
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0002C958 File Offset: 0x0002AB58
		public static SmoothMouseWheelScroll EnsureForItem(ScrollRect target, Component item, float fallbackItemSize)
		{
			RectTransform rectTransform = ((item != null) ? (item.transform as RectTransform) : null);
			float num = fallbackItemSize;
			if (rectTransform != null)
			{
				bool flag = target != null && target.horizontal && !target.vertical;
				float num2 = (flag ? rectTransform.rect.width : rectTransform.rect.height);
				if (num2 <= 1f)
				{
					num2 = (flag ? Mathf.Abs(rectTransform.sizeDelta.x) : Mathf.Abs(rectTransform.sizeDelta.y));
				}
				if (num2 > 1f)
				{
					num = num2;
				}
			}
			return SmoothMouseWheelScroll.EnsureForItem(target, num);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0002CA09 File Offset: 0x0002AC09
		private void Awake()
		{
			this.scrollRect = base.GetComponent<ScrollRect>();
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0002CA17 File Offset: 0x0002AC17
		private void OnEnable()
		{
			if (this.scrollRect == null)
			{
				this.scrollRect = base.GetComponent<ScrollRect>();
			}
			this.CacheAndDisableNativeSensitivity();
			this.ClearWheelSmoothing();
			this.CacheContentPosition();
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0002CA45 File Offset: 0x0002AC45
		private void OnDisable()
		{
			this.ClearWheelSmoothing();
			this.RestoreNativeSensitivity();
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0002CA53 File Offset: 0x0002AC53
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				this.ClearWheelSmoothing();
			}
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0002CA64 File Offset: 0x0002AC64
		public void OnScroll(PointerEventData eventData)
		{
			if (!base.isActiveAndEnabled || this.scrollRect == null || !this.scrollRect.IsActive())
			{
				return;
			}
			Vector2 scrollDelta = eventData.scrollDelta;
			scrollDelta.y *= -1f;
			if (this.scrollRect.vertical && !this.scrollRect.horizontal)
			{
				if (Mathf.Abs(scrollDelta.x) > Mathf.Abs(scrollDelta.y))
				{
					scrollDelta.y = scrollDelta.x;
				}
				scrollDelta.x = 0f;
			}
			if (this.scrollRect.horizontal && !this.scrollRect.vertical)
			{
				if (Mathf.Abs(scrollDelta.y) > Mathf.Abs(scrollDelta.x))
				{
					scrollDelta.x = scrollDelta.y;
				}
				scrollDelta.y = 0f;
			}
			this.remainingWheelOffset += scrollDelta * this.cachedScrollSensitivity;
			this.receivedWheelThisFrame = true;
			this.scrollRect.DOKill(false);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0002CB74 File Offset: 0x0002AD74
		private void LateUpdate()
		{
			if (this.scrollRect == null || this.scrollRect.content == null)
			{
				this.remainingWheelOffset = Vector2.zero;
				this.receivedWheelThisFrame = false;
				return;
			}
			RectTransform content = this.scrollRect.content;
			if (!this.receivedWheelThisFrame && (content.anchoredPosition - this.lastAppliedContentPosition).sqrMagnitude > 0.25f)
			{
				this.remainingWheelOffset = Vector2.zero;
			}
			if (this.remainingWheelOffset.sqrMagnitude > 0.0025000002f)
			{
				float num = Mathf.Min(Time.unscaledDeltaTime, 0.05f);
				if (num > 0f)
				{
					Vector2 vector = this.CalculateWheelStep(this.remainingWheelOffset, num);
					Vector2 anchoredPosition = content.anchoredPosition;
					content.anchoredPosition = anchoredPosition + vector;
					this.ClampToBounds();
					Vector2 vector2 = content.anchoredPosition - anchoredPosition;
					this.remainingWheelOffset -= vector;
					if (Mathf.Abs(vector.x) > 0.05f && Mathf.Abs(vector2.x) <= 0.05f)
					{
						this.remainingWheelOffset.x = 0f;
					}
					if (Mathf.Abs(vector.y) > 0.05f && Mathf.Abs(vector2.y) <= 0.05f)
					{
						this.remainingWheelOffset.y = 0f;
					}
				}
			}
			else
			{
				this.remainingWheelOffset = Vector2.zero;
			}
			this.CacheContentPosition();
			this.receivedWheelThisFrame = false;
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0002CCF4 File Offset: 0x0002AEF4
		private Vector2 CalculateWheelStep(Vector2 remaining, float deltaTime)
		{
			float magnitude = remaining.magnitude;
			float num = 220f * deltaTime;
			if (magnitude <= num)
			{
				return remaining;
			}
			float num2 = Mathf.Max(0.04f, this.wheelSmoothTime);
			float num3 = 1f - Mathf.Exp(-deltaTime / num2);
			float num4 = Mathf.Max(magnitude * num3, num);
			return remaining * (num4 / magnitude);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0002CD50 File Offset: 0x0002AF50
		private void ClampToBounds()
		{
			if (this.scrollRect.movementType != ScrollRect.MovementType.Clamped)
			{
				return;
			}
			if (this.scrollRect.horizontal)
			{
				this.scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(this.scrollRect.horizontalNormalizedPosition);
			}
			if (this.scrollRect.vertical)
			{
				this.scrollRect.verticalNormalizedPosition = Mathf.Clamp01(this.scrollRect.verticalNormalizedPosition);
			}
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0002CDBC File Offset: 0x0002AFBC
		private void ApplyWheelSensitivity(float sensitivity)
		{
			this.cachedScrollSensitivity = sensitivity;
			this.hasCachedSensitivity = true;
			if (base.isActiveAndEnabled && this.scrollRect != null)
			{
				this.scrollRect.scrollSensitivity = 0f;
			}
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0002CDF4 File Offset: 0x0002AFF4
		private void CacheAndDisableNativeSensitivity()
		{
			if (this.scrollRect == null)
			{
				return;
			}
			if (!this.hasCachedSensitivity)
			{
				this.cachedScrollSensitivity = this.scrollRect.scrollSensitivity;
				this.hasCachedSensitivity = true;
			}
			this.scrollRect.scrollSensitivity = 0f;
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0002CE40 File Offset: 0x0002B040
		private void RestoreNativeSensitivity()
		{
			if (this.scrollRect != null && this.hasCachedSensitivity)
			{
				this.scrollRect.scrollSensitivity = this.cachedScrollSensitivity;
			}
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0002CE69 File Offset: 0x0002B069
		private void ClearWheelSmoothing()
		{
			this.remainingWheelOffset = Vector2.zero;
			this.receivedWheelThisFrame = false;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0002CE7D File Offset: 0x0002B07D
		private void CacheContentPosition()
		{
			if (this.scrollRect != null && this.scrollRect.content != null)
			{
				this.lastAppliedContentPosition = this.scrollRect.content.anchoredPosition;
			}
		}

		// Token: 0x0400058F RID: 1423
		private const float WheelDeltaTimeCap = 0.05f;

		// Token: 0x04000590 RID: 1424
		private const float WheelFinishEpsilon = 0.05f;

		// Token: 0x04000591 RID: 1425
		private const float WheelMinFinishSpeed = 220f;

		// Token: 0x04000592 RID: 1426
		private const float ExternalMoveSqrThreshold = 0.25f;

		// Token: 0x04000593 RID: 1427
		[SerializeField]
		[Min(0.04f)]
		private float wheelSmoothTime = 0.1f;

		// Token: 0x04000594 RID: 1428
		private ScrollRect scrollRect;

		// Token: 0x04000595 RID: 1429
		private float cachedScrollSensitivity = 1f;

		// Token: 0x04000596 RID: 1430
		private Vector2 remainingWheelOffset;

		// Token: 0x04000597 RID: 1431
		private Vector2 lastAppliedContentPosition;

		// Token: 0x04000598 RID: 1432
		private bool receivedWheelThisFrame;

		// Token: 0x04000599 RID: 1433
		private bool hasCachedSensitivity;
	}
}

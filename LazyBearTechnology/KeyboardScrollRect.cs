using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace LazyBearTechnology
{
	// Token: 0x0200014E RID: 334
	[DisallowMultipleComponent]
	[RequireComponent(typeof(ScrollRect))]
	public class KeyboardScrollRect : MonoBehaviour
	{
		// Token: 0x06000704 RID: 1796 RVA: 0x000240C7 File Offset: 0x000222C7
		private void Awake()
		{
			this.scrollRect = base.GetComponent<ScrollRect>();
			this.CacheOwnerWindow();
			LazyWindowsStackController.OnWindowBecameVisibleInStack += this.OnWindowBecameVisibleInStack;
			LazyWindowsStackController.OnWindowBecameHiddenInStack += this.OnWindowBecameHiddenInStack;
			this.SyncInputProcessing();
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00024103 File Offset: 0x00022303
		private void OnEnable()
		{
			if (this.scrollRect == null)
			{
				this.scrollRect = base.GetComponent<ScrollRect>();
			}
			this.CacheOwnerWindow();
			this.SyncInputProcessing();
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0002412B File Offset: 0x0002232B
		private void OnDisable()
		{
			this.canProcessInput = false;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00024134 File Offset: 0x00022334
		private void Update()
		{
			try
			{
				if (this.CanScrollThisFrame())
				{
					Vector2 vector = this.ReadScrollInput();
					if (!LazyInput.IsGamepadActive && vector.sqrMagnitude >= 0.0001f)
					{
						if (vector.sqrMagnitude > 1f)
						{
							vector.Normalize();
						}
						float num = (this.useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
						float num2 = this.scrollSpeed * num;
						if (num > 0f && float.IsFinite(num2) && num2 > 0f)
						{
							if (!LazyInput.IsGamepadActive)
							{
								this.scrollRect.StopMovement();
								this.scrollRect.DOKill(false);
								this.ApplyScroll(vector, num2);
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x000241F8 File Offset: 0x000223F8
		private void OnDestroy()
		{
			LazyWindowsStackController.OnWindowBecameVisibleInStack -= this.OnWindowBecameVisibleInStack;
			LazyWindowsStackController.OnWindowBecameHiddenInStack -= this.OnWindowBecameHiddenInStack;
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0002421C File Offset: 0x0002241C
		private bool CanScrollThisFrame()
		{
			return this.canProcessInput && base.isActiveAndEnabled && LazyInput.IsInitialized && !LazyInput.IsGamepadActive && !(this.ownerWindow == null) && !(this.ownerWindow != LazyWindowsStackController.ActiveWindow) && !(this.scrollRect == null) && this.scrollRect.IsActive() && !(this.scrollRect.content == null) && float.IsFinite(this.scrollSpeed) && this.scrollSpeed > 0f;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x000242BC File Offset: 0x000224BC
		private Vector2 ReadScrollInput()
		{
			Vector2 zero = Vector2.zero;
			if (this.scrollRect.horizontal)
			{
				if (KeyboardScrollRect.IsKeyboardKeyHeld(GameKey.Left))
				{
					zero.x -= 1f;
				}
				if (KeyboardScrollRect.IsKeyboardKeyHeld(GameKey.Right))
				{
					zero.x += 1f;
				}
			}
			if (this.scrollRect.vertical)
			{
				if (KeyboardScrollRect.IsKeyboardKeyHeld(GameKey.Up))
				{
					zero.y += 1f;
				}
				if (KeyboardScrollRect.IsKeyboardKeyHeld(GameKey.Down))
				{
					zero.y -= 1f;
				}
			}
			return zero;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0002435A File Offset: 0x0002255A
		private static bool IsKeyboardKeyHeld(GameKey key)
		{
			return key != null && LazyInput.IsInitialized && !LazyInput.IsGamepadActive && LazyInput.GetKey(key);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00024378 File Offset: 0x00022578
		private void ApplyScroll(Vector2 input, float pixelDelta)
		{
			RectTransform content = this.scrollRect.content;
			if (content == null)
			{
				return;
			}
			RectTransform rectTransform = this.scrollRect.viewport;
			if (rectTransform == null)
			{
				rectTransform = this.scrollRect.transform as RectTransform;
			}
			if (rectTransform == null)
			{
				return;
			}
			Rect rect = rectTransform.rect;
			Rect rect2 = content.rect;
			if (this.scrollRect.horizontal && Mathf.Abs(input.x) > 0f)
			{
				float num = rect2.width - rect.width;
				if (num > 0.01f)
				{
					float num2 = this.scrollRect.horizontalNormalizedPosition + input.x * pixelDelta / num;
					if (float.IsFinite(num2))
					{
						this.scrollRect.horizontalNormalizedPosition = num2;
					}
				}
			}
			if (this.scrollRect.vertical && Mathf.Abs(input.y) > 0f)
			{
				float num3 = rect2.height - rect.height;
				if (num3 > 0.01f)
				{
					float num4 = this.scrollRect.verticalNormalizedPosition + input.y * pixelDelta / num3;
					if (float.IsFinite(num4))
					{
						this.scrollRect.verticalNormalizedPosition = num4;
					}
				}
			}
			if (this.scrollRect.movementType == ScrollRect.MovementType.Clamped)
			{
				if (this.scrollRect.horizontal)
				{
					this.scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(this.scrollRect.horizontalNormalizedPosition);
				}
				if (this.scrollRect.vertical)
				{
					this.scrollRect.verticalNormalizedPosition = Mathf.Clamp01(this.scrollRect.verticalNormalizedPosition);
				}
			}
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00024507 File Offset: 0x00022707
		private void OnWindowBecameVisibleInStack(LazyWidgetBase window)
		{
			if (this.ownerWindow != null && window == this.ownerWindow)
			{
				this.canProcessInput = true;
			}
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0002452C File Offset: 0x0002272C
		private void OnWindowBecameHiddenInStack(LazyWidgetBase window)
		{
			if (this.ownerWindow != null && window == this.ownerWindow)
			{
				this.canProcessInput = false;
			}
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00024554 File Offset: 0x00022754
		private void CacheOwnerWindow()
		{
			this.ownerWindow = null;
			Transform transform = base.transform;
			while (transform != null)
			{
				LazyWidgetBase lazyWidgetBase;
				Canvas canvas;
				if (transform.TryGetComponent<LazyWidgetBase>(out lazyWidgetBase) && transform.TryGetComponent<Canvas>(out canvas))
				{
					this.ownerWindow = lazyWidgetBase;
					return;
				}
				transform = transform.parent;
			}
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0002459D File Offset: 0x0002279D
		private void SyncInputProcessing()
		{
			this.canProcessInput = this.ownerWindow != null && this.ownerWindow == LazyWindowsStackController.ActiveWindow;
		}

		// Token: 0x04000439 RID: 1081
		[SerializeField]
		[Min(0f)]
		[Tooltip("Scroll speed in pixels per second.")]
		private float scrollSpeed = 500f;

		// Token: 0x0400043A RID: 1082
		[SerializeField]
		[Tooltip("Use unscaled time so scrolling works while the game is paused.")]
		private bool useUnscaledTime;

		// Token: 0x0400043B RID: 1083
		private ScrollRect scrollRect;

		// Token: 0x0400043C RID: 1084
		private LazyWidgetBase ownerWindow;

		// Token: 0x0400043D RID: 1085
		private bool canProcessInput;
	}
}

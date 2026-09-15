using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000148 RID: 328
	public class GamepadDynamicSelector : MonoBehaviour
	{
		// Token: 0x060006B8 RID: 1720 RVA: 0x000229EA File Offset: 0x00020BEA
		public void Init()
		{
			GamepadNavigationItem.OnUnfocusStatic += this.OnItemUnfocused;
			GamepadNavigationItem.OnFocusStatic += this.OnItemFocused;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00022A0E File Offset: 0x00020C0E
		public void SetOnUpdateCheckActivity(Func<bool> onUpdateCheckActivity)
		{
			this.onUpdateCheckActivity = onUpdateCheckActivity;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00022A18 File Offset: 0x00020C18
		private void OnItemFocused(GamepadNavigationItem gamepadNavigationItem)
		{
			if (!gamepadNavigationItem.Controller.IsEnabled)
			{
				return;
			}
			if (!gamepadNavigationItem.Active)
			{
				return;
			}
			this.currentItem = gamepadNavigationItem;
			this.CalculateEndSizeDelta();
			this.startSizeDelta = this.selectorRectTransform.sizeDelta;
			this.startPosition = this.selectorRectTransform.position;
			if (this.previousItem == null || this.previousItem.Controller != gamepadNavigationItem.Controller)
			{
				this.selectorRectTransform.position = gamepadNavigationItem.FocusRectTransform.position;
				this.selectorRectTransform.sizeDelta = this.endSizeDelta;
				this.selectorRectTransform.pivot = this.currentItem.FocusRectTransform.pivot;
				return;
			}
			this.elapsedTime = this.selectTime;
			this.lerp = true;
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00022AE7 File Offset: 0x00020CE7
		private void OnItemUnfocused(GamepadNavigationItem gamepadNavigationItem)
		{
			this.previousItem = gamepadNavigationItem;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00022AF0 File Offset: 0x00020CF0
		private void Update()
		{
			if (LazyInput.IsGamepadActive && this.currentItem != null && this.currentItem.Controller.IsEnabled && this.currentItem.Active && this.currentItem.gameObject.activeInHierarchy && this.onUpdateCheckActivity != null && this.onUpdateCheckActivity())
			{
				if (!this.selectorRectTransform.gameObject.activeSelf || !this.lerp)
				{
					this.CalculateEndSizeDelta();
					this.selectorRectTransform.position = this.currentItem.FocusRectTransform.position;
					this.selectorRectTransform.sizeDelta = this.endSizeDelta;
					this.selectorRectTransform.pivot = this.currentItem.FocusRectTransform.pivot;
					this.selectorRectTransform.gameObject.SetActive(true);
					return;
				}
				if (this.lerp)
				{
					this.elapsedTime -= (this.useUnscaledTime ? LazyTime.GetUnscaledDeltaTime : Time.deltaTime);
					this.elapsedTime = Mathf.Clamp(this.elapsedTime, 0f, 1f);
					if (this.elapsedTime > 0f)
					{
						this.CalculateEndSizeDelta();
						float num = 1f - this.elapsedTime / this.selectTime;
						this.selectorRectTransform.position = Vector3.Lerp(this.startPosition, this.currentItem.FocusRectTransform.position, num);
						this.selectorRectTransform.sizeDelta = Vector2.Lerp(this.startSizeDelta, this.endSizeDelta, num);
						this.lerp = true;
						return;
					}
					this.lerp = false;
					return;
				}
			}
			else
			{
				this.selectorRectTransform.gameObject.SetActive(false);
				this.currentItem = null;
				this.previousItem = null;
				this.lerp = false;
			}
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00022CC0 File Offset: 0x00020EC0
		private void CalculateEndSizeDelta()
		{
			Rect rect = this.currentItem.FocusRectTransform.rect;
			this.endSizeDelta.x = ((rect.width >= this.minWidth) ? rect.width : this.minWidth);
			this.endSizeDelta.y = ((rect.height >= this.minHeight) ? rect.height : this.minHeight);
		}

		// Token: 0x04000402 RID: 1026
		[SerializeField]
		private RectTransform selectorRectTransform;

		// Token: 0x04000403 RID: 1027
		[SerializeField]
		private float selectTime;

		// Token: 0x04000404 RID: 1028
		[SerializeField]
		private float minWidth;

		// Token: 0x04000405 RID: 1029
		[SerializeField]
		private float minHeight;

		// Token: 0x04000406 RID: 1030
		[SerializeField]
		private bool useUnscaledTime;

		// Token: 0x04000407 RID: 1031
		[Space]
		private GamepadNavigationItem currentItem;

		// Token: 0x04000408 RID: 1032
		private GamepadNavigationItem previousItem;

		// Token: 0x04000409 RID: 1033
		private Func<bool> onUpdateCheckActivity;

		// Token: 0x0400040A RID: 1034
		private bool lerp;

		// Token: 0x0400040B RID: 1035
		private float elapsedTime;

		// Token: 0x0400040C RID: 1036
		private Vector2 startSizeDelta;

		// Token: 0x0400040D RID: 1037
		private Vector3 startPosition;

		// Token: 0x0400040E RID: 1038
		private Vector2 endSizeDelta;
	}
}

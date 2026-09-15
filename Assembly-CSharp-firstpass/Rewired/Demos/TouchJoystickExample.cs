using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x020000F9 RID: 249
	[AddComponentMenu("")]
	[RequireComponent(typeof(Image))]
	public class TouchJoystickExample : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler
	{
		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x000234C0 File Offset: 0x000216C0
		// (set) Token: 0x06000C6F RID: 3183 RVA: 0x000234C8 File Offset: 0x000216C8
		public Vector2 position { get; private set; }

		// Token: 0x06000C70 RID: 3184 RVA: 0x000234D1 File Offset: 0x000216D1
		private void Start()
		{
			if (SystemInfo.deviceType == DeviceType.Handheld)
			{
				this.allowMouseControl = false;
			}
			this.StoreOrigValues();
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x000234E8 File Offset: 0x000216E8
		private void Update()
		{
			if ((float)Screen.width != this.origScreenResolution.x || (float)Screen.height != this.origScreenResolution.y || Screen.orientation != this.origScreenOrientation)
			{
				this.Restart();
				this.StoreOrigValues();
			}
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x00023534 File Offset: 0x00021734
		private void Restart()
		{
			this.hasFinger = false;
			(base.transform as RectTransform).anchoredPosition = this.origAnchoredPosition;
			this.position = Vector2.zero;
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00023560 File Offset: 0x00021760
		private void StoreOrigValues()
		{
			this.origAnchoredPosition = (base.transform as RectTransform).anchoredPosition;
			this.origWorldPosition = base.transform.position;
			this.origScreenResolution = new Vector2((float)Screen.width, (float)Screen.height);
			this.origScreenOrientation = Screen.orientation;
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x000235B8 File Offset: 0x000217B8
		private void UpdateValue(Vector3 value)
		{
			Vector3 vector = this.origWorldPosition - value;
			vector.y = -vector.y;
			vector /= (float)this.radius;
			this.position = new Vector2(-vector.x, vector.y);
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x00023606 File Offset: 0x00021806
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (this.hasFinger)
			{
				return;
			}
			if (!this.allowMouseControl && TouchJoystickExample.IsMousePointerId(eventData.pointerId))
			{
				return;
			}
			this.hasFinger = true;
			this.lastFingerId = eventData.pointerId;
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x0002363A File Offset: 0x0002183A
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (eventData.pointerId != this.lastFingerId)
			{
				return;
			}
			if (!this.allowMouseControl && TouchJoystickExample.IsMousePointerId(eventData.pointerId))
			{
				return;
			}
			this.Restart();
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00023668 File Offset: 0x00021868
		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			if (!this.hasFinger || eventData.pointerId != this.lastFingerId)
			{
				return;
			}
			Vector3 vector = new Vector3(eventData.position.x - this.origWorldPosition.x, eventData.position.y - this.origWorldPosition.y);
			vector = Vector3.ClampMagnitude(vector, (float)this.radius);
			Vector3 vector2 = this.origWorldPosition + vector;
			base.transform.position = vector2;
			this.UpdateValue(vector2);
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x0002349F File Offset: 0x0002169F
		private static bool IsMousePointerId(int id)
		{
			return id == -1 || id == -2 || id == -3;
		}

		// Token: 0x04000647 RID: 1607
		public bool allowMouseControl = true;

		// Token: 0x04000648 RID: 1608
		public int radius = 50;

		// Token: 0x04000649 RID: 1609
		private Vector2 origAnchoredPosition;

		// Token: 0x0400064A RID: 1610
		private Vector3 origWorldPosition;

		// Token: 0x0400064B RID: 1611
		private Vector2 origScreenResolution;

		// Token: 0x0400064C RID: 1612
		private ScreenOrientation origScreenOrientation;

		// Token: 0x0400064D RID: 1613
		[NonSerialized]
		private bool hasFinger;

		// Token: 0x0400064E RID: 1614
		[NonSerialized]
		private int lastFingerId;
	}
}

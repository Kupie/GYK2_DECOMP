using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x020000F8 RID: 248
	[AddComponentMenu("")]
	[RequireComponent(typeof(Image))]
	public class TouchButtonExample : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x00023436 File Offset: 0x00021636
		// (set) Token: 0x06000C67 RID: 3175 RVA: 0x0002343E File Offset: 0x0002163E
		public bool isPressed { get; private set; }

		// Token: 0x06000C68 RID: 3176 RVA: 0x00023447 File Offset: 0x00021647
		private void Awake()
		{
			if (SystemInfo.deviceType == DeviceType.Handheld)
			{
				this.allowMouseControl = false;
			}
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x00023458 File Offset: 0x00021658
		private void Restart()
		{
			this.isPressed = false;
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x00023461 File Offset: 0x00021661
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (!this.allowMouseControl && TouchButtonExample.IsMousePointerId(eventData.pointerId))
			{
				return;
			}
			this.isPressed = true;
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x00023480 File Offset: 0x00021680
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (!this.allowMouseControl && TouchButtonExample.IsMousePointerId(eventData.pointerId))
			{
				return;
			}
			this.isPressed = false;
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x0002349F File Offset: 0x0002169F
		private static bool IsMousePointerId(int id)
		{
			return id == -1 || id == -2 || id == -3;
		}

		// Token: 0x04000645 RID: 1605
		public bool allowMouseControl = true;
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000E1 RID: 225
	[AddComponentMenu("")]
	public class UISliderControl : UIControl
	{
		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x0001F465 File Offset: 0x0001D665
		// (set) Token: 0x06000B81 RID: 2945 RVA: 0x0001F46D File Offset: 0x0001D66D
		public bool showIcon
		{
			get
			{
				return this._showIcon;
			}
			set
			{
				if (this.iconImage == null)
				{
					return;
				}
				this.iconImage.gameObject.SetActive(value);
				this._showIcon = value;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x0001F496 File Offset: 0x0001D696
		// (set) Token: 0x06000B83 RID: 2947 RVA: 0x0001F49E File Offset: 0x0001D69E
		public bool showSlider
		{
			get
			{
				return this._showSlider;
			}
			set
			{
				if (this.slider == null)
				{
					return;
				}
				this.slider.gameObject.SetActive(value);
				this._showSlider = value;
			}
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0001F4C8 File Offset: 0x0001D6C8
		public override void SetCancelCallback(Action cancelCallback)
		{
			base.SetCancelCallback(cancelCallback);
			if (cancelCallback == null || this.slider == null)
			{
				return;
			}
			if (this.slider is ICustomSelectable)
			{
				(this.slider as ICustomSelectable).CancelEvent += delegate
				{
					cancelCallback();
				};
				return;
			}
			EventTrigger eventTrigger = this.slider.GetComponent<EventTrigger>();
			if (eventTrigger == null)
			{
				eventTrigger = this.slider.gameObject.AddComponent<EventTrigger>();
			}
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.callback = new EventTrigger.TriggerEvent();
			entry.eventID = EventTriggerType.Cancel;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				cancelCallback();
			});
			if (eventTrigger.triggers == null)
			{
				eventTrigger.triggers = new List<EventTrigger.Entry>();
			}
			eventTrigger.triggers.Add(entry);
		}

		// Token: 0x040005BF RID: 1471
		public Image iconImage;

		// Token: 0x040005C0 RID: 1472
		public Slider slider;

		// Token: 0x040005C1 RID: 1473
		private bool _showIcon;

		// Token: 0x040005C2 RID: 1474
		private bool _showSlider;
	}
}

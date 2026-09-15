using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000DA RID: 218
	[AddComponentMenu("")]
	public class UIControlSet : MonoBehaviour
	{
		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06000B66 RID: 2918 RVA: 0x0001EEC0 File Offset: 0x0001D0C0
		private Dictionary<int, UIControl> controls
		{
			get
			{
				Dictionary<int, UIControl> dictionary;
				if ((dictionary = this._controls) == null)
				{
					dictionary = (this._controls = new Dictionary<int, UIControl>());
				}
				return dictionary;
			}
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0001EEE5 File Offset: 0x0001D0E5
		public void SetTitle(string text)
		{
			if (this.title == null)
			{
				return;
			}
			this.title.text = text;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0001EF04 File Offset: 0x0001D104
		public T GetControl<T>(int uniqueId) where T : UIControl
		{
			UIControl uicontrol;
			this.controls.TryGetValue(uniqueId, out uicontrol);
			return uicontrol as T;
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0001EF2C File Offset: 0x0001D12C
		public UISliderControl CreateSlider(GameObject prefab, Sprite icon, float minValue, float maxValue, Action<int, float> valueChangedCallback, Action<int> cancelCallback)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(prefab);
			UISliderControl control = gameObject.GetComponent<UISliderControl>();
			if (control == null)
			{
				global::UnityEngine.Object.Destroy(gameObject);
				Debug.LogError("Prefab missing UISliderControl component!");
				return null;
			}
			gameObject.transform.SetParent(base.transform, false);
			if (control.iconImage != null)
			{
				control.iconImage.sprite = icon;
			}
			if (control.slider != null)
			{
				control.slider.minValue = minValue;
				control.slider.maxValue = maxValue;
				if (valueChangedCallback != null)
				{
					control.slider.onValueChanged.AddListener(delegate(float value)
					{
						valueChangedCallback(control.id, value);
					});
				}
				if (cancelCallback != null)
				{
					control.SetCancelCallback(delegate
					{
						cancelCallback(control.id);
					});
				}
			}
			this.controls.Add(control.id, control);
			return control;
		}

		// Token: 0x040005AF RID: 1455
		[SerializeField]
		private TMP_Text title;

		// Token: 0x040005B0 RID: 1456
		private Dictionary<int, UIControl> _controls;
	}
}

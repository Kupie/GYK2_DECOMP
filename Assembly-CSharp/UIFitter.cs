using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200083B RID: 2107
public class UIFitter : MonoBehaviour
{
	// Token: 0x17000808 RID: 2056
	// (get) Token: 0x060035D5 RID: 13781 RVA: 0x00102B65 File Offset: 0x00100D65
	private RectTransform RectTransform
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

	// Token: 0x060035D6 RID: 13782 RVA: 0x00102B88 File Offset: 0x00100D88
	public void UpdateSafeArea(float scaleFactor)
	{
		LazyUI.SetSafeZones(Screen.safeArea);
		Bounds screenBounds = LazyUI.GetScreenBounds();
		this.RectTransform.offsetMin = new Vector2(screenBounds.center.x, (float)Screen.height - screenBounds.extents.y - screenBounds.center.y) / scaleFactor;
		this.RectTransform.offsetMax = new Vector2((float)Screen.width - screenBounds.center.x - screenBounds.extents.x, screenBounds.center.y) / scaleFactor;
		Debug.Log(string.Format("SetUISafeArea: res: [{0}x{1}], scale: [{2}], offsetMin: [{3}], offsetMax: [{4}]", new object[]
		{
			Screen.width,
			Screen.height,
			scaleFactor,
			this.RectTransform.offsetMin,
			this.RectTransform.offsetMax
		}));
	}

	// Token: 0x04002B22 RID: 11042
	private RectTransform rectTransform;
}

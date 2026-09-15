using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200089A RID: 2202
public class UIHUDWheelElement : MonoBehaviour
{
	// Token: 0x04002D25 RID: 11557
	public RectTransform rectTransform;

	// Token: 0x04002D26 RID: 11558
	public Image icon;

	// Token: 0x04002D27 RID: 11559
	public Color iconColorDefault = new Color(1f, 1f, 1f, 1f);

	// Token: 0x04002D28 RID: 11560
	public Color iconColorInactive = new Color(1f, 1f, 1f, 0.4f);
}

using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000813 RID: 2067
[RequireComponent(typeof(Image))]
public class BlueColorReplaceComponent : MonoBehaviour
{
	// Token: 0x0600350E RID: 13582 RVA: 0x000FF9FA File Offset: 0x000FDBFA
	private void OnEnable()
	{
		this.SetColor();
	}

	// Token: 0x0600350F RID: 13583 RVA: 0x000FFA02 File Offset: 0x000FDC02
	private void SetColor()
	{
		if (this.image == null)
		{
			this.image = base.GetComponent<Image>();
		}
		this.image.BlueColorReplace(this.image.color);
	}

	// Token: 0x04002A8A RID: 10890
	private Image image;
}

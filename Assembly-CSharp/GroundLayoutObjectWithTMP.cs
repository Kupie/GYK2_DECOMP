using System;
using TMPro;
using UnityEngine;

// Token: 0x02000506 RID: 1286
public class GroundLayoutObjectWithTMP : GroundLayoutObject
{
	// Token: 0x06002154 RID: 8532 RVA: 0x0009D45C File Offset: 0x0009B65C
	protected override void Redraw()
	{
		base.Redraw();
		Vector3 localPosition = this.top.transform.localPosition;
		this.label.rectTransform.localPosition = new Vector3(localPosition.x, this.label.rectTransform.localPosition.y, localPosition.z);
		this.label.rectTransform.localScale = new Vector3(1f, 1.666667f, 1f);
		this.label.rectTransform.sizeDelta = new Vector2(this.top.size.x, this.label.rectTransform.sizeDelta.y);
	}

	// Token: 0x04001DED RID: 7661
	[Space]
	[SerializeField]
	private TextMeshPro label;
}

using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x020008C5 RID: 2245
public class LinkedEntityWidgetWithText : LinkedEntityWidget
{
	// Token: 0x06003AB0 RID: 15024 RVA: 0x001184E0 File Offset: 0x001166E0
	public override void Redraw()
	{
		base.Redraw();
		this.headerStyle.ApplyStyle(this.text, false, null, null, null);
		string text = this.prefixStyle.ApplyStyleToString(this.data.GetLinkedEntityPrefix() + ": ", false, true);
		this.text.text = LLBase.L(text + this.data.GetLinkedEntityHeader());
		this.background.enabled = true;
	}

	// Token: 0x04002E40 RID: 11840
	[SerializeField]
	private TextMeshProUGUI text;

	// Token: 0x04002E41 RID: 11841
	[SerializeField]
	private TextStyle prefixStyle;

	// Token: 0x04002E42 RID: 11842
	[SerializeField]
	private TextStyle headerStyle;
}

using System;
using UnityEngine;

// Token: 0x02000822 RID: 2082
[RequireComponent(typeof(Canvas))]
public class UIFade : UIBasicFade
{
	// Token: 0x06003546 RID: 13638 RVA: 0x001006D4 File Offset: 0x000FE8D4
	public override void Init()
	{
		base.Init();
		this.canvas = base.GetComponent<Canvas>();
		this.canvas.overrideSorting = true;
		this.canvas.sortingOrder = 800;
	}

	// Token: 0x04002AB6 RID: 10934
	private Canvas canvas;
}

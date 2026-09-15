using System;
using TheraBytes.BetterUi;
using UnityEngine;

// Token: 0x02000B41 RID: 2881
public class SetAsAnchorTarget : MonoBehaviour
{
	// Token: 0x06004C8D RID: 19597 RVA: 0x001696D8 File Offset: 0x001678D8
	public void SetTarget(RectTransform target)
	{
		this.affectedObject.CurrentAnchors.Elements[0].Reference = target;
	}

	// Token: 0x04003DAF RID: 15791
	[SerializeField]
	private AnchorOverride affectedObject;
}

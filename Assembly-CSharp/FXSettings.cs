using System;
using UnityEngine;

// Token: 0x02000538 RID: 1336
[CreateAssetMenu(menuName = "GK2/FX Settings")]
public class FXSettings : ScriptableObject
{
	// Token: 0x0600225A RID: 8794 RVA: 0x000A143F File Offset: 0x0009F63F
	public float GetTreeDestroyAnimLen(float height)
	{
		return this.treeDestroyAnimLen * (1f + (height - this.treeDestroyDefaultHeight) * this.treeDestroyHeightK);
	}

	// Token: 0x04001EE5 RID: 7909
	public float treeChopAnimLen = 1f;

	// Token: 0x04001EE6 RID: 7910
	public float treeDestroyAnimLen = 1f;

	// Token: 0x04001EE7 RID: 7911
	public float treeDestroyActionTime = 0.8f;

	// Token: 0x04001EE8 RID: 7912
	public float treeDestroyGndSpriteDisableTime = 0.3f;

	// Token: 0x04001EE9 RID: 7913
	[Tooltip("Base tree height. It sets default animation behaviour")]
	public float treeDestroyDefaultHeight = 1f;

	// Token: 0x04001EEA RID: 7914
	[Tooltip("Uses for the calculation of the time for lower and higher trees")]
	public float treeDestroyHeightK = 1f;
}

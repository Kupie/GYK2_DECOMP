using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000516 RID: 1302
[DisallowMultipleComponent]
public class Object3DOptimized : MonoBehaviour
{
	// Token: 0x04001E43 RID: 7747
	[SerializeField]
	public List<GameObject> optimizedChildren = new List<GameObject>();

	// Token: 0x04001E44 RID: 7748
	[SerializeField]
	public List<GameObject> disabledSourceObjects = new List<GameObject>();

	// Token: 0x04001E45 RID: 7749
	[SerializeField]
	public List<Component> disabledComponents = new List<Component>();
}

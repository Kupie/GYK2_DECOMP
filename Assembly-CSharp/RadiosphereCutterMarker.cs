using System;
using UnityEngine;

// Token: 0x02000AFD RID: 2813
public class RadiosphereCutterMarker : MonoBehaviour
{
	// Token: 0x04003C8A RID: 15498
	[Tooltip("If true, the radius from the CapsuleCollider on this object will be used for NavMesh cutting when a worker is assigned.")]
	public bool useRadiusFromCapsule = true;
}

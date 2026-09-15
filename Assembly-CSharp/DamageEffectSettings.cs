using System;
using DG.Tweening;
using UnityEngine;

// Token: 0x020002B7 RID: 695
[CreateAssetMenu(menuName = "GK2/Fighting/Damage Effect Settings", fileName = "DamageEffectSettings")]
public class DamageEffectSettings : ScriptableObject
{
	// Token: 0x0400139E RID: 5022
	[Header("Visual Effect")]
	public string fxName;

	// Token: 0x0400139F RID: 5023
	public bool useAdditiveTintColor;

	// Token: 0x040013A0 RID: 5024
	public Gradient colorGradient;

	// Token: 0x040013A1 RID: 5025
	[Header("Animation")]
	[Range(0.1f, 2f)]
	public float blinkDuration = 0.3f;

	// Token: 0x040013A2 RID: 5026
	public Ease blinkEase = Ease.OutBounce;

	// Token: 0x040013A3 RID: 5027
	[Header("Decals")]
	public bool spawnBloodPaddle;

	// Token: 0x040013A4 RID: 5028
	public bool spawnGutsDecals;

	// Token: 0x040013A5 RID: 5029
	public bool spawnBonesDecals;
}

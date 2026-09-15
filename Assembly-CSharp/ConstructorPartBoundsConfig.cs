using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000A9E RID: 2718
[CreateAssetMenu(menuName = "GK2/ConstructorPartBoundsConfig", fileName = "ConstructorPartBoundsConfig")]
public class ConstructorPartBoundsConfig : LazySingletonSO<ConstructorPartBoundsConfig>
{
	// Token: 0x17000B28 RID: 2856
	// (get) Token: 0x060049A4 RID: 18852 RVA: 0x0015BDCE File Offset: 0x00159FCE
	public ConstructorPartBoundsCollection BoundsCollection
	{
		get
		{
			return this.boundsCollection;
		}
	}

	// Token: 0x0400396D RID: 14701
	[SerializeField]
	private ConstructorPartBoundsCollection boundsCollection = new ConstructorPartBoundsCollection();
}

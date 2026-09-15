using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

// Token: 0x0200072A RID: 1834
public class GameSceneLinksCollection : MonoBehaviour
{
	// Token: 0x06002FE7 RID: 12263 RVA: 0x000E5B48 File Offset: 0x000E3D48
	public void RefreshLinks()
	{
		foreach (NodeLink2 nodeLink in this.links)
		{
			if (!(nodeLink == null))
			{
				nodeLink.Apply();
			}
		}
	}

	// Token: 0x040026D2 RID: 9938
	[SerializeField]
	private List<NodeLink2> links = new List<NodeLink2>();
}

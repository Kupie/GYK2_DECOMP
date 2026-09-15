using System;
using UnityEngine;

// Token: 0x0200071D RID: 1821
public class SceneWaypointContent : MonoBehaviour, IGameSceneContent
{
	// Token: 0x06002FB1 RID: 12209 RVA: 0x000E5042 File Offset: 0x000E3242
	public void SetId(string id)
	{
		this.worldId = id;
	}

	// Token: 0x06002FB2 RID: 12210 RVA: 0x000E504B File Offset: 0x000E324B
	public string GetId()
	{
		return this.worldId;
	}

	// Token: 0x06002FB3 RID: 12211 RVA: 0x000E5053 File Offset: 0x000E3253
	private void Awake()
	{
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040026A0 RID: 9888
	[SerializeField]
	private string worldId;
}

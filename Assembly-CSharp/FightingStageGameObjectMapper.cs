using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000327 RID: 807
public class FightingStageGameObjectMapper : MonoBehaviour
{
	// Token: 0x060015A0 RID: 5536 RVA: 0x00069644 File Offset: 0x00067844
	public bool TryGetGameObject(string id, out GameObject go)
	{
		go = null;
		if (string.IsNullOrEmpty(id) || this.data == null)
		{
			return false;
		}
		for (int i = 0; i < this.data.Count; i++)
		{
			FightingStageGameObjectMapper.FightingStageGameObjectMapperData fightingStageGameObjectMapperData = this.data[i];
			if (fightingStageGameObjectMapperData != null && fightingStageGameObjectMapperData.id == id)
			{
				go = fightingStageGameObjectMapperData.go;
				return go != null;
			}
		}
		return false;
	}

	// Token: 0x0400161B RID: 5659
	[SerializeField]
	private List<FightingStageGameObjectMapper.FightingStageGameObjectMapperData> data = new List<FightingStageGameObjectMapper.FightingStageGameObjectMapperData>();

	// Token: 0x02000328 RID: 808
	[Serializable]
	public class FightingStageGameObjectMapperData
	{
		// Token: 0x0400161C RID: 5660
		public string id;

		// Token: 0x0400161D RID: 5661
		public GameObject go;
	}
}

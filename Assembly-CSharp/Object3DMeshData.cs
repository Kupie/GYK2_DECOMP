using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000515 RID: 1301
[Serializable]
public class Object3DMeshData
{
	// Token: 0x060021A8 RID: 8616 RVA: 0x00021B94 File Offset: 0x0001FD94
	public Object3DMeshData()
	{
	}

	// Token: 0x060021A9 RID: 8617 RVA: 0x0009E61C File Offset: 0x0009C81C
	public Object3DMeshData(string id, GameObject obj, int materialsCount = 1)
	{
		this.id = id;
		this.obj = obj;
		this.texturesData = new List<TexturesData>();
		for (int i = 0; i < materialsCount; i++)
		{
			this.texturesData.Add(new TexturesData());
		}
	}

	// Token: 0x060021AA RID: 8618 RVA: 0x0009E664 File Offset: 0x0009C864
	public static string GetIdWithoutMetaData(string originalString)
	{
		if (originalString.Contains("-MERGE"))
		{
			originalString = originalString.Split("-MERGE", StringSplitOptions.None)[0];
		}
		return originalString;
	}

	// Token: 0x060021AB RID: 8619 RVA: 0x0009E684 File Offset: 0x0009C884
	public static bool NeedUnlitMaterial(string meshName)
	{
		foreach (string text in DevUtils.MeshUnlitMaterialPostfixes)
		{
			if (meshName.EndsWith(text))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060021AC RID: 8620 RVA: 0x0009E6E0 File Offset: 0x0009C8E0
	public bool IsBlackoutMesh()
	{
		return this.id.EndsWith("-blackout");
	}

	// Token: 0x04001E40 RID: 7744
	public string id;

	// Token: 0x04001E41 RID: 7745
	[FormerlySerializedAs("objTextures")]
	public List<TexturesData> texturesData;

	// Token: 0x04001E42 RID: 7746
	public GameObject obj;
}

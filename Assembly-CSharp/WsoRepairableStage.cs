using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006AC RID: 1708
[ExecuteInEditMode]
public class WsoRepairableStage : MonoBehaviour
{
	// Token: 0x1700071F RID: 1823
	// (get) Token: 0x06002DA4 RID: 11684 RVA: 0x000DA98E File Offset: 0x000D8B8E
	public IReadOnlyList<ConstructorPart> ConstructorParts
	{
		get
		{
			return this.constructorParts;
		}
	}

	// Token: 0x06002DA5 RID: 11685 RVA: 0x000DA998 File Offset: 0x000D8B98
	public void CollectParts()
	{
		this.constructorParts.ForEach(delegate(ConstructorPart part)
		{
			if (!part)
			{
				return;
			}
			part.constructorPartChildData.canNotBeBaked = false;
		});
		this.constructorParts.Clear();
		base.GetComponentsInChildren<ConstructorPart>(true, this.constructorParts);
		this.constructorParts.ForEach(delegate(ConstructorPart part)
		{
			part.constructorPartChildData.canNotBeBaked = true;
		});
	}

	// Token: 0x06002DA6 RID: 11686 RVA: 0x000DAA14 File Offset: 0x000D8C14
	public WsoStageData CreateStageData(int stageIndex)
	{
		this.CollectParts();
		WsoStageData wsoStageData = new WsoStageData(stageIndex);
		foreach (ConstructorPart constructorPart in this.constructorParts)
		{
			if (constructorPart)
			{
				ConstructorPartStateData constructorPartStateData = this.CreatePartStateData(constructorPart);
				if (constructorPartStateData != null)
				{
					wsoStageData.AddPartData(constructorPartStateData);
				}
			}
		}
		return wsoStageData;
	}

	// Token: 0x06002DA7 RID: 11687 RVA: 0x000DAA88 File Offset: 0x000D8C88
	private ConstructorPartStateData CreatePartStateData(ConstructorPart part)
	{
		string text = null;
		ConstructorPartChildData constructorPartChildData = part.constructorPartChildData;
		string text2 = ((constructorPartChildData != null) ? constructorPartChildData.pathToObject : null);
		if (!string.IsNullOrEmpty(text2))
		{
			int num = text2.LastIndexOf('/');
			int num2 = text2.LastIndexOf('.');
			if (num >= 0 && num2 > num)
			{
				text = text2.Substring(num + 1, num2 - num - 1);
			}
		}
		ConstructorPartChildData constructorPartChildData2 = part.constructorPartChildData;
		string text3 = ((((constructorPartChildData2 != null) ? constructorPartChildData2.lut : null) != null) ? part.constructorPartChildData.lut.name : null);
		if (string.IsNullOrEmpty(text))
		{
			Debug.LogWarning("[WsoRepairableStage] ConstructorPart '" + part.name + "' has no model ID, skipping");
			return null;
		}
		Vector3 vector = base.transform.InverseTransformPoint(part.transform.position);
		Vector3 lossyScale = part.transform.lossyScale;
		Vector3 lossyScale2 = base.transform.lossyScale;
		Vector3 localScale = part.transform.localScale;
		if (lossyScale2.x != 0f && lossyScale2.y != 0f && lossyScale2.z != 0f)
		{
			localScale = new Vector3(lossyScale.x / lossyScale2.x, lossyScale.y / lossyScale2.y, lossyScale.z / lossyScale2.z);
		}
		return new ConstructorPartStateData(text, vector, localScale.x, text3);
	}

	// Token: 0x0400249F RID: 9375
	[SerializeField]
	private List<ConstructorPart> constructorParts = new List<ConstructorPart>();
}

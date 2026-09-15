using System;
using UnityEngine;

// Token: 0x020005DC RID: 1500
[Serializable]
public class ConstructorPartStateData
{
	// Token: 0x17000661 RID: 1633
	// (get) Token: 0x060027A4 RID: 10148 RVA: 0x000B9C04 File Offset: 0x000B7E04
	public string OriginalModelId
	{
		get
		{
			return this.originalModelId;
		}
	}

	// Token: 0x17000662 RID: 1634
	// (get) Token: 0x060027A5 RID: 10149 RVA: 0x000B9C0C File Offset: 0x000B7E0C
	public Vector3 LocalPosition
	{
		get
		{
			return this.localPosition;
		}
	}

	// Token: 0x17000663 RID: 1635
	// (get) Token: 0x060027A6 RID: 10150 RVA: 0x000B9C14 File Offset: 0x000B7E14
	public float LocalXScale
	{
		get
		{
			return this.localXScale;
		}
	}

	// Token: 0x17000664 RID: 1636
	// (get) Token: 0x060027A7 RID: 10151 RVA: 0x000B9C1C File Offset: 0x000B7E1C
	public string LutName
	{
		get
		{
			return this.lutName;
		}
	}

	// Token: 0x17000665 RID: 1637
	// (get) Token: 0x060027A8 RID: 10152 RVA: 0x000B9C24 File Offset: 0x000B7E24
	public bool IsRepaired
	{
		get
		{
			return this.isRepaired;
		}
	}

	// Token: 0x17000666 RID: 1638
	// (get) Token: 0x060027A9 RID: 10153 RVA: 0x000B9C2C File Offset: 0x000B7E2C
	public bool IsDeleted
	{
		get
		{
			return this.isDeleted;
		}
	}

	// Token: 0x060027AA RID: 10154 RVA: 0x000B9C34 File Offset: 0x000B7E34
	public ConstructorPartStateData()
	{
	}

	// Token: 0x060027AB RID: 10155 RVA: 0x000B9C47 File Offset: 0x000B7E47
	public ConstructorPartStateData(string originalModelId, Vector3 localPosition, float localXScale, string lutName = null)
	{
		this.originalModelId = originalModelId;
		this.localPosition = localPosition;
		this.localXScale = localXScale;
		this.lutName = lutName;
		this.isRepaired = false;
	}

	// Token: 0x060027AC RID: 10156 RVA: 0x000B9C80 File Offset: 0x000B7E80
	public string GetCurrentModelId(ConstructorPartReplacementConfig config)
	{
		if (this.isDeleted || string.IsNullOrEmpty(this.originalModelId))
		{
			return null;
		}
		string text;
		if (this.isRepaired && config != null && config.TryGetRepairedModel(this.originalModelId, out text) && !string.IsNullOrEmpty(text))
		{
			return text;
		}
		return this.originalModelId;
	}

	// Token: 0x060027AD RID: 10157 RVA: 0x000B9CD8 File Offset: 0x000B7ED8
	public string GetCurrentAssetPath(ConstructorPartReplacementConfig config)
	{
		if (this.isDeleted || string.IsNullOrEmpty(this.originalModelId) || config == null)
		{
			return null;
		}
		string text;
		string text2;
		if (this.isRepaired && config.TryGetRepairedModelWithPath(this.originalModelId, out text, out text2) && !string.IsNullOrEmpty(text2))
		{
			return text2;
		}
		string text3;
		if (!config.TryGetBrokenAssetPath(this.originalModelId, out text3))
		{
			return null;
		}
		return text3;
	}

	// Token: 0x060027AE RID: 10158 RVA: 0x000B9D3C File Offset: 0x000B7F3C
	public void SetRepaired()
	{
		this.isRepaired = true;
		this.isDeleted = false;
	}

	// Token: 0x060027AF RID: 10159 RVA: 0x000B9D4C File Offset: 0x000B7F4C
	public void SetDeleted()
	{
		this.isDeleted = true;
		this.isRepaired = false;
	}

	// Token: 0x060027B0 RID: 10160 RVA: 0x000B9D5C File Offset: 0x000B7F5C
	public void Reset()
	{
		this.isRepaired = false;
		this.isDeleted = false;
	}

	// Token: 0x040021AB RID: 8619
	[Tooltip("Original (broken) model name — stable key for repair mapping, reset, and path lookup")]
	[SerializeField]
	private string originalModelId;

	// Token: 0x040021AC RID: 8620
	[Tooltip("Local position relative to the stage/Wso")]
	[SerializeField]
	private Vector3 localPosition;

	// Token: 0x040021AD RID: 8621
	[Tooltip("Rotation")]
	[SerializeField]
	private Quaternion localRotation = Quaternion.identity;

	// Token: 0x040021AE RID: 8622
	[Tooltip("Scale of current constructor part)")]
	[SerializeField]
	private float localXScale;

	// Token: 0x040021AF RID: 8623
	[Tooltip("LUT name (loaded via config path + name at runtime)")]
	[SerializeField]
	private string lutName;

	// Token: 0x040021B0 RID: 8624
	[Tooltip("Whether this part has been repaired")]
	[SerializeField]
	private bool isRepaired;

	// Token: 0x040021B1 RID: 8625
	[Tooltip("Whether this part should be deleted (not replaced)")]
	[SerializeField]
	private bool isDeleted;
}

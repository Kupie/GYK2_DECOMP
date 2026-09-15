using System;
using UnityEngine;

// Token: 0x020005D6 RID: 1494
[Serializable]
public abstract class WsoComponentDataBase
{
	// Token: 0x17000652 RID: 1618
	// (get) Token: 0x06002765 RID: 10085 RVA: 0x000B920D File Offset: 0x000B740D
	public string ComponentType
	{
		get
		{
			return this.componentType;
		}
	}

	// Token: 0x06002766 RID: 10086 RVA: 0x000B9215 File Offset: 0x000B7415
	protected WsoComponentDataBase()
	{
		this.componentType = base.GetType().Name;
	}

	// Token: 0x06002767 RID: 10087 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void PrepareForGame()
	{
	}

	// Token: 0x06002768 RID: 10088 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void Cleanup()
	{
	}

	// Token: 0x0400219A RID: 8602
	[SerializeField]
	protected string componentType;
}

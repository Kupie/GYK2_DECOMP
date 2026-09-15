using System;
using UnityEngine;

// Token: 0x02000893 RID: 2195
public abstract class UIBaseNotification : MonoBehaviour
{
	// Token: 0x17000864 RID: 2148
	// (get) Token: 0x06003857 RID: 14423 RVA: 0x0010EB6C File Offset: 0x0010CD6C
	// (set) Token: 0x06003858 RID: 14424 RVA: 0x0010EB74 File Offset: 0x0010CD74
	public float CurrentTime { get; set; }

	// Token: 0x17000865 RID: 2149
	// (get) Token: 0x06003859 RID: 14425 RVA: 0x0010EB7D File Offset: 0x0010CD7D
	// (set) Token: 0x0600385A RID: 14426 RVA: 0x0010EB85 File Offset: 0x0010CD85
	public bool IsTimerActive { get; set; }

	// Token: 0x17000866 RID: 2150
	// (get) Token: 0x0600385B RID: 14427 RVA: 0x0010ACC4 File Offset: 0x00108EC4
	public RectTransform RectTransform
	{
		get
		{
			return base.transform as RectTransform;
		}
	}

	// Token: 0x0600385C RID: 14428
	public abstract void Draw();

	// Token: 0x0600385D RID: 14429
	public abstract void ReleaseToPool();

	// Token: 0x04002CD8 RID: 11480
	private const float HEIGHT = 58f;

	// Token: 0x04002CD9 RID: 11481
	public float displayingTime = 4f;
}

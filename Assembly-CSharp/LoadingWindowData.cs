using System;
using LazyBearTechnology;

// Token: 0x020009F3 RID: 2547
public class LoadingWindowData : LazyWidgetDataBase
{
	// Token: 0x17000A79 RID: 2681
	// (get) Token: 0x060044A9 RID: 17577 RVA: 0x001458A9 File Offset: 0x00143AA9
	// (set) Token: 0x060044AA RID: 17578 RVA: 0x001458B1 File Offset: 0x00143AB1
	public Action OnAnimationComplete { get; private set; }

	// Token: 0x17000A7A RID: 2682
	// (get) Token: 0x060044AB RID: 17579 RVA: 0x001458BA File Offset: 0x00143ABA
	// (set) Token: 0x060044AC RID: 17580 RVA: 0x001458C2 File Offset: 0x00143AC2
	public string SceneId { get; private set; }

	// Token: 0x17000A7B RID: 2683
	// (get) Token: 0x060044AD RID: 17581 RVA: 0x001458CB File Offset: 0x00143ACB
	// (set) Token: 0x060044AE RID: 17582 RVA: 0x001458D3 File Offset: 0x00143AD3
	public bool IsCrossSceneLoading { get; private set; }

	// Token: 0x060044AF RID: 17583 RVA: 0x001458DC File Offset: 0x00143ADC
	public LoadingWindowData(string sceneId, Action onAnimationComplete, bool isCrossSceneLoading = false)
	{
		this.SceneId = sceneId;
		this.OnAnimationComplete = onAnimationComplete;
		this.IsCrossSceneLoading = isCrossSceneLoading;
	}
}

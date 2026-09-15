using System;

// Token: 0x02000AFA RID: 2810
public interface IQuadTreeObjectBounds<in T>
{
	// Token: 0x06004AF0 RID: 19184
	float GetTop(T obj);

	// Token: 0x06004AF1 RID: 19185
	float GetRight(T obj);

	// Token: 0x06004AF2 RID: 19186
	float GetBot(T obj);

	// Token: 0x06004AF3 RID: 19187
	float GetLeft(T obj);
}

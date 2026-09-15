using System;

// Token: 0x0200081A RID: 2074
[Serializable]
public class CursorState
{
	// Token: 0x06003532 RID: 13618 RVA: 0x00100246 File Offset: 0x000FE446
	public CursorState(CursorType type, ICursorChanger changer)
	{
		this.type = type;
		this.changer = changer;
	}

	// Token: 0x04002A9F RID: 10911
	public CursorType type;

	// Token: 0x04002AA0 RID: 10912
	public readonly ICursorChanger changer;
}

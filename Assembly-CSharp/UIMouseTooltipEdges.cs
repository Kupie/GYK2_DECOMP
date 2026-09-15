using System;

// Token: 0x02000843 RID: 2115
[Serializable]
public struct UIMouseTooltipEdges
{
	// Token: 0x060035FA RID: 13818 RVA: 0x00103280 File Offset: 0x00101480
	public UIMouseTooltipEdges(float left = 0f, float right = 0f, float top = 0f, float bottom = 0f)
	{
		this.left = left;
		this.right = right;
		this.top = top;
		this.bottom = bottom;
	}

	// Token: 0x1700080B RID: 2059
	// (get) Token: 0x060035FB RID: 13819 RVA: 0x0010329F File Offset: 0x0010149F
	public bool IsZero
	{
		get
		{
			return this.left == 0f && this.right == 0f && this.top == 0f && this.bottom == 0f;
		}
	}

	// Token: 0x060035FC RID: 13820 RVA: 0x001032D7 File Offset: 0x001014D7
	public static UIMouseTooltipEdges All(float value)
	{
		return new UIMouseTooltipEdges(value, value, value, value);
	}

	// Token: 0x04002B3F RID: 11071
	public float left;

	// Token: 0x04002B40 RID: 11072
	public float right;

	// Token: 0x04002B41 RID: 11073
	public float top;

	// Token: 0x04002B42 RID: 11074
	public float bottom;
}

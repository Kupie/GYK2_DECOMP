using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000835 RID: 2101
[ExecuteAlways]
[DisallowMultipleComponent]
[RequireComponent(typeof(Graphic))]
[AddComponentMenu("UI/Progress Cell Atlas Size")]
public class ProgressCellAtlasSize : BaseMeshEffect
{
	// Token: 0x060035BA RID: 13754 RVA: 0x0010242E File Offset: 0x0010062E
	public void SetCellCount(int count)
	{
		if (this.cellCount == count)
		{
			return;
		}
		this.cellCount = count;
		if (base.graphic != null)
		{
			base.graphic.SetVerticesDirty();
		}
	}

	// Token: 0x060035BB RID: 13755 RVA: 0x0010245A File Offset: 0x0010065A
	protected override void OnEnable()
	{
		base.OnEnable();
		this.EnableCanvasTexCoord1();
	}

	// Token: 0x060035BC RID: 13756 RVA: 0x00102468 File Offset: 0x00100668
	protected override void OnTransformParentChanged()
	{
		base.OnTransformParentChanged();
		this.EnableCanvasTexCoord1();
	}

	// Token: 0x060035BD RID: 13757 RVA: 0x00102476 File Offset: 0x00100676
	protected override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		if (base.graphic != null)
		{
			base.graphic.SetVerticesDirty();
		}
	}

	// Token: 0x060035BE RID: 13758 RVA: 0x00102497 File Offset: 0x00100697
	protected override void OnDidApplyAnimationProperties()
	{
		base.OnDidApplyAnimationProperties();
		if (base.graphic != null)
		{
			base.graphic.SetVerticesDirty();
		}
	}

	// Token: 0x060035BF RID: 13759 RVA: 0x001024B8 File Offset: 0x001006B8
	public override void ModifyMesh(VertexHelper vh)
	{
		if (!this.IsActive() || vh == null)
		{
			return;
		}
		Vector2 size = ((RectTransform)base.transform).rect.size;
		float num = ((this.cellCount == 1) ? 1f : 0f);
		UIVertex uivertex = default(UIVertex);
		int currentVertCount = vh.currentVertCount;
		for (int i = 0; i < currentVertCount; i++)
		{
			vh.PopulateUIVertex(ref uivertex, i);
			uivertex.uv1 = new Vector4(size.x, size.y, (float)this.cellCount, num);
			vh.SetUIVertex(uivertex, i);
		}
	}

	// Token: 0x060035C0 RID: 13760 RVA: 0x00102554 File Offset: 0x00100754
	private void EnableCanvasTexCoord1()
	{
		if (base.graphic == null)
		{
			return;
		}
		Canvas canvas = base.graphic.canvas;
		if (canvas == null)
		{
			return;
		}
		canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
	}

	// Token: 0x04002B0C RID: 11020
	private const float SingleCellXOffsetPx = 1f;

	// Token: 0x04002B0D RID: 11021
	[SerializeField]
	private int cellCount;
}

using System;
using System.Collections.Generic;

// Token: 0x02000AFC RID: 2812
public class QuadTreeRoot<T> : QuadTreeNode<T>
{
	// Token: 0x06004AFF RID: 19199 RVA: 0x00161E00 File Offset: 0x00160000
	public QuadTreeRoot(float x, float y, float width, float height, IQuadTreeObjectBounds<T> objectBounds, float minLeafWidth, float minLeafHeight, int currentLevel = 0)
		: base(x, y, width, height, objectBounds, minLeafHeight, minLeafHeight, currentLevel)
	{
		this.objectsPerNodes = new Dictionary<T, QuadTreeNode<T>>();
	}

	// Token: 0x06004B00 RID: 19200 RVA: 0x00161E2C File Offset: 0x0016002C
	public override bool Insert(T obj)
	{
		List<QuadTreeNode<T>> list;
		List<T> list2;
		if (base.InsertInternal(obj, out list, out list2))
		{
			for (int i = 0; i < list.Count; i++)
			{
				this.objectsPerNodes.TryAdd(list2[i], list[i]);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06004B01 RID: 19201 RVA: 0x00161E74 File Offset: 0x00160074
	public override bool Remove(T obj)
	{
		QuadTreeNode<T> quadTreeNode;
		return this.objectsPerNodes.TryGetValue(obj, out quadTreeNode) && base.Remove(obj);
	}

	// Token: 0x06004B02 RID: 19202 RVA: 0x00161E9A File Offset: 0x0016009A
	public override void Clear()
	{
		base.Clear();
		this.objectsPerNodes.Clear();
	}

	// Token: 0x04003C89 RID: 15497
	private readonly Dictionary<T, QuadTreeNode<T>> objectsPerNodes;
}

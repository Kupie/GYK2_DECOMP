using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AFB RID: 2811
public class QuadTreeNode<T>
{
	// Token: 0x06004AF4 RID: 19188 RVA: 0x001618E4 File Offset: 0x0015FAE4
	public QuadTreeNode(float x, float y, float width, float height, IQuadTreeObjectBounds<T> objectBounds, float minLeafWidth, float minLeafHeight, int currentLevel = 0)
	{
		this.areaRect = new Rect(x, y, width, height);
		this.objects = new HashSet<T>();
		this.objectBounds = objectBounds;
		QuadTreeNode<T>.minLeafWidth = minLeafWidth;
		QuadTreeNode<T>.minLeafHeight = minLeafHeight;
		this.currentLevel = currentLevel;
	}

	// Token: 0x06004AF5 RID: 19189 RVA: 0x00161930 File Offset: 0x0015FB30
	public virtual bool Insert(T obj)
	{
		List<QuadTreeNode<T>> list;
		List<T> list2;
		return this.InsertInternal(obj, out list, out list2);
	}

	// Token: 0x06004AF6 RID: 19190 RVA: 0x00161948 File Offset: 0x0015FB48
	public virtual bool Remove(T obj)
	{
		return this.objects.Remove(obj);
	}

	// Token: 0x06004AF7 RID: 19191 RVA: 0x00161958 File Offset: 0x0015FB58
	protected bool InsertInternal(T obj, out List<QuadTreeNode<T>> insertedNodes, out List<T> insertedObjects)
	{
		insertedNodes = new List<QuadTreeNode<T>>();
		insertedObjects = new List<T>();
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		if (!this.IsObjectInside(obj))
		{
			return false;
		}
		if (!this.hasChildren)
		{
			this.objects.Add(obj);
			if ((this.areaRect.width > QuadTreeNode<T>.minLeafWidth || this.areaRect.height > QuadTreeNode<T>.minLeafHeight) && this.objects.Count > 1)
			{
				this.Quarter(out insertedNodes, out insertedObjects);
			}
			return true;
		}
		if (this.leftTop.InsertInternal(obj, out insertedNodes, out insertedObjects))
		{
			return true;
		}
		if (this.rightTop.InsertInternal(obj, out insertedNodes, out insertedObjects))
		{
			return true;
		}
		if (this.rightBot.InsertInternal(obj, out insertedNodes, out insertedObjects))
		{
			return true;
		}
		this.leftBot.InsertInternal(obj, out insertedNodes, out insertedObjects);
		return true;
	}

	// Token: 0x06004AF8 RID: 19192 RVA: 0x00161A28 File Offset: 0x0015FC28
	public void InsertRange(IEnumerable<T> objects)
	{
		foreach (T t in objects)
		{
			this.Insert(t);
		}
	}

	// Token: 0x06004AF9 RID: 19193 RVA: 0x00161A74 File Offset: 0x0015FC74
	public virtual void Clear()
	{
		if (this.hasChildren)
		{
			this.leftTop.Clear();
			this.leftTop = null;
			this.rightTop.Clear();
			this.rightTop = null;
			this.rightBot.Clear();
			this.rightBot = null;
			this.leftBot.Clear();
			this.leftBot = null;
		}
		this.objects.Clear();
		this.hasChildren = false;
	}

	// Token: 0x06004AFA RID: 19194 RVA: 0x00161AE4 File Offset: 0x0015FCE4
	public int Count()
	{
		int num = 0;
		if (this.hasChildren)
		{
			num += this.leftTop.Count();
			num += this.rightTop.Count();
			num += this.rightBot.Count();
			num += this.leftBot.Count();
		}
		else
		{
			num = this.objects.Count;
		}
		return num;
	}

	// Token: 0x06004AFB RID: 19195 RVA: 0x00161B44 File Offset: 0x0015FD44
	public List<T> FindObjects(Rect searchRect)
	{
		List<T> list = new List<T>();
		if (this.hasChildren)
		{
			list.AddRange(this.leftTop.FindObjects(searchRect));
			list.AddRange(this.rightTop.FindObjects(searchRect));
			list.AddRange(this.rightBot.FindObjects(searchRect));
			list.AddRange(this.leftBot.FindObjects(searchRect));
		}
		else if (this.IsOverlapping(searchRect))
		{
			list.AddRange(this.objects);
		}
		return list;
	}

	// Token: 0x06004AFC RID: 19196 RVA: 0x00161BC0 File Offset: 0x0015FDC0
	protected bool IsObjectInside(T obj)
	{
		return this.objectBounds.GetTop(obj) <= this.areaRect.yMax && this.objectBounds.GetBot(obj) >= this.areaRect.yMin && this.objectBounds.GetRight(obj) >= this.areaRect.xMin && this.objectBounds.GetLeft(obj) <= this.areaRect.xMax;
	}

	// Token: 0x06004AFD RID: 19197 RVA: 0x00161C3A File Offset: 0x0015FE3A
	protected bool IsOverlapping(Rect rect)
	{
		return this.areaRect.Overlaps(rect);
	}

	// Token: 0x06004AFE RID: 19198 RVA: 0x00161C48 File Offset: 0x0015FE48
	protected void Quarter(out List<QuadTreeNode<T>> insertedNodes, out List<T> insertedObjects)
	{
		insertedNodes = new List<QuadTreeNode<T>>();
		insertedObjects = new List<T>();
		if (this.areaRect.width < QuadTreeNode<T>.minLeafWidth && this.areaRect.height < QuadTreeNode<T>.minLeafHeight)
		{
			return;
		}
		int num = this.currentLevel + 1;
		this.hasChildren = true;
		float num2 = this.areaRect.width / 2f;
		float num3 = this.areaRect.height / 2f;
		this.leftTop = new QuadTreeNode<T>(this.areaRect.xMin, this.areaRect.yMin, num2, num3, this.objectBounds, QuadTreeNode<T>.minLeafHeight, QuadTreeNode<T>.minLeafHeight, num);
		this.rightTop = new QuadTreeNode<T>(this.areaRect.xMin + num2, this.areaRect.yMin, num2, num3, this.objectBounds, QuadTreeNode<T>.minLeafHeight, QuadTreeNode<T>.minLeafHeight, num);
		this.rightBot = new QuadTreeNode<T>(this.areaRect.xMin + num2, this.areaRect.yMin + num3, num2, num3, this.objectBounds, QuadTreeNode<T>.minLeafHeight, QuadTreeNode<T>.minLeafHeight, num);
		this.leftBot = new QuadTreeNode<T>(this.areaRect.xMin, this.areaRect.yMin + num3, num2, num3, this.objectBounds, QuadTreeNode<T>.minLeafHeight, QuadTreeNode<T>.minLeafHeight, num);
		foreach (T t in this.objects)
		{
			List<QuadTreeNode<T>> list;
			List<T> list2;
			if (this.InsertInternal(t, out list, out list2))
			{
				insertedNodes.AddRange(list);
				insertedObjects.AddRange(list2);
			}
		}
		this.objects.Clear();
	}

	// Token: 0x04003C7E RID: 15486
	public Rect areaRect;

	// Token: 0x04003C7F RID: 15487
	protected readonly HashSet<T> objects;

	// Token: 0x04003C80 RID: 15488
	protected IQuadTreeObjectBounds<T> objectBounds;

	// Token: 0x04003C81 RID: 15489
	protected bool hasChildren;

	// Token: 0x04003C82 RID: 15490
	protected QuadTreeNode<T> leftTop;

	// Token: 0x04003C83 RID: 15491
	protected QuadTreeNode<T> rightTop;

	// Token: 0x04003C84 RID: 15492
	protected QuadTreeNode<T> rightBot;

	// Token: 0x04003C85 RID: 15493
	private QuadTreeNode<T> leftBot;

	// Token: 0x04003C86 RID: 15494
	protected int currentLevel;

	// Token: 0x04003C87 RID: 15495
	protected static float minLeafWidth;

	// Token: 0x04003C88 RID: 15496
	protected static float minLeafHeight;
}

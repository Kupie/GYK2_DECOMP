using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000765 RID: 1893
public class FixedSizeQueue<T> : IEnumerable<T>, IEnumerable
{
	// Token: 0x1700078A RID: 1930
	// (get) Token: 0x06003126 RID: 12582 RVA: 0x000E9639 File Offset: 0x000E7839
	public int Count
	{
		get
		{
			return this.items.Count;
		}
	}

	// Token: 0x1700078B RID: 1931
	// (get) Token: 0x06003127 RID: 12583 RVA: 0x000E9646 File Offset: 0x000E7846
	public int MaxSize
	{
		get
		{
			return this.maxSize;
		}
	}

	// Token: 0x06003128 RID: 12584 RVA: 0x000E964E File Offset: 0x000E784E
	public FixedSizeQueue(int maxSize)
	{
		if (maxSize <= 0)
		{
			throw new ArgumentOutOfRangeException("maxSize", "Max size must be greater than zero.");
		}
		this.maxSize = maxSize;
	}

	// Token: 0x06003129 RID: 12585 RVA: 0x000E967C File Offset: 0x000E787C
	public void Enqueue(T item)
	{
		if (this.items.Count == this.maxSize)
		{
			this.items.RemoveFirst();
		}
		this.items.AddLast(item);
	}

	// Token: 0x0600312A RID: 12586 RVA: 0x000E96A9 File Offset: 0x000E78A9
	public T Dequeue()
	{
		if (this.items.Count == 0)
		{
			throw new InvalidOperationException("The queue is empty.");
		}
		T value = this.items.First.Value;
		this.items.RemoveFirst();
		return value;
	}

	// Token: 0x0600312B RID: 12587 RVA: 0x000E96DE File Offset: 0x000E78DE
	public T Peek()
	{
		if (this.items.Count == 0)
		{
			throw new InvalidOperationException("The queue is empty.");
		}
		return this.items.First.Value;
	}

	// Token: 0x0600312C RID: 12588 RVA: 0x000E9708 File Offset: 0x000E7908
	public T Last()
	{
		if (this.items.Count == 0)
		{
			throw new InvalidOperationException("The queue is empty.");
		}
		return this.items.Last.Value;
	}

	// Token: 0x0600312D RID: 12589 RVA: 0x000E9732 File Offset: 0x000E7932
	public IEnumerator<T> GetEnumerator()
	{
		return this.items.GetEnumerator();
	}

	// Token: 0x0600312E RID: 12590 RVA: 0x000E9744 File Offset: 0x000E7944
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x04002778 RID: 10104
	private readonly LinkedList<T> items = new LinkedList<T>();

	// Token: 0x04002779 RID: 10105
	private readonly int maxSize;
}

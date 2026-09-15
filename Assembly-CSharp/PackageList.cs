using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000767 RID: 1895
public class PackageList<T> : IEnumerable<T>, IEnumerable where T : CommandPackage
{
	// Token: 0x1700078E RID: 1934
	// (get) Token: 0x0600313E RID: 12606 RVA: 0x000E9ACC File Offset: 0x000E7CCC
	public int Count
	{
		get
		{
			return this.items.Count;
		}
	}

	// Token: 0x0600313F RID: 12607 RVA: 0x000E9AD9 File Offset: 0x000E7CD9
	public PackageList(int packageQueueMaxSize)
	{
		this.items = new FixedSizeQueue<T>(packageQueueMaxSize);
		this.idsToItems = new Dictionary<ulong, T>();
	}

	// Token: 0x06003140 RID: 12608 RVA: 0x000E9AF8 File Offset: 0x000E7CF8
	public bool TryAdd(T item)
	{
		if (this.items.Count == this.items.MaxSize)
		{
			if (this.oldestSentPackage == 0UL)
			{
				return false;
			}
			T t = this.items.Dequeue();
			this.idsToItems.Remove(t.packageId);
			if (this.lastSentPackage == t.packageId)
			{
				this.lastSentPackage = 0UL;
			}
			this.oldestSentPackage = ((this.lastSentPackage == 0UL) ? 0UL : this.items.Peek().packageId);
		}
		this.items.Enqueue(item);
		this.idsToItems.Add(item.packageId, item);
		return true;
	}

	// Token: 0x06003141 RID: 12609 RVA: 0x000E9BB0 File Offset: 0x000E7DB0
	public bool TryGetForSending(out T item)
	{
		item = default(T);
		if (this.items.Count == 0)
		{
			return false;
		}
		if (this.lastSentPackage == this.items.Last().packageId)
		{
			return false;
		}
		item = this.idsToItems[(this.lastSentPackage == 0UL) ? this.items.Last().packageId : (this.lastSentPackage + 1UL)];
		if (this.lastSentPackage == 0UL)
		{
			this.lastSentPackage = this.items.Last().packageId;
		}
		else
		{
			this.lastSentPackage += 1UL;
		}
		if (this.oldestSentPackage == 0UL)
		{
			this.oldestSentPackage = this.lastSentPackage;
		}
		return true;
	}

	// Token: 0x06003142 RID: 12610 RVA: 0x000E9C74 File Offset: 0x000E7E74
	public bool TryGetById(ulong packageId, out T package)
	{
		package = default(T);
		return this.idsToItems.TryGetValue(packageId, out package);
	}

	// Token: 0x06003143 RID: 12611 RVA: 0x000E9C8A File Offset: 0x000E7E8A
	public IEnumerator<T> GetEnumerator()
	{
		return this.items.GetEnumerator();
	}

	// Token: 0x06003144 RID: 12612 RVA: 0x000E9C98 File Offset: 0x000E7E98
	public override string ToString()
	{
		string text = "Package Queue:";
		int num = 0;
		foreach (T t in this.items)
		{
			text += string.Format("\n[{0}]: Package #{1}; Sent = {2}", num, t, t.packageId <= this.lastSentPackage);
		}
		return text;
	}

	// Token: 0x06003145 RID: 12613 RVA: 0x000E9D20 File Offset: 0x000E7F20
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x04002780 RID: 10112
	private readonly FixedSizeQueue<T> items;

	// Token: 0x04002781 RID: 10113
	private Dictionary<ulong, T> idsToItems;

	// Token: 0x04002782 RID: 10114
	private ulong lastSentPackage;

	// Token: 0x04002783 RID: 10115
	private ulong oldestSentPackage;
}

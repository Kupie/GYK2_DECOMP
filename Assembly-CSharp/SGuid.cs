using System;
using UnityEngine;

// Token: 0x02000B02 RID: 2818
[Serializable]
public class SGuid : IEquatable<SGuid>
{
	// Token: 0x17000B4F RID: 2895
	// (get) Token: 0x06004B14 RID: 19220 RVA: 0x001621E0 File Offset: 0x001603E0
	// (set) Token: 0x06004B15 RID: 19221 RVA: 0x001621E8 File Offset: 0x001603E8
	public string Id
	{
		get
		{
			return this.id;
		}
		set
		{
			this.id = value;
			this.guid = new Guid(this.id);
		}
	}

	// Token: 0x17000B50 RID: 2896
	// (get) Token: 0x06004B16 RID: 19222 RVA: 0x00162202 File Offset: 0x00160402
	public Guid Guid
	{
		get
		{
			if (this.guid == Guid.Empty)
			{
				this.guid = new Guid(this.id);
			}
			return this.guid;
		}
	}

	// Token: 0x17000B51 RID: 2897
	// (get) Token: 0x06004B17 RID: 19223 RVA: 0x0016222D File Offset: 0x0016042D
	public static SGuid Empty
	{
		get
		{
			return new SGuid(Guid.Empty);
		}
	}

	// Token: 0x06004B18 RID: 19224 RVA: 0x0016223C File Offset: 0x0016043C
	public SGuid()
	{
		this.id = Guid.NewGuid().ToString();
	}

	// Token: 0x06004B19 RID: 19225 RVA: 0x00162268 File Offset: 0x00160468
	public SGuid(Guid newGuid)
	{
		this.id = newGuid.ToString();
		this.guid = newGuid;
	}

	// Token: 0x06004B1A RID: 19226 RVA: 0x0016228A File Offset: 0x0016048A
	public SGuid(string strRepresentation)
	{
		this.guid = Guid.Parse(strRepresentation);
		this.id = strRepresentation;
	}

	// Token: 0x06004B1B RID: 19227 RVA: 0x001622A5 File Offset: 0x001604A5
	public void SetGuid(SGuid newGuid)
	{
		this.id = newGuid.id;
		this.guid = newGuid.Guid;
	}

	// Token: 0x17000B52 RID: 2898
	// (get) Token: 0x06004B1C RID: 19228 RVA: 0x001622BF File Offset: 0x001604BF
	public bool IsEmpty
	{
		get
		{
			return this == SGuid.Empty;
		}
	}

	// Token: 0x06004B1D RID: 19229 RVA: 0x001622CC File Offset: 0x001604CC
	public static SGuid Parse(string strRepresentation)
	{
		return new SGuid(strRepresentation);
	}

	// Token: 0x06004B1E RID: 19230 RVA: 0x001622D4 File Offset: 0x001604D4
	public static bool operator ==(SGuid left, SGuid right)
	{
		return left == right || (left != null && right != null && left.Guid == right.Guid);
	}

	// Token: 0x06004B1F RID: 19231 RVA: 0x001622F5 File Offset: 0x001604F5
	public static bool operator !=(SGuid left, SGuid right)
	{
		return !(left == right);
	}

	// Token: 0x06004B20 RID: 19232 RVA: 0x001621E0 File Offset: 0x001603E0
	public override string ToString()
	{
		return this.id;
	}

	// Token: 0x06004B21 RID: 19233 RVA: 0x00162301 File Offset: 0x00160501
	public bool Equals(SGuid other)
	{
		return other != null && (this == other || (this.id == other.id && object.Equals(this.Guid, other.Guid)));
	}

	// Token: 0x06004B22 RID: 19234 RVA: 0x0016233E File Offset: 0x0016053E
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((SGuid)obj)));
	}

	// Token: 0x06004B23 RID: 19235 RVA: 0x0016236C File Offset: 0x0016056C
	public override int GetHashCode()
	{
		return this.id.GetHashCode();
	}

	// Token: 0x06004B24 RID: 19236 RVA: 0x00162379 File Offset: 0x00160579
	public static bool IsNullOrEmpty(SGuid sGuid)
	{
		return sGuid == null || sGuid.IsEmpty;
	}

	// Token: 0x04003C94 RID: 15508
	[SerializeField]
	private string id;

	// Token: 0x04003C95 RID: 15509
	private Guid guid;
}

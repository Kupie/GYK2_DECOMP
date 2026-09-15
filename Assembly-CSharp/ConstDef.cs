using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020001DF RID: 479
[Serializable]
public class ConstDef : BalanceBaseObject
{
	// Token: 0x17000208 RID: 520
	// (get) Token: 0x06000C15 RID: 3093 RVA: 0x0003CF5A File Offset: 0x0003B15A
	public bool BoolValue
	{
		get
		{
			return this.boolValue;
		}
	}

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x06000C16 RID: 3094 RVA: 0x0003CF62 File Offset: 0x0003B162
	public int IntValue
	{
		get
		{
			return this.intValue;
		}
	}

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x06000C17 RID: 3095 RVA: 0x0003CF6A File Offset: 0x0003B16A
	public float FloatValue
	{
		get
		{
			return this.floatValue;
		}
	}

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06000C18 RID: 3096 RVA: 0x0003CF72 File Offset: 0x0003B172
	public string StringValue
	{
		get
		{
			return this.stringValue;
		}
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x0003CF7A File Offset: 0x0003B17A
	public static ConstDef Get(string constName)
	{
		return GameBalance.Me.GetData<ConstDef>(constName);
	}

	// Token: 0x04000D58 RID: 3416
	[SerializeField]
	private ConstDef.ConstType type;

	// Token: 0x04000D59 RID: 3417
	[SerializeField]
	private bool boolValue;

	// Token: 0x04000D5A RID: 3418
	[SerializeField]
	private int intValue;

	// Token: 0x04000D5B RID: 3419
	[SerializeField]
	private float floatValue;

	// Token: 0x04000D5C RID: 3420
	[SerializeField]
	private string stringValue;

	// Token: 0x020001E0 RID: 480
	public enum ConstType
	{
		// Token: 0x04000D5E RID: 3422
		@bool,
		// Token: 0x04000D5F RID: 3423
		@int,
		// Token: 0x04000D60 RID: 3424
		@float,
		// Token: 0x04000D61 RID: 3425
		@string
	}
}

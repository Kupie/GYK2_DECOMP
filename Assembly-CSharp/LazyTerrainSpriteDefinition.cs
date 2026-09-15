using System;

// Token: 0x020006B0 RID: 1712
[Serializable]
public class LazyTerrainSpriteDefinition
{
	// Token: 0x06002DB1 RID: 11697 RVA: 0x000DAD6C File Offset: 0x000D8F6C
	public void GetTypeAndIndex(out string stype, out int idx)
	{
		if (this.type.Length > 1)
		{
			string text = this.type;
			if (char.IsDigit(text[text.Length - 1]))
			{
				string text2 = this.type;
				idx = int.Parse(text2[text2.Length - 1].ToString() ?? "");
				string text3 = this.type;
				stype = text3.Substring(0, text3.Length - 1);
				return;
			}
		}
		stype = this.type;
		idx = 0;
	}

	// Token: 0x040024B3 RID: 9395
	public string name;

	// Token: 0x040024B4 RID: 9396
	public byte tx;

	// Token: 0x040024B5 RID: 9397
	public byte x;

	// Token: 0x040024B6 RID: 9398
	public byte y;

	// Token: 0x040024B7 RID: 9399
	public byte w;

	// Token: 0x040024B8 RID: 9400
	public byte h;

	// Token: 0x040024B9 RID: 9401
	public string type = "-";
}

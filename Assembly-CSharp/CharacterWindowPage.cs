using System;
using LazyBearTechnology;

// Token: 0x0200090C RID: 2316
public abstract class CharacterWindowPage : LazyWidget<CharacterWindowPageData>
{
	// Token: 0x06003CEE RID: 15598 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x06003CEF RID: 15599 RVA: 0x00123263 File Offset: 0x00121463
	protected T GetData<T>() where T : CharacterWindowPageData
	{
		return this.data as T;
	}
}

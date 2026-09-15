using System;
using LazyBearTechnology;

// Token: 0x02000AF8 RID: 2808
public class PlatformDependentElementGK2 : LazyPlatformDependentElement
{
	// Token: 0x06004AEC RID: 19180 RVA: 0x0016187C File Offset: 0x0015FA7C
	public override bool Init()
	{
		bool flag = base.Init();
		if (SwitchLightPolicy.UseLightRT)
		{
			flag &= this.rtLightInUse;
		}
		else
		{
			flag &= this.rtLightNotInUse;
		}
		base.gameObject.SetActive(flag);
		base.IsActive = flag;
		return flag;
	}

	// Token: 0x04003C7B RID: 15483
	public bool rtLightInUse = true;

	// Token: 0x04003C7C RID: 15484
	public bool rtLightNotInUse = true;
}

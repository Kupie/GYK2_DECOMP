using System;

// Token: 0x02000B1D RID: 2845
[Serializable]
public abstract class ControllableParameter
{
	// Token: 0x06004BF1 RID: 19441 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void Init()
	{
	}

	// Token: 0x06004BF2 RID: 19442 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void OnDisable()
	{
	}

	// Token: 0x06004BF3 RID: 19443
	public abstract void UpdateParameter(float v, WeatherComponent weatherComponent);

	// Token: 0x06004BF4 RID: 19444 RVA: 0x00002318 File Offset: 0x00000518
	protected void Editor_ApplyValueChange()
	{
	}
}

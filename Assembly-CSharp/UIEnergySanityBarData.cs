using System;
using LazyBearTechnology;

// Token: 0x0200087B RID: 2171
public class UIEnergySanityBarData : LazyWidgetDataBase
{
	// Token: 0x1700083C RID: 2108
	// (get) Token: 0x0600377F RID: 14207 RVA: 0x0010BD54 File Offset: 0x00109F54
	public PlayerData PlayerData { get; }

	// Token: 0x1700083D RID: 2109
	// (get) Token: 0x06003780 RID: 14208 RVA: 0x0010BD5C File Offset: 0x00109F5C
	// (set) Token: 0x06003781 RID: 14209 RVA: 0x0010BD64 File Offset: 0x00109F64
	public float EnergyFillValue { get; private set; }

	// Token: 0x1700083E RID: 2110
	// (get) Token: 0x06003782 RID: 14210 RVA: 0x0010BD6D File Offset: 0x00109F6D
	// (set) Token: 0x06003783 RID: 14211 RVA: 0x0010BD75 File Offset: 0x00109F75
	public float InsanityFillValue { get; private set; }

	// Token: 0x06003784 RID: 14212 RVA: 0x0010BD80 File Offset: 0x00109F80
	public UIEnergySanityBarData(GameSave gameSave)
	{
		this.PlayerData = gameSave.playerData;
		this.UpdateFillData(0f);
		PlayerEnergyGameResSystem system = PlayerEnergyGameResSystem.GetSystem();
		system.onValueChanged = (Action<float>)Delegate.Combine(system.onValueChanged, new Action<float>(this.UpdateFillData));
		PlayerInsanityGameResSystem system2 = PlayerInsanityGameResSystem.GetSystem();
		system2.onValueChanged = (Action<float>)Delegate.Combine(system2.onValueChanged, new Action<float>(this.UpdateFillData));
	}

	// Token: 0x06003785 RID: 14213 RVA: 0x0010BDF8 File Offset: 0x00109FF8
	private void UpdateFillData(float value)
	{
		this.EnergyFillValue = this.PlayerData.GetRes("energy", 0f) / 100f;
		this.InsanityFillValue = this.PlayerData.GetRes("insanity", 0f) / 100f;
		Action action = this.onFillValueChanged;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x04002C33 RID: 11315
	public Action onFillValueChanged;
}

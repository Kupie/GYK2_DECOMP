using System;
using LazyBearTechnology;

// Token: 0x020007ED RID: 2029
public class StaminaBarPlayerWidgetData : LazyWidgetDataBase
{
	// Token: 0x170007D7 RID: 2007
	// (get) Token: 0x0600342B RID: 13355 RVA: 0x000FB8A7 File Offset: 0x000F9AA7
	// (set) Token: 0x0600342C RID: 13356 RVA: 0x000FB8AF File Offset: 0x000F9AAF
	public PlayerStaminaGameResSystem StaminaResSystem { get; private set; }

	// Token: 0x0600342D RID: 13357 RVA: 0x000FB8B8 File Offset: 0x000F9AB8
	public StaminaBarPlayerWidgetData()
	{
		this.StaminaResSystem = PlayerStaminaGameResSystem.GetSystem();
		this.staminaSystem = MainGame.PlayerData.staminaSystem;
	}

	// Token: 0x0600342E RID: 13358 RVA: 0x000FB8DB File Offset: 0x000F9ADB
	public void SubscribeToDataChanges()
	{
		if (!this.subscribedToDataChanges)
		{
			this.staminaSystem.OnNotEnoughStamina += this.onNotEnoughStamina;
			this.subscribedToDataChanges = true;
		}
	}

	// Token: 0x0600342F RID: 13359 RVA: 0x000FB8FD File Offset: 0x000F9AFD
	public void UnsubscribeFromDataChanges()
	{
		if (this.subscribedToDataChanges)
		{
			this.staminaSystem.OnNotEnoughStamina -= this.onNotEnoughStamina;
			this.subscribedToDataChanges = false;
		}
	}

	// Token: 0x040029AC RID: 10668
	public Action onNotEnoughStamina;

	// Token: 0x040029AE RID: 10670
	private StaminaSystem staminaSystem;

	// Token: 0x040029AF RID: 10671
	private bool subscribedToDataChanges;
}

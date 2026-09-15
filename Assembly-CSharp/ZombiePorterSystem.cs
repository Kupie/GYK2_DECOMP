using System;

// Token: 0x02000446 RID: 1094
public class ZombiePorterSystem : ICustomUpdatable
{
	// Token: 0x06001CCB RID: 7371 RVA: 0x00086B08 File Offset: 0x00084D08
	public void CustomUpdate(float deltaTime)
	{
		foreach (SGuid sguid in MainGame.ZombieSystemData.zombieOnSceneWgoIds)
		{
			MainGame.ZombieSystemData.GetZombie(sguid).PorterCheckDeliveryStart();
		}
	}
}

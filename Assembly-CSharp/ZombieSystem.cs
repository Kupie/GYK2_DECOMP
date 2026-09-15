using System;

// Token: 0x02000447 RID: 1095
public class ZombieSystem : ICustomUpdatable
{
	// Token: 0x06001CCD RID: 7373 RVA: 0x00086B6C File Offset: 0x00084D6C
	public void CustomUpdate(float deltaTime)
	{
		foreach (SGuid sguid in MainGame.ZombieSystemData.zombieOnSceneWgoIds)
		{
			MainGame.ZombieSystemData.GetZombie(sguid).CustomUpdate(deltaTime);
		}
	}
}

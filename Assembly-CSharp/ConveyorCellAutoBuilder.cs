using System;
using UnityEngine;

// Token: 0x02000151 RID: 337
public class ConveyorCellAutoBuilder : MonoBehaviour
{
	// Token: 0x06000803 RID: 2051 RVA: 0x00027598 File Offset: 0x00025798
	public Wgo AutoBuildCell()
	{
		ConveyorWgoData conveyorWgoData = new ConveyorWgoData(ConveyorElementType.Cell, "conveyor_cell", base.transform.position, MainGame.PlayerController.CurrentGameScene.Id);
		conveyorWgoData.MainWgoPartData.variationId = "start";
		conveyorWgoData.MainWgoPartData.rotationIndex = this.rotationIndex;
		Wgo wgo = MainGame.PlayerController.CurrentGameScene.AddWgoData(conveyorWgoData, false);
		if (wgo == null)
		{
			Debug.LogError("Failed to build conveyor cell");
			return null;
		}
		return wgo;
	}

	// Token: 0x040009FE RID: 2558
	[SerializeField]
	private int rotationIndex;
}

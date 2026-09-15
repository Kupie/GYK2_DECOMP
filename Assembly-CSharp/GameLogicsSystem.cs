using System;

// Token: 0x02000433 RID: 1075
public class GameLogicsSystem : ICustomUpdatable
{
	// Token: 0x170004E8 RID: 1256
	// (get) Token: 0x06001C74 RID: 7284 RVA: 0x000850FB File Offset: 0x000832FB
	private GameLogicsSystemData Data
	{
		get
		{
			return MainGame.Instance.GameSave.gameLogicSystemData;
		}
	}

	// Token: 0x06001C75 RID: 7285 RVA: 0x0008510C File Offset: 0x0008330C
	public void CustomUpdate(float deltaTime)
	{
		foreach (GameLogicData gameLogicData in this.Data.gameLogics)
		{
			EnvironmentData environmentData = MainGame.Instance.GameSave.environmentData;
			if (gameLogicData is CustomGameLogicData)
			{
				goto IL_004D;
			}
			GameLogicDef definition = gameLogicData.Definition;
			if (definition != null && definition.gameLogicStartType == GameLogicStartType.Period)
			{
				goto IL_004D;
			}
			IL_007D:
			GameLogicDef definition2 = gameLogicData.Definition;
			if (definition2 != null && definition2.gameLogicStartType == GameLogicStartType.Day && environmentData.CurrentDayNumber == ConstDef.Get(gameLogicData.Definition.dayNumber).IntValue && environmentData.TimeOfDay >= gameLogicData.Definition.dayTime && environmentData.Day > gameLogicData.lastExecDay)
			{
				gameLogicData.lastExecDay = environmentData.Day;
				gameLogicData.TryExecute();
				continue;
			}
			continue;
			IL_004D:
			if (environmentData.Day > gameLogicData.execDay || (environmentData.Day == gameLogicData.execDay && environmentData.TimeOfDay >= gameLogicData.execTime))
			{
				gameLogicData.TryExecute();
				goto IL_007D;
			}
			goto IL_007D;
		}
	}
}

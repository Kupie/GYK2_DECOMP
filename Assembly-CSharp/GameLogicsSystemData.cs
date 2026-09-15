using System;
using System.Collections.Generic;

// Token: 0x0200049F RID: 1183
[Serializable]
public class GameLogicsSystemData
{
	// Token: 0x06001F7D RID: 8061 RVA: 0x00095168 File Offset: 0x00093368
	public void PrepareForGame()
	{
		this.gameLogics.RemoveAll((GameLogicData x) => !GameBalance.Me.gameLogicsDefs.Contains(x.Definition));
		using (List<GameLogicDef>.Enumerator enumerator = GameBalance.Me.gameLogicsDefs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameLogicDef def = enumerator.Current;
				if (!this.gameLogics.Exists((GameLogicData x) => x.Definition == def))
				{
					this.gameLogics.Add(new GameLogicData(def.id));
				}
			}
		}
		this.gameLogics.ForEach(delegate(GameLogicData data)
		{
			data.Init();
		});
	}

	// Token: 0x06001F7E RID: 8062 RVA: 0x0009524C File Offset: 0x0009344C
	public void PrepareForNewGame()
	{
		this.gameLogics.Clear();
		using (List<GameLogicDef>.Enumerator enumerator = GameBalance.Me.gameLogicsDefs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameLogicDef def = enumerator.Current;
				if (!this.gameLogics.Exists((GameLogicData x) => x.Definition == def))
				{
					this.gameLogics.Add(new GameLogicData(def.id));
				}
			}
		}
		this.gameLogics.ForEach(delegate(GameLogicData data)
		{
			data.Init();
		});
	}

	// Token: 0x04001C36 RID: 7222
	public List<GameLogicData> gameLogics = new List<GameLogicData>();
}

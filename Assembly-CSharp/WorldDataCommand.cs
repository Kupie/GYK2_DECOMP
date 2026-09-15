using System;
using UnityEngine;

// Token: 0x02000747 RID: 1863
[Command]
[Serializable]
public class WorldDataCommand : CommandTargeted<WorldData>
{
	// Token: 0x0600306B RID: 12395 RVA: 0x000E7F98 File Offset: 0x000E6198
	public override void Execute(ulong senderClientId, GameSave gameSave)
	{
		Debug.Log(string.Format("Command: {0} [op:{1}, wgoId:{2}, uID:{3}, pos:{4}, locId:{5}]", new object[] { "WorldDataCommand", this.operation, this.wgoId, this.uniqueId, this.position, this.gameSceneId }));
		WorldDataCommand.Operation operation = this.operation;
		if (operation == WorldDataCommand.Operation.AddWgoToScene)
		{
			WgoData wgoData = new WgoData(this.wgoId, this.position, this.gameSceneId);
			if (!LazyNetwork.NetworkManager.IsHost)
			{
				wgoData.UniqueId.Id = this.uniqueId;
			}
			else
			{
				this.uniqueId = wgoData.UniqueId.Id;
			}
			gameSave.worldData.AddWgoData(wgoData);
			return;
		}
		if (operation != WorldDataCommand.Operation.RemoveWgoFromScene)
		{
			return;
		}
		gameSave.worldData.RemoveWgoDataFromGameScene(SGuid.Parse(this.uniqueId));
	}

	// Token: 0x0600306C RID: 12396 RVA: 0x000E8074 File Offset: 0x000E6274
	public void AddWgoDataToGameScene(WgoData data)
	{
		this.operation = WorldDataCommand.Operation.AddWgoToScene;
		this.wgoId = data.id;
		this.uniqueId = data.UniqueId.Id;
		this.gameSceneId = this.gameSceneId;
		this.position = data.Position;
		this.HandleCommand(this);
	}

	// Token: 0x0600306D RID: 12397 RVA: 0x000E80C4 File Offset: 0x000E62C4
	public void RemoveWgoDataFromGameScene(WgoData data)
	{
		this.operation = WorldDataCommand.Operation.RemoveWgoFromScene;
		this.wgoId = data.id;
		this.uniqueId = data.UniqueId.Id;
		this.gameSceneId = this.gameSceneId;
		this.HandleCommand(this);
	}

	// Token: 0x0600306E RID: 12398 RVA: 0x000E80FD File Offset: 0x000E62FD
	public void MoveWgoDataToAnotherGameScene(string wgoId, string locationIdFrom, string locationIdTo)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600306F RID: 12399 RVA: 0x000E80FD File Offset: 0x000E62FD
	public void ReplaceWgoDataGameScene(WgoData wgoData, string locationIdFrom, string locationIdTo)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003070 RID: 12400 RVA: 0x000E8104 File Offset: 0x000E6304
	public WorldDataCommand(WorldData target)
		: base(target)
	{
	}

	// Token: 0x06003071 RID: 12401 RVA: 0x000E810D File Offset: 0x000E630D
	public WorldDataCommand()
	{
	}

	// Token: 0x04002733 RID: 10035
	[SerializeField]
	private WorldDataCommand.Operation operation;

	// Token: 0x04002734 RID: 10036
	[SerializeField]
	private string wgoId;

	// Token: 0x04002735 RID: 10037
	[SerializeField]
	private string uniqueId;

	// Token: 0x04002736 RID: 10038
	[SerializeField]
	private string gameSceneId;

	// Token: 0x04002737 RID: 10039
	[SerializeField]
	private Vector3 position;

	// Token: 0x02000748 RID: 1864
	public enum Operation : byte
	{
		// Token: 0x04002739 RID: 10041
		None,
		// Token: 0x0400273A RID: 10042
		AddWgoToScene,
		// Token: 0x0400273B RID: 10043
		RemoveWgoFromScene
	}
}

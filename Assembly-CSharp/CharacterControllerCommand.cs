using System;
using UnityEngine;

// Token: 0x02000738 RID: 1848
[Command]
[Serializable]
public class CharacterControllerCommand : Command
{
	// Token: 0x0600303B RID: 12347 RVA: 0x000E78A8 File Offset: 0x000E5AA8
	public override void Execute(ulong senderClientId, GameSave gameSave)
	{
		Debug.Log(string.Format("Command: {0} [op:{1}, itemGUID:{2}]", "CharacterControllerCommand", this.operation, this.itemGuid));
		if (this.operation == CharacterControllerCommand.Operation.UseTool)
		{
			MainGame.PlayerController.PlayerData.toolBeltInventory.GetItemByUniqueId(this.itemGuid);
		}
	}

	// Token: 0x0600303C RID: 12348 RVA: 0x000E78FE File Offset: 0x000E5AFE
	public void UseTool(float tickRate, Item tool)
	{
		this.operation = CharacterControllerCommand.Operation.UseTool;
		this.itemGuid = tool.UniqueId.ToString();
		this.tickRate = tickRate;
	}

	// Token: 0x04002710 RID: 10000
	[SerializeField]
	public CharacterControllerCommand.Operation operation;

	// Token: 0x04002711 RID: 10001
	[SerializeField]
	public string itemGuid;

	// Token: 0x04002712 RID: 10002
	[SerializeField]
	public float tickRate;

	// Token: 0x02000739 RID: 1849
	public enum Operation : byte
	{
		// Token: 0x04002714 RID: 10004
		None,
		// Token: 0x04002715 RID: 10005
		UseTool
	}
}

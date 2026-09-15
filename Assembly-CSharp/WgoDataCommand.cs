using System;
using UnityEngine;

// Token: 0x02000745 RID: 1861
[Command]
[Serializable]
public class WgoDataCommand : CommandTargeted<WgoData>
{
	// Token: 0x06003067 RID: 12391 RVA: 0x000E7EA8 File Offset: 0x000E60A8
	public override void Execute(ulong senderClientId, GameSave gameSave)
	{
		Debug.Log(string.Format("Command: {0} [op:{1}, who:{2}, wgoId:{3}, tickRate:{4}, toolInUseGUID:{5}]", new object[] { "WgoDataCommand", this.operation, this.clientTriggerId, this.wgoUniqueId, this.tickRate, this.toolInUseGuid }));
		if (this.operation == WgoDataCommand.Operation.ApplyTool)
		{
			NetworkPlayer networkPlayer;
			MainGame.Instance.GameSave.GetClient((int)this.clientTriggerId, out networkPlayer, true);
			networkPlayer.playerData.toolBeltInventory.GetItemByUniqueId(this.toolInUseGuid);
		}
	}

	// Token: 0x06003068 RID: 12392 RVA: 0x000E7F48 File Offset: 0x000E6148
	public void ApplyTool(float tickRate, Item tool)
	{
		this.operation = WgoDataCommand.Operation.ApplyTool;
		this.wgoUniqueId = this.target.UniqueId.Id;
		this.tickRate = tickRate;
		this.toolInUseGuid = tool.UniqueId.ToString();
		this.HandleCommand(this);
	}

	// Token: 0x06003069 RID: 12393 RVA: 0x000E7F86 File Offset: 0x000E6186
	public WgoDataCommand()
	{
	}

	// Token: 0x0600306A RID: 12394 RVA: 0x000E7F8E File Offset: 0x000E618E
	public WgoDataCommand(WgoData target)
		: base(target)
	{
	}

	// Token: 0x0400272B RID: 10027
	[SerializeField]
	private WgoDataCommand.Operation operation;

	// Token: 0x0400272C RID: 10028
	[SerializeField]
	private ulong clientTriggerId;

	// Token: 0x0400272D RID: 10029
	[SerializeField]
	private string wgoUniqueId;

	// Token: 0x0400272E RID: 10030
	[SerializeField]
	private float tickRate;

	// Token: 0x0400272F RID: 10031
	[SerializeField]
	private string toolInUseGuid;

	// Token: 0x02000746 RID: 1862
	public enum Operation
	{
		// Token: 0x04002731 RID: 10033
		None,
		// Token: 0x04002732 RID: 10034
		ApplyTool
	}
}

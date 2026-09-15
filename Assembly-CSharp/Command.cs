using System;
using System.Collections.ObjectModel;
using System.Reflection;
using LazyBearTechnology;

// Token: 0x0200073B RID: 1851
public abstract class Command : ICommand
{
	// Token: 0x06003040 RID: 12352
	public abstract void Execute(ulong senderClientId, GameSave gameSave);

	// Token: 0x06003041 RID: 12353 RVA: 0x000E7992 File Offset: 0x000E5B92
	public virtual byte[] Serialize()
	{
		return LazySerializer.Serialize<Command>(this);
	}

	// Token: 0x06003042 RID: 12354 RVA: 0x000E799A File Offset: 0x000E5B9A
	public virtual void Deserialize(byte[] data)
	{
		LazySerializer.DeserializeInto<Command>(this, data);
	}

	// Token: 0x06003043 RID: 12355 RVA: 0x000E79A3 File Offset: 0x000E5BA3
	protected int GetCommandSerializedFieldsSize<T>(T obj) where T : Command
	{
		return CommandFactory.GetCommandTypeFieldsSize<T>(obj);
	}

	// Token: 0x06003044 RID: 12356 RVA: 0x000E79AB File Offset: 0x000E5BAB
	protected ReadOnlyCollection<FieldInfo> GetCommandSerializedFields<T>(T obj) where T : Command
	{
		return CommandFactory.GetCommandTypeSerializedFields<T>(obj);
	}

	// Token: 0x06003045 RID: 12357 RVA: 0x000E79B3 File Offset: 0x000E5BB3
	protected virtual void HandleCommand(Command command)
	{
		LazyNetwork.ConnectionManager.CurrentState.SendNetworkData<Command>(command);
	}
}

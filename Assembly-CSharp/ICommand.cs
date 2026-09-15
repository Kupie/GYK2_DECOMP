using System;

// Token: 0x02000741 RID: 1857
public interface ICommand
{
	// Token: 0x06003057 RID: 12375
	void Execute(ulong senderClientId, GameSave gameSave);

	// Token: 0x06003058 RID: 12376
	byte[] Serialize();

	// Token: 0x06003059 RID: 12377
	void Deserialize(byte[] data);
}

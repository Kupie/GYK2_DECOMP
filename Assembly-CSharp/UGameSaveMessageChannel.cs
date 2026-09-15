using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

// Token: 0x0200076A RID: 1898
public class UGameSaveMessageChannel : UNetworkMessageChannel<GameSave>
{
	// Token: 0x0600314C RID: 12620 RVA: 0x000E9F5C File Offset: 0x000E815C
	public UGameSaveMessageChannel()
	{
		this.networkDelivery = NetworkDelivery.ReliableFragmentedSequenced;
	}

	// Token: 0x0600314D RID: 12621 RVA: 0x000E9F6C File Offset: 0x000E816C
	public override void Publish(GameSave data, ulong customClientId = 0UL)
	{
		try
		{
			byte[] array = BinaryDataSerializer.SerializeData<GameSave>(data);
			using (FastBufferWriter fastBufferWriter = new FastBufferWriter(array.Length, Allocator.Temp, -1))
			{
				if (!fastBufferWriter.TryBeginWrite(array.Length))
				{
					throw new Exception("Can't write data");
				}
				fastBufferWriter.WriteBytes(array, -1, 0);
				base.SendMessage(fastBufferWriter, customClientId);
			}
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
			throw;
		}
	}

	// Token: 0x0600314E RID: 12622 RVA: 0x000E9FEC File Offset: 0x000E81EC
	public override void OnReceive(ulong senderClientId, IDisposable messagePayload)
	{
		FastBufferReader fastBufferReader = (FastBufferReader)messagePayload;
		int num = fastBufferReader.Length - fastBufferReader.Position;
		byte[] array = new byte[num];
		if (fastBufferReader.TryBeginRead(num))
		{
			fastBufferReader.ReadBytes(ref array, array.Length, 0);
			LobbyHelper.Client_InitGameSave(BinaryDataSerializer.DeserializeData<GameSave>(array));
			return;
		}
		throw new Exception("Can't read data");
	}
}

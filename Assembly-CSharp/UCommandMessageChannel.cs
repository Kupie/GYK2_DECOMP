using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000768 RID: 1896
public class UCommandMessageChannel : UNetworkMessageChannel<ICommand>
{
	// Token: 0x06003146 RID: 12614 RVA: 0x000E9D28 File Offset: 0x000E7F28
	public override void Publish(ICommand data, ulong customClientId = 0UL)
	{
		try
		{
			byte[] array = CommandFactory.SerializeCommand(data);
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

	// Token: 0x06003147 RID: 12615 RVA: 0x000E9DA8 File Offset: 0x000E7FA8
	public override void OnReceive(ulong senderClientId, IDisposable messagePayload)
	{
		FastBufferReader fastBufferReader = (FastBufferReader)messagePayload;
		int num = fastBufferReader.Length - fastBufferReader.Position;
		byte[] array = new byte[num];
		if (fastBufferReader.TryBeginRead(num))
		{
			fastBufferReader.ReadBytes(ref array, array.Length, 0);
			ICommand command = CommandFactory.DeserializeCommand(array);
			base.NotifyOnMessageReceive(command, senderClientId);
			return;
		}
		throw new Exception("Can't read data");
	}
}

using System;
using LazyBearTechnology;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

// Token: 0x0200076C RID: 1900
public class UOrderMessageChannel : UNetworkMessageChannel<OrderMessage>
{
	// Token: 0x06003155 RID: 12629 RVA: 0x000EA150 File Offset: 0x000E8350
	public override void Publish(OrderMessage data, ulong customClientId = 0UL)
	{
		try
		{
			byte[] array = LazySerializer.Serialize<OrderMessage>(data);
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

	// Token: 0x06003156 RID: 12630 RVA: 0x000EA1D0 File Offset: 0x000E83D0
	public override void OnReceive(ulong senderClientId, IDisposable messagePayload)
	{
		FastBufferReader fastBufferReader = (FastBufferReader)messagePayload;
		int num = fastBufferReader.Length - fastBufferReader.Position;
		byte[] array = new byte[num];
		if (fastBufferReader.TryBeginRead(num))
		{
			fastBufferReader.ReadBytes(ref array, array.Length, 0);
			LazySerializer.Deserialize<OrderMessage>(array).Execute(senderClientId);
			return;
		}
		throw new Exception("Can't read data");
	}
}

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000769 RID: 1897
public class UCommandPackageMessageChannel : UNetworkMessageChannel<CommandPackage>
{
	// Token: 0x06003149 RID: 12617 RVA: 0x000E9E0C File Offset: 0x000E800C
	public UCommandPackageMessageChannel()
	{
		this.networkDelivery = NetworkDelivery.ReliableFragmentedSequenced;
	}

	// Token: 0x0600314A RID: 12618 RVA: 0x000E9E1C File Offset: 0x000E801C
	public override void Publish(CommandPackage data, ulong customClientId = 0UL)
	{
		try
		{
			byte[] array = null;
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			using (MemoryStream memoryStream = new MemoryStream())
			{
				binaryFormatter.Serialize(memoryStream, data);
				array = memoryStream.ToArray();
			}
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

	// Token: 0x0600314B RID: 12619 RVA: 0x000E9EC8 File Offset: 0x000E80C8
	public override void OnReceive(ulong senderClientId, IDisposable messagePayload)
	{
		FastBufferReader fastBufferReader = (FastBufferReader)messagePayload;
		int num = fastBufferReader.Length - fastBufferReader.Position;
		byte[] array = new byte[num];
		if (fastBufferReader.TryBeginRead(num))
		{
			fastBufferReader.ReadBytes(ref array, array.Length, 0);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			CommandPackage commandPackage;
			using (MemoryStream memoryStream = new MemoryStream(array))
			{
				commandPackage = (CommandPackage)binaryFormatter.Deserialize(memoryStream);
			}
			base.NotifyOnMessageReceive(commandPackage, senderClientId);
			return;
		}
		throw new Exception("Can't read data");
	}
}

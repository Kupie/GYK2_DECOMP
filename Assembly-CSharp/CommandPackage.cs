using System;
using System.Collections.Generic;

// Token: 0x02000764 RID: 1892
[Serializable]
public class CommandPackage
{
	// Token: 0x17000789 RID: 1929
	// (get) Token: 0x06003121 RID: 12577 RVA: 0x000E955C File Offset: 0x000E775C
	public int PackageSize
	{
		get
		{
			int num = 0;
			for (int i = 0; i < this.serializedCommands.Count; i++)
			{
				num += this.serializedCommands[i].Length;
			}
			return num;
		}
	}

	// Token: 0x06003122 RID: 12578 RVA: 0x00021B94 File Offset: 0x0001FD94
	public CommandPackage()
	{
	}

	// Token: 0x06003123 RID: 12579 RVA: 0x000E9593 File Offset: 0x000E7793
	public CommandPackage(ulong packageId)
	{
		this.packageId = packageId;
		this.serializedCommands = new List<byte[]>();
	}

	// Token: 0x06003124 RID: 12580 RVA: 0x000E95B0 File Offset: 0x000E77B0
	public bool TryAddCommand(ICommand command)
	{
		byte[] array = CommandFactory.SerializeCommand(command);
		int maxPayloadSize = LazyNetwork.NetworkManager.MaxPayloadSize;
		if (array.Length > maxPayloadSize)
		{
			throw new Exception(string.Format("Command size is way too large. MaxPayloadSize = {0}", maxPayloadSize));
		}
		if (this.PackageSize + array.Length > maxPayloadSize)
		{
			return false;
		}
		this.serializedCommands.Add(array);
		return true;
	}

	// Token: 0x06003125 RID: 12581 RVA: 0x000E9607 File Offset: 0x000E7807
	public override string ToString()
	{
		return string.Format("[Package #{0}]: commands = {1}; size = {2} bytes", this.packageId, this.serializedCommands.Count, this.PackageSize);
	}

	// Token: 0x04002776 RID: 10102
	public ulong packageId;

	// Token: 0x04002777 RID: 10103
	public List<byte[]> serializedCommands;
}

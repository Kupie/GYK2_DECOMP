using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using LinqTools;
using UnityEngine;

// Token: 0x0200073C RID: 1852
public static class CommandFactory
{
	// Token: 0x06003047 RID: 12359 RVA: 0x000E79C5 File Offset: 0x000E5BC5
	static CommandFactory()
	{
		CommandFactory.RegisterCommands();
	}

	// Token: 0x06003048 RID: 12360 RVA: 0x000E79E8 File Offset: 0x000E5BE8
	private static void RegisterCommands()
	{
		Type commandType = typeof(ICommand);
		Type commandAttributeType = typeof(CommandAttribute);
		foreach (Type type in from p in AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly s) => s.GetTypes())
			where commandType.IsAssignableFrom(p) && p.IsDefined(commandAttributeType, false)
			select p)
		{
			CommandFactory.CommandTypeData commandTypeData = new CommandFactory.CommandTypeData(type, CommandFactory.typeUId += 1);
			CommandFactory.types.Add(type.Name, commandTypeData);
			CommandFactory.typesById.Add(commandTypeData.id, commandTypeData.type);
			foreach (FieldInfo fieldInfo in CommandFieldAttributeHelper.GetCommandSerializedFields(type))
			{
				commandTypeData.AddField(fieldInfo);
			}
		}
		Debug.Log(string.Format("Command types registered count: {0}", CommandFactory.types.Count<KeyValuePair<string, CommandFactory.CommandTypeData>>()));
	}

	// Token: 0x06003049 RID: 12361 RVA: 0x000E7B30 File Offset: 0x000E5D30
	public static byte[] SerializeCommand(ICommand data)
	{
		byte[] array2;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				binaryWriter.Write(CommandFactory.types[data.GetType().Name].id);
				byte[] array = data.Serialize();
				binaryWriter.Write(array.Length);
				binaryWriter.Write(array);
				array2 = memoryStream.ToArray();
			}
		}
		return array2;
	}

	// Token: 0x0600304A RID: 12362 RVA: 0x000E7BBC File Offset: 0x000E5DBC
	public static ICommand DeserializeCommand(byte[] data)
	{
		ICommand command2;
		using (MemoryStream memoryStream = new MemoryStream(data))
		{
			using (BinaryReader binaryReader = new BinaryReader(memoryStream))
			{
				ushort num = binaryReader.ReadUInt16();
				int num2 = binaryReader.ReadInt32();
				byte[] array = binaryReader.ReadBytes(num2);
				ICommand command = (ICommand)Activator.CreateInstance(CommandFactory.typesById[num]);
				command.Deserialize(array);
				command2 = command;
			}
		}
		return command2;
	}

	// Token: 0x0600304B RID: 12363 RVA: 0x000E7C44 File Offset: 0x000E5E44
	public static int GetCommandTypeFieldsSize<T>(T obj) where T : Command
	{
		return CommandFactory.types[obj.GetType().Name].GetFieldsSizeInByte<T>(obj);
	}

	// Token: 0x0600304C RID: 12364 RVA: 0x000E7C66 File Offset: 0x000E5E66
	public static ReadOnlyCollection<FieldInfo> GetCommandTypeSerializedFields<T>(T obj) where T : Command
	{
		return CommandFactory.types[obj.GetType().Name].fields.AsReadOnly();
	}

	// Token: 0x0400271A RID: 10010
	private static ushort typeUId = 0;

	// Token: 0x0400271B RID: 10011
	private static readonly Dictionary<string, CommandFactory.CommandTypeData> types = new Dictionary<string, CommandFactory.CommandTypeData>();

	// Token: 0x0400271C RID: 10012
	private static readonly Dictionary<ushort, Type> typesById = new Dictionary<ushort, Type>();

	// Token: 0x0200073D RID: 1853
	private class CommandTypeData
	{
		// Token: 0x0600304D RID: 12365 RVA: 0x000E7C8C File Offset: 0x000E5E8C
		public CommandTypeData(Type type, ushort id)
		{
			this.type = type;
			this.id = id;
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x000E7CB8 File Offset: 0x000E5EB8
		public void AddField(FieldInfo fieldInfo)
		{
			this.fields.Add(fieldInfo);
			this.fieldsCache.Add((ushort)this.fields.Count, fieldInfo);
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x000E7CDE File Offset: 0x000E5EDE
		public int GetFieldsSizeInByte<T>(T obj) where T : Command
		{
			return CommandFieldAttributeHelper.GetCommandFieldsSerializedSize<T>(obj, this.fields);
		}

		// Token: 0x0400271D RID: 10013
		public Type type;

		// Token: 0x0400271E RID: 10014
		public ushort id;

		// Token: 0x0400271F RID: 10015
		public List<FieldInfo> fields = new List<FieldInfo>();

		// Token: 0x04002720 RID: 10016
		public Dictionary<ushort, FieldInfo> fieldsCache = new Dictionary<ushort, FieldInfo>();
	}
}

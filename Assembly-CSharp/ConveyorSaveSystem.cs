using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200048B RID: 1163
public static class ConveyorSaveSystem
{
	// Token: 0x1700053B RID: 1339
	// (get) Token: 0x06001EDC RID: 7900 RVA: 0x00091FC4 File Offset: 0x000901C4
	private static ConveyorPresetSerializer Serializer
	{
		get
		{
			if (ConveyorSaveSystem.serializer != null)
			{
				return ConveyorSaveSystem.serializer;
			}
			ConveyorSaveSystem.serializer = new ConveyorPresetSerializer(".bytes", null, null);
			return ConveyorSaveSystem.serializer;
		}
	}

	// Token: 0x06001EDD RID: 7901 RVA: 0x00091FEC File Offset: 0x000901EC
	public static List<ConveyorPreset> ReadConveyorPresets(string folder)
	{
		List<ConveyorPreset> list2;
		try
		{
			List<ConveyorPreset> presets = null;
			ConveyorSaveSystem.lazySaveSystem.LoadAll<ConveyorPreset>(folder, delegate([TupleElementNames(new string[] { "data", "fileName" })] List<ValueTuple<ConveyorPreset, string>> list)
			{
				ConveyorSaveSystem.<>c__DisplayClass5_1 CS$<>8__locals2;
				CS$<>8__locals2.list = list;
				base.<ReadConveyorPresets>g__AfterLoadAll|1(ref CS$<>8__locals2);
			});
			list2 = presets;
		}
		catch (Exception ex)
		{
			Debug.Log(string.Format("Error during load save slots:[{0}]", ex));
			list2 = new List<ConveyorPreset>();
		}
		return list2;
	}

	// Token: 0x06001EDE RID: 7902 RVA: 0x00092050 File Offset: 0x00090250
	public static void LoadConveyorPreset(string directory, string presetName, Action<ConveyorPreset> callback)
	{
		try
		{
			ConveyorSaveSystem.lazySaveSystem.Load<ConveyorPreset>(directory, presetName, callback);
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Format("Error during load save:[{0}]", ex));
		}
	}

	// Token: 0x06001EDF RID: 7903 RVA: 0x00092090 File Offset: 0x00090290
	public static void SaveConveyorPreset(ConveyorPreset conveyorPreset, string directory, string presetName, Action callbackSuccessful = null, Action callbackUnsuccessful = null, bool autoOnSaveStart = true)
	{
		try
		{
			ConveyorSaveSystem.lazySaveSystem.Save<ConveyorPreset>(conveyorPreset, directory, presetName, null);
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Format("Error during save:[{0}]", ex));
			if (callbackUnsuccessful != null)
			{
				callbackUnsuccessful();
			}
		}
	}

	// Token: 0x04001BD4 RID: 7124
	private const string DATA_FILE_EXTENSION = ".bytes";

	// Token: 0x04001BD5 RID: 7125
	private static ConveyorPresetSerializer serializer;

	// Token: 0x04001BD6 RID: 7126
	public static LazySaveSystem lazySaveSystem = new LazySaveSystem(new ValueTuple<Type, IDataSerializer>[]
	{
		new ValueTuple<Type, IDataSerializer>(typeof(ConveyorPreset), ConveyorSaveSystem.Serializer)
	});
}

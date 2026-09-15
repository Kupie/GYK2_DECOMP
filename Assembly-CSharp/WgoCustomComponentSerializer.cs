using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200068D RID: 1677
public static class WgoCustomComponentSerializer
{
	// Token: 0x06002CE3 RID: 11491 RVA: 0x000D5D44 File Offset: 0x000D3F44
	public static void SaveToData(Wgo wgo, WgoData wgoData)
	{
		if (wgo == null || wgoData == null)
		{
			return;
		}
		List<WgoCustomComponentData> customComponentsData = wgoData.customComponentsData;
		customComponentsData.Clear();
		foreach (MonoBehaviour monoBehaviour in wgo.GetComponentsInChildren<MonoBehaviour>())
		{
			if (!(monoBehaviour == null) && !(monoBehaviour is Wgo))
			{
				Type type = monoBehaviour.GetType();
				Type customComponentInterface = WgoCustomComponentSerializer.GetCustomComponentInterface(type);
				if (!(customComponentInterface == null))
				{
					try
					{
						object obj = customComponentInterface.GetMethod("OnSave").Invoke(monoBehaviour, null);
						if (obj != null)
						{
							string text = JsonUtility.ToJson(obj, true);
							if (!string.IsNullOrEmpty(text) && !(text == "{}"))
							{
								customComponentsData.Add(new WgoCustomComponentData
								{
									typeName = type.AssemblyQualifiedName,
									json = text
								});
							}
						}
					}
					catch (Exception ex)
					{
						Debug.LogError("[WgoCustomComponentSerializer] SaveToData failed for " + type.Name + ": " + ex.Message, wgo);
					}
				}
			}
		}
	}

	// Token: 0x06002CE4 RID: 11492 RVA: 0x000D5E60 File Offset: 0x000D4060
	public static void RestoreComponents(Wgo wgo, WgoData wgoData)
	{
		if (wgo == null || wgoData == null)
		{
			return;
		}
		foreach (WgoCustomComponentData wgoCustomComponentData in wgoData.customComponentsData)
		{
			Type type = Type.GetType(wgoCustomComponentData.typeName);
			if (!(type == null))
			{
				MonoBehaviour monoBehaviour = wgo.GetComponentInChildren(type, true) as MonoBehaviour;
				if (monoBehaviour == null)
				{
					Debug.LogError("[WgoCustomComponentSerializer] Component not found: " + type.Name + ", for wgo: " + wgo.Id, wgo);
				}
				else
				{
					Type customComponentInterface = WgoCustomComponentSerializer.GetCustomComponentInterface(type);
					if (!(customComponentInterface == null))
					{
						try
						{
							Type type2 = customComponentInterface.GetGenericArguments()[0];
							object obj = JsonUtility.FromJson(wgoCustomComponentData.json, type2);
							customComponentInterface.GetMethod("OnLoad").Invoke(monoBehaviour, new object[] { obj });
						}
						catch (Exception ex)
						{
							Debug.LogError("[WgoCustomComponentSerializer] Deserialize failed for " + type.Name + ": " + ex.Message, wgo);
						}
					}
				}
			}
		}
	}

	// Token: 0x06002CE5 RID: 11493 RVA: 0x000D5F90 File Offset: 0x000D4190
	public static void ResetComponents(Wgo wgo)
	{
		if (wgo == null)
		{
			return;
		}
		foreach (MonoBehaviour monoBehaviour in wgo.GetComponentsInChildren<MonoBehaviour>(true))
		{
			if (!(monoBehaviour == null) && !(monoBehaviour is Wgo))
			{
				Type type = monoBehaviour.GetType();
				Type customComponentInterface = WgoCustomComponentSerializer.GetCustomComponentInterface(type);
				if (!(customComponentInterface == null))
				{
					try
					{
						customComponentInterface.GetMethod("OnUnload").Invoke(monoBehaviour, null);
					}
					catch (Exception ex)
					{
						Debug.LogError("[WgoCustomComponentSerializer] Unload failed for " + type.Name + ": " + ex.Message, wgo);
					}
				}
			}
		}
	}

	// Token: 0x06002CE6 RID: 11494 RVA: 0x000D6038 File Offset: 0x000D4238
	private static Type GetCustomComponentInterface(Type compType)
	{
		foreach (Type type in compType.GetInterfaces())
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IWgoCustomComponent<>))
			{
				return type;
			}
		}
		return null;
	}
}

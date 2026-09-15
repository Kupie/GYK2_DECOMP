using System;
using UnityEngine;

// Token: 0x020003C3 RID: 963
[Serializable]
public class TownBuildingObjectConfiguration
{
	// Token: 0x04001911 RID: 6417
	[Tooltip("Берется из баланса таба WGOs")]
	public string wgoId;

	// Token: 0x04001912 RID: 6418
	[Tooltip("Глобальная позиция на сцене")]
	public Vector3 position;

	// Token: 0x04001913 RID: 6419
	[Tooltip("Scale объекта на сцене")]
	public Vector3 scale = Vector3.one;

	// Token: 0x04001914 RID: 6420
	[HideInInspector]
	public SGuid createdWgoUniqueId = SGuid.Empty;
}

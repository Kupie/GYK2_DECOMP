using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AAA RID: 2730
[CreateAssetMenu(menuName = "GK2/ConstructorPresetSettings", fileName = "ConstructorPresetSettings")]
public class ConstructorPresetSettings : ScriptableObject
{
	// Token: 0x040039A0 RID: 14752
	public string presetId;

	// Token: 0x040039A1 RID: 14753
	public List<ConstructorPresetLutSetting> lutSettings;
}

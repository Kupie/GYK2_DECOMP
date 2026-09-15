using System;
using System.Collections.Generic;

// Token: 0x020004BB RID: 1211
[Serializable]
public class WeatherData
{
	// Token: 0x04001CD7 RID: 7383
	public float currentPhaseLen;

	// Token: 0x04001CD8 RID: 7384
	public string stateName = "";

	// Token: 0x04001CD9 RID: 7385
	public List<string> enabledWeatherComponents = new List<string>();

	// Token: 0x04001CDA RID: 7386
	public bool hasForceState;

	// Token: 0x04001CDB RID: 7387
	public bool isSoundEnabled;

	// Token: 0x04001CDC RID: 7388
	public bool isIndoorSfxEnabled;

	// Token: 0x04001CDD RID: 7389
	public bool isWeatherPausedByTimeOfDay;

	// Token: 0x04001CDE RID: 7390
	public bool isWeatherPausedByCinematics;
}

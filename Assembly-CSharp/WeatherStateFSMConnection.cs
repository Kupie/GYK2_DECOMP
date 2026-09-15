using System;
using NodeCanvas.StateMachines;

// Token: 0x02000B30 RID: 2864
public class WeatherStateFSMConnection : FSMConnection
{
	// Token: 0x06004C34 RID: 19508 RVA: 0x00167868 File Offset: 0x00165A68
	public override void OnDestroy()
	{
		foreach (FSMWeatherState.WeatherStateExit weatherStateExit in (base.sourceNode as FSMWeatherState).exits)
		{
			if (weatherStateExit.connection == this)
			{
				weatherStateExit.connection = null;
			}
		}
		base.OnDestroy();
	}
}

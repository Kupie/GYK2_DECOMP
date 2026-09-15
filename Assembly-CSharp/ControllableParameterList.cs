using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B1E RID: 2846
[Serializable]
public class ControllableParameterList
{
	// Token: 0x06004BF6 RID: 19446 RVA: 0x00166D58 File Offset: 0x00164F58
	public void Init()
	{
		foreach (ControllableParameter controllableParameter in this.parameters)
		{
			controllableParameter.Init();
		}
	}

	// Token: 0x06004BF7 RID: 19447 RVA: 0x00166DA8 File Offset: 0x00164FA8
	public void UpdateParameters(float v, WeatherComponent weatherComponent)
	{
		for (int i = 0; i < this.parameters.Count; i++)
		{
			this.parameters[i].UpdateParameter(v, weatherComponent);
		}
	}

	// Token: 0x06004BF8 RID: 19448 RVA: 0x00166DE0 File Offset: 0x00164FE0
	public void OnDisable()
	{
		foreach (ControllableParameter controllableParameter in this.parameters)
		{
			controllableParameter.OnDisable();
		}
	}

	// Token: 0x04003D30 RID: 15664
	[SerializeReference]
	public List<ControllableParameter> parameters = new List<ControllableParameter>();
}

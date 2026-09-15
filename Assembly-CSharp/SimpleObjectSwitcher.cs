using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B05 RID: 2821
public class SimpleObjectSwitcher : MonoBehaviour
{
	// Token: 0x06004B27 RID: 19239 RVA: 0x001623A0 File Offset: 0x001605A0
	public void EnableObject(SimpleObjectSwitcher.CustomObject customObject)
	{
		if (customObject == null)
		{
			return;
		}
		foreach (SimpleObjectSwitcher.CustomObject customObject2 in this.controlObjects)
		{
			if (customObject2.go != null)
			{
				customObject2.go.SetActive(false);
			}
		}
		this.currentSelectedObj = customObject;
		if (this.currentSelectedObj.go != null)
		{
			this.currentSelectedObj.go.SetActive(true);
		}
		if (this.currentSelectedObj.lightPreset != null)
		{
			EnvironmentEngine.Instance.ApplyOverridePreset(this.currentSelectedObj.lightPreset, 1f);
		}
	}

	// Token: 0x06004B28 RID: 19240 RVA: 0x00002318 File Offset: 0x00000518
	public void DestroyAllExceptCurrent()
	{
	}

	// Token: 0x06004B29 RID: 19241 RVA: 0x00162464 File Offset: 0x00160664
	private void SwitchToPreviousObject()
	{
		int num = this.currentItemIdx - 1;
		this.currentItemIdx = num;
		if (num < 0)
		{
			this.currentItemIdx = this.controlObjects.Count - 1;
		}
		this.EnableCurrentObject();
	}

	// Token: 0x06004B2A RID: 19242 RVA: 0x001624A0 File Offset: 0x001606A0
	private void SwitchToNextObject()
	{
		int num = this.currentItemIdx + 1;
		this.currentItemIdx = num;
		if (num > this.controlObjects.Count - 1)
		{
			this.currentItemIdx = 0;
		}
		this.EnableCurrentObject();
	}

	// Token: 0x06004B2B RID: 19243 RVA: 0x00002318 File Offset: 0x00000518
	private void FinalizeSwitcher()
	{
	}

	// Token: 0x06004B2C RID: 19244 RVA: 0x001624DC File Offset: 0x001606DC
	private void EnableCurrentObject()
	{
		if (this.controlObjects.Count == 0)
		{
			return;
		}
		this.currentItemIdx = Mathf.Clamp(this.currentItemIdx, 0, this.controlObjects.Count - 1);
		this.currentSelectedObj = this.controlObjects[this.currentItemIdx];
		this.EnableObject(this.currentSelectedObj);
	}

	// Token: 0x04003C9B RID: 15515
	[SerializeField]
	private List<SimpleObjectSwitcher.CustomObject> controlObjects = new List<SimpleObjectSwitcher.CustomObject>();

	// Token: 0x04003C9C RID: 15516
	private SimpleObjectSwitcher.CustomObject currentSelectedObj;

	// Token: 0x04003C9D RID: 15517
	[SerializeField]
	private int currentItemIdx = -1;

	// Token: 0x02000B06 RID: 2822
	[Serializable]
	public class CustomObject
	{
		// Token: 0x04003C9E RID: 15518
		public GameObject go;

		// Token: 0x04003C9F RID: 15519
		public LightEnvironmentPreset lightPreset;
	}
}

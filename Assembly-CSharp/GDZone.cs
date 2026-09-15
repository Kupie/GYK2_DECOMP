using System;
using FlowCanvas;
using UnityEngine;

// Token: 0x02000541 RID: 1345
[RequireComponent(typeof(Collider))]
public class GDZone : MonoBehaviour
{
	// Token: 0x06002287 RID: 8839 RVA: 0x000A1F7C File Offset: 0x000A017C
	private void OnTriggerEnter(Collider collision)
	{
		IPhysicallyMutable componentInChildren = collision.gameObject.GetComponentInChildren<IPhysicallyMutable>();
		if (componentInChildren == null)
		{
			return;
		}
		if (componentInChildren.IsMuted)
		{
			return;
		}
		if (collision.transform.gameObject.layer != 10 || this.isPlayerInside)
		{
			return;
		}
		this.isPlayerInside = true;
		this.enterPoint = collision.transform.position;
		Debug.Log("GDZone.OnTriggerEnter " + base.name, this);
		this.ExecuteEvent(this.onEnter);
		if (!string.IsNullOrEmpty(this.customTag))
		{
			GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.PlayerEnterGDZone, this.customTag);
		}
	}

	// Token: 0x06002288 RID: 8840 RVA: 0x000A2014 File Offset: 0x000A0214
	private void OnTriggerExit(Collider collision)
	{
		if (collision.transform.gameObject.layer != 10 || !this.isPlayerInside)
		{
			return;
		}
		IPhysicallyMutable componentInChildren = collision.gameObject.GetComponentInChildren<IPhysicallyMutable>();
		if (componentInChildren == null)
		{
			return;
		}
		if (componentInChildren.IsMuted)
		{
			return;
		}
		RaycastHit[] array = new RaycastHit[5];
		int num = Physics.RaycastNonAlloc(new Ray(collision.transform.position + Vector3.down * 100f, Vector3.up), array, 200f, 8388608);
		for (int i = 0; i < num; i++)
		{
			RaycastHit raycastHit = array[i];
			GDZone gdzone;
			if (raycastHit.collider.TryGetComponent<GDZone>(out gdzone) && gdzone == this)
			{
				return;
			}
		}
		this.isPlayerInside = false;
		Vector3 vector = collision.transform.position - this.enterPoint;
		Debug.Log("GDZone.OnTriggerExit " + base.name + ", diff_vector = " + vector.ToString(), this);
		this.ExecuteEvent(this.onExit);
		if (vector.x * 2f > 100f)
		{
			this.ExecuteEvent(this.onCrossedToRight);
		}
		if (vector.x * 2f < -100f)
		{
			this.ExecuteEvent(this.onCrossedToLeft);
		}
		if (vector.z * 2f > 100f)
		{
			this.ExecuteEvent(this.onCrossedToDown);
		}
		if (vector.z * 2f < -100f)
		{
			this.ExecuteEvent(this.onCrossedToUp);
		}
		if (!string.IsNullOrEmpty(this.customTag))
		{
			GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.PlayerExitGDZone, this.customTag);
		}
	}

	// Token: 0x06002289 RID: 8841 RVA: 0x000A21B4 File Offset: 0x000A03B4
	private void ExecuteEvent(GDZone.GDZoneEvent e)
	{
		if (e == null || e.flowScript == null || GlobalScriptsManager.HasFlowScript(e.flowScript.name))
		{
			return;
		}
		if (e.flowScript != null)
		{
			GlobalScriptsManager.RunFlowScript(e.flowScript, null, FlowScriptLoadMode.DeserializeOnInit);
		}
	}

	// Token: 0x0600228A RID: 8842 RVA: 0x000A2200 File Offset: 0x000A0400
	public void RecalculateBounds()
	{
		this.p1 = base.transform.position + this.distP1 * 48f;
		this.p2 = base.transform.position + this.distP2 * 48f;
		this.lineDirection = (this.p2 - this.p1).normalized;
		this.perpendicular = Quaternion.AngleAxis(90f, this.lineDirection) * Vector3.forward;
		this.innerPoly = new Vector3[]
		{
			this.p1 + this.perpendicular * this.distSize * 48f,
			this.p2 + this.perpendicular * this.distSize * 48f,
			this.p2 - this.perpendicular * this.distSize * 48f,
			this.p1 - this.perpendicular * this.distSize * 48f
		};
	}

	// Token: 0x04001F1C RID: 7964
	public const float COLLIDER_MIN_SIZE = 100f;

	// Token: 0x04001F1D RID: 7965
	public string customTag;

	// Token: 0x04001F1E RID: 7966
	public GDZone.GDZoneEvent onEnter;

	// Token: 0x04001F1F RID: 7967
	public GDZone.GDZoneEvent onExit;

	// Token: 0x04001F20 RID: 7968
	public GDZone.GDZoneEvent onCrossedToRight;

	// Token: 0x04001F21 RID: 7969
	public GDZone.GDZoneEvent onCrossedToLeft;

	// Token: 0x04001F22 RID: 7970
	public GDZone.GDZoneEvent onCrossedToUp;

	// Token: 0x04001F23 RID: 7971
	public GDZone.GDZoneEvent onCrossedToDown;

	// Token: 0x04001F24 RID: 7972
	private Vector3 enterPoint;

	// Token: 0x04001F25 RID: 7973
	private bool isPlayerInside;

	// Token: 0x04001F26 RID: 7974
	public GDZone.DistanceType distanceCounter;

	// Token: 0x04001F27 RID: 7975
	public Vector2 distP1;

	// Token: 0x04001F28 RID: 7976
	public Vector2 distP2;

	// Token: 0x04001F29 RID: 7977
	public float distSize = 1f;

	// Token: 0x04001F2A RID: 7978
	public float distFadeSize = 2f;

	// Token: 0x04001F2B RID: 7979
	[SerializeField]
	private Vector3 p1;

	// Token: 0x04001F2C RID: 7980
	[SerializeField]
	private Vector3 p2;

	// Token: 0x04001F2D RID: 7981
	[SerializeField]
	private Vector3[] innerPoly;

	// Token: 0x04001F2E RID: 7982
	[SerializeField]
	private Vector3 perpendicular;

	// Token: 0x04001F2F RID: 7983
	[SerializeField]
	private Vector3 lineDirection;

	// Token: 0x02000542 RID: 1346
	public enum DistanceType
	{
		// Token: 0x04001F31 RID: 7985
		None,
		// Token: 0x04001F32 RID: 7986
		Point,
		// Token: 0x04001F33 RID: 7987
		Line
	}

	// Token: 0x02000543 RID: 1347
	[Serializable]
	public class GDZoneEvent
	{
		// Token: 0x04001F34 RID: 7988
		public FlowScript flowScript;
	}
}

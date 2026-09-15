using System;
using UnityEngine;

// Token: 0x02000548 RID: 1352
[ExecuteInEditMode]
public class RopeRenderer : MonoBehaviour
{
	// Token: 0x1700059F RID: 1439
	// (get) Token: 0x060022B5 RID: 8885 RVA: 0x000A2B58 File Offset: 0x000A0D58
	// (set) Token: 0x060022B6 RID: 8886 RVA: 0x000A2B60 File Offset: 0x000A0D60
	public RopePoint StartPoint
	{
		get
		{
			return this.startPoint;
		}
		set
		{
			this.startPoint = value;
		}
	}

	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x060022B7 RID: 8887 RVA: 0x000A2B69 File Offset: 0x000A0D69
	// (set) Token: 0x060022B8 RID: 8888 RVA: 0x000A2B71 File Offset: 0x000A0D71
	public RopePoint EndPoint
	{
		get
		{
			return this.endPoint;
		}
		set
		{
			this.endPoint = value;
		}
	}

	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x060022B9 RID: 8889 RVA: 0x000A2B7A File Offset: 0x000A0D7A
	// (set) Token: 0x060022BA RID: 8890 RVA: 0x000A2B82 File Offset: 0x000A0D82
	public float Looseness
	{
		get
		{
			return this.looseness;
		}
		set
		{
			this.looseness = value;
		}
	}

	// Token: 0x060022BB RID: 8891 RVA: 0x000A2B8B File Offset: 0x000A0D8B
	private void Awake()
	{
		this.InitializeComponents();
	}

	// Token: 0x060022BC RID: 8892 RVA: 0x000A2B93 File Offset: 0x000A0D93
	private void OnEnable()
	{
		this.ValidateRopePoints();
		this.UpdateRopeParameters();
	}

	// Token: 0x060022BD RID: 8893 RVA: 0x000A2BA1 File Offset: 0x000A0DA1
	private void LateUpdate()
	{
		this.CheckForPositionChanges();
		if (this.needsUpdate)
		{
			this.UpdateRopeParameters();
			this.needsUpdate = false;
		}
	}

	// Token: 0x060022BE RID: 8894 RVA: 0x000A2BBE File Offset: 0x000A0DBE
	private void InitializeComponents()
	{
		if (this.meshView == null)
		{
			this.meshView = base.GetComponentInChildren<RopeMeshView>(true);
		}
		if (this.meshView != null)
		{
			this.meshView.EnsureInitialized();
		}
	}

	// Token: 0x060022BF RID: 8895 RVA: 0x000A2BF4 File Offset: 0x000A0DF4
	private void ValidateRopePoints()
	{
		if (this.startPoint == null)
		{
			this.startPoint = base.GetComponentInChildren<RopePoint>(true);
			if (this.startPoint != null)
			{
				Debug.LogWarning("Start point was null, automatically assigned first found RopePoint child.");
			}
		}
		if (this.endPoint == null)
		{
			RopePoint[] componentsInChildren = base.GetComponentsInChildren<RopePoint>(true);
			if (componentsInChildren.Length > 1)
			{
				this.endPoint = componentsInChildren[1];
				Debug.LogWarning("End point was null, automatically assigned second found RopePoint child.");
			}
		}
		if (this.startPoint == null || this.endPoint == null)
		{
			Debug.LogError("RopeRenderer requires two RopePoint child objects!");
		}
	}

	// Token: 0x060022C0 RID: 8896 RVA: 0x000A2C8C File Offset: 0x000A0E8C
	private void CheckForPositionChanges()
	{
		if (this.startPoint == null || this.endPoint == null)
		{
			return;
		}
		Vector3 localPosition = this.startPoint.transform.localPosition;
		Vector3 localPosition2 = this.endPoint.transform.localPosition;
		if (Vector3.Distance(localPosition, this.lastStartPos) > 0.001f || Vector3.Distance(localPosition2, this.lastEndPos) > 0.001f)
		{
			this.lastStartPos = localPosition;
			this.lastEndPos = localPosition2;
			this.needsUpdate = true;
		}
	}

	// Token: 0x060022C1 RID: 8897 RVA: 0x000A2D14 File Offset: 0x000A0F14
	private void UpdateRopeParameters()
	{
		if (this.meshView == null || this.startPoint == null || this.endPoint == null)
		{
			return;
		}
		Vector3 localPosition = this.startPoint.transform.localPosition;
		Vector3 vector = this.endPoint.transform.localPosition - localPosition;
		Vector3 vector2 = vector;
		this.meshView.AlignToEndPoint(vector2);
		float magnitude = new Vector2(vector.x, vector.z).magnitude;
		this.ropeLength = new Vector2(magnitude, vector.y).magnitude;
		Vector3 meshScale = this.meshView.MeshScale;
		Vector2 zero = Vector2.zero;
		Vector2 vector3 = new Vector2(-magnitude / meshScale.x, vector.y / meshScale.y) * 2f;
		this.meshView.UpdateRopeVisuals(zero, vector3, this.looseness, this.ropeThickness, this.ropeColor, this.emissionIntensity, this.gravityStrength, this.textureResolution, this.antiAliasing);
	}

	// Token: 0x060022C2 RID: 8898 RVA: 0x000A2E2E File Offset: 0x000A102E
	public void SetLooseness(float value)
	{
		this.looseness = Mathf.Clamp01(value);
		this.needsUpdate = true;
	}

	// Token: 0x060022C3 RID: 8899 RVA: 0x000A2E43 File Offset: 0x000A1043
	public void SetThickness(float value)
	{
		this.ropeThickness = Mathf.Clamp(value, 0.001f, 0.1f);
		this.needsUpdate = true;
	}

	// Token: 0x060022C4 RID: 8900 RVA: 0x000A2E62 File Offset: 0x000A1062
	public void SetColor(Color color)
	{
		this.ropeColor = color;
		this.needsUpdate = true;
	}

	// Token: 0x060022C5 RID: 8901 RVA: 0x000A2E72 File Offset: 0x000A1072
	public void SetEmission(float intensity)
	{
		this.emissionIntensity = Mathf.Clamp(intensity, 0f, 5f);
		this.needsUpdate = true;
	}

	// Token: 0x060022C6 RID: 8902 RVA: 0x000A2E91 File Offset: 0x000A1091
	public void SetGravityStrength(float value)
	{
		this.gravityStrength = Mathf.Clamp(value, 1f, 10f);
		this.needsUpdate = true;
	}

	// Token: 0x060022C7 RID: 8903 RVA: 0x000A2EB0 File Offset: 0x000A10B0
	private void OnValidate()
	{
		this.needsUpdate = true;
	}

	// Token: 0x060022C8 RID: 8904 RVA: 0x000A2EBC File Offset: 0x000A10BC
	private void OnRopeParameterChanged()
	{
		if (this.meshView != null)
		{
			this.meshView.EnsureInitialized();
			this.meshView.UpdateRopeParameters(this.looseness, this.ropeThickness, this.ropeColor, this.emissionIntensity, this.gravityStrength);
			this.needsUpdate = true;
		}
	}

	// Token: 0x060022C9 RID: 8905 RVA: 0x000A2F12 File Offset: 0x000A1112
	private void OnRopePointChanged()
	{
		this.needsUpdate = true;
		if (Application.isPlaying)
		{
			this.ValidateRopePoints();
		}
	}

	// Token: 0x060022CA RID: 8906 RVA: 0x000A2F28 File Offset: 0x000A1128
	private void OnQualityParameterChanged()
	{
		if (this.meshView != null)
		{
			this.meshView.EnsureInitialized();
			this.meshView.UpdateQualitySettings(this.textureResolution, this.antiAliasing);
		}
	}

	// Token: 0x04001F5B RID: 8027
	[Header("Rope Settings")]
	[SerializeField]
	[Range(0f, 1f)]
	public float looseness = 0.5f;

	// Token: 0x04001F5C RID: 8028
	[SerializeField]
	[Range(0.001f, 0.1f)]
	private float ropeThickness = 0.02f;

	// Token: 0x04001F5D RID: 8029
	[SerializeField]
	public Color ropeColor = Color.white;

	// Token: 0x04001F5E RID: 8030
	[SerializeField]
	[Range(0f, 5f)]
	private float emissionIntensity = 0.3f;

	// Token: 0x04001F5F RID: 8031
	[SerializeField]
	[Range(1f, 10f)]
	private float gravityStrength = 2f;

	// Token: 0x04001F60 RID: 8032
	[Header("Rope Points")]
	[SerializeField]
	private RopePoint startPoint;

	// Token: 0x04001F61 RID: 8033
	[SerializeField]
	private RopePoint endPoint;

	// Token: 0x04001F62 RID: 8034
	[Header("Quality Settings")]
	[SerializeField]
	[Range(64f, 512f)]
	private int textureResolution = 256;

	// Token: 0x04001F63 RID: 8035
	[SerializeField]
	private bool antiAliasing = true;

	// Token: 0x04001F64 RID: 8036
	[Header("Mesh View")]
	[SerializeField]
	private RopeMeshView meshView;

	// Token: 0x04001F65 RID: 8037
	private float ropeLength;

	// Token: 0x04001F66 RID: 8038
	private Vector3 lastStartPos;

	// Token: 0x04001F67 RID: 8039
	private Vector3 lastEndPos;

	// Token: 0x04001F68 RID: 8040
	private bool needsUpdate = true;
}

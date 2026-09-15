using System;
using UnityEngine;

// Token: 0x02000173 RID: 371
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class DeformTextureCamera : MonoBehaviour
{
	// Token: 0x1700016A RID: 362
	// (get) Token: 0x06000926 RID: 2342 RVA: 0x0002EF27 File Offset: 0x0002D127
	private Camera Camera
	{
		get
		{
			if (!this.camera)
			{
				this.camera = base.GetComponent<Camera>();
			}
			return this.camera;
		}
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x0002EF48 File Offset: 0x0002D148
	private void Awake()
	{
		this.SetRenderTexture();
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x0002EF50 File Offset: 0x0002D150
	private void SetRenderTexture()
	{
		if (this.renderTexture)
		{
			Shader.SetGlobalTexture("_ScreenDeformTex", this.renderTexture);
		}
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x0002EF70 File Offset: 0x0002D170
	private void OnPreRender()
	{
		Shader.EnableKeyword("DEFORM_RENDERING");
		if (this.renderTexture == null)
		{
			return;
		}
		Graphics.Blit(this.renderTexture, this.secondRenderTexture);
		this.CalculateCameraOffset();
		this.CalculateCameraMatrices();
		Graphics.Blit(this.secondRenderTexture, this.renderTexture, this.blitMaterial);
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x0002EFCC File Offset: 0x0002D1CC
	private void CalculateCameraOffset()
	{
		Vector2 cameraXY = this.GetCameraXY();
		Vector2 vector = this.prevCameraPos - cameraXY;
		this.blitMaterial.SetVector(DeformTextureCamera.idOffset, vector);
		this.prevCameraPos = cameraXY;
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x0002F00C File Offset: 0x0002D20C
	private void CalculateCameraMatrices()
	{
		Camera camera = this.camera;
		Matrix4x4 worldToCameraMatrix = camera.worldToCameraMatrix;
		Matrix4x4 gpuprojectionMatrix = GL.GetGPUProjectionMatrix(camera.projectionMatrix, true);
		Matrix4x4 matrix4x = gpuprojectionMatrix * worldToCameraMatrix;
		Shader.SetGlobalMatrix(DeformTextureCamera.idCameraViewMatrix, worldToCameraMatrix);
		Shader.SetGlobalMatrix(DeformTextureCamera.idCameraProjectionMatrix, gpuprojectionMatrix);
		Shader.SetGlobalMatrix(DeformTextureCamera.idCameraVpMatrix, matrix4x);
		Vector4 vector = new Vector4(1f / camera.projectionMatrix[1, 1], 1f / camera.projectionMatrix[0, 0], camera.nearClipPlane, camera.farClipPlane);
		Shader.SetGlobalVector(DeformTextureCamera.idCameraProjectionParams, vector);
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x0002F0AA File Offset: 0x0002D2AA
	private void OnPostRender()
	{
		Shader.DisableKeyword("DEFORM_RENDERING");
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x0002F0B8 File Offset: 0x0002D2B8
	private Vector2 GetCameraXY()
	{
		Vector3 position = this.camera.transform.position;
		return new Vector2(position.x / 0.01f / this.pixelSize, position.z / 0.0125f / this.pixelSize);
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x0002F104 File Offset: 0x0002D304
	public void ChangeOrthographicSize(int screenW, int screenH, float mainCameraOrthoSize)
	{
		int num = Mathf.RoundToInt((float)screenW / this.pixelSize);
		int num2 = Mathf.RoundToInt((float)screenH / this.pixelSize);
		this.ChangeRenderTargetSize(num, num2, mainCameraOrthoSize);
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x0002F138 File Offset: 0x0002D338
	public void ChangeRenderTargetSize(int textureWidth, int textureHeight, float mainCameraOrthoSize)
	{
		this.camera.orthographicSize = mainCameraOrthoSize;
		this.pixelSize = (float)ResolutionConfig.PixelSize;
		this.prevCameraPos = Vector2.negativeInfinity;
		if (this.renderTexture)
		{
			RenderTexture.ReleaseTemporary(this.renderTexture);
		}
		if (this.secondRenderTexture)
		{
			RenderTexture.ReleaseTemporary(this.secondRenderTexture);
		}
		Debug.Log(string.Format("DeformTexture: Texture: {0}x{1}, PixelSize: {2}", textureWidth, textureHeight, this.pixelSize));
		this.renderTexture = RenderTexture.GetTemporary(textureWidth, textureHeight, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
		this.renderTexture.filterMode = FilterMode.Trilinear;
		this.secondRenderTexture = RenderTexture.GetTemporary(textureWidth, textureHeight, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
		this.secondRenderTexture.filterMode = FilterMode.Trilinear;
		this.camera.targetTexture = this.renderTexture;
		this.SetRenderTexture();
		Shader.SetGlobalFloat("_DeformCameraPixelScale", 1f / this.pixelSize);
		this.CalculateCameraMatrices();
		LightRTManager.NotifyWorldRenderTargetChanged();
	}

	// Token: 0x04000AD3 RID: 2771
	public float pixelSize = 1f;

	// Token: 0x04000AD4 RID: 2772
	[SerializeField]
	private Camera camera;

	// Token: 0x04000AD5 RID: 2773
	private RenderTexture renderTexture;

	// Token: 0x04000AD6 RID: 2774
	private RenderTexture secondRenderTexture;

	// Token: 0x04000AD7 RID: 2775
	public Material blitMaterial;

	// Token: 0x04000AD8 RID: 2776
	private Vector2 prevCameraPos = Vector2.negativeInfinity;

	// Token: 0x04000AD9 RID: 2777
	private static readonly int idOffset = Shader.PropertyToID("_Offset");

	// Token: 0x04000ADA RID: 2778
	private static readonly int idCameraViewMatrix = Shader.PropertyToID("_CameraViewMatrix");

	// Token: 0x04000ADB RID: 2779
	private static readonly int idCameraProjectionMatrix = Shader.PropertyToID("_CameraProjectionMatrix");

	// Token: 0x04000ADC RID: 2780
	private static readonly int idCameraVpMatrix = Shader.PropertyToID("_CameraVPMatrix");

	// Token: 0x04000ADD RID: 2781
	private static readonly int idCameraProjectionParams = Shader.PropertyToID("_CameraProjectionParams");
}

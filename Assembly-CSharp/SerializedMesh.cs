using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000B00 RID: 2816
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class SerializedMesh : MonoBehaviour
{
	// Token: 0x17000B4D RID: 2893
	// (get) Token: 0x06004B06 RID: 19206 RVA: 0x00161F13 File Offset: 0x00160113
	public MeshFilter MeshFilter
	{
		get
		{
			if (this.meshFilter != null)
			{
				return this.meshFilter;
			}
			this.meshFilter = base.GetComponent<MeshFilter>();
			return this.meshFilter;
		}
	}

	// Token: 0x17000B4E RID: 2894
	// (get) Token: 0x06004B07 RID: 19207 RVA: 0x00161F3C File Offset: 0x0016013C
	public Vector3[] Vertices
	{
		get
		{
			return this.vertices;
		}
	}

	// Token: 0x06004B08 RID: 19208 RVA: 0x00161F44 File Offset: 0x00160144
	public void RebuildMesh()
	{
		if (!this.isSerialized)
		{
			Debug.LogError("Trying build the mesh before it's serialized. Be sure you were serialized it first");
			return;
		}
		Mesh mesh = new Mesh
		{
			vertices = this.vertices,
			triangles = this.triangles
		};
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
		mesh.RecalculateBounds();
		for (int i = 0; i < 8; i++)
		{
			mesh.SetUVs(i, this.uvs[i].uvs);
		}
		this.MeshFilter.sharedMesh = mesh;
	}

	// Token: 0x06004B09 RID: 19209 RVA: 0x00161FC0 File Offset: 0x001601C0
	public void Serialize()
	{
		this.SerializeFrom(this.MeshFilter.sharedMesh);
	}

	// Token: 0x06004B0A RID: 19210 RVA: 0x00161FD4 File Offset: 0x001601D4
	public void SerializeFrom(Mesh mesh)
	{
		this.isSerialized = true;
		this.uvs = new UVs[8];
		this.vertices = mesh.vertices;
		this.triangles = mesh.triangles;
		for (int i = 0; i < 8; i++)
		{
			this.EnsureHasUVsByChannel(i);
			mesh.GetUVs(i, this.uvs[i].uvs);
		}
	}

	// Token: 0x06004B0B RID: 19211 RVA: 0x00162034 File Offset: 0x00160234
	public void GetUVs(int channel, List<Vector4> uvs)
	{
		this.EnsureIsCorrectChannelValue(channel);
		uvs.Clear();
		uvs.AddRange(Enumerable.Repeat<Vector4>(Vector4.zero, this.vertices.Length));
		for (int i = 0; i < this.uvs[channel].uvs.Count; i++)
		{
			uvs[i] = this.uvs[channel].uvs[i];
		}
	}

	// Token: 0x06004B0C RID: 19212 RVA: 0x001620A0 File Offset: 0x001602A0
	public void SetUVs(int channel, List<Vector4> uvs)
	{
		this.EnsureIsCorrectChannelValue(channel);
		this.EnsureHasUVsByChannel(channel);
		for (int i = 0; i < uvs.Count; i++)
		{
			this.uvs[channel].uvs[i] = uvs[i];
		}
		if (this.meshFilter != null)
		{
			this.meshFilter.sharedMesh.SetUVs(channel, uvs);
		}
	}

	// Token: 0x06004B0D RID: 19213 RVA: 0x00162106 File Offset: 0x00160306
	public void SetUVs(int channel, Vector4[] uvs)
	{
		this.SetUVs(channel, uvs.ToList<Vector4>());
	}

	// Token: 0x06004B0E RID: 19214 RVA: 0x00162118 File Offset: 0x00160318
	public void EnsureHasUVsByChannel(int channel)
	{
		this.EnsureIsCorrectChannelValue(channel);
		if (!this.isSerialized)
		{
			return;
		}
		int num = this.vertices.Length;
		UVs uvs = this.uvs[channel];
		if (uvs != null && uvs.uvs.Count == num)
		{
			return;
		}
		this.uvs[channel] = new UVs(num);
		this.uvs[channel].uvs.AddRange(Enumerable.Repeat<Vector4>(Vector4.zero, num));
	}

	// Token: 0x06004B0F RID: 19215 RVA: 0x00162188 File Offset: 0x00160388
	private void Awake()
	{
		if (this.isSerialized)
		{
			this.RebuildMesh();
		}
	}

	// Token: 0x06004B10 RID: 19216 RVA: 0x00162198 File Offset: 0x00160398
	private void Start()
	{
		bool flag = this.isSerialized;
	}

	// Token: 0x06004B11 RID: 19217 RVA: 0x001621A1 File Offset: 0x001603A1
	private void EnsureIsCorrectChannelValue(int channel)
	{
		if (channel < 0 || channel > 7)
		{
			throw new ArgumentOutOfRangeException(string.Format("Invalid value for the channel: {0}", channel));
		}
	}

	// Token: 0x04003C8C RID: 15500
	private const int UVS_CHANNELS_COUNT = 8;

	// Token: 0x04003C8D RID: 15501
	[SerializeField]
	private UVs[] uvs;

	// Token: 0x04003C8E RID: 15502
	[SerializeField]
	private Vector3[] vertices;

	// Token: 0x04003C8F RID: 15503
	[SerializeField]
	private int[] triangles;

	// Token: 0x04003C90 RID: 15504
	[SerializeField]
	private bool isSerialized;

	// Token: 0x04003C91 RID: 15505
	[SerializeField]
	private MeshFilter meshFilter;

	// Token: 0x04003C92 RID: 15506
	public static bool debug;
}

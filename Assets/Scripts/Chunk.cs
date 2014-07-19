using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimplexNoise;

[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshCollider))]
[RequireComponent (typeof(MeshFilter))]
public class Chunk : MonoBehaviour {
	
	public byte[,,] map;
	public Mesh visualMesh;
	protected MeshRenderer meshRenderer;
	protected MeshCollider meshCollider;
	protected MeshFilter meshFilter;

	// Use this for initialization
	void Start () {
		
		meshRenderer = GetComponent<MeshRenderer>();
		meshCollider = GetComponent<MeshCollider>();
		meshFilter = GetComponent<MeshFilter>();
		
		
	
		map = new byte[World.currentWorld.chunkWidth, World.currentWorld.chunkHeight, World.currentWorld.chunkWidth];
		
		for (int x = 0; x < World.currentWorld.chunkWidth; x++)
		{
			float noiseX = (float)x / 20;
					
			for (int y = 0; y < World.currentWorld.chunkHeight; y++)
			{
				float noiseY = (float) y / 20;
			
				for (int z = 0; z < World.currentWorld.chunkWidth; z++)
				{
					float noiseZ = (float) z / 20;
					
					float noiseValue = Noise.Generate(noiseX, noiseY, noiseZ);
					
					noiseValue += (10f - (float)y) / 10;
					
					
					if (noiseValue > 0.2f)
						map[x, y, z] = 1;
				
				}
			}
		}
		
		CreateVisualMesh();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public virtual void CreateVisualMesh() {
		visualMesh = new Mesh();
		
		List<Vector3> verts = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> tris = new List<int>();
		
		
		for (int x = 0; x < World.currentWorld.chunkWidth; x++)
		{
			for (int y = 0; y < World.currentWorld.chunkHeight; y++)
			{
				for (int z = 0; z < World.currentWorld.chunkWidth; z++)
				{
					if (map[x,y,z] == 0) continue;
					
					byte brick = map[x,y,z];
					// Left wall
					if (IsTransparent(x - 1, y, z))
						BuildFace (brick, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);
					// Right wall
					if (IsTransparent(x + 1, y , z))
						BuildFace (brick, new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true, verts, uvs, tris);
					
					// Bottom wall
					if (IsTransparent(x, y - 1 , z))
						BuildFace (brick, new Vector3(x, y, z), Vector3.forward, Vector3.right, false, verts, uvs, tris);
					// Top wall
					if (IsTransparent(x, y + 1, z))
						BuildFace (brick, new Vector3(x, y + 1, z), Vector3.forward, Vector3.right, true, verts, uvs, tris);
					
					// Back
					if (IsTransparent(x, y, z - 1))
						BuildFace (brick, new Vector3(x, y, z), Vector3.up, Vector3.right, true, verts, uvs, tris);
					// Front
					if (IsTransparent(x, y, z + 1))
						BuildFace (brick, new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false, verts, uvs, tris);
					
					
				}
			}
		}
					
		visualMesh.vertices = verts.ToArray();
		visualMesh.uv = uvs.ToArray();
		visualMesh.triangles = tris.ToArray();
		visualMesh.RecalculateBounds();
		visualMesh.RecalculateNormals();
		
		meshFilter.mesh = visualMesh;
		meshCollider.sharedMesh = visualMesh;
		
	}
	public virtual void BuildFace(byte brick, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
	{
		int index = verts.Count;
		
		verts.Add (corner);
		verts.Add (corner + up);
		verts.Add (corner + up + right);
		verts.Add (corner + right);
		
		uvs.Add(new Vector2(0,0));
		uvs.Add(new Vector2(0,1));
		uvs.Add(new Vector2(1,1));
		uvs.Add(new Vector2(1,0));
		
		if (reversed)
		{
			tris.Add(index + 0);
			tris.Add(index + 1);
			tris.Add(index + 2);
			tris.Add(index + 2);
			tris.Add(index + 3);
			tris.Add(index + 0);
		}
		else
		{
			tris.Add(index + 1);
			tris.Add(index + 0);
			tris.Add(index + 2);
			tris.Add(index + 3);
			tris.Add(index + 2);
			tris.Add(index + 0);
		}
		
	}
	public virtual bool IsTransparent (int x, int y, int z)
	{
		byte brick = GetByte(x,y,z);
		switch (brick)
		{
		default:
		case 0: 
			return true;
			
		case 1: return false;
		}
	}
	public virtual byte GetByte (int x, int y , int z)
	{
		if ( (x < 0) || (y < 0) || (z < 0) || (y >= World.currentWorld.chunkHeight) || (x >= World.currentWorld.chunkWidth) || (z >= World.currentWorld.chunkWidth))
			return 0;
		return map[x,y,z];
	}
	
}



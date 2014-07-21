using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {
	
	public Texture2D defaultBrickTexture;
	
	public Biome[] biomes;
	
	public static World currentWorld;
	public int chunkWidth = 20, chunkHeight = 20, seed = 0;
	public float viewRange = 30;
	
	public float brickHeight = 1;
	
	public Chunk chunkFab;
	
	// Use this for initialization
	void Awake () {
		currentWorld = this;
		if (seed == 0)
			seed = Random.Range(0, int.MaxValue);
	}
	
	// Update is called once per frame
	void Update () {
		
		for (int a = 0; a < Chunk.chunks.Count; a++)
		{
			Vector3 pos = Chunk.chunks[a].transform.position;
			Vector3 delta = pos - transform.position;
			delta.y = 0;
			if (delta.magnitude < viewRange + chunkWidth * 3) continue;
			Destroy (Chunk.chunks[a].gameObject);
		}
		
		
		for (float x = transform.position.x - viewRange; x < transform.position.x + viewRange; x+= chunkWidth)
		{
			for (float z = transform.position.z - viewRange; z < transform.position.z + viewRange; z+= chunkWidth)
			{
				
				Vector3 pos = new Vector3(x, 0, z);
				
				
				pos.x = Mathf.Floor(pos.x / (float)chunkWidth) * chunkWidth;
				pos.z = Mathf.Floor(pos.z / (float)chunkWidth) * chunkWidth;
				
				// Shave square.
				Vector3 delta = pos - transform.position;
				delta.y = 0;
				if (delta.magnitude > viewRange) continue;
				
				
				Chunk chunk = Chunk.FindChunk(pos);
				if (chunk != null)
				{
					continue;
				}
				
				chunk = (Chunk)Instantiate(chunkFab, pos, Quaternion.identity);
				
				
				
			}
		}
		
	}
	
	public static Biome GetIdealBiome(float moisture, float rockiness)
	{
		float bestBid = 0;
		Biome biome = currentWorld.biomes[0];
		for (int a = 0; a < currentWorld.biomes.Length; a++)
		{
			float bid = currentWorld.biomes[a].Bid(moisture, rockiness);
			if (bid > bestBid)
			{
				bestBid = bid;
				biome = currentWorld.biomes[a];
			}
		}
		return biome;
	}
	
	
}



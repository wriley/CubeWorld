using UnityEngine;
using System.Collections;

[System.Serializable]
public class Biome {
	public string name = "Unknown Biome";
	[Multiline]
	public string desc = "No desc";
	
	public float idealMoisture = 0, idealRockiness = 0;
	
	
	public BrickLayer[] brickLayers;
	
	public byte GetBrick(int y, float mountainValue, float blobValue, float moisture, float rockiness) {
		BrickLayer bestBidder = null;
		float bestBid = 0;
		for (int a = 0; a < brickLayers.Length; a++)
		{
			float bid = brickLayers[a].Bid(y, mountainValue, blobValue, moisture, rockiness);
			if (bid > bestBid)
			{
				bestBid = bid;
				bestBidder = brickLayers[a];
			}
		}
		
		if (bestBidder == null)
			return 0;
		else
			return (byte)bestBidder.brick;
	}
	public float Bid (float moisture, float rockiness) {
		float delta = Mathf.Abs(moisture - idealMoisture) + Mathf.Abs(rockiness - idealRockiness);
		return 100 / (delta + 1);
	}
}

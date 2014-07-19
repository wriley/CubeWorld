using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {
	
	public static World currentWorld;
	
	public int chunkWidth = 20, chunkHeight = 20, seed = 0;

	// Use this for initialization
	void Awake () {
		currentWorld = this;
		if (seed == 0)
			seed = Random.Range(0, int.MaxValue);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

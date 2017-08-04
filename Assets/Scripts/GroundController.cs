using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour {
	public GameObject player;

	private Terrain terrain;
	private TerrainData terrainData;
	private Vector3 terrainPosition;
	private int alphamapWidth;
	private int alphamapHeight;
	private float[,,] splatMapData;
	private int numTextures;

	void Start() {
		terrain = Terrain.activeTerrain;
		terrainData = terrain.terrainData;
		terrainPosition = terrain.GetPosition ();
		alphamapWidth = terrainData.alphamapWidth;
		alphamapHeight = terrainData.alphamapHeight;

		splatMapData = terrainData.GetAlphamaps (0, 0, alphamapWidth, alphamapHeight);
		numTextures = splatMapData.Length / (alphamapWidth * alphamapHeight);
	}

	private Vector3 ConvertToSplatMapCoordinate(Vector3 playerPos) {
		Vector3 vecRet = new Vector3();
		vecRet.x = ((playerPos.x - terrainPosition.x) / terrainData.size.x) * terrainData.alphamapWidth;
		vecRet.z = ((playerPos.z - terrainPosition.z) / terrainData.size.z) * terrainData.alphamapHeight;
		return vecRet;
	}

	void Update() {	
		int terrainIndex = GetActiveTerrainTextureIndex ();
		print (terrainIndex);
	}

	private int GetActiveTerrainTextureIndex() {
		Vector3 coords = ConvertToSplatMapCoordinate (player.GetComponent<Rigidbody>().transform.position);
		int returnValue = 0;
		float comp = 0f;
		for (int i = 0; i < numTextures; i++) {
			if (comp < splatMapData [(int)coords.z, (int)coords.x, i])
				returnValue = i;
		}
		return returnValue;
	}
}

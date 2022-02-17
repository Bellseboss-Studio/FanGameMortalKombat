using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Try this every step instead of update. 
public class TerrainChecker : MonoBehaviour
{
    public int surfaceIndex = 0;
    public int surfaceChecker;
    private Terrain terrain;
    private TerrainData terrainData;
    private Vector3 terrainPos;

    void Start()
    {
        terrain = Terrain.activeTerrain;
        terrainData = terrain.terrainData;
        terrainPos = terrain.transform.position;
    }

    void Update()
    {
        
    }

    public string CheckTerrainNow()
    {
        surfaceIndex = GetMainTexture(transform.position);
        Debug.Log(DebugTerrainToConsole());
        return (terrainData.terrainLayers[surfaceIndex].name.ToString());
    }

    public string DebugTerrainToConsole()
    {
        
        return terrainData.terrainLayers[surfaceIndex].name.ToString();

    }

    private float[] GetTextureIndx(Vector3 WorldPos)
    {
        
        int mapX = (int)(((WorldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = (int)(((WorldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);

        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        float[] cellIndx = new float[splatmapData.GetUpperBound(2) + 1];

        for (int n = 0; n < cellIndx.Length; n++)
        {
            cellIndx[n] = splatmapData[0, 0, n];
        }
        return cellIndx;
    }

    private int GetMainTexture(Vector3 WorldPos)
    {
        float[] mIndx = GetTextureIndx(WorldPos);

        float maxIndx = 0;
        int maxIndex = 0;

        for (int n = 0; n < mIndx.Length; n++)
        {
            if (mIndx[n] > maxIndx)
            {
                maxIndex = n;
                maxIndx = mIndx[n];
            }
        }
        return maxIndex;
    }
}

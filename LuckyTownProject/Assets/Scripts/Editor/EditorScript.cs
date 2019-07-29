using System.Collections.Generic;
using UnityEngine;

public class EditorScript : MonoBehaviour
{
    public List<GameObject> prefabs;

    public Transform startPos;

    public float xSize;
    public float zSize;

    private void Start()
    {
        if (xSize > zSize)
            Camera.main.orthographicSize = xSize + 4;
        else
            Camera.main.orthographicSize = zSize + 4;

        Vector3 offset = prefabs[0].GetComponent<MeshRenderer>().bounds.size;
        CreateField(offset.x, offset.y);
    }

    private void CreateField(float xOffset, float zOffset)
    {
        var startX = startPos.position.x;
        var startZ = startPos.position.z;

        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                if (x % 2 == 0)
                {
                    if (z % 2 == 0)
                    {
                        Instantiate(prefabs[0], new Vector3(startX + (xOffset * x), 0, startZ + (zOffset * z)), prefabs[0].transform.rotation, startPos);
                    }
                    else
                    {
                        Instantiate(prefabs[1], new Vector3(startX + (xOffset * x), 0, startZ + (zOffset * z)), prefabs[1].transform.rotation, startPos);
                    }
                }
                else
                {
                    if (z % 2 == 0)
                    {
                        Instantiate(prefabs[1], new Vector3(startX + (xOffset * x), 0, startZ + (zOffset * z)), prefabs[1].transform.rotation, startPos);
                    }
                    else
                    {
                        Instantiate(prefabs[0], new Vector3(startX + (xOffset * x), 0, startZ + (zOffset * z)), prefabs[0].transform.rotation, startPos);
                    }
                }                
            }
        }
    }
}

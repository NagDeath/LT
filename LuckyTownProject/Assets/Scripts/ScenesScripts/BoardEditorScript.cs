using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class BoardEditorScript : MonoBehaviour
{
#if UNITY_EDITOR
    public static Action<GameObject> createTownAssetDelegate;
    public static Action<int, int, string, LevelSettingsSO> createDelegate;

    public static Action<LevelSettingsSO> adjustDelegate;

    public List<GameObject> prefabs;

    public Transform startPos;

    private void OnEnable()
    {
        createTownAssetDelegate += InstantiateTownAsset;
        adjustDelegate += Adjust;
        createDelegate += CreateField;
    }

    private void OnDisable()
    {
        createTownAssetDelegate -= InstantiateTownAsset;
        adjustDelegate -= Adjust;
        createDelegate -= CreateField;
    }
    private void CreateField(int xSize, int zSize, string prefabName, LevelSettingsSO levelSettings)
    {
        var oldGo = GameObject.FindGameObjectWithTag("ParentTerrainBlocks");
        var objects = GameObject.FindGameObjectsWithTag("Obstacle").ToList();

        if (oldGo != null)
        {
            PrefabUtility.UnpackPrefabInstance(oldGo, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

            foreach (var item in objects)
                DestroyImmediate(item);

            DestroyImmediate(oldGo);
        }

        if (xSize > 4 && xSize < 7)
        {
            Camera.main.orthographicSize = xSize * 0.8f;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 17.2f, Camera.main.transform.position.z);
        }
        else if (xSize >= 7)
        {
            Camera.main.orthographicSize = xSize * 0.8f;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 18f, Camera.main.transform.position.z);
        }
        else
        {
            Camera.main.orthographicSize = xSize;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 16.5f, Camera.main.transform.position.z);
        }

        Vector3 offset = prefabs[0].GetComponentInChildren<MeshRenderer>().bounds.size;
        var startX = startPos.position.x;
        var startZ = startPos.position.z - ((xSize -2) * 0.07f);

        GameObject parent = new GameObject
        {
            tag = "ParentTerrainBlocks",
            name = prefabName
        };

        var settings = parent.AddComponent<PrefabSettingsScript>();
        settings.LevelSettings = levelSettings;
        settings.LevelSettings.XSize = xSize;
        settings.LevelSettings.ZSize = zSize;

        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                GameObject tempObject;

                if (x % 2 == 0)
                {
                    if (z % 2 == 0)
                    {
                        tempObject = PrefabUtility.InstantiatePrefab(prefabs[0], startPos) as GameObject;
                        tempObject.transform.position = new Vector3(startX + (offset.x * x), 0, startZ + (offset.y * z)); 
                    }
                    else
                    {
                        tempObject = PrefabUtility.InstantiatePrefab(prefabs[1], startPos) as GameObject;
                        tempObject.transform.position = new Vector3(startX + (offset.x * x), 0, startZ + (offset.y * z));

                    }
                }
                else
                {
                    if (z % 2 == 0)
                    {
                        tempObject = PrefabUtility.InstantiatePrefab(prefabs[1], startPos) as GameObject;
                        tempObject.transform.position = new Vector3(startX + (offset.x * x), 0, startZ + (offset.y * z));
                    }
                    else
                    {
                        tempObject = PrefabUtility.InstantiatePrefab(prefabs[0], startPos) as GameObject;
                        tempObject.transform.position = new Vector3(startX + (offset.x * x), 0, startZ + (offset.y * z));
                    }
                }

                tempObject.transform.parent = parent.transform;
                tempObject.name = string.Format("{0},{1}", x, z);
            }
        }

        EditorUtility.SetDirty(levelSettings);

        PrefabUtility.SaveAsPrefabAssetAndConnect(parent, "Assets/Prefabs/Levels/" + prefabName + ".prefab", InteractionMode.AutomatedAction);
    }

    private void InstantiateTownAsset(GameObject gO)
    {
        var parent = GameObject.FindGameObjectWithTag("ParentTerrainBlocks");
        var townAsset = PrefabUtility.InstantiatePrefab(gO, parent.transform) as GameObject;
        townAsset.name = gO.name;
        townAsset.transform.position = new Vector3(parent.transform.GetChild(0).position.x, gO.transform.position.y, parent.transform.GetChild(0).position.z);
        townAsset.transform.parent = parent.transform;
    }

    private void Adjust(LevelSettingsSO levelSettings)
    {
        var parent = GameObject.FindGameObjectWithTag("ParentTerrainBlocks");
        if (parent != null)
        {
            var objects = GameObject.FindGameObjectsWithTag("Obstacle").ToList();
            var allObjects = objects.Union(GameObject.FindGameObjectsWithTag("Road").ToList());

            PrefabUtility.UnpackPrefabInstance(parent, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);

            var prefabSettings = parent.GetComponent<PrefabSettingsScript>();
            var matrix = prefabSettings.CreateMatrix();
            prefabSettings.LevelSettings = levelSettings;

            allObjects.ToList().ForEach(o =>
            {
                float tempDist = float.MaxValue;
                GameObject tempGo = null;
                o.transform.position = new Vector3(o.transform.position.x, o.transform.position.y, o.transform.position.z);
                foreach (var item in matrix)
                {
                    float dist = Vector3.Distance(o.transform.position, item.transform.position);
                    if (dist < tempDist)
                    {
                        tempDist = dist;
                        tempGo = item;
                    }
                }

                o.transform.position = new Vector3(tempGo.transform.position.x, o.transform.position.y, tempGo.transform.position.z);
                o.transform.parent = tempGo.transform.GetChild(0);
            });

            EditorUtility.SetDirty(levelSettings);

            PrefabUtility.SaveAsPrefabAssetAndConnect(parent, "Assets/Prefabs/Levels/" + parent.name + ".prefab", InteractionMode.AutomatedAction);
        }
    }
#endif
}
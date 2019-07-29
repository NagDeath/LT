using UnityEngine;
using UnityEditor;

public class LevelDesignEditor : EditorWindow
{
    private string prefabName = "Prefab Name";

    private int xSize = 0;
    private int zSize = 0;

    private int lightning = 0;
    private int meteor = 0;
    private int tornado = 0;
    private int earthquake = 0;

    private LevelMode levelMode = LevelMode.normal;

    private Vector2 townAssetsScrollPosition;
    private Vector2 levelsScrollPosition;
   
    [MenuItem("Window/LevelEditor")]
    public static void ShowWindow()
    {
        GetWindow<LevelDesignEditor>("LevelDesign");
    }

    private void OnGUI()
    {
        //Terrain creation
        GUILayout.Label("Terrain settings:", GUILayout.ExpandWidth(false));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Level name", GUILayout.ExpandWidth(false));
        prefabName = EditorGUILayout.TextField(prefabName, GUILayout.ExpandWidth(false));
        GUILayout.Label("xSize", GUILayout.ExpandWidth(false));
        xSize = EditorGUILayout.IntField(xSize, GUILayout.ExpandWidth(false));
        GUILayout.Label("zSize", GUILayout.ExpandWidth(false));
        zSize = EditorGUILayout.IntField(zSize, GUILayout.ExpandWidth(false), GUILayout.Height(20f));
        
        GUILayout.EndHorizontal();
        GUILayout.Label("Cataclysms settings:", GUILayout.ExpandWidth(false));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Lightning", GUILayout.ExpandWidth(false));
        lightning = EditorGUILayout.IntField(lightning, GUILayout.ExpandWidth(false));
        GUILayout.Label("Meteor", GUILayout.ExpandWidth(false));
        meteor = EditorGUILayout.IntField(meteor, GUILayout.ExpandWidth(false));
        GUILayout.Label("Tornado", GUILayout.ExpandWidth(false));
        tornado = EditorGUILayout.IntField(tornado, GUILayout.ExpandWidth(false));
        GUILayout.Label("Earthquake", GUILayout.ExpandWidth(false));
        earthquake = EditorGUILayout.IntField(earthquake, GUILayout.ExpandWidth(false), GUILayout.Height(20f));
        GUILayout.EndHorizontal();

        GUILayout.Label("Mode settings:", GUILayout.ExpandWidth(false));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Level Mode", GUILayout.ExpandWidth(false));
        levelMode = (LevelMode)EditorGUILayout.EnumPopup(levelMode, GUILayout.ExpandWidth(false), GUILayout.Width(100f), GUILayout.Height(20f));
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Create", GUILayout.Height(30f)))
        {
            BoardEditorScript.createDelegate?.Invoke(xSize, zSize, prefabName, AssetSettings());
        }

        //TownAssets creation
        GUILayout.Label("TownAssets:", GUILayout.ExpandWidth(false));
        townAssetsScrollPosition = GUILayout.BeginScrollView(townAssetsScrollPosition, GUILayout.ExpandHeight(false));
        GUILayout.BeginHorizontal();
        var prefabs = PrefabLoaderScript.LoadAllPrefabsAt("Assets/Prefabs/TownAssets");
        foreach (var obj in prefabs)
        {
            if (GUILayout.Button(AssetPreview.GetAssetPreview(obj), GUILayout.ExpandWidth(false)))
                BoardEditorScript.createTownAssetDelegate?.Invoke(obj);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();

        //Saved levels creation
        GUILayout.Label("Levels:", GUILayout.ExpandWidth(false));
        levelsScrollPosition = GUILayout.BeginScrollView(levelsScrollPosition, GUILayout.ExpandHeight(false));
        GUILayout.BeginHorizontal();
        var levels = PrefabLoaderScript.LoadAllPrefabsAt("Assets/Prefabs/Levels");
        foreach (var obj in levels)
        {
            if (GUILayout.Button(new GUIContent(obj.name, AssetPreview.GetAssetPreview(obj)), GUILayout.ExpandWidth(false)))
            {
                var oldGo = GameObject.FindGameObjectWithTag("ParentTerrainBlocks");

                if (oldGo != null)
                    DestroyImmediate(oldGo);

                if (obj.GetComponent<PrefabSettingsScript>().LevelSettings.XSize > 4 && obj.GetComponent<PrefabSettingsScript>().LevelSettings.XSize < 7)
                {
                    Camera.main.orthographicSize = obj.GetComponent<PrefabSettingsScript>().LevelSettings.XSize * 0.8f;
                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 17.2f, Camera.main.transform.position.z);
                }
                else if (obj.GetComponent<PrefabSettingsScript>().LevelSettings.XSize >= 7)
                {
                    Camera.main.orthographicSize = obj.GetComponent<PrefabSettingsScript>().LevelSettings.XSize * 0.8f;
                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 18f, Camera.main.transform.position.z);
                }
                else
                {
                    Camera.main.orthographicSize = obj.GetComponent<PrefabSettingsScript>().LevelSettings.XSize;
                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 16.5f, Camera.main.transform.position.z);
                }

                PrefabUtility.InstantiatePrefab(obj as GameObject);
            }

        }
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();

        if (GUILayout.Button("Adjust", GUILayout.Height(30f)))
        {
            BoardEditorScript.adjustDelegate?.Invoke(AssetSettings());
        }       
    }

    private LevelSettingsSO AssetSettings()
    {
        string assetPathAndName = "Assets/ScriptableObjects/LevelSettings/" + prefabName + ".asset";
        var asset = AssetDatabase.LoadAssetAtPath(assetPathAndName, typeof(LevelSettingsSO)) as LevelSettingsSO;
        if (asset == null)
        {
            asset = CreateInstance<LevelSettingsSO>();
            AssetDatabase.CreateAsset(asset, assetPathAndName);
        }

        asset.Lightning = lightning;
        asset.Meteor = meteor;
        asset.Tornado = tornado;
        asset.Earthquake = earthquake;
        asset.LevelMode = levelMode;

        return asset;
    }
}

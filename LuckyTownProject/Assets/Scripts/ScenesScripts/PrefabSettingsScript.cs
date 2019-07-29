using UnityEngine;

public class PrefabSettingsScript : MonoBehaviour
{
    [SerializeField]
    private LevelSettingsSO levelSettings;

    [SerializeField]
    private GameObject[,] matrix;

    public LevelSettingsSO LevelSettings { get => levelSettings; set => levelSettings = value; }
    public GameObject[,] Matrix { get => matrix; set => matrix = value; }

    private void OnEnable()
    {
        CreateMatrix();
    }

    public GameObject[,] CreateMatrix()
    {
        matrix = new GameObject[levelSettings.XSize, levelSettings.ZSize];

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.name.Contains(","))
            {
                string[] xz = child.name.Split(',');
                matrix[int.Parse(xz[0]), int.Parse(xz[1])] = child.gameObject;
            }
        }

        return matrix;
    }    
}
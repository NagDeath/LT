using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CataclysmSettings", menuName = "CataclysmSettings", order = 51)]
public class CataclysmSettingsSO : ScriptableObject
{
    [SerializeField]
    private List<CataclysmModel> wavesSettings;
    public List<CataclysmModel> WavesSettings { get => wavesSettings; set => wavesSettings = value; }
}

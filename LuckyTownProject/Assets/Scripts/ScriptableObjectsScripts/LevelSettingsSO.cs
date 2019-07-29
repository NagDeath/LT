using UnityEngine;

public enum LevelMode
{
    normal = 0,
    hard = 1,
    veryHard = 2
}

[CreateAssetMenu(fileName = "LevelSettings", menuName = "LevelSettings", order = 51)]
public class LevelSettingsSO : ScriptableObject
{
    [SerializeField]
    private int lightning;
    [SerializeField]
    private int meteor;
    [SerializeField]
    private int tornado;
    [SerializeField]
    private int earthquake;
    [SerializeField]
    private LevelMode levelMode;
    [SerializeField]
    private int xSize;
    [SerializeField]
    private int zSize;

    public int XSize { get => xSize; set => xSize = value; }
    public int ZSize { get => zSize; set => zSize = value; }
    public int Lightning { get => lightning; set => lightning = value; }
    public int Meteor { get => meteor; set => meteor = value; }
    public int Tornado { get => tornado; set => tornado = value; }
    public int Earthquake { get => earthquake; set => earthquake = value; }
    public LevelMode LevelMode { get => levelMode; set => levelMode = value; }
}

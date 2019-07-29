using System;
public enum Cataclysm
{
    None = 0,
    Lightning = 1,
    Meteor = 2,
    Tornado = 3,
    Earthquake = 4
}

public enum EarthquakeState
{
    Vertical = 0,
    Horizontal = 1
}

public class GlobalContainer
{
    public event Action calculatingDelegate;
    public event Action startStateDelegate;
    public event Action endStateDelegate;

    private Cataclysm cataclysm;

    private EarthquakeState earthquakeState;

    private bool isGameStarted;
    private bool isAnimEnded = true;

    private LevelSettingsSO levelSettings;
    public LevelSettingsSO LevelSettings { get => levelSettings; set => levelSettings = value; }
    public EarthquakeState EarthquakeState { get => earthquakeState; set => earthquakeState = value; }
    public Cataclysm Cataclysm { get => cataclysm; set => cataclysm = value; }
    public bool IsGameStarted { get => isGameStarted; }
    public bool IsAnimEnded { get => isAnimEnded; set => isAnimEnded = value; }

    private static GlobalContainer instance;

    GlobalContainer(){ }
    public static GlobalContainer GetInstance()
    {
        if (instance == null)
            return instance = new GlobalContainer();

        return instance;
    }

    public void StartState()
    {
        isGameStarted = true;
        startStateDelegate?.Invoke();
    }

    public void EndState()
    {
        isGameStarted = false;
        endStateDelegate?.Invoke();
    }

    public void CalculatingLightning()
    {
        levelSettings.Lightning--;
        calculatingDelegate?.Invoke();
    }

    public void CalculatingMeteor()
    {
        levelSettings.Meteor--;
        calculatingDelegate?.Invoke();
    }

    public void CalculatingTornado()
    {
        levelSettings.Tornado--;
        calculatingDelegate?.Invoke();
    }

    public void CalculatingEarthquake()
    {
        levelSettings.Earthquake--;
        calculatingDelegate?.Invoke();
    }
}

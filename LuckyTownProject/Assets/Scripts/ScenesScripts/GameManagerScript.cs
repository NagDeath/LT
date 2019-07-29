using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static Action<float> animEndDelegate;

    [SerializeField]
    private List<GameObject> cards;

    [SerializeField]
    private Animation lightningAnim;

    [SerializeField]
    private GameObject meteorGO;

    [SerializeField]
    private GameObject[] tornadosGO;

    [SerializeField]
    private TextMeshProUGUI lightningText;
    [SerializeField]
    private TextMeshProUGUI meteorText;
    [SerializeField]
    private TextMeshProUGUI tornadoText;
    [SerializeField]
    private TextMeshProUGUI earthquakeText;

    [SerializeField]
    private TextMeshProUGUI level;
    [SerializeField]
    private DoTweenController cardPanel;
    [SerializeField]
    private DoTweenController winScreen;
    [SerializeField]
    private DoTweenController nextBTN;
    [SerializeField]
    private DoTweenController loseScreen;
    [SerializeField]
    private DoTweenController hardMode;
    [SerializeField]
    private DoTweenController veryHardMode;
    [SerializeField]
    private DoTweenController lightningTutor;
    [SerializeField]
    private DoTweenController meteorTutor;
    [SerializeField]
    private DoTweenController tornadoTutor;
    [SerializeField]
    private DoTweenController earthQuakeTutor;
    [SerializeField]
    private DoTweenController skipLevelMenu;
    [SerializeField]
    private DoTweenController returnBTN; 

    [SerializeField]
    private RectTransform lightningTutorTarget;
    [SerializeField]
    private RectTransform meteorTutorTarget;
    [SerializeField]
    private RectTransform tornadoTutorTarget;
    [SerializeField]
    private RectTransform firstEarthQuakeTutorTarget;
    [SerializeField]
    private RectTransform secondEarthQuakeTutorTarget;

    [SerializeField]
    private ParticleSystem winConfetti;

    private List<List<GameObject>> waves;

    private GameObject[,] matrix;

    private string prefix;

    private bool returnToStandartColor;
    private bool getTutorialPos = true;
    private bool isSkipBTNShown;

    private Vector3 startPos;

    private GameObject curLevel;
    private GameObject curGO;
    private GameObject prevGO;

    private static AssetBundle loadedAssetBundle;

    private int x;
    private int z;
    private int levelName;
    private int countingForAd;

    private float afkTimer;

    public List<CataclysmSettingsSO> cataclysmsSettings;

    public float delay = 0.1f; //задержка анимации кубов

    private void OnEnable()
    {
        DragDropScript.onDragDelegate += OnDragDelegate;
        animEndDelegate += StartWaves;
        GlobalContainer.GetInstance().calculatingDelegate += CatalysmsCounting;
    }

    private void OnDisable()
    {
        DragDropScript.onDragDelegate -= OnDragDelegate;
        GlobalContainer.GetInstance().calculatingDelegate -= CatalysmsCounting;
        animEndDelegate -= StartWaves;
    }

    private void Start()
    {
#if UNITY_EDITOR
        curLevel = GameObject.FindGameObjectWithTag("ParentTerrainBlocks");
        if (curLevel != null)
        {
            GlobalContainer.GetInstance().LevelSettings = curLevel.GetComponent<PrefabSettingsScript>().LevelSettings.Copy();
            CatalysmsCounting();
            GameStartBTN();
            levelName = 1;
            //Tutorial(true);
        }
#elif UNITY_IOS
        prefix = "IOS";
#elif UNITY_ANDROID
        prefix = "Android";

        LoadLevel();
#endif
    }

    private void OnDragDelegate(bool state)
    {
        //isSkipBTNShown = false;
        afkTimer = 0;

        Tutorial(state);
    }

    private void Tutorial(bool state)
    {
        switch (levelName)
        {
            case 1:
                if (state)
                {
                    if (getTutorialPos)
                    {
                        startPos = lightningTutor.GetComponent<RectTransform>().position;
                        getTutorialPos = false;
                    }

                    lightningTutor.FromLocation = startPos;
                    lightningTutor.ToLocation = lightningTutorTarget.position;

                    if (!lightningTutor.isActiveAndEnabled)
                    {
                        lightningTutor.gameObject.SetActive(true);
                        lightningTutor.DOTweenPlay();
                    }
                }
                else
                {
                    lightningTutor.gameObject.SetActive(false);
                    lightningTutor.transform.DOKill(false);

                }
                break;
            case 4:
                if (state)
                {
                    if (getTutorialPos)
                    {
                        startPos = meteorTutor.GetComponent<RectTransform>().position;
                        getTutorialPos = false;
                    }

                    meteorTutor.FromLocation = startPos;
                    meteorTutor.ToLocation = meteorTutorTarget.position;

                    if (!meteorTutor.isActiveAndEnabled)
                    {
                        meteorTutor.gameObject.SetActive(true);
                        meteorTutor.DOTweenPlay();
                    }
                }
                else
                {
                    meteorTutor.gameObject.SetActive(false);
                    meteorTutor.transform.DOKill(false);
                }
                break;
            case 7:
                if (state)
                {
                    if (getTutorialPos)
                    {
                        startPos = tornadoTutor.GetComponent<RectTransform>().position;
                        getTutorialPos = false;
                    }

                    tornadoTutor.FromLocation = startPos;
                    tornadoTutor.ToLocation = tornadoTutorTarget.position;

                    if (!tornadoTutor.isActiveAndEnabled)
                    {
                        tornadoTutor.gameObject.SetActive(true);
                        tornadoTutor.DOTweenPlay();
                    }
                }
                else
                {
                    tornadoTutor.gameObject.SetActive(false);
                    tornadoTutor.transform.DOKill(false);
                }
                break;
            case 8:
                if (state)
                {
                    if (getTutorialPos)
                    {
                        startPos = tornadoTutor.GetComponent<RectTransform>().position;
                        getTutorialPos = false;
                    }

                    earthQuakeTutor.FromLocation = startPos;

                    if (!earthQuakeTutor.isActiveAndEnabled)
                    {
                        if (GlobalContainer.GetInstance().LevelSettings.Earthquake == 2)
                            earthQuakeTutor.ToLocation = firstEarthQuakeTutorTarget.position;
                        else
                            earthQuakeTutor.ToLocation = secondEarthQuakeTutorTarget.position;

                        earthQuakeTutor.gameObject.SetActive(true);
                        earthQuakeTutor.DOTweenPlay();
                    }
                }
                else
                {
                    earthQuakeTutor.gameObject.SetActive(false);
                    earthQuakeTutor.transform.DOKill(false
                        );
                }
                break;
            default:
                break;
        }
    }

    private bool TutorialTarget()
    {
        switch (levelName)
        {
            case 1:
                if (curGO == matrix[1, 1])
                    return false;

                return true;
            case 4:
                if (curGO == matrix[2, 2])
                    return false;

                return true;
            case 7:
                if (curGO == matrix[2, 2])
                    return false;

                return true;
            case 8:
                if (GlobalContainer.GetInstance().LevelSettings.Earthquake == 2)
                {
                    if (curGO == matrix[0, 3])
                        return false;
                }
                else
                {
                    if (curGO == matrix[3, 0])
                        return false;
                }
                return true;
            default:
                break;
        }

        return false;
    }


    private void LoadLevel()
    {
        var oldGo = GameObject.FindGameObjectWithTag("ParentTerrainBlocks");
        if (oldGo != null)
            Destroy(oldGo);
        if (loadedAssetBundle != null)
            loadedAssetBundle.Unload(true);

        levelName = PlayerPrefs.GetInt("level", 9);

        curLevel = LoadingPrefabs(levelName);
        if (curLevel == null)
        {
            levelName--;
            curLevel = LoadingPrefabs(levelName);
        }

        level.text = string.Format("LEVEL {0}", levelName);

        GlobalContainer.GetInstance().LevelSettings = curLevel.GetComponent<PrefabSettingsScript>().LevelSettings;
        InitLevel();
    }

#region Launcher
    private GameObject LoadingPrefabs(int levelName)
    {
        loadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath + "/" + prefix + "/levels"));

        if (loadedAssetBundle == null)
        {
            Debug.Log("Failed to load!");
            return null;
        }

        return loadedAssetBundle.LoadAsset<GameObject>("l" + levelName);
    }
#endregion

    private void InitLevel()
    {
        if (GlobalContainer.GetInstance().LevelSettings.XSize > 4 && GlobalContainer.GetInstance().LevelSettings.XSize < 7)
        {
            Camera.main.orthographicSize = GlobalContainer.GetInstance().LevelSettings.XSize * 0.8f;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 17.2f, Camera.main.transform.position.z);
        }
        else if (GlobalContainer.GetInstance().LevelSettings.XSize >= 7)
        {
            Camera.main.orthographicSize = GlobalContainer.GetInstance().LevelSettings.XSize * 0.8f;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 18f, Camera.main.transform.position.z);
        }
        else
        {
            Camera.main.orthographicSize = GlobalContainer.GetInstance().LevelSettings.XSize;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 16.5f, Camera.main.transform.position.z);
        }

        CatalysmsCounting();
        Instantiate(curLevel);
    }

    private void CatalysmsCounting()
    {
        if (GlobalContainer.GetInstance().LevelSettings.Lightning == 0)
            cards.FirstOrDefault(c => c.name == "LightningCard").SetActive(false);
        if (GlobalContainer.GetInstance().LevelSettings.Meteor == 0)
            cards.FirstOrDefault(c => c.name == "MeteorCard").SetActive(false);
        if (GlobalContainer.GetInstance().LevelSettings.Tornado == 0)
            cards.FirstOrDefault(c => c.name == "TornadoCard").SetActive(false);
        if (GlobalContainer.GetInstance().LevelSettings.Earthquake == 0)
            cards.FirstOrDefault(c => c.name == "EarthquakeCard").SetActive(false);

        lightningText.text = string.Format("x{0}", GlobalContainer.GetInstance().LevelSettings.Lightning.ToString());
        meteorText.text = string.Format("x{0}", GlobalContainer.GetInstance().LevelSettings.Meteor.ToString());
        tornadoText.text = string.Format("x{0}", GlobalContainer.GetInstance().LevelSettings.Tornado.ToString());
        earthquakeText.text = string.Format("x{0}", GlobalContainer.GetInstance().LevelSettings.Earthquake.ToString());
    }

    private void Update()
    {
        if (GlobalContainer.GetInstance().IsGameStarted)
        {
            afkTimer += Time.deltaTime;
            if (afkTimer > 15 && !isSkipBTNShown)
            {
                isSkipBTNShown = true;
                SkipLevelMenuForward();
            }

            if (Input.GetMouseButton(0))
            {
                var layerMask = 1 << 8;
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask) && GlobalContainer.GetInstance().IsAnimEnded)
                {
                    returnToStandartColor = true;
                    HighlightArea(hit.collider.name, hit.collider.GetComponentInParent<PrefabSettingsScript>().Matrix);
                }
                else
                {
                    if (returnToStandartColor)
                    {
                        prevGO = null;
                        MaterialChangeScript.changeToStandartMatDelegate?.Invoke();
                        returnToStandartColor = false;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                var layerMask = 1 << 8;
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask) && GlobalContainer.GetInstance().IsAnimEnded)
                {
                    if (!TutorialTarget())
                    {
                        GlobalContainer.GetInstance().IsAnimEnded = false;
                        CataclysmInit();
                    }
                }
                else
                {
                    GlobalContainer.GetInstance().Cataclysm = Cataclysm.None;
                }

                if (GlobalContainer.GetInstance().IsAnimEnded)
                    Tutorial(true);

                MaterialChangeScript.changeToStandartMatDelegate?.Invoke();
            }
        }
    }

    private void SkipLevelMenuForward()
    {
        if (AppodealManagerScript.instance.CanShowRewardedVideo())
        {
            skipLevelMenu.FromLocation = new Vector3(1235f, -317.5f, 0f);
            skipLevelMenu.ToLocation = new Vector3(925f, -317.5f, 0f);
            skipLevelMenu.DOTweenPlay();
        }
        else
        {
            isSkipBTNShown = false;
            afkTimer = 0;
        }
    }

    private void SkipLevelMenuReverse()
    {
        skipLevelMenu.FromLocation = new Vector3(925f, -317.5f, 0f);
        skipLevelMenu.ToLocation = new Vector3(1235f, -317.5f, 0f);
        skipLevelMenu.DOTweenPlay();
    }

    public void SkipLevelBTN()
    {
        AppodealManagerScript.instance.ShowRewardedVideo(NextLevel);
    }

    private void NextLevel()
    {
        SkipLevelMenuReverse();
        PlayerPrefs.SetInt("level", levelName + 1);
        NextBTN();
    }

    private void HighlightArea(string name, GameObject[,] matrix)
    {
        GameObject obj = null;
        foreach (var item in matrix)
        {
            if (item.name == name)
            {
                obj = item;
                break;
            }
        }
        string[] xz = obj.name.Split(',');
        x = int.Parse(xz[0]);
        z = int.Parse(xz[1]);
        curGO = obj;

        var cataclysmScript = new CataclysmScript();

        this.matrix = matrix;

        if (GlobalContainer.GetInstance().Cataclysm == Cataclysm.Earthquake)
        {
            if (x == 0 && z != 0)
            {
                GlobalContainer.GetInstance().EarthquakeState = EarthquakeState.Horizontal;
                waves = cataclysmScript.CalculateAOE(x, z, matrix, cataclysmsSettings);

            }
            else if (z == 0 && x != 0)
            {
                GlobalContainer.GetInstance().EarthquakeState = EarthquakeState.Vertical;
                waves = cataclysmScript.CalculateAOE(x, z, matrix, cataclysmsSettings);
            }
            else
                waves = null;
        }
        else
            waves = cataclysmScript.CalculateAOE(x, z, matrix, cataclysmsSettings);

        if (curGO != prevGO && waves != null)
        {
            prevGO = curGO;
            MaterialChangeScript.changeToStandartMatDelegate?.Invoke();

            for (int i = 0; i < waves.Count; i++)
            {
                waves[i].ForEach(e =>
                {
                    e.GetComponent<MaterialChangeScript>().ChangeMaterial();
                });
            }
        }
    }

    private void CataclysmInit()
    {
        if (waves != null && waves.Count > 0)
        {
            switch (GlobalContainer.GetInstance().Cataclysm)
            {
                case Cataclysm.None:
                    break;
                case Cataclysm.Lightning:
                    GlobalContainer.GetInstance().CalculatingLightning();
                    break;
                case Cataclysm.Meteor:
                    GlobalContainer.GetInstance().CalculatingMeteor();
                    break;
                case Cataclysm.Tornado:
                    GlobalContainer.GetInstance().CalculatingTornado();
                    break;
                case Cataclysm.Earthquake:
                    GlobalContainer.GetInstance().CalculatingEarthquake();
                    break;
                default:
                    break;
            }

            AnimationState(x, z);
        }
        else
            GlobalContainer.GetInstance().IsAnimEnded = true;
    }

    private void AnimationState(int x, int z)
    {
        switch (GlobalContainer.GetInstance().Cataclysm)
        {
            case Cataclysm.Lightning:
                lightningAnim.transform.position = waves[0][0].transform.position;
                lightningAnim.Play();
                break;
            case Cataclysm.Meteor:
                meteorGO.GetComponent<MeteorScript>().Target = waves[0][0].transform;
                meteorGO.GetComponent<MeteorScript>().IsMoving = true;
                break;
            case Cataclysm.Tornado:
                for (int i = 0; i < tornadosGO.Length; i++)
                {
                    tornadosGO[i].transform.position = new Vector3(waves[0][0].transform.position.x, tornadosGO[i].transform.position.y, waves[0][0].transform.position.z);
                }

                if (x != 0)
                {
                    tornadosGO[0].SetActive(true);
                    tornadosGO[0].GetComponent<TornadoScript>().Target = new Vector3(matrix[0, z].transform.position.x - 0.5f, tornadosGO[0].transform.position.y, matrix[0, z].transform.position.z);
                }
                if (z != 0)
                {
                    tornadosGO[1].SetActive(true);
                    tornadosGO[1].GetComponent<TornadoScript>().Target = new Vector3(matrix[x, 0].transform.position.x, tornadosGO[0].transform.position.y, matrix[x, 0].transform.position.z - 0.5f);
                }
                if (z != matrix.GetLength(0) - 1)
                {
                    tornadosGO[2].SetActive(true);
                    tornadosGO[2].GetComponent<TornadoScript>().Target = new Vector3(matrix[x, matrix.GetLength(1) - 1].transform.position.x, tornadosGO[0].transform.position.y, matrix[x, matrix.GetLength(1) - 1].transform.position.z + 0.5f);
                }
                if (x != matrix.GetLength(1) - 1)
                {
                    tornadosGO[3].SetActive(true);
                    tornadosGO[3].GetComponent<TornadoScript>().Target = new Vector3(matrix[matrix.GetLength(0) - 1, z].transform.position.x + 0.5f, tornadosGO[0].transform.position.y, matrix[matrix.GetLength(0) - 1, z].transform.position.z);
                }

                animEndDelegate?.Invoke(1.2f);
                break;
            case Cataclysm.Earthquake:
                animEndDelegate?.Invoke(1.2f);
                break;
            default:
                GlobalContainer.GetInstance().IsAnimEnded = true;
                break;
        }

        GlobalContainer.GetInstance().Cataclysm = Cataclysm.None;
    }

    private void StartWaves(float animDelay)
    {
        StartCoroutine(StartWavesCor(animDelay));
    }

    private IEnumerator StartWavesCor(float animDelay)
    {
        RaycastHit hit;

        for (int i = 0; i < waves.Count; i++)
        {
            waves[i].ForEach(e =>
            {
                e.GetComponent<TerrainBlockScript>().Animation();
                if (Physics.Raycast(e.transform.position, Vector3.up, out hit))
                {
                    if (hit.collider.tag == "Obstacle")
                        StartCoroutine(hit.collider.GetComponent<ObstacleScript>().GetDamage(1, delay));
                }
            });

            yield return new WaitForSeconds(delay);
        }

        //yield return new WaitForSeconds(animDelay);

        GlobalContainer.GetInstance().IsAnimEnded = true;
        GameManager();
    }

    public void ReturnBTN()
    {
        if (GlobalContainer.GetInstance().IsAnimEnded)
        {
            GameOverBTN();

            GlobalContainer.GetInstance().EndState();
            Tutorial(false);
#if UNITY_EDITOR

#else
            LoadLevel();
#endif
            GameStartBTN();
        }
    }

    public void GameStartBTN()
    {
        Tutorial(true);

        afkTimer = 0;
        isSkipBTNShown = false;

        hardMode.DoTweenDelay = 0f;
        hardMode.ScaleMultiplier = 0f;
        hardMode.DOTweenPlay();
        veryHardMode.DoTweenDelay = 0f;
        veryHardMode.ScaleMultiplier = 0f;
        veryHardMode.DOTweenPlay();

        cardPanel.gameObject.SetActive(true);
        cardPanel.DoTweenDelay = 0.5f;
        cardPanel.FadeValue = DoTweenFade.From;
        cardPanel.DOTweenPlay();

        returnBTN.gameObject.SetActive(true);
        returnBTN.DoTweenDelay = 0.5f;
        returnBTN.FadeValue = DoTweenFade.From;
        returnBTN.DOTweenPlay();

        switch (GlobalContainer.GetInstance().LevelSettings.LevelMode)
        {
            case LevelMode.normal:
                break;
            case LevelMode.hard:
                hardMode.DoTweenDelay = 0.5f;
                hardMode.ScaleMultiplier = 1f;
                hardMode.DOTweenPlay();
                break;
            case LevelMode.veryHard:
                veryHardMode.DoTweenDelay = 0.5f;
                veryHardMode.ScaleMultiplier = 1f;
                veryHardMode.DOTweenPlay();
                break;
            default:
                break;
        }

        GlobalContainer.GetInstance().StartState();
    }

    public void GameOverBTN()
    {
        if (isSkipBTNShown)
            SkipLevelMenuReverse();

        returnBTN.gameObject.SetActive(false);
        returnBTN.DoTweenDelay = 0f;
        returnBTN.FadeValue = DoTweenFade.To;
        returnBTN.DOTweenPlay();

        cardPanel.gameObject.SetActive(false);
        cardPanel.DoTweenDelay = 0;
        cardPanel.FadeValue = DoTweenFade.To;
        cardPanel.DOTweenPlay();
        cards.ForEach(c => c.SetActive(true));
    }

    public void NextBTN()
    {
        if (levelName > 5 && levelName % 2 == 1)
            AppodealManagerScript.instance.ShowInterstitial();

        Tutorial(false);
        WinStateReverse();
#if UNITY_EDITOR

#else
        LoadLevel();
#endif
        GameStartBTN();
    }

    private void WinStateForward()
    {
        GlobalContainer.GetInstance().EndState();

        GameOverBTN();
        winConfetti.Play();

        winScreen.FromLocation = new Vector3(0f, 480f, 0f);
        winScreen.ToLocation = new Vector3(0f, -223f, 0f);
        winScreen.DOTweenPlay();
        nextBTN.GetComponent<CanvasGroup>().blocksRaycasts = true;
        nextBTN.FadeValue = DoTweenFade.From;
        nextBTN.DOTweenPlay();
    }

    private void WinStateReverse()
    {
        GameOverBTN();
        winScreen.FromLocation = new Vector3(0f, -223f, 0f);
        winScreen.ToLocation = new Vector3(0f, 480f, 0f);
        winScreen.DOTweenPlay();
        nextBTN.GetComponent<CanvasGroup>().blocksRaycasts = false;
        nextBTN.FadeValue = DoTweenFade.To;
        nextBTN.DOTweenPlay();
    }

    private void LoseStateForward()
    {
        if (countingForAd == 3)
        {
            countingForAd = 0;
            AppodealManagerScript.instance.ShowInterstitial();
        }
        else
            countingForAd++;

        GameOverBTN();
        loseScreen.FromLocation = new Vector3(0f, -480f, 0f);
        loseScreen.ToLocation = new Vector3(0f, 223f, 0f);
        loseScreen.DOTweenPlay();
        GlobalContainer.GetInstance().EndState();
    }

    private void LoseStateReverse()
    {
        loseScreen.FromLocation = new Vector3(0f, 223f, 0f);
        loseScreen.ToLocation = new Vector3(0f, -480f, 0f);
        loseScreen.DOTweenPlay();
    }

    public void RetryBTN()
    {
        LoseStateReverse();
#if UNITY_EDITOR

#else
        LoadLevel();
#endif
        GameStartBTN();
    }

    private void GameManager()
    {
        var obstacles = GameObject.FindGameObjectsWithTag("Obstacle").ToList();

        if (obstacles.All(o => o.GetComponent<ObstacleScript>().IsDestroyed))
        {
            print("Win");
            WinStateForward();
#if UNITY_EDITOR

#else
            //levelName = PlayerPrefs.GetInt("level", 1);
            PlayerPrefs.SetInt("level", levelName + 1);
#endif
            return;
        }

        if (GlobalContainer.GetInstance().LevelSettings.Lightning == 0 && GlobalContainer.GetInstance().LevelSettings.Meteor == 0 &&
            GlobalContainer.GetInstance().LevelSettings.Tornado == 0 && GlobalContainer.GetInstance().LevelSettings.Earthquake == 0)
        {
            print("Lose");
            LoseStateForward();
        }
        else
            Tutorial(true);
    }
}

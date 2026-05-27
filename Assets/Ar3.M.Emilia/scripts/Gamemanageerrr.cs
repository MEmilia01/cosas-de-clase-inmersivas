using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.XR.ARSubsystems;


public class GameManagerAR : MonoBehaviour
{
    public static GameManagerAR Instance;

    [Header("Parameters")]
    public GameParameters parameters;
    public GameParameters parametros;

    [Header("AR Components")]
    public ARPlaneManager planeManager;
    public AROcclusionManager occlusionManager;
    public GameObject gemPrefab;

    [Header("UI")]
    public TMPro.TextMeshProUGUI collectedText;
    public TMPro.TextMeshProUGUI remainingText;
    public TMPro.TextMeshProUGUI planesText;
    public TMPro.TextMeshProUGUI timeText;

    public GameObject endPanel;
    public CanvasGroup gameUICanvasGroup;
    public AudioClip gemSound;

    private float currentTime;
    private int gemsCollected = 0;
    private int gemsTarget = 0;
    public bool gameActive = false;
    private bool gameEnded = false;

    //private int detectedHorizontalPlanes = 0;
    //private int detectedVerticalPlanes = 0;

    //private bool horizontalRequirementMet = false;
    //private bool verticalRequirementMet = false;
    //private bool gameStarted = false;

    //private readonly HashSet<TrackableId> countedPlanes = new HashSet<TrackableId>();
    //private readonly List<GameObject> spawnedGems = new List<GameObject>();
 

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //void Start()
    //{
    //    parameters = FindAnyObjectByType<GameParameters>();
    //    planeManager = FindAnyObjectByType<ARPlaneManager>();
    //    occlusionManager = FindAnyObjectByType<AROcclusionManager>();

    //}
    IEnumerator Start()
{
    planeManager.enabled = false;

    yield return new WaitForSeconds(2f);

    if (parameters == null)
    {
        Debug.LogError("GameParameters no asignado.");
        yield break;
    }

    gemsTarget = parameters.HorizontalPlanes + parameters.VerticalPlanes;

    if (occlusionManager != null)
        occlusionManager.enabled = parameters.UseOcclusion;

    planeManager.requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
    if (parameters.HorizontalPlanes > 0)
        planeManager.requestedDetectionMode |= UnityEngine.XR.ARSubsystems.PlaneDetectionMode.Horizontal;
    if (parameters.VerticalPlanes > 0)
        planeManager.requestedDetectionMode |= UnityEngine.XR.ARSubsystems.PlaneDetectionMode.Vertical;

    planeManager.enabled = true;
    planeManager.planesChanged += (args) => OnPlanesChanged(args);


    UpdateUI();
}

void Update()
{
    if (!gameActive) return;

    currentTime -= Time.deltaTime;
    UpdateUI();

    if (currentTime <= 0)
    {
        planeManager.enabled = false;
        EndGame(); 
    }
}

void OnPlanesChanged(ARPlanesChangedEventArgs args)
{
        Debug.Log("Hey");
        int totalDetected = planeManager.trackables.count;
        if (totalDetected == gemsTarget)
            { StartGame(); planeManager.enabled = false; }
}

void StartGame()
{
    currentTime = parameters.TotalTime;

    gameActive = true;
    Debug.Log("Juego iniciado.");

    int spawned = 0;
    foreach (ARPlane plane in planeManager.trackables)
    {
        if (spawned >= gemsTarget) break;
        Instantiate(gemPrefab, plane.center, Quaternion.identity);
        spawned++;
    }

    UpdateUI();
}

public void CollectGem(GameObject gameObject)
{
    gemsCollected++;
    UpdateUI();

    if (gemsCollected >= gemsTarget)
        EndGame();

    AudioManager.Instance.PlayGemSound(gemSound);
    Destroy(gameObject);
}

int GemsRemaining()
{
    return Mathf.Max(0, gemsTarget - gemsCollected);
}

    void UpdateUI()
    {
        if (collectedText != null)
            collectedText.text = "Gemas recogidas: " + gemsCollected;

        if (remainingText != null)
            remainingText.text = "Gemas restantes: " + GemsRemaining();

        if (planesText != null)
            planesText.text = "Planos objetivo: " + gemsTarget;

        if (timeText != null)
        { timeText.text = "Tiempo: " + Mathf.CeilToInt(currentTime) + "s"; }

    }
    public void EndGame()
    {
        if (gameEnded) return;

        gameEnded = true;
        gameActive = false;

        if (planeManager != null)
            planeManager.planesChanged -= OnPlanesChanged;

        if (planeManager != null)
            planeManager.enabled = false;

        if (occlusionManager != null)
            occlusionManager.enabled = false;

        if (gameUICanvasGroup != null)
        {
            gameUICanvasGroup.interactable = false;
            gameUICanvasGroup.blocksRaycasts = false;
        }

        if (endPanel != null)
            endPanel.SetActive(true);

        Debug.Log("Juego finalizado. Gemas: " + gemsCollected);

    }

    public void VolverAlInicio()
    {
        SceneManager.LoadScene("InicioAr3");
    }


}
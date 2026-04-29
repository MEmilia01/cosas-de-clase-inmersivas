using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameManagerAR : MonoBehaviour
{
    public static GameManagerAR Instance;

    [Header("Parameters")]
    public GameParameters parameters;

    [Header("AR Components")]
    public ARPlaneManager planeManager;
    public AROcclusionManager occlusionManager;
    public GameObject gemPrefab;

    [Header("UI")]
    public TMPro.TextMeshProUGUI collectedText;
    public TMPro.TextMeshProUGUI remainingText;
    public TMPro.TextMeshProUGUI planesText;
    public TMPro.TextMeshProUGUI timeText;

    private float currentTime;
    private int gemsCollected = 0;
    private int gemsTarget = 0;
    public bool gameActive = false;

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

    IEnumerator Start()
    {
        planeManager.enabled = false;

        yield return new WaitForSeconds(1f);

        if (parameters == null)
        {
            Debug.LogError("GameParameters no asignado.");
            yield break;
        }

        currentTime = parameters.totalTime;
        gemsTarget = parameters.horizontalPlanes + parameters.verticalPlanes;

        if (occlusionManager != null)
            occlusionManager.enabled = parameters.useOcclusion;

        planeManager.requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
        if (parameters.horizontalPlanes > 0)
            planeManager.requestedDetectionMode |= UnityEngine.XR.ARSubsystems.PlaneDetectionMode.Horizontal;
        if (parameters.verticalPlanes > 0)
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
            EndGame();
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (!gameActive)
        {
            int totalDetected = planeManager.trackables.count;
            if (totalDetected >= gemsTarget)
                StartGame();
        }
    }

    void StartGame()
    {
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

    public void CollectGem()
    {
        gemsCollected++;
        UpdateUI();

        if (gemsCollected >= gemsTarget)
            EndGame();
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
            timeText.text = "Tiempo: " + Mathf.CeilToInt(currentTime) + "s";
    }

    void EndGame()
    {
        gameActive = false;
        if (planeManager != null)
            planeManager.planesChanged -= OnPlanesChanged;

        Debug.Log("Juego finalizado. Gemas: " + gemsCollected);
    }

    public void VolverAlInicio()
    {
        SceneManager.LoadScene("InicioAr3");
    }
}
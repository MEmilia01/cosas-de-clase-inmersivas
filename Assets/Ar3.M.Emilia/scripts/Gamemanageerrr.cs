using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using System.Collections;


public class GameManagerAR : MonoBehaviour
{
    [Header("Parameters")]
    public GameParameters parameters;

    [Header("AR Components")]
    public ARPlaneManager planeManager;
    public AROcclusionManager occlusionManager;
    public GameObject gemPrefab;

    private float currentTime;
    private int gemsCollected = 0;
    public bool gameActive = false;

    IEnumerator Start()
    {
        Debug.Log($"Valores recibidos en Escena 1: H={parameters.horizontalPlanes}, V={parameters.verticalPlanes}");
        // 1. Desactivamos la detección hasta que estemos listos
        planeManager.enabled = false;

        // 2. Esperamos 1 segundo para que la cámara y el tracking AR se inicialicen
        yield return new WaitForSeconds(1.0f);

        // 3. Aplicamos parámetros
        currentTime = parameters.totalTime;
        if (occlusionManager != null)
            occlusionManager.enabled = parameters.useOcclusion;

        // 4. Configurar modos de detección
        planeManager.requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
        if (parameters.horizontalPlanes > 0) planeManager.requestedDetectionMode |= UnityEngine.XR.ARSubsystems.PlaneDetectionMode.Horizontal;
        if (parameters.verticalPlanes > 0) planeManager.requestedDetectionMode |= UnityEngine.XR.ARSubsystems.PlaneDetectionMode.Vertical;

        // 5. Activamos el manager y nos suscribimos al evento
        planeManager.enabled = true;
        planeManager.planesChanged += (args) => OnPlanesChanged(args);

        Debug.Log("Sistema AR iniciado correctamente.");
    }

    void Update()
    {
        if (!gameActive) return;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0) EndGame();
    }

    // Firma actualizada para AR Foundation 6.0+
    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        // Solo instanciamos si el juego aún no está activo
        if (!gameActive)
        {
            int totalDetected = planeManager.trackables.count;
            if (totalDetected >= (parameters.horizontalPlanes + parameters.verticalPlanes))
            {
                StartGame();
            }
        }
    }

    void StartGame()
    {
        gameActive = true;
        Debug.Log("Juego iniciado.");

        // Instanciar una gema por cada plano detectado hasta el límite
        foreach (ARPlane plane in planeManager.trackables)
        {
            Instantiate(gemPrefab, plane.center, Quaternion.identity);
        }
    }

    public void CollectGem()
    {
        gemsCollected++;
        if (gemsCollected >= (parameters.horizontalPlanes + parameters.verticalPlanes))
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameActive = false;
        planeManager.planesChanged -= OnPlanesChanged;
        Debug.Log("Juego finalizado. Gemas: " + gemsCollected);
    }
}
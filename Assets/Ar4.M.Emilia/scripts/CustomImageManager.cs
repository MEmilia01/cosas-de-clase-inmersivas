using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CustomImageManager : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    void Awake()
    {
        // Optimizamos la búsqueda para Unity 6
        trackedImageManager = FindFirstObjectByType<ARTrackedImageManager>();

        if (trackedImageManager == null)
        {
            Debug.LogError("CustomImageManager: ARTrackedImageManager no encontrado en la escena.");
            enabled = false; 
        }
    }

    void OnEnable()
    {
        if (trackedImageManager != null)
        {
            // NUEVA API: Usamos trackablesChanged y AddListener
            trackedImageManager.trackablesChanged.AddListener(OnTrackablesChanged);
        }
    }

    void OnDisable()
    {
        if (trackedImageManager != null)
        {
            // NUEVA API: Usamos RemoveListener
            trackedImageManager.trackablesChanged.RemoveListener(OnTrackablesChanged);
        }
    }

    void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        // 1. Imágenes recién detectadas
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            Debug.Log($"[AR] Imagen AÑADIDA: '{trackedImage.referenceImage.name}'");
            // Aquí puedes inicializar el estado del modelo 3D
        }

        // 2. Imágenes que se mueven o cambian de estado de seguimiento
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                 // La cámara ve la imagen claramente
                 Debug.Log($"[AR] Imagen '{trackedImage.referenceImage.name}' TRACKING.");
            }
            else 
            {
                 // La cámara perdió la imagen (Limited o None)
                 Debug.Log($"[AR] Imagen '{trackedImage.referenceImage.name}' PERDIDA.");
            }
        }
    }
}
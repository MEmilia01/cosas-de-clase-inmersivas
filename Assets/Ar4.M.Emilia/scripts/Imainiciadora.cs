using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTargetAnchor : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private GameObject contentPrefab;

    private readonly Dictionary<TrackableId, GameObject> spawnedContent = new();

    private void OnEnable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackablesChanged.AddListener(OnTrackedImagesChanged);
    }

    private void OnDisable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackablesChanged.RemoveListener(OnTrackedImagesChanged);
    }

    private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
            HandleImage(trackedImage);

        foreach (var trackedImage in eventArgs.updated)
            HandleImage(trackedImage);

        foreach (var trackedImage in eventArgs.removed)
        {
            if (spawnedContent.TryGetValue(trackedImage.Key, out GameObject obj))
            {
                Destroy(obj);
                spawnedContent.Remove(trackedImage.Key);
            }
        }
    }

    private void HandleImage(ARTrackedImage trackedImage)
    {
        if (trackedImage.trackingState != TrackingState.Tracking)
        {
            if (spawnedContent.TryGetValue(trackedImage.trackableId, out GameObject obj))
                obj.SetActive(false);
            return;
        }

        if (!spawnedContent.TryGetValue(trackedImage.trackableId, out GameObject content))
        {
            content = Instantiate(contentPrefab, trackedImage.transform);
            content.transform.localPosition = Vector3.zero;
            content.transform.localRotation = Quaternion.identity;
            content.transform.localScale = Vector3.one;
            spawnedContent.Add(trackedImage.trackableId, content);
        }

        content.SetActive(true);
        content.transform.SetParent(trackedImage.transform, false);
        content.transform.localPosition = Vector3.zero;
        content.transform.localRotation = Quaternion.identity;
    }
}
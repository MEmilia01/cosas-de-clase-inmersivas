using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArrowSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ArrowData
    {
        public GameObject prefab;
        public Vector3 localPosition;
        public Vector3 localRotationEuler;
        public Vector3 localScale = Vector3.one;
    }

    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private List<ArrowData> arrows = new List<ArrowData>();

    private readonly Dictionary<string, GameObject> spawnedByImage = new Dictionary<string, GameObject>();

    private void OnEnable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
            SpawnOrUpdate(trackedImage);

        foreach (var trackedImage in args.updated)
            SpawnOrUpdate(trackedImage);

        foreach (var trackedImage in args.removed)
            RemoveSpawned(trackedImage);
    }

    private void SpawnOrUpdate(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (trackedImage.trackingState != TrackingState.Tracking)
        {
            if (spawnedByImage.ContainsKey(imageName))
                spawnedByImage[imageName].SetActive(false);
            return;
        }

        if (!spawnedByImage.ContainsKey(imageName))
        {
            GameObject container = new GameObject($"Arrows_{imageName}");
            container.transform.SetParent(trackedImage.transform, false);
            container.transform.localPosition = Vector3.zero;
            container.transform.localRotation = Quaternion.identity;
            container.transform.localScale = Vector3.one;

            for (int i = 0; i < arrows.Count; i++)
            {
                if (arrows[i].prefab == null) continue;

                GameObject arrow = Instantiate(arrows[i].prefab, container.transform);
                arrow.transform.localPosition = arrows[i].localPosition;
                arrow.transform.localRotation = Quaternion.Euler(arrows[i].localRotationEuler);
                arrow.transform.localScale = arrows[i].localScale;
                arrow.SetActive(i == 0);
            }

            spawnedByImage.Add(imageName, container);
        }
        else
        {
            spawnedByImage[imageName].SetActive(true);
        }
    }

    private void RemoveSpawned(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (spawnedByImage.ContainsKey(imageName))
        {
            Destroy(spawnedByImage[imageName]);
            spawnedByImage.Remove(imageName);
        }
    }
}

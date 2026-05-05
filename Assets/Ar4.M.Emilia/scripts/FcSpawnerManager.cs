using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArrowSpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class ArrowStep
    {
        public GameObject prefab;
        public Vector3 localPosition;
        public Vector3 localRotationEuler;
        public Vector3 localScale = Vector3.one;
    }

    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private List<ArrowStep> arrowSteps = new List<ArrowStep>();

    private class SpawnedSequence
    {
        public GameObject root;
        public List<GameObject> arrows = new List<GameObject>();
        public int currentIndex = 0;
    }

    private readonly Dictionary<string, SpawnedSequence> sequencesByImage = new Dictionary<string, SpawnedSequence>();

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

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
            CreateOrRefreshSequence(trackedImage);

        foreach (var trackedImage in eventArgs.updated)
            CreateOrRefreshSequence(trackedImage);

        foreach (var trackedImage in eventArgs.removed)
            RemoveSequence(trackedImage);
    }

    private void CreateOrRefreshSequence(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (trackedImage.trackingState != TrackingState.Tracking)
        {
            if (sequencesByImage.ContainsKey(imageName))
                sequencesByImage[imageName].root.SetActive(false);
            return;
        }

        if (!sequencesByImage.ContainsKey(imageName))
        {
            SpawnedSequence sequence = new SpawnedSequence();

            sequence.root = new GameObject($"ArrowSequence_{imageName}");
            sequence.root.transform.SetParent(trackedImage.transform, false);
            sequence.root.transform.localPosition = Vector3.zero;
            sequence.root.transform.localRotation = Quaternion.identity;
            sequence.root.transform.localScale = Vector3.one;

            for (int i = 0; i < arrowSteps.Count; i++)
            {
                if (arrowSteps[i].prefab == null) continue;

                GameObject arrow = Instantiate(arrowSteps[i].prefab, sequence.root.transform);
                arrow.transform.localPosition = arrowSteps[i].localPosition;
                arrow.transform.localRotation = Quaternion.Euler(arrowSteps[i].localRotationEuler);
                arrow.transform.localScale = arrowSteps[i].localScale;
                arrow.SetActive(i == 0);
                sequence.arrows.Add(arrow);
            }

            sequencesByImage.Add(imageName, sequence);
        }
        else
        {
            sequencesByImage[imageName].root.SetActive(true);
        }
    }

    private void RemoveSequence(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (!sequencesByImage.ContainsKey(imageName)) return;

        Destroy(sequencesByImage[imageName].root);
        sequencesByImage.Remove(imageName);
    }

    public void ActivateNextArrow(string imageName)
    {
        if (!sequencesByImage.ContainsKey(imageName)) return;

        SpawnedSequence sequence = sequencesByImage[imageName];

        if (sequence.currentIndex < sequence.arrows.Count)
            sequence.arrows[sequence.currentIndex].SetActive(false);

        sequence.currentIndex++;

        if (sequence.currentIndex < sequence.arrows.Count)
            sequence.arrows[sequence.currentIndex].SetActive(true);
    }

    public void RestartSequence(string imageName)
    {
        if (!sequencesByImage.ContainsKey(imageName)) return;

        SpawnedSequence sequence = sequencesByImage[imageName];

        for (int i = 0; i < sequence.arrows.Count; i++)
            sequence.arrows[i].SetActive(i == 0);

        sequence.currentIndex = 0;
    }
}
using UnityEngine;

public class ArrowTrigger : MonoBehaviour
{
    [SerializeField] private ArrowSpawnManager arrowSpawnManager;
    [SerializeField] private string imageName;
    [SerializeField] private string playerTag = "MainCamera";

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag(playerTag)) return;

        triggered = true;

        if (arrowSpawnManager != null)
        {
            arrowSpawnManager.ActivateNextArrow(imageName);
        }

        gameObject.SetActive(false);
    }
}
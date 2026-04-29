
using UnityEngine;

public class GemController : MonoBehaviour
{
    public AudioClip collectSound;
    private GameManagerAR gameManager;

    void Start()
    {
        gameManager = Object.FindFirstObjectByType<GameManagerAR>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);

        if (gameManager != null)
            gameManager.CollectGem();

        Destroy(gameObject);
    }
}
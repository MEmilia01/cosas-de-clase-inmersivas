using UnityEngine;

public class GemController : MonoBehaviour
{
    // Asigna el clip de sonido en el Inspector para esta gema
    public AudioClip collectSound;
    private GameManagerAR gameManager;

    void Start()
    {
        // Busca el GameManager en la escena para notificarle cuando se recoja la gema
        gameManager = Object.FindFirstObjectByType<GameManagerAR>();
    }

    // Detecta cuando la cámara (o el jugador) entra en el trigger de la gema
    void OnTriggerEnter(Collider other)
    {
        // Asegúrate de que tu cámara tenga el tag "MainCamera"
        if (other.CompareTag("MainCamera"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        // Reproduce el sonido en la posición de la gema antes de destruirla
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        // Notifica al gestor del juego que una gema ha sido recogida
        if (gameManager != null)
        {
            gameManager.CollectGem();
        }

        // Elimina la gema de la escena
        Destroy(gameObject);
    }
}
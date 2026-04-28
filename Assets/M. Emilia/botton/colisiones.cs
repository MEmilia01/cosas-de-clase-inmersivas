using UnityEngine;

public class Colisiones : MonoBehaviour
{
    [SerializeField] private Camescenas camescenas;

    private void Start()
    {
        if (camescenas == null)
            camescenas = FindFirstObjectByType<Camescenas>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("paraescenauno"))
        {
            camescenas.MostrarMenuEscena1();
            Debug.Log("Bloque de escena 1 detectado.");
        }
        else if (collision.gameObject.CompareTag("paraescena2"))
        {
            camescenas.MostrarMenuEscena2();
            Debug.Log("Bloque de escena 2 detectado.");
        }
    }
}

using UnityEngine;

public class Colisiones : MonoBehaviour
{
    [SerializeField] private Camescenas camescenas;

    private void Start()
    {
        if (camescenas == null)
            camescenas = FindFirstObjectByType<Camescenas>();

        if (camescenas == null)
            Debug.LogError("No se encontró Camescenas en la escena.");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detectado con: " + other.name + " | Tag: " + other.tag);

        if (camescenas == null)
            return;

        if (other.CompareTag("paraescenauno"))
        {
            camescenas.MostrarMenuEscena1();
            Debug.Log("Bloque de escena 1 detectado.");
        }
        else if (other.CompareTag("paraescena2"))
        {
            camescenas.MostrarMenuEscena2();
            Debug.Log("Bloque de escena 2 detectado.");
        }
        else
        {
            Debug.Log("Entró algo al trigger, pero el tag no coincide.");
        }
    }
}

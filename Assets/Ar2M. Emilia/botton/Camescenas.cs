using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class Camescenas : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] public GameObject menuPregunta;

    public string escenaDestino = "";

    void Start()
    {
        if (menuPregunta != null)
            menuPregunta.SetActive(false);
        else
            Debug.LogError("No has asignado menuPregunta en el Inspector.");
    }

    public void MostrarMenuEscena1()
    {
        escenaDestino = "escena1";
        if (menuPregunta != null)
            menuPregunta.SetActive(true);
    }

    public void MostrarMenuEscena2()
    {
        escenaDestino = "escenas2";
        if (menuPregunta != null)
            menuPregunta.SetActive(true);
    }

    public void CancelarMenu()
    {
        if (menuPregunta != null)
            menuPregunta.SetActive(false);
    }

    public void Salir()
    {
        SceneManager.LoadScene("Inicio");
    }

    public void ConfirmarCambio()
    {
        if (string.IsNullOrEmpty(escenaDestino))
        {
            Debug.LogWarning("No hay escena destino asignada.");
            return;
        }

        SceneManager.LoadScene(escenaDestino);
    }

    public void ProbarTrigger(string nombreObjeto)
    {
        Debug.Log("TRIGGER detectado con: " + nombreObjeto);
    }

    public void ProbarColision(string nombreObjeto)
    {
        Debug.Log("COLISION detectada con: " + nombreObjeto);
    }
}
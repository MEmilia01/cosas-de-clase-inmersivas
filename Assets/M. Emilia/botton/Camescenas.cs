using UnityEngine;
using UnityEngine.SceneManagement;

public class Camescenas : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Cambioesc()
    {
        SceneManager.LoadScene("Inicio");
    }

    public void Cambioescuno()
    {
        SceneManager.LoadScene("Inicio");
    }
    public void Cambioescdos()
    {
        SceneManager.LoadScene("Inicio");
    }


    public void OnCollisionEnter(Collision collision)
    {

    }
}
//tengo que hacer que al colisoniar con los bloques, este de le la opcion al menu y vaya a la escena necesaria
using UnityEngine;
using UnityEngine.SceneManagement;

public class Camescenas : MonoBehaviour
{
    public GameObject menupregunta;
    public bool iraesc1;

    Scene actual;

    void Start()
    {
        iraesc1 = true;
        menupregunta.SetActive(false);
        SceneManager.LoadScene("Inicio");
    }

    private void Update()
    {
        if (actual.name == "escena1")
        {
            menupregunta = null;
        }
        else if (actual.name == "escenas2")
        {
            menupregunta = null;
        }
    }

    public void Cambioesc()
    {
        Debug.Log("putamierda");
        SceneManager.LoadScene("Inicio");
    }

    public void Cambioescuno()
    {
        SceneManager.LoadScene("escena1");
    }
    public void Cambioescdos()
    {
        SceneManager.LoadScene("escenas2");
    }

    public void OnCollisionEnter(Collision collision)
    {

    }
}
//tengo que hacer que al colisoniar con los bloques, este de le la opcion al menu y vaya a la escena necesaria
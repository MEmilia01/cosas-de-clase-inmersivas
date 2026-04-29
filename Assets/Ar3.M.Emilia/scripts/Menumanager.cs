using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Sliders")]
    public Slider horizontalSlider;
    public Slider verticalSlider;
    public Slider timeSlider;

    [Header("Text Displays")]
    public TextMeshProUGUI horizontalText;
    public TextMeshProUGUI verticalText;
    public TextMeshProUGUI timeText;

    [Header("Data Asset")]
    public GameParameters gameParameters;

    public Toggle sonido;

    void Start()
    {
        // Añadir listeners para actualizar el texto en tiempo real
        horizontalSlider.onValueChanged.AddListener(UpdateUI);
        verticalSlider.onValueChanged.AddListener(UpdateUI);
        timeSlider.onValueChanged.AddListener(UpdateUI);

        // Llamada inicial para fijar los textos
        UpdateUI(0);
    }


    void UpdateUI(float value)
    {
        horizontalText.text = "Planos Horizontales: " + (int)horizontalSlider.value;
        verticalText.text = "Planos Verticales: " + (int)verticalSlider.value;
        timeText.text = "Tiempo: " + (int)timeSlider.value + "s";
    }

    public void OnStartGameButtonClicked()
    {
        if (gameParameters != null)
        {
            gameParameters.horizontalPlanes = (int)horizontalSlider.value;
            gameParameters.verticalPlanes = (int)verticalSlider.value;
            gameParameters.totalTime = timeSlider.value;
        }

        SceneManager.LoadScene("JuegoAr3");
    }

    public void ToggleSound(bool isMuted)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMute(isMuted);
            Debug.Log("Estado de silencio: " + isMuted);
        }
    }
}
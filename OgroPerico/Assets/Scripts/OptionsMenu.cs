using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    void Start()
    {
        // Inicializa el slider con el volumen actual
        volumeSlider.value = AudioManager.Instance.volumen;

        // Escucha cambios del slider
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    public void ChangeVolume(float value)
    {
        AudioManager.Instance.volumen = value;
    }
}


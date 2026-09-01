using UnityEngine;
using UnityEngine.UI;

public class UI_MiniHealthBarControl : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private float lastValue;

    private void Awake()
    {
        lastValue = slider.value;
        slider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (slider.value != lastValue)
        {
            lastValue = slider.value;

            slider.gameObject.SetActive(slider.value > 0);
        }
    }
}

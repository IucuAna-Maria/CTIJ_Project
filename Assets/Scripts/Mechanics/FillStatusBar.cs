using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.UI;


public class FillStatusBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth; 
    [SerializeField] private Image fillImage; 
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        if(slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }

        if (slider.value > slider.minValue && !fillImage.enabled)
        { 
            fillImage.enabled = true;
        }

        float fillValue = playerHealth.CurrentHP;
        slider.value = fillValue;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider; // Slider reference
    public Vector3 offset; // Offset to adjust the position above the unit's head

    private Transform target; // The unit the health bar follows

    void Start()
    {
        // Ensure the slider is properly assigned
        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();
        }
    }

    void Update()
    {
        // Follow the target's position
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    public void SetHealth(float health, float maxHealth)
    {
        slider.value = health / maxHealth;
    }
}

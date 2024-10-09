using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Attributes")]
    public Color barColor = Color.green;
    public Color backgoundColor = Color.red;
    [Header("Internals")]
    public Image foreground;
    public Image background;

    public void OnEnable()
    {
        foreground.color = barColor;
        background.color = backgoundColor;
    }

    public void SetPercent(float p)
    {
        foreground.fillAmount = Mathf.Clamp(p, 0f, 1f);
    }

    public void MoveTo(Vector3 p)
    {
        transform.position = p;
    }
}

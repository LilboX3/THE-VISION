using UnityEngine;
using UnityEngine.UI;

public class LineRendererUI : MonoBehaviour
{
    [SerializeField] private RectTransform m_myTransform;
    [SerializeField] private Image m_image;

    public void CreateLine(Vector3 positionOne, Vector3 positionTwo, Color color)
    {
        m_image.color = color;

        Vector2 start = positionOne;
        Vector2 end = positionTwo;
        Vector2 dir = end - start;

        m_myTransform.position = start;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        m_myTransform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Set line length
        m_myTransform.sizeDelta = new Vector2(dir.magnitude, m_myTransform.sizeDelta.y);
    }
}

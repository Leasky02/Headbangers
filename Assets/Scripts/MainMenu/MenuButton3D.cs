using TMPro;
using UnityEngine;

public class MenuButton3D : MonoBehaviour
{
    [SerializeField] private Renderer cubeRenderer;

    [SerializeField] private TextMeshPro text;

    private bool m_selected = false;

    void Awake()
    {
        // SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        m_selected = selected;
        if (m_selected)
        {
            cubeRenderer.material.color = new Color(0, 204f / 255, 0, 1);
            text.color = new Color(1, 1, 1, 1);
        }
        else
        {
            cubeRenderer.material.color = new Color(255f / 255, 204f / 255, 0, 1);
            text.color = new Color(1, 1, 1, 1);
        }
    }
}

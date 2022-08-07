using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayNameText : MonoBehaviour
{
    [SerializeField] private int width = 150;

    [SerializeField] private int height = 50;

    [SerializeField] private GUIStyle textStyle;

    private GUIContent m_guiContent;

    private Vector3 m_screenPos;

    private Camera m_camera = null;

    private bool m_shouldRender = true; // TODO: set when button being held

    private bool m_onScreen = false;

    public void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void SetDisplayName(string displayName)
    {
        m_guiContent = new GUIContent(displayName);
    }

    public void Update()
    {
        if (m_camera && m_shouldRender)
        {
            m_screenPos = m_camera.WorldToScreenPoint(transform.position);

            // We need to fix up the Y value, because of a difference in space between the Screen and the GUI
            m_screenPos.y = Screen.height - m_screenPos.y;

            // Find wether m_screenPos is in the screen
            // We need to take into account that the GUI should be in the middle of the object
            m_onScreen = (width / 2 < m_screenPos.x && m_screenPos.x < Screen.width - width / 2 &&
                            height / 2 < m_screenPos.y && m_screenPos.y < Screen.height - height / 2 &&
                            0.1 < m_screenPos.z && m_screenPos.z < 1000);
        }
        else
        {
            m_onScreen = false;
        }
    }

    public void OnGUI()
    {
        if (m_onScreen)
        {
            // When displaying the GUI, we need to take into account that the position is in the top right
            GUI.Label(new Rect(m_screenPos.x - width / 2, m_screenPos.y - height / 2, width, height), m_guiContent, textStyle);
        }
    }
}

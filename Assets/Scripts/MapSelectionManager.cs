using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectionManager : MonoBehaviour
{
    [SerializeField] string[] mapNames;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SelectRandomMap", 1f);
    }

    private void SelectRandomMap()
    {
        int randomMapIndex = Random.Range(0, mapNames.Length);
        SceneManager.LoadScene(mapNames[randomMapIndex]);
    }
}

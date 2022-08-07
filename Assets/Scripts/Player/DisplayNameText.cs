using TMPro;
using UnityEngine;

public class DisplayNameText : MonoBehaviour
{
    public void Awake()
    {
        gameObject.SetActive(false);
    }

    public void SetText(string text)
    {
        GetComponent<TextMeshPro>().text = text;
        gameObject.SetActive(true);
    }
}

using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
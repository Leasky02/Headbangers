using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private MenuButton3D menuButton3D;

    public void OnSelect(BaseEventData eventData)
    {
        menuButton3D.SetSelected(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        menuButton3D.SetSelected(false);
    }
}

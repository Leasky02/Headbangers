using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    [SerializeField] private Outline playerOutline;

    [SerializeField] private MeshRenderer bodyMeshRenderer;
    [SerializeField] private MeshRenderer headphoneMeshRenderer;

    [SerializeField] private Material standardMaterial;
    [SerializeField] private Material transparentMaterial;

    [SerializeField] private Material standardMaterial_Headphone;
    [SerializeField] private Material transparentMaterial_Headphone;

    [SerializeField] private MeshRenderer shoeMeshRenderer_L;
    [SerializeField] private MeshRenderer shoeMeshRenderer_R;

    [SerializeField] private SpriteRenderer[] faceParts;

    [SerializeField] private float deadTransparency;

    public void ApplyColor(Color playerColor)
    {
        playerOutline.OutlineColor = new Color(playerColor.r, playerColor.g, playerColor.b, 1);
        UpdateMaterial();
    }

    //update material with colour and transparency based on dead state
    public void UpdateMaterial()
    {
        Color playerColor = PlayerColorManager.Instance.GetColor(PlayerConfigurationManager.Instance.GetPlayerColorID(Player.GetPlayerComponent(gameObject).GetPlayerIndex()));
        bool isDead = Player.GetPlayerComponent(gameObject).IsDead();
        if (isDead)
        {

            Color transparentColor_Headphone = new Color(transparentMaterial_Headphone.color.r, transparentMaterial_Headphone.color.g, transparentMaterial_Headphone.color.b, deadTransparency);
            headphoneMeshRenderer.material = transparentMaterial_Headphone;
            headphoneMeshRenderer.material.color = transparentColor_Headphone;

            Color transparentColor = new Color(playerColor.r, playerColor.g, playerColor.b, deadTransparency);
            bodyMeshRenderer.material = transparentMaterial;
            bodyMeshRenderer.material.color = transparentColor;
        }
        else
        {
            bodyMeshRenderer.material = standardMaterial;

            headphoneMeshRenderer.material = standardMaterial_Headphone;

            bodyMeshRenderer.material.color = playerColor;
        }

        UpdateFace(isDead);
    }

    //update transparency of face based on dead state
    private void UpdateFace(bool isDead)
    {
        //change transparency of each face component
        foreach (SpriteRenderer facePart in faceParts)
        {
            if (isDead)
            {
                Color newColor = new Color(facePart.color.r, facePart.color.g, facePart.color.b, deadTransparency);
                facePart.color = newColor;
            }
            else
            {
                Color newColor = new Color(facePart.color.r, facePart.color.g, facePart.color.b, 1f);
                facePart.color = newColor;
            }
        }
    }
}

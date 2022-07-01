using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    [SerializeField] private MeshRenderer bodyMeshRenderer;

    [SerializeField] private Material standardMaterial;
    [SerializeField] private Material transparentMaterial;

    [SerializeField] private Material standardMaterial_Shoe;
    [SerializeField] private Material transparentMaterial_Shoe;

    [SerializeField] private MeshRenderer shoeMeshRenderer_L;
    [SerializeField] private MeshRenderer shoeMeshRenderer_R;

    [SerializeField] private SpriteRenderer[] faceParts;

    private Color playerColor;
    [SerializeField] private float deadTransparency;

    private void Start()
    {
        playerColor = playerData.GetPlayerColor();
        bodyMeshRenderer.material.color = playerColor;
    }

    private void UpdateColor(Color newColor)
    {
        playerData.SetPlayerColor(newColor);
        playerColor = newColor;

        UpdateMaterial();
    }

    public void UpdateMaterial()
    {
        if(playerData.GetDead())
        {
            bodyMeshRenderer.material = transparentMaterial;

            shoeMeshRenderer_L.material = transparentMaterial_Shoe;
            shoeMeshRenderer_R.material = transparentMaterial_Shoe;

            Color transparentColor = new Color(playerColor.r, playerColor.g, playerColor.b, deadTransparency);
            bodyMeshRenderer.material.color = transparentColor;
        }
        else
        {
            bodyMeshRenderer.material = standardMaterial;

            shoeMeshRenderer_L.material = standardMaterial_Shoe;
            shoeMeshRenderer_R.material = standardMaterial_Shoe;

            bodyMeshRenderer.material.color = playerColor;
        }

        UpdateFace();
    }

    public void UpdateFace()
    {
        //change transparency of face
        foreach (SpriteRenderer facePart in faceParts)
        {
            if(playerData.GetDead())
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

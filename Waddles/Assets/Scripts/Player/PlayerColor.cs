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

    [SerializeField] private float deadTransparency;

    private void Start()
    {
        bodyMeshRenderer.material.color = playerData.GetPlayerColor();
    }

    //change player color
    private void UpdateColor(Color newColor)
    {
        playerData.SetPlayerColor(newColor);

        UpdateMaterial();
    }

    //update material with colour and transparency based on dead state
    public void UpdateMaterial()
    {
        if(playerData.GetDead())
        {
            bodyMeshRenderer.material = transparentMaterial;

            shoeMeshRenderer_L.material = transparentMaterial_Shoe;
            shoeMeshRenderer_R.material = transparentMaterial_Shoe;

            Color transparentColor = new Color(playerData.GetPlayerColor().r, playerData.GetPlayerColor().g, playerData.GetPlayerColor().b, deadTransparency);
            bodyMeshRenderer.material.color = transparentColor;
        }
        else
        {
            bodyMeshRenderer.material = standardMaterial;

            shoeMeshRenderer_L.material = standardMaterial_Shoe;
            shoeMeshRenderer_R.material = standardMaterial_Shoe;

            bodyMeshRenderer.material.color = playerData.GetPlayerColor();
        }

        UpdateFace();
    }

    //update transparency of face based on dead state
    public void UpdateFace()
    {
        //change transparency of each face component
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

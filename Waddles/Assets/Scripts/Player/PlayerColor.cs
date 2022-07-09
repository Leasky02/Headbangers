using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerColor : MonoBehaviour
{
    //default colours for the player
    [SerializeField] private Color[] playerColours;
    [SerializeField] public static bool[] colorTaken = new bool[8];
    [SerializeField] private int currentColorID;

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

    private PlayerConfiguration myPlayerConfig;

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

    public void OnColorUp(InputAction.CallbackContext context)
    {
        if (myPlayerConfig.IsReady)
            return;

        if (!context.performed)
        {
            return;
        }
        Debug.Log("colorUp");
        ColorUp();
    }
    public void OnColorDown(InputAction.CallbackContext context)
    {
        if (myPlayerConfig.IsReady)
            return;

        if (!context.performed)
        {
            return;
        }
        Debug.Log("colorDown");
        ColorDown();
    }

    public void DefaultColor(PlayerConfiguration playerConfig)
    {
        myPlayerConfig = playerConfig;
        bool colorAssigned = false;
        currentColorID = playerConfig.PlayerIndex;
        Color newColor;
        do
        {
            if(!colorTaken[currentColorID])
            {
                newColor = playerColours[currentColorID];
                UpdateColor(newColor);
                colorTaken[currentColorID] = true;
                colorAssigned = true;
            }
            else
            {
                colorAssigned = false;
                currentColorID++;
                if(currentColorID >= colorTaken.Length)
                    currentColorID = 0;
            }
        } while (colorAssigned == false);
    }

    public void ColorUp()
    {
        //determine if another colour is available
        bool availableColor = false;
        foreach(bool colorUsed in colorTaken)
        {
            if(colorUsed == false)
            {
                availableColor = true;
                break;
            }
            else
            {
                availableColor = false;
            }
        }

        if(availableColor)
        {
            bool colorAssigned = false;
            Color newColor;

            colorTaken[currentColorID] = false;

            do
            {
                currentColorID++;
                if (currentColorID >= colorTaken.Length)
                    currentColorID = 0;

                if (!colorTaken[currentColorID])
                {
                    newColor = playerColours[currentColorID];
                    UpdateColor(newColor);
                    colorTaken[currentColorID] = true;
                    colorAssigned = true;

                }

            } while (colorAssigned == false);
        }
    }

    public void ColorDown()
    {
        //determine if another colour is available
        bool availableColor = false;
        foreach (bool colorUsed in colorTaken)
        {
            if (colorUsed == false)
            {
                availableColor = true;
                break;
            }
            else
            {
                availableColor = false;
            }
        }

        if (availableColor)
        {
            bool colorAssigned = false;
            Color newColor;

            colorTaken[currentColorID] = false;

            do
            {
                currentColorID--;
                if (currentColorID < 0)
                    currentColorID = colorTaken.Length - 1;

                if (!colorTaken[currentColorID])
                {
                    newColor = playerColours[currentColorID];
                    UpdateColor(newColor);
                    colorTaken[currentColorID] = true;
                    colorAssigned = true;

                }

            } while (colorAssigned == false);
        }
    }

    public int GetColourID()
    {
        return currentColorID;
    }
}

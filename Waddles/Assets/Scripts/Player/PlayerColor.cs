using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerColor : MonoBehaviour
{
    //default colours for the player
    [SerializeField] private Color[] playerColours;
    [SerializeField] public static bool[] colorTaken = new bool[8];
    [SerializeField] private int currentColorID;

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

    //change player color
    private void UpdateColor(Color newColor)
    {
        PlayerConfigurationManager.Instance.SetPlayerColor(Player.GetPlayerComponent(gameObject).GetPlayerIndex(), newColor);
        playerOutline.OutlineColor = new Color(newColor.r, newColor.g, newColor.b, 1);

        UpdateMaterial();
    }

    //update material with colour and transparency based on dead state
    public void UpdateMaterial()
    {
        Color playerColor = PlayerConfigurationManager.Instance.GetPlayerColor(Player.GetPlayerComponent(gameObject).GetPlayerIndex());
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

    public void OnColorUp(InputAction.CallbackContext context)
    {
        if (PlayerConfigurationManager.Instance.IsPlayerReady(Player.GetPlayerComponent(gameObject).GetPlayerIndex()))
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
        if (PlayerConfigurationManager.Instance.IsPlayerReady(Player.GetPlayerComponent(gameObject).GetPlayerIndex()))
            return;

        if (!context.performed)
        {
            return;
        }
        Debug.Log("colorDown");
        ColorDown();
    }

    public void Init()
    {
        bool colorAssigned = false;
        currentColorID = Player.GetPlayerComponent(gameObject).GetPlayerIndex();
        Color newColor;
        do
        {
            if (!colorTaken[currentColorID])
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
                if (currentColorID >= colorTaken.Length)
                    currentColorID = 0;
            }
        } while (colorAssigned == false);
    }

    public void ColorUp()
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

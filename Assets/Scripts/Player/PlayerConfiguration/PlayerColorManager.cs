using System.Globalization;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorManager : Singleton<PlayerColorManager>
{
    //default colours for the player
    // [SerializeField]
    private Color[] playerColors = new Color[] {
        // HexToColor("E75C61"),
        // HexToColor("85BCB9"),
        // HexToColor("86CB79"),
        // HexToColor("DDCD84"),
        // HexToColor("E96897"),
        // HexToColor("B766D2"),
        // HexToColor("E2935D"),
        // HexToColor("7FA7F3"),
        new Color( 0.9056604f, 0.36226413f, 0.38078904f),
        new Color( 0.52156866f, 0.7372549f, 0.7254902f),
        new Color( 0.5254902f, 0.79607844f, 0.4745098f),
        new Color( 0.8679245f, 0.80521756f, 0.5191171f),
        new Color( 0.9150943f, 0.40920252f, 0.59062576f),
        new Color( 0.7176471f, 0.4f, 0.8235294f),
        new Color( 0.8867924f, 0.57771796f, 0.36308295f),
        new Color( 0.49798858f, 0.65494096f, 0.9528302f)
    };

    private List<int> m_colorIDsAvailable = new List<int>();

    public void Start()
    {
        for (int i = 0; i < playerColors.Length; ++i)
        {
            m_colorIDsAvailable.Add(i);
        }
    }

    private static Color HexToColor(string hex)
    {
        var bigint = int.Parse(hex, NumberStyles.HexNumber);
        var r = (bigint >> 16) & 255;
        var g = (bigint >> 8) & 255;
        var b = bigint & 255;

        return new Color(r, g, b);
    }

    public Color GetColor(int colorID)
    {
        // TODO: check within bounds
        return playerColors[colorID];
    }

    public bool IsColorAvailable(int colorID)
    {
        return m_colorIDsAvailable.Contains(colorID);
    }

    public void SetColorAvailable(int colorID)
    {
        if (colorID < 0)
        {
            return;
        }
        if (m_colorIDsAvailable.Contains(colorID))
        {
            Debug.LogError("colorID should not already be in m_colorIDsAvailable: " + colorID.ToString());
            return;
        }
        m_colorIDsAvailable.Add(colorID);
        m_colorIDsAvailable.Sort();
    }

    private void SetColorUnavailable(int colorID)
    {
        if (colorID < 0)
        {
            return;
        }
        if (!m_colorIDsAvailable.Contains(colorID))
        {
            Debug.LogError("colorID should not already be in m_colorIDsAvailable: " + colorID.ToString());
            return;
        }
        m_colorIDsAvailable.Remove(colorID);
        m_colorIDsAvailable.Sort();
    }

    public int TakeNextAvailableColorID(int currentColorID = -1)
    {
        int numAvailableColors = m_colorIDsAvailable.ToArray().Length;
        if (numAvailableColors > 0)
        {
            int nextColorID = m_colorIDsAvailable[0];
            for (int i = 0; i < numAvailableColors; ++i)
            {
                if (m_colorIDsAvailable[i] > currentColorID)
                {
                    nextColorID = m_colorIDsAvailable[i];
                    break;
                }
            }

            SetColorUnavailable(nextColorID);
            SetColorAvailable(currentColorID);
            return nextColorID;
        }

        Debug.LogError("There are no more available colours");
        return currentColorID;
    }

    public int TakePreviousAvailableColorID(int currentColorID = -1)
    {
        int numAvailableColors = m_colorIDsAvailable.ToArray().Length;
        if (numAvailableColors > 0)
        {
            int nextColorID = m_colorIDsAvailable[numAvailableColors - 1];
            for (int i = numAvailableColors - 1; i >= 0; --i)
            {
                if (m_colorIDsAvailable[i] < currentColorID)
                {
                    nextColorID = m_colorIDsAvailable[i];
                    break;
                }
            }

            SetColorUnavailable(nextColorID);
            SetColorAvailable(currentColorID);
            return nextColorID;
        }

        Debug.LogError("There are no more available colours");
        return currentColorID;
    }

    public bool HasColorsAvailable()
    {
        return m_colorIDsAvailable.ToArray().Length > 0;
    }

}
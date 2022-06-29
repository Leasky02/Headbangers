using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTest : MonoBehaviour
{
    public float transparency;
    // Start is called before the first frame update
    void Start()
    {
        Color materialColor = GetComponent<MeshRenderer>().material.color;
        GetComponent<MeshRenderer>().material.color = new Color(materialColor.r, materialColor.g, materialColor.b, transparency);
    }
}

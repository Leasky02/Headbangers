using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] meshes;

    public MeshRenderer[] GetMeshes()
    {
        return meshes;
    }

}

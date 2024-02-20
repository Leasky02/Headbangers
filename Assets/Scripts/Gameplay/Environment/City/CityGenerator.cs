using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    private Transform gridParent;

    [SerializeField] private int gridSizeX = 5; // Set the size of the grid along the X-axis
    [SerializeField] private int gridSizeY = 5; // Set the size of the grid along the Y-axis

    [Tooltip("Adjust how far apart buildings are spread")]
    [SerializeField] private float spread = 1f;

    [Tooltip("Extent of the city orientation can vary")]
    [SerializeField] private float rotationVariability = 30f;

    [Tooltip("Extent that each building orientation can vary")][Range(0f, 1f)]
    [SerializeField] private float heightAmplifier = 1f;

    [Tooltip("Extent of the building height can vary")]
    [SerializeField] private float heightVariation = 5f;

    [Tooltip("maximum height reduction")]
    [SerializeField] private float maxHeightReduction = 10f;

    [SerializeField] private Color[] buildingColours;

    //grid boundaries
    float lowestX = 0f;
    float highestX = 0f;
    float lowestZ = 0f;
    float highestZ = 0f;

    Vector3 cityCentre;

    [Tooltip("Building Prefabs")]
    [SerializeField] private GameObject[] building;

    private List<GameObject> cells;

    void Start()
    {
        gridParent = transform.GetChild(0);
        cells = new List<GameObject>();

        BuildCity();
    }

    void BuildCity()
    {
        CreateGrid();
        SetCityCentre();
        AddBuildings();
    }

    void CreateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //name object
                GameObject cell = new GameObject(x.ToString() + ", " + y.ToString());

                // Set cell position to keep the grid central
                float xPos = (x - (gridSizeX-1)/2f) * spread;
                float yPos = 0;
                float zPos = (y - (gridSizeY-1)/2f) * spread;

                //set position
                cell.transform.position = new Vector3(xPos, yPos, zPos);
                
                cell.transform.position += transform.position;

                //organise into grid parent
                cell.transform.SetParent(gridParent);

                // Update the highest and lowest values
                lowestX = Mathf.Min(lowestX, xPos);
                highestX = Mathf.Max(highestX, xPos);
                lowestZ = Mathf.Min(lowestZ, zPos);
                highestZ = Mathf.Max(highestZ, zPos);

                //add cell to list
                cells.Add(cell);
                //AddBuilding(cell.transform.position, cell.transform);
            }
        }
    }

    void SetCityCentre()
    {
        float randomX = Random.Range(lowestX, highestX);
        float randomZ = Random.Range(lowestZ, highestZ);

        //divide z by 2 to ensure the centre isnt to close to the side
        cityCentre = new Vector3(randomX/2, 0, randomZ/2);
    }

    void AddBuildings()
    {
        foreach(GameObject build in cells)
        {
            // Array of allowed rotation angles
            float[] allowedAngles = { 0f, 90f, 180f, 270f };

            // Randomly select an angle from the array
            float randomAngle = allowedAngles[Random.Range(0, allowedAngles.Length)];

            //make building
            GameObject newBuilding = Instantiate(building[Random.Range(0, building.Length)], build.transform.position, Quaternion.Euler(0f, randomAngle, 0f), build.transform);

            // Set height of the building based on the distance from the city center
            Vector3 distanceFromCentre = build.transform.localPosition - cityCentre;
            float heightAdjustment = (-distanceFromCentre.magnitude) * heightAmplifier;

            if(heightAdjustment < -maxHeightReduction)
            {
                heightAdjustment = -maxHeightReduction;
            }

            //apply slight height variation
            heightAdjustment += Random.Range(-heightVariation, heightVariation);

            // Set the new y position of the building
            build.transform.position = new Vector3(build.transform.position.x, heightAdjustment, build.transform.position.z);
            float rotationOffset = Random.Range(-rotationVariability, rotationVariability);

            //randomise rotation slightly
            build.transform.eulerAngles = new Vector3(0f, transform.rotation.y - rotationOffset, 0f);

            //get meshes for colour change
            MeshRenderer[] meshes = newBuilding.GetComponent<Building>().GetMeshes();
            //get colour
            Color col = buildingColours[Random.Range(0, buildingColours.Length)];
            //apply colour
            foreach (MeshRenderer mesh in meshes)
            {
                mesh.material.color = col;
            }
        }
    }
}

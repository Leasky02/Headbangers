using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    //targets for camera
    [SerializeField] private Transform[] targets;

    //velocity of camera movement
    private Vector3 velocity;
    private float smoothTime = 0.2f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 mapCenter;

    //zoom boundries
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float zoomLimiter;

    //camera component
    private Camera cam;

    //Start called on first frame
    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    //called after every update
    private void FixedUpdate()
    {
        //if no target, return
        if (targets.Length == 0)
        {
            return;
        }

        //adjust camera position and zoom
        Move();
        Zoom();
    }

    private void Zoom()
    {
        //camera zoom caluclation
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        //smoothly zoom camera
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    private float GetGreatestDistance()
    {
        //calculate width of encapsulated targets

        //set bounds default to spawn point
        Bounds bounds = new Bounds(mapCenter, Vector3.zero);
        bool boundsSet = false;

        foreach (Transform player in targets)
        {
            //if camera is within playzone
            bool isCameraTarget = player.transform.parent.GetComponent<PlayerData>().GetIsCameraTarget();
            if (isCameraTarget)
            {
                if (!boundsSet)
                {
                    bounds = new Bounds(player.position, Vector3.zero);
                    boundsSet = true;
                }
                bounds.Encapsulate(player.position);
            }
        }

        if (!boundsSet)
        {
            bounds = new Bounds(targets[0].position, Vector3.zero);
        }

        return bounds.size.magnitude;
    }

    private void Move()
    {
        //center point calculated in function which takes all players and gets center point
        Vector3 centerPoint = GetCenterPoint();
        //take into account the offset from the center point
        Vector3 newPosition = centerPoint + offset;
        //set camera position to the new position
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private Vector3 GetCenterPoint()
    {
        //set bounds default to spawn point
        Bounds bounds = new Bounds(mapCenter, Vector3.zero);
        bool boundsSet = false;

        foreach (Transform player in targets)
        {
            //if camera is within playzone
            bool isCameraTarget = player.transform.parent.GetComponent<PlayerData>().GetIsCameraTarget();
            if (isCameraTarget)
            {
                if (!boundsSet)
                {
                    bounds = new Bounds(player.position, Vector3.zero);
                    boundsSet = true;
                }
                bounds.Encapsulate(player.position);
            }
        }
        //return center point of bounds
        return bounds.center;
    }
}
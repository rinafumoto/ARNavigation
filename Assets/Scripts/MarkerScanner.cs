using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MarkerScanner : MonoBehaviour
{
    public Text text;
    public GameObject dropdown;
    public GameObject minimap;
    public GameObject minimapCamera;
    public GameObject ARCamera;
    public GameObject map;
    private GameObject marker;
    private bool scanned;
    public GameObject anchor;
    //private NavMeshPath navmesh;
    //public GameObject path;
    //private LineRenderer line;
    //public GameObject pointer;



    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;

    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            // Handle added event
            if (text.text == "Please scan a marker")
            {
                text.text = "";
                dropdown.SetActive(true);
                minimap.SetActive(true);
            }
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            if (updatedImage.trackingState != TrackingState.Tracking)
            {
                scanned = false;
            }
                // Handle updated event
            if (updatedImage.trackingState == TrackingState.Tracking && !scanned)
            {
                scanned = true;
                marker = GameObject.Find(updatedImage.referenceImage.name);
                minimapCamera.transform.position = marker.transform.position;
                anchor.transform.position = ARCamera.transform.position;
                anchor.transform.eulerAngles = ARCamera.transform.eulerAngles + new Vector3(0, -marker.transform.eulerAngles.y, 0);
            }
        }

        foreach (var removedImage in eventArgs.removed)
        {
            // Handle removed event
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public GameObject anchor;
    public GameObject pointer;
    private GameObject marker;
    private bool scanned;



    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;

    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
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
            if (updatedImage.trackingState == TrackingState.Tracking && !scanned)
            {
                scanned = true;
                marker = GameObject.Find(updatedImage.referenceImage.name);
                minimapCamera.transform.position = marker.transform.position;
                pointer.transform.position = new Vector3(pointer.transform.position.x, 0, pointer.transform.position.z);
                anchor.transform.position = ARCamera.transform.position;
                anchor.transform.eulerAngles = ARCamera.transform.eulerAngles + new Vector3(0, -marker.transform.eulerAngles.y, 0);
            }
        }
    }

}

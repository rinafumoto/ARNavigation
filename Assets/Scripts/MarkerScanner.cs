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
    public GameObject bee;
    public GameObject butterfly;
    public GameObject daisy;
    public GameObject map;
    //public GameObject ARSessionOrigin;
    //public ARSession ARSession;
    public bool scanned = false;
    public GameObject anchor;


    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;

    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            // Handle added event
            text.text = "";
            dropdown.SetActive(true);
            minimap.SetActive(true);
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
                if (updatedImage.referenceImage.name == "bee")
                {
                    map.transform.eulerAngles = new Vector3(0, map.transform.eulerAngles.y - bee.transform.eulerAngles.y, 0);
                    minimapCamera.transform.position = bee.transform.position;
                    anchor.transform.position = ARCamera.transform.position;
                    anchor.transform.rotation = ARCamera.transform.rotation;
                }
                if (updatedImage.referenceImage.name == "butterfly")
                {
                    map.transform.eulerAngles = new Vector3(0, map.transform.eulerAngles.y - butterfly.transform.eulerAngles.y, 0);
                    minimapCamera.transform.position = butterfly.transform.position;
                    anchor.transform.position = ARCamera.transform.position;
                    anchor.transform.rotation = ARCamera.transform.rotation;
                }
                if (updatedImage.referenceImage.name == "daisy")
                {
                    map.transform.eulerAngles = new Vector3(0, map.transform.eulerAngles.y - daisy.transform.eulerAngles.y, 0);
                    minimapCamera.transform.position = daisy.transform.position;
                    anchor.transform.position = ARCamera.transform.position;
                    anchor.transform.rotation = ARCamera.transform.rotation;
                }
            }
        }

        foreach (var removedImage in eventArgs.removed)
        {
            // Handle removed event
        }
    }
}

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
            // Handle updated event
            if(updatedImage.trackingState == TrackingState.Tracking)
            {
                if (updatedImage.referenceImage.name == "bee")
                {
                    minimapCamera.transform.position = new Vector3(-110, 5, -1.3f);
                    minimapCamera.transform.rotation = Quaternion.Euler(90, 0, 180);
                }
                if (updatedImage.referenceImage.name == "butterfly")
                {
                    minimapCamera.transform.position = new Vector3(-107.5f, 5, -0.6f);
                    minimapCamera.transform.rotation = Quaternion.Euler(90, 0, -90);
                }
                if (updatedImage.referenceImage.name == "daisy")
                {
                    minimapCamera.transform.position = new Vector3(-107.5f, 5, 0.4f);
                    minimapCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
                }
            }
        }

        foreach (var removedImage in eventArgs.removed)
        {
            // Handle removed event
        }
    }
}

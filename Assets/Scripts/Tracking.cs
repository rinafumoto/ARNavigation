using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tracking : MonoBehaviour
{
    public GameObject minimapCamera;
    public GameObject ARCamera;
    public GameObject ARSessionOrigin;
    public Vector3 prevPosition;
    public Vector3 currPosition;
    public Text text;
    public GameObject anchor;

    // Start is called before the first frame update
    void Start()
    {
        prevPosition = anchor.transform.InverseTransformPoint(ARCamera.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        currPosition = anchor.transform.InverseTransformPoint(ARCamera.transform.position);
        Vector3 diffPosition = currPosition - prevPosition;
        diffPosition.y = 0.0f;
        minimapCamera.transform.position = minimapCamera.transform.position + diffPosition;
        prevPosition = currPosition;
    }
}

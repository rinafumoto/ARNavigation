using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Tracking : MonoBehaviour
{
    public GameObject minimapCamera;
    public GameObject ARCamera;
    public Vector3 prevPosition;
    public Vector3 currPosition;
    public Text text;
    public GameObject anchor;
    private NavMeshPath navmesh;
    public GameObject path;
    private LineRenderer line;
    public GameObject dropdown;
    public GameObject pointer;
    private GameObject dest;
    public GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        prevPosition = anchor.transform.InverseTransformPoint(ARCamera.transform.position);
        navmesh = new NavMeshPath();
        line = path.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        currPosition = anchor.transform.InverseTransformPoint(ARCamera.transform.position);
        Vector3 diffPosition = currPosition - prevPosition;
        diffPosition.y = 0.0f;
        minimapCamera.transform.position = minimapCamera.transform.position + diffPosition;
        prevPosition = currPosition;
        Quaternion diffrot = ARCamera.transform.rotation * Quaternion.Inverse(anchor.transform.rotation);
        minimapCamera.transform.eulerAngles = new Vector3(90, diffrot.eulerAngles.y, 0);
        if (dropdown.GetComponent<Dropdown>().value != 0)
        {
            dest = GameObject.Find(dropdown.GetComponent<Dropdown>().captionText.text);
            NavMesh.CalculatePath(pointer.transform.position, dest.transform.position, NavMesh.AllAreas, navmesh);
            line.positionCount = navmesh.corners.Length;
            line.SetPositions(navmesh.corners);
            line.enabled = true;
            arrow.SetActive(true);
            arrow.transform.position = ARCamera.transform.position + ARCamera.transform.forward * 3 - ARCamera.transform.up;
        }
    }
}

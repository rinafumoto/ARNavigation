using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Tracking : MonoBehaviour
{
    public GameObject minimapCamera;
    public GameObject ARCamera;
    private Vector3 prevPosition;
    private Vector3 currPosition;
    public Text text;
    public GameObject anchor;
    private NavMeshPath navmesh;
    public GameObject path;
    private LineRenderer line;
    public Dropdown dropdown;
    public GameObject pointer;
    private GameObject dest;
    public GameObject arrow;
    private Vector3 direction;
    public bool selected;
    public GameObject dest_point;

    // Start is called before the first frame update
    void Start()
    {
        prevPosition = anchor.transform.InverseTransformPoint(ARCamera.transform.position);
        navmesh = new NavMeshPath();
        line = path.GetComponent<LineRenderer>();
        selected = false;
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
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
        if (selected)
        {
            NavMesh.CalculatePath(pointer.transform.position, dest.transform.position, NavMesh.AllAreas, navmesh);
            line.positionCount = navmesh.corners.Length;
            line.SetPositions(navmesh.corners);
            line.enabled = true;
            direction = line.GetPosition(1) - pointer.transform.position;
            arrow.transform.eulerAngles = new Vector3(0, ARCamera.transform.eulerAngles.y + Vector3.SignedAngle(minimapCamera.transform.up, direction, Vector3.up), 0);
            pointer.transform.eulerAngles = new Vector3(0, minimapCamera.transform.eulerAngles.y, 0);
            Vector3 dest_pos = pointer.transform.InverseTransformPoint(dest.transform.position);
            GameObject camera_pos = new GameObject();
            camera_pos.transform.position = ARCamera.transform.position;
            camera_pos.transform.eulerAngles = new Vector3(0, ARCamera.transform.eulerAngles.y, 0);
            dest_point.transform.position = camera_pos.transform.TransformPoint(dest_pos);
        }
    }

    void DropdownValueChanged(Dropdown change)
    {
        if (!selected && change.value != 0)
        {
            dropdown.options.RemoveAt(0);
            arrow.SetActive(true);
            dest_point.SetActive(true);
            selected = true;
        }
        dest = GameObject.Find(dropdown.captionText.text);
    }
}

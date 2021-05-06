using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Tracking : MonoBehaviour
{
    public GameObject minimapCamera;
    public GameObject ARCamera;
    public GameObject anchor;
    public GameObject path;
    public GameObject pointer;
    public GameObject arrow;
    public GameObject dest_point;
    public Dropdown dropdown;
    private GameObject dest;
    private GameObject camera_pos;
    private Vector3 prevPosition;
    private Vector3 currPosition;
    private Vector3 direction;
    private Vector3 diffPosition;
    private Vector3 dest_pos;
    private NavMeshPath navmesh;
    private LineRenderer line;
    private Quaternion diffrot;
    private bool selected;
    public Text text;


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
        List<string> options = new List<string>();
        int skip = 0;
        if (!selected)
        {
            skip = 1;
            options.Add(dropdown.options[0].text);
        }
        for (int i = skip; i < dropdown.options.Count; ++i)
        {
            GameObject destination = GameObject.Find(dropdown.options[i].text.Split(' ')[0]);
            NavMesh.CalculatePath(pointer.transform.position, destination.transform.position, NavMesh.AllAreas, navmesh);
            float dist = 0.0f;
            for (int j = 1; j < navmesh.corners.Length; ++j)
            {
                dist += Vector3.Distance(navmesh.corners[j - 1], navmesh.corners[j]);
            }
            options.Add(dropdown.options[i].text.Split(' ')[0] + " (" + Math.Round(dist,2) + "m)");
        }
        dropdown.options.Clear();
        dropdown.AddOptions(options);
        currPosition = anchor.transform.InverseTransformPoint(ARCamera.transform.position);
        diffPosition = currPosition - prevPosition;
        diffPosition.y = 0.0f;
        minimapCamera.transform.position = minimapCamera.transform.position + diffPosition;
        prevPosition = currPosition;
        diffrot = ARCamera.transform.rotation * Quaternion.Inverse(anchor.transform.rotation);
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
            dest_pos = pointer.transform.InverseTransformPoint(dest.transform.position);
            camera_pos = new GameObject();
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
        dest = GameObject.Find(dropdown.captionText.text.Split(' ')[0]);
    }
}

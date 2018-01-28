using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentStart : MonoBehaviour
{   
    public StartNode[] StartNodes;

    [SerializeField]
    private int _segmentLength;
    public int SegmentLength { get { return _segmentLength; } }

    [SerializeField]
    private Material _laneMaterial;
    [SerializeField]
    private Material _harmfulLaneMaterial;

    /// <summary> InterSegment is an segment between "real" segments, used to make sure he map is playable: </summary>
    private bool _isInterSegment = false;
    public bool IsInterSegment { get { return _isInterSegment; } set { _isInterSegment = value; } }

    public void GenerateConnections(Material _harmfulMaterial, Material _laneMaterial)
    {
        //Debug.Log("Generating Connections");
        // iterate over all nodes and assign startnodes
        for(int i = 0; i < transform.childCount; i++)
        {
            StartNode start = transform.GetChild(i).GetComponentInChildren<StartNode>();
            if (start != null)
            {
                if(Vector3.Distance(start.getPosition(), new Vector3(-1, 0, transform.position.x)) < 0.001)
                {
                    StartNodes[0] = start;
                }
                if (Vector3.Distance(start.getPosition(), new Vector3(0, 0, transform.position.x)) < 0.001)
                {
                    StartNodes[1] = start;
                }
                if (Vector3.Distance(start.getPosition(), new Vector3(1, 0, transform.position.x)) < 0.001)
                {
                    StartNodes[2] = start;
                }
            }

            SegmentEnd segmentEnd = transform.GetComponent<SegmentEnd>();

            EndNode end = transform.GetChild(i).GetComponentInChildren<EndNode>();
            if (end != null)
            {
                end.outgoingNodes.Clear();
                for( int j = 0; j < transform.childCount; j++)
                {
                    if (j == i) continue;
                    StartNode nextNode = transform.GetChild(j).GetComponentInChildren<StartNode>();
                    if(nextNode != null && Vector3.Distance(end.getPosition(), nextNode.getPosition()) < 0.001)
                    {
                        end.outgoingNodes.Add(nextNode);
                    }
                }



                if (Vector3.Distance(end.getPosition(), new Vector3(-1, 0, segmentEnd.transform.position.x)) < 0.001)
                {
                    segmentEnd.EndNodes[0] = end;
                }
                if (Vector3.Distance(end.getPosition(), new Vector3(0, 0, segmentEnd.transform.position.x)) < 0.001)
                {
                    segmentEnd.EndNodes[1] = end;
                }
                if (Vector3.Distance(end.getPosition(), new Vector3(1, 0, segmentEnd.transform.position.x)) < 0.001)
                {
                    segmentEnd.EndNodes[2] = end;
                }
            }
        }


        var startNodes = gameObject.GetComponentsInChildren<StartNode>();

        foreach (var node in startNodes)
        {
            if(node.IsHarmful == true)
            {
                node.GenerateLane(_harmfulMaterial);
            }
            else
            {
                node.GenerateLane(_laneMaterial);
            }
        }
    }

    public void GenerateConnections()
    {
        //Debug.Log("Generating Connections");
        // iterate over all nodes and assign startnodes
        for (int i = 0; i < transform.childCount; i++)
        {
            StartNode start = transform.GetChild(i).GetComponentInChildren<StartNode>();
            if (start != null)
            {
                if (Vector3.Distance(start.getPosition(), new Vector3(-1, 0, transform.position.x)) < 0.001)
                {
                    StartNodes[0] = start;
                }
                if (Vector3.Distance(start.getPosition(), new Vector3(0, 0, transform.position.x)) < 0.001)
                {
                    StartNodes[1] = start;
                }
                if (Vector3.Distance(start.getPosition(), new Vector3(1, 0, transform.position.x)) < 0.001)
                {
                    StartNodes[2] = start;
                }
            }

            SegmentEnd segmentEnd = transform.GetComponent<SegmentEnd>();

            EndNode end = transform.GetChild(i).GetComponentInChildren<EndNode>();
            if (end != null)
            {
                end.outgoingNodes.Clear();
                for (int j = 0; j < transform.childCount; j++)
                {
                    if (j == i) continue;
                    StartNode nextNode = transform.GetChild(j).GetComponentInChildren<StartNode>();
                    if (nextNode != null && Vector3.Distance(end.getPosition(), nextNode.getPosition()) < 0.001)
                    {
                        end.outgoingNodes.Add(nextNode);
                    }
                }



                if (Vector3.Distance(end.getPosition(), new Vector3(-1, 0, segmentEnd.transform.position.x)) < 0.001)
                {
                    segmentEnd.EndNodes[0] = end;
                }
                if (Vector3.Distance(end.getPosition(), new Vector3(0, 0, segmentEnd.transform.position.x)) < 0.001)
                {
                    segmentEnd.EndNodes[1] = end;
                }
                if (Vector3.Distance(end.getPosition(), new Vector3(1, 0, segmentEnd.transform.position.x)) < 0.001)
                {
                    segmentEnd.EndNodes[2] = end;
                }
            }
        }


        var startNodes = gameObject.GetComponentsInChildren<StartNode>();

        foreach (var node in startNodes)
        {
            if (node.IsHarmful == true)
            {
                node.GenerateLane(_harmfulLaneMaterial);
            }
            else
            {
                node.GenerateLane(_laneMaterial);
            }
        }
    }
}

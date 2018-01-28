using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNode : MonoBehaviour
{
    [SerializeField]
    private EndNode _endNode;

    public bool IsHarmful;

    public void GenerateLane(Material material)
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();

        if(!lineRenderer)
            lineRenderer = gameObject.AddComponent<LineRenderer>();


        lineRenderer.material = material;
        lineRenderer.alignment = LineAlignment.Local;
        lineRenderer.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);

        if (IsHarmful == false)
        {
            lineRenderer.widthMultiplier = 0.15f;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, getPosition());
            lineRenderer.SetPosition(1, GetEndNode().getPosition());
        }
        else
        {
            lineRenderer.widthMultiplier = 0.5f;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, getPosition());
            lineRenderer.SetPosition(1, GetEndNode().getPosition());
        }
    }

    public EndNode GetEndNode()
    {
        if(_endNode == null)
            _endNode = transform.parent.GetComponentInChildren<EndNode>();

        return _endNode;
    }

    public Vector3 getPosition()
    {
        return gameObject.transform.position;
    }

    private void OnDrawGizmos()
    {
        Vector3 endPos = Vector3.zero;

        if(GetEndNode())
            endPos = _endNode.transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, endPos);
    }

    
}

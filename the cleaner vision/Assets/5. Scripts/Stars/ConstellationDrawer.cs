using System.Collections.Generic;
using UnityEngine;

public class ConstellationDrawer : MonoBehaviour
{
    public List<Transform> stars;
    public LineRendererUI lineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<Vector3> starPositions = new List<Vector3>();
        foreach (var star in stars)
        {
            Vector3 worldPos = star.position;
            starPositions.Add(worldPos);
        }
        lineRenderer.CreateLine(starPositions[0], starPositions[1], Color.yellow);
        lineRenderer.CreateLine(starPositions[1], starPositions[2], Color.yellow);
    }
}


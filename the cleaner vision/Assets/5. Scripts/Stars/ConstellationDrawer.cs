using System.Collections.Generic;
using UnityEngine;

public class ConstellationDrawer : MonoBehaviour
{
    public List<Transform> stars;
    [SerializeField] private LineRendererUI linePrefab; 
    [SerializeField] private RectTransform container; //Parent, canvas or star background

    private List<LineRendererUI> activeLines = new List<LineRendererUI>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var line in activeLines)
        {
            Destroy(line.gameObject);
        }
        activeLines.Clear();

        for(int i = 0; i < stars.Count - 1; i++)
        {
            var line = Instantiate(linePrefab, container);
            line.transform.SetAsFirstSibling();
            line.CreateLine(stars[i].position, stars[i+1].position, Color.white);
            activeLines.Add(line);
        }
    }
}


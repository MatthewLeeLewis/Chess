using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This simple script is for grid debug objects that may be activated by uncommenting line 28 of the BoardGrid script.
public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;
    private GridObject gridObject;

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    private void Update()
    {
        textMeshPro.text = gridObject.ToString();
    }
}
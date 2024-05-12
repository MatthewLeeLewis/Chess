using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is the system used to generate usable grids based on entered parameters with useful built-in functionality..
 */

public class GridSystem 
{
    private int width;
    private int height;
    private float cellSize;

    private GridObject[,] gridObjectArray; // Defining a 2D array for storing the grid spaces as objects that store data.

    public GridSystem(int width, int height, float cellSize) // Constructor method to generate the grid using the entered values.
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new GridObject[width, height]; // Generate the array to store the objects that store the data within each grid position.

        for (int x = 0; x < width; x++) // Loop the x positions based on width
        {
            for (int z = 0; z < height; z++) // Loop the z positions based on height
            {
                GridPosition gridPosition = new GridPosition(x, z); // Identify this grid position as a GridPosition struct.
                gridObjectArray[x,z] = new GridObject(this, gridPosition); // Create a new Grid Object identifying itself as belonging to this grid system and this position in the grid for storing data, and allocate it properly into the array for future access.
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition) // Get the world position from grid coordinates.
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) // Get the grid position from world coordinates.
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
        );
    }

    public void CreateDebugObjects(Transform debugPrefab) // Public debug method to preview grid
    {
        for (int x = 0; x < width; x++) // Loop the x positions based on width
        {
            for (int z = 0; z < height; z++) // Loop the z positions based on height
            {
                GridPosition gridPosition = new GridPosition(x, z); // Identify the x and z as a GridPosition struct.

                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity); // Use GameObject to instantiate since MonoBehavior isn't used.
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>(); // Create the debug object in the coorect position.
                gridDebugObject.SetGridObject(GetGridObject(gridPosition)); // Set it as a Grid Object
            }
        }
    }

    public GridObject GetGridObject(GridPosition gridPosition) // Gets a grid object based on coordinates.
    {
        return gridObjectArray[gridPosition.x, gridPosition.z]; // return the grid object based on the position within the array.
    }

    public bool IsValidGridPosition(GridPosition gridPosition) // Return if grid position is on the grid (true) or out of bounds (false)
    {
        return gridPosition.x >= 0 && 
               gridPosition.z >= 0 && 
               gridPosition.x < width && 
               gridPosition.z < height;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
}

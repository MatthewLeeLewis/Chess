using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script keeps track of the mouse cursor's position within the worldspace of the game.
 */

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance; // Static instance of this script
    [SerializeField] private LayerMask mousePlaneLayerMask; // Variable to identify the mouse world layer.

    private void Awake()
    {
        instance = this; // Instantiate the static instance
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Create a ray to determine the mouse's position within 3d space.
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask); // Identify where in the mouse world plane contact is occuring...
        return raycastHit.point; // Return that point.
    }
}
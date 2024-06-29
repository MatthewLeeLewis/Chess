using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBox : MonoBehaviour
{
    // The test box script is made explicitly for collision detection for rays in the case of hypothetical moves for selected pieces.
    private void Start()
    {
        PieceControlSystem.TestBoxDestroy += TestBoxDestroy;
        AI_system.TestBoxDestroy += TestBoxDestroy;
    }

    public void Move(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    private void TestBoxDestroy(object sender, EventArgs e)
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        PieceControlSystem.TestBoxDestroy -= TestBoxDestroy;
        AI_system.TestBoxDestroy -= TestBoxDestroy;
    }
}

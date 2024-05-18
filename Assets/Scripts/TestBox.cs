using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBox : MonoBehaviour
{
    private void Start()
    {
        PieceControlSystem.TestBoxDestroy += TestBoxDestroy;
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
    }
}

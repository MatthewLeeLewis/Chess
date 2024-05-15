using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBox : MonoBehaviour
{
    void Start()
    {
        PawnAction.TestBoxDestroy += TestBoxDestroy;
        KnightAction.TestBoxDestroy += TestBoxDestroy;
        PieceControlSystem.TestBoxDestroy += TestBoxDestroy;
    }

    private void TestBoxDestroy(object sender, EventArgs e)
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        PawnAction.TestBoxDestroy -= TestBoxDestroy;
        KnightAction.TestBoxDestroy -= TestBoxDestroy;
        PieceControlSystem.TestBoxDestroy -= TestBoxDestroy;
    }
}

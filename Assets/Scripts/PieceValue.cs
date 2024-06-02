using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceValue : MonoBehaviour
{
    private Piece piece;

    private void Awake()
    {
        piece = GetComponent<Piece>();
    }

    public float GetPower(GridPosition gridPosition)
    {
        switch (piece.GetPieceType())
        {
            case "King":
                {
                    return piece.GetRelativePower() + GetKingMod(gridPosition);
                }
            case "Queen":
                {
                    return piece.GetRelativePower() + GetQueenMod(gridPosition);
                }
            case "Rook":
                {
                    return piece.GetRelativePower() + GetRookMod(gridPosition);
                }
            case "Bishop":
                {
                    return piece.GetRelativePower() + GetBishopMod(gridPosition);
                }
            case "Knight":
                {
                    return piece.GetRelativePower() + GetKnightMod(gridPosition);
                }
            case "Pawn":
                {
                    return piece.GetRelativePower() + GetPawnMod(gridPosition);
                }
        }
        return 0f;
    }


    public float GetPawnMod(GridPosition gridPosition)
    {
        switch (gridPosition.z)
        {
            case 0:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return 0f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 0f;
                            }
                        case 3:
                            {
                                return 0f;
                            }
                        case 4:
                            {
                                return 0f;
                            }
                        case 5:
                            {
                                return 0f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return 0f;
                            }
                    }
                    break;
                }
            case 1:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return 0.5f;
                            }
                        case 1:
                            {
                                return 1f;
                            }
                        case 2:
                            {
                                return 1f;
                            }
                        case 3:
                            {
                                return -2f;
                            }
                        case 4:
                            {
                                return -2f;
                            }
                        case 5:
                            {
                                return 1f;
                            }
                        case 6:
                            {
                                return 1f;
                            }
                        case 7:
                            {
                                return 0.5f;
                            }
                    }
                    break;
                }
            case 2:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return 0.5f;
                            }
                        case 1:
                            {
                                return -0.5f;
                            }
                        case 2:
                            {
                                return -1f;
                            }
                        case 3:
                            {
                                return 0f;
                            }
                        case 4:
                            {
                                return 0f;
                            }
                        case 5:
                            {
                                return -1f;
                            }
                        case 6:
                            {
                                return -0.5f;
                            }
                        case 7:
                            {
                                return 0.5f;
                            }
                    }
                    break;
                }
            case 3:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return 0f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 0f;
                            }
                        case 3:
                            {
                                return 2f;
                            }
                        case 4:
                            {
                                return 2f;
                            }
                        case 5:
                            {
                                return 0f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return 0f;
                            }
                    }
                    break;
                }
            case 4:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return 0.5f;
                            }
                        case 1:
                            {
                                return 0.5f;
                            }
                        case 2:
                            {
                                return 1f;
                            }
                        case 3:
                            {
                                return 2.5f;
                            }
                        case 4:
                            {
                                return 2.5f;
                            }
                        case 5:
                            {
                                return 1f;
                            }
                        case 6:
                            {
                                return 0.5f;
                            }
                        case 7:
                            {
                                return 0.5f;
                            }
                    }
                    break;
                }
            case 5:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return 1f;
                            }
                        case 1:
                            {
                                return 1f;
                            }
                        case 2:
                            {
                                return 2f;
                            }
                        case 3:
                            {
                                return 3f;
                            }
                        case 4:
                            {
                                return 3f;
                            }
                        case 5:
                            {
                                return 2f;
                            }
                        case 6:
                            {
                                return 1f;
                            }
                        case 7:
                            {
                                return 1f;
                            }
                    }
                    break;
                }
            case 6:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return 5f;
                            }
                        case 1:
                            {
                                return 5f;
                            }
                        case 2:
                            {
                                return 5f;
                            }
                        case 3:
                            {
                                return 5f;
                            }
                        case 4:
                            {
                                return 5f;
                            }
                        case 5:
                            {
                                return 5f;
                            }
                        case 6:
                            {
                                return 5f;
                            }
                        case 7:
                            {
                                return 5f;
                            }
                    }
                    break;
                }
            case 7:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return 0f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 0f;
                            }
                        case 3:
                            {
                                return 0f;
                            }
                        case 4:
                            {
                                return 0f;
                            }
                        case 5:
                            {
                                return 0f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return 0f;
                            }
                    }
                    break;
                }

        }
        return 0f;
    }


    public float GetQueenMod(GridPosition gridPosition)
    {
        switch (gridPosition.z)
        {
            case 0:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -2f;
                            }
                        case 1:
                            {
                                return -1f;
                            }
                        case 2:
                            {
                                return -1f;
                            }
                        case 3:
                            {
                                return -0.5f;
                            }
                        case 4:
                            {
                                return -0.5f;
                            }
                        case 5:
                            {
                                return -1f;
                            }
                        case 6:
                            {
                                return -1f;
                            }
                        case 7:
                            {
                                return -2f;
                            }
                    }
                    break;
                }
            case 1:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -1f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 0.5f;
                            }
                        case 3:
                            {
                                return 0f;
                            }
                        case 4:
                            {
                                return 0f;
                            }
                        case 5:
                            {
                                return 0f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return -1f;
                            }
                    }
                    break;
                }
            case 2:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -1f;
                            }
                        case 1:
                            {
                                return 0.5f;
                            }
                        case 2:
                            {
                                return 0.5f;
                            }
                        case 3:
                            {
                                return 0.5f;
                            }
                        case 4:
                            {
                                return 0.5f;
                            }
                        case 5:
                            {
                                return 0.5f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return -1f;
                            }
                    }
                    break;
                }
            case 3:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return 0f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 0.5f;
                            }
                        case 3:
                            {
                                return 0.5f;
                            }
                        case 4:
                            {
                                return 0.5f;
                            }
                        case 5:
                            {
                                return 0.5f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return -0.5f;
                            }
                    }
                    break;
                }
            case 4:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -0.5f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 0.5f;
                            }
                        case 3:
                            {
                                return 0.5f;
                            }
                        case 4:
                            {
                                return 0.5f;
                            }
                        case 5:
                            {
                                return 0.5f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return -0.5f;
                            }
                    }
                    break;
                }
            case 5:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -1f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 0.5f;
                            }
                        case 3:
                            {
                                return 0.5f;
                            }
                        case 4:
                            {
                                return 0.5f;
                            }
                        case 5:
                            {
                                return 0.5f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return -1f;
                            }
                    }
                    break;
                }
            case 6:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -1f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 0f;
                            }
                        case 3:
                            {
                                return 0f;
                            }
                        case 4:
                            {
                                return 0f;
                            }
                        case 5:
                            {
                                return 0f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return -1f;
                            }
                    }
                    break;
                }
            case 7:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -2f;
                            }
                        case 1:
                            {
                                return -1f;
                            }
                        case 2:
                            {
                                return -1f;
                            }
                        case 3:
                            {
                                return -0.5f;
                            }
                        case 4:
                            {
                                return -0.5f;
                            }
                        case 5:
                            {
                                return -1f;
                            }
                        case 6:
                            {
                                return -1f;
                            }
                        case 7:
                            {
                                return -2f;
                            }
                    }
                }
                break;
        }
        return 0f;
    }

    public float GetKingMod(GridPosition gridPosition)
    {
        switch (gridPosition.z)
        {
            case 0:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return 2f;
                            }
                        case 1:
                            {
                                return 3f;
                            }
                        case 2:
                            {
                                return 1f;
                            }
                        case 3:
                            {
                                return 0f;
                            }
                        case 4:
                            {
                                return 0f;
                            }
                        case 5:
                            {
                                return 1f;
                            }
                        case 6:
                            {
                                return 3f;
                            }
                        case 7:
                            {
                                return 2f;
                            }
                    }
                    break;
                }
            case 1:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return 2f;
                            }
                        case 1:
                            {
                                return 2f;
                            }
                        case 2:
                            {
                                return 0f;
                            }
                        case 3:
                            {
                                return 0f;
                            }
                        case 4:
                            {
                                return 0f;
                            }
                        case 5:
                            {
                                return 0f;
                            }
                        case 6:
                            {
                                return 2f;
                            }
                        case 7:
                            {
                                return 2f;
                            }
                    }
                    break;
                }
            case 2:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -1f;
                            }
                        case 1:
                            {
                                return -2f;
                            }
                        case 2:
                            {
                                return -2f;
                            }
                        case 3:
                            {
                                return -2f;
                            }
                        case 4:
                            {
                                return -2f;
                            }
                        case 5:
                            {
                                return -2f;
                            }
                        case 6:
                            {
                                return -2f;
                            }
                        case 7:
                            {
                                return -1f;
                            }
                    }
                    break;
                }
            case 3:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -2f;
                            }
                        case 1:
                            {
                                return -3f;
                            }
                        case 2:
                            {
                                return -3f;
                            }
                        case 3:
                            {
                                return -4f;
                            }
                        case 4:
                            {
                                return -4f;
                            }
                        case 5:
                            {
                                return -3f;
                            }
                        case 6:
                            {
                                return -3f;
                            }
                        case 7:
                            {
                                return -2f;
                            }
                    }
                    break;
                }
            case 4:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -3f;
                            }
                        case 1:
                            {
                                return -4f;
                            }
                        case 2:
                            {
                                return -4f;
                            }
                        case 3:
                            {
                                return -5f;
                            }
                        case 4:
                            {
                                return -5f;
                            }
                        case 5:
                            {
                                return -4f;
                            }
                        case 6:
                            {
                                return -4f;
                            }
                        case 7:
                            {
                                return -3f;
                            }
                    }
                    break;
                }
            case 5:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -3f;
                            }
                        case 1:
                            {
                                return -4f;
                            }
                        case 2:
                            {
                                return -4f;
                            }
                        case 3:
                            {
                                return -5f;
                            }
                        case 4:
                            {
                                return -5f;
                            }
                        case 5:
                            {
                                return -4f;
                            }
                        case 6:
                            {
                                return -4f;
                            }
                        case 7:
                            {
                                return -3f;
                            }
                    }
                    break;
                }
            case 6:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -3f;
                            }
                        case 1:
                            {
                                return -4f;
                            }
                        case 2:
                            {
                                return -4f;
                            }
                        case 3:
                            {
                                return -5f;
                            }
                        case 4:
                            {
                                return -5f;
                            }
                        case 5:
                            {
                                return -4f;
                            }
                        case 6:
                            {
                                return -4f;
                            }
                        case 7:
                            {
                                return -3f;
                            }
                    }
                    break;
                }
            case 7:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -3f;
                            }
                        case 1:
                            {
                                return -4f;
                            }
                        case 2:
                            {
                                return -4f;
                            }
                        case 3:
                            {
                                return -5f;
                            }
                        case 4:
                            {
                                return -5f;
                            }
                        case 5:
                            {
                                return -4f;
                            }
                        case 6:
                            {
                                return -4f;
                            }
                        case 7:
                            {
                                return -3f;
                            }
                    }
                    break;
                }
        }
        return 0f;
    }

    public float GetBishopMod(GridPosition gridPosition)
    {
        switch (gridPosition.z)
        {
            case 0:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -2f;
                            }
                        case 1:
                            {
                                return -1f;
                            }
                        case 2:
                            {
                                return -1f;
                            }
                        case 3:
                            {
                                return -1f;
                            }
                        case 4:
                            {
                                return -1f;
                            }
                        case 5:
                            {
                                return -1f;
                            }
                        case 6:
                            {
                                return -1f;
                            }
                        case 7:
                            {
                                return -2f;
                            }
                    }
                    break;
                }
            case 1:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -1f;
                            }
                        case 1:
                            {
                                return 0.5f;
                            }
                        case 2:
                            {
                                return 0f;
                            }
                        case 3:
                            {
                                return 0f;
                            }
                        case 4:
                            {
                                return 0f;
                            }
                        case 5:
                            {
                                return 0f;
                            }
                        case 6:
                            {
                                return 0.5f;
                            }
                        case 7:
                            {
                                return -1f;
                            }
                    }
                    break;
                }
            case 2:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -1f;
                            }
                        case 1:
                            {
                                return 1f;
                            }
                        case 2:
                            {
                                return 1f;
                            }
                        case 3:
                            {
                                return 1f;
                            }
                        case 4:
                            {
                                return 1f;
                            }
                        case 5:
                            {
                                return 1f;
                            }
                        case 6:
                            {
                                return 1f;
                            }
                        case 7:
                            {
                                return -1f;
                            }
                    }
                    break;
                }
            case 3:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -1f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 1f;
                            }
                        case 3:
                            {
                                return 1f;
                            }
                        case 4:
                            {
                                return 1f;
                            }
                        case 5:
                            {
                                return 1f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return -1f;
                            }
                    }
                    break;
                }
            case 4:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -1f;
                            }
                        case 1:
                            {
                                return 0.5f;
                            }
                        case 2:
                            {
                                return 0.5f;
                            }
                        case 3:
                            {
                                return 1f;
                            }
                        case 4:
                            {
                                return 1f;
                            }
                        case 5:
                            {
                                return 0.5f;
                            }
                        case 6:
                            {
                                return 0.5f;
                            }
                        case 7:
                            {
                                return -1f;
                            }
                    }
                    break;
                }
            case 5:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -1;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 0.5f;
                            }
                        case 3:
                            {
                                return 1f;
                            }
                        case 4:
                            {
                                return 1f;
                            }
                        case 5:
                            {
                                return 0.5f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return -1f;
                            }
                    }
                    break;
                }
            case 6:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -1f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 0f;
                            }
                        case 3:
                            {
                                return 0f;
                            }
                        case 4:
                            {
                                return 0f;
                            }
                        case 5:
                            {
                                return 0f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return -1f;
                            }
                    }
                    break;
                }
            case 7:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -2f;
                            }
                        case 1:
                            {
                                return -1f;
                            }
                        case 2:
                            {
                                return -1f;
                            }
                        case 3:
                            {
                                return -1f;
                            }
                        case 4:
                            {
                                return -1f;
                            }
                        case 5:
                            {
                                return -1f;
                            }
                        case 6:
                            {
                                return -1f;
                            }
                        case 7:
                            {
                                return -2f;
                            }
                    }
                    break;
                }
        }
        return 0f;
    }

    public float GetRookMod(GridPosition gridPosition)
    {
        switch (gridPosition.z)
        {
            case 0:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return 0f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 0f;
                            }
                        case 3:
                            {
                                return 0.5f;
                            }
                        case 4:
                            {
                                return 0.5f;
                            }
                        case 5:
                            {
                                return 0f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return 0f;
                            }
                    }
                    break;
                }
            case 1:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -0.5f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 0f;
                            }
                        case 3:
                            {
                                return 0f;
                            }
                        case 4:
                            {
                                return 0f;
                            }
                        case 5:
                            {
                                return 0f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return -0.5f;
                            }
                    }
                    break;
                }
            case 2:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -0.5f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 0f;
                            }
                        case 3:
                            {
                                return 0f;
                            }
                        case 4:
                            {
                                return 0f;
                            }
                        case 5:
                            {
                                return 0f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return -0.5f;
                            }
                    }
                    break;
                }
            case 3:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -0.5f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 0f;
                            }
                        case 3:
                            {
                                return 0f;
                            }
                        case 4:
                            {
                                return 0f;
                            }
                        case 5:
                            {
                                return 0f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return -0.5f;
                            }
                    }
                    break;
                }
            case 4:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -0.5f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 0f;
                            }
                        case 3:
                            {
                                return 0f;
                            }
                        case 4:
                            {
                                return 0f;
                            }
                        case 5:
                            {
                                return 0f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return -0.5f;
                            }
                    }
                    break;
                }
            case 5:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -0.5f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 0f;
                            }
                        case 3:
                            {
                                return 0f;
                            }
                        case 4:
                            {
                                return 0f;
                            }
                        case 5:
                            {
                                return 0f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return -0.5f;
                            }
                    }
                    break;
                }
            case 6:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return 0.5f;
                            }
                        case 1:
                            {
                                return 1f;
                            }
                        case 2:
                            {
                                return 1f;
                            }
                        case 3:
                            {
                                return 1f;
                            }
                        case 4:
                            {
                                return 1f;
                            }
                        case 5:
                            {
                                return 1f;
                            }
                        case 6:
                            {
                                return 1f;
                            }
                        case 7:
                            {
                                return 0.5f;
                            }
                    }
                    break;
                }
            case 7:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return 0f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 0f;
                            }
                        case 3:
                            {
                                return 0f;
                            }
                        case 4:
                            {
                                return 0f;
                            }
                        case 5:
                            {
                                return 0f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return 0f;
                            }
                    }
                    break;
                }
        }
        return 0f;
    }

    public float GetKnightMod(GridPosition gridPosition)
    {
        switch (gridPosition.z)
        {
            case 0:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -5f;
                            }
                        case 1:
                            {
                                return -4f;
                            }
                        case 2:
                            {
                                return -3f;
                            }
                        case 3:
                            {
                                return -3f;
                            }
                        case 4:
                            {
                                return -3f;
                            }
                        case 5:
                            {
                                return -3f;
                            }
                        case 6:
                            {
                                return -4f;
                            }
                        case 7:
                            {
                                return -5f;
                            }
                    }
                    break;
                }
            case 1:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -4f;
                            }
                        case 1:
                            {
                                return -2f;
                            }
                        case 2:
                            {
                                return 0f;
                            }
                        case 3:
                            {
                                return 0.5f;
                            }
                        case 4:
                            {
                                return 0.5f;
                            }
                        case 5:
                            {
                                return 0f;
                            }
                        case 6:
                            {
                                return -2f;
                            }
                        case 7:
                            {
                                return -4f;
                            }
                    }
                    break;
                }
            case 2:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -3f;
                            }
                        case 1:
                            {
                                return 0.5f;
                            }
                        case 2:
                            {
                                return 1f;
                            }
                        case 3:
                            {
                                return 1.5f;
                            }
                        case 4:
                            {
                                return 1.5f;
                            }
                        case 5:
                            {
                                return 1f;
                            }
                        case 6:
                            {
                                return 0.5f;
                            }
                        case 7:
                            {
                                return -3f;
                            }
                    }
                    break;
                }
            case 3:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -3f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 1.5f;
                            }
                        case 3:
                            {
                                return 2f;
                            }
                        case 4:
                            {
                                return 2f;
                            }
                        case 5:
                            {
                                return 1.5f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return -3f;
                            }
                    }
                    break;
                }
            case 4:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -3f;
                            }
                        case 1:
                            {
                                return 0.5f;
                            }
                        case 2:
                            {
                                return 1.5f;
                            }
                        case 3:
                            {
                                return 2f;
                            }
                        case 4:
                            {
                                return 2f;
                            }
                        case 5:
                            {
                                return 1.5f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return -3f;
                            }
                    }
                    break;
                }
            case 5:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -3f;
                            }
                        case 1:
                            {
                                return 0f;
                            }
                        case 2:
                            {
                                return 1f;
                            }
                        case 3:
                            {
                                return 1.5f;
                            }
                        case 4:
                            {
                                return 1.5f;
                            }
                        case 5:
                            {
                                return 1f;
                            }
                        case 6:
                            {
                                return 0f;
                            }
                        case 7:
                            {
                                return -3f;
                            }
                    }
                    break;
                }
            case 6:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -4f;
                            }
                        case 1:
                            {
                                return -2f;
                            }
                        case 2:
                            {
                                return 0f;
                            }
                        case 3:
                            {
                                return 0f;
                            }
                        case 4:
                            {
                                return 0f;
                            }
                        case 5:
                            {
                                return 0f;
                            }
                        case 6:
                            {
                                return -2f;
                            }
                        case 7:
                            {
                                return -4f;
                            }
                    }
                    break;
                }
            case 7:
                {
                    switch (gridPosition.x)
                    {
                        case 0:
                            {
                                return -5f;
                            }
                        case 1:
                            {
                                return -4f;
                            }
                        case 2:
                            {
                                return -3f;
                            }
                        case 3:
                            {
                                return -3f;
                            }
                        case 4:
                            {
                                return -3f;
                            }
                        case 5:
                            {
                                return -3f;
                            }
                        case 6:
                            {
                                return -4f;
                            }
                        case 7:
                            {
                                return -5f;
                            }
                        
                    }
                    break;
                }
        }
        return 0f;
    }
}

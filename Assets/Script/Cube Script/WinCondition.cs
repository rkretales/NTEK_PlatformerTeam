using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    private CubeState cubeState;
    private Automate automate;
    private LongClickButton longClick;

    private void Start()
    {
        longClick = FindObjectOfType<LongClickButton>();
        cubeState = FindObjectOfType<CubeState>();
        automate = FindObjectOfType<Automate>();
    }

    private void Update()
    {
        if (CheckWinCondition() == true)
        {
            SceneManager.LoadScene("WinningScene");
        }
    }

    private Color GetColorOfPiece(GameObject piece)
    {
        Renderer renderer = piece.GetComponent<Renderer>();
        if (renderer != null && renderer.material != null)
        {
            return renderer.material.color;
        }

        // Return a default color (e.g., white) if the piece or its material is not found
        return Color.white;
    }

    private bool CheckWinCondition()
    {
        Color upColor = GetColorOfPiece(cubeState.up[4]);

        if (automate.shuffled && longClick.isPressed)
        {
            // Check if all pieces of the "up" side have the same color
            foreach (GameObject piece in cubeState.up)
            {
                if (GetColorOfPiece(piece) != upColor)
                {
                    return false;
                }
            }

            Color downColor = GetColorOfPiece(cubeState.down[4]);

            // Check if all pieces of the "down" side have the same color
            foreach (GameObject piece in cubeState.down)
            {
                if (GetColorOfPiece(piece) != downColor)
                {
                    return false;
                }
            }

            Color leftColor = GetColorOfPiece(cubeState.left[4]);

            // Check if all pieces of the "left" side have the same color
            foreach (GameObject piece in cubeState.left)
            {
                if (GetColorOfPiece(piece) != leftColor)
                {
                    return false;
                }
            }

            Color rightColor = GetColorOfPiece(cubeState.right[4]);

            // Check if all pieces of the "right" side have the same color
            foreach (GameObject piece in cubeState.right)
            {
                if (GetColorOfPiece(piece) != rightColor)
                {
                    return false;
                }
            }

            Color frontColor = GetColorOfPiece(cubeState.front[4]);

            // Check if all pieces of the "front" side have the same color
            foreach (GameObject piece in cubeState.front)
            {
                if (GetColorOfPiece(piece) != frontColor)
                {
                    return false;
                }
            }

            Color backColor = GetColorOfPiece(cubeState.back[4]);

            // Check if all pieces of the "back" side have the same color
            foreach (GameObject piece in cubeState.back)
            {
                if (GetColorOfPiece(piece) != backColor)
                {
                    return false;
                }
            }

            // If all sides have the same color, it's a win
            return true;
        }
        // Return false if the condition is not met
        return false;
    }
}

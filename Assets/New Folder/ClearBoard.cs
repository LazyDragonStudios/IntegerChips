using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearBoard : MonoBehaviour
{
    public void DeleteChips()
    {
        // Find all game objects with the "Chip" tag in the scene
        GameObject[] chips = GameObject.FindGameObjectsWithTag("Chip");

        // Loop through all chips and destroy them
        foreach (GameObject chip in chips)
        {
            Destroy(chip);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class FlagSpawner : MonoBehaviour
{
    public GameObject flagPrefab;     // Assign your flag prefab here
    public TMP_Text scoreText;        // Assign a TMP text element for score display

    private int score = 0;

    // Stores GPS coordinates where flags have been placed
    public List<Vector2> spawnedFlagPositions = new List<Vector2>();

    public void TryPlaceFlag()
    {
        float playerLat = LocationManager.Instance.Latitude;
        float playerLon = LocationManager.Instance.Longitude;

        // ✅ 1. Check if player is near a valid zone
        if (!LocationManager.Instance.IsPlayerNearValidZone(playerLat, playerLon, 1000f))
        {
            Debug.Log("You're not near a valid flag zone!");
            return;
        }

        // ✅ 2. Check if player already placed a flag in this area
        if (IsFlagAlreadyPlacedNearby(playerLat, playerLon, 1000f))
        {
            Debug.Log("A flag is already placed in this area.");
            return;
        }

        // ✅ 3. Instantiate the flag prefab in front of the camera
        Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
        Instantiate(flagPrefab, spawnPosition, Quaternion.identity);

        // ✅ 4. Record this flag's location
        spawnedFlagPositions.Add(new Vector2(playerLat, playerLon));

        // ✅ 5. Update score
        score++;
        scoreText.text = "Score: " + score;

        Debug.Log("Flag placed successfully!");
    }

    private bool IsFlagAlreadyPlacedNearby(float lat, float lon, float radiusMeters)
    {
        foreach (var pos in spawnedFlagPositions)
        {
            float distance = LocationManager.Instance.GetDistanceInMeters(lat, lon, pos.x, pos.y);
            if (distance < radiusMeters)
                return true;
        }
        return false;
    }
}

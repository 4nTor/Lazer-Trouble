using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlagLocation
{
    public string name;
    public float latitude;
    public float longitude;
}
public class LocationManager : MonoBehaviour
{
    public static LocationManager Instance;
    public float Latitude { get; private set; }
    public float Longitude { get; private set; }

    public List<FlagLocation> allowedZones = new List<FlagLocation>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            allowedZones.Add(new FlagLocation { name = "Library", latitude = 26.5123f, longitude = 80.2324f });
            allowedZones.Add(new FlagLocation { name = "OAT", latitude = 26.5137f, longitude = 80.2310f });
            allowedZones.Add(new FlagLocation { name = "Hall 2", latitude = 26.5110f, longitude = 80.2355f });
            allowedZones.Add(new FlagLocation { name = "Home", latitude = 28.4060657f, longitude = 77.3424699f });
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location not enabled by user.");
            yield break;
        }

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location.");
            yield break;
        }
        else
        {
            Latitude = Input.location.lastData.latitude;
            Longitude = Input.location.lastData.longitude;
            Debug.Log($"Lat: {Latitude}, Lon: {Longitude}");
        }
    }

    private void Update()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            Latitude = Input.location.lastData.latitude;
            Longitude = Input.location.lastData.longitude;
        }
    }
    public bool IsPlayerNearValidZone(float playerLat, float playerLon, float radiusMeters = 1000f)
    {
        foreach (var zone in allowedZones)
        {
            float distance = GetDistanceInMeters(playerLat, playerLon, zone.latitude, zone.longitude);
            if (distance < radiusMeters)
                return true;
        }
        return false;
    }

    // Haversine formula for GPS distance
    public float GetDistanceInMeters(float lat1, float lon1, float lat2, float lon2)
    {
        float R = 6371000f; // Radius of Earth in meters
        float dLat = Mathf.Deg2Rad * (lat2 - lat1);
        float dLon = Mathf.Deg2Rad * (lon2 - lon1);

        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
                  Mathf.Cos(Mathf.Deg2Rad * lat1) * Mathf.Cos(Mathf.Deg2Rad * lat2) *
                  Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));

        return R * c;
    }
}

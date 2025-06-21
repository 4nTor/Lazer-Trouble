using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redirector : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform emitterFace;  // Assign this to your LaserEmitterFace in prefab

    private bool hasEmitted = false; // Optional: prevent repeated triggering

    public void OnLaserHit()
    {
        if (hasEmitted) return;

        // Instantiate a new laser at emitter face
        GameObject newLaser = Instantiate(laserPrefab);

        newLaser.transform.position = emitterFace.position;
        newLaser.transform.rotation = emitterFace.rotation;  // So it emits in face direction

        hasEmitted = true;
    }
}

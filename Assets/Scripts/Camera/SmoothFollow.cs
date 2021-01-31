using System;
using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    private Camera cam;
    private Rigidbody2D targetRb;

    private void Start()
    {
        targetRb = target.GetComponent<Rigidbody2D>();
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (targetRb != null && cam != null)
        {
            var targetSize = Mathf.Clamp(targetRb.velocity.magnitude * 2f, 4f, 14f);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime);
        }

        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, -10));

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}

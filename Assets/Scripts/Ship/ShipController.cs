using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour
{
    public GameOptions GameOptions;
    private Rigidbody2D rb;

    void Awake()
    {
        Debug.Assert(GameOptions != null, "Missing game options!");
        rb = GetComponent<Rigidbody2D>();
        Debug.Assert(rb != null, "Missing rigidbody!");
    }

    void ProcessInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(rb.transform.up * Time.deltaTime * GameOptions.Ship_Speed);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * GameOptions.Ship_Speed);
            rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, 0f, Time.deltaTime);
            // rb.AddForce(-rb.transform.up * Time.deltaTime * GameOptions.Ship_Speed);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddTorque(Time.deltaTime * GameOptions.Ship_RotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddTorque(-Time.deltaTime * GameOptions.Ship_RotationSpeed);
        }
    }

    float angle;
    float startRotationOffset = -90;
    private Vector3 mousePositionInWorld;
    void RotateToMouse()
    {
        mousePositionInWorld = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        angle = ( Mathf.Atan2 (mousePositionInWorld.y - transform.position.y, mousePositionInWorld.x - transform.position.x) * Mathf.Rad2Deg);
        transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler(0, 0, angle+startRotationOffset),GameOptions.Ship_RotationSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        ProcessInput();
        RotateToMouse();
    }
}

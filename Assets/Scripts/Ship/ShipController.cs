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
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(rb.transform.up * Time.deltaTime * GameOptions.Ship_Speed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-rb.transform.up * Time.deltaTime * GameOptions.Ship_Speed);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(Time.deltaTime * GameOptions.Ship_RotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(-Time.deltaTime * GameOptions.Ship_RotationSpeed);
        }
    }

    void FixedUpdate()
    {
        ProcessInput();
    }
}

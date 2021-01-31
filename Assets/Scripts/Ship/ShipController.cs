using System;
using System.Collections;
using System.Collections.Generic;
using Logging;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Interactor))]
public class ShipController : MonoBehaviour, IPlanetNotificationReceiver
{
    public GameOptions GameOptions;
    public Action OnInteract;
    private Rigidbody2D rb;
    private Interactor interactor;
    private HashSet<CelestialBody> nearbyCelestialBodies;

    void Awake()
    {
        interactor = GetComponent<Interactor>();
        Debug.Assert(GameOptions != null, "Missing game options!");
        Debug.Assert(interactor != null, "Missing interactor!");
        rb = GetComponent<Rigidbody2D>();
        Debug.Assert(rb != null, "Missing rigidbody!");
        nearbyCelestialBodies = new HashSet<CelestialBody>();
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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (nearbyCelestialBodies.Count > 0)
            {
                foreach (var cb in nearbyCelestialBodies)
                {
                    L.og(L.Contexts.SHIP_CONTROLLER, $"I am near: {cb.Name}");
                }
            }
            else
            {
                L.og(L.Contexts.SHIP_CONTROLLER, $"There is nothing around me :(");
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            var cbs = interactor.Interact();
            if (OnInteract != null) OnInteract();
            if (cbs == null) return;
            foreach (var cb in cbs)
            {
                var response = cb.Interact();
                L.og(L.Contexts.SHIP_CONTROLLER, $"{cb.BodyType} {cb.Name} said: {response}");
            }
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

    public void Notify(CelestialBody cb, bool inRange)
    {
        if (inRange) nearbyCelestialBodies.Add(cb);
        else nearbyCelestialBodies.Remove(cb);
        var msg = inRange ? "in" : "out of";
        L.og(L.Contexts.SHIP_CONTROLLER, $"Planet {cb.Name} {msg} range");
    }
}

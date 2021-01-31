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
    public Action<bool> OnEngineRunningStateChange;
    private Rigidbody2D rb;
    private Interactor interactor;
    public HashSet<CelestialBody> nearbyCelestialBodies;
    public GameObject RadarUI;
    public float fuel=0;
    private GameManager gm;

    void Awake()
    {
        interactor = GetComponent<Interactor>();
        Debug.Assert(GameOptions != null, "Missing game options!");
        Debug.Assert(interactor != null, "Missing interactor!");
        rb = GetComponent<Rigidbody2D>();
        Debug.Assert(rb != null, "Missing rigidbody!");
        nearbyCelestialBodies = new HashSet<CelestialBody>();
        gm = GameObject.FindObjectOfType<GameManager>();
        Debug.Assert(interactor != null, "Missing game manager!");
        Debug.Assert(RadarUI != null, "Missing radar ui!");
    }
    bool isfullFuel = false;
    void Update()
    {
        ProcessInput();
        RotateToMouse();

        foreach(var cb in nearbyCelestialBodies)
        {   
            float angle = Mathf.Atan2(transform.position.y - cb.transform.position.y,transform.position.x - cb.transform.position.x) * Mathf.Rad2Deg+90f;
            RadarUI.transform.rotation = Quaternion.Euler(0,0,angle);
            break;
        }
        if(fuel>100000 && !isfullFuel )
        {
            isfullFuel=true;
            gm.HandleEvent(("", new CelestialBody.EventID("full fuel")));
        }
    }

    void ProcessInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(rb.transform.up * Time.deltaTime * GameOptions.Ship_Speed);
        }
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            OnEngineRunningStateChange?.Invoke(true);
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            OnEngineRunningStateChange?.Invoke(false);
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
                gm.HandleEvent(response);
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

    void AddFuel(float f){
        fuel+=f;
    }

    private bool seenAPlanet;
    private bool seenAStar;
    public void Notify(CelestialBody cb, bool inRange)
    {
        switch (cb)
        {
            case Planet p:
                if (!seenAPlanet)
                {
                    gm.HandleEvent(("", new CelestialBody.EventID("first planet")));
                    seenAPlanet = true;
                }
                break;
            case Star s:
                if (!seenAStar)
                {
                    gm.HandleEvent(("", new CelestialBody.EventID("first star")));
                    seenAStar = true;
                }
                break;
        }

        if (inRange) {
            nearbyCelestialBodies.Add(cb);
            RadarUI.SetActive(true);
            var exp = cb.GetComponent<CalcExplosion>();
            if(exp != null) exp.OnDamageDone += AddFuel;
        }
        else{
            nearbyCelestialBodies.Remove(cb);
            RadarUI.SetActive(false);
            var exp = cb.GetComponent<CalcExplosion>();
            if(exp != null) exp.OnDamageDone -= AddFuel;
        }
        var msg = inRange ? "in" : "out of";
        L.og(L.Contexts.SHIP_CONTROLLER, $"Planet {cb.Name} {msg} range");
    }
}

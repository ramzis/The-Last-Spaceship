using System;
using Logging;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HandleTrigger : MonoBehaviour
{
    public CelestialBody cb;
    public Collider2D triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider2D>();
        cb = this.GetComponentInChildren<CelestialBody>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player") return;
        var receiver = other.GetComponent<IPlanetNotificationReceiver>();
        if (receiver == null || cb == null)
        {
            L.og(L.Contexts.CELESTIAL_BODY, $"{cb.Name} can't notify: receiver or cb null");
            return;
        }
        receiver.Notify(cb, true);
        L.og(L.Contexts.CELESTIAL_BODY, "Notified in range");
        cb.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Player") return;
        var receiver = other.GetComponent<IPlanetNotificationReceiver>();
        if (receiver == null || cb == null)
        {
            L.og(L.Contexts.CELESTIAL_BODY, $"{cb.Name} can't notify: receiver or cb null");
            return;
        }
        receiver.Notify(cb, false);
        L.og(L.Contexts.CELESTIAL_BODY, "Notified out of range");
        cb.gameObject.SetActive(false);
    }
}

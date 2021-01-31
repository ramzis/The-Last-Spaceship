using System.Collections;
using System.Collections.Generic;
using Logging;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public GameOptions GameOptions;
    public LayerMask LayerMask;

    private void OnValidate()
    {
        Debug.Assert(GameOptions != null, "Missing game options!");
    }

    private Collider2D[] objs;
    private List<CelestialBody> cbs;

    public List<CelestialBody> Interact()
    {
        cbs = new List<CelestialBody>();
        foreach (var coll in Physics2D.OverlapCircleAll(transform.position, GameOptions.Ship_InteractionRadius))
        {
            if (coll.tag != "CB") continue;
            L.og(L.Contexts.SHIP_CONTROLLER, $"Hit {coll.name}");
            var cb = coll.GetComponent<CelestialBody>();
            if (cb == null) continue;
            L.og(L.Contexts.SHIP_CONTROLLER, $"Interacted with {cb.Name}");
            cbs.Add(cb);
            break;
        }

        /*
        float angle = 0;
        for (int i=0; i<30; i++) {
            float x = Mathf.Sin (angle);
            float y = Mathf.Cos (angle);
            angle += 2 * Mathf.PI / 30;

            bool found = false;
            Vector3 dir = new Vector3 (transform.position.x + x, transform.position.y + y, 0);
            Debug.DrawLine (transform.position, dir, Color.red);
            foreach (var hit in Physics2D.RaycastAll(transform.position, dir, 10f, LayerMask))
            {
                if(hit.collider.tag != "CB") continue;
                L.og(L.Contexts.SHIP_CONTROLLER, $"Hit {hit.transform.name}");
                var cb = hit.transform.GetComponent<CelestialBody>();
                if(cb == null) continue;
                L.og(L.Contexts.SHIP_CONTROLLER, $"Interacted with {cb.Name}");
                cbs.Add(cb);
                found = true;
                break;
            }
            if (found) break;
        }*/
        return cbs;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public GameObject player;
    public GameObject[] tiles;

    void Start()
    {
        for(int y=0;y<5;y++)
        {
            for(int x=0;x<7;x++)
            {
                tiles[x+y*7].transform.position = new Vector3((x-3)*10.24f,(y-2)*10.24f,0);
            }
        }
    }
    void Update()
    {
        for(int y=0;y<5;y++)
        {
            for(int x=0;x<7;x++)
            {
                //tiles[x+y*5].transform.position = new Vector3();
            }
        }
    }
}

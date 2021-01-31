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
                tiles[x+y*7].transform.rotation = Quaternion.Euler(0,0,Random.Range(0,4)*90f);
            }
        }
    }


    float xJump = 7*10.24f;
    float yJump = 5*10.24f;
    float localX;
    float localY;
    Vector3 playerPosInt;
    Vector3 tilePosInt;
    void Update()
    {
        playerPosInt = player.transform.position;
        playerPosInt = new Vector3(Mathf.Round(playerPosInt.x/10.24f),Mathf.Round(playerPosInt.y/10.24f),0);
        //Debug.Log(playerPosInt);

        for(int y=0;y<5;y++)
        {
            for(int x=0;x<7;x++)
            {
                localX = (x-3)*10.24f;
                localY = (y-2)*10.24f;

                tilePosInt = tiles[x+y*7].transform.position;
                tilePosInt = new Vector3(Mathf.Round(tilePosInt.x/10.24f),Mathf.Round(tilePosInt.y/10.24f),0);
                if(tilePosInt.x - playerPosInt.x>=4)
                {
                    tiles[x+y*7].transform.Translate(-xJump,0,0,Space.World);
                }
                if(tilePosInt.x - playerPosInt.x<=-4)
                {
                    tiles[x+y*7].transform.Translate(xJump,0,0,Space.World);
                }
                if(tilePosInt.y - playerPosInt.y>=3)
                {
                    tiles[x+y*7].transform.Translate(0,-yJump,0,Space.World);
                }
                if(tilePosInt.y - playerPosInt.y<=-3)
                {
                    tiles[x+y*7].transform.Translate(0,yJump,0,Space.World);
                }
            }
        }
        
    }
}

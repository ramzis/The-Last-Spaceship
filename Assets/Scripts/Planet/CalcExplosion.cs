using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logging;
using System;

public class CalcExplosion : MonoBehaviour
{
     RenderTexture rTex;
     RenderTexture Original;
    Material myMaterial;
    public ComputeShader compute;
    ComputeBuffer SamplesForColliders;
    // Start is called before the first frame update
    int size = 1024;
    public float scale = 1;

    BoxCollider2D[] a = new BoxCollider2D[64*64]; 

    public Action<float> OnDamageDone;
    void OnEnable()
    {
        Start();
    }
    
    void Awake()
    {
        myMaterial = GetComponent<Renderer>().material;
        Original = new RenderTexture(size, size, 16, RenderTextureFormat.ARGB32);
        Graphics.Blit(myMaterial.mainTexture, Original);
    }
    void Start()
    {
        transform.localScale = new Vector2(scale,scale);
        
        //

        rTex = new RenderTexture(size, size, 16, RenderTextureFormat.ARGB32);
        
        
       
        rTex.filterMode = FilterMode.Trilinear;
        rTex.wrapMode = TextureWrapMode.Repeat;
        rTex.enableRandomWrite = true;
        Graphics.Blit(myMaterial.mainTexture, rTex);
        
        rTex.Create();
        myMaterial.mainTexture = rTex;
        compute.SetTexture(0, "Result", rTex);
        compute.SetTexture(1, "Result", rTex);
        compute.SetTexture(2, "Result", rTex);
        compute.SetTexture(3, "Result", rTex);
        compute.SetTexture(3, "Original", Original);

        SamplesForColliders = new ComputeBuffer(64*64,4);
        compute.SetBuffer(2,"Colliders",SamplesForColliders);
    }

    // Update is called once per frame
    // void MouseClick()
    // {
    //     Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     //Debug.Log(worldPosition.x*size);


    //     compute.SetFloat("Dx", (worldPosition.x+0.5f*scale-transform.position.x)*(size/scale));
    //     compute.SetFloat("Dy", (worldPosition.y+0.5f*scale-transform.position.y)*(size/scale));
        

    //     compute.SetFloat("Power", 300f/scale);
    //     int kernelId = compute.FindKernel("CSMain");
    //     if (Input.GetMouseButtonDown(0))
    //     compute.Dispatch(0, size / 32, size / 32, 1);
    //     if (Input.GetMouseButtonDown(1))
    //     compute.Dispatch(3, size / 32, size / 32, 1);
        
    // }
    int startHitBoxes;
    int endHitBoxes;
    float resourcesGot;
    int iii;
    public void DoDamage(float power, Vector2 point, bool doHeal){

        compute.SetFloat("Dx", (point.x+0.5f*scale-transform.position.x)*(size/scale));
        compute.SetFloat("Dy", (point.y+0.5f*scale-transform.position.y)*(size/scale));

        compute.SetFloat("Power", power/Mathf.Sqrt(scale));
        if(!doHeal)
        {   
            compute.Dispatch(0, size / 32, size / 32, 1);
            
        }
        else{
        // compute.SetFloat("Power", power*10/scale);
        // compute.Dispatch(3, size / 32, size / 32, 1);
        }
    }

    int[] arr = new int[64*64];
    void Update()
    {
        //if (Input.GetMouseButtonDown(0)||Input.GetMouseButtonDown(1))MouseClick();
        
        //for(int a=0;a<3;a++)
        compute.Dispatch(1, size / 32, size / 32, 1);

        //calculate damage done
        startHitBoxes =0;
        for( iii =0;iii<64*64;iii++)
        {
            if(arr[iii]==1)
                startHitBoxes ++;
        }
        //=========
        compute.Dispatch(2, 64 / 32, 64 / 32, 1);
        
        SamplesForColliders.GetData(arr);

        //calculate damage done
        endHitBoxes =0;
        for( iii =0;iii<64*64;iii++)
        {
            if(arr[iii]==1)
                endHitBoxes++;
        }
        if(endHitBoxes==0)Destroy(gameObject.transform.parent.gameObject);
        resourcesGot = (startHitBoxes-endHitBoxes)*scale;

        if(resourcesGot<0)resourcesGot=0;
        if(OnDamageDone!=null)
        {
            if(resourcesGot!=0)
                OnDamageDone(resourcesGot);
            
        }
        L.og(L.Contexts.RESOURCES, $"{resourcesGot}");
        //=========

        for(int y =0;y<64;y++)
        {
            for(int x =0;x<64;x++)
            {   
                //Debug.Log(arr[x+y*64]);
                if(a[x+y*64]==null && arr[x+y*64]==1)
                {
                    a[x+y*64] = (BoxCollider2D)gameObject.AddComponent<BoxCollider2D>();
                    a[x+y*64].size= new Vector2(1f/64f,1f/64f);
                    a[x+y*64].offset = new Vector2(1f/64f*x-1f/64f*32f,1f/64f*y-1f/64f*32f);
                }
                if(a[x+y*64]!=null && arr[x+y*64]==0)
                {
                    Destroy(a[x+y*64]);
                }
                
            }
        }

    }
}


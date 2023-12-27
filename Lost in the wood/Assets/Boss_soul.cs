using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_soul : MonoBehaviour
{
    [SerializeField] private float moveStart;
    [SerializeField] private float _y;
    [SerializeField] private float moveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }
   
    // Update is called once per frame
    void Update()
    {

        moveStart += moveSpeed * Time.deltaTime;
       // gameObject.transform.position.x = new Vector3(moveupdate*Time.deltaTime,0f,0f);
        gameObject.transform.position = new Vector3(moveStart, _y, 0f);
    }
}

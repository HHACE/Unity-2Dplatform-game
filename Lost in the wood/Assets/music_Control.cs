using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music_Control : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

        GameObject[] StackMusicCheck = GameObject.FindGameObjectsWithTag("music");
        if (StackMusicCheck.Length > 1)
        {
            Destroy(this.gameObject);
        }


        DontDestroyOnLoad(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

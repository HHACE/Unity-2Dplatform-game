using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exist : MonoBehaviour
{
    private GameObject _music;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Stage-Chase")
        {
            SceneManager.LoadScene("End");
        }
        else if (sceneName == "Stage-Tutorial")
        {
            SceneManager.LoadScene("Stage-Begin");
        }
        else if (sceneName == "Stage-Begin") {
            _music = GameObject.FindGameObjectWithTag("music");
            Destroy(_music);
            SceneManager.LoadScene("Stage-Chase");
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_control : MonoBehaviour
{
    private GameObject _music;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        pressSpace();
    }

    private void pressSpace()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (sceneName == "End")
            {
                _music = GameObject.FindGameObjectWithTag("music");
                Destroy(_music);
                SceneManager.LoadScene("Menu");
            }
            else
            {
                SceneManager.LoadScene("Stage-Tutorial");
            }
        }
    }

}

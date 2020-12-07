using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
        public void LoadScene(int sceneIndex){
            SceneManager.LoadScene(sceneIndex);
        }
    

    // Update is called once per frame
    public void QuitGame() {
        Application.Quit();
    }
}

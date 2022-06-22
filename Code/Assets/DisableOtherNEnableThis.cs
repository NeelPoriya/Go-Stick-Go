using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisableOtherNEnableThis : MonoBehaviour
{
    public Canvas ToBeDisabled;
    private void Awake()
    {
        ToBeDisabled.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void LoadThisLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

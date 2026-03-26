using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{

    [SerializeField] GameObject Settings;
    [SerializeField] GameObject UI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(2);
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

    public void CloseSettings()
    {
        Settings.SetActive(false);
        UI.SetActive(true);
    }
}

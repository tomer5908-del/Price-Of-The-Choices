using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public GameObject game_scene;
    public GameObject main_scene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Movement.game_running = false;
        game_scene.SetActive(false);
        main_scene.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        Movement.game_running = true;
        game_scene.SetActive(true);
        main_scene.SetActive(false);
    }
}

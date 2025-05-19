using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.MainMenu
{
    public class PlayButton : MonoBehaviour
    {
        public void StartGame()
        {
            // Load the game scene here
            SceneManager.LoadScene("GameScene");
        }
    }
}

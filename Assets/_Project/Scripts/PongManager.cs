using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts
{
    public class PongManager : MonoBehaviour
    {
        private void Update()
        {
            if (InputManager.Restart())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}

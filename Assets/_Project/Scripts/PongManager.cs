using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts
{
    public class PongManager : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseScreen;
        private int _currentTimeScale = 1;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            SetPauseState(false);
        }

        private void Update()
        {
            if (InputManager.Restart())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            if (InputManager.Pause())
            {
                TogglePause();
            }
            
            if (InputManager.Escape())
            {
                SceneManager.LoadScene(ScenesHolder.Menu);
            }
        }

        private void TogglePause()
        {
            _currentTimeScale = _currentTimeScale == 1 ? 0 : 1;
            _pauseScreen.SetActive(_currentTimeScale == 0);
            Time.timeScale = _currentTimeScale;
        }

        private void SetPauseState(bool state)
        {
            _currentTimeScale = state ? 0 : 1;
            _pauseScreen.SetActive(state);
            Time.timeScale = _currentTimeScale;
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private Button _slowButton;
        [SerializeField] private Button _fastButton;
        [SerializeField] private GameObject _loadingMessage;
        
        private bool IsLoadingScene { get; set; }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Cursor.visible = true;
            IsLoadingScene = false;
            _loadingMessage.SetActive(false);
            _slowButton.onClick.AddListener(OnSlowButtonClick);
            _fastButton.onClick.AddListener(OnFastButtonClick);
        }

        private void OnSlowButtonClick()
        {
            LoadScene(ScenesHolder.Slow);
        }
        
        private void OnFastButtonClick()
        {
            LoadScene(ScenesHolder.Fast);
        }

        private void LoadScene(int scene)
        {
            if (IsLoadingScene) return;
            
            StartCoroutine(LoadSceneDelayed());

            IEnumerator LoadSceneDelayed()
            {
                IsLoadingScene = true;
                _loadingMessage.SetActive(true);
                yield return new WaitForSeconds(0.2f);
                SceneManager.LoadScene(scene);
            }
        }

        private void Update()
        {
            if (InputManager.Escape())
            {
                Application.Quit();
            }
        }
    }
}

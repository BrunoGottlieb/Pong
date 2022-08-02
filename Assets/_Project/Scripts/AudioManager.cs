using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private bool _isEnabled;
        [SerializeField] private Ball _ball;

        [Header("Sounds")]
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip _leftTouchClip;
        [SerializeField] private AudioClip _rightTouchClip;
        [SerializeField] private AudioClip _wallClip;
        [SerializeField] private AudioClip _scoreClip;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (!_isEnabled) return;

            _ball.OnLeftTouch += OnLeftTouch;
            _ball.OnRightTouch += OnRightTouch;
            _ball.OnWallHit += OnWallHit;
            _ball.OnLeftScore += OnScore;
            _ball.OnRightScore += OnScore;
        }

        private void OnScore()
        {
            PlayClip(_scoreClip);
        }

        private void OnWallHit()
        {
            PlayClip(_wallClip);
        }

        private void OnLeftTouch()
        {
            PlayClip(_leftTouchClip);
        }
        
        private void OnRightTouch()
        {
            PlayClip(_rightTouchClip);
        }

        private void PlayClip(AudioClip clip)
        {
            _source.PlayOneShot(clip);
        }
    }
}

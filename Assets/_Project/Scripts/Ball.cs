using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace _Project.Scripts
{
    public class Ball : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerAgent leftPlayerAgent;
        [SerializeField] private PlayerAgent rightPlayerAgent;
        [SerializeField] private Transform _leftPlayer;
        [SerializeField] private Transform _rightPlayer;
        [SerializeField] private TextMeshProUGUI _text;
        
        [Header("Values")]
        [SerializeField] private float _speed = 15f;

        private Vector2 _leftPlayerDir;
        private Vector2 _rightPlayerDir;

        private Vector2 _referential;

        private const float _playerY = 0.9f;
        private const float _playerX = 0.25f;

        private const float _boardHeight = 4.5f;
        private float _boardWidth;
        
        private Vector2 PlayerDir => _currentForward == Side.Left ? _leftPlayerDir : _rightPlayerDir;
        private float Speed { get; set; }

        private enum Side
        {
            Left,
            Right
        }
        
        private enum Wall
        {
            None,
            Up,
            Down
        }
        
        private Side _currentForward = Side.Left;
        private Wall _lastWallCollided = Wall.None;

        private Coroutine _coroutine = null;

        private const float _timeToTouch = 7;
        private float _timeSinceLastTouch = _timeToTouch;
        
        private void Awake()
        {
            _boardWidth = _rightPlayer.position.x + 1;
            ResetGameState();
        }

        private void Update()
        {
            _text.text = PlayerDir.ToString();

            CalculateDirections();
            PlayerCollision();
            WallCollision();
            Move();
            Score();

            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            _timeSinceLastTouch -= Time.deltaTime;
            if(_timeSinceLastTouch <= 0)
                ResetGameState();
        }

        private void ResetGameState()
        {
            leftPlayerAgent.EndEpisode();
            rightPlayerAgent.EndEpisode();
            
            _timeSinceLastTouch = _timeToTouch;
            transform.position = Vector2.zero;
            _referential = Vector3.zero;
            _currentForward = Side.Left;
            _lastWallCollided = Wall.None;
            if(_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(ThrowBall());
        }

        IEnumerator ThrowBall()
        {
            yield return new WaitForSeconds(1);
            _referential = new Vector2(-10, Random.Range(-3, 3)) * 100;
            Speed = _speed / 2;
        }

        private void CalculateDirections()
        {
            Vector3 pos = transform.position;
            _leftPlayerDir = pos - _leftPlayer.position;
            _rightPlayerDir = pos - _rightPlayer.position;
        }

        private void Score()
        {
            if (transform.position.x > _boardWidth)
            {
                leftPlayerAgent.SetReward(1);
                rightPlayerAgent.SetReward(-1);
                ResetGameState();
            }
            else if (transform.position.x < - _boardWidth)
            {
                leftPlayerAgent.SetReward(-1);
                rightPlayerAgent.SetReward(1);
                ResetGameState();
            }
        }

        private void Move()
        {
            Vector2 pos;
            
            pos = transform.position;
            pos = Vector2.MoveTowards(pos, _referential, Speed * Time.deltaTime);
            transform.position = pos;
        }

        private void WallCollision()
        {
            Vector2 pos = transform.position;
            if (pos.y >= _boardHeight && (_lastWallCollided == Wall.Down || _lastWallCollided == Wall.None))
            {
                ToggleReferentialY();
                _lastWallCollided = Wall.Up;
            }
            else if (pos.y <= - _boardHeight && (_lastWallCollided == Wall.Up || _lastWallCollided == Wall.None))
            {
                ToggleReferentialY();
                _lastWallCollided = Wall.Down;
            }
        }

        private void ToggleReferentialY()
        {
            _referential = new Vector2(_referential.x, - _referential.y);
        }

        private void PlayerCollision()
        {
            if (IsUnderXLimit() && IsUnderYLimit())
            {
                _text.color = Color.green;
                SetReferential(PlayerDir);
                ToggleSide();
                _lastWallCollided = Wall.None;
                Speed = _speed;
                _timeSinceLastTouch = _timeToTouch;
            }
            else _text.color = Color.white;
        }

        private bool IsUnderXLimit()
        {
            return PlayerDir.x <= _playerX && PlayerDir.x > -_playerX;
        }
        
        private bool IsUnderYLimit()
        {
            return PlayerDir.y <= _playerY && PlayerDir.y > -_playerY;
        }

        private void ToggleSide()
        {
            _currentForward = _currentForward == Side.Left ? Side.Right : Side.Left;
        }

        private void SetReferential(Vector2 value)
        {
            Vector2 preValue;
            preValue.x = value.x;
            preValue.y = value.y * 0.5f;
            _referential = preValue * 100;
        }
    }
}

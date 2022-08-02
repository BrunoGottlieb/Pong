using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts
{
    public class Ball : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _leftPlayer;
        [SerializeField] private Transform _rightPlayer;
        [SerializeField] private Transform _upWall;
        [SerializeField] private Transform _downWall;
        [SerializeField] private bool _movementEnabled;
        
        [Header("Values")]
        [SerializeField] private float _speed = 15f;

        private Vector2 _leftPlayerDir;
        private Vector2 _rightPlayerDir;

        private Vector2 _referential;

        private const float _playerY = 0.9f;
        private const float _playerX = 0.25f;

        private const float _boardHeight = 4.5f;
        private float _boardWidth;

        private Vector2 _ballInitialPos;

        private float _timeSinceLastTouch;
        private float _maxTimeWithoutTouch = 7;
        
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

        public Action OnLeftTouch;
        public Action OnRightTouch;
        public Action OnLeftScore;
        public Action OnRightScore;
        public Action OnTimeOut;
        public Action OnWallHit;

        private void Awake()
        {
            _boardWidth = _rightPlayer.localPosition.x + 1;
            _ballInitialPos = transform.localPosition;
            ResetGameState();
        }

        private void Update()
        {
            CalculateDirections();
            PlayerCollision();
            WallCollision();
            Move();
            Score();
            TimeOut();
        }

        private void TimeOut()
        {
            _timeSinceLastTouch -= Time.deltaTime;

            if (_timeSinceLastTouch <= 0)
            {
                print("TimeOut: " + _referential.ToString());
                OnTimeOut?.Invoke();
            }
        }

        public void ResetGameState()
        {
            _timeSinceLastTouch = _maxTimeWithoutTouch;
            Speed = 0;
            transform.localPosition = _ballInitialPos;
            _referential = Vector3.zero;
            if(_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(ThrowBall());
        }

        IEnumerator ThrowBall()
        {
            yield return new WaitForSeconds(1);
            _currentForward = Side.Left;
            _lastWallCollided = Wall.None;
            _referential = new Vector2(-10, Random.Range(-3, 3)) * 100;
            Speed = _speed / 2;
            
            /*yield return new WaitForSeconds(1);
            _currentForward = Side.Right;
            _lastWallCollided = Wall.None;
            transform.localPosition =  new Vector2(-8, Random.Range(-4.5f, 4.5f));
            _referential = new Vector2(10, Random.Range(-20f, 20f)) * 100;
            Speed = _speed;*/
        }

        private void CalculateDirections()
        {
            Vector3 pos = transform.localPosition;
            _leftPlayerDir = pos - _leftPlayer.localPosition;
            _rightPlayerDir = pos - _rightPlayer.localPosition;
        }

        private void Score()
        {
            if (transform.localPosition.x > _boardWidth)
            {
                OnLeftScore?.Invoke();
            }
            else if (transform.localPosition.x < - _boardWidth)
            {
                OnRightScore?.Invoke();
            }
        }

        private void Move()
        {
            if (!_movementEnabled) return;
            
            Vector2 pos;
            
            pos = transform.localPosition;
            pos = Vector2.MoveTowards(pos, _referential, Speed * Time.deltaTime);
            transform.localPosition = pos;
        }

        private void WallCollision()
        {
            Vector2 pos = transform.localPosition;
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
            OnWallHit?.Invoke();
        }

        private void PlayerCollision()
        {
            if (Speed == 0)
                return;
            
            if (IsUnderXLimit() && IsUnderYLimit())
            {
                SetReferential(PlayerDir);
                _lastWallCollided = Wall.None;
                Speed = _speed;
                
                if(_currentForward == Side.Right)
                    OnRightTouch?.Invoke();
                else if(_currentForward == Side.Left)
                    OnLeftTouch?.Invoke();

                _timeSinceLastTouch = _maxTimeWithoutTouch;
                
                ToggleSide();
            }
        }

        private bool IsUnderXLimit()
        {
            return PlayerDir.x < _playerX && PlayerDir.x > -_playerX;
        }
        
        private bool IsUnderYLimit()
        {
            return PlayerDir.y < _playerY && PlayerDir.y > -_playerY;
        }

        private void ToggleSide()
        {
            _currentForward = _currentForward == Side.Left ? Side.Right : Side.Left;
        }

        private void SetReferential(Vector2 value)
        {
            Vector2 preValue;
            if (value.x >= 0 && value.x < 0.2f) value.x = 0.2f;
            else if (value.x > -0.2f && value.x < 0) value.x = -0.2f;
            preValue.x = value.x;
            preValue.y = value.y / _playerY;
            preValue.y *= 0.35f;
            _referential = preValue * 1000;
        }
    }
}

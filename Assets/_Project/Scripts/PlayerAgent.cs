using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts
{
    public class PlayerAgent : Agent
    {
        [SerializeField] private int _id;
        [SerializeField] private float _speed = 10;
        [SerializeField] private SpriteRenderer _image;
        [SerializeField] private Ball _ball;
        
        private const int _targetDistance = 10;
        private const float _boardHeight = 4f;

        private Coroutine _blinkRoutine;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_id == 0) // Left
            {
                _ball.OnRightScore += OnScore;
                _ball.OnLeftTouch += OnTouch;
                _ball.OnTimeOut += OnTimeOut;
            }
            else if (_id == 1) // Right
            {
                _ball.OnLeftScore += OnScore;
                _ball.OnRightTouch += OnTouch;
                _ball.OnTimeOut += OnTimeOut;
            }
        }

        private void OnTouch()
        {
            SetReward(1f);
            BlinkPad(Color.green);
            //EndEpisode();
        }

        private void OnScore()
        {
            //SetReward(-1);
            BlinkPad(Color.red);
            EndEpisode();
        }

        private void OnTimeOut()
        {
            BlinkPad(Color.yellow);
            EndEpisode();
        }

        private void BlinkPad(Color color)
        {
            if(_blinkRoutine != null)
                StopCoroutine(_blinkRoutine);
            _blinkRoutine = StartCoroutine(BlinkColor(color));
        }

        private IEnumerator BlinkColor(Color color)
        {
            _image.color = color;
            yield return new WaitForSeconds(1);
            _image.color = Color.white;
        }

        private void Move(ActionBuffers actionBuffers)
        {
            if (actionBuffers.DiscreteActions[0] == 0)
            {
                DoMove(_targetDistance);
            }
            else if (actionBuffers.DiscreteActions[0] == 1)
            {
                DoMove(_targetDistance * -1);
            }
        }

        private void DoMove(float towards)
        {
            Vector2 pos = transform.localPosition;
            pos.y = Mathf.MoveTowards(pos.y, towards, _speed * Time.deltaTime);
            if(IsUnderLimit(pos.y))
                transform.localPosition = pos;
        }
        
        private bool IsUnderLimit(float y)
        {
            return y is < _boardHeight and > _boardHeight * -1;
        }

        public override void OnEpisodeBegin()
        {
            _ball.ResetGameState();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(_ball.transform.localPosition.x);
            sensor.AddObservation(_ball.transform.localPosition.y);
            if(SceneManager.GetActiveScene().buildIndex == ScenesHolder.Fast)
                sensor.AddObservation(transform.localPosition - _ball.transform.localPosition);
            sensor.AddObservation(transform.localPosition.y);
        }

        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            Move(actionBuffers);
            //AddReward(Mathf.Abs(transform.localPosition.y) * -0.0004f);
        }
        
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var discreteActionsOut = actionsOut.DiscreteActions;
            
            if (InputManager.MoveUp())
            {
                discreteActionsOut[0] = 0;
            }
            else if (InputManager.MoveDown())
            {
                discreteActionsOut[0] = 1;
            }
            else
            {
                discreteActionsOut[0] = 2;
            }
        }
    }
}
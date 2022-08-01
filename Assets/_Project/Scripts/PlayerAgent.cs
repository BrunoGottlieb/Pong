using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace _Project.Scripts
{
    public class PlayerAgent : Agent
    {
        [SerializeField] private int _id;
        [SerializeField] private SpriteRenderer _image;
        [SerializeField] private Ball _ball;
        
        private const float _speed = 10;
        private const int _targetDistance = 10;
        private const float _boardHeight = 4;

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
            AddReward(1);
            if(_blinkRoutine != null)
                StopCoroutine(_blinkRoutine);
            _blinkRoutine = StartCoroutine(BlinkColor(Color.green));
            //EndEpisode();
        }

        private void OnScore()
        {
            //SetReward(-1);
            if(_blinkRoutine != null)
                StopCoroutine(_blinkRoutine);
            _blinkRoutine = StartCoroutine(BlinkColor(Color.red));
            EndEpisode();
        }

        private void OnTimeOut()
        {
            if(_blinkRoutine != null)
                StopCoroutine(_blinkRoutine);
            _blinkRoutine = StartCoroutine(BlinkColor(Color.yellow));
            EndEpisode();
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
            sensor.AddObservation(_ball.transform.localPosition);
            sensor.AddObservation(transform.localPosition);
        }

        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            Move(actionBuffers);
        }
        
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var discreteActionsOut = actionsOut.DiscreteActions;
            
            if (Input.GetKey(KeyCode.UpArrow))
            {
                discreteActionsOut[0] = 0;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                discreteActionsOut[0] = 1;
            }
            else
            {
                discreteActionsOut[0] = -1;
            }
        }
    }
}
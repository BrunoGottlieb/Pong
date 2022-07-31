using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace _Project.Scripts
{
    public class PlayerAgent : Agent
    {
        [SerializeField] private Ball _ball;
        
        private const float _speed = 10;
        private const int _targetDistance = 10;
        private const float _boardHeight = 4;

        private void Move(ActionBuffers actionBuffers)
        {
            if (actionBuffers.ContinuousActions[0] > 0)
            {
                DoMove(_targetDistance);
            }
            else if (actionBuffers.ContinuousActions[0] < 0)
            {
                DoMove(_targetDistance * -1);
            }
        }

        private void DoMove(float towards)
        {
            Vector2 pos = transform.position;
            pos.y = Mathf.MoveTowards(pos.y, towards, _speed * Time.deltaTime);
            if(IsUnderLimit(pos.y))
                transform.position = pos;
        }
        
        private bool IsUnderLimit(float y)
        {
            return y is < _boardHeight and > _boardHeight * -1;
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(_ball.transform.position);
            sensor.AddObservation(transform.position);
        }

        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            Move(actionBuffers);
        }
        
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var continuousActionsOut = actionsOut.ContinuousActions;
            
            if (Input.GetKey(KeyCode.UpArrow))
            {
                continuousActionsOut[0] = 1;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                continuousActionsOut[0] = -1;
            }
        }
    }
}
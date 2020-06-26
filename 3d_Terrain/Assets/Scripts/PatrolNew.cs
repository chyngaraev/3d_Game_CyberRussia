using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolNew : MonoBehaviour
{
    [SerializeField]
    bool _patrolWaiting;

    [SerializeField]
    float _totalWaitTime = 3f;

    [SerializeField]
    float _switchProbability = 0.2f;

    [SerializeField]
    List<Waypoint> _patrolPoints;

    NavMeshAgent _navMeshAgent;
    int _currentPatrolIndex;
    bool _travalling;
    bool _waiting;
    bool _patrolForward;
    float _waiTimer;

    public void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (_navMeshAgent == null)
        {
            Debug.LogError(" 4eta tam " + gameObject.name);
        }
        else
        {
            if (_patrolPoints != null && _patrolPoints.Count >=2)
            {
                _currentPatrolIndex = 0;
                SetDestination();
            }
            else
            {
                Debug.Log("4eta tam ");
            }
        }
    }


    public void Update()
    {
        if(_travalling && _navMeshAgent.remainingDistance <= 1.0f)
        {
            _travalling = false;

            if (_patrolWaiting)
            {
                _waiting = false;
                _waiTimer = 0f;
                gameObject.GetComponent<Animator>().SetTrigger("Idle");
            }
            else
            {
                ChangePatrolPoint();
                SetDestination();
            }
        }

        if(_waiting)
        {
            _waiTimer += Time.deltaTime;
            if(_waiTimer >= _totalWaitTime)
            {
                _waiting = false;
                gameObject.GetComponent<Animator>().SetTrigger("Run");
                ChangePatrolPoint();
                SetDestination();
            }
        }
    }


    private void SetDestination()
    {
        if(_patrolPoints != null)
        {
            Vector3 targetVector = _patrolPoints[_currentPatrolIndex].transform.position;
            _navMeshAgent.SetDestination(targetVector);
            _travalling = true;
        }
    }


    private void ChangePatrolPoint()
    {
        if(UnityEngine.Random.Range(0f, 1f) <= _switchProbability)
        {
            _patrolForward = !_patrolForward;
        }

        if(_patrolForward)
        {
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
        }
        else
        {
            if(--_currentPatrolIndex < 0)
            {
                _currentPatrolIndex = _patrolPoints.Count - 1;
            }
        }
    }
}

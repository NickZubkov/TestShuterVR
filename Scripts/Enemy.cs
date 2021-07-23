using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamage
{
    
    [SerializeField] private Transform _target = null;
    [Space]
    [SerializeField] private int _damage = 1;
    [SerializeField] private int _hp = 1;
    [SerializeField] private int _points = 5;
    [Space]
    [SerializeField] private GameObject _gun = null;
    [SerializeField] private GameObject[] _itemsToDrop = null;
    [SerializeField][Range(0, 100)] private int _dropChance = 100;



    private bool _dropFlag = false;
    private NavMeshAgent _navMeshAgent = null;
    private Vector3 _curentWayPoint = Vector3.zero;
    private float _shootTime = 1.5f;
    private bool _shootDelay = false;
    
    private void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _curentWayPoint = GetRandomPoint(transform.position, 50);
    }
    private void FixedUpdate()
    {
        Patrol();
        PursuitAndShooting();
    }
    private void Patrol()
    {
        var distanceToTarget = Vector3.Distance(_curentWayPoint, transform.position);
        if (distanceToTarget <= _navMeshAgent.stoppingDistance)
        {
            _curentWayPoint = GetRandomPoint(transform.position, 50);
        }
        _navMeshAgent.SetDestination(_curentWayPoint);
    }

    private void PursuitAndShooting()
    {
        var _targetDistance = Vector3.Distance(_target.position, transform.position);

        if (_targetDistance <= 50.0f)
        {
            _curentWayPoint = _target.position;
            if (_targetDistance <= 40f && _targetDistance >= 25 && !_shootDelay)
            {
                _shootTime = 1.5f;
                Shoot();
            }
            if (_targetDistance <= 24f && _targetDistance >= 11 && !_shootDelay)
            {
                _shootTime = 0.7f;
                Shoot();
            }
            if (_targetDistance <= 10f && !_shootDelay)
            {
                _shootTime = 0.1f;
                Shoot();
            }
        }
    }

    private static Vector3 GetRandomPoint(Vector3 center, float maxDistance) //случайная точка для патруля
    {
        Vector3 randomPos = Random.insideUnitSphere * maxDistance + center;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);
        return hit.position;
    }
    private void Shoot()
    {
        RaycastHit hit;
        var startPosition = transform.position;
        startPosition.y += 1.5f; // луч запускается на уровне оружия, а не из ног
        var rayDirection = _target.position - transform.position;
        var rayCast = Physics.Raycast(startPosition, rayDirection, out hit, 1000);

        if (hit.collider.tag == "Player")
        {
            hit.collider.GetComponent<IDamage>().TakeDamage(_damage);
            _shootDelay = true;
            Invoke("ShootOffDelay", _shootTime);
        }
    }
    private void ShootOffDelay()//задержка выстрела для разных режимов стрельбы
    {
        _shootDelay = false;
    }

    public void TakeDamage(int damage)
    {
        _hp -= damage;
        if(_hp <= 0)
        {
            _target.GetComponent<IGamePoints>().TakePoints(_points);
            Drop();
            Destroy(gameObject, 1f);
        }
    }
    private void Drop()
    {
        var itemID = Random.Range(0, _itemsToDrop.Length);
        var gunDrop = Random.Range(0, 101);
        if(gunDrop <=_dropChance && !_dropFlag)
        {
            var drop = Instantiate(_itemsToDrop[itemID], gameObject.transform.position, Quaternion.identity);
        }
        _dropFlag = true;
    }

}

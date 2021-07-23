using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamage, IGamePoints, IHealing
{
    [SerializeField] private int _hp = 100;
    [SerializeField] private int _points = 0;
    [Space]
    [SerializeField] private GameObject _gunCurrent = null;
    [SerializeField] private Transform _gunPosition = null;
    [SerializeField] private GameObject[] _guns = null;
    [Space]
    [SerializeField] private int _speed = 3;

    private bool _fire = false;
    private bool _gunChange = false;
    private bool _actionButton = false;
    private int _gunID = 0;
    private Vector3 _playerMoov = Vector3.zero;
    private Vector3 _playerDirection = Vector3.zero;
    private CharacterController controller = null;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _playerMoov = new Vector3(_speed * OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x, 0, _speed * OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y);
        _playerDirection = new Vector3(0, _speed * OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x, 0);
        controller.SimpleMove(_playerMoov);
        gameObject.transform.Rotate(_playerDirection);
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && !_fire)
        {
            _fire = true;
        }
        if (_fire)
        {
            _gunCurrent.GetComponent<IGun>().Fire();
            _fire = false;
        }
        if (OVRInput.Get(OVRInput.RawButton.RHandTrigger) && !_actionButton)
        {
            _actionButton = true;
        }
        if (!OVRInput.Get(OVRInput.RawButton.RHandTrigger) && _actionButton)
        {
            _actionButton = false;
        }
        if (OVRInput.Get(OVRInput.RawButton.LHandTrigger) && !_gunChange)
        {
            _gunChange = true;
        }
        if (_gunChange)
        {
            GunCurrent(_gunID);
            _gunChange = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MashinGun") && _actionButton)
        {
            var bullets = other.GetComponentInParent<Gun>()._bullets;
            _guns[1].GetComponent<IGun>().AddBullet(bullets);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Pistol") && _actionButton)
        {
            var bullets = other.GetComponentInParent<Gun>()._bullets;
            _guns[0].GetComponent<IGun>().AddBullet(bullets);
            Destroy(other.gameObject);
        }
    }
    private void GunCurrent(int gunID)
    {
        if (_gunID == 0)
        {
            _gunID = 1;
        }
        else if (_gunID == 1)
        {
            _gunID = 0;
        }
        _gunCurrent.SetActive(false);
        _gunCurrent = _guns[gunID];
        _gunCurrent.SetActive(true);
    }
    public void TakeDamage(int damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            Destroy(gameObject, 2f);
        }
    }
    public void TakePoints(int points)
    {
        _points += points;
    }
    public void Healing(int hp)
    {
        _hp += hp;
    }
}

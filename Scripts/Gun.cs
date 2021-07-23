using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IGun
{
    public int _bullets = 45;
    [SerializeField] private int _damage = 2;
    [SerializeField] private GameObject _shootFlash = null;
    [SerializeField] private float _shootTime = 0.2f;
    private bool _shootDelay = false;
    private float _shootFlashTime = 0.1f;


    public void Fire()
    {
        RaycastHit hit;
        var startPosition = transform.position;
        var rayDirection = transform.forward * 50;
        if (_bullets > 0 && !_shootDelay)
        {
            var rayCast = Physics.Raycast(startPosition, rayDirection, out hit, Mathf.Infinity);
            Debug.DrawRay(startPosition, rayDirection, Color.red, 0.5f );
            if (hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<IDamage>().TakeDamage(_damage);
            }
            _shootDelay = true;
            _shootFlash.SetActive(true);
            Invoke("ShootFlash", _shootFlashTime);
            Invoke("ShootDelay", _shootTime);
            _bullets--;
        }
    }
    public void AddBullet(int bullet)
    {
        _bullets += bullet;
    }
    private void ShootDelay()
    {
        _shootDelay = false;
    }
    private void ShootFlash()
    {
        _shootFlash.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    [SerializeField] private GameObject _target = null;
    [SerializeField] private int _hp = 100;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _target.GetComponent<IHealing>().Healing(_hp);
            Destroy(gameObject);
        }
    }
}

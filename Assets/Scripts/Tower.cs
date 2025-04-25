using System;
using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Room Room { get; set; }
    [SerializeField] private float _timeToActivate = 3f;
    private Collider _collider;
    private bool _isActivate = false;
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _isActivate = false;
    }

    private void OnEnable()
    {
        EventHandlers.OnActivateTower += OnActivateTower;
    }
    private void OnDisable()
    {
        EventHandlers.OnActivateTower -= OnActivateTower;
    }
    private void OnActivateTower(Room room)
    {
        if (room != Room) return;
        _isActivate = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Helpers.Tag.Player))
        {
            if (_isActivate) return;
            StartCoroutine(ActivateTower());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Helpers.Tag.Player))
        {
            if (_isActivate) return;
            StopAllCoroutines();
        }
    }

    private IEnumerator ActivateTower()
    {
        yield return new WaitForSeconds(_timeToActivate);
        EventHandlers.CallOnActivateTower(Room);
    }

    public void Interactive(bool isInteractive)
    {
        _collider.enabled = isInteractive;
    }

}

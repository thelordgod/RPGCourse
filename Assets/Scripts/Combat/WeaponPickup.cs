using System;
using System.Collections;
using UnityEngine;

namespace Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        [SerializeField] private float respawnTime = 5;

        private SphereCollider _collider;
        private Transform[] _children;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _children = GetComponentsInChildren<Transform>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            other.GetComponent<Fighter>().EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            HidePickup();
            yield return new WaitForSeconds(seconds);
            ShowPickup();
        }

        private void ShowPickup()
        {
            print("attempting to show pickup...");
            _collider.enabled = true;
            foreach (var child in _children)
            {
                if (child == gameObject.transform) continue;
                child.gameObject.SetActive(true);
            }
        }

        private void HidePickup()
        {
            print("attempting to hide pickup...");
            _collider.enabled = false;
            foreach (var child in _children)
            {
                if (child == gameObject.transform) continue;
                child.gameObject.SetActive(false);
            }
        }
    }
}
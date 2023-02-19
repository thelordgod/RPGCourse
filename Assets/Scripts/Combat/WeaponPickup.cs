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
        private MeshRenderer _meshRenderer;
        private ParticleSystem.EmissionModule _particleSystemEmission;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            _particleSystemEmission = GetComponentInChildren<ParticleSystem>().emission;
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
            _collider.enabled = true;
            _meshRenderer.enabled = true;
            _particleSystemEmission.enabled = true;
        }

        private void HidePickup()
        {
            _collider.enabled = false;
            _meshRenderer.enabled = false;
            _particleSystemEmission.enabled = false;
        }
    }
}
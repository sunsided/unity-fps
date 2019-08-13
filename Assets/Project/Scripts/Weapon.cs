﻿using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Models")]
    public ObjectPool bulletPool;
    public Transform muzzle;
    public bool isPlayer;

    [Header("Ammo")]
    public int currentAmmo;
    public int maxAmmo;
    public bool infiniteAmmo;

    [Header("Shooting")]
    public float bulletSpeed;
    public float shootRate;

    private float _lastShootTime;

    public bool CanShoot()
    {
        if (Time.time - _lastShootTime < shootRate) return false;
        return currentAmmo > 0 || infiniteAmmo;
    }

    public void Shoot()
    {
        _lastShootTime = Time.time;
        --currentAmmo;

        var bullet = bulletPool.GetObject();
        bullet.transform.position = muzzle.position;
        bullet.transform.rotation = muzzle.rotation;

        bullet.GetComponent<Rigidbody>().velocity = muzzle.forward * bulletSpeed;
    }

    private void Awake()
    {
        // Are we attached to the player?
        isPlayer = GetComponent<Player>();
    }
}

﻿using UnityEngine;

public class RayGun : GunParent
{
    public LineRenderer gunLine;

    public float effectsDisplayTime = 0.2f; // how long lineRenderer will stay in a scene
    int shootableMask; //enemy or should be shootable
    public AudioSource aud;

    private static GameObject hitParticle = null;
    private static GameObject flashParticle = null;
    private static GameObject ChargedLaser = null;
    private static GameObject ChargingLaserParticle = null;


    private void Start()
    {
        //gunLine = GetComponent<LineRenderer>();
        if (hitParticle == null)
            hitParticle = Resources.Load("Particle_LaserHit", typeof(GameObject)) as GameObject;

        if (flashParticle == null)
            flashParticle = Resources.Load("Particle_LaserFlash", typeof(GameObject)) as GameObject;

        if (ChargedLaser == null)
            ChargedLaser = Resources.Load("GO_ChargedLaser", typeof(GameObject)) as GameObject;

        if (ChargingLaserParticle == null)
            ChargingLaserParticle = Resources.Load("Particle_LaserCharging", typeof(GameObject)) as GameObject;
    }

    public override void Update()
    {
        base.Update();
        if (timer >= reFireTime * effectsDisplayTime)
        {
            DisableEffects();
        }

        if (isCharging)
        {
            chargingLaser += Time.deltaTime;
            GameObject goCharingLaserParticle = (GameObject)Instantiate(ChargingLaserParticle, transform.position, transform.rotation);
            Destroy(goCharingLaserParticle, 0.1f);
        }
        else
            chargingLaser = 0.0f;
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
    }

    protected override void Shoot()
    {
        base.Shoot();


        if (chargingLaser < minChargingLaser && isCharging)
        {
            NormalShoot();
        }

        if (chargingLaser > minChargingLaser && isCharging)
        {
            ChargedLaserShoot();
        }

    }

    private void ChargedLaserShoot()
    {
        GameObject goChargedLaser = (GameObject)Instantiate(ChargedLaser, transform.position, transform.rotation);
    }

    private void NormalShoot()
    {
        //FlashLight
        GameObject goFlash = (GameObject)Instantiate(flashParticle, transform.position, transform.rotation);
        Destroy(goFlash, 0.22f);


        aud.Play();
        gunLine.enabled = true;
        Transform shotTransform = transform;
        gunLine.SetPosition(0, shotTransform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            gunLine.SetPosition(1, hit.point);
            InstantiateParticle(hit);

            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<Enemy>().TakeDamage(damage);
            }
            else if (hit.transform.tag == "Boss")
            {
                hit.transform.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
        else
        {
            gunLine.SetPosition(1, shotTransform.position + shotTransform.forward * range);
        }
    }

    private void InstantiateParticle(RaycastHit hit)
    {
        GameObject tempParticle = (GameObject)Instantiate(hitParticle, hit.point, hit.transform.rotation);
        tempParticle.transform.LookAt(transform, Vector3.up);
        tempParticle.SetActive(true);
        Destroy(tempParticle, 1.4f);
    }

}
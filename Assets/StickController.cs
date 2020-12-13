using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    public enum StickModes
    {
        Configuration, ShootAnimation, Shoot, Shooted
    }

    public StickModes mode;
    public Transform target;
    public float speed = 10;
    public float maxTranslation = 10f;
    private float totalTranslation = 0f;
    private float shootSpeed = 0f;

    void Update()
    {
        switch (mode)
        {
            case StickModes.Configuration:
                UpdateMovement();
                break;
            case StickModes.ShootAnimation:
                AnimateShoot();
                break;
            case StickModes.Shoot:
                Shoot();
                break;
        }
    }

    private void UpdateMovement()
    {
        float rotation = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float translation = Input.GetAxis("Vertical") * Time.deltaTime;

        transform.RotateAround(target.position, target.forward, rotation);

        var nextTranslation = translation;
        if (totalTranslation + translation >= 0)
            nextTranslation = totalTranslation * (-1);
        else if (totalTranslation + translation <= maxTranslation)
            nextTranslation = maxTranslation - totalTranslation;

        totalTranslation += nextTranslation;
        transform.Translate(0, 0, nextTranslation);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            shootSpeed = totalTranslation * (-1) * 10;
            mode = StickModes.ShootAnimation;
        }
    }

    private void AnimateShoot()
    {
        float translation = speed * Time.deltaTime;
        var nextTranslation = translation;
        if (totalTranslation + translation >= 0)
            nextTranslation = totalTranslation * (-1);

        totalTranslation += nextTranslation;
        transform.Translate(0, 0, nextTranslation);

        if (totalTranslation == 0f)
            mode = StickModes.Shoot;
    }

    private void Shoot()
    {
        var relativePosition = new Vector3(transform.position.x, target.transform.position.y, transform.position.z);
        var shootForce = (target.transform.position - relativePosition).normalized * (shootSpeed) * 50 * target.GetComponent<Rigidbody>().mass;
        target.GetComponent<Rigidbody>().AddForce(shootForce);
        mode = StickModes.Shooted;
    }

}

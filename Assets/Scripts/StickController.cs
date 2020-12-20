using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    public enum StickModes
    {
        Configuration, ShootAnimation, Shoot, Shooted
    }

    public Transform StickModel;
    public StickModes mode;
    public Transform target;
    public float speed = 10;
    public float maxTranslation = 10f;
    private float totalTranslation = 0f;
    private float shootSpeed = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

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
            case StickModes.Shooted:
                Wait();
                break;
        }
    }

    private void UpdateMovement()
    {
        float rotation = (Input.GetAxis("Mouse X") + Input.GetAxis("Horizontal")) * speed * Time.deltaTime * 10;
        float translation = (Input.GetAxis("Mouse Y") + Input.GetAxis("Vertical")) * Time.deltaTime * 10;

        StickModel.RotateAround(target.position, target.forward, rotation);

        var nextTranslation = translation;
        if (totalTranslation + translation >= 0)
            nextTranslation = totalTranslation * (-1);
        else if (totalTranslation + translation <= maxTranslation)
            nextTranslation = maxTranslation - totalTranslation;

        totalTranslation += nextTranslation;
        StickModel.Translate(0, 0, nextTranslation);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
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
        StickModel.Translate(0, 0, nextTranslation);

        if (totalTranslation == 0f)
            mode = StickModes.Shoot;
    }

    private void Shoot()
    {
        var relativePosition = new Vector3(StickModel.position.x, target.transform.position.y, StickModel.position.z);
        var shootForce = (target.transform.position - relativePosition).normalized * (shootSpeed) * 50 * target.GetComponent<Rigidbody>().mass;
        StickModel.gameObject.SetActive(false);
        target.GetComponent<Rigidbody>().AddForce(shootForce);
        mode = StickModes.Shooted;
    }

    private void Wait()
    {
        
    }

    public void ShowStick()
    {
        transform.position = target.transform.position;
        StickModel.gameObject.SetActive(true);
        mode = StickModes.Configuration;
    }
}

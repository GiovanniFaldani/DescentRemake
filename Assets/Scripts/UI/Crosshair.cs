using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Color hittableColor;
    private RawImage crosshairSprite;
    private Color baseColor;

    void Start()
    {
        crosshairSprite = GetComponent<RawImage>();
        baseColor = crosshairSprite.color;
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemy();
    }

    //check raycast to see if enemy is in crosshairs and turn red
    private void CheckEnemy()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, float.MaxValue))
        {
            if (hitData.collider.CompareTag("Enemy"))
            {
                ChangeOnHittable();
            }
            else
            {
                ReturnNeutral();
            }
        }
        else
        {
            ReturnNeutral();
        }
    }

    private void ChangeOnHittable()
    {
        crosshairSprite.color = hittableColor;
    }

    private void ReturnNeutral()
    {
        crosshairSprite.color = baseColor;
    }
}

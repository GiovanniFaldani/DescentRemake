using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Player playerRef;
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
        // TODO check raycast to see if enemy is in crosshairs and turn red
    }

    private void ChangeOnHittable(Color hittableColor)
    {
        crosshairSprite.color = hittableColor;
    }

    private void ReturnNeutral()
    {
        crosshairSprite.color = baseColor;
    }
}

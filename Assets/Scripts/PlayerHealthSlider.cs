using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    GameObject parant;

    private void Start()
    {
        parant = transform.parent.gameObject;
        slider.maxValue = parant.GetComponent<Stats>().maxHealth;
        
    }

    private void Update()
    {
        slider.value = parant.GetComponent<Stats>().Health;
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}

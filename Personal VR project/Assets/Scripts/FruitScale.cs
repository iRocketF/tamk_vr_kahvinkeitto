using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FruitScale : MonoBehaviour
{
    public TextMeshPro scaleText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fruit"))
        {
            FruitResizer currentFruit = other.gameObject.GetComponent<FruitResizer>();

            scaleText.text = currentFruit.weight.ToString();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Fruit"))
        {
            scaleText.text = "0";
        }
    }
}

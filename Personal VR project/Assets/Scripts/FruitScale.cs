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
        scaleText.text = "0g";
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

            if(currentFruit == null)
                currentFruit = other.gameObject.GetComponentInParent<FruitResizer>();

            if(!currentFruit.isCollected)
            {
                scaleText.text = currentFruit.weight.ToString() + "g";
                currentFruit.isWeighted = true;
                currentFruit.isCollected = true;
            }


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Fruit"))
        {
            scaleText.text = "0g";
        }
    }
}

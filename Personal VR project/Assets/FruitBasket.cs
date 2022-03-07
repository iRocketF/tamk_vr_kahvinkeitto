using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FruitBasket : MonoBehaviour
{
    public List<string> basketContent;

    public TextMeshProUGUI currentBasket;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        currentBasket.text = string.Join(", ", basketContent );
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Fruit"))
        {
            FruitResizer currentFruit = other.gameObject.GetComponent<FruitResizer>();

            if (currentFruit == null)
                currentFruit = other.gameObject.GetComponentInParent<FruitResizer>();

            if (currentFruit.isWeighted)
                basketContent.Add((currentFruit.gameObject.name + " " + currentFruit.weight.ToString() + "g"));
        }

    }
}

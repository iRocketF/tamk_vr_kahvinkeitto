using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitResizer : MonoBehaviour
{
    private GameObject fruit;
    private Vector3 scale;

    public float maxSizeModifier;
    public float minSizeModifier;
    private float sizeModifier;

    public float weight;
    public bool isWeighted;

    // Start is called before the first frame update
    void Start()
    {
        fruit = gameObject;
        sizeModifier = Random.Range(minSizeModifier, maxSizeModifier);
        scale = (fruit.transform.localScale * sizeModifier);
        fruit.transform.localScale = scale;

        weight = Mathf.RoundToInt(weight * sizeModifier);
        
    }

}

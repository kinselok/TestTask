using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Column : MonoBehaviour
{
    public event Action<Column, int[]> SpinEnd;
    public int RowsVisible { get; set; }

    #region Editor
    [SerializeField]
    private float elementsStep;
    [SerializeField]
    private int minElementsForSpin;
    [SerializeField]
    private int maxElementsForSpin;
    [SerializeField]
    private float spinStep;
    [Header("It's automatically filled with child elements if there are no elements here. ")]
    [SerializeField]
    private Element[] elements;
    #endregion

    private Vector3 startPosition;
    //Index of the element that is now in the first position
    [SerializeField]
    private int currentElement;
    [SerializeField]
    private float minPosition;

    private void Start()
    {
        SlotInput.BaseInput.Play += StartSpin;

        elements = GetComponentsInChildren<Element>();
        QSort(new List<Element>(elements));
        
        startPosition = transform.position;

        if(RowsVisible == 0)
            Debug.LogError("RowsVisible isn't initialized yet. Change execution order or initialize it in Awake method");
        minPosition = startPosition.y - elements.Length * elementsStep + RowsVisible * elementsStep;
        

        //set random position
        ChangeCurrentElement(UnityEngine.Random.Range(minElementsForSpin, maxElementsForSpin));
        AlignAccordingCurrentElement();
    }


    #region Initialization stuff
    private List<Element> QSort(List<Element> list)
    {
        if(list.Count < 2)
            return list;
        else
        {
            var pivot = list[0];
            list.RemoveAt(0);
            var less = new List<Element>();
            var greater = new List<Element>();

            foreach(var elem in list)
            {
                if(elem.transform.position.y <= pivot.transform.position.y)
                    less.Add(elem);
                else
                    greater.Add(elem);
            }

            var result = new List<Element>();
            result.AddRange(QSort(less));
            result.Add(pivot);
            result.AddRange(QSort(greater));
            return result;
        }
    }
    #endregion


    void ChangeCurrentElement(int i)
    {
        if(i + currentElement >= elements.Length - RowsVisible)
        {
            currentElement = (i + currentElement) % (elements.Length - RowsVisible);
        }
        else
            currentElement += i;
    }


    private void StartSpin()
    {
        StartCoroutine(Spin());
    }


    IEnumerator Spin()
    {
        int elementsForSpin = UnityEngine.Random.Range(minElementsForSpin, maxElementsForSpin);
        var totalDistance = elementsForSpin * elementsStep;
        while(totalDistance > 0)
        {
            yield return new WaitForFixedUpdate();
            totalDistance -= spinStep;

            transform.position -= new Vector3(0, spinStep, 0);

            if(transform.position.y < minPosition)
                transform.position = startPosition;
        }
        ChangeCurrentElement(elementsForSpin);
        AlignAccordingCurrentElement();
        DisableElementsBlur();
        EndSpin();
    }


    void AlignAccordingCurrentElement()
    {
        var currentPos = transform.position;
        
        currentPos.y = startPosition.y - currentElement * elementsStep;

        transform.position = currentPos;
    }


    void DisableElementsBlur()
    {
        foreach(var element in elements)
            element.OnMovementStop();
    }


    void EndSpin()
    {
        int[] visibleElements = new int[RowsVisible];
        for(int i = 0; i < RowsVisible; i++)
            visibleElements[i] = elements[currentElement + i].ID;

        SpinEnd(this, visibleElements);
    }


    private void OnDestroy()
    {
        SlotInput.BaseInput.Play -= StartSpin;
    }
}


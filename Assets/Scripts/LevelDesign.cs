using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class LevelDesign : MonoBehaviour
{
    public GameObject dogDie;
    public int maxPointLineCanDraw = 200;
    private AstarPath astarPath;
    private GameObject hintLine;
    public List<CatController> listCat = new List<CatController>();
    
    private void Start()
    {
        hintLine = this.gameObject.transform.Find("HintLine").gameObject;
        astarPath = transform.Find("Path").GetComponent<AstarPath>();
        StartCoroutine(UpdateScan());
        
        if (DataGame.Instance.lvl_current == 1)
        {
            if (hintLine != null)
            {
                hintLine.SetActive(true);
            }
        }
        else
        {
            if (hintLine != null)
            {
                hintLine.SetActive(false);
            }
        }

        listCat.Clear();
        foreach (Transform cat in transform)
        {
            if (cat.GetComponent<CatController>() != null)
            {
                listCat.Add(cat.GetComponent<CatController>());
            }
        }
    }
    
    IEnumerator UpdateScan()
    {
        while (!GameController.Instance.b_EndGame)
        {
            astarPath.Scan();
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void ActiveHint()
    {
        GameController.Instance.btn_hint.gameObject.SetActive(false);
        hintLine.SetActive(true);
    }

    public void UnActiveHint()
    {
        if (hintLine != null)
        {
            hintLine.SetActive(false);
        }
    }

    public void DogDie()
    {
        Instantiate(dogDie, this.transform);
    }

    public void ActiveWin()
    {
        if (listCat == null)
        {
            Debug.LogError("list cat null");
            return;
        }
        foreach (CatController cat in listCat)
        {
            if (cat == null)
            {
                Debug.LogError("cat null");
                continue;
            }
            cat.RunAnimWin();
        }
    }
    
    public void ActiveLose()
    {
        if (listCat == null)
        {
            Debug.LogError("list cat null");
            return;
        }
        foreach (CatController cat in listCat)
        {
            if (cat == null)
            {
                Debug.LogError("cat null");
                continue;
            }
            cat.RunAnimLose();
        }
    }
}

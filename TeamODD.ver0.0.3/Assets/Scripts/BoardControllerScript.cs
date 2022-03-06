﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardControllerScript : MonoBehaviour
{
    public GameObject[] stampPrefabs;
    private GameObject stampPrefab;
    //private SpriteRenderer stampInjuTapObjRenderer;
    public Sprite[] stampSprite;
    
    public GameObject stampInjuAnimation;
    public GameObject injuAmount;

    public Text injuText;

    //GameObject stampInjuTap;
    GameObject stampInju;
    public GameObject stampInju2;

    public Sprite[] stampInjuClosedSprite;
    public Sprite[] stampInjuOpenSprite;

    public Sprite[] tableSprite;

    Vector2 mouseUpPosition;
    Vector2 mouseDownPosition;

    public bool stampTouch = false;
    public bool otherTouch = false;

    private bool waxOn = false;
    private bool stampInjuOpen = false;
    private bool stampInjuTouch = false;
    private int inkValue = 10;

    public int stamp;

    GameObject BackgroundIMG;
    private SpriteRenderer BackgroundIMGRenderer;
    private Sprite stamp_sprite;

    // Start is called before the first frame update
    void Start()
    {
        SaveData.DoLoadData = true;
        SaveData.Loads();
        injuText.text = "x10";
        //stampInjuTap = GameObject.Find("StampInjuTapObj");
        BackgroundIMG = GameObject.Find("BackgroundIMG");
        //stampInjuTap.GetComponent<SpriteRenderer>().enabled = false;
        stampPrefab = stampPrefabs[SaveData.ListNum_S];

        //stampInjuTapObjRenderer = stampInjuTap.GetComponent<SpriteRenderer>();
        //stampInjuTapObjRenderer.sprite = stampSprite[SaveData.ListNum_S];

        BackgroundIMGRenderer = BackgroundIMG.GetComponent<SpriteRenderer>();
        BackgroundIMGRenderer.sprite = tableSprite[SaveData.ListNum_T];

        stampInju2.GetComponent<SpriteRenderer>().sprite = stampInjuClosedSprite[SaveData.ListNum_I];
    }

    // Update is called once per frame
    void Update()
    {
        SaveData.Loads();
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

        if (Input.GetMouseButtonDown(0))
        {
            //stamp board click 도장 생성
            if (hit.transform.gameObject.tag == "StampBoard")
            {
                if (waxOn == false)
                {
                    if (inkValue > 0)
                    {
                        if (stampTouch == false)
                        {
                            inkValue--;
                            injuText.text = "x" + inkValue;
                       
                            Instantiate(stampPrefab, new Vector2(touchPos.x, touchPos.y), Quaternion.identity);
                            SoundManager.soundManager.StampTapPlaySound();

                            if (hit.transform.gameObject.name == "StampBoardTap")
                            {
                                if (otherTouch == false)
                                {
                                    GameObject.Find("GameController").GetComponent<GeneratorControllerScript>().Score();
                                }
                            }
                            else
                            {
                                otherTouch = true;

                            }
                        }
                    }

                    else
                    {
                        SoundManager.soundManager.StampEmptyPlaySound();
                    }
                }

            }

            //수정 *********

            //stamp inju click
            if (hit.transform.gameObject.name == "StampInjuObj")
            {
                //inju fill
                if (stampInjuOpen == true)
                {
                    inkValue = 10;
                    injuText.text = "x" + inkValue;
                    stampInju = hit.transform.gameObject;
                    //stampInjuTap.GetComponent<SpriteRenderer>().enabled = true;
                    stampInjuOpen = false;
                    Invoke("InjuClose", 0.3f);
                }
                else
                {
                    mouseDownPosition = touchPos;
                    stampInjuTouch = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //stamp inju click
            if (stampInjuTouch == true)
            {
                mouseUpPosition = touchPos;

                //inju open
                if (mouseUpPosition.y > mouseDownPosition.y + 0.3f)
                {
                    if (hit.transform.gameObject.name == "StampInjuObj")
                    {
                        SoundManager.soundManager.INZ_1PlaySound();
                        //hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/StampInjuFullImg");
                        hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite = stampInjuOpenSprite[SaveData.ListNum_I];
                        stampInjuOpen = true;
                        stampInjuTouch = false;
                    }
                }

            }

            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    mouseUpPosition = touchPos;

                    //inju open
                    if (mouseUpPosition.y > mouseDownPosition.y + 0.3f)
                    {
                        if (hit.transform.gameObject.name == "StampInjuObj")
                        {
                            SoundManager.soundManager.INZ_1PlaySound();
                            //hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/StampInjuFullImg");
                            hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite = stampInjuOpenSprite[SaveData.ListNum_I];
                            stampInjuOpen = true;
                            stampInjuTouch = false;
                        }
                    }
                }
            }

        }

        injuAmount.GetComponent<Image>().fillAmount = ((float)inkValue / 10);
    }

    //인주 닫기
    public void InjuClose()
    {
        SaveData.Loads();
        SoundManager.soundManager.INZ_2PlaySound();
       // stampInjuTap.GetComponent<SpriteRenderer>().enabled = false;
        //stampInju.transform.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/StampInjuCloseImg");
        stampInju.transform.gameObject.GetComponent<SpriteRenderer>().sprite = stampInjuClosedSprite[SaveData.ListNum_I];
        //Instantiate(stampInjuAnimation, new Vector2(stampInjuTap.transform.position.x, stampInjuTap.transform.position.y), Quaternion.identity);
        Destroy(GameObject.Find(stampInjuAnimation.name + "(Clone)"), 0.2f);

    }

    public void StampBoardOnOff()
    {
        if (stampTouch == true)
        {
            stampTouch = false;
        }
        else if (stampTouch == false)
        {
            stampTouch = true;
        }
    }

    public void StampOff()
    {
        waxOn = true;
    }

    public void StampOn()
    {
        waxOn = false;
    }

    public void StampBoardReset()
    {
        Debug.Log("23");

        otherTouch = false;
    }
}

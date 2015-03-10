using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SelectCharacter : MonoBehaviour
{
    //0:白色，1：红色，2：绿色，3：蓝色，4：黑色
    public List<UIButton> CardList;

    public string Card="";
    public UIButton EnterButton;
    public SelectCardMainView MainView;

    void Awake()
    {
        MainView = GameObject.Find("MainView").GetComponent<SelectCardMainView>();

        EventDelegate.Add(CardList[0].onClick, OnClickWhite);
        EventDelegate.Add(CardList[1].onClick, OnClickRed);
        EventDelegate.Add(CardList[2].onClick, OnClickGreen);
        EventDelegate.Add(CardList[3].onClick, OnClickBlue);
        EventDelegate.Add(CardList[4].onClick, OnClickBlack);

        EventDelegate.Add(EnterButton.onClick, OnClickEnterBtn);
    }
    //选择白色主角
    void OnClickWhite()
    {
        Card = "White";
    }
    //选择红色主角
    void OnClickRed()
    {
        Card = "Red";
    }
    //选择绿色主角
    void OnClickGreen()
    {
        Card = "Green";
    }
    //选择蓝色主角
    void OnClickBlue()
    {
        Card = "Blue";
    }
    //选择黑色主角
    void OnClickBlack()
    {
        Card = "Black";
    }
    //点击确定按钮
    void OnClickEnterBtn()
    {
        Debug.Log("00000"+Card);
        if (string.IsNullOrEmpty(Card))
        {
            Debug.Log("需要选择初始卡牌颜色");
        }
        else
        {
            switch (Card)
            {
                case "Red":
                    Debug.Log("have select Red");
                    break;
                case "White":
                    Debug.Log("have select white");
                    break;
                case "Blue":
                    Debug.Log("have select Blue");
                    break;
                case "Green":
                    Debug.Log("have select Green");
                    break;
                case "Black":
                    Debug.Log("have select Black");
                    break;
            }
            MainView.OpenView(MainView.SelectFenShen);
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

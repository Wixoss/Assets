using UnityEngine;
using System.Collections;

public class SelectMainCard : MonoBehaviour {
    public UIButton SortSpriteBtn;                   //分身分类按钮
    public UIButton SortJiYiBtn;                      //记忆卡分类按钮
    public UIButton LeftBtn;                          //左边按钮
    public UIButton RightBtn;                         //右边按钮
    public UIButton EnterBtn;                         //确定按钮
    public SelectCardMainView MainView;
    void Awake()
    {
        MainView = GameObject.Find("MainView").GetComponent<SelectCardMainView>();
        EventDelegate.Add(SortSpriteBtn.onClick, OnClickFenShen);
        EventDelegate.Add(SortJiYiBtn.onClick, OnClickJiYi);
        EventDelegate.Add(LeftBtn.onClick, OnClickLeftBtn);
        EventDelegate.Add(RightBtn.onClick, OnClickRightBtn);
        EventDelegate.Add(EnterBtn.onClick, OnClickEnterBtn);
    }

    //点击分身分类按钮
    void OnClickFenShen()
    {
        Debug.Log("点击分身按钮");
    }
    //点击记忆分类按钮
    void OnClickJiYi()
    {
        Debug.Log("点击记忆按钮");
    }
    //点击左边按钮
    void OnClickLeftBtn()
    {
        Debug.Log("点击左边按钮");
    }
    //点击右边按钮
    void OnClickRightBtn()
    {
        Debug.Log("点击右边按钮");
    }
    //点击确定按钮
    void OnClickEnterBtn()
    {
        Debug.Log("点击确定");
    }
    // Use this for initialization
    void Start()
    {
     
    }
}

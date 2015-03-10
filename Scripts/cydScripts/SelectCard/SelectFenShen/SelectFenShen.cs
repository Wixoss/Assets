using UnityEngine;
using System.Collections;

public class SelectFenShen : MonoBehaviour
{
    public UIButton SortFenShenBtn;                   //分身分类按钮
    public UIButton SortMagicBtn;                      //记忆卡分类按钮
    public UIButton LeftBtn;                          //左边按钮
    public UIButton RightBtn;                         //右边按钮
    public UIButton EnterBtn;                         //确定按钮
    public SelectCardMainView MainView;
    void Awake()
    {
        MainView = GameObject.Find("MainView").GetComponent<SelectCardMainView>();
        EventDelegate.Add(SortFenShenBtn.onClick, OnClickFenShen);
        EventDelegate.Add(SortMagicBtn.onClick, OnClickJiYi);
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
        Debug.Log("点击魔法按钮");
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
        MainView.OpenView(MainView.SelectMainCard);
    }
	// Use this for initialization
	void Start () {
	
	}
}

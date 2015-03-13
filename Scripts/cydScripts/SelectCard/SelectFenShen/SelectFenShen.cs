using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
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
    public Card.CardType MyType=Card.CardType.分身卡;

    public List<GameObject> ShowCards;
    public List<CardItem> ShowCardItems; 
    public int LBeginPage;
    public int LEndPage;
    public int ABeginPage;
    public int AEndPage;
    void Awake()
    {
        MainView = GameObject.Find("MainView").GetComponent<SelectCardMainView>();
        EventDelegate.Add(SortFenShenBtn.onClick, OnClickFenShen);
        EventDelegate.Add(SortMagicBtn.onClick, OnClickJiYi);
        EventDelegate.Add(LeftBtn.onClick, OnClickLeftBtn);
        EventDelegate.Add(RightBtn.onClick, OnClickRightBtn);
        EventDelegate.Add(EnterBtn.onClick, OnClickEnterBtn);

        for (int i = 0; i < ShowCards.Count; i++)
        {
            UIEventListener.Get(ShowCards[i]).onClick = OnClickCard;
        }

        LBeginPage = 0;
        LEndPage = ShowCards.Count;
        ABeginPage = 0;
        AEndPage = ShowCards.Count;
    }
    //设置分身卡牌的显示
    public void SetShowLCard(int _BeginPage, int _EndPage)
    {
        Debug.LogError("count"+DataSource.LrigCards.Count);
        int j = 0;
        for (int i = _BeginPage; i < _EndPage; i++)
        {
            
            if (i < DataSource.LrigCards.Count)
            {
                
                Debug.LogError("begin"+_BeginPage);
                Debug.LogError("end"+_EndPage);
                Debug.LogError("======"+i);
                ShowCards[j].GetComponent<UITexture>().mainTexture =Resources.Load(DataSource.LrigCards[i])as Texture2D;
                ShowCardItems[j]._Detial = DataSource.GetDetialById(DataSource.LrigCards[i]);
                ShowCardItems[j]._Cardname = DataSource.GetNameById(DataSource.LrigCards[i]);
                ShowCardItems[j]._CardId = DataSource.LrigCards[i];
            }
            else
            {
                ShowCards[j].GetComponent<UITexture>().mainTexture = Resources.Load("") as Texture2D;
                ShowCardItems[j]._Detial = "";
                ShowCardItems[j]._Cardname = "";
                ShowCardItems[j]._CardId = "";
            }
            j++;
        }
    }
    //设置技艺卡牌的显示
    public void SetShowACard(int _BeginPage, int _EndPage)
    {
        Debug.LogError("count" + DataSource.ArtCards.Count);
        int j = 0;
        for (int i = _BeginPage; i < _EndPage; i++)
        {

            if (i < DataSource.ArtCards.Count)
            {

                Debug.LogError("begin" + _BeginPage);
                Debug.LogError("end" + _EndPage);
                Debug.LogError("======" + i);
                ShowCards[j].GetComponent<UITexture>().mainTexture = Resources.Load(DataSource.ArtCards[i]) as Texture2D;
                ShowCardItems[j]._Detial = DataSource.GetDetialById(DataSource.LrigCards[i]);
                ShowCardItems[j]._Cardname = DataSource.GetNameById(DataSource.LrigCards[i]);
                ShowCardItems[j]._CardId = DataSource.LrigCards[i];
            }
            else
            {
                ShowCards[j].GetComponent<UITexture>().mainTexture = Resources.Load("") as Texture2D;
                ShowCardItems[j]._Detial = "";
                ShowCardItems[j]._Cardname = "";
                ShowCardItems[j]._CardId = "";
            }
            j++;
        }
    }

    //点击卡片出现的效果
    public void OnClickCard(GameObject go)
    {
        
    }
    //点击分身分类按钮
    void OnClickFenShen()
    {
        MyType=Card.CardType.分身卡;
        SetShowLCard(LBeginPage, LEndPage);
        Debug.Log("点击分身按钮");
    }
    //点击记忆分类按钮
    void OnClickJiYi()
    {
        MyType=Card.CardType.技艺卡;
        SetShowACard(ABeginPage, AEndPage);
        Debug.Log("点击技艺按钮");
    }
    //点击左边按钮
    void OnClickLeftBtn()
    {
        switch (MyType)
        {
                case Card.CardType.分身卡:
                if (LBeginPage <= 0)
                {
                    return;
                }
                else
                {
                    LBeginPage -= ShowCards.Count;
                    LEndPage -= ShowCards.Count;
                    SetShowLCard(LBeginPage, LEndPage);
                }
                break;
                case Card.CardType.技艺卡:
                if (ABeginPage <= 0)
                {
                    return;
                }
                else
                {
                    ABeginPage -= ShowCards.Count;
                    AEndPage -= ShowCards.Count;
                    SetShowACard(ABeginPage, AEndPage);
                }
                break;
        }
        
        Debug.Log("点击左边按钮");
    }
    //点击右边按钮
    void OnClickRightBtn()
    {
        switch (MyType)
        {
                case Card.CardType.分身卡:
                if (LEndPage >= DataSource.LrigCards.Count)
                {
                    return;
                }
                else
                {
                    LBeginPage += ShowCards.Count;
                    LEndPage += ShowCards.Count;
                    SetShowLCard(LBeginPage, LEndPage);
                }
                break;
                case Card.CardType.技艺卡:
                if (AEndPage >= DataSource.ArtCards.Count)
                {
                    return;
                }
                else
                {
                    ABeginPage += ShowCards.Count;
                    AEndPage += ShowCards.Count;
                    SetShowACard(ABeginPage, AEndPage);
                }
                break;
        }
        
        Debug.Log("点击右边按钮");
    }
    //点击确定按钮
    void OnClickEnterBtn()
    {
        Debug.Log("点击确定");
        MainView.OpenView(MainView.SelectMainCard);
    }
	// Use this for initialization
	void Start ()
	{
        SetShowLCard(LBeginPage, LEndPage);
	}
}

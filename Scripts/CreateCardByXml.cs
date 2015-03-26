using System;
using System.IO.IsolatedStorage;
using UnityEngine;

namespace Assets.Scripts
{
    public class CreateCardByXml : MonoBehaviour
    {
        //        public static string Path =
        //#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        // Application.streamingAssetsPath + "/Xml/Wixoss_Project.xml";
        //#elif UNITY_STANDALONE_OSX
        //		Application.streamingAssetsPath + "/Xml/Wixoss_Project.xml";
        //#endif
        //
        //        public static string MyXml;
        //        public static XElement LoadFromXml()
        //        {
        //            XElement root = XElement.Load(Path);
        //            return root;
        //        }
        //
        //        /// <summary>
        //        /// 通过cardid来获取card的信息
        //        /// </summary>
        //        /// <returns>The card by card identifier.</returns>
        //        /// <param name="cardid">Cardid.</param>
        //        public static IEnumerable<XElement> GetCardByCardId(string cardid)
        //        {
        //            var root = LoadFromXml();
        //            var element = root.Elements();
        //            var card = from i in element where i.Element("CardId").Value == cardid select i;
        //            return card;
        //        }

        //        public static System.Collections.IEnumerator GetXml()
        //        {
        //            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        //            {
        //                Path = Application.streamingAssetsPath + "/Xml/Wixoss_Project.xml";
        //            }
        //            else
        //            {
        //                Path = "file://" + Application.streamingAssetsPath + "/Xml/Wixoss_Project.xml";
        //            }
        //
        //            var www = new WWW(Path);
        //            while (!www.isDone)
        //            {
        //                yield return www;
        //                MyXml = www.text;
        //                Debug.Log(www.text);
        //            }
        //        }
        //
        //        public static XmlDocument LoadFromXml(string text)
        //        {
        //            XmlDocument xmlDoc = new XmlDocument();
        //            xmlDoc.LoadXml(text);
        //            return xmlDoc;
        //        }
        //
        //        public static IEnumerable<XElement> GetCardByCardid(string cardid)
        //        {
        //            var xmlDoc = LoadFromXml(MyXml);
        //            IEnumerable<XElement> myXElement = null;
        //            XmlNodeList detail = xmlDoc.SelectSingleNode("Wixoss").ChildNodes;
        //
        //            foreach (IEnumerable<XElement> i in detail)
        //            {
        //                if (i.Elements().Any(x => x.Element("CardId").Value == cardid))
        //                {
        //                    myXElement = i;
        //                }
        //            }
        //            return myXElement;
        //        }

        public TextAsset MyXmlAll;
        private XMLNodeList _myLrigList;
        private XMLNodeList _myArtList;
        private XMLNodeList _mySigniList;
        private XMLNodeList _mySpellList;

        private XMLNode _myXmlNode;

        public void GetXml()
        {
            var xmlPer = new XMLParser();
            _myXmlNode = xmlPer.Parse(MyXmlAll.text);
            _myLrigList = _myXmlNode.GetNodeList("ROOT>0>Lrig");
            _myArtList = _myXmlNode.GetNodeList("ROOT>0>Art");
            _mySigniList = _myXmlNode.GetNodeList("ROOT>0>Signi");
            _mySpellList = _myXmlNode.GetNodeList("ROOT>0>Spell");
        }

        public void GetCardDetailFromXml(Card card, int min, int max)
        {
            XMLNodeList myXmlNodeList = new XMLNodeList();
            string type = "";
            bool bSelect = false;

            min = min + 1;
            switch (min)
            {
                case 1:
                    myXmlNodeList = _myLrigList;
                    type = "Lrig>";
                    break;
                case 2:
                    myXmlNodeList = _myArtList;
                    type = "Art>";
                    break;
                case 3:
                    myXmlNodeList = _mySigniList;
                    type = "Signi>";
                    break;
                case 4:
                    myXmlNodeList = _mySpellList;
                    type = "Spell>";
                    break;
            }

            for (int i = 0; i < myXmlNodeList.Count; i++)
            {
                if (_myXmlNode.GetValue("ROOT>0>" + type + i + ">@CardId") == card.CardId)
                {
                    bSelect = true;
					card.CardName = _myXmlNode.GetValue("ROOT>0>" + type + i + ">@CardName");
                    card.MyCardColor = card.GetCardColorByString(_myXmlNode.GetValue("ROOT>0>" + type + i + ">@Color"));
                    card.MyCardType = card.GetCardTypeByString(_myXmlNode.GetValue("ROOT>0>" + type + i + ">@CardType"));
                    card.MyEner.MyEnerType = card.GetEnerTypeByString(_myXmlNode.GetValue("ROOT>0>" + type + i + ">@Color"));
                    if (_myXmlNode.GetValue("ROOT>0>" + type + i + ">@Level") != null)
                    {
                        card.Level = Convert.ToInt16(_myXmlNode.GetValue("ROOT>0>" + type + i + ">@Level"));
                    }
                    if (_myXmlNode.GetValue("ROOT>0>" + type + i + ">@BBrust") != null)
                    {
                        card.HasBrust = Convert.ToInt16(_myXmlNode.GetValue("ROOT>0>" + type + i + ">@BBrust")) != 0;
                    }
                    if (_myXmlNode.GetValue("ROOT>0>" + type + i + ">@CardDetail") != null)
                    {
                        card.CardDetail = _myXmlNode.GetValue("ROOT>0>" + type + i + ">@CardDetail");
                    }
                    if (_myXmlNode.GetValue("ROOT>0>" + type + i + ">@EffectCost") != null)
                    {
                        card.Cost = card.GetCostByString(_myXmlNode.GetValue("ROOT>0>" + type + i + ">@EffectCost"));
                    }
                    if (_myXmlNode.GetValue("ROOT>0>" + type + i + ">@EffectCost_Qi") != null)
                    {
                        card.EffectCostQi = card.GetCostByString(_myXmlNode.GetValue("ROOT>0>" + type + i + ">@EffectCost_Qi"));
                    }
                    if (_myXmlNode.GetValue("ROOT>0>" + type + i + ">@EffectCost_Chu") != null)
                    {
                        card.EffectCostChu = card.GetCostByString(_myXmlNode.GetValue("ROOT>0>" + type + i + ">@EffectCost_Chu"));
                    }
                    if (_myXmlNode.GetValue("ROOT>0>" + type + i + ">@GrowCost") != null)
                    {
                        card.GrowCost = card.GetCostByString(_myXmlNode.GetValue("ROOT>0>" + type + i + ">@GrowCost"));
                    }
                    if (_myXmlNode.GetValue("ROOT>0>" + type + i + ">@Type") != null)
                    {
                        card.Type = _myXmlNode.GetValue("ROOT>0>" + type + i + ">@Type");
                    }
                    if (_myXmlNode.GetValue("ROOT>0>" + type + i + ">@Limit") != null)
                    {
                        card.Limit = Convert.ToInt16(_myXmlNode.GetValue("ROOT>0>" + type + i + ">@Limit"));
                    }
                    if (_myXmlNode.GetValue("ROOT>0>" + type + i + ">@Timing") != null)
                    {
                        card.MyTiming = card.GetTimingByString(_myXmlNode.GetValue("ROOT>0>" + type + i + ">@Timing"));
                    }
                    if (_myXmlNode.GetValue("ROOT>0>" + type + i + ">@TypeOnly") != null)
                    {
                        card.TypeOnly = _myXmlNode.GetValue("ROOT>0>" + type + i + ">@TypeOnly");
                    }
                    if (_myXmlNode.GetValue("ROOT>0>" + type + i + ">@BDef") != null)
                    {
                        card.BCanGuard = Convert.ToInt16(_myXmlNode.GetValue("ROOT>0>" + type + i + ">@BDef")) != 0;
                    }
                    if (_myXmlNode.GetValue("ROOT>0>" + type + i + ">@Atk") != null)
                    {
                        card.BaseAtk = Convert.ToInt16(_myXmlNode.GetValue("ROOT>0>" + type + i + ">@Atk"));
                        card.Atk = card.BaseAtk;
                    }
                    card.MyEner.Num = 1;
                }
            }

            if (min < max && !bSelect)
            {
                GetCardDetailFromXml(card, min, max);
            }
        }
    }
}

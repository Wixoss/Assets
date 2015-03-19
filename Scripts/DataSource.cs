using System.Collections.Generic;

namespace Assets.Scripts
{
    public class DataSource
    {
        public static Player ClientPlayer;
        public static Player ServerPlayer;

        public static Deck ClientDeck;
        public static string ClientDeckName;
        public static Deck ServerDeck;
        public static string ServerDeckName;
        /// <summary>
        /// 主卡组
        /// </summary>
        public static List<Card> MainDeck = new List<Card>(); 

        /// <summary>
        /// 分身卡组
        /// </summary>
        public static List<Card> LrigDeck = new List<Card>();


        public static List<string> LrigCards = new List<string>();//分身卡组

        public static List<string> ArtCards = new List<string>(); //技艺卡组
        public static List<string> SigniCards = new List<string>();//精灵卡组
        public static List<string> SpellCards = new List<string>();//魔法卡组


        //根据id读取详情
        public static string GetDetialById(string CardId)
        {
            string detial = null;
            foreach (var item in CreateCardByXml.GetCardByCardId(CardId))
            {
                var ret = item.Element("CardDetail");
                if (ret != null) detial = ret.Value;
            }
            return detial;
        }
        //根据id读取名字
        public static string GetNameById(string CardId)
        {
            string name = null;
            foreach (var item in CreateCardByXml.GetCardByCardId(CardId))
            {
                var ret = item.Element("CardName");
                if (ret != null) name = ret.Value;
            }
            return name;
        }
    }
}

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
    }
}

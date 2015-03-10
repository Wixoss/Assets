namespace Assets.Scripts
{
    public class Player
    {
        public string PlayerIp;

        public string Name;

        public Deck MyDeck;

        /// <summary>
        /// 一个完整的回合包含的东西
        /// </summary>
//        public enum GameState
//        {
//            /// <summary>
//            /// 被冰冻的精灵在下个竖置阶段不能竖置
//            /// </summary>
//            竖置阶段,
//            /// <summary>
//            /// 先攻第一回合抽一张卡,其余抽2卡
//            /// </summary>
//            抽牌阶段,
//            /// <summary>
//            /// 只能把场上或者手牌上的一张卡充能,也可以跳过充能阶段
//            /// </summary>
//            充能阶段,
//            /// <summary>
//            /// 满足条件,支付足够费用后分身可以成长,或者跳过成长阶段
//            /// </summary>
//            成长阶段,
//            /// <summary>
//            /// 能使用魔法卡,技艺卡,精灵出场,发动效果等,要手动结束
//            /// </summary>
//            主要阶段,
//            /// <summary>
//            /// 做出攻击宣言,双方能使用[攻击时点]的技艺卡或魔法卡
//            /// </summary>
//            攻击宣言阶段,
//            /// <summary>
//            /// 精灵间的战斗,力量大于或等于其正对面的精灵时,可以把对方的精灵驱逐(放置到能量区),若正面没有精灵,则可以对对方造成伤害
//            /// </summary>
//            精灵攻击阶段,
//            /// <summary>
//            /// 当所有精灵都攻击结束后,分身发动攻击
//            /// </summary>
//            分身攻击阶段,
//            /// <summary>
//            /// 对方要从手卡上丢弃一张带有[防御]的卡,否则受到伤害
//            /// </summary>
//            防御阶段,
//            /// <summary>
//            /// 自己回合结束,若手牌大于6张时,要丢弃到6张,交换回合
//            /// </summary>
//            结束阶段,
//        }

        /// <summary>
        /// 是否该玩家的回合
        /// </summary>
        public bool BRound;

        /// <summary>
        /// 回合数
        /// </summary>
        public int RoundNum;

        /// <summary>
        /// 是否已经选择好卡组,进入准备阶段
        /// </summary>
        public bool BReady;

        /// <summary>
        /// 是否已经load完
        /// </summary>
        public bool BLoad;

        /// <summary>
        /// 是否选择好猜拳
        /// </summary>
        public bool BJyanKen;

        /// <summary>
        /// 猜拳的数字
        /// </summary>
        public int JyanKenNum;

        /// <summary>
        /// 猜拳是否胜出
        /// </summary>
        public bool BJyanKenWin;

        /// <summary>
        /// 是否先攻
        /// </summary>
        public bool BFirst;

        /// <summary>
        /// 准备阶段结束,游戏正式开始
        /// </summary>
        public bool BReadyPhaseEnd;

        /// <summary>
        /// 主卡组数量
        /// </summary>
        public int MainDeckNum;
        /// <summary>
        /// 废置区数量
        /// </summary>
        public int TrashNum;
        /// <summary>
        /// 生命护甲数量
        /// </summary>
        public int LifeClothNum;
        /// <summary>
        /// 能量区数量
        /// </summary>
        public int EnerNum;
        /// <summary>
        /// 手牌数量
        /// </summary>
        public int Hands;
        /// <summary>
        /// 分身卡组数量
        /// </summary>
        public int LrigDeckNum;
        /// <summary>
        /// 分身废置数量
        /// </summary>
        public int LrigTrashNum;
    }
}

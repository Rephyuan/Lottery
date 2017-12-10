using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Model.BetForm
{
    /// <summary>
    /// Summary description for Define
    /// </summary>
    public class Define
    {
        SqlConnection conn = new SqlConnection(GlobalVar.sql_con_str_main);

        public Define()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public class BetFormStatus
        {
            public const string Effective = "1";//有效
            public const string Invalid = "2";//無效 (投注金額不退回 wallet)
            public const string Violation = "3";// (投注金額退回 wallet)
            public const string Success = "4";//計算完成
        }

        public static Dictionary<string, string> BetFormStatusLangMap = new Dictionary<string, string>
        {
            {BetFormStatus.Effective, "BETFORM_STATUS_" + BetFormStatus.Effective},
            {BetFormStatus.Invalid, "BETFORM_STATUS_" + BetFormStatus.Invalid},
            {BetFormStatus.Violation, "BETFORM_STATUS_" + BetFormStatus.Violation},
            {BetFormStatus.Success, "BETFORM_STATUS_" + BetFormStatus.Success}
        };

        public class ReportTypes
        {
            public const string Member = "m";
            public const string BetDetail = "b";
        }

        public static Dictionary<string, string> ReportTypesLangMap = new Dictionary<string, string>
        {
            { ReportTypes.Member , "REPORT_TYPES_" + ReportTypes.Member },
            { ReportTypes.BetDetail , "REPORT_TYPES_" +  ReportTypes.BetDetail }
        };

        public class AllPassDefine
        {
            public const string Single = "01";
            public const string Double = "02";
            public const string Big = "03";
            public const string Small = "04";
            public const string Red = "05";
            public const string Blue = "06";
            public const string Green = "07";
        }
        public class PermutationAndCombination<T>
        {
            /// <summary>
            /// 交换两个变量
            /// </summary>
            /// <param name="a">变量1</param>
            /// <param name="b">变量2</param>
            public static void Swap(ref T a, ref T b)
            {
                T temp = a;
                a = b;
                b = temp;
            }
            /// <summary>
            /// 递归算法求数组的组合(私有成员)
            /// </summary>
            /// <param name="list">返回的范型</param>
            /// <param name="t">所求数组</param>
            /// <param name="n">辅助变量</param>
            /// <param name="m">辅助变量</param>
            /// <param name="b">辅助数组</param>
            /// <param name="M">辅助变量M</param>
            private static void GetCombination(ref List<T[]> list, T[] t, int n, int m, int[] b, int M)
            {
                for (int i = n; i >= m; i--)
                {
                    b[m - 1] = i - 1;
                    if (m > 1)
                    {
                        GetCombination(ref list, t, i - 1, m - 1, b, M);
                    }
                    else
                    {
                        if (list == null)
                        {
                            list = new List<T[]>();
                        }
                        T[] temp = new T[M];
                        for (int j = 0; j < b.Length; j++)
                        {
                            temp[j] = t[b[j]];
                        }
                        list.Add(temp);
                    }
                }
            }

            /// <summary>
            /// 求数组中n个元素的组合
            /// </summary>
            /// <param name="t">所求数组</param>
            /// <param name="n">元素个数</param>
            /// <returns>数组中n个元素的组合的范型</returns>
            public static List<T[]> GetCombination(T[] t, int n)
            {
                if (t.Length < n)
                {
                    return null;
                }
                int[] temp = new int[n];
                List<T[]> list = new List<T[]>();
                GetCombination(ref list, t, t.Length, n, temp, n);
                return list;
            }
        }

        public class BallColorNormalSpecialDefine
        {
            public const string Blue = "b1";
            public const string BlueSingle = "b2";
            public const string BlueDouble = "b3";
            public const string BlueBig = "b4";
            public const string BlueSmall = "b5";
            public const string BlueBigSingle = "b6";
            public const string BlueBigDouble = "b7";
            public const string BlueSmallSingle = "b8";
            public const string BlueSmallDouble = "b9";

            public const string Red = "r1";
            public const string RedSingle = "r2";
            public const string RedDouble = "r3";
            public const string RedBig = "r4";
            public const string RedSmall = "r5";
            public const string RedBigSingle = "r6";
            public const string RedBigDouble = "r7";
            public const string RedSmallSingle = "r8";
            public const string RedSmallDouble = "r9";

            public const string Green = "g1";
            public const string GreenSingle = "g2";
            public const string GreenDouble = "g3";
            public const string GreenBig = "g4";
            public const string GreenSmall = "g5";
            public const string GreenBigSingle = "g6";
            public const string GreenBigDouble = "g7";
            public const string GreenSmallSingle = "g8";
            public const string GreenSmallDouble = "g9";
        }

        public static Dictionary<string, string> BallColorNormalSpecialLangMap = new Dictionary<string, string>
        {
            { BallColorNormalSpecialDefine.Blue , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.Blue },
            { BallColorNormalSpecialDefine.BlueSingle , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.BlueSingle },
            { BallColorNormalSpecialDefine.BlueDouble , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.BlueDouble },
            { BallColorNormalSpecialDefine.BlueBig , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.BlueBig },
            { BallColorNormalSpecialDefine.BlueSmall , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.BlueSmall },
            { BallColorNormalSpecialDefine.BlueBigSingle , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.BlueBigSingle },
            { BallColorNormalSpecialDefine.BlueBigDouble , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.BlueBigDouble },
            { BallColorNormalSpecialDefine.BlueSmallSingle , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.BlueSmallSingle },
            { BallColorNormalSpecialDefine.BlueSmallDouble , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.BlueSmallDouble },

            { BallColorNormalSpecialDefine.Red , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.Red },
            { BallColorNormalSpecialDefine.RedSingle , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.RedSingle },
            { BallColorNormalSpecialDefine.RedDouble , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.RedDouble },
            { BallColorNormalSpecialDefine.RedBig , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.RedBig },
            { BallColorNormalSpecialDefine.RedSmall , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.RedSmall },
            { BallColorNormalSpecialDefine.RedBigSingle , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.RedBigSingle },
            { BallColorNormalSpecialDefine.RedBigDouble , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.RedBigDouble },
            { BallColorNormalSpecialDefine.RedSmallSingle , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.RedSmallSingle },
            { BallColorNormalSpecialDefine.RedSmallDouble , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.RedSmallDouble },

            { BallColorNormalSpecialDefine.Green , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.Green },
            { BallColorNormalSpecialDefine.GreenSingle , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.GreenSingle },
            { BallColorNormalSpecialDefine.GreenDouble , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.GreenDouble },
            { BallColorNormalSpecialDefine.GreenBig , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.GreenBig },
            { BallColorNormalSpecialDefine.GreenSmall , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.GreenSmall },
            { BallColorNormalSpecialDefine.GreenBigSingle , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.GreenBigSingle },
            { BallColorNormalSpecialDefine.GreenBigDouble , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.GreenBigDouble },
            { BallColorNormalSpecialDefine.GreenSmallSingle , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.GreenSmallSingle },
            { BallColorNormalSpecialDefine.GreenSmallDouble , "BALLCOLORNORMALSPECIAL_" + BallColorNormalSpecialDefine.GreenSmallDouble },
        };

        public class NormalSpecialBallOneDefine
        {
            public const string Single = "a";
            public const string Double = "b";
            public const string Big = "c";
            public const string Small = "d";
            public const string SumSingle = "e";
            public const string SumDouble = "f";
            public const string SumBig = "g";
            public const string SumSmall = "h";
        }
        public static Dictionary<string, string> NormalSpecialBallOneLangMap = new Dictionary<string, string>
        {
            { NormalSpecialBallOneDefine.Single , "NORMALSPECIALBALLONE_" + NormalSpecialBallOneDefine.Single },
            { NormalSpecialBallOneDefine.Double , "NORMALSPECIALBALLONE_" + NormalSpecialBallOneDefine.Double },
            { NormalSpecialBallOneDefine.Big , "NORMALSPECIALBALLONE_" + NormalSpecialBallOneDefine.Big },
            { NormalSpecialBallOneDefine.Small , "NORMALSPECIALBALLONE_" + NormalSpecialBallOneDefine.Small },
            { NormalSpecialBallOneDefine.SumSingle , "NORMALSPECIALBALLONE_" + NormalSpecialBallOneDefine.SumSingle },
            { NormalSpecialBallOneDefine.SumDouble , "NORMALSPECIALBALLONE_" + NormalSpecialBallOneDefine.SumDouble },
            { NormalSpecialBallOneDefine.SumBig , "NORMALSPECIALBALLONE_" + NormalSpecialBallOneDefine.SumBig },
            { NormalSpecialBallOneDefine.SumSmall , "NORMALSPECIALBALLONE_" + NormalSpecialBallOneDefine.SumSmall },
        };

        public class ZodiacDefine
        {
            public const string Rat = "01";
            public const string Cattle = "02";
            public const string Tiger = "03";
            public const string Rabbit = "04";
            public const string Dragon = "05";
            public const string Snake = "06";
            public const string Horse = "07";
            public const string Sheep = "08";
            public const string Monkey = "09";
            public const string Chicken = "10";
            public const string Dog = "11";
            public const string Pig = "12";
        }

        public static Dictionary<string, string> ZodiacStatusLangMap = new Dictionary<string, string>
        {
            { ZodiacDefine.Rat , "ZODIAC_" + ZodiacDefine.Rat },
            { ZodiacDefine.Cattle , "ZODIAC_" + ZodiacDefine.Cattle },
            { ZodiacDefine.Tiger , "ZODIAC_" + ZodiacDefine.Tiger },
            { ZodiacDefine.Rabbit , "ZODIAC_" + ZodiacDefine.Rabbit },
            { ZodiacDefine.Dragon , "ZODIAC_" + ZodiacDefine.Dragon },
            { ZodiacDefine.Snake , "ZODIAC_" + ZodiacDefine.Snake },
            { ZodiacDefine.Horse , "ZODIAC_" + ZodiacDefine.Horse },
            { ZodiacDefine.Sheep , "ZODIAC_" + ZodiacDefine.Sheep },
            { ZodiacDefine.Monkey , "ZODIAC_" + ZodiacDefine.Monkey },
            { ZodiacDefine.Chicken , "ZODIAC_" + ZodiacDefine.Chicken },
            { ZodiacDefine.Dog , "ZODIAC_" + ZodiacDefine.Dog },
            { ZodiacDefine.Pig , "ZODIAC_" + ZodiacDefine.Pig },
        };

        public class BetSetting
        {
            public const string SingleBetMix = "a";
            public const string SingleBetMax = "b";
            public const string SingleBetTypeMax = "c";
            public const string AgentMemberMax = "d";
            public const string xxxxx = "e";
        }

        public class BetTypes
        {
            public const string TwoStar = "2";//二星
            public const string ThreeStar = "3";//三星
            public const string FourStar = "4";//四星
            public const string Special3 = "20";//特三
            public const string Terrace = "21";//台號

            public const string NormalSpecialBallOne = "131";//正碼1
            public const string NormalSpecialBallTwo = "132";//正碼2
            public const string NormalSpecialBallThree = "133";//正碼3
            public const string NormalSpecialBallFour = "134";//正碼4
            public const string NormalSpecialBallFive = "135";//正碼5
            public const string NormalSpecialBallSix = "136";//正碼6
            public const string NormalSpecialBallSpecial = "137";//特別號

            public const string BallColorNormalSpecialOne = "141";//球色正碼1
            public const string BallColorNormalSpecialTwo = "142";//球色正碼2
            public const string BallColorNormalSpecialThree = "143";//球色正碼3
            public const string BallColorNormalSpecialFour = "144";//球色正碼4
            public const string BallColorNormalSpecialFive = "145";//球色正碼5
            public const string BallColorNormalSpecialSix = "146";//球色正碼6
            public const string BallColorNormalSpecialSpecial = "147";//球色特別號

            public const string ZodiacNormalSpecialOne = "161";//生肖正碼1
            public const string ZodiacNormalSpecialTwo = "162";//生肖正碼2
            public const string ZodiacNormalSpecialThree = "163";//生肖正碼3
            public const string ZodiacNormalSpecialFour = "164";//生肖正碼4
            public const string ZodiacNormalSpecialFive = "165";//生肖正碼5
            public const string ZodiacNormalSpecialSix = "166";//生肖正碼6
            public const string ZodiacNormalSpecialSpecial = "167";//生肖特別號

            public const string SelectNumberBingoOneDoubleFive = "231";//五選中一(複)
            public const string SelectNumberBingoOneDoubleSix = "232";//六選中一(複)
            public const string SelectNumberBingoOneDoubleSeven = "233";//七選中一(複)
            public const string SelectNumberBingoOneDoubleEight = "234";//八選中一(複)
            public const string SelectNumberBingoOneDoubleNine = "235";//九選中一(複)
            public const string SelectNumberBingoOneDoubleTen = "236";//十選中一(複)

            public const string SelectNumberBingoOneSingleFive = "241";//五選中一(單)
            public const string SelectNumberBingoOneSingleSix = "242";//六選中一(單)
            public const string SelectNumberBingoOneSingleSeven = "243";//七選中一(單)
            public const string SelectNumberBingoOneSingleEight = "244";//八選中一(單)
            public const string SelectNumberBingoOneSingleNine = "245";//九選中一(單)
            public const string SelectNumberBingoOneSingleTen = "246";//十選中一(單)

            public const string SelectNumberNotBingoOneDoubleFive = "251";//五選不中(複)
            public const string SelectNumberNotBingoOneDoubleSix = "252";//六選不中(複)
            public const string SelectNumberNotBingoOneDoubleSeven = "253";//七選不中(複)
            public const string SelectNumberNotBingoOneDoubleEight = "254";//八選不中(複)
            public const string SelectNumberNotBingoOneDoubleNine = "255";//九選不中(複)
            public const string SelectNumberNotBingoOneDoubleTen = "256";//十選不中(複)

            public const string SelectNumberNotBingoOneSingleFive = "261";//五選不中(單)
            public const string SelectNumberNotBingoOneSingleSix = "262";//六選不中(單)
            public const string SelectNumberNotBingoOneSingleSeven = "263";//七選不中(單)
            public const string SelectNumberNotBingoOneSingleEight = "264";//八選不中(單)
            public const string SelectNumberNotBingoOneSingleNine = "265";//九選不中(單)
            public const string SelectNumberNotBingoOneSingleTen = "266";//十選不中(單)

            public const string ZodiacNormalSpecialSingle = "171";//總單
            public const string ZodiacNormalSpecialDouble = "172";//總雙
            public const string ZodiacNormalSpecialBig = "173";//和>=175
            public const string ZodiacNormalSpecialSmall = "174";//和<=174
            public const string ZodiacNormalSpecialOneSix = "175";//1~6球
            public const string ZodiacNormalSpecialOneSpecial = "176";//平特一肖
            public const string ZodiacNormalSpecialNotBingo = "177";//一肖不中

            public const string SpecialZodiacTwo = "352";//二肖
            public const string SpecialZodiacThree = "353";//三肖
            public const string SpecialZodiacFour = "354";//四肖
            public const string SpecialZodiacFive = "355";//五肖
            public const string SpecialZodiacSix = "356";//六肖

            public const string SpecialConnectTwo = "29";//特串連碼2
            public const string SpecialConnectThree = "30";//特串連碼3

            public const string DoubleConnetThree = "8";//雙星連注碰
            public const string DoubleConnetFour = "9";//雙連碰

            public const string SkyBingoTwo = "27";//天地碰2星
            public const string SkyBingoThree = "28";//天地碰3星

            public const string AllPass = "191";//過關

            public const string SevenBingoSingle = "221";//七碼位數單
            public const string SevenBingoDouble = "222";//七碼位數雙
            public const string SevenBingoBig = "223";//七碼位數大
            public const string SevenBingoSmall = "224";//七碼位數小

            public const string ZodiacConnetTwo = "182";//二肖中
            public const string ZodiacConnetThree = "183";//三肖中
            public const string ZodiacConnetFour = "184";//四肖中

            public const string ZodiacConnetTwoNotBingo = "152";//二肖不中
            public const string ZodiacConnetThreeNotBingo = "153";//三肖不中
            public const string ZodiacConnetFourNotBingo = "154";//四肖不中

            public const string PillarTwo = "72";//立柱2星
            public const string PillarThree = "73";//立柱3星
            public const string PillarFour = "74";//立柱4星

            public const string SkyThreePillar = "31";//天三立柱

            public const string SpecialConnectPillarTwo = "32";//特串立柱2星
            public const string SpecialConnectPillarThree = "33";//特串立柱3星

            public const string ThreeBingoTwo = "52";//三中二

            public const string TwoBingoSpecial = "62";//二中特

            public const string AllCarDouble = "116";//全車雙
            public const string AllCarSingle = "117";//全車單
            public const string AllCarBig = "118";//全車大
            public const string AllCarSmall = "119";//全車小
            public const string AllCar = "111";//全車
        }

        public static Dictionary<string, string> BetTypesLangMap = new Dictionary<string, string>()
        {
            {BetTypes.TwoStar, "BETTYPES_" +BetTypes.TwoStar },
            {BetTypes.ThreeStar, "BETTYPES_"+ BetTypes.ThreeStar},
            {BetTypes.FourStar, "BETTYPES_"+ BetTypes.FourStar},
            {BetTypes.Special3, "BETTYPES_"+ BetTypes.Special3},
            {BetTypes.Terrace, "BETTYPES_"+ BetTypes.Terrace},
            {BetTypes.NormalSpecialBallOne, "BETTYPES_"+ BetTypes.NormalSpecialBallOne},
            {BetTypes.NormalSpecialBallTwo, "BETTYPES_"+ BetTypes.NormalSpecialBallTwo},
            {BetTypes.NormalSpecialBallThree, "BETTYPES_"+ BetTypes.NormalSpecialBallThree},
            {BetTypes.NormalSpecialBallFour, "BETTYPES_"+ BetTypes.NormalSpecialBallFour},
            {BetTypes.NormalSpecialBallFive, "BETTYPES_"+ BetTypes.NormalSpecialBallFive},
            {BetTypes.NormalSpecialBallSix, "BETTYPES_"+ BetTypes.NormalSpecialBallSix},
            {BetTypes.NormalSpecialBallSpecial, "BETTYPES_"+ BetTypes.NormalSpecialBallSpecial},
            {BetTypes.BallColorNormalSpecialOne, "BETTYPES_"+ BetTypes.BallColorNormalSpecialOne},
            {BetTypes.BallColorNormalSpecialTwo, "BETTYPES_"+ BetTypes.BallColorNormalSpecialTwo},
            {BetTypes.BallColorNormalSpecialThree, "BETTYPES_"+ BetTypes.BallColorNormalSpecialThree},
            {BetTypes.BallColorNormalSpecialFour, "BETTYPES_"+ BetTypes.BallColorNormalSpecialFour},
            {BetTypes.BallColorNormalSpecialFive, "BETTYPES_"+ BetTypes.BallColorNormalSpecialFive},
            {BetTypes.BallColorNormalSpecialSix, "BETTYPES_"+ BetTypes.BallColorNormalSpecialSix},
            {BetTypes.BallColorNormalSpecialSpecial, "BETTYPES_"+ BetTypes.BallColorNormalSpecialSpecial},
            {BetTypes.ZodiacNormalSpecialOne, "BETTYPES_"+ BetTypes.ZodiacNormalSpecialOne},
            {BetTypes.ZodiacNormalSpecialTwo, "BETTYPES_"+ BetTypes.ZodiacNormalSpecialTwo},
            {BetTypes.ZodiacNormalSpecialThree, "BETTYPES_"+ BetTypes.ZodiacNormalSpecialThree},
            {BetTypes.ZodiacNormalSpecialFour, "BETTYPES_"+ BetTypes.ZodiacNormalSpecialFour},
            {BetTypes.ZodiacNormalSpecialFive, "BETTYPES_"+ BetTypes.ZodiacNormalSpecialFive},
            {BetTypes.ZodiacNormalSpecialSix, "BETTYPES_"+ BetTypes.ZodiacNormalSpecialSix},
            {BetTypes.ZodiacNormalSpecialSpecial, "BETTYPES_"+ BetTypes.ZodiacNormalSpecialSpecial},
            {BetTypes.SelectNumberBingoOneDoubleFive, "BETTYPES_"+ BetTypes.SelectNumberBingoOneDoubleFive},
            {BetTypes.SelectNumberBingoOneDoubleSix, "BETTYPES_"+ BetTypes.SelectNumberBingoOneDoubleSix},
            {BetTypes.SelectNumberBingoOneDoubleSeven, "BETTYPES_"+ BetTypes.SelectNumberBingoOneDoubleSeven},
            {BetTypes.SelectNumberBingoOneDoubleEight, "BETTYPES_"+ BetTypes.SelectNumberBingoOneDoubleEight},
            {BetTypes.SelectNumberBingoOneDoubleNine, "BETTYPES_"+ BetTypes.SelectNumberBingoOneDoubleNine},
            {BetTypes.SelectNumberBingoOneDoubleTen, "BETTYPES_"+ BetTypes.SelectNumberBingoOneDoubleTen},
            {BetTypes.SelectNumberBingoOneSingleFive, "BETTYPES_"+ BetTypes.SelectNumberBingoOneSingleFive},
            {BetTypes.SelectNumberBingoOneSingleSix, "BETTYPES_"+ BetTypes.SelectNumberBingoOneSingleSix},
            {BetTypes.SelectNumberBingoOneSingleSeven, "BETTYPES_"+ BetTypes.SelectNumberBingoOneSingleSeven},
            {BetTypes.SelectNumberBingoOneSingleEight, "BETTYPES_"+ BetTypes.SelectNumberBingoOneSingleEight},
            {BetTypes.SelectNumberBingoOneSingleNine, "BETTYPES_"+ BetTypes.SelectNumberBingoOneSingleNine},
            {BetTypes.SelectNumberBingoOneSingleTen, "BETTYPES_"+ BetTypes.SelectNumberBingoOneSingleTen},
            {BetTypes.SelectNumberNotBingoOneDoubleFive, "BETTYPES_"+ BetTypes.SelectNumberNotBingoOneDoubleFive},
            {BetTypes.SelectNumberNotBingoOneDoubleSix, "BETTYPES_"+ BetTypes.SelectNumberNotBingoOneDoubleSix},
            {BetTypes.SelectNumberNotBingoOneDoubleSeven, "BETTYPES_"+ BetTypes.SelectNumberNotBingoOneDoubleSeven},
            {BetTypes.SelectNumberNotBingoOneDoubleEight, "BETTYPES_"+ BetTypes.SelectNumberNotBingoOneDoubleEight},
            {BetTypes.SelectNumberNotBingoOneDoubleNine, "BETTYPES_"+ BetTypes.SelectNumberNotBingoOneDoubleNine},
            {BetTypes.SelectNumberNotBingoOneDoubleTen, "BETTYPES_"+ BetTypes.SelectNumberNotBingoOneDoubleTen},
            {BetTypes.SelectNumberNotBingoOneSingleFive, "BETTYPES_"+ BetTypes.SelectNumberNotBingoOneSingleFive},
            {BetTypes.SelectNumberNotBingoOneSingleSix, "BETTYPES_"+ BetTypes.SelectNumberNotBingoOneSingleSix},
            {BetTypes.SelectNumberNotBingoOneSingleSeven, "BETTYPES_"+ BetTypes.SelectNumberNotBingoOneSingleSeven},
            {BetTypes.SelectNumberNotBingoOneSingleEight, "BETTYPES_"+ BetTypes.SelectNumberNotBingoOneSingleEight},
            {BetTypes.SelectNumberNotBingoOneSingleNine, "BETTYPES_"+ BetTypes.SelectNumberNotBingoOneSingleNine},
            {BetTypes.SelectNumberNotBingoOneSingleTen, "BETTYPES_"+ BetTypes.SelectNumberNotBingoOneSingleTen},
            {BetTypes.ZodiacNormalSpecialSingle, "BETTYPES_"+ BetTypes.ZodiacNormalSpecialSingle},
            {BetTypes.ZodiacNormalSpecialDouble, "BETTYPES_"+ BetTypes.ZodiacNormalSpecialDouble},
            {BetTypes.ZodiacNormalSpecialBig, "BETTYPES_"+ BetTypes.ZodiacNormalSpecialBig},
            {BetTypes.ZodiacNormalSpecialSmall, "BETTYPES_"+ BetTypes.ZodiacNormalSpecialSmall},
            {BetTypes.ZodiacNormalSpecialOneSix, "BETTYPES_"+ BetTypes.ZodiacNormalSpecialOneSix},
            {BetTypes.ZodiacNormalSpecialOneSpecial, "BETTYPES_"+ BetTypes.ZodiacNormalSpecialOneSpecial},
            {BetTypes.ZodiacNormalSpecialNotBingo, "BETTYPES_"+ BetTypes.ZodiacNormalSpecialNotBingo},
            {BetTypes.SpecialZodiacTwo, "BETTYPES_"+ BetTypes.SpecialZodiacTwo},
            {BetTypes.SpecialZodiacThree, "BETTYPES_"+ BetTypes.SpecialZodiacThree},
            {BetTypes.SpecialZodiacFour, "BETTYPES_"+ BetTypes.SpecialZodiacFour},
            {BetTypes.SpecialZodiacFive, "BETTYPES_"+ BetTypes.SpecialZodiacFive},
            {BetTypes.SpecialZodiacSix, "BETTYPES_"+ BetTypes.SpecialZodiacSix},
            {BetTypes.SpecialConnectTwo, "BETTYPES_"+ BetTypes.SpecialConnectTwo},
            {BetTypes.SpecialConnectThree, "BETTYPES_"+ BetTypes.SpecialConnectThree},
            {BetTypes.DoubleConnetThree, "BETTYPES_"+ BetTypes.DoubleConnetThree},
            {BetTypes.DoubleConnetFour, "BETTYPES_"+ BetTypes.DoubleConnetFour},
            {BetTypes.SkyBingoTwo, "BETTYPES_"+ BetTypes.SkyBingoTwo},
            {BetTypes.SkyBingoThree, "BETTYPES_"+ BetTypes.SkyBingoThree},
            {BetTypes.AllPass, "BETTYPES_"+ BetTypes.AllPass},
            {BetTypes.SevenBingoSingle, "BETTYPES_"+ BetTypes.SevenBingoSingle},
            {BetTypes.SevenBingoDouble, "BETTYPES_"+ BetTypes.SevenBingoDouble},
            {BetTypes.SevenBingoBig, "BETTYPES_"+ BetTypes.SevenBingoBig},
            {BetTypes.SevenBingoSmall, "BETTYPES_"+ BetTypes.SevenBingoSmall},
            {BetTypes.ZodiacConnetTwo, "BETTYPES_"+ BetTypes.ZodiacConnetTwo},
            {BetTypes.ZodiacConnetThree, "BETTYPES_"+ BetTypes.ZodiacConnetThree},
            {BetTypes.ZodiacConnetFour, "BETTYPES_"+ BetTypes.ZodiacConnetFour},
            {BetTypes.ZodiacConnetTwoNotBingo, "BETTYPES_"+ BetTypes.ZodiacConnetTwoNotBingo},
            {BetTypes.ZodiacConnetThreeNotBingo, "BETTYPES_"+ BetTypes.ZodiacConnetThreeNotBingo},
            {BetTypes.ZodiacConnetFourNotBingo, "BETTYPES_"+ BetTypes.ZodiacConnetFourNotBingo},
            {BetTypes.PillarTwo, "BETTYPES_"+ BetTypes.PillarTwo},
            {BetTypes.PillarThree, "BETTYPES_"+ BetTypes.PillarThree},
            {BetTypes.PillarFour, "BETTYPES_"+ BetTypes.PillarFour},
            {BetTypes.SkyThreePillar, "BETTYPES_"+ BetTypes.SkyThreePillar},
            {BetTypes.SpecialConnectPillarTwo, "BETTYPES_"+ BetTypes.SpecialConnectPillarTwo},
            {BetTypes.SpecialConnectPillarThree, "BETTYPES_"+ BetTypes.SpecialConnectPillarThree},
            {BetTypes.ThreeBingoTwo, "BETTYPES_"+ BetTypes.ThreeBingoTwo},
            {BetTypes.TwoBingoSpecial, "BETTYPES_"+ BetTypes.TwoBingoSpecial},
            {BetTypes.AllCarBig, "BETTYPES_"+ BetTypes.AllCarBig},
            {BetTypes.AllCarSmall, "BETTYPES_"+ BetTypes.AllCarSmall},
            {BetTypes.AllCarSingle, "BETTYPES_"+ BetTypes.AllCarSingle},
            {BetTypes.AllCarDouble, "BETTYPES_"+ BetTypes.AllCarDouble},
            {BetTypes.AllCar, "BETTYPES_"+ BetTypes.AllCar},
        };

        public decimal GetTotalBetByMember(int id, string periodId, string betType)
        {
            string select_str = "select sum(totalBet) totalBet from [lottery].[dbo].[betForm] with(nolock) ";
            string where_str = " where periodId = @periodId and l1 = @l1 and betType = @betType";

            var e = conn.Query<betForm>(select_str + where_str,
               new { periodId = periodId, l1 = id, betType = betType }).FirstOrDefault();

            decimal totalBetByMember = Convert.ToDecimal(e.totalBet);

            return totalBetByMember;
        }

        public decimal GetTotalBetByAgent(int id , string periodId)
        {
            string select_str = "select sum(totalBet) totalBet from [lottery].[dbo].[betForm] with(nolock) ";
            string where_str = " where periodId = @periodId and parentId = @parentId ";

            var e = conn.Query<betForm>(select_str + where_str,
               new { periodId = periodId, parentId = id }).FirstOrDefault();

            decimal totalBetByAgent = Convert.ToDecimal(e.totalBet);

            return totalBetByAgent;
        }

        public bool CheckChooseBallItemCorrect(JObject chooseBallObj, string betType, int agentId)
        {
            string select_str = "select betRate from [lottery].[dbo].[company] with(nolock)";
            string where_str = " Where principalId = @id ";
            var c = conn.Query<company>(select_str + where_str, new { id = agentId }).FirstOrDefault();

            var betRateObj = JsonConvert.DeserializeObject<JObject>(c.betRate);

            foreach (var pillar in chooseBallObj)
            {
                var chooseNumObj = JsonConvert.DeserializeObject<JObject>(pillar.Value.ToString());

                foreach (var numRatePair in chooseNumObj)
                {
                    if (Convert.ToDecimal(betRateObj[betType][numRatePair.Key.ToString()]) != Convert.ToDecimal(numRatePair.Value.ToString()))
                        return false;
                }
            }
            return true;
        }
        public bool CheckChooseBallCntCorrect(JObject chooseBallObj, string betType)
        {
            string[] singleBall = { BetTypes.ZodiacNormalSpecialSingle, BetTypes.ZodiacNormalSpecialDouble, BetTypes.ZodiacNormalSpecialBig, BetTypes.ZodiacNormalSpecialSmall, BetTypes.AllCarDouble, BetTypes.AllCarSingle, BetTypes.AllCarBig, BetTypes.AllCarSmall, BetTypes.Special3, BetTypes.Terrace, BetTypes.ZodiacNormalSpecialOneSix, BetTypes.ZodiacNormalSpecialOneSpecial, BetTypes.ZodiacNormalSpecialNotBingo, BetTypes.SevenBingoSingle, BetTypes.SevenBingoDouble, BetTypes.SevenBingoBig, BetTypes.SevenBingoSmall, BetTypes.AllCar, BetTypes.NormalSpecialBallOne, BetTypes.NormalSpecialBallTwo, BetTypes.NormalSpecialBallThree, BetTypes.NormalSpecialBallFour, BetTypes.NormalSpecialBallFive, BetTypes.NormalSpecialBallSix, BetTypes.NormalSpecialBallSpecial, BetTypes.BallColorNormalSpecialOne, BetTypes.BallColorNormalSpecialTwo, BetTypes.BallColorNormalSpecialThree, BetTypes.BallColorNormalSpecialFour, BetTypes.BallColorNormalSpecialFive, BetTypes.BallColorNormalSpecialSix, BetTypes.BallColorNormalSpecialSpecial, BetTypes.ZodiacNormalSpecialOne, BetTypes.ZodiacNormalSpecialTwo, BetTypes.ZodiacNormalSpecialThree, BetTypes.ZodiacNormalSpecialFour, BetTypes.ZodiacNormalSpecialFive, BetTypes.ZodiacNormalSpecialSix, BetTypes.ZodiacNormalSpecialSpecial };
            string[] multiBall = { BetTypes.SelectNumberBingoOneSingleFive, BetTypes.SelectNumberBingoOneSingleSix, BetTypes.SelectNumberBingoOneSingleSeven, BetTypes.SelectNumberBingoOneSingleEight, BetTypes.SelectNumberBingoOneSingleNine, BetTypes.SelectNumberBingoOneSingleTen, BetTypes.SelectNumberNotBingoOneSingleFive, BetTypes.SelectNumberNotBingoOneSingleSix, BetTypes.SelectNumberNotBingoOneSingleSeven, BetTypes.SelectNumberNotBingoOneSingleEight, BetTypes.SelectNumberNotBingoOneSingleNine, BetTypes.SelectNumberNotBingoOneSingleTen, BetTypes.SpecialZodiacTwo, BetTypes.SpecialZodiacThree, BetTypes.SpecialZodiacFour, BetTypes.SpecialZodiacFive, BetTypes.SpecialZodiacSix };
            string[] comboNonPillar = { BetTypes.TwoStar, BetTypes.ThreeStar, BetTypes.FourStar, BetTypes.SelectNumberBingoOneDoubleFive, BetTypes.SelectNumberBingoOneDoubleSix, BetTypes.SelectNumberBingoOneDoubleSeven, BetTypes.SelectNumberBingoOneDoubleEight, BetTypes.SelectNumberBingoOneDoubleNine, BetTypes.SelectNumberBingoOneDoubleTen, BetTypes.SelectNumberNotBingoOneDoubleFive, BetTypes.SelectNumberNotBingoOneDoubleSix, BetTypes.SelectNumberNotBingoOneDoubleSeven, BetTypes.SelectNumberNotBingoOneDoubleEight, BetTypes.SelectNumberNotBingoOneDoubleNine, BetTypes.SelectNumberNotBingoOneDoubleTen, BetTypes.SpecialConnectTwo, BetTypes.SpecialConnectThree, BetTypes.ZodiacConnetTwo, BetTypes.ZodiacConnetThree, BetTypes.ZodiacConnetFour, BetTypes.ZodiacConnetTwoNotBingo, BetTypes.ZodiacConnetThreeNotBingo, BetTypes.ZodiacConnetFourNotBingo, BetTypes.ThreeBingoTwo, BetTypes.TwoBingoSpecial };
            string[] pillarType = { BetTypes.PillarTwo, BetTypes.PillarThree, BetTypes.PillarFour, BetTypes.SkyThreePillar, BetTypes.SpecialConnectPillarTwo, BetTypes.SpecialConnectPillarThree, BetTypes.DoubleConnetThree, BetTypes.DoubleConnetFour, BetTypes.SkyBingoTwo, BetTypes.SkyBingoThree, BetTypes.AllPass };

            if (Array.IndexOf<string>(singleBall, betType) > -1)
            {
                //171, 172, 173, 174, 116, 117, 118, 119 , 20, 21, 175, 176, 177, 221, 222, 223, 224, 111 , 131, 132, 133, 134, 135, 136, 137, 141, 142, 143, 144, 145, 146, 147, 161, 162, 163, 164, 165, 166, 167
                if (chooseBallObj["0"] == null)
                    return false;

                if (chooseBallObj.Count != 1)
                    return false;

                var chooseNumObj = JsonConvert.DeserializeObject<JObject>(chooseBallObj["0"].ToString());

                if (chooseNumObj.Count != 1)
                    return false;
                return true;
            }
            else if (Array.IndexOf<string>(multiBall, betType) > -1)
            {
                // 多選不 combo 不立柱 [選號數量固定]
                // 241, 242, 243, 244, 245, 246, 261, 262, 263, 264, 265, 266, 352, 353, 354, 355, 356
                if (chooseBallObj["0"] == null)
                    return false;

                if (chooseBallObj.Count != 1)
                    return false;

                var chooseNumObj = JsonConvert.DeserializeObject<JObject>(chooseBallObj["0"].ToString());

                int ballCnt = -1;

                switch (betType)
                {
                    case BetTypes.SpecialZodiacTwo:
                        ballCnt = 2;
                        break;
                    case BetTypes.SpecialZodiacThree:
                        ballCnt = 3;
                        break;
                    case BetTypes.SpecialZodiacFour:
                        ballCnt = 4;
                        break;
                    case BetTypes.SpecialZodiacFive:
                    case BetTypes.SelectNumberBingoOneSingleFive:
                    case BetTypes.SelectNumberNotBingoOneSingleFive:
                        ballCnt = 5;
                        break;
                    case BetTypes.SpecialZodiacSix:
                    case BetTypes.SelectNumberBingoOneSingleSix:
                    case BetTypes.SelectNumberNotBingoOneSingleSix:
                        ballCnt = 6;
                        break;
                    case BetTypes.SelectNumberBingoOneSingleSeven:
                    case BetTypes.SelectNumberNotBingoOneSingleSeven:
                        ballCnt = 7;
                        break;
                    case BetTypes.SelectNumberBingoOneSingleEight:
                    case BetTypes.SelectNumberNotBingoOneSingleEight:
                        ballCnt = 8;
                        break;
                    case BetTypes.SelectNumberBingoOneSingleNine:
                    case BetTypes.SelectNumberNotBingoOneSingleNine:
                        ballCnt = 9;
                        break;
                    case BetTypes.SelectNumberBingoOneSingleTen:
                    case BetTypes.SelectNumberNotBingoOneSingleTen:
                        ballCnt = 10;
                        break;
                    default:
                        break;
                }

                if (chooseNumObj.Count != ballCnt)
                    return false;

                return true;
            }
            else if (Array.IndexOf<string>(comboNonPillar, betType) > -1)
            {
                // 多選 combo 不立柱 [選號數量限制下限]
                //2, 3, 4, 231, 232, 233, 234, 235, 236, 251, 252, 253, 254, 255, 256, 29, 30, 182, 183, 184, 152, 153, 154, 52, 62

                if (chooseBallObj["0"] == null)
                    return false;

                if (chooseBallObj.Count != 1)
                    return false;

                var chooseNumObj = JsonConvert.DeserializeObject<JObject>(chooseBallObj["0"].ToString());

                int ballMinCnt = int.MaxValue;

                switch (betType)
                {
                    case BetTypes.TwoStar:
                    case BetTypes.SpecialConnectTwo:
                    case BetTypes.TwoBingoSpecial:
                    case BetTypes.ZodiacConnetTwoNotBingo:
                    case BetTypes.ZodiacConnetTwo:
                        ballMinCnt = 2;
                        break;
                    case BetTypes.ThreeStar:
                    case BetTypes.SpecialConnectThree:
                    case BetTypes.ThreeBingoTwo:
                    case BetTypes.ZodiacConnetThreeNotBingo:
                    case BetTypes.ZodiacConnetThree:
                        ballMinCnt = 3;
                        break;
                    case BetTypes.FourStar:
                    case BetTypes.ZodiacConnetFourNotBingo:
                    case BetTypes.ZodiacConnetFour:
                        ballMinCnt = 4;
                        break;
                    case BetTypes.SelectNumberBingoOneDoubleFive:
                    case BetTypes.SelectNumberNotBingoOneDoubleFive:
                        ballMinCnt = 5;
                        break;
                    case BetTypes.SelectNumberBingoOneDoubleSix:
                    case BetTypes.SelectNumberNotBingoOneDoubleSix:
                        ballMinCnt = 6;
                        break;
                    case BetTypes.SelectNumberBingoOneDoubleSeven:
                    case BetTypes.SelectNumberNotBingoOneDoubleSeven:
                        ballMinCnt = 7;
                        break;
                    case BetTypes.SelectNumberBingoOneDoubleEight:
                    case BetTypes.SelectNumberNotBingoOneDoubleEight:
                        ballMinCnt = 8;
                        break;
                    case BetTypes.SelectNumberBingoOneDoubleNine:
                    case BetTypes.SelectNumberNotBingoOneDoubleNine:
                        ballMinCnt = 9;
                        break;
                    case BetTypes.SelectNumberBingoOneDoubleTen:
                    case BetTypes.SelectNumberNotBingoOneDoubleTen:
                        ballMinCnt = 10;
                        break;
                    default:
                        break;
                }

                if (chooseNumObj.Count < ballMinCnt)
                    return false;

                return true;
            }
            else if (Array.IndexOf<string>(pillarType, betType) > -1)
            {
                // 多選立柱 [一柱一號]
                // 72, 73, 74, 31, 32, 33 , 8, 9, 27,28 ,191
                if (betType == BetTypes.PillarTwo || betType == BetTypes.SpecialConnectPillarTwo || betType == BetTypes.AllPass) // 二星立柱 , 特串二星立柱 , 過關
                {
                    if (chooseBallObj.Count < 2)
                        return false;
                }
                else if (betType == BetTypes.PillarThree || betType == BetTypes.SpecialConnectPillarThree) // 三星立柱 , 特串三星立柱
                {
                    if (chooseBallObj.Count < 3)
                        return false;
                }
                else if (betType == BetTypes.PillarFour) // 四星立柱
                {
                    if (chooseBallObj.Count < 4)
                        return false;
                }
                else if (betType == BetTypes.SkyThreePillar) // 天地碰三星立柱
                {
                    if (chooseBallObj["s"] == null)
                        return false;

                    if (chooseBallObj.Count < 3)
                        return false;
                }
                else if (betType == BetTypes.DoubleConnetThree || betType == BetTypes.DoubleConnetFour) // 雙星連柱碰 , 雙連碰
                {
                    if (chooseBallObj.Count != 2)
                        return false;

                    if (chooseBallObj["1"] == null)
                        return false;

                    if (chooseBallObj["2"] == null)
                        return false;
                }
                else if (betType == BetTypes.SkyBingoTwo || betType == BetTypes.SkyBingoThree) // 天地碰二星 , 天地碰三星
                {
                    if (chooseBallObj.Count != 2)
                        return false;

                    if (chooseBallObj["s"] == null)
                        return false;

                    if (chooseBallObj["1"] == null)
                        return false;
                }
                else if (betType == BetTypes.AllPass)
                {
                    if (chooseBallObj.Count < 2)
                        return false;
                }
                else
                {
                    return false;
                }

                if (betType == BetTypes.DoubleConnetThree || betType == BetTypes.DoubleConnetFour || betType == BetTypes.SkyBingoThree) // 一柱多選
                {
                    if (betType == BetTypes.DoubleConnetThree)
                    {
                        var choosePillarFirstObj = JsonConvert.DeserializeObject<JObject>(chooseBallObj["1"].ToString());

                        var choosePillarSecondObj = JsonConvert.DeserializeObject<JObject>(chooseBallObj["2"].ToString());

                        if (choosePillarFirstObj.Count < 2)
                            return false;
                        if (choosePillarSecondObj.Count < 1)
                            return false;
                    }
                    else if (betType == BetTypes.DoubleConnetFour)
                    {
                        var choosePillarFirstObj = JsonConvert.DeserializeObject<JObject>(chooseBallObj["1"].ToString());

                        var choosePillarSecondObj = JsonConvert.DeserializeObject<JObject>(chooseBallObj["2"].ToString());

                        if (choosePillarFirstObj.Count < 2)
                            return false;
                        if (choosePillarSecondObj.Count < 2)
                            return false;
                    }
                    else if (betType == BetTypes.SkyBingoThree)
                    {
                        var choosePillarSpecialObj = JsonConvert.DeserializeObject<JObject>(chooseBallObj["s"].ToString());

                        var choosePillarFirstObj = JsonConvert.DeserializeObject<JObject>(chooseBallObj["1"].ToString());

                        if (choosePillarSpecialObj.Count < 1)
                            return false;
                        if (choosePillarFirstObj.Count < 2)
                            return false;
                    }
                }
                else
                {
                    foreach (var chooserPillarObj in chooseBallObj)
                    {
                        var chooseNumObj = JsonConvert.DeserializeObject<JObject>(chooserPillarObj.Value.ToString());

                        if (chooseBallObj.Count < 1)
                            return false;
                    }
                }
                return true;
            }
            return false;
        }
        public decimal CalcBetFormRate(JObject chooseBallObj, string betType)
        {
            string[] nonComboType = { BetTypes.ZodiacNormalSpecialSingle, BetTypes.ZodiacNormalSpecialDouble, BetTypes.ZodiacNormalSpecialBig, BetTypes.ZodiacNormalSpecialSmall, BetTypes.AllCarDouble, BetTypes.AllCarSingle, BetTypes.AllCarBig, BetTypes.AllCarSmall, BetTypes.Special3, BetTypes.Terrace, BetTypes.ZodiacNormalSpecialOneSix, BetTypes.ZodiacNormalSpecialOneSpecial, BetTypes.ZodiacNormalSpecialNotBingo, BetTypes.SevenBingoSingle, BetTypes.SevenBingoDouble, BetTypes.SevenBingoBig, BetTypes.SevenBingoSmall, BetTypes.AllCar, BetTypes.NormalSpecialBallOne, BetTypes.NormalSpecialBallTwo, BetTypes.NormalSpecialBallThree, BetTypes.NormalSpecialBallFour, BetTypes.NormalSpecialBallFive, BetTypes.NormalSpecialBallSix, BetTypes.NormalSpecialBallSpecial, BetTypes.BallColorNormalSpecialOne, BetTypes.BallColorNormalSpecialTwo, BetTypes.BallColorNormalSpecialThree, BetTypes.BallColorNormalSpecialFour, BetTypes.BallColorNormalSpecialFive, BetTypes.BallColorNormalSpecialSix, BetTypes.BallColorNormalSpecialSpecial, BetTypes.ZodiacNormalSpecialOne, BetTypes.ZodiacNormalSpecialTwo, BetTypes.ZodiacNormalSpecialThree, BetTypes.ZodiacNormalSpecialFour, BetTypes.ZodiacNormalSpecialFive, BetTypes.ZodiacNormalSpecialSix, BetTypes.ZodiacNormalSpecialSpecial, BetTypes.SelectNumberBingoOneSingleFive, BetTypes.SelectNumberBingoOneSingleSix, BetTypes.SelectNumberBingoOneSingleSeven, BetTypes.SelectNumberBingoOneSingleEight, BetTypes.SelectNumberBingoOneSingleNine, BetTypes.SelectNumberBingoOneSingleTen, BetTypes.SelectNumberNotBingoOneSingleFive, BetTypes.SelectNumberNotBingoOneSingleSix, BetTypes.SelectNumberNotBingoOneSingleSeven, BetTypes.SelectNumberNotBingoOneSingleEight, BetTypes.SelectNumberNotBingoOneSingleNine, BetTypes.SelectNumberNotBingoOneSingleTen, BetTypes.SpecialZodiacTwo, BetTypes.SpecialZodiacThree, BetTypes.SpecialZodiacFour, BetTypes.SpecialZodiacFive, BetTypes.SpecialZodiacSix };
            string[] comboType = { BetTypes.TwoStar, BetTypes.ThreeStar, BetTypes.FourStar, BetTypes.SelectNumberBingoOneDoubleFive, BetTypes.SelectNumberBingoOneDoubleSix, BetTypes.SelectNumberBingoOneDoubleSeven, BetTypes.SelectNumberBingoOneDoubleEight, BetTypes.SelectNumberBingoOneDoubleNine, BetTypes.SelectNumberBingoOneDoubleTen, BetTypes.SelectNumberNotBingoOneDoubleFive, BetTypes.SelectNumberNotBingoOneDoubleSix, BetTypes.SelectNumberNotBingoOneDoubleSeven, BetTypes.SelectNumberNotBingoOneDoubleEight, BetTypes.SelectNumberNotBingoOneDoubleNine, BetTypes.SelectNumberNotBingoOneDoubleTen, BetTypes.SpecialConnectTwo, BetTypes.SpecialConnectThree, BetTypes.ZodiacConnetTwo, BetTypes.ZodiacConnetThree, BetTypes.ZodiacConnetFour, BetTypes.ZodiacConnetTwoNotBingo, BetTypes.ZodiacConnetThreeNotBingo, BetTypes.ZodiacConnetFourNotBingo, BetTypes.ThreeBingoTwo, BetTypes.TwoBingoSpecial, BetTypes.PillarTwo, BetTypes.PillarThree, BetTypes.PillarFour, BetTypes.SpecialConnectPillarTwo, BetTypes.SpecialConnectPillarThree, BetTypes.DoubleConnetThree, BetTypes.DoubleConnetFour, BetTypes.SkyBingoThree, BetTypes.SkyBingoTwo, BetTypes.SkyThreePillar };

            if (Array.IndexOf<string>(nonComboType, betType) > -1)
            {
                var chooseNumObj = JsonConvert.DeserializeObject<JObject>(chooseBallObj["0"].ToString());

                decimal rateTemp = decimal.MaxValue;

                foreach (var numRatePair in chooseNumObj)
                {
                    decimal rateValue = decimal.Parse(numRatePair.Value.ToString());
                    if (rateValue < rateTemp)
                        rateTemp = rateValue;
                }

                return rateTemp;
            }
            else if (Array.IndexOf<string>(comboType, betType) > -1) // 需 combo 的玩法賠率設定 0
            {
                return 0;
            }
            else // betType = 191 過關 , 賠率相乘
            {
                decimal rateTemp = 1;

                foreach (var chooserPillarObj in chooseBallObj)
                {
                    var chooseNumObj = JsonConvert.DeserializeObject<JObject>(chooserPillarObj.Value.ToString());

                    foreach (var numRatePair in chooseNumObj)
                    {
                        decimal rateValue = decimal.Parse(numRatePair.Value.ToString());

                        rateTemp *= rateValue;
                    }
                }

                return rateTemp;
            }
        }

        public int CalcBetFormCombo(JObject chooseBallObj, string betType)
        {
            string[] comboPure = { BetTypes.TwoStar, BetTypes.ThreeStar, BetTypes.FourStar, BetTypes.SelectNumberBingoOneDoubleFive, BetTypes.SelectNumberBingoOneDoubleSix, BetTypes.SelectNumberBingoOneDoubleSeven, BetTypes.SelectNumberBingoOneDoubleEight, BetTypes.SelectNumberBingoOneDoubleNine, BetTypes.SelectNumberBingoOneDoubleTen, BetTypes.SelectNumberNotBingoOneDoubleFive, BetTypes.SelectNumberNotBingoOneDoubleSix, BetTypes.SelectNumberNotBingoOneDoubleSeven, BetTypes.SelectNumberNotBingoOneDoubleEight, BetTypes.SelectNumberNotBingoOneDoubleNine, BetTypes.SelectNumberNotBingoOneDoubleTen, BetTypes.SpecialConnectTwo, BetTypes.SpecialConnectThree, BetTypes.ZodiacConnetTwo, BetTypes.ZodiacConnetThree, BetTypes.ZodiacConnetFour, BetTypes.ZodiacConnetTwoNotBingo, BetTypes.ZodiacConnetThreeNotBingo, BetTypes.ZodiacConnetFourNotBingo, BetTypes.ThreeBingoTwo, BetTypes.TwoBingoSpecial };
            string[] comboPillarSingle = { BetTypes.PillarTwo, BetTypes.PillarThree, BetTypes.PillarFour, BetTypes.SpecialConnectPillarTwo, BetTypes.SpecialConnectPillarThree, BetTypes.SkyBingoTwo, BetTypes.SkyThreePillar };
            string[] comboPillarMulti = { BetTypes.DoubleConnetThree, BetTypes.DoubleConnetFour, BetTypes.SkyBingoThree };
            string[] other = { BetTypes.ZodiacNormalSpecialSingle, BetTypes.ZodiacNormalSpecialDouble, BetTypes.ZodiacNormalSpecialBig, BetTypes.ZodiacNormalSpecialSmall, BetTypes.AllCarDouble, BetTypes.AllCarSingle, BetTypes.AllCarBig, BetTypes.AllCarSmall, BetTypes.Special3, BetTypes.Terrace, BetTypes.ZodiacNormalSpecialOneSix, BetTypes.ZodiacNormalSpecialOneSpecial, BetTypes.ZodiacNormalSpecialNotBingo, BetTypes.SevenBingoSingle, BetTypes.SevenBingoDouble, BetTypes.SevenBingoBig, BetTypes.SevenBingoSmall, BetTypes.AllCar, BetTypes.NormalSpecialBallOne, BetTypes.NormalSpecialBallTwo, BetTypes.NormalSpecialBallThree, BetTypes.NormalSpecialBallFour, BetTypes.NormalSpecialBallFive, BetTypes.NormalSpecialBallSix, BetTypes.NormalSpecialBallSpecial, BetTypes.BallColorNormalSpecialOne, BetTypes.BallColorNormalSpecialTwo, BetTypes.BallColorNormalSpecialThree, BetTypes.BallColorNormalSpecialFour, BetTypes.BallColorNormalSpecialFive, BetTypes.BallColorNormalSpecialSix, BetTypes.BallColorNormalSpecialSpecial, BetTypes.ZodiacNormalSpecialOne, BetTypes.ZodiacNormalSpecialTwo, BetTypes.ZodiacNormalSpecialThree, BetTypes.ZodiacNormalSpecialFour, BetTypes.ZodiacNormalSpecialFive, BetTypes.ZodiacNormalSpecialSix, BetTypes.ZodiacNormalSpecialSpecial, BetTypes.SelectNumberBingoOneSingleFive, BetTypes.SelectNumberBingoOneSingleSix, BetTypes.SelectNumberBingoOneSingleSeven, BetTypes.SelectNumberBingoOneSingleEight, BetTypes.SelectNumberBingoOneSingleNine, BetTypes.SelectNumberBingoOneSingleTen, BetTypes.SelectNumberNotBingoOneSingleFive, BetTypes.SelectNumberNotBingoOneSingleSix, BetTypes.SelectNumberNotBingoOneSingleSeven, BetTypes.SelectNumberNotBingoOneSingleEight, BetTypes.SelectNumberNotBingoOneSingleNine, BetTypes.SelectNumberNotBingoOneSingleTen, BetTypes.SpecialZodiacTwo, BetTypes.SpecialZodiacThree, BetTypes.SpecialZodiacFour, BetTypes.SpecialZodiacFive, BetTypes.SpecialZodiacSix, BetTypes.AllPass };

            if (Array.IndexOf<string>(comboPure, betType) > -1)
            {
                var chooseNumObj = JsonConvert.DeserializeObject<JObject>(chooseBallObj["0"].ToString());

                int numCnt = chooseNumObj.Count;
                int selectCnt;
                switch (betType)
                {
                    case BetTypes.TwoStar:
                    case BetTypes.SpecialConnectTwo:
                    case BetTypes.ZodiacConnetTwo:
                    case BetTypes.ZodiacConnetTwoNotBingo:
                    case BetTypes.TwoBingoSpecial:
                        selectCnt = 2;
                        break;
                    case BetTypes.ThreeStar:
                    case BetTypes.SpecialConnectThree:
                    case BetTypes.ZodiacConnetThree:
                    case BetTypes.ZodiacConnetThreeNotBingo:
                    case BetTypes.ThreeBingoTwo:
                        selectCnt = 3;
                        break;
                    case BetTypes.FourStar:
                    case BetTypes.ZodiacConnetFour:
                    case BetTypes.ZodiacConnetFourNotBingo:
                        selectCnt = 4;
                        break;
                    case BetTypes.SelectNumberBingoOneDoubleFive:
                    case BetTypes.SelectNumberNotBingoOneDoubleFive:
                        selectCnt = 5;
                        break;
                    case BetTypes.SelectNumberBingoOneDoubleSix:
                    case BetTypes.SelectNumberNotBingoOneDoubleSix:
                        selectCnt = 6;
                        break;
                    case BetTypes.SelectNumberBingoOneDoubleSeven:
                    case BetTypes.SelectNumberNotBingoOneDoubleSeven:
                        selectCnt = 7;
                        break;
                    case BetTypes.SelectNumberBingoOneDoubleEight:
                    case BetTypes.SelectNumberNotBingoOneDoubleEight:
                        selectCnt = 8;
                        break;
                    case BetTypes.SelectNumberBingoOneDoubleNine:
                    case BetTypes.SelectNumberNotBingoOneDoubleNine:
                        selectCnt = 9;
                        break;
                    case BetTypes.SelectNumberBingoOneDoubleTen:
                    case BetTypes.SelectNumberNotBingoOneDoubleTen:
                        selectCnt = 10;
                        break;
                    default:
                        return -1;
                }

                int comboCnt = CalcCombinationCnt(numCnt, selectCnt);
                return comboCnt;
            }
            else if (Array.IndexOf<string>(comboPillarSingle, betType) > -1)
            {
                int specialPillarCnt = -1;

                if (betType == BetTypes.SkyThreePillar)
                {
                    specialPillarCnt = JsonConvert.DeserializeObject<JObject>(chooseBallObj["s"].ToString()).Count;

                    chooseBallObj.Remove("s");
                }

                int[] pillarNumCnt = new int[chooseBallObj.Count];
                int index = 0;

                foreach (var pillarPosi in chooseBallObj)
                {
                    var chooseNumObj = JsonConvert.DeserializeObject<JObject>(pillarPosi.Value.ToString());

                    pillarNumCnt[index] = chooseNumObj.Count;

                    index++;
                }

                int selectCnt;

                switch (betType)
                {
                    case BetTypes.PillarTwo:
                    case BetTypes.SpecialConnectPillarTwo:
                    case BetTypes.SkyBingoTwo:
                    case BetTypes.SkyThreePillar:
                        selectCnt = 2;
                        break;
                    case BetTypes.PillarThree:
                    case BetTypes.SpecialConnectPillarThree:
                        selectCnt = 3;
                        break;
                    case BetTypes.PillarFour:
                        selectCnt = 4;
                        break;
                    default:
                        return -1;
                }

                List<int[]> numComAry = Model.BetForm.Define.PermutationAndCombination<int>.GetCombination(pillarNumCnt, selectCnt);

                int comboInner = 0;

                foreach (var numCom in numComAry)
                {
                    int multiplyValue = 1;
                    foreach (var num in numCom)
                    {
                        multiplyValue *= num;
                    }
                    comboInner += multiplyValue;
                }

                if (betType == BetTypes.SkyThreePillar)
                {
                    int comboCnt = comboInner * specialPillarCnt;
                    return comboCnt;
                }
                else
                {
                    return comboInner;
                }
            }
            else if (Array.IndexOf<string>(comboPillarMulti, betType) > -1)
            {
                int comboCnt;

                if (betType == BetTypes.DoubleConnetThree) // 雙星連柱碰
                {
                    var firstPillar = JsonConvert.DeserializeObject<JObject>(chooseBallObj["1"].ToString());
                    var secondPillar = JsonConvert.DeserializeObject<JObject>(chooseBallObj["2"].ToString());

                    comboCnt = CalcCombinationCnt(firstPillar.Count, 2) * secondPillar.Count;

                    return comboCnt;
                }
                else if (betType == BetTypes.DoubleConnetFour) // 雙連碰
                {
                    var firstPillar = JsonConvert.DeserializeObject<JObject>(chooseBallObj["1"].ToString());
                    var secondPillar = JsonConvert.DeserializeObject<JObject>(chooseBallObj["2"].ToString());

                    comboCnt = CalcCombinationCnt(firstPillar.Count, 2) * CalcCombinationCnt(secondPillar.Count, 2);

                    return comboCnt;
                }

                else if (betType == BetTypes.SkyBingoThree) // 天地碰三星
                {
                    var specialPillar = JsonConvert.DeserializeObject<JObject>(chooseBallObj["s"].ToString()); // 特碼柱擇 1 碼
                    var normalPillar = JsonConvert.DeserializeObject<JObject>(chooseBallObj["1"].ToString()); // 正碼柱擇 2 碼

                    comboCnt = specialPillar.Count * CalcCombinationCnt(normalPillar.Count, 2);

                    return comboCnt;
                }
            }
            else if (Array.IndexOf<string>(other, betType) > -1)
            {
                return 1;
            }
            return -1;
        }

        public int CalcCombinationCnt(int n , int m)
        {
            if (n < m || n < 1 || m <1)
                return -1;

            int dividends = 1;
            int divisor = 1;

            while (m > 0)
            {
                dividends *= n;
                divisor *= m;
                n--;
                m--;
            }

            return dividends / divisor;
        }

        public string ConvertNormalToZodiac(string lotteryResult)
        {
            string[] lotteryResultAry = lotteryResult.Split(',');

            string zodiacResult = "";

            TaiwanLunisolarCalendar tlc = new TaiwanLunisolarCalendar();

            for (int i = 0; i < lotteryResultAry.Length; i++)
            {
                lotteryResultAry[i] = (12 - (11 - (tlc.GetYear(DateTime.Now) - 97) % 12 + (Convert.ToInt32(lotteryResultAry[i]) - 1) % 12) % 12).ToString("00");

                zodiacResult += lotteryResultAry[i] + ",";
            }
            
            zodiacResult = zodiacResult.TrimEnd(',');

            return zodiacResult;
        }

        public string GetLotteryPeriodId()
        {
            return "10006";
        }

        public bool CheckBetFormExist(int memberId, JObject chooseBallObj, string betType)
        {
            string chooseBall = JsonConvert.SerializeObject(chooseBallObj);
            string select_str = "select id from [lottery].[dbo].[betForm] with(nolock)";
            string where_str = " Where l1 = @l1 and chooseBall = @chooseBall and status = 1 and betType = @betType ";

            var r = conn.Query<betForm>(select_str + where_str,
               new { l1 = memberId, chooseBall = chooseBall, betType = betType }).FirstOrDefault();
            if (r != null)
            {
                return true;
            }
            return false;
        }

        public bool CheckBetTime()
        {
            string createDateTime_s = DateTime.Now.ToString("yyyy/MM/dd 21:29:00");

            if (DateTime.Now > DateTime.Parse(createDateTime_s))
            {
                return false;
            }
            return true;
        }
        public List<string> GetSelectedNum(string chooseBall, string pillarMark, bool removeSpecial)
        {
            var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBall);

            if (removeSpecial == true) chooseObj.Remove("s");

            List<string> chooseBallList = new List<string>();

            if (new Regex("^[0-9s]{1}$").IsMatch(pillarMark))
            {
                var numPairObj = JsonConvert.DeserializeObject<JObject>(chooseObj[pillarMark].ToString().TrimEnd(','));

                foreach (var numRatePair in numPairObj)
                {
                    chooseBallList.Add(numRatePair.Key.ToString());
                }
            }
            else if (pillarMark == "all")
            {
                foreach (var pillarObj in chooseObj)
                {
                    var numPairObj = JsonConvert.DeserializeObject<JObject>(pillarObj.Value.ToString().TrimEnd(','));

                    foreach (var numRatePair in numPairObj)
                    {
                        chooseBallList.Add(numRatePair.Key.ToString());
                    }
                }
            }

            return chooseBallList;
        }

        public decimal MappingNumToRate(string chooseBall, string num)
        {
            var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBall);

            decimal rate = 0;

            foreach (var pillarObj in chooseObj)
            {
                var numPairObj = JsonConvert.DeserializeObject<JObject>(pillarObj.Value.ToString().TrimEnd(','));

                foreach (var numRatePair in numPairObj)
                {
                    if (numRatePair.Key.ToString() == num) rate = (decimal)numRatePair.Value;
                }
            }
            return rate;
        }
    }
}
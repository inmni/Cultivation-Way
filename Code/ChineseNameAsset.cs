using System.Collections.Generic;

namespace Cultivation_Way
{
    class ChineseNameAsset : Asset
    {
        //采用模板法命名
        public string[] templates = { "addition_start", "part", "addition_end" };//命名模板
        /*
         * 大致模板为addition_start,parts,addition_end
         * 模板元素前缀加上R.6F表示60%产生
         */

        public List<string> addition_startList = new List<string>();//前缀,也用作姓氏

        public List<string> addition_endList = new List<string>();//后缀，也用作名

        public List<string> partsList = new List<string>();//中间1，也用作辈分

        public List<string> partsList2 = new List<string>();//中间2

        public List<string> fixedList = new List<string>();//可作为固定生成的名字

        public List<string> special1 = new List<string>();//特殊词缀一组

        public List<string> special2 = new List<string>();//特殊词缀二组

        public bool onlyByTemplate = false;//是否只按照模板生成（即不生成预订的名字

        public float fixedNameChance = 0.5f;//表示产生固定名字的概率（仅在上一项为false时有效），0.5f表示50%

        public static List<string> familyNameTotal = new List<string>()
        {"甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸",
            "赵", "钱", "孙", "李", "周", "吴", "郑", "王", "冯", "陈",
            "褚", "卫", "蒋", "沈", "韩", "杨", "朱", "秦", "尤", "许",
            "何", "吕", "施", "张", "孔", "曹", "严", "华", "金", "魏",
            "陶", "姜", "费", "廉", "岑", "薛", "雷", "贺", "倪", "汤",
            "滕", "殷", "罗", "毕", "郝", "邬", "安", "常", "乐", "于",
            "时", "傅", "皮", "卞", "齐", "康", "伍", "余", "元", "卜",
            "顾", "孟", "平", "黄", "杜", "阮", "蓝", "闵", "席", "季",
            "麻", "强", "贾", "路", "娄", "危", "江", "童", "颜", "郭",
            "梅", "盛", "林", "刁", "钟", "徐", "邱", "骆", "高", "夏",
            "蔡", "田", "樊", "胡", "凌", "霍", "程", "嵇", "邢", "滑",
            "裴", "陆", "荣", "翁", "荀", "羊", "於", "惠", "甄", "魏",
            "加", "封", "芮", "羿", "储", "靳", "汲", "邴", "糜", "松",
            "井", "段", "富", "巫", "乌", "焦", "巴", "弓", "叶", "幸",
            "司", "韶", "郜", "黎", "蓟", "薄", "印", "宿", "白", "怀",
            "蒲", "台", "从", "鄂", "索", "咸", "籍", "赖", "卓", "蔺",
            "屠", "蒙", "池", "乔", "阴", "郁", "胥", "能", "苍", "双",
            "温", "别", "庄", "晏", "柴", "瞿", "阎", "充", "慕", "连",
            "茹", "习", "宦", "艾", "鱼", "容", "向", "古", "易", "慎",
            "戈", "廖", "庚", "终", "暨", "居", "衡", "步", "都", "耿",
            "满", "弘", "曾", "毋", "沙", "乜", "养", "鞠", "须", "丰",
            "巢", "关", "蒯", "相", "查", "后", "江", "红", "游", "竺",
            "权", "逯", "盖", "益", "桓", "公", "万俟", "司马", "上官", "欧阳",
            "公孙", "乐正", "轩辕", "令狐", "钟离", "闾丘", "长孙", "慕容", "鲜于", "宇文",
            "司徒", "司空", "亓官", "司寇", "仉督", "子车", "段干", "百里", "东郭", "南门",
            "呼延", "妫海", "羊舌", "微生", "岳帅", "缑亢", "况後", "有琴", "梁丘", "左丘",
            "东门", "西门","変態","U"
        };//全部姓氏

        public static string[] rankName = new string[10] { "凡", "黄", "玄", "地", "天", "荒", "洪", "宙", "宇", "圣" };//品阶，仅限一字
        public static string[] rankName1 = new string[5] { "凡", "灵", "仙", "魔", "道" };
        public ChineseNameAsset()
        {
        }
        static ChineseNameAsset()
        {

        }
    }
}

﻿using System.Collections.Generic;

namespace Cultivation_Way
{
    class ChineseNameAsset : Asset
    {
        /// <summary>
        /// 采用模板法命名
        /// 大致模板为addition_start,parts,addition_end                     
        /// 模板元素前缀加上R.6F表示60%产生
        /// </summary>
        public string[] templates = { "addition_start", "part", "addition_end" };
        
        /// <summary>
        /// 前缀，用作智慧种族姓氏
        /// </summary>
        public List<string> addition_startList = new List<string>();

        /// <summary>
        /// 后缀，用作非智慧种族姓氏
        /// </summary>
        public List<string> addition_endList = new List<string>();
        /// <summary>
        /// 中间部分1
        /// </summary>
        public List<string> partsList = new List<string>();
        /// <summary>
        /// 中间部分2
        /// </summary>
        public List<string> partsList2 = new List<string>();
        /// <summary>
        /// 预设名字
        /// </summary>
        public List<string> fixedList = new List<string>();
        /// <summary>
        /// 额外词缀1
        /// </summary>
        public List<string> special1 = new List<string>();
        /// <summary>
        /// 额外词缀2
        /// </summary>
        public List<string> special2 = new List<string>();
        /// <summary>
        /// 是否只按照模板生成（人名务必选择是）
        /// </summary>
        public bool onlyByTemplate = false;
        /// <summary>
        /// 产生预设名字的概率（仅在onlyByTemplate为false时有效），0.5f表示50%
        /// </summary>
        public float fixedNameChance = 0.005f;
        /// <summary>
        /// 全部姓氏，切勿重复添加
        /// </summary>
        public static List<string> familyNameTotal = new List<string>()
        {"赵", "钱", "孙", "李", "周", "吴", "郑", "王", "冯", "陈",
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
            "妄", "邪", "幽", "阴", "葬", "馆", "夜", "冥", "溟", "亡",
            "天", "光", "道", "源", "灵", "玄", "耀", "阳", "姬", "极",
            "诡", "太", "宇", "宙", "简", "易", "初", "始", "皇", "权",
            "帝", "尊", "军", "君", "瑞", "白", "雪", "仙", "圣", "神",
            "武", "苍", "穹", "空", "魔", "禁", "嗜", "噬", "刹", "杀",
            "逆", "媚", "煞", "狱", "骨", "墟", "炼", "阎", "焚", "巫",
            "炎", "雷", "冰", "金", "木", "水", "火", "土", "风", "咒",
            "霸", "镇", "御", "蛊", "祭", "罪", "梦", "荒", "祭", "战",
            "陨", "寂", "狱", "祖", "封", "印", "夜溟", "冥司", "幽暮", "永暗",
            "无极", "太上", "紫薇", "玲珑", "深渊", "焚天", "自在", "道", "逍遥", "归灵",
            "权", "逯", "盖", "益", "桓", "公", "万俟", "司马", "上官", "欧阳",
            "公孙", "乐正", "轩辕", "令狐", "钟离", "闾丘", "长孙", "慕容", "鲜于", "宇文",
            "司徒", "司空", "亓官", "司寇", "仉督", "子车", "段干", "百里", "东郭", "南门",
            "呼延", "妫海", "羊舌", "微生", "岳帅", "缑亢", "况後", "有琴", "梁丘", "左丘",
            "东门", "西门","変態","U","甲",
             "火","炎","焰","焱","灵","龙","圣","炎","千官","炼","战","武","天工","楚上","无双","炉","幽仁","帝昊","将","轩辕","斩兵"

        };

        public static string[] rankName = new string[10] { "凡", "黄", "玄", "地", "天", "荒", "洪", "宙", "宇", "圣" };//品阶，仅限一字
        public static string[] rankName1 = new string[6] { "(凡阶)", "(黄阶)","(玄阶)", "(地阶)", "(天阶)", "(帝阶)" };
        public ChineseNameAsset()
        {
        }
        static ChineseNameAsset()
        {

        }
    }
}

﻿using System.Collections.Generic;

namespace Cultivation_Way
{
    class ChineseNameLibrary : AssetLibrary<ChineseNameAsset>
    {
        //初始化
        public override void init()
        {
            base.init();

            #region 国家名
            //人类
            this.add(new ChineseNameAsset
            {
                id = "human_kingdom",
                onlyByTemplate = false,
                fixedNameChance = 0.02f
            });
            this.t.addition_startList = new List<string> { "北", "南", "东", "西" };
            this.t.addition_endList = new List<string> { "王朝", "帝国", "国", "朝" };
            this.t.partsList = new List<string> { "赵", "钱", "孙", "李", "周", "吴", "郑", "王", "冯", "陈",
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
            "东门", "西门", "天", "地", "玄", "黄", "宇", "宙", "洪", "荒",
            "日", "月", "盈", "昃", "辰", "宿", "列", "张", "云", "腾",
            "致", "雨", "露", "结", "为", "霜", "金", "生", "丽", "水",
            "玉", "出", "昆", "冈", "海", "咸", "河", "淡", "鳞", "潜",
            "羽", "翔", "龙", "师", "火", "帝", "鸟", "官", "人", "皇",
            "吊", "民", "伐", "罪", "周", "发", "殷", "汤", "坐", "朝",
            "问", "道", "垂", "拱", "平", "章", "鸣", "凤", "在", "竹",
            "白", "驹", "食", "场", "化", "被", "草", "木", "赖", "及",
            "万", "方", "女", "慕", "贞", "洁", "男", "效", "才", "良",
            "知", "过", "必", "改", "得", "能", "莫", "忘", "墨", "悲",
            "丝", "染", "诗", "赞", "羔", "羊", "景", "行", "维", "贤",
            "克", "念", "作", "圣", "祸", "因", "恶", "积", "福", "缘",
            "善", "庆", "尺", "璧", "非", "宝", "寸", "阴", "是", "竞",
            "临", "深", "履", "薄", "夙", "兴", "温", "凊", "似", "兰",
            "斯", "馨", "如", "松", "之", "盛", "笃", "初", "诚", "美",
            "慎", "终", "宜", "令", "荣", "业", "所", "基", "籍", "甚",
            "无", "竟", "乐", "殊", "贵", "贱", "礼", "别", "尊", "卑",
            "上", "和", "下", "睦", "夫", "唱", "妇", "随", "孔", "怀",
            "兄", "弟", "同", "气", "连", "枝", "交", "友", "投", "分",
            "切", "磨", "箴", "规", "性", "静", "情", "逸", "心", "动",
            "神", "疲", "守", "真", "志", "满", "逐", "物", "意", "移",
            "背", "邙", "面", "洛", "浮", "渭", "据", "泾", "宫", "殿",
            "盘", "郁", "楼", "观", "飞", "惊", "肆", "筵", "设", "席",
            "鼓", "瑟", "吹", "笙", "升", "阶", "纳", "陛", "弁", "转",
            "疑", "星", "杜", "稿", "钟", "隶", "漆", "书", "壁", "经",
            "府", "罗", "将", "相", "路", "侠", "槐", "卿", "世", "禄",
            "侈", "富", "车", "驾", "肥", "轻", "策", "功", "茂", "实",
            "勒", "碑", "刻", "铭", "桓", "公", "匡", "合", "济", "弱",
            "扶", "倾", "绮", "回", "汉", "惠", "说", "感", "武", "丁",
            "假", "途", "灭", "虢", "践", "土", "会", "盟", "何", "遵",
            "约", "法", "韩", "弊", "烦", "刑", "九", "州", "禹", "迹",
            "百", "郡", "秦", "并", "岳", "宗", "泰", "岱", "禅", "主",
            "云", "亭", "旷", "远", "绵", "邈", "岩", "岫", "杳", "冥",
            "治", "本", "于", "农", "务", "资", "稼", "穑", "孟", "轲",
            "敦", "素", "史", "鱼", "秉", "直", "庶", "几", "中", "庸",
            "劳", "谦", "谨", "敕", "省", "躬", "讥", "诫", "宠", "增",
            "抗", "极", "殆", "辱", "近", "耻", "林", "皋", "幸", "即",
            "求", "古", "寻", "论", "散", "虑", "逍", "遥", "欣", "奏",
            "累", "遣", "戚", "谢", "欢", "招", "陈", "根", "委", "翳",
            "落", "叶", "飘", "摇", "游", "鹍", "独", "运", "凌", "摩",
            "绛", "霄", "具", "膳", "餐", "饭", "适", "口", "充", "肠",
            "饱", "饫", "烹", "宰", "饥", "厌", "糟", "糠", "纨", "扇",
            "圆", "絜", "银", "烛", "炜", "煌", "昼", "眠", "夕", "寐",
            "蓝", "笋", "象", "床", "嫡", "后", "嗣", "续", "祭", "祀",
            "烝", "尝", "稽", "颡", "再", "拜", "悚", "惧", "恐", "惶",
            "驴", "骡", "犊", "特", "骇", "跃", "超", "骧", "诛", "斩",
            "贼", "盗", "捕", "获", "叛", "亡", "释", "纷", "利", "俗",
            "竝", "皆", "佳", "妙", "毛", "施", "淑", "姿", "工", "颦",
            "妍", "笑", "指", "薪", "修", "祜", "永", "绥", "吉", "劭",
            "矩", "步", "引", "领", "俯", "仰", "廊", "庙", "谓", "语",
            "助", "者", "焉", "哉", "乎", "也"};
            this.t.fixedList = new List<string> { "星棋盘王朝", "屹昂帝国" };
            this.t.templates = new string[] { "R.3Faddition_start", "part1", "R.2Fpart1", "addition_end" };
            //兽人，直接复制人类
            this.clone("orc_kingdom", "human_kingdom");
            //精灵，直接复制人类
            this.clone("elf_kingdom", "human_kingdom");
            //矮人，直接复制人类
            this.clone("dwarf_kingdom", "human_kingdom");
            //天族，直接复制人类
            this.clone("Tian_kingdom", "human_kingdom");
            //冥族，直接复制人类
            this.clone("Ming_kingdom", "human_kingdom");
            #endregion

            #region 城市名
            //人类城市，直接复制国家命名
            this.clone("human_city", "human_kingdom");
            this.t.addition_endList = new List<string> { "城", "镇", "村", "域", "州", "府" };
            this.t.fixedList = new List<string> { "星棋盘州", "人间府" };
            this.t.templates = new string[] { "part1", "R.2Fpart1", "addition_end" };
            //兽人，直接复制人类
            this.clone("orc_city", "human_city");
            //精灵，直接复制人类
            this.clone("elf_city", "human_city");
            //矮人，直接复制人类
            this.clone("dwarf_city", "human_city");
            //天族，直接复制人类
            this.clone("Tian_city", "human_city");
            //冥族，直接复制人类
            this.clone("Ming_city", "human_city");
            #endregion

            #region 文化名
            //人类文化，直接复制城市
            this.clone("human_culture", "human_city");
            this.t.addition_endList = new List<string> { "文化", "氏" };
            this.t.fixedList = new List<string> { "一米氏", "人间文化" };
            this.t.templates = new string[] { "part1", "part1", "addition_end" };
            //兽人，直接复制人类
            this.clone("orc_culture", "human_culture");
            //精灵，直接复制人类
            this.clone("elf_culture", "human_culture");
            //矮人，直接复制人类
            this.clone("dwarf_culture", "human_culture");
            //天族，直接复制人类
            this.clone("Tian_culture", "human_culture");
            //冥族，直接复制人类
            this.clone("Ming_culture", "human_culture");
            #endregion

            #region 国家格言
            this.add(new ChineseNameAsset
            {
                id = "kingdom_mottos",
                onlyByTemplate = false,
                fixedNameChance = 1f
            });
            this.t.fixedList = new List<string>
            {
                "有志者自有千计万计，无志者只感千难万难。",
                "不安于现状，不甘于平庸！",
                "再长的路，一步步也能走完。",
                "自己打败自己是最可悲的失败！",
                "有志者，事竟成，破釜沉舟，百二秦关终属楚；有心人，天不负，卧薪尝胆，三千越甲可吞吴。",
                "万事皆由人的意志创造。",
                "志之所向，金石为开，谁能御之？",
                "不安于小成，然后足以成大器",
                "不诱于小利，然后可以立远功。",
                "“失败”只存在于废人的字典里。",
                "国者，天下之大器也，重任也。",
                "古之得道者，穷亦乐，达亦乐，所乐非穷达也。",
                "师之所处，荆棘生焉。大军之后，必有凶年。",
                "凡有血气，皆有争心。",
                "上不失天时，下不失地利，中得人和，而百事不废。",
                "风水人间不可无，也须阴骘两相扶。",
                "古之为政，爱人为大。",
                "尔等不过以五十步笑百步。",
                "三杯通大道，一醉解千愁。",
                "天下熙熙，皆为利来；天下攘攘，皆为利往。",
                "任力者故劳，任人者故逸。",
                "怠慢忘身，祸灾乃作。",
                "心以启智，智以启财，财以启众，众以启贤，贤之有启，以王天下。",
                "上士闻道，勤而行之；中士闻道，若存若亡；下士闻道，大笑之。",
                "天道远，人道迩，非所及也，何以知之？",
                "宁教我负天下人，切莫天下人负我。",
                "法，历则国正", "善，大爱于天下。",
                "君子之国", "商贸之国",
                "穷则独善其身，达则兼济天下",
                "蛮夷之国",
                "礼乐征伐自天子出",
                "问苍茫大地，谁主沉浮？",
                "一生二，二生三，三生万物。",
                "天子守国门，君王死社稷。",
                "山河无恙，立当自强。",
                "江山如此多娇，引无数英雄竞折腰。",
                "沙场染血何忧惧，马革裹尸还！",
                "雄视天下",
                "风起----",
                "为天下命",
                "雄踞一方，草视天下",
                "文化昌盛",
                "天地不仁，以万物为刍狗！",
                "文章千秋治，武者甲子休。",
                "九劫空，苍天逆。旭日腾，圣族兴。",
                "走，我带你杀人去。",
                "亦昂牛批，yyds，",
                "退一步是人间，进一步就在幽冥。",
                "陨道战天，谁与争锋。",
                "义，以命为抵", "宁为刀下魂，不做膝下鬼。",
                "黄沙百战穿金甲，不破楼兰终不还。",
                "天国万年", "与天博弈",
                "此间乐，不思蜀。",
                "兵锋所向，所向披靡。",
                "开科举，兴文化，安四方，万国来朝。",
                "愿效太宗，再开盛世。",
                "文明古国", "水能载舟亦能覆舟",
                "为千秋谋大计，为万世开太平",
                "大江东去，浪淘尽，千古风流人物。",
                "顺天意，行大道。",
                "领天意，征伐天下。",
                "祝修仙群所有群友，前程似锦",
                "知天意，逆天难",
                "王侯将相宁有种乎",
                "祝修仙群所有群友，步步高升",
                "一代天骄---",
                "纵横之道，以苍生为棋",
                "周易，阴阳之道",
                "祝修仙群所有群友，万事顺意",
                "兼爱非攻",
                "道，自然也",
                "初九 潜龙勿用",
                "冲阵杀敌，无往不利",
                "千万人吾往矣",
                "歌舞祥和，太平盛景乐",
                "九五 飞龙在天",
                "周公吐哺，天下归心",
                "数风流人物，还看今朝",
                "万里长征人未还",
                "悠悠万国，吾为正统。",
                "预祝新年快乐！"
            };
            #endregion

            #region 人名
            this.add(new ChineseNameAsset
            {
                id = "human_name",
                onlyByTemplate = true
            });


            //姓氏
            this.t.addition_startList = new List<string>
            {
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
            "东门", "西门"
            };
            //辈分
            this.t.partsList = new List<string>
            {
                "诚","朴","雄","伟","励","学","敦","行", "富","强","民","主","文","明","和","谐", "自","由","平","等","公","正","法","治", "爱","国","敬","业","诚","信","友","善", "天","地","玄","黄","宇","宙","洪","荒", "安","石","寓","丝","竹","方","朔","杂", "诙","谐","昂","霄","气","概","古","来", "无","地","可","容","才","不","见","骑", "鲸","仙","伯","唾","手","功","名","事", "了","猿","鹤","与","同","侪","有","意",
            "谢","轩","冕","无","计","避","嫌","猜", "静","中","乐","山","照","座","月","浮", "杯","忘","形","湛","辈","一","笑","丘", "壑","写","高","怀","只","恐","天","催", "玉","斧","为","破","烟","尘","昏","翳", "人","自","日","边","来","东","阁","动", "天","乙","开","运","太","岳","钟","英", "荣","河","温","洛","睿","哲","初","生", "甘","泉","濯","耳","畴","产","并","耕", "尚","书","道","博","说","文","义","精",
            "弥","纶","彝","宪","淳","化","休","明", "光","州","衮","冕","汝","南","旦","评", "勋","猷","炳","焕","炯","鉴","渊","清", "有","唐","受","命","凤","翥","龙","翔", "宣","威","漳","镇","雍","穆","宁","康", "扶","伦","翼","教","功","成","紫","阳", "名","贤","冢","宰","德","立","嗣","昌", "少","师","秉","政","严","肃","端","庄", "环","奇","都","附","爵","秩","尊","良", "云","兴","骥","展","桂","馥","兰","香",
            "缉","熙","景","祚","福","祉","绵","长", "侯","衍","伯","浩","长","朝","国","教", "修","齐","治","平","中","华","位","育"
            };
            //最后一个字
            this.t.addition_endList = new List<string>
            {
                "世", "舜", "丞", "主", "产", "仁", "仇", "仓", "仕", "仞",
            "任", "伋", "众", "伸", "佐", "佺", "侃", "侪", "促", "俟",
            "信", "俣", "修", "倝", "倡", "倧", "偿", "储", "僖", "僧",
            "僳", "儒", "俊", "伟", "列", "则", "刚", "创", "前", "剑",
            "助", "劭", "势", "勘", "参", "叔", "吏", "嗣", "士", "壮",
            "孺", "守", "宽", "宾", "宋", "宗", "宙", "宣", "实", "宰",
            "尊", "峙", "峻", "崇", "崈", "川", "州", "巡", "帅", "庚",
            "战", "才", "承", "拯", "操", "斋", "昌", "晁", "暠", "曹",
            "曾", "珺", "玮", "珹", "琒", "琛", "琩", "琮", "琸", "瑎",
            "玚", "璟", "璥", "瑜", "生", "畴", "矗", "矢", "石", "磊",
            "砂", "碫", "示", "社", "祖", "祚", "祥", "禅", "稹", "穆",
            "竣", "竦", "综", "缜", "绪", "舱", "舷", "船", "蚩", "襦",
            "轼", "辑", "轩", "子", "杰", "榜", "碧", "葆", "莱", "蒲",
            "天", "乐", "东", "钢", "铎", "铖", "铠", "铸", "铿", "锋",
            "镇", "键", "镰", "馗", "旭", "骏", "骢", "骥", "驹", "驾",
            "骄", "诚", "诤", "赐", "慕", "端", "征", "坚", "建", "弓",
            "强", "彦", "御", "悍", "擎", "攀", "旷", "昂", "晷", "健",
            "冀", "凯", "劻", "啸", "柴", "木", "林", "森", "朴", "骞",
            "寒", "函", "高", "魁", "魏", "鲛", "鲲", "鹰", "丕", "乒",
            "候", "冕", "勰", "备", "宪", "宾", "密", "封", "山", "峰",
            "弼", "彪", "彭", "旁", "日", "明", "昪", "昴", "胜", "汉",
            "涵", "汗", "浩", "涛", "淏", "清", "澜", "浦", "澉", "澎",
            "澔", "濮", "濯", "瀚", "瀛", "灏", "沧", "虚", "豪", "豹",
            "辅", "辈", "迈", "邶", "合", "部", "阔", "雄", "霆", "震",
            "韩", "俯", "颁", "颇", "频", "颔", "风", "飒", "飙", "飚",
            "马", "亮", "仑", "仝", "代", "儋", "利", "力", "劼", "勒",
            "卓", "哲", "喆", "展", "帝", "弛", "弢", "弩", "彰", "征",
            "律", "德", "志", "忠", "思", "振", "挺", "掣", "旲", "旻",
            "昊", "昮", "晋", "晟", "晸", "朕", "朗", "段", "殿", "泰",
            "滕", "炅", "炜", "煜", "煊", "炎", "选", "玄", "勇", "君",
            "稼", "黎", "利", "贤", "谊", "金", "鑫", "辉", "墨", "欧",
            "有", "友", "闻", "问", "澜", "纯", "毓", "悦", "昭", "冰",
            "爽", "琬", "茗", "羽", "希", "芳", "莉", "雅", "芝", "文",
            "晨", "宇", "怡", "全", "子", "凡", "悦", "清", "吉", "克",
            "先", "浩", "泓", "嫣", "倩", "妍", "萱", "雨", "月", "沫",
            "曦", "露", "彤", "情", "伊", "依", "亦", "宜", "艺", "翼",
            "怡", "冰", "悦", "欣", "涵", "萍", "如", "诗", "幂", "娜",
            "薇", "香", "丹", "巧"
            };
            this.t.templates = new string[] { "addition_start", "R.7Fpart1", "addition_end" };
            //兽人，直接复制人类
            this.clone("orc_name", "human_name");
            //精灵，直接复制人类
            this.clone("elf_name", "human_name");
            //矮人，直接复制人类
            this.clone("dwarf_name", "human_name");
            //天族，直接复制人类
            this.clone("Tian_name", "human_name");
            //冥族，直接复制人类
            this.clone("Ming_name", "human_name");
            #endregion

            #region 其他生物名
            this.add(new ChineseNameAsset
            {
                id = "sheep_name",
                onlyByTemplate = true
            });
            this.t.partsList = new List<string> { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };
            this.t.partsList2 = new List<string>
            {
                "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉",
                "戌", "亥"
            };
            this.t.addition_endList = new List<string> { "羊" };
            this.t.templates = new string[] { "part1", "part2", "addition_end" };
            //其他生物与羊同理
            this.clone("penguin_name", "sheep_name");
            this.t.addition_endList = new List<string> { "企鹅" };
            this.clone("turtle_name", "sheep_name");
            this.t.addition_endList = new List<string> { "龟" };
            this.clone("wolf_name", "sheep_name");
            this.t.addition_endList = new List<string> { "狼" };
            this.clone("greg_name", "sheep_name");
            this.t.addition_endList = new List<string> { "格雷格" };
            this.clone("chicken_name", "sheep_name");
            this.t.addition_endList = new List<string> { "鸡" };
            this.clone("alien_name", "sheep_name");
            this.t.addition_endList = new List<string> { "星人" };
            this.clone("ufo_name", "sheep_name");
            this.t.addition_endList = new List<string> { "飞行器" };
            this.clone("cold_one_name", "sheep_name");
            this.t.addition_endList = new List<string> { "恶魔" };
            this.clone("bug_name", "sheep_name");
            this.t.addition_endList = new List<string> { "虫" };
            this.clone("ant_name", "sheep_name");
            this.t.addition_endList = new List<string> { "蚂蚁" };
            this.clone("demon_name", "sheep_name");
            this.t.addition_endList = new List<string> { "恶魔" };
            this.clone("fairy_name", "sheep_name");
            this.t.addition_endList = new List<string> { "精灵" };
            this.clone("crab_name", "sheep_name");
            this.t.addition_endList = new List<string> { "螃蟹" };
            this.clone("cow_name", "sheep_name");
            this.t.addition_endList = new List<string> { "牛" };
            this.clone("evil_mage_name", "sheep_name");
            this.t.addition_endList = new List<string> { "法师" };
            this.clone("rhino_name", "sheep_name");
            this.t.addition_endList = new List<string> { "犀牛" };
            this.clone("monkey_name", "sheep_name");
            this.t.addition_endList = new List<string> { "猴" };
            this.clone("buffalo_name", "sheep_name");
            this.t.addition_endList = new List<string> { "水牛" };
            this.clone("fox_name", "sheep_name");
            this.t.addition_endList = new List<string> { "狐狸" };
            this.clone("hyena_name", "sheep_name");
            this.t.addition_endList = new List<string> { "鬣狗" };
            this.clone("crocodile_name", "sheep_name");
            this.t.addition_endList = new List<string> { "鳄鱼" };
            this.clone("snake_name", "sheep_name");
            this.t.addition_endList = new List<string> { "蛇" };
            this.clone("frog_name", "sheep_name");
            this.t.addition_endList = new List<string> { "蛙" };
            this.clone("bioblob_name", "sheep_name");
            this.t.addition_endList = new List<string> { "生物质" };
            this.clone("assimilator_name", "sheep_name");
            this.t.addition_endList = new List<string> { "同化者" };
            this.clone("fire_skull_name", "sheep_name");
            this.t.addition_endList = new List<string> { "火焰头颅" };
            this.clone("acid_blob_name", "sheep_name");
            this.t.addition_endList = new List<string> { "腐蚀者" };
            this.clone("jumpy_skull_name", "sheep_name");
            this.t.addition_endList = new List<string> { "跳跃(?)头颅" };
            this.clone("lil_pumpkin_name", "sheep_name");
            this.t.addition_endList = new List<string> { "小南瓜" };
            this.clone("rat_name", "sheep_name");
            this.t.addition_endList = new List<string> { "鼠" };
            this.clone("cat_name", "sheep_name");
            this.t.addition_endList = new List<string> { "猫" };
            this.clone("rabbit_name", "sheep_name");
            this.t.addition_endList = new List<string> { "兔" };
            this.clone("piranha_name", "sheep_name");
            this.t.addition_endList = new List<string> { "食人鱼" };
            this.clone("snowman_name", "sheep_name");
            this.t.addition_endList = new List<string> { "雪人" };
            this.clone("bear_name", "sheep_name");
            this.t.addition_endList = new List<string> { "熊" };
            this.clone("skeleton_name", "sheep_name");
            this.t.addition_endList = new List<string> { "骷髅" };
            this.clone("homie_name", "sheep_name");
            this.t.addition_endList = new List<string> { "房子" };
            this.clone("living_plant_name", "sheep_name");
            this.t.addition_endList = new List<string> { "树" };
            this.clone("default_name", "sheep_name");
            this.t.addition_endList = new List<string> { "生物" };
            this.add(new ChineseNameAsset
            {
                id = "MengZhu_name",
                fixedNameChance = 1.2f,
                onlyByTemplate = false,
                fixedList = new List<string> { "変態盟主" }
            });
            #endregion

            #region 功法名
            this.add(new ChineseNameAsset
            {
                id = "book_name",
                fixedNameChance = 0.0001f,
                onlyByTemplate = false,
                templates = new string[] { "part1","part2", "addition_end" }
            });
            this.t.addition_endList = new List<string> { "功法","圣典" };
            this.t.partsList = new List<string> { "青","开","中","上","下", "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸","杨", "朱", "秦", "尤", "许",
            "何", "吕", "施", "张", "孔", "曹", "严", "华", "金", "魏",
            "陶", "姜", "费", "廉", "岑", "薛", "雷" };
            this.t.partsList2 = new List<string> { "水", "火", "金", "土", "木", "元", "灵", "微", "风", "雷", "取", "文", "德" };
            this.t.fixedList = new List<string> { "道心种魔大法", "混沌九转灭神霸决", "降妖伏魔弑神诛仙破天裂地纵横四海横扫八荒唯我独尊圣典" };
            #endregion

            #region 体质名
            this.add(new ChineseNameAsset
            {
                id = "specialBody_name",
                fixedNameChance = 0.0001f,
                onlyByTemplate = false,
                templates = new string[] { "part1", "part2" }
            });
            //这个还是不要按照字随机吧？
            this.t.partsList = new List<string> { "青", "荒", "雷", "天", "元","地" };
            this.t.partsList2 = new List<string> {"阴","阳","初","九"};
            this.t.fixedList = new List<string> { "破灭", "混沌", "纵横" };
            #endregion
        }
    }
}

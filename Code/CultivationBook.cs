using UnityEngine;
namespace Cultivation_Way
{
    internal enum CultivationBookType
    {
        NONE,
        CULTIVATE,
        ATTACK,
        DEFEND,
        MOVE,
        OTHER
    }

    internal class CultivationBook
    {

        public string bookName;//名字

        public int spellCount;//法术数量

        public float[] cultiVelco;//各个境界修炼速度影响因子

        public int rank;//品级，10级一阶

        private uint allowedCultiSystem;//允许修炼的修炼体系（允许多体系

        private CultivationBookType bookType;//功法类型

        public ExtendedSpell[] spells;//法术（内置法术属性

        public MoreStats[] stats;//各个境界加成

        public ChineseElement element;//功法属性

        internal CultivationBookType BookType { get => bookType; }
        internal bool allowCultiSystem(uint cultiSystemID)
        {
            return (allowedCultiSystem & cultiSystemID) > 0;
        }
        internal bool allowActor(ExtendedActorStatus pStatus)
        {
            return allowCultiSystem(AddAssetManager.cultisystemLibrary.get(pStatus.cultisystem).flag);
        }
        internal bool allowActor(ExtendedActor pActor)
        {
            return allowCultiSystem(AddAssetManager.cultisystemLibrary.get(pActor.extendedData.status.cultisystem).flag);
        }
        internal float getMatchDegree(ExtendedActor pActor)
        {
            return ChineseElement.getMatchDegree(pActor.extendedData.status.chineseElement, element,true);
        }
        internal float getMatchDegree(ExtendedActorStatus pStatus)
        {
            return ChineseElement.getMatchDegree(pStatus.chineseElement, element, true);
        }
        public CultivationBook getCopyOne()
        {
            return new CultivationBook(this);
        }
        public bool isSimilar(CultivationBook another)
        {
            return ChineseElement.getMatchDegree(element, another.element, false) < 20;
        }
        public void addNewSpell()
        {
            if (spellCount >=10)
            {
                spells.GetRandom().might *= 1.05f;
                return;
            }
            //UnityEngine.Debug.Log("Try to get new spell...");
            string addSpell = AddAssetManager.extensionSpellLibrary.spellList.GetRandom();
            for (int i = 0; i < spells.Length; i++)
            {
                //Debug.Log("Try count: " + (i + 1));
                if (spells[i] == null)
                {
                    //Debug.Log("Get " + addSpell);
                    spells[i] = new ExtendedSpell(addSpell);
                    spellCount++;
                    return;
                }
                if (spells[i].spellAssetID == addSpell)
                {
                    spells[i].might *= 1.05f;
                    return;
                }
            }
        }
        public CultivationBook modify(int pValue,int[] elementContent)//pValue∈[0,30]
        {
            CultivationBook newBook = this.getCopyOne();
            newBook.element.deflectTo(elementContent, Toolbox.randomFloat(0, 0.8f));
            if (newBook.rank >= 81)
            {
                newBook.rank = 81;
            }
            else
            {
                newBook.rank++;
            }
            for (int i = 0; i < 20; i++) {
                newBook.stats[i] = Toolbox.randomFloat(1f,pValue/500f+1f)*this.stats[i];
            }
            newBook.addNewSpell();
            if (ChineseElement.getMatchDegree(elementContent,newBook.element.baseElementContainer,true)>800)
            {
                newBook.bookName = ChineseNameGenerator.getName("book_name") + "(" + ChineseNameAsset.rankName[(newBook.rank + 9) / 10] + "阶)";
            }
            else
            {
                newBook.bookName = newBook.bookName.Remove(newBook.bookName.Length - 4, 4) + "(" + ChineseNameAsset.rankName[(newBook.rank + 9) / 10] + "阶)";
            }
            return newBook;
        }
        public void setContent(CultivationBook book)
        {
            this.bookName = book.bookName;
            this.bookType = book.bookType;
            this.rank = book.rank;
            this.cultiVelco = book.cultiVelco;
            this.spellCount = book.spellCount;
            this.element = book.element;
            this.allowedCultiSystem = book.allowedCultiSystem;
            for(int i = 0; i < this.spells.Length; i++)
            {
                this.spells[i] = book.spells[i];
            }
            for(int i = 0; i < this.stats.Length; i++)
            {
                this.stats[i] = book.stats[i];
                this.cultiVelco[i] = book.cultiVelco[i];
            }
        }
        public CultivationBook(CultivationBook from)
        {
            bookName = from.bookName;
            bookType = from.BookType;
            allowedCultiSystem = from.allowedCultiSystem;
            rank = from.rank;
            spellCount = from.spellCount;
            element = new ChineseElement(from.element.baseElementContainer);
            spells = new ExtendedSpell[10];
            stats = new MoreStats[20];
            cultiVelco = new float[20];
            for(int i = 0; i < from.spellCount; i++)
            {
                spells[i] = new ExtendedSpell(from.spells[i]);
            }
            for(int i = 0; i < 20; i++)
            {
                stats[i] = new MoreStats().addAnotherStats(from.stats[i]);
                cultiVelco[i] = from.cultiVelco[i];
            }
        }
        public CultivationBook(CultivationBookType bookType = CultivationBookType.CULTIVATE,string bookName = null)
        {
            if (bookName == null)
            {
                this.bookName = ChineseNameGenerator.getName("book_name") + "(凡阶)";
            }
            else
            {
                this.bookName = bookName + "(凡阶)";
            }
            rank = 1;
            spellCount = 0;
            spells = new ExtendedSpell[10];
            cultiVelco = new float[20];
            stats = new MoreStats[20];
            element = new ChineseElement();
            allowedCultiSystem = 2047;
            this.bookType = bookType;

            for (int i = 0; i < 20; i++)
            {
                stats[i] = new MoreStats();
                cultiVelco[i] = 1.0f;
            }
            stats[0].setBasicStats(0, 0, 0, 0);
            for (int i = 1; i < 20; i++)
            {
                stats[i].setBasicStats(i, rank, 0, rank / 20);
                stats[i].addAnotherStats(stats[i - 1]);
            }
        }
    }
}

public class TnsNamesAliasParser
    {
        string[] aliasesForTnsNames;
        List<TnsNamesBlock> blocks = new List<TnsNamesBlock>();

        //1.zaczynaj od lewej . ustal pierwszy znak rowna 
        //2.ustal start i koniec bloku dla pierwszego znaku rowna  , czytaj tak dlugo az uzyskasz parzysta liczne klamer otwarc i zamkniec
        //3.ustal od konca bloku pierwszego znaku rowna kolejny blok 
        //czy znak rowna jest rootem a galezia
        public TnsNamesAliasParser(string fileContents)
        {
            ReadAliases(fileContents);
        }
        public string[] AliasesForTnsNames { 
            get => aliasesForTnsNames; 
        }
        private void ReadAliases(string fileContent)
        {
            string[] endingBrackets;
            int startIdx = 0, endIdx = 0;
            TnsNamesBlock block = null;

            startIdx = FindFirsEqualityCharacterGivesStartIdx(fileContent);
            endIdx = FindEndIdx(fileContent.Substring(startIdx)); //we will see where is close of block
            System.Windows.Forms.MessageBox.Show(startIdx.ToString() + " " + endIdx.ToString());
        }
        private int FindFirsEqualityCharacterGivesStartIdx(string fileContent)
        {
            Match mFirst = Regex.Match(fileContent.ToUpper(), @"=");
            if (mFirst.Success)
                return mFirst.Index;
            else
                return 0;
        }
        private int FindEndIdx(string blockTextFromequalityChar)
        {
            int start = 0;
            int end = 0;
            for (int i = 1; i < blockTextFromequalityChar.Length - 1; i++)
            {
                if (IsCorrectBlock( blockTextFromequalityChar.Substring(start,i) ,out end)) //bez dlugosci 0
                {
                    return end;
                }
            }
            return 0;
        }
        bool IsCorrectBlock(string block, out int endindex)
        {
            int count = 0;
            endindex = 0;
            for (int i = 0; i < block.Length; i++)
            {
                endindex = i;
                switch (block[i])
                {
                    case '(': count++; break;
                    case ')': count--; break;
                    default: break;
                }
            }
            return count == 0;
        }
    }
    public class TnsNamesBlock
    {
        int startIdx = 0;
        int endIdx = 0;
        string aliases = "";
        string definitionArea = "";

        public TnsNamesBlock(int startIdx, int endIdx, string aliases, string definitionArea)
        {
            this.StartIdx = startIdx;
            this.EndIdx = endIdx;
            this.Aliases = aliases;
            this.DefinitionArea = definitionArea;
        }

        public int StartIdx { get => startIdx; set => startIdx = value; }
        public int EndIdx { get => endIdx; set => endIdx = value; }
        public string Aliases { get => aliases; set => aliases = value; }
        public string DefinitionArea { get => definitionArea; set => definitionArea = value; }
    }
    public class TnsNamesAliasExamples
    {
        public static string example = @"A01.WORLD=
  (DESCRIPTION =
    (SDU = 32768)
    (ADDRESS_LIST =
        (ADDRESS =
          (COMMUNITY = SAP.WORLD)
          (PROTOCOL = TCP)
          (HOST = columbus)
          (PORT = 1529)
       )
    (CONNECT_DATA =
       (SID = A01)
       (GLOBAL_NAME = A01.WORLD)
    )
  )

TEC.WORLD=
  (DESCRIPTION =
    (ADDRESS_LIST =
        (ADDRESS =
          (COMMUNITY = TEC.WORLD)
          (PROTOCOL = TCP)
          (HOST = columbus)
          (PORT = 1521)
        )
    )
    (CONNECT_DATA =
       (SID = TEC)
        (GLOBAL_NAME = TEC.WORLD)
        )
)";
        public static string example2 = @"xx=( (aa=) (bb=) ) yy=( (aa=))";
    }

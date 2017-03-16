using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VRGoHelper
{
    public partial class Main : Form
    {
        // 定义下拉列表框
        private ComboBox DropBox = new ComboBox();
        private CsvHelper Helper = new CsvHelper();
        private Dictionary<DropBoxDataSourceType, DataTable> DictionaryDropBoxDataSource = new Dictionary<DropBoxDataSourceType, DataTable>((int)DropBoxDataSourceType.NUM);


        private enum DropBoxDataSourceType
        {
            COUNTRY = 0,
            CATEGORY = 1,
            BRAND = 2,

            NUM = 3,

        }


        public Main()
        {
            InitializeComponent();
            InitDropBox();
            InitData();
        }

        

        private void InitData()
        {

            DataTable CommodityDataTable = new DataTable();
            CommodityContent.DataSource = CommodityDataTable;

            foreach (Commodity Each in Helper.List_Commodity)
            {
                DataRow NewRow = CommodityDataTable.NewRow();

                for (int i = 0; i < Helper.Dictionary_Data.Count; i++)
                {
                    var DataPair = Helper.Dictionary_Data.ElementAt(i);

                    if (!CommodityDataTable.Columns.Contains(DataPair.Key))
                    {
                        CommodityDataTable.Columns.Add(DataPair.Key);
                    }

                    NewRow[DataPair.Key] = Each.CommodityData[i];

                }

                CommodityDataTable.Rows.Add(NewRow);
            }
        }

        private void Buttton_Save_Click(object sender, EventArgs e)
        {
            SaveDataToFile();
            MessageBox.Show("保存成功");
        }

        private void SaveDataToFile()
        {
            string Data = "";

            // append country data
            foreach (var Each in Helper.List_Country)
            {
                Data += Each.ToString();
            }

            Data += "\n\n\n";

            // append category data
            foreach (var Each in Helper.List_Category)
            {
                Data += Each.ToString();
            }

            Data += "\n\n\n";

            // append brand data
            foreach (var Each in Helper.List_Brand)
            {
                Data += Each.ToString();
            }

            Data += "\n\n\n";

            for (int i = 0; i < CommodityContent.Rows.Count - 1; ++i)
            {
                for (int j = 0; j < CommodityContent.Columns.Count; ++j)
                {
                    Data += CommodityContent.Rows[i].Cells[j].Value.ToString() + ",";
                }

                Data = Data.Remove(Data.Length - 1, 1) + "\n";
            }

            File.WriteAllText(Config.CsvFilePath, Data);
        }


        private void InitDropBox()
        {
            DataTable table;

            // country
            table = new DataTable();
            table.Columns.Add("Name");
            foreach(var Each in Helper.List_Country)
            {
                DataRow row = table.NewRow();
                row[0] = Each.Name;
                table.Rows.Add(row);
            }
            DictionaryDropBoxDataSource[DropBoxDataSourceType.COUNTRY] = table;

            // category
            table = new DataTable();
            table.Columns.Add("Name");
            foreach (var Each in Helper.List_Category)
            {
                DataRow row = table.NewRow();
                row[0] = Each.Name;
                table.Rows.Add(row);
            }
            DictionaryDropBoxDataSource[DropBoxDataSourceType.CATEGORY] = table;

            // brand
            table = new DataTable();
            table.Columns.Add("Name");
            foreach (var Each in Helper.List_Brand)
            {
                DataRow row = table.NewRow();
                row[0] = Each.Name.ToString();
                table.Rows.Add(row);
            }
            DictionaryDropBoxDataSource[DropBoxDataSourceType.BRAND] = table;

            DropBox.ValueMember = "Value";
            DropBox.DisplayMember = "Name";
            DropBox.DropDownStyle = ComboBoxStyle.DropDownList;
            DropBox.Visible = false;
            DropBox.SelectionChangeCommitted += new EventHandler(DropBox_SelectionChangeCommitted);

            CommodityContent.Controls.Add(DropBox);
            CommodityContent.CurrentCellChanged += new EventHandler(CommodityContent_CurrentCellChanged);
            CommodityContent.Scroll += new ScrollEventHandler(CommodityContent_Scroll);
            CommodityContent.ColumnWidthChanged += new DataGridViewColumnEventHandler(CommodityContent_ColumnWidthChanged);
        }


        private void SwitchDropBoxDataSource(DropBoxDataSourceType Type)
        {
            DropBox.DataSource = DictionaryDropBoxDataSource[Type];
        }

        private void CommodityContent_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                int ColumnIndexBegin = 3; 
                SwitchDropBoxDataSource((DropBoxDataSourceType)CommodityContent.CurrentCell.ColumnIndex - ColumnIndexBegin);


                Rectangle rect = CommodityContent.GetCellDisplayRectangle(CommodityContent.CurrentCell.ColumnIndex, CommodityContent.CurrentCell.RowIndex, true);

                DropBox.Left = rect.Left;
                DropBox.Top = rect.Top;
                DropBox.Width = rect.Width;
                DropBox.Height = rect.Height;
                DropBox.Text = CommodityContent.CurrentCell.Value.ToString();
                DropBox.Visible = true;
            }
            catch(Exception)
            { }
        }

        private void DropBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CommodityContent.CurrentCell.Value = ((ComboBox)sender).Text;
        }
        
        // 滚动DataGridView时将下拉列表框设为不可见
        private void CommodityContent_Scroll(object sender, ScrollEventArgs e)
        {
            DropBox.Visible = false;
        }

        // 改变DataGridView列宽时将下拉列表框设为不可见
        private void CommodityContent_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            DropBox.Visible = false;
        }
    }


    public class CsvHelper
    {
        public List<Country> List_Country = new List<Country>();
        public List<Category> List_Category = new List<Category>();
        public List<Brand> List_Brand = new List<Brand>();

        public List<Commodity> List_Commodity = new List<Commodity>();

        public Dictionary<string, string> Dictionary_Data = new Dictionary<string, string>();

        public CsvHelper()
        {
            ParseDataFromFile();
            PrepareForDictionaryData();
        }

        private void ParseDataFromFile()
        {
            if (!Directory.Exists(Config.FolderName))
            {
                Directory.CreateDirectory(Config.FolderName);
            }

            if (!File.Exists(Config.CsvFilePath))
            {
                MessageBox.Show("No file\n" + Config.CsvFilePath);
                return;
            }

            string[] Items = File.ReadAllLines(Config.CsvFilePath, System.Text.Encoding.UTF8);

            for (int i = 0; i < Items.Length; i++)
            {
                string EachLine = Items[i];
                string[] DataArr = EachLine.Split(',');

                if (DataArr[0] == Config.COUNTRY)
                {
                    Country NewItem = new Country(DataArr);
                    List_Country.Add(NewItem);
                }
                else if (DataArr[0] == Config.CATEGORY)
                {
                    Category NewItem = new Category(DataArr);
                    List_Category.Add(NewItem);
                }
                else if (DataArr[0] == Config.BRAND)
                {
                    Brand NewItem = new Brand(DataArr);
                    List_Brand.Add(NewItem);
                }
                else if (DataArr[0] == Config.COMMODITY)
                {
                    Commodity NewItem = new Commodity(DataArr);
                    List_Commodity.Add(NewItem);
                }
            }
        }

        private void PrepareForDictionaryData()
        {
            for (int i = 0; i < Config.CommodityAttributes_Chinese.Length; ++i)
            {
                Dictionary_Data[GetKey(i)] = "";
            }
        }

        private string GetKey(int Index)
        {
            return Config.CommodityAttributes_Chinese[Index];
        }
    }

    public class Country
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public Country(string[] Datas)
        {
            Id = int.Parse(Datas[1]);
            Name = Datas[2];
        }

        public override string ToString()
        {
            return Config.COUNTRY + "," + Id + "," + Name + "\n";
        }
    }

    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public Category(string[] Datas)
        {
            Id = int.Parse(Datas[1]);
            Name = Datas[2];
        }

        public override string ToString()
        {
            return Config.CATEGORY + "," + Id + "," + Name + "\n";
        }
    }

    public class Brand
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public Brand(string[] Datas)
        {
            Id = int.Parse(Datas[1]);
            Name = Datas[2];
        }

        public override string ToString()
        {
            return Config.BRAND + "," + Id + "," + Name + "\n";
        }
    }
    

    public class Commodity
    {
        // 表头，自然id, 商品name, 国家id，分类id，品牌id, 海外直邮对应的价格id，跨境保税对应的价格id，国内现货对应的价格id，  -----商品表
        //COMMODITY,0,液体乳钙90粒,0,1:2,0,0,1,2

        public string[] CommodityData;

        public Commodity(string[] Datas)
        {
            CommodityData = Datas;

            /*
            CommodityData = new string[20];
            CommodityData[0] = Datas[0];    // Table Title
            CommodityData[1] = Datas[1];    // ID
            CommodityData[2] = Datas[2];    // Name
            CommodityData[3] = List_Country.Find(it => it.Id == int.Parse(Datas[3].Split(':')[0])).Name;             // Country
            CommodityData[4] = List_Category.Find(it => it.Id == int.Parse(Datas[4].Split(':')[0])).Name;           // Category
            CommodityData[5] = List_Brand.Find(it => it.Id == int.Parse(Datas[5])).Name;                             // Brand

            //海外直邮对应的价格
            CommodityData[6] = List_Price.Find(it => it.Id == int.Parse(Datas[6])).X1.ToString();                             // Brand
            CommodityData[7] = List_Price.Find(it => it.Id == int.Parse(Datas[6])).X2.ToString();                             // Brand
            CommodityData[8] = List_Price.Find(it => it.Id == int.Parse(Datas[6])).X3.ToString();                             // Brand
            CommodityData[9] = List_Price.Find(it => it.Id == int.Parse(Datas[6])).X6.ToString();                             // Brand

            //跨境保税对应的价格
            CommodityData[10] = List_Price.Find(it => it.Id == int.Parse(Datas[7])).X1.ToString();                             // Brand
            CommodityData[11] = List_Price.Find(it => it.Id == int.Parse(Datas[7])).X2.ToString();                             // Brand
            CommodityData[12] = List_Price.Find(it => it.Id == int.Parse(Datas[7])).X3.ToString();                             // Brand
            CommodityData[13] = List_Price.Find(it => it.Id == int.Parse(Datas[7])).X6.ToString();

            //国内现货对应的价格
            CommodityData[14] = List_Price.Find(it => it.Id == int.Parse(Datas[8])).X1.ToString();                             // Brand
            CommodityData[15] = List_Price.Find(it => it.Id == int.Parse(Datas[8])).X2.ToString();                             // Brand
            CommodityData[16] = List_Price.Find(it => it.Id == int.Parse(Datas[8])).X3.ToString();                             // Brand
            CommodityData[17] = List_Price.Find(it => it.Id == int.Parse(Datas[8])).X6.ToString();
            */
        }
    }


    public static class Config
    {
        public const string FolderName = @"C:\VRGo\";
        public const string CsvFileName = "CommodityData.csv";
        public static string CsvFilePath = Path.Combine(FolderName + CsvFileName);
        public static string CsvFilePathTest = "C:\\VRGo\\__CommodityData.csv";

        // csv table
        public const string COUNTRY = "COUNTRY";
        public const string CATEGORY = "CATEGORY";
        public const string BRAND = "BRAND";
        public const string PRICE = "PRICE";
        public const string IMAGE = "IMAGE";
        public const string COMMODITY = "COMMODITY";


        // 表头，自然id, 商品name, 国家id，分类id，品牌id, 海外直邮对应的价格id，跨境保税对应的价格id，国内现货对应的价格id，  -----商品表
        //COMMODITY,0,液体乳钙90粒,0,1:2,0,0,1,2
      
        public static string[] CommodityAttributes_Chinese = {
            "表头(Title)(请勿修改)", "自然序列(ID)(请勿修改)", "商品名(Name)", "国家(Country)", "分类(Catagery)", "品牌(Brand)",
            "海外直邮x1(PirceOverSeaDirectMail)", "海外直邮x2(PirceOverSeaDirectMail)", "海外直邮x3(PirceOverSeaDirectMail)", "海外直邮x6(PirceOverSeaDirectMail)",
            "跨境保税x1(PirceCrossBorderBonded)", "跨境保税x2(PirceCrossBorderBonded)", "跨境保税x3(PirceCrossBorderBonded)", "跨境保税x6(PirceCrossBorderBonded)",
            "国内现货x1(PirceDomesticSpot)", "国内现货x2(PirceDomesticSpot)", "国内现货x3(PirceDomesticSpot)", "国内现货x6(PirceDomesticSpot)"};


        public static string GetCommodityOriginData()
        {
            return "";
        }
    }

}

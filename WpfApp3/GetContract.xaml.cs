using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Word = Microsoft.Office.Interop.Word;



namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для GetContract.xaml
    /// </summary>
    public partial class GetContract : System.Windows.Window
    {
        Dictionary<string, string> info = new Dictionary<string, string>();
        private string workerName,
            jobName;

        public GetContract(string workerName, string jobName)
        {
            InitializeComponent();
            this.workerName = workerName;
            this.jobName = jobName;
            WorkerNameBox.Text = workerName;
            jobTitle.Text = jobName;
    
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            var helper = new WordHelper("D:\\wisualStudio_projects\\WpfApp3\\WpfApp3\\blank-trudovogo-dogovora.docx");
            var values = new Dictionary<string, string>()
            {
                {"<NUM>", documentNumBox.Text },
                {"<DOCYEAR>", "2024" },
                {"<DOCDAY>", "23" },
                {"<DOCMONTH>", "мая"},
                {"<COMPANYTYPE>", CompanyTypeBox.Text},
                {"<COMPANYNAME>", CompanyNameBox.Text},
                {"<COMPANYBOSS>", "Борисов Федор"},
                {"<JOBTITLE>", jobName },
                {"<EMPNAME>", workerName},
                {"<WORKADDRESS>", WorkPlaceAddress.Text},
                {"<STARTDATE>", StartDateBox.Text},
                {"<INTERSHIPLEN>", InternshipBox.Text},
                {"<SALARY>", SalaryBox.Text},
                {"<SALARYOWRDS>", NumberToWords(int.Parse(SalaryBox.Text)) },
                {"<OPD>", PayoutDocument.Text},
                {"<INN>", InnBox.Text},
                {"<PASSPORTNUM>", PassportBox.Text},
                { "<PASSPORTFROM>", "ГУ МВД ПО НСК" },
                {"COMPANYADDRESS", "Кольцово 22"}


            };

            helper.Process(values);
        }

        static bool CheckDictionaryValues(Dictionary<string, string> dict)
        {
            foreach (var pair in dict)
            {
                // Проверяем, пустое ли значение или null
                if (!string.IsNullOrEmpty(pair.Value))
                {
                    MessageBox.Show($"Значение для ключа '{pair.Key}' не пустое.");
                    return false;
                }
            }
            return true;
        }

        

        static string[] units = { "", "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять" };
        static string[] teens = { "", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };
        static string[] tens = { "", "десять", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто" };
        static string[] hundreds = { "", "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот" };
        static string[] thousands = { "", "тысяча", "тысячи", "тысяч" };

        static string NumberToWords(int number)
        {
            if (number == 0)
                return "ноль";

            if (number > 100000)
                throw new ArgumentOutOfRangeException("number", "Число должно быть от 1 до 100000.");

            string words = "";

            if ((number / 1000) > 0)
            {
                words += hundreds[number / 1000] + " ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += hundreds[number / 100] + " ";
                number %= 100;
            }

            if ((number / 10) > 1)
            {
                words += tens[number / 10] + " ";
                number %= 10;
            }
            else if (number / 10 == 1)
            {
                words += teens[number % 10] + " ";
                number = 0;
            }

            if (number > 0)
            {
                words += units[number];
            }

            return words;
        }




    }
}

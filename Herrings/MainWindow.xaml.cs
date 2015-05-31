using CsvHelper;
using Herrings.Logic;
using Herrings.Logic.Csv;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Herrings
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Corporation corporation = new Corporation();
            loadCorporation(corporation);
        }

        private void CsvPicker_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV data files (*.csv)|*.csv|All Files|*.*";
            if (ofd.ShowDialog() == true)
            {
                string filePath = ofd.FileName;
                string safeFilePath = ofd.SafeFileName;
                using (var reader = new CsvReader(new StreamReader(filePath)))
                {
                    reader.Configuration.RegisterClassMap<ShareholderMap>();
                    Dictionary<int, Shareholder> shareholders = new Dictionary<int, Shareholder>();
                    while (reader.Read())
                    {
                        try
                        {
                            Shareholder instance = reader.GetRecord<Shareholder>();
                            shareholders.Add(instance, instance);
                        }
                        catch (Exception ex)
                        {
                            MessageBoxResult result = MessageBox.Show("Provided CSV file is invalid. Look into the instructions section and try again.", "Invalid CSV data format", MessageBoxButton.OK, MessageBoxImage.Warning);
                            //if (result == MessageBoxResult.OK || result == MessageBoxResult.None || result == MessageBoxResult.Cancel)
                            //{
                                
                            //}
                            return;
                        }
                    }
                    foreach(int shareholderId in shareholders.Keys)
                    {
                        int spiedId = shareholders[shareholderId].SpiedId;
                        if(spiedId > 0) {
                            shareholders[shareholderId].SpiedShareholder = shareholders[spiedId];
                        }
                    }
                    Corporation corporation = new Corporation(shareholders);
                    loadCorporation(corporation);
                }
            }
        }

        private void loadCorporation(Corporation corporation)
        {
            getCorporationStructure(corporation);
            getResultsForCorporation(corporation);
        }

        private void getCorporationStructure(Corporation corporation)
        {
            string structure = "Corporation shareholders:\n\n";
            foreach (Shareholder shareholder in corporation.Shareholders)
            {
                structure += shareholder.ToString() + "\n";
            }
            structure += String.Format("\nTotal corporation shares: {0}\n", corporation.getTotalShares());
            Structure.Text = structure;
        }

        private void getResultsForCorporation(Corporation corporation)
        {
            KeyValuePair<float, List<Shareholder>> bestGroup = corporation.getIndependentShareholdersWithHighestShare();
            string result = "Most influential, independent group in corporation:\n\n";
            foreach (Shareholder shareholder in bestGroup.Value)
            {
                result += shareholder.ToString() + "\n";
            }
            double totalPercent = Math.Round((bestGroup.Key / corporation.getTotalShares()), 4) * 100;
            result += String.Format("\nTotal group share: {0} ({1}%)\n", bestGroup.Key, totalPercent);
            Results.Text = result;
        }
    }
}

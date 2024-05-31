using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFLab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public class NameConvertor : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            => $"{values[0]} {values[1]}";

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<Employee> _employees;
        public MainWindow()
        {
            InitializeComponent();
            _employees = [];
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            List<string> csvContainer = [];
            OpenFileDialog dial = new();
            dial.Filter = "Csv file (*.csv)|*.csv";
            if (dial.ShowDialog()!.Value)
            {
                csvContainer = [.. System.IO.File.ReadAllText(dial.FileName).Split(['\n', '\r'])];
                for (int i = 1; i < csvContainer.Count - 1; ++i) 
                {
                    var properties = csvContainer[i].Split(';');
                    _employees.Add(new Employee(properties[0], properties[1], properties[2], DateTime.ParseExact(properties[3], "dd.MM.yyyy", CultureInfo.InvariantCulture), properties[4], int.Parse(properties[5]), (Currency)int.Parse(properties[6]), (Role)int.Parse(properties[6])));
                }
            }

            DataContext = _employees;
        }
    }
}
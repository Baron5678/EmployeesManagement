using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.WebSockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFLab2
{
    public class SexConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sexString = value as string ?? throw new ArgumentException("Not string");
            return sexString == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
        { 
            var isChecked = value as bool? ?? throw new ArgumentException("Not bool");
            if (isChecked)
                return parameter.ToString()!;
            return string.Empty;
        }
           
    }
    public partial class AddNewEmployee : Window
    {
        public Employee NewEmployee { get; private set; }
        private readonly ObservableCollection<Employee> _employees;
        public AddNewEmployee(ObservableCollection<Employee> employees)
        {
            InitializeComponent();
            _employees = employees;
            NewEmployee = new();
            DataContext = NewEmployee;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            _employees.Add(NewEmployee);
            NewEmployee = new();
            DataContext = NewEmployee;
        }
    }
}

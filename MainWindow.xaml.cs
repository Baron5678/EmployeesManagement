using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFLab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public class MinSalaryValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var minSalary = value as string ?? throw new ArgumentException("Not string");
            if (int.TryParse(minSalary, out int res))
            {
                if (res < 5000)
                    return new ValidationResult(false, "Salary must be greater than 5000!");
            }
            else
            {
                return new ValidationResult(false, "Not a number!");
            }

            return ValidationResult.ValidResult;
        }
    }

    public class ValidationErrorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is bool hasError && hasError && values[1] is ReadOnlyObservableCollection<ValidationError> errors && errors.Count > 0)
            {
                return errors[0].ErrorContent;
            }

            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VisibilityConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var IsError = value as bool? ?? throw new ArgumentException("Not bool");
            return IsError ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => DependencyProperty.UnsetValue;
    }

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
        private AddNewEmployee? _adder;
        private string _sourceFilePath;
        private string _header;
        private bool _isChanged = false;
        private bool _isInitialized = false;
        private bool _isOpened = false;
        public MainWindow()
        {
            InitializeComponent();
            _employees = [];
            _sourceFilePath = string.Empty;
            _header = string.Empty;
            _adder = null;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var action = SaveIfChanged();
            if (action == MessageBoxResult.Cancel)
            {
                _isChanged = true;
                return;
            }

            List<string> csvContainer = [];
            OpenFileDialog dial = new();
            dial.Filter = "Csv file (*.csv)|*.csv";
            if (dial.ShowDialog()!.Value)
            {
                _sourceFilePath = dial.FileName;
                csvContainer = [.. System.IO.File.ReadAllText(dial.FileName).Split('\n')];
                _header = csvContainer[0];
                for (int i = 1, j = 0; i < csvContainer.Count; ++i, ++j)
                {
                    var properties = csvContainer[i].Split(';');
                    if (csvContainer[i] != "")
                    {
                        var employee = new Employee(properties[0],
                                                    properties[1],
                                                    properties[2],
                                                    DateTime.ParseExact(properties[3], "dd.MM.yyyy", CultureInfo.InvariantCulture),
                                                    properties[4], int.Parse(properties[5]),
                                                    (Currency)int.Parse(properties[6]),
                                                    (Role)int.Parse(properties[7]));
                        employee.PropertyChanged += EmployeePropertyChanged;
                        if (_isInitialized)
                        {
                            if (j >= _employees.Count)
                            {
                                _employees.Add(employee);
                            }
                            else
                            {
                                _employees[j] = employee;
                            }
                        }
                        else
                        {
                            _employees.Add(employee);
                        }

                    }

                }

                if (_employees.Count != 0)
                {
                    if (_employees.Count > csvContainer.Count - 2)
                    {
                        for (int i = csvContainer.Count - 2; i < _employees.Count; ++i)
                            _employees.RemoveAt(i);
                    }
                }

                _isInitialized = true;
            }

            DataContext = _employees;
        }

        private void EmployeePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
            => _isChanged = true;


        private void MoveDown(object sender, RoutedEventArgs e)
        {
            int position = EmployeeList.SelectedIndex;
            if (position < _employees.Count - 1)
            {
                (_employees[position + 1], _employees[position]) = (_employees[position], _employees[position + 1]);
                EmployeeList.SelectedIndex = position + 1;
            }
        }

        private void MoveUp(object sender, RoutedEventArgs e)
        {
            int position = EmployeeList.SelectedIndex;
            if (position > 0)
            {
                (_employees[position - 1], _employees[position]) = (_employees[position], _employees[position - 1]);
                EmployeeList.SelectedIndex = position - 1;
            }

        }

        private void Save(object sender, RoutedEventArgs e)
        {
            string data = CreateCSVString();
            System.IO.File.WriteAllText(_sourceFilePath, data);
            _isChanged = false;
        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Csv file (*.csv)|*.csv",
                Title = "Save As",
                DefaultExt = "csv",
                AddExtension = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using StreamWriter writer = new(saveFileDialog.FileName);
                writer.Write(CreateCSVString());

            }

            _isChanged = false;

        }

        private void Close(object sender, RoutedEventArgs e)
        {
            var action = SaveIfChanged();
            if (action == MessageBoxResult.Cancel)
            {
                _isChanged = true;
            }
            else
            {
                Close();
                _adder?.Close();
            }

        }

        private string CreateCSVString()
        {
            StringBuilder csvStringBuilder = new(_header + "\n");
            foreach (var employee in _employees)
            {
                csvStringBuilder.Append($"{employee.FirstName};{employee.LastName};{employee.Sex};{employee.BirthDate.ToString("dd.MM.yyyy")};{employee.BirthCountry};{employee.Salary};{(int)employee.SalaryCurrency};{(int)employee.CompanyRole}\n");
            }
            return csvStringBuilder.ToString();
        }

        private MessageBoxResult SaveIfChanged()
        {
            MessageBoxResult msgBoxRes = MessageBoxResult.None;
            if (_isChanged)
            {
                msgBoxRes = MessageBox.Show("Do you want to save changes?", "Save changes", MessageBoxButton.YesNoCancel);
                if (msgBoxRes == MessageBoxResult.Yes)
                {
                    Save(this, new RoutedEventArgs());
                }
                else if (msgBoxRes == MessageBoxResult.No)
                {
                    _isChanged = false;
                }
            }
            return msgBoxRes;
        }

        private void RemoveEmployee(object sender, RoutedEventArgs e)
        {
            if (EmployeeList.SelectedItem is not null)
            {
                _employees.Remove((Employee)EmployeeList.SelectedItem);
                _isChanged = true;
            }
        }

        private void AddEmployee(object sender, RoutedEventArgs e)
        {
            if (!_isOpened)
            {
                _adder = new AddNewEmployee(_employees);
                _adder.Show();
                _adder.Closed += Adder_Closed;
                _isOpened = true;
            }

            if (_adder is not null && _adder.WindowState == WindowState.Minimized)
                _adder.WindowState = WindowState.Normal;
        }

        private void Adder_Closed(object? sender, EventArgs e)
        {
            _isOpened = false;
            _adder = null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLab2
{
    public enum Currency { PLN, USD, EUR, GBP, NOK }
    public enum Role { Worker, SeniorWorker, IT, Manager, Director, CEO }
    public class Employee : INotifyPropertyChanged
    { 
        private string _firstName;
        private string _lastName;
        private string _sex;
        private DateTime _birthDate;
        private string _birthCountry;
        private int _salary;
        private Currency _salaryCurrency;
        private Role _companyRole;

        //  Properties for the fields   
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string Sex 
        {
            get => _sex;
            set 
            {
                _sex = value;
                OnPropertyChanged(nameof(Sex));
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }
        
        public string BirthCountry
        {
            get => _birthCountry;
            set
            {
                _birthCountry = value;
                OnPropertyChanged(nameof(BirthCountry));
            }
        }

        public int Salary
        {
            get => _salary;
            set
            {
                _salary = value;
                OnPropertyChanged(nameof(Salary));
            }
        }

        public Currency SalaryCurrency
        {
            get => _salaryCurrency;
            set
            {
                _salaryCurrency = value;
                OnPropertyChanged(nameof(SalaryCurrency));
            }
        }

        public Role CompanyRole
        {
            get => _companyRole;
            set
            {
                _companyRole = value;
                OnPropertyChanged(nameof(CompanyRole));
            }
        }

        public Employee() 
        {
            _firstName = string.Empty;
            _lastName = string.Empty;
            _birthCountry = string.Empty;
            _sex = "Male";
            _birthDate = DateTime.Now.AddYears(-30);
            _salary = 5000;
            _salaryCurrency = Currency.PLN;
            _companyRole = Role.Worker;
        }

        public Employee(string firstName, string lastName, string sex, DateTime birthDate, string birthCountry, int salary, Currency salaryCurrency, Role companyRole)
        {
            _firstName = firstName;
            _lastName = lastName;
            _sex = sex;
            _birthDate = birthDate;
            _birthCountry = birthCountry;
            _salary = salary;
            _salaryCurrency = salaryCurrency;
            _companyRole = companyRole;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFLab2
{
    public class CompanyRoleDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; } 
        public DataTemplate? CEOTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            if (item is Employee employee)
            {
                switch (employee.CompanyRole)
                {
                    case Role.CEO:
                        return CEOTemplate is not null ? CEOTemplate : base.SelectTemplate(item, container);
                    default:
                        return DefaultTemplate is not null ? DefaultTemplate : base.SelectTemplate(item, container);
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}

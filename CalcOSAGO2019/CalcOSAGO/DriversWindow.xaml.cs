using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CalcOSAGO
{
    /// <summary>
    /// Логика взаимодействия для DriversWindow.xaml
    /// </summary>
    public partial class DriversWindow : Window
    {
        public List<Driver> _drivers = new List<Driver>();
        public DriversWindow(List<Driver> drivers)
        {
            InitializeComponent();
            if(drivers.Count > 0)
            {
                _drivers = drivers;
            }
            DriversList.ItemsSource = _drivers;
            DriversList.Items.Refresh();
        }
        /// <summary>
        /// Обработчик добавления допущенного водителя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDriver(object sender, RoutedEventArgs e)
        {
            // максимальное количество водителей = 4
            if(_drivers.Count < 4)
            {
                Driver tmp_driver = new Driver();
                _drivers.Add(tmp_driver);
                DriversList.ItemsSource = _drivers;
                DriversList.Items.Refresh();
            }
        }
        /// <summary>
        /// Обработчик удаления допущенного водителя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelDriver(object sender, RoutedEventArgs e)
        {
            // если что-то выбрано
            if(DriversList.SelectedIndex != -1)
            {
                _drivers.RemoveAt(DriversList.SelectedIndex);
            }
            DriversList.ItemsSource = _drivers;
            DriversList.Items.Refresh();
        }
        /// <summary>
        /// Обработчик принятия изменений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Accept(object sender, RoutedEventArgs e)
        {
            //возвращаем результат выбора
            if (_drivers.Count > 0)
            {
                //закрываем окно
                DialogResult = true;
            }
        }
    }
}

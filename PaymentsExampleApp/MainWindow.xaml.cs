using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaymentsExampleApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PaymentsBaseEntities _context = new PaymentsBaseEntities(); // Создаем область построения графиков

        public MainWindow()
        {
            InitializeComponent();
            ChartPayments.ChartAreas.Add(new ChartArea("Main"));

            var currentSeries = new Series("Payments") //Добавляем наборы данных
            {
                IsValueShownAsLabel = true
            };
            ChartPayments.Series.Add(currentSeries);

            ComboUsers.ItemsSource = _context.User.ToList(); //Загружаем данные из базы
            ComboChartTypes.ItemsSource = Enum.GetValues(typeof(SeriesChartType));

        }

        private void UpdateChart(object sender, SelectionChangedEventArgs e)
        {
            if (ComboUsers.SelectedItem is User currentUser && 
                ComboChartTypes.SelectedItem is SeriesChartType currentType) // Получаем выбранные значения в выпадающих списках
            {
                Series currentSeries = ChartPayments.Series.FirstOrDefault(); // Обрабатываем данные программы
                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();

                var categoriesList = _context.Category.ToList(); // Обрабатываем список категорий 
                foreach (var category in categoriesList)
                {
                    currentSeries.Points.AddXY(category.Name,
                        _context.Payment.ToList().Where(p => p.User == currentUser
                        && p.Category == category).Sum(p => p.Price * p.Num));
                }
            }
        }
    }
}

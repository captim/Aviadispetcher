using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace Aviadispetcher
{
    public partial class MainWindow : Window
    {
        private int flightNum;
        private bool flightAdd = false;
        public FlightList fList = new FlightList();
        private List<string> allCities = new List<string>();
        public static string selectedCity;
        public static TimeSpan timeFlight;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenDbFile()
        {
            try
            {
                fList = DBConnection.GetInstance().GetAllFlights();
                FlightListDG.ItemsSource = fList.Flights_list;
            }
            catch (Exception ex)
            {
                string errMsg = "";
                if (ex.Message == "Unable to connect to any of the specified MySQL hosts.")
                {
                    errMsg = "Підключіть веб-сервер MySQL та виконайте команду Файл-Завантажити";
                }
                else
                {
                    errMsg = "Для завантаження файлу виконайте команду Файл-Завантажити";
                }
                MessageBox.Show(ex.Message + char.ConvertFromUtf32(13) + char.ConvertFromUtf32(13) +
                                errMsg, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InfoFlightForm_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = FlightListDG.Margin.Left + FlightListDG.RenderSize.Width + FlightListDG.Margin.Right + 60;
            OpenDbFile();
            flightGroupBox.Visibility = Visibility.Hidden;
            selFlightGroupBox.Visibility = Visibility.Hidden; 
        }

        private void LoadDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FlightListDG.ItemsSource = null;
                fList.Flights_list.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + char.ConvertFromUtf32(13),
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            OpenDbFile();
            flightGroupBox.Visibility = Visibility.Hidden;
            this.Width = FlightListDG.Margin.Left + FlightListDG.RenderSize.Width + FlightListDG.Margin.Right + 60;
        }

        private void AddDataMenuItem_Click(Object sender, RoutedEventArgs e)
        {
            flightGroupBox.Visibility = Visibility.Visible;
            this.Width = FlightListDG.Margin.Left + FlightListDG.RenderSize.Width + 20 + 
                flightGroupBox.Margin.Right + flightGroupBox.RenderSize.Width + 35;
            flightAdd = true;
            flightNum = fList.Flights_list.Count;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeFlightListData(flightNum);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void EditDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            flightGroupBox.Visibility = Visibility.Visible;
            this.Width = FlightListDG.Margin.Left + FlightListDG.RenderSize.Width + 20 +
                flightGroupBox.Margin.Right + flightGroupBox.RenderSize.Width + 35;
            flightAdd = false;
        }

        private void FlightListDG_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Flight editedFlight = FlightListDG.SelectedItem as Flight;
            try
            {
                numFlightTextBox.Text = editedFlight.number;
                cityFlightTextBox.Text = editedFlight.city;
                timeFlightTextBox.Text = editedFlight.depature_time.ToString(@"hh\:mm");
                freeSeatsTextBox.Text = editedFlight.free_seats.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + char.ConvertFromUtf32(13) + char.ConvertFromUtf32(13), "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            flightNum = FlightListDG.SelectedIndex;
        }

        private void ChangeFlightListData(int num)
        {
            if (flightAdd && fList.Flights_list.Count == FlightList.MAX_AMOUNT)
            {
                MessageBox.Show("Неможливо додати рейс. Максимальна кількість рейсів = 85. Оберіть рейс для заміни", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            TimeSpan depTime;
            if (flightAdd)
            {
                fList.Add(new Flight(fList.Flights_list.Count + 1, "", "", TimeSpan.Zero, 0));
                num = fList.Flights_list.Count - 1;
            }
            fList.Flights_list[num].number = numFlightTextBox.Text;
            fList.Flights_list[num].city = cityFlightTextBox.Text;
            if (TimeSpan.TryParse(timeFlightTextBox.Text, out depTime))
            {
                fList.Flights_list[num].depature_time = depTime;
            }
            fList.Flights_list[num].free_seats = Convert.ToInt16(freeSeatsTextBox.Text);
            FlightListDG.ItemsSource = null;
            FlightListDG.ItemsSource = fList.Flights_list;
            try
            {
                if (flightAdd)
                {
                    DBConnection db = DBConnection.GetInstance();
                    db.Update(fList.Flights_list[num]);
                    fList.Flights_list[num].id = db.GetMaxId() + 1;
                }
                else
                {
                    DBConnection.GetInstance().Add(fList.Flights_list[num]);
                }
            } catch (Exception ex)
            {
                string errMsg;
                if (ex.Message == "Unable to connect to any of the specificated MySQL hosts.")
                {
                    errMsg = "Підключіть веб-сервер MySQL та завантажте дані командою Файл-Завантажити";
                }
                else
                {
                    errMsg = "Для завантаження даних виконайте команду Файл - Завантажити";
                }
                MessageBox.Show(ex.Message + char.ConvertFromUtf32(13) + char.ConvertFromUtf32(13) + errMsg, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectXMenuItem_Click(object sender, RoutedEventArgs e)
        {
            selFlightGroupBox.Visibility = Visibility.Visible;
            sTime.IsEnabled = false;
            this.Width = FlightListDG.Margin.Left + FlightListDG.RenderSize.Width + 20 +
                flightGroupBox.Margin.Right + flightGroupBox.RenderSize.Width + 35;
            allCities = DBConnection.GetInstance().SelectAllCities();
            cityList.ItemsSource = allCities;
        }
        private void SelectXYMenuItem_Click(object sender, RoutedEventArgs e)
        {
            selFlightGroupBox.Visibility = Visibility.Visible;
            sTime.IsEnabled = true;
            this.Width = FlightListDG.Margin.Left + FlightListDG.RenderSize.Width + 20 +
                flightGroupBox.Margin.Right + flightGroupBox.RenderSize.Width + 35;
            allCities = DBConnection.GetInstance().SelectAllCities();
            cityList.ItemsSource = allCities;
        }
        private void selBtn_Click(object sender, RoutedEventArgs e)
        {
            selectedCity = (string)cityList.SelectedItem;
            if (selectedCity == null)
            {
                MessageBox.Show("Оберіть пункт призначення", "Увага", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string selectedTime = sTime.Text;
            if (selectedTime.Equals(""))
            {
                FlightListDG.ItemsSource = SelectData.SelectX(fList, selectedCity);
            }
            else if (TimeSpan.TryParse(selectedTime, out timeFlight))
            {
                FlightListDG.ItemsSource = SelectData.SelectXY(fList, selectedCity, timeFlight);
            }
        }

        private void saveSelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sTime.IsEnabled)
            {
                new ConvertDataInDoc().ConvertFlightListInDoc(SelectData.SelectX(fList, selectedCity), SelectData.SelectXY(fList, selectedCity, timeFlight));
            } else
            {
                new ConvertDataInDoc().ConvertFlightListInDoc(SelectData.SelectX(fList, selectedCity), new List<Flight>());
            }
        }
    }

}

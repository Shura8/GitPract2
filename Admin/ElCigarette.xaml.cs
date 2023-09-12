using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VapeAPI.Models;

namespace Admin
{
    /// <summary>
    /// Логика взаимодействия для ElCigarette.xaml
    /// </summary>
    public partial class ElCigarette : Window
    {
        AuthWin main = new AuthWin();
        public ElCigarette()
        {
            InitializeComponent();
            GetElCigarette();
        }

        public async void GetElCigarette()
        {
            var response = await main.client.GetStringAsync("ElectronicCigarettes");
            var electronicCigarettes = JsonConvert.DeserializeObject<List<ElectronicCigarette>>(response);
            dgElCigarettes.ItemsSource = electronicCigarettes;
            dgElCigarettes.Columns[0].Visibility = Visibility.Hidden;
            dgElCigarettes.Columns[1].Header = "Название эл.сигареты";
            dgElCigarettes.Columns[2].Header = "Описание эл.сигареты";
            dgElCigarettes.Columns[3].Header = "Id типа эл.сигареты";
            dgElCigarettes.Columns[4].Header = "Кол-во тяг";
            dgElCigarettes.Columns[5].Header = "Стоимость эл.сигареты";
            dgElCigarettes.Columns[6].Header = "Кол-во эл.сигарет";
            dgElCigarettes.Columns[7].Header = "Удалена";
        }

        public async void AddElCigarette(ElectronicCigarette electronic)
        {
            await main.client.PostAsJsonAsync("ElectronicCigarettes", electronic);
            GetElCigarette();
        }
        public async void UpdateElCigarette(ElectronicCigarette electronic)
        {
            var response = await main.client.GetStringAsync("ElectronicCigarettes");
            var electronicCigarettes = JsonConvert.DeserializeObject<List<ElectronicCigarette>>(response);
            foreach (var c in electronicCigarettes)
            {
                if (c.IdElectronicCigarette == electronic.IdElectronicCigarette)
                {
                    if (c.IsDeleted == false)
                    {
                        await main.client.PutAsJsonAsync("ElectronicCigarettes/" + electronic.IdElectronicCigarette, electronic);
                        break;
                    }
                }
            }
            GetElCigarette();
        }

        public async void DelElCigarette(int ElectronicCigaretteId)
        {
            var response = await main.client.GetStringAsync("ElectronicCigarettes");
            var electronicCigarettes = JsonConvert.DeserializeObject<List<ElectronicCigarette>>(response);
            foreach (var c in electronicCigarettes)
            {
                if (c.IdElectronicCigarette == ElectronicCigaretteId)
                {
                    if (c.IsDeleted == false)
                    {
                        var electronic = new ElectronicCigarette()
                        {
                            IdElectronicCigarette = c.IdElectronicCigarette,
                            NameElectronicCigarette = c.NameElectronicCigarette,
                            DescribeElectronicCigarette = c.DescribeElectronicCigarette,
                            TypeElectronicCigaretteId = c.TypeElectronicCigaretteId,
                            TractionElectronicCigarette = c.TractionElectronicCigarette,
                            CostElectronicCigarette = c.CostElectronicCigarette,
                            CountElectronicCigarette = c.CountElectronicCigarette,
                            IsDeleted = true
                        };
                        await main.client.PutAsJsonAsync("ElectronicCigarettes/" + electronic.IdElectronicCigarette, electronic);
                        break;
                    }
                    else
                    {
                        var electronic = new ElectronicCigarette()
                        {
                            IdElectronicCigarette = c.IdElectronicCigarette,
                            NameElectronicCigarette = c.NameElectronicCigarette,
                            DescribeElectronicCigarette = c.DescribeElectronicCigarette,
                            TypeElectronicCigaretteId = c.TypeElectronicCigaretteId,
                            TractionElectronicCigarette = c.TractionElectronicCigarette,
                            CostElectronicCigarette = c.CostElectronicCigarette,
                            CountElectronicCigarette = c.CountElectronicCigarette,
                            IsDeleted = false
                        };
                        await main.client.PutAsJsonAsync("ElectronicCigarettes/" + electronic.IdElectronicCigarette, electronic);
                        break;
                    }
                }
            }
            GetElCigarette();
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var electronic = new ElectronicCigarette()
                {
                    NameElectronicCigarette = name.Text,
                    DescribeElectronicCigarette = describe.Text,
                    TypeElectronicCigaretteId = Convert.ToInt32(typeId.Text),
                    TractionElectronicCigarette = Convert.ToInt32(traction.Text),
                    CostElectronicCigarette = Convert.ToInt32(cost.Text),
                    CountElectronicCigarette = Convert.ToInt32(count.Text),
                    IsDeleted = false
                };
                AddElCigarette(electronic);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }
        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var electronic = new ElectronicCigarette()
                {
                    IdElectronicCigarette = dgElCigarettes.SelectedIndex + 1,
                    NameElectronicCigarette = name.Text,
                    DescribeElectronicCigarette = describe.Text,
                    TypeElectronicCigaretteId = Convert.ToInt32(typeId.Text),
                    TractionElectronicCigarette = Convert.ToInt32(traction.Text),
                    CostElectronicCigarette = Convert.ToInt32(cost.Text),
                    CountElectronicCigarette = Convert.ToInt32(count.Text),
                    IsDeleted = false
                };
                UpdateElCigarette(electronic);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }
        }

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            DelElCigarette(dgElCigarettes.SelectedIndex + 1);
        }
    }
}

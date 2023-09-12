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
    /// Логика взаимодействия для Cigarette.xaml
    /// </summary>
    public partial class Cigarette : Window
    {
        AuthWin main = new AuthWin();
        public Cigarette()
        {
            InitializeComponent();
            GetCigarette();
        }

        public async void GetCigarette()
        {
            var response = await main.client.GetStringAsync("Cigarettes");
            var cigarettes = JsonConvert.DeserializeObject<List<Cigarettes>>(response);
            dgCigarette.ItemsSource = cigarettes;
            dgCigarette.Columns[0].Visibility = Visibility.Hidden;
            dgCigarette.Columns[1].Header = "Название сигареты";
            dgCigarette.Columns[2].Header = "Описание сигареты";
            dgCigarette.Columns[3].Header = "Id табака";
            dgCigarette.Columns[4].Header = "Стоимость сигареты";
            dgCigarette.Columns[5].Header = "Кол-во пачек сигарет";
            dgCigarette.Columns[6].Header = "Удалена";
        }

        public async void AddCigarette(Cigarettes cigarettes)
        {
            await main.client.PostAsJsonAsync("Cigarettes", cigarettes);
            GetCigarette();
        }
        public async void UpdateCigarette(Cigarettes cigarette)
        {
            var response = await main.client.GetStringAsync("Cigarettes");
            var cigarettes = JsonConvert.DeserializeObject<List<Cigarettes>>(response);
            foreach (var c in cigarettes)
            {
                if (c.IdCigarette == cigarette.IdCigarette)
                {
                    if (c.IsDeleted == false)
                    {
                        await main.client.PutAsJsonAsync("Cigarettes/" + cigarette.IdCigarette, cigarette);
                        break;
                    }
                }
            }
            GetCigarette();
        }
        public async void DelCigarette(int CigarettesId)
        {
            var response = await main.client.GetStringAsync("Cigarettes");
            var cigarettes = JsonConvert.DeserializeObject<List<Cigarettes>>(response);
            foreach(var c in cigarettes)
            {
                if(c.IdCigarette == CigarettesId)
                {
                    if(c.IsDeleted == false)
                    {
                        var cigarette = new Cigarettes()
                        {
                            IdCigarette = c.IdCigarette,
                            NameCigarette = c.NameCigarette,
                            DescribeCigarette = c.DescribeCigarette,
                            TobaccoId = c.TobaccoId,
                            CostCigarette = c.CostCigarette,
                            CountCigarette = c.CountCigarette,
                            IsDeleted = true
                        };
                        await main.client.PutAsJsonAsync("Cigarettes/" + cigarette.IdCigarette, cigarette);
                        break;
                    }
                    else
                    {
                        var cigarette = new Cigarettes()
                        {
                            IdCigarette = c.IdCigarette,
                            NameCigarette = c.NameCigarette,
                            DescribeCigarette = c.DescribeCigarette,
                            TobaccoId = c.TobaccoId,
                            CostCigarette = c.CostCigarette,
                            CountCigarette = c.CountCigarette,
                            IsDeleted = false
                        };
                        await main.client.PutAsJsonAsync("Cigarettes/" + cigarette.IdCigarette, cigarette);
                        break;
                    }
                }
            }
            GetCigarette();
        }
        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cigarettes = new Cigarettes()
                {
                    NameCigarette = name.Text,
                    DescribeCigarette = describe.Text,
                    TobaccoId = Convert.ToInt32(tobaccoId.Text),
                    CostCigarette = Convert.ToInt32(cost.Text),
                    CountCigarette = Convert.ToInt32(count.Text),
                    IsDeleted = false
                };
                AddCigarette(cigarettes);
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
                var cigarettes = new Cigarettes()
                {
                    IdCigarette = dgCigarette.SelectedIndex + 1,
                    NameCigarette = name.Text,
                    DescribeCigarette = describe.Text,
                    TobaccoId = Convert.ToInt32(tobaccoId.Text),
                    CostCigarette = Convert.ToInt32(cost.Text),
                    CountCigarette = Convert.ToInt32(count.Text),
                    IsDeleted = false
                };
                UpdateCigarette(cigarettes);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }

        }

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            DelCigarette(dgCigarette.SelectedIndex + 1);
        }
    }
}

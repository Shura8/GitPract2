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
    /// Логика взаимодействия для Tobacco.xaml
    /// </summary>
    public partial class Tobacco : Window
    {
        AuthWin main = new AuthWin();
        public Tobacco()
        {
            InitializeComponent();
            GetTobacco();
        }

        public async void GetTobacco()
        {
            var response = await main.client.GetStringAsync("Tobacco");
            var tobacoo = JsonConvert.DeserializeObject<List<Tobaccos>>(response);
            dgTobacco.ItemsSource = tobacoo;
            dgTobacco.Columns[0].Visibility = Visibility.Hidden;
            dgTobacco.Columns[1].Header = "Название табака";
            dgTobacco.Columns[2].Header = "Страна производства табака";
            dgTobacco.Columns[3].Header = "Id крепкости";
            dgTobacco.Columns[4].Header = "Удалена";
        }

        public async void AddTobacco(Tobaccos tobacoo)
        {
            await main.client.PostAsJsonAsync("Tobacco", tobacoo);
            GetTobacco();
        }
        public async void UpdateTobacco(Tobaccos tobacoo)
        {
            var response = await main.client.GetStringAsync("Tobacco");
            var tobacoos = JsonConvert.DeserializeObject<List<Tobaccos>>(response);
            foreach (var c in tobacoos)
            {
                if (c.IdTobacco == tobacoo.IdTobacco)
                {
                    if (c.IsDeleted == false)
                    {
                        await main.client.PutAsJsonAsync("Tobacco/" + tobacoo.IdTobacco, tobacoo);
                        break;
                    }
                }
            }
            GetTobacco();
        }

        public async void DelTobacco(int TobaccoId)
        {
            var response = await main.client.GetStringAsync("Tobacco");
            var tobacoo = JsonConvert.DeserializeObject<List<Tobaccos>>(response);
            foreach (var c in tobacoo)
            {
                if (c.IdTobacco == TobaccoId)
                {
                    if (c.IsDeleted == false)
                    {
                        var tobacoos = new Tobaccos()
                        {
                            IdTobacco = c.IdTobacco,
                            NameTobacco = c.NameTobacco,
                            CountryTobacco = c.CountryTobacco,
                            StrengthId = c.StrengthId,
                            IsDeleted = true
                        };
                        await main.client.PutAsJsonAsync("Tobacco/" + tobacoos.IdTobacco, tobacoos);
                        break;
                    }
                    else
                    {
                        var tobacoos = new Tobaccos()
                        {
                            IdTobacco = c.IdTobacco,
                            NameTobacco = c.NameTobacco,
                            CountryTobacco = c.CountryTobacco,
                            StrengthId = c.StrengthId,
                            IsDeleted = false
                        };
                        await main.client.PutAsJsonAsync("Tobacco/" + tobacoos.IdTobacco, tobacoos);
                        break;
                    }
                }
            }
            GetTobacco();
        }
        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tobacco = new Tobaccos()
                {
                    NameTobacco = name.Text,
                    CountryTobacco = country.Text,
                    StrengthId = Convert.ToInt32(strengthId.Text),
                    IsDeleted = false
                };
                AddTobacco(tobacco);
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
                var tobacco = new Tobaccos()
                {
                    IdTobacco = dgTobacco.SelectedIndex + 1,
                    NameTobacco = name.Text,
                    CountryTobacco = country.Text,
                    StrengthId = Convert.ToInt32(strengthId.Text),
                    IsDeleted = false
                };
                UpdateTobacco(tobacco);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }
        }

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            DelTobacco(dgTobacco.SelectedIndex + 1);
        }
    }
}

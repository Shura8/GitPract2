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
    /// Логика взаимодействия для Taste.xaml
    /// </summary>
    public partial class Taste : Window
    {
        AuthWin main = new AuthWin();
        public Taste()
        {
            InitializeComponent();
            GetTastes();
        }

        public async void GetTastes()
        {
            var response = await main.client.GetStringAsync("Tastes");
            var tastes = JsonConvert.DeserializeObject<List<Tastes>>(response);
            dgTastes.ItemsSource = tastes;
            dgTastes.Columns[0].Visibility = Visibility.Hidden;
            dgTastes.Columns[1].Header = "Название вкуса";
            dgTastes.Columns[2].Header = "Удалена";
        }

        public async void AddTastes(Tastes tastes)
        {
            await main.client.PostAsJsonAsync("Tastes", tastes);
            GetTastes();
        }
        public async void UpdateTastes(Tastes tastes)
        {
            var response = await main.client.GetStringAsync("Tastes");
            var taste = JsonConvert.DeserializeObject<List<Tastes>>(response);
            foreach (var c in taste)
            {
                if (c.IdTaste == tastes.IdTaste)
                {
                    if (c.IsDeleted == false)
                    {
                        await main.client.PutAsJsonAsync("Tastes/" + tastes.IdTaste, tastes);
                        break;
                    }
                }
            }
            GetTastes();
        }

        public async void DelTastes(int TastesId)
        {
            var response = await main.client.GetStringAsync("Tastes");
            var tastes = JsonConvert.DeserializeObject<List<Tastes>>(response);
            foreach (var c in tastes)
            {
                if (c.IdTaste == TastesId)
                {
                    if (c.IsDeleted == false)
                    {
                        var taste = new Tastes()
                        {
                            IdTaste = c.IdTaste,
                            NameTaste = c.NameTaste,
                            IsDeleted = true
                        };
                        await main.client.PutAsJsonAsync("Tastes/" + taste.IdTaste, taste);
                        break;
                    }
                    else
                    {
                        var taste = new Tastes()
                        {
                            IdTaste = c.IdTaste,
                            NameTaste = c.NameTaste,
                            IsDeleted = false
                        };
                        await main.client.PutAsJsonAsync("Tastes/" + taste.IdTaste, taste);
                        break;
                    }
                }
            }
            GetTastes();
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var taste = new Tastes()
                {
                    NameTaste = name.Text,
                    IsDeleted = false
                };
                AddTastes(taste);
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
                var taste = new Tastes()
                {
                    IdTaste = dgTastes.SelectedIndex + 1,
                    NameTaste = name.Text,
                    IsDeleted = false
                };
                UpdateTastes(taste);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }
        }

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            DelTastes(dgTastes.SelectedIndex + 1);
        }
    }
}

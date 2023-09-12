using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
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
    /// Логика взаимодействия для StrengthWin.xaml
    /// </summary>
    public partial class StrengthWin : Window
    {
        AuthWin main = new AuthWin();
        public StrengthWin()
        {
            InitializeComponent();
            GetStrength();
        }

        public async void GetStrength()
        {
            var response = await main.client.GetStringAsync("Strengths");
            var strength = JsonConvert.DeserializeObject<List<Strength>>(response);
            dgStrength.ItemsSource = strength;
            dgStrength.Columns[0].Visibility = Visibility.Hidden;
            dgStrength.Columns[1].Header = "Название крепкости";
            dgStrength.Columns[2].Header = "Удалена";
        }
        public async void AddStrength(Strength strength)
        {
            await main.client.PostAsJsonAsync("Strengths", strength);
            GetStrength();
        }
        public async void UpdateStrength(Strength strength)
        {
            var response = await main.client.GetStringAsync("Strengths");
            var strengths = JsonConvert.DeserializeObject<List<Strength>>(response);
            foreach (var c in strengths)
            {
                if (c.IdStrength == strength.IdStrength)
                {
                    if (c.IsDeleted == false)
                    {
                        await main.client.PutAsJsonAsync("Strengths/" + strength.IdStrength, strength);
                        break;
                    }
                }
            }
            GetStrength();
        }

        public async void DelStrength(int StrengthId)
        {
            var response = await main.client.GetStringAsync("Strengths");
            var strengths = JsonConvert.DeserializeObject<List<Strength>>(response);
            foreach (var c in strengths)
            {
                if (c.IdStrength == StrengthId)
                {
                    if (c.IsDeleted == false)
                    {
                        var strength = new Strength()
                        {
                            IdStrength = c.IdStrength,
                            NameStrength = c.NameStrength,
                            IsDeleted = true
                        };
                        await main.client.PutAsJsonAsync("Strengths/" + strength.IdStrength, strength);
                        break;
                    }
                    else
                    {
                        var strength = new Strength()
                        {
                            IdStrength = c.IdStrength,
                            NameStrength = c.NameStrength,
                            IsDeleted = false
                        };
                        await main.client.PutAsJsonAsync("Strengths/" + strength.IdStrength, strength);
                        break;
                    }
                }
            }
            GetStrength();
        }
        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var strength = new Strength()
                {
                    NameStrength = name.Text,
                    IsDeleted = false
                };
                AddStrength(strength);
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
                var strength = new Strength()
                {
                    IdStrength = dgStrength.SelectedIndex + 1,
                    NameStrength = name.Text,
                    IsDeleted = false
                };
                UpdateStrength(strength);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }
        }

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            DelStrength(dgStrength.SelectedIndex + 1);
        }
    }
}

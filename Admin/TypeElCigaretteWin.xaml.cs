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
    /// Логика взаимодействия для TypeElCigaretteWin.xaml
    /// </summary>
    public partial class TypeElCigaretteWin : Window
    {
        AuthWin main = new AuthWin();
        public TypeElCigaretteWin()
        {
            InitializeComponent();
            GetTypeElCigarette();
        }

        public async void GetTypeElCigarette()
        {
            var response = await main.client.GetStringAsync("TypeElectronicCigarettes");
            var typeElectronicCigarette = JsonConvert.DeserializeObject<List<TypeElectronicCigarette>>(response);
            dgTypeEl.ItemsSource = typeElectronicCigarette;
            dgTypeEl.Columns[0].Visibility = Visibility.Hidden;
            dgTypeEl.Columns[1].Header = "Название типа эл.сигареты";
            dgTypeEl.Columns[2].Header = "Удалена";
        }
        public async void AddTypeElCigarette(TypeElectronicCigarette typeElectronicCigarette)
        {
            await main.client.PostAsJsonAsync("TypeElectronicCigarettes", typeElectronicCigarette);
            GetTypeElCigarette();
        }
        public async void UpdateTypeElCigarette(TypeElectronicCigarette typeElectronicCigarette)
        {
            var response = await main.client.GetStringAsync("TypeElectronicCigarettes");
            var typeElectronicCigarettes = JsonConvert.DeserializeObject<List<TypeElectronicCigarette>>(response);
            foreach (var c in typeElectronicCigarettes)
            {
                if (c.IdTypeElectronicCigarette == typeElectronicCigarette.IdTypeElectronicCigarette)
                {
                    if (c.IsDeleted == false)
                    {
                        await main.client.PutAsJsonAsync("TypeElectronicCigarettes/" + typeElectronicCigarette.IdTypeElectronicCigarette, typeElectronicCigarette);
                        break;
                    }
                }
            }
            GetTypeElCigarette();
        }

        public async void DelTypeElCigarette(int TypeElCigaretteId)
        {
            var response = await main.client.GetStringAsync("TypeElectronicCigarettes");
            var typeElectronicCigarettes = JsonConvert.DeserializeObject<List<TypeElectronicCigarette>>(response);
            foreach (var c in typeElectronicCigarettes)
            {
                if (c.IdTypeElectronicCigarette == TypeElCigaretteId)
                {
                    if (c.IsDeleted == false)
                    {
                        var typeElectronicCigarette = new TypeElectronicCigarette()
                        {
                            IdTypeElectronicCigarette = c.IdTypeElectronicCigarette,
                            NameTypeElectronicCigarette = c.NameTypeElectronicCigarette,
                            IsDeleted = true
                        };
                        await main.client.PutAsJsonAsync("TypeElectronicCigarettes/" + typeElectronicCigarette.IdTypeElectronicCigarette, typeElectronicCigarette);
                        break;
                    }
                    else
                    {
                        var typeElectronicCigarette = new TypeElectronicCigarette()
                        {
                            IdTypeElectronicCigarette = c.IdTypeElectronicCigarette,
                            NameTypeElectronicCigarette = c.NameTypeElectronicCigarette,
                            IsDeleted = false
                        };
                        await main.client.PutAsJsonAsync("TypeElectronicCigarettes/" + typeElectronicCigarette.IdTypeElectronicCigarette, typeElectronicCigarette);
                        break;
                    }
                }
            }
            GetTypeElCigarette();
        }
        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var typeElectronicCigarette = new TypeElectronicCigarette()
                {
                    NameTypeElectronicCigarette = name.Text,
                    IsDeleted = false
                };
                AddTypeElCigarette(typeElectronicCigarette);
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
                var typeElectronicCigarette = new TypeElectronicCigarette()
                {
                    IdTypeElectronicCigarette = dgTypeEl.SelectedIndex + 1,
                    NameTypeElectronicCigarette = name.Text,
                    IsDeleted = false
                };
                UpdateTypeElCigarette(typeElectronicCigarette);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }
        }

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            DelTypeElCigarette(dgTypeEl.SelectedIndex + 1);
        }
    }
}

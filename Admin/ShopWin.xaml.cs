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
    /// Логика взаимодействия для ShopWin.xaml
    /// </summary>
    public partial class ShopWin : Window
    {
        AuthWin main = new AuthWin();
        public ShopWin()
        {
            InitializeComponent();
            GetShops();
        }

        public async void GetShops()
        {
            var response = await main.client.GetStringAsync("Shops");
            var shops = JsonConvert.DeserializeObject<List<Shop>>(response);
            dgShop.ItemsSource = shops;
            dgShop.Columns[0].Visibility = Visibility.Hidden;
            dgShop.Columns[1].Header = "Адрес магазина";
            dgShop.Columns[2].Header = "Удалена";
        }
        public async void AddShops(Shop shops)
        {
            await main.client.PostAsJsonAsync("Shops", shops);
            GetShops();
        }
        public async void UpdateShops(Shop shops)
        {
            var response = await main.client.GetStringAsync("Shops");
            var shop = JsonConvert.DeserializeObject<List<Shop>>(response);
            foreach (var c in shop)
            {
                if (c.IdShop == shops.IdShop)
                {
                    if (c.IsDeleted == false)
                    {
                        await main.client.PutAsJsonAsync("Shops/" + shops.IdShop, shops);
                        break;
                    }
                }
            }
            GetShops();
        }

        public async void DelShops(int ShopId)
        {
            var response = await main.client.GetStringAsync("Shops");
            var shop = JsonConvert.DeserializeObject<List<Shop>>(response);
            foreach (var c in shop)
            {
                if (c.IdShop == ShopId)
                {
                    if (c.IsDeleted == false)
                    {
                        var shops = new Shop()
                        {
                            IdShop = c.IdShop,
                            AddressShop = c.AddressShop,
                            IsDeleted = true
                        };
                        await main.client.PutAsJsonAsync("Shops/" + shops.IdShop, shops);
                        break;
                    }
                    else
                    {
                        var shops = new Shop()
                        {
                            IdShop = c.IdShop,
                            AddressShop = c.AddressShop,
                            IsDeleted = false
                        };
                        await main.client.PutAsJsonAsync("Shops/" + shops.IdShop, shops);
                        break;
                    }
                }
            }
            GetShops();
        }
        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var shops = new Shop()
                {
                    AddressShop = address.Text,
                    IsDeleted = false
                };
                AddShops(shops);
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
                var shops = new Shop()
                {
                    IdShop = dgShop.SelectedIndex + 1,
                    AddressShop = address.Text,
                    IsDeleted = false
                };
                UpdateShops(shops);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }
        }

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            DelShops(dgShop.SelectedIndex + 1);
        }
    }
}

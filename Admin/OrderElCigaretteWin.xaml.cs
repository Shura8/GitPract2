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
    /// Логика взаимодействия для OrderElCigaretteWin.xaml
    /// </summary>
    public partial class OrderElCigaretteWin : Window
    {
        AuthWin main = new AuthWin();
        public OrderElCigaretteWin()
        {
            InitializeComponent();
            GetOrderElCigarette();
        }

        public async void GetOrderElCigarette()
        {
            var response = await main.client.GetStringAsync("OrderElectronicCigarettes");
            var orderElCigarette = JsonConvert.DeserializeObject<List<OrderElectronicCigarette>>(response);
            dgOrderElCigarette.ItemsSource = orderElCigarette;
            dgOrderElCigarette.Columns[0].Visibility = Visibility.Hidden;
            dgOrderElCigarette.Columns[1].Header = "Id заказа";
            dgOrderElCigarette.Columns[2].Header = "Id эл.сигареты";
            dgOrderElCigarette.Columns[3].Header = "Удалена";
        }

        public async void AddOrderElCigarette(OrderElectronicCigarette orderElCigarette)
        {
            await main.client.PostAsJsonAsync("OrderElectronicCigarettes", orderElCigarette);
            GetOrderElCigarette();
        }
        public async void UpdateOrderElCigarette(OrderElectronicCigarette orderElCigarette)
        {
            var response = await main.client.GetStringAsync("OrderElectronicCigarettes");
            var orderElCigarettes = JsonConvert.DeserializeObject<List<OrderElectronicCigarette>>(response);
            foreach (var c in orderElCigarettes)
            {
                if (c.IdOrderElectronicCigarette == orderElCigarette.IdOrderElectronicCigarette)
                {
                    if (c.IsDeleted == false)
                    {
                        await main.client.PutAsJsonAsync("OrderElectronicCigarettes/" + orderElCigarette.IdOrderElectronicCigarette, orderElCigarette);
                        break;
                    }
                }
            }
            GetOrderElCigarette();
        }

        public async void DelOrderElCigarette(int OrderElCigaretteId)
        {
            var response = await main.client.GetStringAsync("OrderElectronicCigarettes");
            var orderElCigarettes = JsonConvert.DeserializeObject<List<OrderElectronicCigarette>>(response);
            foreach (var c in orderElCigarettes)
            {
                if (c.IdOrderElectronicCigarette == OrderElCigaretteId)
                {
                    if (c.IsDeleted == false)
                    {
                        var orderElCigarette = new OrderElectronicCigarette()
                        {
                            IdOrderElectronicCigarette = c.IdOrderElectronicCigarette,
                            OrderId = c.OrderId,
                            ElectronicCigaretteId = c.ElectronicCigaretteId,
                            IsDeleted = true
                        };
                        await main.client.PutAsJsonAsync("OrderElectronicCigarettes/" + orderElCigarette.IdOrderElectronicCigarette, orderElCigarette);
                        break;
                    }
                    else
                    {
                        var orderElCigarette = new OrderElectronicCigarette()
                        {
                            IdOrderElectronicCigarette = c.IdOrderElectronicCigarette,
                            OrderId = c.OrderId,
                            ElectronicCigaretteId = c.ElectronicCigaretteId,
                            IsDeleted = false
                        };
                        await main.client.PutAsJsonAsync("OrderElectronicCigarettes/" + orderElCigarette.IdOrderElectronicCigarette, orderElCigarette);
                        break;
                    }
                }
            }
            GetOrderElCigarette();
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var orderElCigarette = new OrderElectronicCigarette()
                {
                    OrderId = Convert.ToInt32(orderId.Text),
                    ElectronicCigaretteId = Convert.ToInt32(elCigaretteId.Text),
                    IsDeleted = false
                };
                AddOrderElCigarette(orderElCigarette);
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
                var orderElCigarette = new OrderElectronicCigarette()
                {
                    IdOrderElectronicCigarette = dgOrderElCigarette.SelectedIndex + 1,
                    OrderId = Convert.ToInt32(orderId.Text),
                    ElectronicCigaretteId = Convert.ToInt32(elCigaretteId.Text),
                    IsDeleted = false
                };
                UpdateOrderElCigarette(orderElCigarette);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }
        }

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            DelOrderElCigarette(dgOrderElCigarette.SelectedIndex + 1);
        }
    }
}

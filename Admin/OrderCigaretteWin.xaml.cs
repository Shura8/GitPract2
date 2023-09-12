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
using System.Xml.Linq;
using VapeAPI.Models;

namespace Admin
{
    /// <summary>
    /// Логика взаимодействия для OrderCigaretteWin.xaml
    /// </summary>
    public partial class OrderCigaretteWin : Window
    {
        AuthWin main = new AuthWin();
        public OrderCigaretteWin()
        {
            InitializeComponent(); 
            GetOrderCigarette();
        }

        public async void GetOrderCigarette()
        {
            var response = await main.client.GetStringAsync("OrderCigarettes");
            var orderCigarette = JsonConvert.DeserializeObject<List<OrderCigarette>>(response);
            dgOrderCigarette.ItemsSource = orderCigarette;
            dgOrderCigarette.Columns[0].Visibility = Visibility.Hidden;
            dgOrderCigarette.Columns[1].Header = "Id заказа";
            dgOrderCigarette.Columns[2].Header = "Id сигареты";
            dgOrderCigarette.Columns[3].Header = "Удалена";
        }
        public async void AddOrderCigarette(OrderCigarette orderCigarette)
        {
            await main.client.PostAsJsonAsync("OrderCigarettes", orderCigarette);
            GetOrderCigarette();
        }
        public async void UpdateOrderCigarette(OrderCigarette orderCigarette)
        {
            var response = await main.client.GetStringAsync("OrderCigarettes");
            var orderCigarettes = JsonConvert.DeserializeObject<List<OrderCigarette>>(response);
            foreach (var c in orderCigarettes)
            {
                if (c.IdOrderCigarette == orderCigarette.IdOrderCigarette)
                {
                    if (c.IsDeleted == false)
                    {
                        await main.client.PutAsJsonAsync("OrderCigarettes/" + orderCigarette.IdOrderCigarette, orderCigarette);
                        break;
                    }
                }
            }
            GetOrderCigarette();
        }

        public async void DelOrderCigarette(int OrderCigaretteId)
        {
            var response = await main.client.GetStringAsync("OrderCigarettes");
            var orderCigarettes = JsonConvert.DeserializeObject<List<OrderCigarette>>(response);
            foreach (var c in orderCigarettes)
            {
                if (c.IdOrderCigarette == OrderCigaretteId)
                {
                    if (c.IsDeleted == false)
                    {
                        var orderCigarette = new OrderCigarette()
                        {
                            IdOrderCigarette = c.IdOrderCigarette,
                            OrderId = c.OrderId,
                            CigaretteId = c.CigaretteId,
                            IsDeleted = true
                        };
                        await main.client.PutAsJsonAsync("OrderCigarettes/" + orderCigarette.IdOrderCigarette, orderCigarette);
                        break;
                    }
                    else
                    {
                        var orderCigarette = new OrderCigarette()
                        {
                            IdOrderCigarette = c.IdOrderCigarette,
                            OrderId = c.OrderId,
                            CigaretteId = c.CigaretteId,
                            IsDeleted = false
                        };
                        await main.client.PutAsJsonAsync("OrderCigarettes/" + orderCigarette.IdOrderCigarette, orderCigarette);
                        break;
                    }
                }
            }
            GetOrderCigarette();
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var orderCigarette = new OrderCigarette()
                {
                    OrderId = Convert.ToInt32(orderId.Text),
                    CigaretteId = Convert.ToInt32(cigaretteId.Text),
                    IsDeleted = false
                };
                AddOrderCigarette(orderCigarette);
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
                var orderCigarette = new OrderCigarette()
                {
                    IdOrderCigarette = dgOrderCigarette.SelectedIndex + 1,
                    OrderId = Convert.ToInt32(orderId.Text),
                    CigaretteId = Convert.ToInt32(cigaretteId.Text),
                    IsDeleted = false
                };
                UpdateOrderCigarette(orderCigarette);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }
        }

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            DelOrderCigarette(dgOrderCigarette.SelectedIndex + 1);
        }
    }
}

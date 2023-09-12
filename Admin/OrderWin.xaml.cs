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
    /// Логика взаимодействия для OrderWin.xaml
    /// </summary>
    public partial class OrderWin : Window
    {
        AuthWin main = new AuthWin();
        public OrderWin()
        {
            InitializeComponent();
            GetOrder();
        }

        public async void GetOrder()
        {
            var response = await main.client.GetStringAsync("Orders");
            var order = JsonConvert.DeserializeObject<List<Orders>>(response);
            dgOrder.ItemsSource = order;
            dgOrder.Columns[0].Visibility = Visibility.Hidden;
            dgOrder.Columns[1].Header = "Номер заказа";
            dgOrder.Columns[2].Header = "Сумма заказа";
            dgOrder.Columns[3].Header = "Время заказа";
            dgOrder.Columns[4].Header = "Id магазина";
            dgOrder.Columns[5].Header = "Id статуса заказа";
            dgOrder.Columns[6].Header = "Id пользователя";
            dgOrder.Columns[7].Header = "Удалена";
        }
        public async void AddOrder(Orders order)
        {
            await main.client.PostAsJsonAsync("Orders", order);
            GetOrder();
        }
        public async void UpdateOrder(Orders order)
        {
            var response = await main.client.GetStringAsync("Orders");
            var orders = JsonConvert.DeserializeObject<List<Orders>>(response);
            foreach (var c in orders)
            {
                if (c.IdOrder == order.IdOrder)
                {
                    if (c.IsDeleted == false)
                    {
                        await main.client.PutAsJsonAsync("Orders/" + order.IdOrder, order);
                        break;
                    }
                }
            }
            GetOrder();
        }

        public async void DelOrder(int OrderId)
        {
            var response = await main.client.GetStringAsync("Orders");
            var orders = JsonConvert.DeserializeObject<List<Orders>>(response);
            foreach (var c in orders)
            {
                if (c.IdOrder == OrderId)
                {
                    if (c.IsDeleted == false)
                    {
                        var order = new Orders()
                        {
                            IdOrder = c.IdOrder,
                            NumberOrder = c.NumberOrder,
                            TimeOrder = c.TimeOrder,
                            OrderSum = c.OrderSum,
                            ShopId = c.ShopId,
                            StatusOrderId = c.StatusOrderId,
                            UsersId = c.UsersId,
                            IsDeleted = true
                        };
                        await main.client.PutAsJsonAsync("Orders/" + order.IdOrder, order);
                        break;
                    }
                    else
                    {
                        var order = new Orders()
                        {
                            IdOrder = c.IdOrder,
                            NumberOrder = c.NumberOrder,
                            TimeOrder = c.TimeOrder,
                            OrderSum = c.OrderSum,
                            ShopId = c.ShopId,
                            StatusOrderId = c.StatusOrderId,
                            UsersId = c.UsersId,
                            IsDeleted = false
                        };
                        await main.client.PutAsJsonAsync("Orders/" + order.IdOrder, order);
                        break;
                    }
                }
            }
            GetOrder();
        }
        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var order = new Orders()
                {
                    NumberOrder = number.Text,
                    OrderSum = Convert.ToInt32(sum.Text),
                    TimeOrder = DateTime.Now.ToString(),
                    ShopId = Convert.ToInt32(shopId.Text),
                    StatusOrderId = Convert.ToInt32(statusId.Text),
                    UsersId = Convert.ToInt32(userId.Text),
                    IsDeleted = false
                };
                AddOrder(order);
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
                var order = new Orders()
                {
                    IdOrder = dgOrder.SelectedIndex + 1,
                    NumberOrder = number.Text,
                    TimeOrder = DateTime.Now.ToString(),
                    OrderSum = Convert.ToInt32(sum.Text),
                    ShopId = Convert.ToInt32(shopId.Text),
                    StatusOrderId = Convert.ToInt32(statusId.Text),
                    UsersId = Convert.ToInt32(userId.Text),
                    IsDeleted = false
                };
                UpdateOrder(order);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }
        }

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            DelOrder(dgOrder.SelectedIndex + 1);
        }
    }
}

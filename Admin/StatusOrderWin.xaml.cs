using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
    /// Логика взаимодействия для StatusOrderWin.xaml
    /// </summary>
    public partial class StatusOrderWin : Window
    {
        AuthWin main = new AuthWin();
        public StatusOrderWin()
        {
            InitializeComponent();
            GetStatusOrder();
        }

        public async void GetStatusOrder()
        {
            var response = await main.client.GetStringAsync("StatusOrders");
            var statusOrder = JsonConvert.DeserializeObject<List<StatusOrder>>(response);
            dgStatus.ItemsSource = statusOrder;
            dgStatus.Columns[0].Visibility = Visibility.Hidden;
            dgStatus.Columns[1].Header = "Название статуса заказа";
            dgStatus.Columns[2].Header = "Удалена";
        }
        public async void AddStatusOrder(StatusOrder statusOrder)
        {
            await main.client.PostAsJsonAsync("StatusOrders", statusOrder);
            GetStatusOrder();
        }
        public async void UpdateStatusOrder(StatusOrder statusOrder)
        {
            var response = await main.client.GetStringAsync("StatusOrders");
            var statusOrders = JsonConvert.DeserializeObject<List<StatusOrder>>(response);
            foreach (var c in statusOrders)
            {
                if (c.IdStatusOrder == statusOrder.IdStatusOrder)
                {
                    if (c.IsDeleted == false)
                    {
                        await main.client.PutAsJsonAsync("StatusOrders/" + statusOrder.IdStatusOrder, statusOrder);
                        break;
                    }
                }
            }
            GetStatusOrder();
        }

        public async void DelStatusOrder(int StatusOrderId)
        {
            var response = await main.client.GetStringAsync("StatusOrders");
            var statusOrders = JsonConvert.DeserializeObject<List<StatusOrder>>(response);
            foreach (var c in statusOrders)
            {
                if (c.IdStatusOrder == StatusOrderId)
                {
                    if (c.IsDeleted == false)
                    {
                        var statusOrder = new StatusOrder()
                        {
                            IdStatusOrder = c.IdStatusOrder,
                            NameStatusOrder = c.NameStatusOrder,
                            IsDeleted = true
                        };
                        await main.client.PutAsJsonAsync("StatusOrders/" + statusOrder.IdStatusOrder, statusOrder);
                        break;
                    }
                    else
                    {
                        var statusOrder = new StatusOrder()
                        {
                            IdStatusOrder = c.IdStatusOrder,
                            NameStatusOrder = c.NameStatusOrder,
                            IsDeleted = false
                        };
                        await main.client.PutAsJsonAsync("StatusOrders/" + statusOrder.IdStatusOrder, statusOrder);
                        break;
                    }
                }
            }
            GetStatusOrder();
        }
        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var statusOrder = new StatusOrder()
                {
                    NameStatusOrder = name.Text,
                    IsDeleted = false
                };
                AddStatusOrder(statusOrder);
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
                var statusOrder = new StatusOrder() 
                { 
                    IdStatusOrder = dgStatus.SelectedIndex + 1,
                    NameStatusOrder = name.Text,
                    IsDeleted = false
                };
                UpdateStatusOrder(statusOrder);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }
        }

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            DelStatusOrder(dgStatus.SelectedIndex + 1);
        }
    }
}

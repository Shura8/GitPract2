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
    /// Логика взаимодействия для OrderFluidWin.xaml
    /// </summary>
    public partial class OrderFluidWin : Window
    {
        AuthWin main = new AuthWin();
        public OrderFluidWin()
        {
            InitializeComponent();
            GetOrderFluid();
        }

        public async void GetOrderFluid()
        {
            var response = await main.client.GetStringAsync("OrderFluids");
            var orderFluid = JsonConvert.DeserializeObject<List<OrderFluid>>(response);
            dgOrderFluid.ItemsSource = orderFluid;
            dgOrderFluid.Columns[0].Visibility = Visibility.Hidden;
            dgOrderFluid.Columns[1].Header = "Id заказа";
            dgOrderFluid.Columns[2].Header = "Id жидкости";
            dgOrderFluid.Columns[3].Header = "Удалена";
        }
        public async void AddOrderFluid(OrderFluid orderFluid)
        {
            await main.client.PostAsJsonAsync("OrderFluids", orderFluid);
            GetOrderFluid();
        }
        public async void UpdateOrderFluid(OrderFluid orderFluid)
        {
            var response = await main.client.GetStringAsync("OrderFluids");
            var orderFluids = JsonConvert.DeserializeObject<List<OrderFluid>>(response);
            foreach (var c in orderFluids)
            {
                if (c.IdOrderFluid == orderFluid.IdOrderFluid)
                {
                    if (c.IsDeleted == false)
                    {
                        await main.client.PutAsJsonAsync("OrderFluids/" + orderFluid.IdOrderFluid, orderFluid);
                        break;
                    }
                }
            }
            GetOrderFluid();
        }

        public async void DelOrderFluid(int OrderFluidId)
        {
            var response = await main.client.GetStringAsync("OrderFluids");
            var orderFluids = JsonConvert.DeserializeObject<List<OrderFluid>>(response);
            foreach (var c in orderFluids)
            {
                if (c.IdOrderFluid == OrderFluidId)
                {
                    if (c.IsDeleted == false)
                    {
                        var orderFluid = new OrderFluid()
                        {
                            IdOrderFluid = c.IdOrderFluid,
                            OrderId = c.OrderId,
                            FluidId = c.FluidId,
                            IsDeleted = true
                        };
                        await main.client.PutAsJsonAsync("OrderFluids/" + orderFluid.IdOrderFluid, orderFluid);
                        break;
                    }
                    else
                    {
                        var orderFluid = new OrderFluid()
                        {
                            IdOrderFluid = c.IdOrderFluid,
                            OrderId = c.OrderId,
                            FluidId = c.FluidId,
                            IsDeleted = false
                        };
                        await main.client.PutAsJsonAsync("OrderFluids/" + orderFluid.IdOrderFluid, orderFluid);
                        break;
                    }
                }
            }
            GetOrderFluid();
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var orderFluid = new OrderFluid()
                {
                    OrderId = Convert.ToInt32(orderId.Text),
                    FluidId = Convert.ToInt32(fluidId.Text),
                    IsDeleted = false
                };
                AddOrderFluid(orderFluid);
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
                var orderFluid = new OrderFluid()
                {
                    IdOrderFluid = dgOrderFluid.SelectedIndex + 1,
                    OrderId = Convert.ToInt32(orderId.Text),
                    FluidId = Convert.ToInt32(fluidId.Text),
                    IsDeleted = false
                };
                UpdateOrderFluid(orderFluid);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }
        }

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            DelOrderFluid(dgOrderFluid.SelectedIndex + 1);
        }
    }
}

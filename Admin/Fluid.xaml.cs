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
    /// Логика взаимодействия для Fluid.xaml
    /// </summary>
    public partial class Fluid : Window
    {
        AuthWin main = new AuthWin();
        public Fluid()
        {
            InitializeComponent();
            GetFluid();
        }

        public async void GetFluid()
        {
            var response = await main.client.GetStringAsync("Fluids");
            var fluids = JsonConvert.DeserializeObject<List<Fluids>>(response);
            dgFluid.ItemsSource = fluids;
            dgFluid.Columns[0].Visibility = Visibility.Hidden;
            dgFluid.Columns[1].Header = "Название жидкости";
            dgFluid.Columns[2].Header = "Id вкуса";
            dgFluid.Columns[3].Header = "Никотин(г.)";
            dgFluid.Columns[4].Header = "Объем(г.)";
            dgFluid.Columns[5].Header = "Стоимость жидкости";
            dgFluid.Columns[6].Header = "Кол-во жидкостей";
            dgFluid.Columns[7].Header = "Удалена";
        }

        public async void AddFluid(Fluids fluid)
        {
            await main.client.PostAsJsonAsync("Fluids", fluid);
            GetFluid();
        }
        public async void UpdateFluid(Fluids fluid)
        {
            var response = await main.client.GetStringAsync("Fluids");
            var fluids = JsonConvert.DeserializeObject<List<Fluids>>(response);
            foreach (var c in fluids)
            {
                if (c.IdFluid == fluid.IdFluid)
                {
                    if (c.IsDeleted == false)
                    {
                        await main.client.PutAsJsonAsync("Fluids/" + fluid.IdFluid, fluid);
                        break;
                    }
                }
            }
            GetFluid();
        }
        public async void DelFluid(int FluidId)
        {
            var response = await main.client.GetStringAsync("Fluids");
            var fluids = JsonConvert.DeserializeObject<List<Fluids>>(response);
            foreach (var c in fluids)
            {
                if (c.IdFluid == FluidId)
                {
                    if (c.IsDeleted == false)
                    {
                        var fluid = new Fluids()
                        {
                            IdFluid = c.IdFluid,
                            NameFluid = c.NameFluid,
                            Nicotine = c.Nicotine,
                            TasteId = c.TasteId,
                            VolumeFluid = c.VolumeFluid,
                            CostFluid = c.CostFluid,
                            CountFluid = c.CountFluid,
                            IsDeleted = true
                        };
                        await main.client.PutAsJsonAsync("Fluids/" + fluid.IdFluid, fluid);
                        break;
                    }
                    else
                    {
                        var fluid = new Fluids()
                        {
                            IdFluid = c.IdFluid,
                            NameFluid = c.NameFluid,
                            Nicotine = c.Nicotine,
                            TasteId = c.TasteId,
                            VolumeFluid = c.VolumeFluid,
                            CostFluid = c.CostFluid,
                            CountFluid = c.CountFluid,
                            IsDeleted = false
                        };
                        await main.client.PutAsJsonAsync("Fluids/" + fluid.IdFluid, fluid);
                        break;
                    }
                }
            }
            GetFluid();
        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fluid = new Fluids()
                {
                    IdFluid = dgFluid.SelectedIndex + 1,
                    NameFluid = name.Text,
                    Nicotine = Convert.ToInt32(nicotine.Text),
                    TasteId = Convert.ToInt32(tasteId.Text),
                    VolumeFluid = Convert.ToInt32(volume.Text),
                    CostFluid = Convert.ToInt32(cost.Text),
                    CountFluid = Convert.ToInt32(count.Text),
                    IsDeleted = false
                };
                UpdateFluid(fluid);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }
        }

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            DelFluid(dgFluid.SelectedIndex + 1);
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fluid = new Fluids()
                {
                    NameFluid = name.Text,
                    Nicotine = Convert.ToInt32(nicotine.Text),
                    TasteId = Convert.ToInt32(tasteId.Text),
                    VolumeFluid = Convert.ToInt32(volume.Text),
                    CostFluid = Convert.ToInt32(cost.Text),
                    CountFluid = Convert.ToInt32(count.Text),
                    IsDeleted = false
                };
                AddFluid(fluid);
            }
            catch
            {
                MessageBox.Show("Неправильные данные!");
            }
        }
    }
}

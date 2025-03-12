using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;

namespace RealTimeMonitoring.WPF
{
    public partial class MainWindow : Window
    {
        private static readonly string ApiBaseUrl = "https://localhost:5001/"; //  Update to match your API
        private readonly HttpClient _httpClient;
        private HubConnection _hubConnection;
        public ObservableCollection<ProductionData> ProductionRecords { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            _httpClient = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };

            ProductionRecords = new ObservableCollection<ProductionData>();
            ProductionDataGrid.ItemsSource = ProductionRecords;

            InitializeSignalR();
        }

        private async void InitializeSignalR()
        {
            try
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl($"{ApiBaseUrl}productionHub")
                    .WithAutomaticReconnect()
                    .Build();

                _hubConnection.On<ProductionData>("ReceiveProductionData", data =>
                {
                    Application.Current.Dispatcher.Invoke(() => ProductionRecords.Add(data));
                });

                await _hubConnection.StartAsync();
                MessageBox.Show("Connected to SignalR!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SignalR Connection Failed: {ex.Message}");
            }
        }

        private async void FetchData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = await _httpClient.GetFromJsonAsync<ProductionData[]>("api/ProductionData");
                if (data == null) return;

                ProductionRecords.Clear();
                foreach (var record in data)
                {
                    ProductionRecords.Add(record);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching data: {ex.Message}");
            }
        }

        private async void AddRecord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newRecord = new ProductionData
                {
                    MachineName = "Machine A",
                    Efficiency = 95.5,
                    Status = "Running",
                    ProductionCount = 100,
                    Temperature = 25.3,
                    Humidity = 60.5,
                    ErrorLog = "No errors",
                    Timestamp = DateTime.Now
                };

                var response = await _httpClient.PostAsJsonAsync("api/ProductionData", newRecord);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Record added successfully!");
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Failed to add record: {response.StatusCode}\n{errorMessage}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding record: {ex.Message}");
            }
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
            }

            _httpClient.Dispose();
        }
    }

    public class ProductionData
    {
        public int Id { get; set; }
        public string MachineName { get; set; }
        public double Efficiency { get; set; }
        public string Status { get; set; }
        public int ProductionCount { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public string ErrorLog { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

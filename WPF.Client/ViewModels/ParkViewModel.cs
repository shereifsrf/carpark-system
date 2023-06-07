using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CPM.Shared.ClientServices;
using CPM.Shared.DTO;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WPF.Client.ViewModels
{
    public partial class ParkViewModel : ObservableObject
    {


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ParkCommand))]
        [NotifyCanExecuteChangedFor(nameof(UnParkCommand))]
        private string numberPlate;
        [ObservableProperty]
        private string status;
        [ObservableProperty]
        private string? entry;
        private readonly IClientParkService _clientParkService;

        public ParkViewModel()
        {
            _clientParkService = App.Services.GetService<IClientParkService>();
        }

        partial void OnNumberPlateChanged(string _)
        {
            Status = "";
            Entry = null;
        }

        [RelayCommand(CanExecute = nameof(CanExecuteCommand))]
        public async void Park()
        {
            var response = await _clientParkService.Park(NumberPlate);
            if (string.IsNullOrEmpty(response))
            {
                Status = "Car parking entry is success";
            }
            else
            {
                Status = response;
            }
        }

        [RelayCommand(CanExecute = nameof(CanExecuteCommand))]
        public async void UnPark()
        {
            var response = await _clientParkService.UnPark(NumberPlate);
            if (response.ErrorStatus != null)
            {
                NumberPlate = "";
                Status = response.ErrorStatus.Message;
                Entry = null;
            }
            else
            {
                Status = "Success";
                Entry = getEntry(response.Data);
            }
        }

        private bool CanExecuteCommand()
        {
            if (string.IsNullOrEmpty(NumberPlate))
                return false;
            else
                return true;
        }

        private string getEntry(EntryUnParkDTO? entry)
        {
            if (entry == null)
                return "";

            // build each attribute in new line
            var sb = new StringBuilder();
            sb.AppendLine(entry.NumberPlate + "\n");
            sb.AppendLine(entry.From + "\n");
            sb.AppendLine(entry.To + "\n");
            sb.AppendLine(entry.ParkingTime + "\n");
            sb.AppendLine(entry.Amount + "\n");

            return sb.ToString();

        }
    }

    //NullToVisibilityConverter
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

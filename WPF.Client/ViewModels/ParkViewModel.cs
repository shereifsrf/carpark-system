using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CPM.Shared.ClientServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Client.ViewModels
{
    public partial class ParkViewModel : ObservableObject
    {

        private readonly IClientParkService _clientParkService;

        [ObservableProperty]
        private string numberPlate;
        [ObservableProperty]
        private string status;

        public ParkViewModel()
        {
            _clientParkService = App.Services.GetService<IClientParkService>();
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
            var (response, error) = await _clientParkService.UnPark(NumberPlate);
            if (error != null)
            {
                NumberPlate = "";
                Status = error.Message;
            }
            else
            {
                Status = "Success";
            }
        }

        private bool CanExecuteCommand()
        {
            if (string.IsNullOrEmpty(NumberPlate))
                return false;
            else
                return true;
        }
    }
}

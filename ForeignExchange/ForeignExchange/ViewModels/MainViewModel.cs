namespace ForeignExchange.ViewModels
{
    using ForeignExchange.Clases;
    using GalaSoft.MvvmLight.Command;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Net.Http;
    using System.Windows.Input;
    using System.Reflection;



    public class MainViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Atributos
        private ExchangeRates exchangeRates;
        private bool isRunning;
        private decimal amount;
        private double sourceRate;
        private double targetRate;
        private bool isEnabled;

       
        
        #endregion


        #region Properties
        public ObservableCollection<Rate> Rates { get; set; }
        public decimal Amount
        {
            set
            {
                if (amount != value)
                {
                    amount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Amount"));
                }
            }
            get
            {
                return amount;
            }

        }

        

        public double SourceRate
        {
            set
            {
                if (sourceRate != value)
                {
                    sourceRate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SourceRate"));
                }
            }
            get => sourceRate;


        }

        public double TargetRate
        {
            set
            {
                if (targetRate != value)
                {
                    targetRate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TargetRate"));
                }
            }
            get
            {
                return targetRate;
            }

        }
        public bool IsRunning
        {

            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(this,
                        new PropertyChangedEventArgs("IsRunning"));
                }
            }

            get
            {
                return isRunning;

            }
        }
        
        public bool IsEnabled
        {
            
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    PropertyChanged?.Invoke(
                        this,new PropertyChangedEventArgs("IsEnabled"));
                }
            }
            get
            {
                return isEnabled;
            }

        }

        public string Message
        {

            set
            {
                if (message != value)
                {
                    message= value;
                    PropertyChanged?.Invoke(
                        this, new PropertyChangedEventArgs("Message"));
                }
            }
            get
            {
                return message;
            }

        }






        #endregion
        public MainViewModel()
        {
            Rates = new ObservableCollection<Rate>();

            Message = "Ingrese una cantidad, seleccione una moneda fuente, seleccione una moneda de conversion";
            IsEnabled = false;
            LoadRates();

        }



        #region Metodos

        private async void LoadRates()
        {
            isRunning = true;
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new
                    Uri("https://openexchangerates.org");
                var url = "/api/latest.json?app_id=57658348f64f4d0a8ba9b21ef5912545";
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert("Error", response.StatusCode.ToString(), "Aceptar");
                    isRunning = false;
                    return;
                }

                var result = await response.Content.ReadAsStringAsync();
                exchangeRates = JsonConvert.DeserializeObject<ExchangeRates>(result);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
                IsRunning = false;
                IsEnabled = false;
                return;

            }
            LoadRates();
            IsRunning = false;
            IsEnabled = true;

        }
        private void ConvertRates()
        {
            Rates.Clear();
            var type = typeof(Rates);
            var properties = type.GetRuntimeFields();

            foreach (var property in properties)
            {
                var code = property.Name.Substring(1, 3);
                Rates.Add(new Rate
                {
                    Code = code,
                    TaxRate = (double)property.GetValue(exchangeRates.Rates),
                });
            }


        }

        #endregion



    }


    /*






     #region Commands
     public ICommand ConvertCommand
         {
             get
             {
                 return new RelayCommand(ConvertMoney);
             }
         }

          void ConvertMoney()
         {
             throw new NotImplementedException();
         }
         */



}



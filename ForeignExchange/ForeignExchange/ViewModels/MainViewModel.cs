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

       
        string _result;
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

        

        public Rate SourceRate
        {
            get;
            set;

        }

        public Rate TargetRate
        {
            get;
            set;
        }
        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
            set
            {
                if(isRunning !=value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof (IsRunning)));
                }
            }
        }
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }

        public string Result
        {
            get
            {
                return _result;
            }
            set
            {
                if (_result != value)
                {
                    _result = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Result)));
                }
            }
        }


        #endregion
        public MainViewModel()
        {
            LoadRates();
        }

        

        #region Metodos
        async void LoadRates()
        {
            IsRunning = true;
            Result = "Loading Rates....";

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new 
                    Uri("https://openexchangerates.org");
                var url = "/api/latest.json?app_id=f490efbcd52d48ee98fd62cf33c47b9e";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    IsRunning = false;
                    Result = result;
                }

                var rates = JsonConvert.DeserializeObject<List<Rate>>(result);
                Rates = new ObservableCollection<Rate>(rates);

                IsRunning = false;
                IsEnabled = true;
                Result = "Ready to convert";
            }
            catch (Exception ex)
            {
                IsRunning = false;
                Result = ex.Message;
            }



        }

        #endregion




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

        #endregion


    }
}


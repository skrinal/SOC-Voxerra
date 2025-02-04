using System.Text.RegularExpressions;

namespace Voxerra.ViewModels.Settings.Account
{
    public class NameViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query == null || query.Count == 0) return;

            UserName = HttpUtility.UrlDecode(query["UserName"].ToString());
            
            Initialize();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DataCenterService _dataCenterService;
        private ServiceProvider _serviceProvider;

        private string CurrentUserName;

        public NameViewModel(DataCenterService dataCenterService, ServiceProvider serviceProvider)
        {
            _dataCenterService = dataCenterService;
            _serviceProvider = serviceProvider;

            UpdateNameCommand = new Command(() =>
            {
                if (IsProcessing) return;

                if (!isUserNameUnique) return;

                IsProcessing = true;

                UpdateUserName().GetAwaiter().OnCompleted(() =>
                {
                    IsProcessing = false;
                });
            });

            ButtonStatus = false;
            LabelIcon = "";
            LabelColor= "Transparent";


            ButtonColor = "Gray";
            ButtonColorV2 = "Gray";

            GoBackCommand = new Command(OnGoBack);
        }

        public async Task UpdateUserName()
        {

        }

        public void GetUserName()
        {
            userName = _dataCenterService.UserInfo.UserName;
            CurrentUserName = userName;
        }

        public void Initialize()
        {
            Task.Run(async () =>
            {
                IsProcessing = true;
                GetUserName();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsProcessing = false;
                OnPropertyChanged(nameof(UserName));

            });
        }

        private async Task IsUserNameUniqueCall(string userName)
        {
            try
            {
                if (userName != CurrentUserName)
                {
                    var request = new IsUserNameUniqueRequest
                    {
                        UserName = userName
                    };

                    var response = await _serviceProvider.CallWebApi<IsUserNameUniqueRequest, BaseResponse>(
                        "/Registration/IsUserNameUnique", HttpMethod.Post, request);


                    if (response.StatusCode == 200)
                    {
                        LabelIcon = "check";
                        LabelColor = "Green";
                        isUserNameUnique = true;
                        ButtonStatus = true;
                    }
                    else
                    {
                        LabelIcon = "close";
                        LabelColor = "Red";
                        isUserNameUnique = false;
                        ButtonStatus = false;
                    }
                }
            }
            catch (Exception ex)
            {
                isUserNameUnique = false;
                await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
            }
        }


        private CancellationTokenSource _cts;
        public async void DebounceUserName(string query)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            try
            {
                await Task.Delay(800, _cts.Token);
                

                if (string.IsNullOrWhiteSpace(query)) return; //set mesage


                IsProcessing = true;
                await IsUserNameUniqueCall(query);
                IsProcessing = false;
            }
            catch (TaskCanceledException) { }
        }

        private async void OnGoBack()
        {
            await Shell.Current.GoToAsync("AccountDetailsPage");
        }
        private bool IsValidUsername(string username)
        {
            // Reset colors to white initially
            RuleColor1 = "white";
            RuleColor2 = "white";
            RuleColor3 = "white";

            if (string.Equals(username, CurrentUserName, StringComparison.OrdinalIgnoreCase))
            {
                isUserNameUnique = false;
                ButtonStatus = false;
                LabelIcon = "";
                LabelColor = "Transparent";
                return false;
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                RuleColor1 = "Red";
                RuleColor2 = "Red";
                RuleColor3 = "Red";
                LabelIcon = "close";
                LabelColor = "Red";
                isUserNameUnique = false;
                ButtonStatus = false;
                return false;
            }

            if (username.Length < 3 || username.Length > 15)
            {
                RuleColor1 = "Red";
                RuleColor2 = "Red";
                RuleColor3 = "Red";
                LabelIcon = "close";
                LabelColor = "Red";
                isUserNameUnique = false;
                ButtonStatus = false;
                return false;
            }

            string pattern = @"^[A-Za-z][A-Za-z0-9]{2,}$"; 
            if (!Regex.IsMatch(username, pattern))
            {
                RuleColor1 = "Red";
                RuleColor2 = "Red";
                RuleColor3 = "Red";
                LabelIcon = "close";
                LabelColor = "Red";
                isUserNameUnique = false;
                ButtonStatus = false;
                return false;
            }

            RuleColor1 = "white";
            RuleColor2 = "white";
            RuleColor3 = "white";
            LabelIcon = "check"; 
            LabelColor = "Green";
            isUserNameUnique = true;
            ButtonStatus = true;
            return true;
        }

        private bool isProcessing;
        private bool buttonStatus;
        private string userName;
        private bool isUserNameUnique;

        private string buttonColor;
        private string buttonColorV2;

        private string labelIcon;
        private string labelColor;

        private string ruleColor1;
        private string ruleColor2;
        private string ruleColor3;
        private string ruleColor4;

        /*public string EntryQueary
        {
            get => entryQueary;
            set
            {
                entryQueary = value;
                OnPropertyChanged();
                
                DebounceUserName(value);
            }
        }*/

        public bool IsProcessing
        {
            get { return isProcessing; }
            set { isProcessing = value; OnPropertyChanged(); }
        }
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged();
                if (IsValidUsername(value))
                {
                    DebounceUserName(value);
                }
            }
        }
        public bool ButtonStatus
        {
            get { return buttonStatus; }
            set { buttonStatus = value; OnPropertyChanged(); }
        }
        public string ButtonColor
        {
            get { return buttonColor; }
            set { buttonColor = value; OnPropertyChanged(); }
        }
        public string ButtonColorV2
        {
            get { return buttonColorV2; }
            set { buttonColorV2 = value; OnPropertyChanged(); }
        }

        public string LabelIcon
        {
            get { return labelIcon; }
            set { labelIcon = value; OnPropertyChanged(); }
        }
        public string LabelColor
        {
            get { return labelColor; }
            set { labelColor = value; OnPropertyChanged(); }
        }


        public string RuleColor1
        {
            get { return ruleColor1; }
            set { ruleColor1 = value; OnPropertyChanged(); }
        }
        public string RuleColor2
        {
            get { return ruleColor2; }
            set { ruleColor2 = value; OnPropertyChanged(); }
        }
        public string RuleColor3
        {
            get { return ruleColor3; }
            set { ruleColor3 = value; OnPropertyChanged(); }
        }
        public string RuleColor4
        {
            get { return ruleColor4; }
            set { ruleColor4 = value; OnPropertyChanged(); }
        }

        public ICommand UpdateNameCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
    }   
}
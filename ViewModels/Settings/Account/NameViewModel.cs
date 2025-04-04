using System.Text.RegularExpressions;

namespace Voxerra.ViewModels.Settings.Account
{
    public class NameViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query == null || query.Count == 0) return;

            UserName = HttpUtility.UrlDecode(query["UserName"].ToString());
            CurrentUserName = UserName;
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

                UpdateUserName().GetAwaiter().OnCompleted(() => { IsProcessing = false; });
            });

            ButtonStatus = false;
            LabelIcon = "";
            LabelColor = "Transparent";

            GoBackCommand = new Command(OnGoBack);
        }

        public async Task UpdateUserName()
        {
            try
            {
                var response = await _serviceProvider.CallWebApi<string, BaseResponse>
                    ("/UserSettings/ChangeUserName", HttpMethod.Post, UserName);

                if (response.StatusCode == 200)
                {
                    _dataCenterService.UserInfo.UserName = userName;
                    ResultColor = "green";
                    ResultText = "Successfully updated the name.";
                    ButtonStatus = false;
                    CurrentUserName = UserName;
                    OnPropertyChanged(nameof(UserName));
                }
                else
                {
                    ResultColor = "red";
                    ResultText = "Failed to update the name.";
                    ButtonStatus = false;
                }
            }
            catch (Exception ex)
            {
                isUserNameUnique = false;
                await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
            }
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

                    var response = await _serviceProvider.CallWebApi<IsUserNameUniqueRequest, BaseResponse>
                        ("/Registration/IsUserNameUnique", HttpMethod.Post, request);


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
                await Task.Delay(1000, _cts.Token);


                if (string.IsNullOrWhiteSpace(query)) return; //set mesage


                IsProcessing = true;
                await IsUserNameUniqueCall(query);
                IsProcessing = false;
            }
            catch (TaskCanceledException)
            {
            }
        }

        private async void OnGoBack()
        {
            await Shell.Current.GoToAsync("MainSettingPage");
        }

        int firstStart = 0;

        private bool IsValidUsername(string username)
        {
            if (firstStart < 2)
            {
                firstStart++;
                return false; // First 2 attempts, return false immediately
            }

            // Reset all rule colors to white initially
            RuleColor1 = "white";
            RuleColor2 = "white";
            RuleColor3 = "white";

            ResultColor = "transparent";
            ResultText = "";

            // Check if the username is empty
            if (string.IsNullOrWhiteSpace(username)) // Checks for empty, null, or only whitespace
            {
                ButtonStatus = false; // Disable the button when the input is empty
                LabelIcon = ""; // No icon
                LabelColor = "Transparent"; // No label color
                return false; // Return false when input is empty
            }

            // Check if the username matches the current username
            if (string.Equals(username, CurrentUserName, StringComparison.OrdinalIgnoreCase))
            {
                isUserNameUnique = false;
                ButtonStatus = false;
                LabelIcon = "";
                LabelColor = "Transparent";
                return false;
            }

            // Rule 3: Check for spaces specifically
            if (username.Contains(" "))
            {
                RuleColor3 = "Red"; // "No spaces" rule failed
                LabelIcon = "close";
                LabelColor = "Red";
                isUserNameUnique = false;
                ButtonStatus = false;
                return false;
            }

            // Rule 2: Check for special characters (excluding spaces)
            string
                pattern =
                    @"^[A-Za-z][A-Za-z0-9]*$"; // updated regex to disallow special characters but allow alphanumeric
            if (!Regex.IsMatch(username, pattern))
            {
                RuleColor2 = "Red"; // "No special characters" rule failed
                LabelIcon = "close";
                LabelColor = "Red";
                isUserNameUnique = false;
                ButtonStatus = false;
                return false;
            }

            // Rule 1: Check for username length
            if (username.Length < 3 || username.Length > 15)
            {
                RuleColor1 = "Red"; // "Minimum 3, maximum 15 characters" rule failed
                LabelIcon = "close";
                LabelColor = "Red";
                isUserNameUnique = false;
                ButtonStatus = false;
                return false;
            }

            // If all rules pass, show success status
            RuleColor1 = "white";
            RuleColor2 = "white";
            RuleColor3 = "white";
            LabelIcon = "check";
            LabelColor = "Green";
            isUserNameUnique = true;
            ButtonStatus = true;

            return true; // Validation passed
        }


        private bool isProcessing;
        private bool buttonStatus;
        private string userName;
        private bool isUserNameUnique;

        private string labelIcon;
        private string labelColor;

        private string ruleColor1;
        private string ruleColor2;
        private string ruleColor3;

        private string resultColor;
        private string resultText;

        public bool IsProcessing
        {
            get { return isProcessing; }
            set
            {
                isProcessing = value;
                OnPropertyChanged();
            }
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
            set
            {
                buttonStatus = value;
                OnPropertyChanged();
            }
        }

        public string LabelIcon
        {
            get { return labelIcon; }
            set
            {
                labelIcon = value;
                OnPropertyChanged();
            }
        }

        public string LabelColor
        {
            get { return labelColor; }
            set
            {
                labelColor = value;
                OnPropertyChanged();
            }
        }

        public string RuleColor1
        {
            get { return ruleColor1; }
            set
            {
                ruleColor1 = value;
                OnPropertyChanged();
            }
        }

        public string RuleColor2
        {
            get { return ruleColor2; }
            set
            {
                ruleColor2 = value;
                OnPropertyChanged();
            }
        }

        public string RuleColor3
        {
            get { return ruleColor3; }
            set
            {
                ruleColor3 = value;
                OnPropertyChanged();
            }
        }

        public string ResultColor
        {
            get { return resultColor; }
            set
            {
                resultColor = value;
                OnPropertyChanged();
            }
        }

        public string ResultText
        {
            get { return resultText; }
            set
            {
                resultText = value;
                OnPropertyChanged();
            }
        }

        public ICommand UpdateNameCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
    }
}
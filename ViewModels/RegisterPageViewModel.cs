using System.Text.RegularExpressions;

namespace Voxerra.ViewModels
{
    public class RegisterPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private CancellationTokenSource _debounceNameTokenSource;
        private CancellationTokenSource _debounceEmailTokenSource;
        private CancellationTokenSource _debouncePasswordTokenSource;
        private CancellationTokenSource _debounceRePasswordTokenSource;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ServiceProvider _serviceProvider;

        public RegisterPageViewModel(ServiceProvider serviceProvider)
        {
            ButtonStatus = false;
            
            RegisterCommand = new Command(() =>
            {
                if (IsProcessing) return;

                if (!isUserNameUnique || !isEmailUnique
                    || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(RePassword)
                    && Password == RePassword) return;

                IsProcessing = true;
                Register().GetAwaiter().OnCompleted(() =>
                {
                    IsProcessing = false;
                });
            });

            GoBackCommand = new Command(OnGoBack);

            _serviceProvider = serviceProvider;
        }

        public void ClearEnteries()
        {
            userName = "";
            email = "";
            password = "";
            repassword = "";
            
            OnPropertyChanged(nameof(UserName));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Password));
            OnPropertyChanged(nameof(RePassword));
        }
        
        private async void OnGoBack()
        {
            ClearEnteries();
            await Shell.Current.GoToAsync("//LoginPage");
        }
        private async Task Register()
        {
            try
            {
                var request = new RegistrationInitializeRequest
                {
                    Username = UserName,
                    Password = Password,
                    Email = Email
                };
                var response = await _serviceProvider.CallWebApi<RegistrationInitializeRequest, BaseResponse>
                ("/Registration/RegisterUser", HttpMethod.Post, request);


                if (response.StatusCode == 200)
                {
                    await Shell.Current.GoToAsync($"RegisterConfirmationPage?email={Email}");

                    //OnGoBack();
                }
                else
                {
                    await AppShell.Current.DisplayAlert("Voxerra", response.StatusMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
            }
        }
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
        }
        private bool IsValidUsername(string usernameTest)
        {
            if (string.IsNullOrWhiteSpace(usernameTest) || usernameTest.Length < 3 || usernameTest.Length > 18) 
                return false;

            // (a-z or A-Z)
            // cislo moze byt po pismenku
            // bez medzier
            string pattern = @"^[A-Za-z][A-Za-z0-9]{2,}$";

            return Regex.IsMatch(usernameTest, pattern);
        }
        private async Task IsEmailUniqueCall(string emailTest)
        {
            try
            {
                var response = await _serviceProvider.CallWebApi<string, BaseResponse>(
                    "/Registration/IsEmailUnique", HttpMethod.Post, emailTest);
                
                if (response.StatusCode == 200)
                {
                    isEmailUnique = true;
                    LabelIconE = "check";
                    LabelColorE = "Green";
                }
                else 
                {
                    isEmailUnique = false;
                    LabelIconE = "close";
                    LabelColorE = "Red";
                    //await AppShell.Current.DisplayAlert("Voxerra", "Email is already in use.", "OK");
                }
            }
            catch (Exception ex)
            {
                LabelIconE = "close";
                LabelColorE = "Red";
                isEmailUnique = false;
                await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
            }
        }
        private async Task IsUserNameUniqueCall(string userNameTest)
        {
            try
            {
                var response = await _serviceProvider.CallWebApi<string, BaseResponse>(
                    "/Registration/IsUserNameUnique", HttpMethod.Post, userNameTest);
                
                if (response.StatusCode == 200)
                {
                    isUserNameUnique = true;
                    LabelIconN = "check";
                    LabelColorN = "Green";
                }
                else
                {
                    isUserNameUnique = false;
                    LabelIconN = "close";
                    LabelColorN = "Red";
                    //await AppShell.Current.DisplayAlert("Voxerra", "Email is already in use.", "OK");
                }
            }
            catch (Exception ex)
            {
                isUserNameUnique = false;
                await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
            }
        }
        private void PasswordCheck(string passwordCheck)
        {
            LabelIconP = "";
            if (string.IsNullOrWhiteSpace(passwordCheck))
            {
                isPasswordCorrect = false;
                LabelIconP = "close";
                LabelColorP = "Red";
                return;
            }
            if (passwordCheck.Length < 4 || passwordCheck.Length >= 32)
            {
                isPasswordCorrect = false;
                LabelIconP = "close";
                LabelColorP = "Red";
                return;
            }
            
            isPasswordCorrect = true;
            LabelIconP = "check";
            LabelColorP = "Green";
        }
        private void RePasswordCheck(string repasswordCheck)
        {
            LabelIconRp = "";
            if (!isPasswordCorrect)
            {
                isRepasswordCorrect = false;
                LabelIconRp = "close";
                LabelColorRp = "Red";
                return;
            }
            if (Password != repasswordCheck) 
            {
                isRepasswordCorrect = false;
                LabelIconRp = "close";
                LabelColorRp = "Red";
                return;
            }
            
            isRepasswordCorrect = true;
            LabelIconRp = "check";
            LabelColorRp = "Green";
        }
        private async void DebounceName(string input)
        {
            _debounceNameTokenSource?.Cancel();
            _debounceNameTokenSource = new CancellationTokenSource();
            var token = _debounceNameTokenSource.Token;

            try
            {
                IsProcessing = true;
                await Task.Delay(1500, token);
                await IsUserNameUniqueCall(input);
                DoneCheck();
                IsProcessing = false;
            }
            catch (TaskCanceledException) { }
        }
        private async void DebounceEmail(string input)
        {
            _debounceEmailTokenSource?.Cancel();
            _debounceEmailTokenSource = new CancellationTokenSource();
            var token = _debounceEmailTokenSource.Token;

            try
            {
                IsProcessing = true;
                await Task.Delay(1500, token);
                await IsEmailUniqueCall(input);
                DoneCheck();
                IsProcessing = false;
            }
            catch (TaskCanceledException) { }
        }
        private async void DebouncePassword(string input)
        {
            _debouncePasswordTokenSource?.Cancel();
            _debouncePasswordTokenSource = new CancellationTokenSource();
            var token = _debouncePasswordTokenSource.Token;

            try
            {
                IsProcessing = true;
                await Task.Delay(1000, token);
                PasswordCheck(input);
                DoneCheck();
                IsProcessing = false;
            }
            catch (TaskCanceledException) { }
        }
        private async void DebounceRePassword(string input)
        {
            _debounceRePasswordTokenSource?.Cancel();
            _debounceRePasswordTokenSource = new CancellationTokenSource();
            var token = _debounceRePasswordTokenSource.Token;
            
            try
            {
                IsProcessing = true;
                await Task.Delay(1000, token);
                RePasswordCheck(input);
                DoneCheck();
                IsProcessing = false;
            }
            catch (TaskCanceledException) { }
        }

        private void DoneCheck()
        {
            if (isUserNameUnique && 
                isEmailUnique &&
                isPasswordCorrect &&
                isRepasswordCorrect)
            {
                ButtonStatus = true;
                return;
            }
            ButtonStatus = false;
            FrameBackground = ButtonStatus ? CreateDisabledBrush() : CreateEnabledBrush();
        }
        private Brush CreateEnabledBrush()
        {
            return new LinearGradientBrush
            {
                GradientStops = new GradientStopCollection
                {
                    new GradientStop { Color = Color.FromArgb("#b414de"), Offset = 0.0f },
                    new GradientStop { Color = Color.FromArgb("#d127a5"), Offset = 1.0f }
                }
            };
        }
        private Brush CreateDisabledBrush()
        {
            return new SolidColorBrush(Colors.Gray);
        }
        
        
        private string userName;
        private string email;
        private string password;
        private string repassword;
        private bool isProcessing;
        private bool isEmailUnique;
        private bool isUserNameUnique;
        private bool isPasswordCorrect;
        private bool isRepasswordCorrect;
        
        private string labelIconN;
        private string labelColorN;
        
        private string labelIconE;
        private string labelColorE;
        
        private string labelIconP;
        private string labelColorP;
        
        private string labelIconRp;
        private string labelColorRp;

        private bool buttonStatus;
        private Brush _frameBackground;
        
        public string UserName
        {
            get { return userName; }
            set 
            { 
                ButtonStatus = false;
                if (userName == value) return;
                
                userName = value; 
                OnPropertyChanged();
                if (IsValidUsername(value))
                {
                    DebounceName(value);
                }
                else
                {
                    isUserNameUnique = false;
                }
            }
        }
        public string Email
        {
            get { return email; }
            set 
            { 
                ButtonStatus = false;
                if (email == value) return;
                
                email = value; 
                OnPropertyChanged(); 
                
                if (IsValidEmail(value))
                {
                    DebounceEmail(value);
                }
                else
                {
                    isEmailUnique = false;
                }
            }
        }
        public string Password
        {
            get { return password; }
            set 
            { 
                ButtonStatus = false;
                password = value; 
                OnPropertyChanged();
                DebouncePassword(value);
            }
        }
        public string RePassword
        {
            get { return repassword; }
            set
            {
                ButtonStatus = false;
                repassword = value; 
                OnPropertyChanged(); 
                DebounceRePassword(value);
            }
        }
        public bool IsProcessing
        {
            get { return isProcessing; }
            set { isProcessing = value; OnPropertyChanged(); }
        }
        
        public string LabelIconN
        {
            get { return labelIconN; }
            set { labelIconN = value; OnPropertyChanged(); }
        }
        public string LabelColorN
        {
            get { return labelColorN; }
            set { labelColorN = value; OnPropertyChanged(); }
        }
        
        public string LabelIconE
        {
            get { return labelIconE; }
            set { labelIconE = value; OnPropertyChanged(); }
        }
        public string LabelColorE
        {
            get { return labelColorE; }
            set { labelColorE = value; OnPropertyChanged(); }
        }
        
        public string LabelIconP
        {
            get { return labelIconP; }
            set { labelIconP = value; OnPropertyChanged(); }
        }
        public string LabelColorP
        {
            get { return labelColorP; }
            set { labelColorP = value; OnPropertyChanged(); }
        }
        
        public string LabelIconRp
        {
            get { return labelIconRp; }
            set { labelIconRp = value; OnPropertyChanged(); }
        }
        public string LabelColorRp
        {
            get { return labelColorRp; }
            set { labelColorRp = value; OnPropertyChanged(); }
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
        public Brush FrameBackground
        {
            get => _frameBackground;
            set
            {
                _frameBackground = value;
                OnPropertyChanged();
            }
        }
        public ICommand RegisterCommand { get; set; }
        public ICommand GoBackCommand { get; set; }

    }
}

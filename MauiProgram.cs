

namespace Voxerra
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("OpenSans-Bold.ttf", "OpenSansBold");
                    fonts.AddFont("MaterialSymbolsRounded-Thin.ttf", "GoogleFontThin");
                    fonts.AddFont("MaterialSymbolsRounded-Regular.ttf", "GoogleFont");
                    fonts.AddFont("MaterialSymbolsRounded_Filled-Regular.ttf", "GoogleFontFilled");

                });


#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<ChatHub>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<ServiceProvider>();
            builder.Services.AddSingleton<DataCenterService>();

            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<LoginPageViewModel>();
            builder.Services.AddSingleton<RegisterPage>();
            builder.Services.AddSingleton<RegisterPageViewModel>();
            builder.Services.AddSingleton<RegisterConfirmationPage>();
            builder.Services.AddSingleton<RegisterConfirmationViewModel>();
            //builder.Services.AddSingleton<ForgotPasswordPage>();
            //builder.Services.AddSingleton<ForgotPasswordViewModel>();
            //builder.Services.AddSingleton<PasswordConfirmationPage>();
            //builder.Services.AddSingleton<PasswordConfirmationViewModel>();


            builder.Services.AddSingleton<MessageCenterPage>();
            builder.Services.AddSingleton<MessageCenterPageViewModel>();
            builder.Services.AddSingleton<ChatPage>();
            builder.Services.AddSingleton<ChatPageViewModel>();

            builder.Services.AddSingleton<AddFriendPage>();
            builder.Services.AddSingleton<AddFriendViewModel>();
            builder.Services.AddSingleton<PublicProfilePage>();
            builder.Services.AddSingleton<PublicProfileViewModel>();


            builder.Services.AddSingleton<ProfilePage>();
            builder.Services.AddSingleton<ProfilePageViewModel>();
            builder.Services.AddSingleton<FriendRequestPage>();
            builder.Services.AddSingleton<FriendRequestViewModel>();
            builder.Services.AddSingleton<MainSettingPage>();
            builder.Services.AddSingleton<MainSettingViewModel>();



            builder.Services.AddSingleton<AccountDetailsPage>();
            builder.Services.AddSingleton<AccountDetailsViewModel>();
            
            builder.Services.AddSingleton<NamePage>();
            builder.Services.AddSingleton<NameViewModel>();
            builder.Services.AddSingleton<EmailPage>();
            builder.Services.AddSingleton<EmailViewModel>();
            builder.Services.AddSingleton<ProfilePicturePage>();
            builder.Services.AddSingleton<ProfilePictureViewModel>();
            builder.Services.AddSingleton<BioPage>();
            builder.Services.AddSingleton<BioViewModel>();
            builder.Services.AddSingleton<DeleteAccountPage>();
            builder.Services.AddSingleton<DeleteAccountViewModel>();
            
            builder.Services.AddSingleton<NotificationsPage>();
            builder.Services.AddSingleton<NotificationsViewModel>();
            
            
            builder.Services.AddSingleton<SecurityPage>();
            builder.Services.AddSingleton<SecurityViewModel>();
            
            builder.Services.AddSingleton<PasswordPage>();
            builder.Services.AddSingleton<PasswordViewModel>();
            builder.Services.AddSingleton<TwoFactorAuthPage>();
            builder.Services.AddSingleton<TwoFactorAuthViewModel>();
            builder.Services.AddSingleton<SavedLoginPage>();
            builder.Services.AddSingleton<SavedLoginViewModel>();
            builder.Services.AddSingleton<WhereIsUserLoggedPage>();
            builder.Services.AddSingleton<WhereIsUserLoggedViewModel>();
            builder.Services.AddSingleton<LoginAlertsPage>();
            builder.Services.AddSingleton<LoginAlertsViewModel>();

            return builder.Build();
        }
    }
}

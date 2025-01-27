
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
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<MessageCenterPage>();
            builder.Services.AddSingleton<ChatPage>();
            builder.Services.AddSingleton<RegisterPage>();
            builder.Services.AddSingleton<RegisterConfirmationPage>();
            builder.Services.AddSingleton<ForgotPasswordPage>();
            builder.Services.AddSingleton<ProfilePage>();
            builder.Services.AddSingleton<AddFriendPage>();
            builder.Services.AddSingleton<PublicProfilePage>();
            
            builder.Services.AddSingleton<LoginPageViewModel>();
            builder.Services.AddSingleton<MessageCenterPageViewModel>();
            builder.Services.AddSingleton<ChatPageViewModel>();
            builder.Services.AddSingleton<ServiceProvider>();
            builder.Services.AddSingleton<RegisterPageViewModel>();
            builder.Services.AddSingleton<RegisterConfirmationViewModel>();
            builder.Services.AddSingleton<ForgotPasswordViewModel>();
            builder.Services.AddSingleton<ProfilePageViewModel>();
            builder.Services.AddSingleton<AddFriendViewModel>();
            builder.Services.AddSingleton<PublicProfileViewModel>();
            
            return builder.Build();
        }
    }
}

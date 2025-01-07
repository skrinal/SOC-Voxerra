using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;

namespace Voxerra.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        //protected override void OnCreate(Bundle? savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);

        //    CreateNotificationFromIntent(Intent);
        //}

        //protected override void OnNewIntent(Intent? intent)
        //{
        //    base.OnNewIntent(intent);

        //    CreateNotificationFromIntent(intent);
        //}

        //static void CreateNotificationFromIntent(Intent intent)
        //{
        //    if (intent?.Extras != null)
        //    {
        //        string title = intent.GetStringExtra(NotificationManagerService.TitleKey);
        //        string message = intent.GetStringExtra(NotificationManagerService.MessageKey);

        //        var service = IPlatformApplication.Current.Services.GetService<INotificationManagerService>();
        //        service.ReceiveNotification(title, message);
        //    }
        //}




        //const string CHANNEL_ID = "1212345";

        //protected override void OnCreate(Bundle? savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);
        //    CreateNotificationChannel();
        //}

        //private void CreateNotificationChannel()
        //{
        //    if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        //    {
        //        var channel = new NotificationChannel(CHANNEL_ID, "My Notifications", NotificationImportance.Default)
        //        {
        //            Description = "Channel for my app notifications"
        //        };

        //        var notificationManager = (NotificationManager)GetSystemService(NotificationService);
        //        notificationManager.CreateNotificationChannel(channel);
        //    }
        //}

        //public void SendNotification(string title, string message)
        //{
        //    var notificationBuilder = new NotificationCompat.Builder(this, CHANNEL_ID)
        //        .SetContentTitle(title)
        //        .SetContentText(message)
        //        //.SetSmallIcon(Resource.Drawable.ic_notification) // Ensure you have an icon in Resources/drawable
        //        .SetPriority(NotificationCompat.PriorityDefault)
        //        .SetAutoCancel(true);

        //    var notificationManager = NotificationManagerCompat.From(this);
        //    notificationManager.Notify(0, notificationBuilder.Build());
        //}


        //protected override void OnCreate(Bundle? savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);

        //    // Example usage of NotificationService
        //    var notificationService = new NotificationService();
        //    notificationService.SendNotification("Hello", "This is a notification from .NET MAUI!");
        //}


        //protected override void OnCreate(Bundle? savedInstanceState)
        //{
        //    MessagingCenter.Unsubscribe<string, string[]>(this, "MessageNotificationService");
        //    MessagingCenter.Subscribe<string, string[]>(this, "MessageNotificationService", (sender, args) =>
        //    {
        //        var serviceIntent = new Intent(this, typeof(MessageNotificationService));
        //        serviceIntent.SetAction(sender);
        //        serviceIntent.PutStringArrayListExtra("param", args);
        //        StartService(serviceIntent);
        //    });
        //    base.OnCreate(savedInstanceState);
        //}
    }
}

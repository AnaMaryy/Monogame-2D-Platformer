using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;

namespace Platformer.Android
{
    [Activity(
        //Label = "@string/app_name",
        Label = "Archie's Adventures",
        MainLauncher = true,
        Icon = "@drawable/icon2",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.Landscape,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
    )]
    public class Activity1 : AndroidGameActivity
    {
        private Game1 _game;
        private View _view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _game = new Game1();
            _view = _game.Services.GetService(typeof(View)) as View;
            SetContentView(_view);
            _game.Run();
            /*
            _game.GameFinished +=
              (sender, e) =>
              {
                  Process.KillProcess(Process.MyPid());
              };*/
            //SetImmersive();
            //_view.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.HideNavigation | (StatusBarVisibility)SystemUiFlags.ImmersiveSticky;
            //_view.SetOnSystemUiVisibilityChangeListener(new MyUiVisibilityChangeListener(_view));
            HideSystemUI();

        }
        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            if (hasFocus)
                SetImmersive();
        }
        private void SetImmersive()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
                _view.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.LayoutStable | SystemUiFlags.LayoutHideNavigation | SystemUiFlags.LayoutFullscreen | SystemUiFlags.HideNavigation | SystemUiFlags.Fullscreen | SystemUiFlags.ImmersiveSticky);
        }
        private class MyUiVisibilityChangeListener : Java.Lang.Object, View.IOnSystemUiVisibilityChangeListener
        {
            View targetView;
            public MyUiVisibilityChangeListener(View v)
            {
                targetView = v;
            }
            public void OnSystemUiVisibilityChange(StatusBarVisibility v)
            {
                if (targetView.SystemUiVisibility != ((StatusBarVisibility)SystemUiFlags.HideNavigation | (StatusBarVisibility)SystemUiFlags.Immersive))
                {
                    targetView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.HideNavigation | (StatusBarVisibility)SystemUiFlags.ImmersiveSticky;
                }
            }
        }
        public void OnSystemUiVisibilityChange(StatusBarVisibility visibility)
        {
            HideSystemUI();
        }

        private void HideSystemUI()
        {
            SystemUiFlags flags = SystemUiFlags.HideNavigation | SystemUiFlags.Fullscreen | SystemUiFlags.ImmersiveSticky;
            this.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)flags;
        }
    }
    
}

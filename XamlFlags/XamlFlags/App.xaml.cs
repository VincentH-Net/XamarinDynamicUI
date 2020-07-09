using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamlFlags
{
    public partial class App : Application
    {
        public App()
        {
            Device.SetFlags(new string[]{ "Markup_Experimental" });

            InitializeComponent();

            //MainPage = new MainPage();
            MainPage = new MainPageCSharpMarkup();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

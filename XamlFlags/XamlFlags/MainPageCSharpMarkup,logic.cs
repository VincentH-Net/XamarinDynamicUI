using Xamarin.Forms;

namespace XamlFlags
{
    partial class MainPageCSharpMarkup : ContentPage
    {
        readonly MainPageViewModel vm = new MainPageViewModel();

        public MainPageCSharpMarkup()
        {
            BindingContext = vm;
            Build();
        }
    }
}
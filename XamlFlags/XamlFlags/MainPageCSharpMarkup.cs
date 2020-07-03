using Xamarin.Forms;
using Xamarin.Forms.Markup;
using static Xamarin.Forms.Markup.Factory;

namespace XamlFlags
{
    class MainPageCSharpMarkup : ContentPage
    {
        public MainPageCSharpMarkup()
        {
            BindingContext = new MainPageViewModel();

            Content = StackLayout (() => 
                Frame (
                    StackLayout (
                        // TODO: wip
                    )
                )
            );
        }
    }
}

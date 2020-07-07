using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using static Xamarin.Forms.Markup.Factory;
using static XamlFlags.Colors;

namespace XamlFlags
{
    class MainPageCSharpMarkup : ContentPage
    {
        public MainPageCSharpMarkup()
        {
            var vm = new MainPageViewModel(); BindingContext = vm;

            Content = StackLayout (() => 
                Frame (
                    StackLayout (
                        Button ("Select")
                            .BindCommand (nameof(vm.SelectTypeCommand), source: BindingContext),

                        Label ("✓")
                            .TextColor (White) .FontSize (12)
                            .EndExpand () .CenterVertical (),
                            // TODO: triggers

                        Label ()
                            .TextColor (Black)
                            .EndExpand () .CenterVertical ()
                            .Bind (nameof(OptionViewModel.Value))
                            // TODO: triggers

                    // TODO: wip
                    ) .Horizontal () .Padding (5)
                      .ItemsSource (nameof(vm.Options))
                      // TODO: wip .Bind (Xamarin.Forms.StackLayout.BackgroundColorProperty, nameof()
                      // TODO: Triggers
                      // TODO: multibinding / convertor with multiple values, or fody calculated property if needed
                ) .CornerRadius (4) .Padding (0)
            );
        }
    }
}

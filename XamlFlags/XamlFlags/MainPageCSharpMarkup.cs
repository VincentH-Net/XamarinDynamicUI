using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using static Xamarin.Forms.Markup.Factory;
using static XamlFlags.Colors;
using Option = XamlFlags.OptionViewModel;

namespace XamlFlags
{
    class MainPageCSharpMarkup : ContentPage
    {
        public MainPageCSharpMarkup()
        {
            var vm = new MainPageViewModel(); BindingContext = vm;

            Content = StackLayout(() =>
               Frame(
                   StackLayout(
                       Button("Select")
                           .BindCommand(nameof(vm.SelectTypeCommand), source: BindingContext),

                       Label("✓")
                           .TextColor(White).FontSize(12)
                           .EndExpand().CenterVertical()
                           .Bind(Xamarin.Forms.Label.IsVisibleProperty, nameof(Option.IsSelected)),

                       Label()
                           .TextColor(Black)
                           .EndExpand().CenterVertical()
                           .Bind(nameof(Option.Value))
                           .MultiBind(
                                Xamarin.Forms.Label.TextColorProperty,
                                Binding(nameof(Option.IsEnabled)),
                                Binding(nameof(Option.IsSelected)),
                                ((bool isEnabled, bool isSelected) option) => option.isEnabled ? (option.isSelected ? White : Black) : LightGray)

                   ).Horizontal().Padding(5)
                     .Bind(Xamarin.Forms.StackLayout.IsEnabledProperty, nameof(Option.IsEnabled)) // TODO: what is a datatrigger and why not just bind like this?
                     .MultiBind(
                          Xamarin.Forms.StackLayout.BackgroundColorProperty,
                          Binding(nameof(Option.IsEnabled)),
                          Binding(nameof(Option.IsSelected)),
                          ((bool isEnabled, bool isSelected) option) => option.isEnabled ? (option.isSelected ? DarkBlue : White) : DarkGray)
               ).CornerRadius(4).Padding(0)
            ) .ItemsSource (nameof(vm.Options));
;       }
    }



       // TODO: idea - use fluent chain to eliminate repeating class name in property bind. So:
       // .StackLayout() .Bind(...) // default property
       // .StackLayout() .BackgroundColor() .Bind(...)
       // .StackLayout() .BackgroundColor(value)

       // Downsides of factory methods with params instead of object initializers { }:
       // - Editor does not show vertical lines, no collapse
       // - Name clash with static class members - all bindable properties!


    // Hmm maybe a generic BindableProperty
    // or a parameter in the factory method that takes a Action<StackLayout>  ?
    // s => s.Bind(s.BackgroundColorProperty, ...)
    // .Bind(StackLayout.BackgroundColorProperty, ...)
    // in XAML you have assignment of each property with either value or binding. THAT is what we want.

}

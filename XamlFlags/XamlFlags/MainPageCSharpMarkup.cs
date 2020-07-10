using Xamarin.Forms;
using Xamarin.Forms.Markup;
using static Xamarin.Forms.Markup.Factory;
using static XamlFlags.Colors;
using Option = XamlFlags.OptionViewModel;

namespace XamlFlags
{
    // This is the most straightforward implementation of MainPage in C# Markup,
    // using Fody PropertyChanged to automatically make calculated properties data bindable.

    partial class MainPageCSharpMarkup
    {
        void Build() => Content = 
            VStack (() => 
                Frame (
                    HStack (
                        Button ("Select")
                           .BindCommand (nameof(vm.SelectTypeCommand), source: vm),

                        Label ("✓")
                           .TextColor (White) .FontSize (12)
                           .EndExpand () .CenterVertical ()
                           .IsVisible().Bind (nameof(Option.IsSelected)),

                        Label ()
                           .TextColor().Bind (nameof(Option.TextColor))
                           .EndExpand () .CenterVertical ()
                           .Bind (nameof(Option.Value))

                    )  .Color().Bind (nameof(Option.BackgroundColor))
                       .Horizontal() .Padding (5)
                       .IsEnabled().Bind (nameof(Option.IsEnabled))
                )  .CornerRadius (4) .Padding (0)
        )  .ItemsSource (vm.Options);
    }

    partial class OptionViewModel
    {
        public Color TextColor       => IsEnabled ? (IsSelected ? White    : Black) : LightGray;
        public Color BackgroundColor => IsEnabled ? (IsSelected ? DarkBlue : White) : DarkGray;
    }

    // Note that in contrast to Blazor, which only updates in response to UI events, 
    // this will also update correctly when the data is changed from background events
    // (e.g. data coming in from an API)
}
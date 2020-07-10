using Xamarin.Forms;
using Xamarin.Forms.Markup;
using static Xamarin.Forms.Markup.Factory;
using static XamlFlags.Colors;
using Option = XamlFlags.OptionViewModel;

namespace XamlFlags
{
    // This is an alternative implementation of MainPage, using MultiBindings instead of Fody PropertyChanged

    partial class MainPageCSharpMarkup
    {
        void BuildWithMultiBindings() => Content = 
            StackLayout(() => Frame (
                StackLayout (
                    Button ("Select")
                        .BindCommand (nameof(vm.SelectTypeCommand), source: vm),

                    Label ("✓")
                        .TextColor (White) .FontSize (12)
                        .EndExpand () .CenterVertical ()
                        .IsVisible().Bind (nameof(Option.IsSelected)),

                    Label ()
                        .TextColor (Black)
                        .EndExpand () .CenterVertical ()
                        .Bind (nameof(Option.Value))
                        .TextColor().MultiBind (
                            Binding (nameof(Option.IsEnabled)), 
                            Binding (nameof(Option.IsSelected)), 
                            ((bool isEnabled, bool isSelected) option)
                            => option.isEnabled ? (option.isSelected ? White : Black) : LightGray)

                ) .Horizontal() .Padding (5)
                    .IsEnabled().Bind (nameof(Option.IsEnabled))
                    .Color().MultiBind (
                        Binding (nameof(Option.IsEnabled)), 
                        Binding (nameof(Option.IsSelected)), 
                        ((bool isEnabled, bool isSelected) option)
                        => option.isEnabled ? (option.isSelected ? DarkBlue : White) : DarkGray)
            ) .CornerRadius (4) .Padding (0)
        ) .ItemsSource (vm.Options);
    }

    // Note that in contrast to Blazor, which only updates in response to UI events, 
    // this will also update correctly when the data is changed from background events
    // (e.g. data coming in from an API)
}
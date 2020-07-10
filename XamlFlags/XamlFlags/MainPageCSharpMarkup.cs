﻿using Xamarin.Forms;
using Xamarin.Forms.Markup;
using static Xamarin.Forms.Markup.Factory;
using static XamlFlags.Colors;
using Option = XamlFlags.OptionViewModel;

namespace XamlFlags
{
    // This is the most straightforward implementation of MainPage in C#. 
    // It uses Fody PropertyChanged to automatically make calculated properties data bindable.

    partial class MainPageCSharpMarkup
    {
        void Build() => Content = 
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
                        .TextColor().Bind(nameof(Option.TextColor))

                )  .Horizontal() .Padding (5)
                   .IsEnabled().Bind (nameof(Option.IsEnabled))
                   .Color().Bind (nameof(Option.BackgroundColor))
            )  .CornerRadius (4) .Padding (0)
        )  .ItemsSource (vm.Options);
    }

    public partial class OptionViewModel
    {
        public Color TextColor       => IsEnabled ? (IsSelected ? White    : Black) : LightGray;
        public Color BackgroundColor => IsEnabled ? (IsSelected ? DarkBlue : White) : DarkGray;
    }

    // Note that in contrast to Blazor, which only updates in response to UI events, 
    // this will also update correctly when the data is changed from background events
    // (e.g. data coming in from an API)
}
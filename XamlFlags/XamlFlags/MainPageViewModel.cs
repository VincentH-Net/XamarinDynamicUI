using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace XamlFlags
{
    public class MainPageViewModel : BindableBase
    {
        public ICommand SelectTypeCommand { get; }
        public List<OptionViewModel> Options { get; } = new List<OptionViewModel>
        {
            new OptionViewModel { Value = "Option 1-A", Category = "1", Variety = "A" },
            new OptionViewModel { Value = "Option 1-B", Category = "1", Variety = "B" },
            new OptionViewModel { Value = "Option 2-A", Category = "2", Variety = "A" },
            new OptionViewModel { Value = "Option 2-B", Category = "2", Variety = "B" },
            new OptionViewModel { Value = "Option 3-A", Category = "3", Variety = "A" },
            new OptionViewModel { Value = "Option 3-B", Category = "3", Variety = "B" },
        };

        public MainPageViewModel()
        {
            SelectTypeCommand = new Command<OptionViewModel>(OnSelectType);
            OnSelectType(Options.First());
        }

        private void OnSelectType(OptionViewModel option)
        {
            if (option is null) return;

            // reset all options
            Options.ForEach(o => { o.IsEnabled = false; o.IsSelected = false; });

            // enable options of the same variety (ie. A,B)
            Options.Where(o => o.Variety == option.Variety)
                .ForEach(o => { o.IsEnabled = true; });

            // enable options of the same category (ie. 1,2,3)
            Options.Where(o => o.Category == option.Category)
                .ForEach(o => { o.IsEnabled = true; });

            // select the current option
            option.IsSelected = true;
        }
    }

    public partial class OptionViewModel : BindableBase
    {
        public string Value { get; set; }
        public string Variety { get; set; }
        public string Category { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsSelected { get; set; }
    }

    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = null, Action onChanged = null, Action<T> onChanging = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            onChanging?.Invoke(value);

            backingStore = value;

            onChanged?.Invoke();
            OnPropertyChanged(propertyName);

            return true;
        }
    }
}

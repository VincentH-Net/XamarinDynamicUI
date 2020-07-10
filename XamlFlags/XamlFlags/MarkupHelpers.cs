using System;
using System.Collections;
using System.Globalization;

namespace Xamarin.Forms.Markup // TODO: Complete and add to the appropriate classes in Forms & MAUI
{
    public static class LayoutExtensions
    {
		// Layout<View> extensions
		public static TLayout EmptyView<TLayout>(this TLayout layout, object view) where TLayout : Layout<View>
		{ BindableLayout.SetEmptyView(layout, view); return layout; }

		public static TLayout EmptyViewTemplate<TLayout>(this TLayout layout, DataTemplate template) where TLayout : Layout<View>
		{ BindableLayout.SetEmptyViewTemplate(layout, template); return layout; }

		public static TLayout ItemsSource<TLayout>(this TLayout layout, IEnumerable source) where TLayout : Layout<View>
		{ BindableLayout.SetItemsSource(layout, source); return layout; }

		public static TLayout ItemTemplate<TLayout>(this TLayout layout, DataTemplate template) where TLayout : Layout<View>
		{ BindableLayout.SetItemTemplate(layout, template); return layout; }

		public static TLayout ItemTemplateSelector<TLayout>(this TLayout layout, DataTemplateSelector selector) where TLayout : Layout<View>
		{ BindableLayout.SetItemTemplateSelector(layout, selector); return layout; }

		// StackLayout extensions
		public static TLayout Orientation<TLayout>(this TLayout layout, StackOrientation orientation) where TLayout : StackLayout
		{ layout.Orientation = orientation; return layout; }

		public static TLayout Spacing<TLayout>(this TLayout layout, double spacing) where TLayout : StackLayout
		{ layout.Spacing = spacing; return layout; }
    }

	public static class Extensions
	{
		public static TFrame CornerRadius<TFrame>(this TFrame frame, float radius) where TFrame : Frame
		{ frame.CornerRadius = radius; return frame; }

		public static TStackLayout Horizontal<TStackLayout>(this TStackLayout stackLayout) where TStackLayout : StackLayout
		{ stackLayout.Orientation = StackOrientation.Horizontal; return stackLayout; }

		public static TStackLayout Vertical<TStackLayout>(this TStackLayout stackLayout) where TStackLayout : StackLayout
		{ stackLayout.Orientation = StackOrientation.Vertical; return stackLayout; }

		public static TVisualElement Color<TVisualElement>(this TVisualElement element, Color color) where TVisualElement : VisualElement
		{ element.BackgroundColor = color; return element; }

		public static BindPropertyChain<TVisualElement> Color<TVisualElement>(this TVisualElement element) where TVisualElement : VisualElement
		=> new BindPropertyChain<TVisualElement>(element, VisualElement.BackgroundColorProperty);

		public static BindPropertyChain<TVisualElement> IsEnabled<TVisualElement>(this TVisualElement element) where TVisualElement : VisualElement
		=> new BindPropertyChain<TVisualElement>(element, VisualElement.IsEnabledProperty);

		public static BindPropertyChain<TVisualElement> IsVisible<TVisualElement>(this TVisualElement element) where TVisualElement : VisualElement
		=> new BindPropertyChain<TVisualElement>(element, VisualElement.IsVisibleProperty);

		public static BindPropertyChain<TLabel> TextColor<TLabel>(this TLabel label) where TLabel : Label
		=> new BindPropertyChain<TLabel>(label, Label.TextColorProperty);

		public static TTextElement TextColor<TTextElement>(this TTextElement element, Color color) where TTextElement : Label // TODO: ITextElement (is internal)
		{ element.TextColor = color; return element; }

		public static TBindable Bind<TBindable>(
			this BindPropertyChain<TBindable> chain,
			string path = ".",
			object source = null
		) where TBindable : BindableObject
		{ chain.Parent.Bind(chain.Property, path: path, source: source); return chain.Parent; }

		public static TBindable Bind<TBindable, TSource, TDest>(
			this BindPropertyChain<TBindable> chain,
			string path = ".",
			Func<TSource, TDest> convert = null, 
			Func<TDest, TSource> convertBack = null, 
			object source = null
		) where TBindable : BindableObject
		{ chain.Parent.Bind(chain.Property, path: path, convert: convert, convertBack: convertBack, source: source); return chain.Parent; }

		/// <summary>Bind to a specified property with 2 bindings and an inline convertor</summary>
		public static TBindable MultiBind<TBindable, TSource1, TSource2, TDest>(
			this BindPropertyChain<TBindable> chain,
			BindingBase binding1,
			BindingBase binding2,
			Func<ValueTuple<TSource1, TSource2>, TDest> convert = null,
			Func<TDest, ValueTuple<TSource1, TSource2>> convertBack = null,
			BindingMode mode = BindingMode.Default,
			string stringFormat = null,
			object targetNullValue = null,
			object fallbackValue = null
		) where TBindable : BindableObject
		=> MultiBind<TBindable, TSource1, TSource2, object, TDest>(
			   chain, binding1, binding2, convert, convertBack, null, mode, stringFormat, targetNullValue, fallbackValue
		   );

		/// <summary>Bind to a specified property with 2 bindings, an inline convertor and a converter parameter</summary>
		public static TBindable MultiBind<TBindable, TSource1, TSource2, TParam, TDest>(
			this BindPropertyChain<TBindable> chain,
			BindingBase binding1,
			BindingBase binding2,
			Func<ValueTuple<TSource1, TSource2>, TDest> convert = null,
			Func<TDest, ValueTuple<TSource1, TSource2>> convertBack = null,
			TParam converterParameter = default(TParam),
			BindingMode mode = BindingMode.Default,
			string stringFormat = null,
			object targetNullValue = null,
			object fallbackValue = null
		) where TBindable : BindableObject
		{
			chain.Parent.SetBinding(chain.Property, new MultiBinding {
				Bindings = { binding1, binding2 },
				Converter = new Func2Converter<TSource1, TSource2, TDest>(convert, convertBack),
				Mode = mode,
				StringFormat = stringFormat,
				TargetNullValue = targetNullValue,
				FallbackValue = fallbackValue
			});
			return chain.Parent;
		}

		public class BindPropertyChain<TBindableObject> : Subchain<TBindableObject, BindableProperty> where TBindableObject : BindableObject
		{
			public readonly BindableProperty Property;

			public BindPropertyChain(TBindableObject parent, BindableProperty property) : base(parent) { Property = property; }
		}
	}

	public class Subchain<TParent, TChild> where TParent : class
	{
		public readonly TParent Parent;
		public Subchain(TParent parent) => Parent = parent;

		public static implicit operator TParent(Subchain<TParent, TChild> subchain) => subchain.Parent;
	}

	public static class Factory
    {
		static TLayout AddChildren<TLayout>(this TLayout layout, params View[] views) where TLayout : Layout<View>
        {
			foreach (var view in views) if (view != null) layout.Children.Add(view);
			return layout;
        }

		public static StackLayout StackLayout(params View[] views) => new StackLayout().AddChildren(views);

		public static StackLayout StackLayout(Func<object> loadTemplate) => new StackLayout().ItemTemplate(new DataTemplate(loadTemplate));

		public static StackLayout StackLayout(Type type) => new StackLayout().ItemTemplate (new DataTemplate(type));

		public static Frame Frame(View content) => new Frame { Content = content };

		public static Button Button(string text = null) => new Button { Text = text };

		public static Label Label(string text = null) => new Label { Text = text };

		public static Binding Binding(
			string path = ".",
			BindingMode mode = BindingMode.Default,
			IValueConverter converter = null,
			object converterParameter = null,
			string stringFormat = null,
			object source = null,
			object targetNullValue = null,
			object fallbackValue = null
		) => new Binding (path, mode, converter, converterParameter, stringFormat, source) { TargetNullValue = targetNullValue, FallbackValue = fallbackValue };
	}

	public class FuncMultiConverter<TDest, TParam> : IMultiValueConverter
	{
		readonly Func<object[], TDest> convert;
		readonly Func<TDest, object[]> convertBack;

		readonly Func<object[], TParam, TDest> convertWithParam;
		readonly Func<TDest, TParam, object[]> convertBackWithParam;

		readonly Func<object[], TParam, CultureInfo, TDest> convertWithParamAndCulture;
		readonly Func<TDest, TParam, CultureInfo, object[]> convertBackWithParamAndCulture;

		public FuncMultiConverter(Func<object[], TParam, CultureInfo, TDest> convertWithParamAndCulture = null, Func<TDest, TParam, CultureInfo, object[]> convertBackWithParamAndCulture = null)
		{ this.convertWithParamAndCulture = convertWithParamAndCulture; this.convertBackWithParamAndCulture = convertBackWithParamAndCulture; }

		public FuncMultiConverter(Func<object[], TParam, TDest> convertWithParam = null, Func<TDest, TParam, object[]> convertBackWithParam = null)
		{ this.convertWithParam = convertWithParam; this.convertBackWithParam = convertBackWithParam; }

		public FuncMultiConverter(Func<object[], TDest> convert = null, Func<TDest, object[]> convertBack = null)
		{ this.convert = convert; this.convertBack = convertBack; }

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
			if (convert != null)
				return convert(values);

			if (convertWithParam != null)
				return convertWithParam(
					values,
					parameter != null ? (TParam)parameter : default(TParam));

			if (convertWithParamAndCulture != null)
				return convertWithParamAndCulture(
					values,
					parameter != null ? (TParam)parameter : default(TParam),
					culture);

			return BindableProperty.UnsetValue;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
			if (convertBack != null)
				return convertBack(
					value != null ? (TDest)value : default(TDest));

			if (convertBackWithParam != null)
				return convertBackWithParam(
					value != null ? (TDest)value : default(TDest),
					parameter != null ? (TParam)parameter : default(TParam));

			if (convertBackWithParamAndCulture != null)
				return convertBackWithParamAndCulture(
					value != null ? (TDest)value : default(TDest),
					parameter != null ? (TParam)parameter : default(TParam),
					culture);

			return null;
		}
	}

	// TODO: Consider whether we need this class or whether a short way to create a converter is enough - possibly automatically with a type convertor, a tuple of functions?
	public class Func2Converter<TSource1, TSource2, TDest> : FuncMultiConverter<TDest, object>
	{
		static T To<T>(object value) => value != null ? (T)value : default(T);
		static object[] ToObjects(ValueTuple<TSource1, TSource2> values) => new object[] { values.Item1, values.Item2 };

		public Func2Converter(
			Func<ValueTuple<TSource1, TSource2>, TDest> convert = null, 
			Func<TDest, ValueTuple<TSource1, TSource2>> convertBack = null)
		: base (
			convert     == null ? default(Func<object[], TDest>) : (object[] values) => convert((To<TSource1>(values[0]), To<TSource2>(values[1]))),
			convertBack == null ? default(Func<TDest, object[]>) : (TDest value)     => ToObjects(convertBack(value))
		) { }
	}
}
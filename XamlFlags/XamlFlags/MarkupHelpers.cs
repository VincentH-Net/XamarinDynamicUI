using System;
using System.Collections;
using Xamarin.Forms;

namespace Xamarin.Forms.Markup
{
    public static class LayoutExtensions
    {
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

		public static TTextElement TextColor<TTextElement>(this TTextElement element, Color color) where TTextElement : Label // TODO: ITextElement
		{ element.TextColor = color; return element; }
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
	}
}

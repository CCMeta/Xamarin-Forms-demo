using System;
using System.ComponentModel;
using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android.CollectionView;
using Xamarin.Forms.Platform.Android.FastRenderers;
using ARect = Android.Graphics.Rect;
using AViewCompat = AndroidX.Core.View.ViewCompat;

namespace Xamarin.Forms.Platform.Android
{
	public abstract class CustomItemsViewRenderer<TItemsView, TAdapter, TItemsViewSource> : ItemsViewRenderer<TItemsView, TAdapter, TItemsViewSource>
		where TItemsView : ItemsView
		where TAdapter : ItemsViewAdapter<TItemsView, TItemsViewSource>
		where TItemsViewSource : IItemsViewSource
	{

		protected IItemsLayout ItemsLayout { get; set; }

		public CustomItemsViewRenderer(Context context) : base(context)
		{
		}


	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Xamarin.Forms.Platform.Android.CollectionView;
using FormsCarouselView = Xamarin.Forms.CarouselView;
using System.Linq;

[assembly: Xamarin.Forms.ExportRenderer(typeof(Xamarin.Forms.CarouselView), typeof(Xamarin.Forms.Platform.Android.CustomCarouselViewRenderer))]

namespace Xamarin.Forms.Platform.Android
{
    public class CustomCarouselViewRenderer : ItemsViewRenderer<ItemsView, ItemsViewAdapter<ItemsView, IItemsViewSource>, IItemsViewSource>
    {
        protected override IItemsLayout GetItemsLayout() => Carousel.ItemsLayout;

        ItemDecoration _itemDecoration;
        bool _isSwipeEnabled;
        int _oldPosition;
        int _gotoPosition = -1;
        bool _noNeedForScroll;
        bool _initialized;
        bool _isVisible;
        bool _disposed;

        List<View> _oldViews;

        protected FormsCarouselView Carousel
        {
            get => ItemsView as FormsCarouselView;
            set => ItemsView = value;
        }

        public CustomCarouselViewRenderer(Context context) : base(context)
        {
            _oldViews = new List<View>();
        }

        protected override void UpdateItemsSource()
        {
            UpdateAdapter();
            UpdateEmptyView();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs changedProperty)
        {

            if (changedProperty.PropertyName == "CurrentItem")
            {
                //  因为CurrentItem不是NULL 并且要滚动到不是空的位置 但是坑不够了 到底什么是那个坑的range 再找找
                //var currentItemPosition = ItemsViewAdapter.ItemsSource.GetPosition(Carousel.CurrentItem); 这句是重点
                //_gotoPosition == -1 && currentItemPosition != carouselPosition is true in crash
                var carouselPosition = Carousel.Position;
                //_gotoPosition = carouselPosition;
                //    var currentItem = Carousel?.CurrentItem;

                //if (currentItem == null)
                //    return;

                //var currentItemPosition = ItemsViewAdapter.ItemsSource.GetPosition(currentItem);

                //var currentItem = Carousel?.CurrentItem;
                //if (currentItem is null)
                //    return;
                base.OnElementPropertyChanged(sender, changedProperty);

                //Console.WriteLine("get CurrentItemProperty");
            }
            else
            {
                base.OnElementPropertyChanged(sender, changedProperty);

            };
        }

        protected override void SetUpNewElement(ItemsView newElement)
        {
            Carousel = (newElement as FormsCarouselView);
            //if ((newElement as FormsCarouselView).CurrentItem ==null )
            //    return;
            base.SetUpNewElement(newElement);
        }

        protected override void UpdateAdapter()
        {
            //this is it?
            //yes this is it   it about UpdateAdapter.UpdateAdapter is wrong too much

            base.UpdateAdapter();
            return;



            var _ = Carousel;
            Carousel.SetValueFromRenderer(FormsCarouselView.PositionProperty, 0);
            Carousel.SetValueFromRenderer(FormsCarouselView.CurrentItemProperty, null);


            //new SizedItemContentView();
            var oldItemViewAdapter = ItemsViewAdapter;
            if (oldItemViewAdapter != null)
            {
                Carousel.SetValueFromRenderer(FormsCarouselView.PositionProperty, 0);
                Carousel.SetValueFromRenderer(FormsCarouselView.CurrentItemProperty, null);
            }
            ItemsViewAdapter = oldItemViewAdapter;
            SwapAdapter(ItemsViewAdapter, false);


            //call OnElementPropertyChanged  but where is it .
        }

    }
}
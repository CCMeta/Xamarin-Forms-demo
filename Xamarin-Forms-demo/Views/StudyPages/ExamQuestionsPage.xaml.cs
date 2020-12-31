﻿using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class ExamQuestionsPage : ContentPage
    {
        private readonly ExamQuestionsViewModel ExamQuestionsViewModel;
        public ExamQuestionsPage()
        {
            InitializeComponent();
            BindingContext = ExamQuestionsViewModel = new ExamQuestionsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ExamQuestionsViewModel.GetListAsync();
        }

        private async void ToolbarItem_ClickedAsync(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new SendPostPage());
        }
    }
}
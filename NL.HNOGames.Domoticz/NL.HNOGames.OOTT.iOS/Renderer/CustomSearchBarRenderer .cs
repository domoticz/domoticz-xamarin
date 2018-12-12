using NL.HNOGames.OOTT.iOS.Renderer;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBarRenderer))]
namespace NL.HNOGames.OOTT.iOS.Renderer
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                this.Control.TextChanged += (s, ea) =>
                {
                    this.Control.ShowsCancelButton = true;
                };
                this.Control.OnEditingStarted += (s, ea) => 
                {
                    this.Control.ShowsCancelButton = true;
                };
                this.Control.OnEditingStopped += (s, ea) => 
                {
                    this.Control.ShowsCancelButton = false;
                };
            }
        }
    }
}

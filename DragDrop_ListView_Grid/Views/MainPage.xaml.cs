using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace DragDrop_ListView_Grid.Views
{
    public sealed partial class MainPage : Page
    {
        List<TriChar> CharacterList; 
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Add Character to the list
            CharacterList = new List<TriChar>()
            {
                new TriChar(){Name = "Singular", Skill="All Seeing Eye - Singular is able to forsee the future once you earn his trust, making him a good companion for your first Adventure in Khaland", Symbolic ="\uE7B3"},
                new TriChar(){Name = "Twice", Skill="Eternal Ice - As the only survival during the Artice expedition made her gain the strength to turn any liquid to ice. Twice will be happy to travel alongside you in Khaland", Symbolic ="\uE9B7"},
                new TriChar(){Name = "Drai", Skill="Dragonbreath - Finding and befriending her is not easy. But once she's decided she's on your side, there's nothing that can stop you to achieve you goal in Khaland", Symbolic ="\uE9F5"}
            };

            CharacterListView.ItemsSource = CharacterList;
        }

        private void GridDestination_DragOver(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            try
            {
                if (e.DataView?.Properties?.FirstOrDefault().Value?.GetType() == typeof(TriChar))
                {
                    e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
                    try
                    {
                        //Drag UI will be replaced with Drag Icon and Character's name that we are dragging
                        e.DragUIOverride.Caption = "\U0001F91A" + " " + ((TriChar)(e.DataView?.Properties?.FirstOrDefault().Value)).Name;
                        e.DragUIOverride.IsGlyphVisible = false;
                    }catch(Exception ex)
                    {return;}
                    return;
                }
            }
            catch
            {
                e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.None;
            }
        }

        private void GridDestination_DragDrop(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            var item = e.DataView.Properties.FirstOrDefault(x => x.Key == "item");
            if (item.Value?.GetType() == typeof(TriChar))
            {
                // Platform Grid will now show information about the character that you have dragged
                var droppedChar = (TriChar)item.Value;
                PlatformText.Text = droppedChar.Skill;
                PlatformIcon.Glyph = droppedChar.Symbolic;
                CharacterListView.SelectedItem = droppedChar;
            }
            else
                return;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called when the drag is starting. 
        ///             Character information will be 'copied'. </summary>
        ///
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Drag items starting event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void CharacterItem_DragStarting(object sender, DragItemsStartingEventArgs e)
        {
            e.Data.RequestedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
            if (e.Items != null && e.Items.Any())
            {
                e.Data.Properties.Add("item", e.Items.FirstOrDefault());
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Character class to make this tutorial project less boring. </summary>
    ///
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class TriChar
    {
        public string Name { get; set; } = null;
        public string Skill { get; set; } = null;
        public string Symbolic { get; set; } = null;
    }
}
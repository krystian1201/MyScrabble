
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using MyScrabble.Controller.Tiles;


namespace MyScrabble.View
{
    public partial class TileUC : UserControl
    {
        private Tile tile;

        public Tile Tile
        {
            get
            {
                return tile;
            }
        }

        public TileUC(Tile tile)
        {
            InitializeComponent();

            this.tile = tile;

            this.Content = tile.TileImage;

            this.MouseMove += TileUC_MouseMove;
        }

        private void TileUC_MouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            base.OnMouseMove(mouseEventArgs);

            if (mouseEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                // Package the data.
                DataObject data = new DataObject();
                
                data.SetData("TileUC", this);

                // Inititate the drag-and-drop operation.
                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            }
        }

        public void MakeTileUCNonDraggable()
        {
            this.MouseMove -= TileUC_MouseMove;
        }
    }
}


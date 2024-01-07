using chkam05.Tools.ControlsEx;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SystemManager.Data.Macros;

namespace SystemManager.Utilities
{
    public class ListViewItemExMoveHelper<T> where T : class
    {

        //  VARIABLES

        private ListViewEx _listViewEx;

        private int _draggedIndex;
        private bool _isDragging = false;
        private Point _startPoint;


        //  METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ListViewItemExMoveHelper class constructor. </summary>
        /// <param name="listViewEx"> ListViewEx. </param>
        public ListViewItemExMoveHelper(ListViewEx listViewEx)
        {
            _listViewEx = listViewEx;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Item drag enter method. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Drag Event Arguments. </param>
        public void ItemDragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Item drop method/ </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Drag Event Arguments. </param>
        /// <param name="itemsCollection"> Items collection. </param>
        public void ItemDrop(object sender, DragEventArgs e, ObservableCollection<T> itemsCollection)
        {
            int targetIndex = GetIndexUnderCursor(e.GetPosition(_listViewEx));

            if (targetIndex >= 0 && targetIndex != _draggedIndex)
            {
                itemsCollection.Move(_draggedIndex, targetIndex);
            }

            _isDragging = false;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Item preview mouse down method. </summary>
        /// <param name="frameworkElement"> Item framweork element. </param>
        /// <param name="e"> Mouse Button Event Arguments. </param>
        public void ItemPreviewMouseDown(FrameworkElement frameworkElement, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
            _draggedIndex = GetIndexUnderCursor(e.GetPosition(_listViewEx));
            _isDragging = false;

            frameworkElement.CaptureMouse();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Item preview mouse up method. </summary>
        /// <param name="frameworkElement"> Item framweork element. </param>
        /// <param name="e"> Mouse Button Event Arguments. </param>
        public void ItemPreviewMouseUp(FrameworkElement frameworkElement, MouseButtonEventArgs e)
        {
            frameworkElement.ReleaseMouseCapture();

            _isDragging = false;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Item preview mouse move method. </summary>
        /// <param name="frameworkElement"> Item framweork element. </param>
        /// <param name="e"> Mouse Button Event Arguments. </param>
        public void MacroItemHandlerPreviewMouseMove(FrameworkElement frameworkElement, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !_isDragging)
            {
                Point currentPosition = e.GetPosition(null);

                if (Math.Abs(currentPosition.X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(currentPosition.Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    frameworkElement.ReleaseMouseCapture();

                    var listViewItemEx = _listViewEx.ItemContainerGenerator.ContainerFromIndex(_draggedIndex) as ListViewItemEx;

                    StartDrag(listViewItemEx, currentPosition);
                }
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get item index under cursor position. </summary>
        /// <param name="cursorPosition"> Cursor position point. </param>
        /// <returns> Item index under cursor position. </returns>
        private int GetIndexUnderCursor(Point cursorPosition)
        {
            HitTestResult hitTestResult = VisualTreeHelper.HitTest(_listViewEx, cursorPosition);
            DependencyObject target = hitTestResult.VisualHit;

            while (target != null && !(target is ListViewItem))
            {
                target = VisualTreeHelper.GetParent(target);
            }

            var listViewItem = target as ListViewItem;
            return listViewItem != null ? _listViewEx.ItemContainerGenerator.IndexFromContainer(listViewItem) : -1;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Initialize item drag. </summary>
        /// <param name="listViewItem"> Dragged list view item. </param>
        /// <param name="currentPosition"> Current cursor position. </param>
        private void StartDrag(ListViewItem? listViewItem, Point currentPosition)
        {
            if (listViewItem == null)
                return;

            _isDragging = true;

            DataObject data = new DataObject(typeof(int), _draggedIndex);
            DragDrop.DoDragDrop(listViewItem, data, DragDropEffects.Move);
        }

    }
}

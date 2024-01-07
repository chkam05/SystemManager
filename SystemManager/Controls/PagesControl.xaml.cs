using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SystemManager.Pages;

namespace SystemManager.Controls
{
    public partial class PagesControl : UserControl
    {

        //  VARIABLES

        private List<Page> _pages;


        //  GETTERS & SETTERS

        public bool CanGoBack
        {
            get => _pages.Any() && CurrentPage != null && _pages.IndexOf(CurrentPage) > 0;
        }

        public Page? CurrentPage
        {
            get => contentFrame.Content as Page ?? _pages.LastOrDefault();
        }

        public int PagesCount
        {
            get => _pages.Count;
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> PagesControl class constructor. </summary>
        public PagesControl()
        {
            //  Initialize data.
            _pages = new List<Page>();

            //  Initialize user interface.
            InitializeComponent();
        }

        #endregion CLASS METHODS

        #region CONTENT FRAME METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Remove all loaded pages from history. </summary>
        private void ClearAllContents()
        {
            _pages.Clear();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Remove currently loaded page from content frame. </summary>
        private void ClearCurrentContent()
        {
            contentFrame.Content = null;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after loading page in frame. </summary>
        /// <param name="sender"> Object that invoked method. </param>
        /// <param name="e"> Navigation Event Arguments. </param>
        private void ContentFrameNavigated(object sender, NavigationEventArgs e)
        {
            RemoveBackEntry();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Remove previous pages from content frame. </summary>
        private void RemoveBackEntry()
        {
            var backEntry = contentFrame.NavigationService.RemoveBackEntry();

            if (backEntry != null)
                RemoveBackEntry();
        }

        #endregion CONTENT FRAME METHODS

        #region NAVIGATION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Load previous page from history. </summary>
        /// <param name="backCount"> Amount of pages to go back (default 1). </param>
        public void GoBack(int backCount = 1)
        {
            if (CanGoBack && CurrentPage != null)
            {
                var currentPage = CurrentPage;
                var currentPageIndex = _pages.IndexOf(currentPage);
                var destPageIndex = Math.Max(0, currentPageIndex - backCount);
                var nextPageIndex = destPageIndex + 1;

                _pages.RemoveRange(nextPageIndex, PagesCount - (nextPageIndex));

                var destPage = _pages[destPageIndex];

                contentFrame.Navigate(destPage);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Load page if page does not exists, otherwise load last page of this type. </summary>
        /// <typeparam name="T"> Page type. </typeparam>
        /// <param name="page"> Page. </param>
        public void LoadPageOrGetLast<T>(T page) where T : Page
        {
            if (HasPageType(typeof(T)))
            {
                var idxs = GetPageIndexesByPageType(typeof(T));
                GoBack((PagesCount - 1) - idxs.Last());
            }
            else
            {
                LoadPage(page);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Load page. </summary>
        /// <param name="page"> Page to load. </param>
        public void LoadPage(Page page)
        {
            _pages.Add(page);
            contentFrame.Navigate(page);
        }

        #endregion NAVIGATION METHODS

        #region PAGES MANAGEMENT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Get page index. </summary>
        /// <param name="page"> Page instance. </param>
        /// <returns> Index of page, or -1. </returns>
        public int GetPageIndex(Page page)
        {
            if (HasPage(page))
                return _pages.IndexOf(page);

            return -1;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get indexes of pages of a given type. </summary>
        /// <param name="pageType"> Page type. </param>
        /// <returns> Array of pages indexes. </returns>
        public int[] GetPageIndexesByPageType(Type pageType)
        {
            if (HasPageType(pageType))
                return _pages.Where(p => pageType.IsAssignableFrom(((Page)p).GetType()))
                    .Select(p => _pages.IndexOf(p))
                    .ToArray();

            return new int[0];
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Check if page has been loaded previously. </summary>
        /// <param name="page"> Page instance. </param>
        /// <returns> True - page has been loaded previously; False - otherwise. </returns>
        public bool HasPage(Page page)
        {
            return _pages.Contains(page);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Check if page with given type has been loaded previously. </summary>
        /// <param name="pageType"> Page type. </param>
        /// <returns> True - page with given type has been loaded previosuly; False - otherwise. </returns>
        public bool HasPageType(Type pageType)
        {
            return _pages.Any(p => pageType.IsAssignableFrom(((Page)p).GetType()));
        }

        #endregion PAGES MANAGEMENT METHODS

    }
}

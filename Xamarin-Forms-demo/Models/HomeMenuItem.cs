using Xamarin.Forms;
using Xamarin_Forms_demo.Views;

namespace Xamarin_Forms_demo.Models
{
    public enum MenuItemType
    {
        Browse,
        About,
        Subjects,
        Canvas,
        StudyTabbed,
        Posts,
    }

    public class HomeMenuItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public static Page GetPageById(int id)
        {
            Page page;
            switch (id)
            {
                case (int)MenuItemType.Posts:
                    page = new PostsContentPage();
                    break;
                case (int)MenuItemType.Browse:
                    page = new ItemsPage();
                    break;
                case (int)MenuItemType.About:
                    page = new AboutPage();
                    break;
                case (int)MenuItemType.Canvas:
                    page = new CanvasPage();
                    break;
                case (int)MenuItemType.Subjects:
                    page = new SubjectsPage();
                    break;
                case (int)MenuItemType.StudyTabbed:
                    page = new StudyTabbedPage();
                    break;
                default:
                    throw new System.Exception("Nav Bar Is Fucked");
            }
            return page;
        }
    }
}

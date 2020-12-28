using Xamarin.Forms;
using Xamarin_Forms_demo.Views;

namespace Xamarin_Forms_demo.Models
{
    public enum MenuItemType
    {
        Index,
        About,
        Subjects,
        Canvas,
        StudyTabbed,
        SNSTabbed,
        Posts,
        Audio,
    }

    public class HomeMenuItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public static Page GetPageById(int id)
        {
            Page page = id switch
            {
                (int)MenuItemType.Audio => new AudioPage(),
                (int)MenuItemType.Posts => new PostsPage(),
                (int)MenuItemType.About => new AboutPage(),
                (int)MenuItemType.Canvas => new CanvasPage(),
                (int)MenuItemType.Subjects => new SubjectsPage(),
                (int)MenuItemType.StudyTabbed => new StudyTabbedPage(),
                (int)MenuItemType.SNSTabbed => new SNSTabbedPage(),
                _ => throw new System.Exception("Nav Bar Is Fucked"),
            };
            return page;
        }
    }
}

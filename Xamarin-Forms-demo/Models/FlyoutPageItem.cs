using Xamarin.Forms;
using Xamarin_Forms_demo.Views;

namespace Xamarin_Forms_demo.Models
{
    public enum MenuItemType
    {
        //About,
        //Subjects,
        //Canvas,
        //Posts,
        //Audio,
        Index,
        StudyTabbed,
        SNSTabbed,
    }

    public class FlyoutPageItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string IconSource { get; set; }

        public static Page GetPageById(int id)
        {
            Page page = id switch
            {
                //(int)MenuItemType.Audio => new AudioPage(),
                //(int)MenuItemType.Posts => new PostsPage(),
                //(int)MenuItemType.About => new AboutPage(),
                //(int)MenuItemType.Canvas => new CanvasPage(),
                //(int)MenuItemType.Subjects => new SubjectsPage(),
                (int)MenuItemType.StudyTabbed => new StudyTabbedPage(),
                (int)MenuItemType.SNSTabbed => new SNSTabbedPage(),
                _ => throw new System.Exception("Nav Bar Is Fucked"),
            };
            return page;
        }
    }
}

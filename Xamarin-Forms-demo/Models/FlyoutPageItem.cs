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

        public static Page GetPageById(MenuItemType id)
        {
            Page page = id switch
            {
                //MenuItemType.Audio => new AudioPage(),
                //MenuItemType.Posts => new PostsPage(),
                //MenuItemType.About => new AboutPage(),
                //MenuItemType.Canvas => new CanvasPage(),
                //MenuItemType.Subjects => new SubjectsPage(),
                MenuItemType.StudyTabbed => new StudyTabbedPage(),
                MenuItemType.SNSTabbed => new SNSTabbedPage(),
                _ => throw new System.Exception("Nav Bar Is Fucked"),
            };
            return page;
        }
    }
}

namespace Xamarin_Forms_demo.Models
{
    public enum MenuItemType
    {
        Browse,
        About,
        Subjects,
        Canvas,
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}

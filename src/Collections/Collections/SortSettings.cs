namespace Armsoft.Collections
{
    public class SortSettings
    {
        public string PropertyName { get; set; }
        public bool SortAscending { get; set; } = true;
        public SortSettings ThenBy { get; set; }
    }
}
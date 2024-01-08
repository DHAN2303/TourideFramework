namespace Touride.Framework.Data.Entities
{
    public class PagedRequest
    {
        public PagedRequest()
        {
            SorguParametreleri = new List<SorguParametre>();
            Grouping = new Grouping();
            Paginator = new Paginator();
            Sorting = new Sorting();
        }
        public Paginator Paginator { get; set; }
        public Sorting Sorting { get; set; }
        public Grouping Grouping { get; set; }
        public List<SorguParametre> SorguParametreleri { get; set; }
        public object Filter { get; set; }
        public string? SearchTerm { get; set; }

    }

    public class Paginator
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public List<int>? PageSizes { get; set; }
        public int? Total { get; set; }



    }

    public class SorguParametre
    {
        public string? Name { get; set; }  //adi
        public string? Value { get; set; }  //ahmet
        public string? Parametre { get; set; } // ==
    }

    /*
    {
        "Name" : "cahit",   
        "Value" : "adi",
        "Parametre": "=="   
    },
    {
        "Name" : "cahit",
        "Value" : "soyadı",
        "Parametre": "!="   
    }
     * 
     * */

    //

    public class Grouping
    {
        public List<int>? ItemIds { get; set; }
        public object? SelectedRowIds { get; set; }
    }

    public class Sorting
    {
        public string? Column { get; set; }
        public string? Direction { get; set; }
    }


}

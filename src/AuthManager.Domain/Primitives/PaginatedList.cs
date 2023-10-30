namespace AuthManager.Domain.Primitives;

public class PaginatedList<T>
{
    public PaginatedList(
        IEnumerable<T> items,
        int page,
        int pageSize,
        int totalRecords) 
    {
        Page = Math.Max(page, 1);
        PageSize = pageSize >= items.Count() ? pageSize : items.Count();
        Records = new List<T>(items);
        TotalRecords = totalRecords;
    }

    /// <summary>
    /// The current page returned.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// The number of records returned.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// The list containing the records.
    /// </summary>
    public List<T> Records { get; set; }

    /// <summary>
    /// The number of pages returned.
    /// </summary>
    public int TotalPages => Math.Max((int) Math.Ceiling((double)TotalRecords / PageSize), 1);

    /// <summary>
    /// The number of records available.
    /// </summary>
    public int TotalRecords { get; set; }

    public void Add(T item)
    {
        Records.Add(item);

        if (Records.Count > PageSize)
        {
            PageSize = Records.Count;
        }

        if (Records.Count > TotalRecords)
        {
            TotalRecords = Records.Count;
        }
    }

    public void AddRange(IEnumerable<T> items)
    {
        Records.AddRange(items);

        if (Records.Count > PageSize)
        {
            PageSize = Records.Count;
        }

        if (Records.Count > TotalRecords)
        {
            TotalRecords = Records.Count;
        }
    }

    public void Insert(int index, T item) 
    {
        Records.Insert(index, item);

        if (Records.Count > PageSize)
        {
            PageSize = Records.Count;
        }

        if (Records.Count > TotalRecords)
        {
            TotalRecords = Records.Count;
        }
    }

    public void InsertRange(int index, IEnumerable<T> items)
    {  
        Records.InsertRange(index, items);

        if (Records.Count > PageSize)
        {
            PageSize = Records.Count;
        }

        if (Records.Count > TotalRecords) 
        {
            TotalRecords = Records.Count;
        }
    }
}

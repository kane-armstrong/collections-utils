namespace Armsoft.Collections;

public class SortSettings
{
    /// <summary>
    ///     This property can be used to signal that a table alias should be prepended to the property name.
    ///     This is in the format [<see cref="PropertyTableAlias"/>].[<see cref="PropertyName"/>]'. This can help when
    ///     sorting by a column whose name appears in more than one table in a JOIN.
    /// </summary>
    public string PropertyTableAlias { get; set; }
    public string PropertyName { get; set; }
    public bool SortAscending { get; set; } = true;
    public SortSettings ThenBy { get; set; }
}

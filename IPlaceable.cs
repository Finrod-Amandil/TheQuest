namespace TheQuest
{
    interface IPlaceable
    {
        string Name { get; set; }
        Field Field { get; set; }
        bool IsSpawned { get; set; }
    }
}

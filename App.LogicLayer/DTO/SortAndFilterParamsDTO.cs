namespace App.LogicLayer.DTO
{
    public class SortAndFilterParamsDTO
    {
        public string SortProperty { get; set; }
        public string DateStartFromFilter { get; set; }
        public string DateStartToFilter { get; set; }
        public string PriorityFilter { get; set; }
        public string ManagerFilter { get; set; }
    }
}

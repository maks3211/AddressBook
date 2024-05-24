namespace AddressBook.Models
{
    public interface IEntityOperations<T>
    {
        public static abstract bool RemoveById(int id, List<T> list);
        public static abstract int GetIndex(int id, List<T> list);
        public static abstract List<int> GetIndexesOf(string name, string by, List<T> list);
       


    }
}

namespace CrudRestApi.Repositories;

public class InMemoryRepository<T> : IRepository<T> where T : class
{
    // In-memory list that acts as our database
    private readonly List<T> _items = new();

    // Function that knows how to read the Id from any entity T
    private readonly Func<T, int> _getId;

    // Function that knows how to set the Id on any entity T
    private readonly Action<T, int> _setId;

    // Counter that auto-increments to generate unique Ids
    private int _nextId = 1;

    // We receive the Id functions from outside because T has no guaranteed Id property
    public InMemoryRepository(Func<T, int> getId, Action<T, int> setId)
    {
        _getId = getId;
        _setId = setId;
    }

    // Returns all items in memory
    public IEnumerable<T> GetAll() => _items;

    // Finds an item by Id, returns null if not found
    public T? GetById(int id) =>
        _items.FirstOrDefault(item => _getId(item) == id);

    // Assigns a new Id to the entity, adds it to the list and returns it
    public T Add(T entity)
    {
        _setId(entity, _nextId++);
        _items.Add(entity);
        return entity;
    }

    // Replaces the existing entity with the updated one, returns null if not found
    public T? Update(int id, T entity)
    {
        var index = _items.FindIndex(item => _getId(item) == id);
        if (index == -1) return null;
        _setId(entity, id);
        _items[index] = entity;
        return entity;
    }

    // Removes the entity with the given Id, returns false if not found
    public bool Delete(int id)
    {
        var item = GetById(id);
        if (item == null) return false;
        _items.Remove(item);
        return true;
    }
}
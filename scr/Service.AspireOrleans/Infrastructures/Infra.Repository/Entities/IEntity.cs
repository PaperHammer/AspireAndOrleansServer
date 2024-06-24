namespace Infra.Repository.Entities
{
    public interface IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}

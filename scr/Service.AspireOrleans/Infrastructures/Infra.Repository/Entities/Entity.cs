namespace Infra.Repository.Entities
{
    public class Entity : IEntity<long>
    {
        public long Id { get; set; }
    }
}

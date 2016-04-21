using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository.Entity
{
    public class CategoryRepository : CRUDRepository<AGRO_CATEGORY, Entities>, ICategoryRepository
    {
        public CategoryRepository(Entities entities) : base(entities)
        {
        }
    }
}
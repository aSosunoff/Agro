using Model.Engine.Repository.Interface;

namespace Model.Engine.Service.Interface
{
    public interface ICCategoryService : IBaseService<ICCategoryRepository>
    {
        void Create(ccategory item);
        /// <summary>
        /// Виртуальное удаление категории. Удаляет категорию вместе с подкатегориями
        /// </summary>
        /// <param name="id">ID категории</param>
        void VirtualDeleteTreeDown(int id);
        /// <summary>
        /// Виртуальное восстановление удалённых ранее категорий
        /// </summary>
        /// <param name="id"></param>
        void VirtualRecoverTreeDown(int id);
    }
}
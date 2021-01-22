using System;
using System.Threading.Tasks;
using forte.models.classes;

namespace forte.services
{
    public interface IClassMetadataService
    {
        /// <summary>
        ///     Create new equipment record
        /// </summary>
        /// <param name="classEquipment"></param>
        /// <returns></returns>
        Task<ClassEquipmentExModel> CreateClassEquipmentAsync(ClassEquipmentModel classEquipment);

        /// <summary>
        ///     Gets available class equipment.
        /// </summary>
        /// <param name="skip">The number of items to skip.</param>
        /// <param name="take">The number of items to take.</param>
        /// <param name="extended"></param>
        /// <returns>The paged list of the class equipment.</returns>
        Task<ClassEquipmentPageModel> GetClassEquipmentListAsync(int skip, int take, bool extended);

        /// <summary>
        ///     Gets available class equipment.
        /// </summary>
        /// <returns>The paged list of the class equipment.</returns>
        Task<ClassEquipmentModel> GetClassEquipmentAsync(Guid id, bool extended);

        /// <summary>
        ///     Create new equipment record
        /// </summary>
        /// <param name="classEquipment"></param>
        /// <returns></returns>
        Task<ClassEquipmentExModel> UpdateClassEquipmentAsync(ClassEquipmentExModel classEquipment);

        /// <summary>
        ///     Create new equipment record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteClassEquipmentAsync(Guid id);

        Task DeleteClassCategoryAsync(Guid categoryId);

        Task<ClassCategoryExModel> UpdateClassCategoryAsync(ClassCategoryExModel classCategory);

        Task<ClassCategoryExModel> CreateClassCategoryAsync(ClassCategoryExModel classCategory);

        /// <summary>
        ///     Gets available class categories.
        /// </summary>
        /// <param name="skip">The number of items to skip.</param>
        /// <param name="take">The number of items to take.</param>
        /// <param name="extended"></param>
        /// <returns>The paged list of the class categories.</returns>
        ClassCategoryPageModel GetClassCategories(int skip, int take, bool extended, bool hasVideoUrl);

        /// <summary>
        ///     Gets available class types.
        /// </summary>
        /// <param name="live">Return only class types that have live sessions associated.</param>
        /// <param name="scheduled">Return only class types that have scheduled sessions associated.</param>
        /// <param name="ondemand">Return only class types that have ondemand sessions associated.</param>
        /// <param name="skip">The number of items to skip.</param>
        /// <param name="take">The number of items to take.</param>
        /// <param name="extended"></param>
        /// <param name="userClassTypes">Filter by user class types</param>
        /// <returns>The paged list of class types.</returns>
        Task<ClassTypePageModel> GetClassTypes(bool live, bool scheduled, bool ondemand, int skip, int take, bool extended, bool userClassTypes);

        /// <summary>
        ///     Create new class type
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        Task<ClassTypeExModel> CreateClassTypeAsync(ClassTypeExModel classType);

        /// <summary>
        ///     Update existing class type
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        Task<ClassTypeExModel> UpdateClassTypeAsync(ClassTypeExModel classType);

        /// <summary>
        ///     Delete existing class type
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        Task DeleteClassTypeAsync(Guid typeId);
    }
}

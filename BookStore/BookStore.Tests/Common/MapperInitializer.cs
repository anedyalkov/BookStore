
using BookStore.Domain;
using BookStore.Models;
using BookStore.Services.Admin.Models.Users;
using BookStore.Services.Mapping;
using System.Reflection;

namespace BookStore.Tests.Common
{
    public static class MapperInitializer
    {
        public static void InitializeMapper()
        {
            AutoMapperConfig.RegisterMappings(
             typeof(ErrorViewModel).GetTypeInfo().Assembly,
             typeof(AdminUserListingServiceModel).GetTypeInfo().Assembly,
             typeof(Book).GetTypeInfo().Assembly);
        }
    }
}

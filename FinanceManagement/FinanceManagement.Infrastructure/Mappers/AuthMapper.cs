using FinanceManagement.Infrastructure.Dto.Auth;
using FinanceManagement.Infrastructure.Models.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Mappers
{
    public static class AuthMapper
    {
        public static User ToUser(this RegisterRequest request)
        {
            return new User
            {
                Email = request.Email,
                Name = request.Name,
                Password = request.Password
            };
        }
    }
}

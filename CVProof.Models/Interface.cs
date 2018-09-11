using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CVProof.Models
{
    public interface IUserMgr
    {
        String User { get; set; }
        Boolean IsAuthenticated { get; set; }

        Task<Boolean> LoginAsync(String hash);
        Task LogoutAsync();

        //Task RenewTokenAsync(String jwtToken);        
    }
}

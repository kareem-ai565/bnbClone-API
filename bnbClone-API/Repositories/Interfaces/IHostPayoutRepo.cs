﻿using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface IHostPayoutRepo : IGenericRepo<HostPayout>
    {
        Task<IEnumerable<HostPayout>> GetByHostIdAsync(int hostId);
    }
}

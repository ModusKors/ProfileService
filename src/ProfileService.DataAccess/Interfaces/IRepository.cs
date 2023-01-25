﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileService.DataAccess.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task <T> Get(int id, CancellationToken cancellationToken);
        Task Update(T item, CancellationToken cancellationToken);
    }
}

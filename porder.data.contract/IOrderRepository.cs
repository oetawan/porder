﻿using porder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace porder.data.contract
{
    public interface IOrderRepository
    {
        void Add(Order order);
    }
}
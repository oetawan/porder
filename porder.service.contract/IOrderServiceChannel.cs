﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
namespace porder.service.contract
{
    public interface IOrderServiceChannel: IOrderService, IClientChannel
    {
         
    }
}
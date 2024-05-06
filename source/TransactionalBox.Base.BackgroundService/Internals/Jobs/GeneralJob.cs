﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionalBox.Base.BackgroundService.Internals.Jobs
{
    public abstract class GeneralJob
    {
        protected internal abstract Task Execute(CancellationToken stoppingToken);
    }
}
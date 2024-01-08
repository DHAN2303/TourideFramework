using System;
using System.Transactions;
using Touride.Framework.Abstractions.Data.Enums;
using Touride.Framework.Abstractions.Ioc;

namespace Touride.Framework.Abstractions.Data.TransactionManagement
{
    /// <summary>
    /// Transaction kullanılması gereken metodlara attribute olarak eklenecek
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TransactionalAttribute : InterceptionAttribute
    {
        public AllowedIsolationLevel AllowedIsolationLevel { get; set; }

        public TransactionScopeOption TransactionScopeOption { get; set; }

        public TransactionalAttribute(TransactionScopeOption scopeOption = TransactionScopeOption.Required,
                                            AllowedIsolationLevel isolationLevel = AllowedIsolationLevel.ReadCommitted)
        {
            TransactionScopeOption = scopeOption;
            AllowedIsolationLevel = isolationLevel;
        }
    }
}

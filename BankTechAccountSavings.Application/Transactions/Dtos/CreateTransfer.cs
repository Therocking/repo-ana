﻿using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.Transactions.Dtos
{
    public class CreateTransfer
    {
        public Guid SourceProductId { get; set; }
        public Guid DestinationProductId { get; set; }
        public TransferType TransferType { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
    }
}

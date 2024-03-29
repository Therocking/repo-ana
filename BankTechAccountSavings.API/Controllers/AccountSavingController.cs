﻿using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Application.AccountSavings.Interfaces;
using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using BankTechAccountSavings.Domain.Entities;

namespace BankTechAccountSavings.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountSavingController(IAccountSavingService accountSavingService, IValidator<CreateAccountSaving> createAccountSavingValidator, IValidator<UpdateAccountSaving> updateAccountSavingValidator, IValidator<CreateDeposit> createDepositValidator, IValidator<CreateDepositByAccountNumber> createDepositByAccountNumberValidator, IValidator<CreateWithdraw> createWithdrawValidator, IValidator<CreateWithdrawByAccountNumber> createWithdrawByAccountNumberValidator, IValidator<CreateTransfer> createTransferValidator, IValidator<CreateTransferByAccountNumber> createTransferByAccountNumberValidator) : ControllerBase
    {
        private readonly IAccountSavingService _accountService = accountSavingService;
        private readonly IValidator<CreateAccountSaving> _createAccountSavingValidator = createAccountSavingValidator;
        private readonly IValidator<UpdateAccountSaving> _updateAccountSavingValidator = updateAccountSavingValidator;
        private readonly IValidator<CreateDeposit> _createDeposit = createDepositValidator;
        private readonly IValidator<CreateWithdraw> _createWithdraw = createWithdrawValidator;
        private readonly IValidator<CreateTransfer> _createTransfer = createTransferValidator;
        private readonly IValidator<CreateDepositByAccountNumber> _createDepositByAccountNumber = createDepositByAccountNumberValidator;
        private readonly IValidator<CreateWithdrawByAccountNumber> _createWithdrawByAccountNumber = createWithdrawByAccountNumberValidator;
        private readonly IValidator<CreateTransferByAccountNumber> _createTransferByAccountNumber = createTransferByAccountNumberValidator;

        [HttpGet()]
        public async Task<ActionResult<List<GetAccountSaving>>> GetAccounts()
        {
            try
            {
                List<GetAccountSaving> accountSavings = await _accountService.GetAllAccountsAsync();

                if (accountSavings == null || accountSavings.Count == 0)
                {
                    return NotFound(_accountService.FormatErrorResponse("No Accounts were found"));
                }

                return Ok(accountSavings);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }
        }

        [HttpGet("{clientId:int}/paginated")]
        public async Task<ActionResult<Paginated<GetAccountSaving>>> GetPaginatedAccounts(int clientId ,int page, int pageSize)
        {
            try
            {
                Paginated<GetAccountSaving> paginatedResult = await _accountService.GetPaginatedAccountsAsync(clientId, page, pageSize);

                if (paginatedResult.Items == null)
                {
                    return NotFound(_accountService.FormatErrorResponse("No Accounts were found"));
                }

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }
        }

        [HttpGet("client/{clientId:int}/paginated/transactions")]
        public async Task<ActionResult<Paginated<GetTransaction>>> GetClientPaginatedTransactions(int clientId, int page, int pageSize)
        {
            try
            {
                Paginated<GetTransaction> paginatedResult = await _accountService.GetPaginatedTransactionsByAccountAsync(clientId, page, pageSize);

                if (paginatedResult.Items == null)
                {
                    return NotFound(_accountService.FormatErrorResponse($"No Transactions were found for the client"));
                }

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }
        }

        [HttpGet("{accountNumber:long}/paginated/transactions")]
        public async Task<ActionResult<Paginated<GetTransaction>>> GetPaginatedTransactions(long accountNumber, int page, int pageSize)
        {
            try
            {
                Paginated<GetTransaction> paginatedResult = await _accountService.GetPaginatedTransactionsByAccountNumberAsync(accountNumber, page, pageSize);

                if (paginatedResult.Items == null)
                {
                    return NotFound(_accountService.FormatErrorResponse($"No Transactions were found for the account {accountNumber}"));
                }

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }
        }

        [HttpGet("{accountId:Guid}")]
        public async Task<ActionResult<GetAccountSaving>> GetAccountById(Guid accountId)
        {
            try
            {
                GetAccountSaving? account = await _accountService.GetAccountSavingByIdAsync(accountId);

                if (account == null)
                {
                    return NotFound(_accountService.FormatErrorResponse("No Account found"));
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }
        }

        [HttpGet("account-number/{accountNumber:long}")]
        public async Task<ActionResult<GetAccountSaving>> GetAccountByAccountNumber(long accountNumber)
        {
            try
            {
                GetAccountSaving? account = await _accountService.GetAccountSavingByAccountNumberAsync(accountNumber);

                if (account == null)
                {
                    return NotFound(_accountService.FormatErrorResponse("No Account found"));
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }

        }

        [HttpGet("{accountId:Guid}/transactions")]
        public async Task<ActionResult<List<GetTransaction>>> GetTransactionHistory(Guid accountId)
        {
            try
            {
                List<GetTransaction>? transactions = await _accountService.GetTransactionsHistory(accountId);

                if (transactions == null || transactions.Count == 0)
                {
                    return NotFound(_accountService.FormatErrorResponse("No Transactions found"));
                }

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }
        }

        [HttpGet("{accountNumber:long}/transactions")]
        public async Task<ActionResult<List<GetTransaction>>> GetTransactionHistory(long accountNumber)
        {
            try
            {
                List<GetTransaction>? transactions = await _accountService.GetTransactionsHistoryByAccountNumber(accountNumber);

                if (transactions == null || transactions.Count == 0)
                {
                    return NotFound(_accountService.FormatErrorResponse("No Transactions found"));
                }

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }
        }


        [HttpPost("{accountId:Guid}/deposit")]
        public async Task<ActionResult<GetDeposit>?> AddDeposit(int amount, Guid accountId, string? description)
        {
            var createDeposit = new CreateDeposit
            {
                Amount = amount,
                DestinationProductId = accountId,
                Description = description!
            };
            try
            {
                ActionResult? validationResult = await ValidateAndReturnResultAsync(createDeposit, _createDeposit);
                if (validationResult != null)
                {
                    return validationResult;
                }
                GetDeposit? deposit = await _accountService.AddDepositAsync(amount, accountId, description!);

                if (deposit == null)
                {
                    return NotFound(_accountService.FormatErrorResponse("Deposit failed"));
                }

                return Ok(deposit);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }

        }

        [HttpPost("{accountNumber:long}/deposit")]
        public async Task<ActionResult<GetDepositByAccountNumber>?> AddDeposit(int amount, long accountNumber, string? description)
        {
            var createDeposit = new CreateDepositByAccountNumber
            {
                Amount = amount,
                DestinationProductNumber = accountNumber,
                Description = description!
            };
            try
            {
                ActionResult? validationResult = await ValidateAndReturnResultAsync(createDeposit, _createDepositByAccountNumber);
                if (validationResult != null)
                {
                    return validationResult;
                }
                GetDepositByAccountNumber? deposit = await _accountService.AddDepositByAccountNumberAsync(amount, accountNumber, description!);

                if (deposit == null)
                {
                    return NotFound(_accountService.FormatErrorResponse("Deposit failed"));
                }

                return Ok(deposit);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }
        }

        [HttpPost("{accountId:Guid}/withdraw")]
        public async Task<ActionResult<GetWithdraw>?> AddWithDraw(int amount, Guid accountId)
        {
            var createWithdraw = new CreateWithdraw
            {
                SourceProductId = accountId,
                Amount = amount,
            };
            try
            {
                ActionResult? validationResult = await ValidateAndReturnResultAsync(createWithdraw, _createWithdraw);
                if (validationResult != null)
                {
                    return validationResult;
                }
                GetWithdraw? withdraw = await _accountService.WithDrawAsync(amount, accountId);

                if (withdraw == null)
                {
                    return NotFound(_accountService.FormatErrorResponse("WithDraw failed"));
                }

                return Ok(withdraw);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }

        }

        [HttpPost("{accountNumber:long}/withdraw")]
        public async Task<ActionResult<GetWithdrawByAccountNumber>?> AddWithDraw(int amount, long accountNumber)
        {
            var createWithdraw = new CreateWithdrawByAccountNumber
            {
                SourceProductNumber = accountNumber,
                Amount = amount,
            };
            try
            {
                ActionResult? validationResult = await ValidateAndReturnResultAsync(createWithdraw, _createWithdrawByAccountNumber);
                if (validationResult != null)
                {
                    return validationResult;
                }
                GetWithdrawByAccountNumber? withdraw = await _accountService.WithDrawByAccountNumberAsync(amount, accountNumber);

                if (withdraw == null)
                {
                    return NotFound(_accountService.FormatErrorResponse("WithDraw failed"));
                }

                return Ok(withdraw);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }
        }

        [HttpPost("{fromAccountId:Guid}/transfer/{toAccountId:Guid}")]
        public async Task<ActionResult<GetTransfer>?> TransferFunds(Guid fromAccountId, Guid toAccountId, string? description, int transferAmount, TransferType transactionType)
        {
            var createTransfer = new CreateTransfer
            {
                SourceProductId = fromAccountId,
                DestinationProductId = toAccountId,
                Description = description!,
                Amount = transferAmount,
                TransferType = transactionType

            };
            try
            {
                ActionResult? validationResult = await ValidateAndReturnResultAsync(createTransfer, _createTransfer);
                if (validationResult != null)
                {
                    return validationResult;
                }
                GetTransfer? transaction = await _accountService.TransferFunds(fromAccountId, toAccountId, description!, transferAmount, transactionType);

                if (transaction == null)
                {
                    return NotFound(_accountService.FormatErrorResponse("Transaction failed"));
                }

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }

        }

        [HttpPost("{fromAccountNumber:long}/transfer/{toAccountNumber:long}")]
        public async Task<ActionResult<GetTransferByAccountNumber>?> TransferFunds(long fromAccountNumber, long toAccountNumber, string? description, int transferAmount, TransferType transactionType)
        {
            var createTransfer = new CreateTransferByAccountNumber
            {
                SourceProductNumber = fromAccountNumber,
                DestinationProductNumber = toAccountNumber,
                Description = description!,
                Amount = transferAmount,
                TransferType = transactionType

            };
            try
            {
                ActionResult? validationResult = await ValidateAndReturnResultAsync(createTransfer, _createTransferByAccountNumber);
                if (validationResult != null)
                {
                    return validationResult;
                }
                GetTransferByAccountNumber? transaction = await _accountService.TransferFundsByAccountNumberAsync(fromAccountNumber, toAccountNumber, description!, transferAmount, transactionType);

                if (transaction == null)
                {
                    return NotFound("Transaction failed.");
                }

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        public async Task<ActionResult<CreatedAccountSavingResponse>?> CreateAccount(CreateAccountSaving createAccount)
        {
            try
            {
                ActionResult? validationResult = await ValidateAndReturnResultAsync(createAccount, _createAccountSavingValidator);
                if (validationResult != null)
                {
                    return validationResult;
                }

                CreatedAccountSavingResponse? account = await _accountService.CreateAccountSavingAsync(createAccount);

                if (account == null)
                {
                    return NotFound(_accountService.FormatErrorResponse("Failed to create the account"));
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }

        }

        [HttpPut("{accountId:Guid}")]
        public async Task<ActionResult<UpdatedAccountSavingResponse>?> UpdateAccount(Guid accountId, UpdateAccountSaving updateAccountSaving)
        {
            try
            {
                ActionResult? validationResult = await ValidateAndReturnResultAsync(updateAccountSaving, _updateAccountSavingValidator);
                if (validationResult != null)
                {
                    return validationResult;
                }
                UpdatedAccountSavingResponse? account = await _accountService.UpdateAccountSavingAsync(accountId, updateAccountSaving);

                if (account == null)
                {
                    return NotFound(_accountService.FormatErrorResponse("Failed to update the account"));
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }

        }

        [HttpPatch("{accountId:Guid}/close")]
        public async Task<ActionResult<DeletedAccountSavingResponse>?> CloseAccount(Guid accountId, string reasonToCloseAccount)
        {
            try
            {
                DeletedAccountSavingResponse? account = await _accountService.DeleteAccountSavingAsync(accountId, reasonToCloseAccount);

                if (account == null)
                {
                    return NotFound(_accountService.FormatErrorResponse("Failed to close the account"));
                }

                return Ok("Account is closed");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }

        }

        private static async Task<ActionResult?> ValidateAndReturnResultAsync<T>(T model, IValidator<T> validator)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (validationResult.IsValid)
            {
                return null;
            }

            var firstErrorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage;
            if (!string.IsNullOrEmpty(firstErrorMessage))
            {
                var formattedResult = $"\"errorMessage\": \"{firstErrorMessage}\"";
                return new BadRequestObjectResult(formattedResult);
            }
            return new BadRequestObjectResult(firstErrorMessage);
        }
    }
}

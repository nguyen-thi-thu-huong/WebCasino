﻿using System.Collections.Generic;
using System.Threading.Tasks;
using WebCasino.Entities;

namespace WebCasino.Service.Abstract
{
	public interface ITransactionService
    {
		Task<IEnumerable<Transaction>> GetAllTransactions();

		Task<IEnumerable<Transaction>> GetUserTransactions(string userId);

		Task<IEnumerable<Transaction>> GetTransactionByType(string transactionTypeName);
 		
		//TODO: Withdraw,Deposit
		Task<Transaction> AddWinTransaction(string userId, double originalAmount, int transactionTypeId, string description);
		Task<Transaction> AddStakeTransaction(string userId, double originalAmount, int transactionTypeId, string description);
		Task<Transaction> AddWithdrawTransaction(string userId, double originalAmount, int transactionTypeId, string description);
		Task<Transaction> AddDepositTransaction(string userId, double originalAmount, int transactionTypeId, string description);

	}
}

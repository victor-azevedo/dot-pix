using DotPix.Exceptions;
using DotPix.Models;
using DotPix.Models.Dtos;
using DotPix.Repositories;

namespace DotPix.Services;

public class PaymentProviderAccountService(PaymentProviderAccountRepository paymentProviderAccountRepository)
{
    public async Task<PaymentProviderAccount> FindByUserAndPspIdAndAccountOrError(
        User user, int paymentProviderId, PostAccountDto account)
    {
        var userAccount = await paymentProviderAccountRepository
            .FindByUserAndAccount(user, paymentProviderId, account);

        if (userAccount == null)
            throw new AccountNotFoundException();

        return userAccount;
    }
}
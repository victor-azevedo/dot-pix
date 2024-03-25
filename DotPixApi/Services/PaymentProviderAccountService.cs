using DotPixApi.Exceptions;
using DotPixApi.Models;
using DotPixApi.Models.Dtos;
using DotPixApi.Repositories;

namespace DotPixApi.Services;

public class PaymentProviderAccountService(PaymentProviderAccountRepository paymentProviderAccountRepository)
{
    public async Task<PaymentProviderAccount> FindByUserAndPspIdAndAccountOrThrow(
        User user, int paymentProviderId, InAccountDto account)
    {
        var userAccount = await paymentProviderAccountRepository
            .FindByUserAndAccount(user, paymentProviderId, account);

        if (userAccount == null)
            throw new AccountNotFoundException();

        return userAccount;
    }
}
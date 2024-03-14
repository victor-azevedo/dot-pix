using DotPixApi.Exceptions;
using DotPixApi.Models;
using DotPixApi.Models.Dtos;
using DotPixApi.Repositories;

namespace DotPixApi.Services;

public class PixKeyService(
    UserService userService,
    HttpContextService httpContextService,
    PaymentProviderAccountRepository paymentProviderAccountRepository,
    PixKeyRepository pixKeyRepository)
{
    private const int MaxUserPixKeyAllowed = 20;
    private const int MaxUserPixKeyPerPspAllowed = 5;

    public async Task Create(IncomingCreatePixKeyDto incomingCreatePixKeyDto)
    {
        var user = await userService.FindByCpf(incomingCreatePixKeyDto.User.Cpf);

        var allUserAccounts = await paymentProviderAccountRepository.FindByUser(user);

        var allUserPixKeys = new List<PixKey>();
        allUserAccounts.ForEach(account => { allUserPixKeys.AddRange(account.PixKey); });

        if (incomingCreatePixKeyDto.Key.Type == "CPF")
            ValidateUniquePixKeyTypeCpfPerUser(allUserPixKeys);

        ValidateMaximumUserPixKey(allUserPixKeys);

        var paymentProviderId = httpContextService.GetPaymentProviderIdFromHttpContext();
        ValidateMaximumUserPixKeyByPsp(allUserAccounts, paymentProviderId);

        ValidatePixKeyIsUnique();

        var userAccount =
            GetUserAccountInThisPaymentProvider(incomingCreatePixKeyDto.Account, allUserAccounts, paymentProviderId) ??
            NewAccountInThisPaymentProvider(incomingCreatePixKeyDto.Account, user, paymentProviderId);

        var userPixKey = incomingCreatePixKeyDto.Key.ToEntity(userAccount);

        await pixKeyRepository.Create(userPixKey);
    }

    private PaymentProviderAccount? GetUserAccountInThisPaymentProvider(PostAccountDto incomingAccount,
        List<PaymentProviderAccount> allUserAccounts, int paymentProviderId)
    {
        return allUserAccounts.Find(ac =>
            ac.Account == incomingAccount.Number &&
            ac.Agency == incomingAccount.Agency &&
            ac.PaymentProviderId == paymentProviderId);
    }

    private PaymentProviderAccount NewAccountInThisPaymentProvider(PostAccountDto incomingAccount, User user,
        int paymentProviderId)
    {
        return incomingAccount.ToEntity(user, paymentProviderId);
    }

    private void ValidateUniquePixKeyTypeCpfPerUser(List<PixKey> allUserPixKeys)
    {
        var userPixKeyTypeCpf = allUserPixKeys.Find(userPixKey => userPixKey.Type == PixKeyTypes.CPF);
        if (userPixKeyTypeCpf != null)
            throw new ConflictException("User already a CPF key type registered");
    }

    private void ValidateMaximumUserPixKey(List<PixKey> allUserPixKeys)
    {
        if (allUserPixKeys.Count >= MaxUserPixKeyAllowed)
            throw new ConflictException($"Maximum allowed keys ({MaxUserPixKeyAllowed}) reached for the user.");
    }

    private void ValidateMaximumUserPixKeyByPsp(List<PaymentProviderAccount> allUserAccounts, int paymentProviderId)
    {
        var userPixKeyCountForThisPaymentProvider = allUserAccounts
            .Where(account => account.PaymentProviderId == paymentProviderId)
            .Sum(account => account.PixKey.Count);

        if (userPixKeyCountForThisPaymentProvider >= MaxUserPixKeyPerPspAllowed)
            throw new ConflictException(
                $"Maximum allowed keys ({MaxUserPixKeyPerPspAllowed}) reached for the user by Payment Provider..");
    }

    private void ValidatePixKeyIsUnique()
    {
        // Validates uniqueness in the database using the UNIQUE constraint on the @value column in the @pix-key table.
    }

    public async Task<OutgoingGetPixKeyDto> FindKeyByTypeAndValue(string typeStr, string value)
    {
        var pixKeyType = PixKey.ParsePixKeyType(typeStr);

        var key = await pixKeyRepository.FindByTypeAndValueIncludeAccount(pixKeyType, value);

        var response = new OutgoingGetPixKeyDto(key);

        return response;
    }

    public async Task<PixKey> FindByTypeAndValueOrError(string typeStr, string value)
    {
        var pixKeyType = PixKey.ParsePixKeyType(typeStr);

        var key = await pixKeyRepository.FindByTypeAndValue(pixKeyType, value);

        if (key == null)
            throw new PixKeyNotFoundException();

        return key;
    }
}
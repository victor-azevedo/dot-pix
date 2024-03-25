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
    private const int MAX_USER_PIX_KEY_ALLOWED = 20;
    private const int MAX_USER_PIX_KEY_PER_PSP_ALLOWED = 5;

    public async Task Create(InPostKeysDto inPostKeysDto)
    {
        var user = await userService.FindByCpfThrow(inPostKeysDto.User.Cpf);

        var allUserAccounts = await paymentProviderAccountRepository.FindByUser(user);

        var allUserPixKeys = new List<PixKey>();
        allUserAccounts.ForEach(account => { allUserPixKeys.AddRange(account.PixKey); });

        if (inPostKeysDto.Key.Type == "CPF")
            ValidateUniquePixKeyTypeCpfPerUser(allUserPixKeys);

        ValidateMaximumUserPixKey(allUserPixKeys);

        var paymentProviderId = httpContextService.GetPaymentProviderIdFromHttpContext();
        ValidateMaximumUserPixKeyByPsp(allUserAccounts, paymentProviderId);

        ValidatePixKeyIsUnique();

        var userAccount =
            GetUserAccountInThisPaymentProvider(inPostKeysDto.Account, allUserAccounts, paymentProviderId) ??
            NewAccountInThisPaymentProvider(inPostKeysDto.Account, user, paymentProviderId);

        var userPixKey = inPostKeysDto.Key.ToEntity(userAccount);

        await pixKeyRepository.Create(userPixKey);
    }

    private PaymentProviderAccount? GetUserAccountInThisPaymentProvider(InAccountDto incomingAccount,
        List<PaymentProviderAccount> allUserAccounts, int paymentProviderId)
    {
        return allUserAccounts.Find(ac =>
            ac.Account == incomingAccount.Number &&
            ac.Agency == incomingAccount.Agency &&
            ac.PaymentProviderId == paymentProviderId);
    }

    private PaymentProviderAccount NewAccountInThisPaymentProvider(InAccountDto incomingAccount, User user,
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
        if (allUserPixKeys.Count >= MAX_USER_PIX_KEY_ALLOWED)
            throw new ConflictException($"Maximum allowed keys ({MAX_USER_PIX_KEY_ALLOWED}) reached for the user.");
    }

    private void ValidateMaximumUserPixKeyByPsp(List<PaymentProviderAccount> allUserAccounts, int paymentProviderId)
    {
        var userPixKeyCountForThisPaymentProvider = allUserAccounts
            .Where(account => account.PaymentProviderId == paymentProviderId)
            .Sum(account => account.PixKey.Count);

        if (userPixKeyCountForThisPaymentProvider >= MAX_USER_PIX_KEY_PER_PSP_ALLOWED)
            throw new ConflictException(
                $"Maximum allowed keys ({MAX_USER_PIX_KEY_PER_PSP_ALLOWED}) reached for the user by Payment Provider..");
    }

    private void ValidatePixKeyIsUnique()
    {
        // Validates uniqueness in the database using the UNIQUE constraint on the @value column in the @pix-key table.
    }

    public async Task<OutGetPixKeyDto> FindByTypeAndValueIncludeAccountOrThrow(InPixKeyDto inPixKey)
    {
        var key = await pixKeyRepository.FindByValueIncludeAccount(inPixKey.Value);

        if (key == null)
            throw new PixKeyNotFoundException();

        ValidatePixKeyTypes(key, inPixKey.Type);

        var response = new OutGetPixKeyDto(key);

        return response;
    }

    private void ValidatePixKeyTypes(PixKey key, string typeStr)
    {
        if (!string.Equals(typeStr, key.Type.ToString(), StringComparison.CurrentCultureIgnoreCase))
            throw new ConflictException($"This type does not correspond to the key");
    }

    public async Task<PixKey> FindByTypeAndValueOrThrow(InPixKeyDto inPixKey)
    {
        var key = await pixKeyRepository.FindByTypeAndValue(inPixKey.Value);

        if (key == null)
            throw new PixKeyNotFoundException();

        ValidatePixKeyTypes(key, inPixKey.Type);

        return key;
    }
}
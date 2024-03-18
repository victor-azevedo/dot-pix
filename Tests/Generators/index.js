import createUsersJson from "./usersGenerator.js"
import createPaymentProvidersJson from "./paymentProvidersGenerator.js"
import createPaymentProviderTokensJson from "./paymentProviderTokensGenerator.js"
import createAccountsJson from "./accountsGenerator.js";
import createKeysJson from "./keysGenerator.js";
import createPaymentsJson from "./paymentsGenerator.js";

async function createData() {
    try {
        console.log("Creating data...");
        await createUsersJson();
        await createPaymentProvidersJson();
        await createPaymentProviderTokensJson();
        await createAccountsJson();
        await createKeysJson();
        await createPaymentsJson();
    } catch
        (error) {
        console.error(`Error creating seed: ${error}`);
    }
}

await createData()


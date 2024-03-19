import createAccountsJson from "./accountsGenerator.js";
import createKeysJson from "./keysGenerator.js";
import createPaymentProviderTokensJson from "./paymentProviderTokensGenerator.js";
import createPaymentProvidersJson from "./paymentProvidersGenerator.js";
import createPaymentsJson from "./paymentsGenerator.js";
import createUsersJson from "./usersGenerator.js";

async function createData() {
    try {
        console.log("\n*****************************************\n");
        console.log("Creating data...");
        // await createUsersJson();
        // await createPaymentProvidersJson();
        // await createPaymentProviderTokensJson();
        // await createAccountsJson();
        // await createKeysJson();
        await createPaymentsJson();
        console.log("\n*****************************************\n");
    } catch (error) {
        console.error("An error occurred during generate seed data:", error);
    }
}

await createData();
